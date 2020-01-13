using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace SharpLuna
{
    using lua_State = System.IntPtr;
    using static Lua;

    public class GlobalModule : SharpClass
    {
        protected Luna luna;
        public override Luna Luna
        {
            get { return luna; }
        }

        public GlobalModule(Luna luna) : base(LuaRef.Globals(luna.State))
        {
            this.luna = luna;
        }

        public GlobalModule(Luna luna, LuaRef mod) : base(mod)
        {
            this.luna = luna;
        }

    }

    public class SharpModule : SharpClass
    {
        public SharpModule(LuaRef meta) : base(meta)
        {
        }

        public SharpModule(LuaRef meta, string name) : base(meta)
        {
            LuaRef @ref = meta.RawGet(name);
            if (@ref)
            {
                m_meta = @ref;
                return;
            }

            lua_State L = meta.State;
            string type_name = "module<" + GetFullName(meta, name) + ">";
            LuaRef module = LuaRef.CreateTable(L);
            module.SetMetaTable(module);

            module.RawSet("__index", (LuaNativeFunction)BindModuleMetaMethod.Index);
            module.RawSet("__newindex", (LuaNativeFunction)BindModuleMetaMethod.NewIndex);
            module.RawSet("___getters", LuaRef.CreateTable(L));
            module.RawSet("___setters", LuaRef.CreateTable(L));
            module.RawSet("___type", type_name);
            module.RawSet("___parent", meta);
            meta.RawSet(name, module);
            m_meta = module;
        }

    };

    public class BindModuleMetaMethod
    {
        public static int Index(LuaState L)
        {
            // <SP:1> -> table
            // <SP:2> -> key

            // push metatable of table -> <mt>
            lua_getmetatable(L, 1);

            // push metatable[key] -> <mt> <mt[key]>
            lua_pushvalue(L, 2);
            lua_rawget(L, -2);

            if (lua_isnil(L, -1))
            {
                // get metatable.getters -> <mt> <getters>
                lua_pop(L, 1);          // pop nil
                lua_pushliteral(L, "___getters");
                lua_rawget(L, -2);      // get getters table
                assert(lua_istable(L, -1));

                // get metatable.getters[key] -> <mt> <getters> <getters[key]>
                lua_pushvalue(L, 2);    // push key
                lua_rawget(L, -2);      // lookup key in getters

                if (lua_iscfunction(L, -1))
                {
                    // getter function found
                    lua_call(L, 0, 1);
                }
            }

            return 1;
        }

        public static int NewIndex(LuaState L)
        {
            // <SP:1> -> table
            // <SP:2> -> key
            // <SP:3> -> value

            // push metatable of table -> <mt>
            lua_getmetatable(L, 1);

            // get setters subtable of metatable -> <mt> <setters>
            lua_pushliteral(L, "___setters");
            lua_rawget(L, -2);          // get __setters table
            assert(lua_istable(L, -1));

            // get setters[key] -> <mt> <setters> <setters[key]>
            lua_pushvalue(L, 2);        // push key arg2
            lua_rawget(L, -2);          // lookup key in setters

            if (lua_iscfunction(L, -1))
            {
                // setter function found
                lua_pushvalue(L, 3);    // push new value as arg
                lua_call(L, 1, 0);
            }
            else
            {
                // no setter found, just set the table field
                assert(lua_isnil(L, -1));
                lua_pop(L, 3);
                lua_rawset(L, 1);
            }
            return 0;
        }

        public static int ForwardCall(LuaState L)
        {
            // <SP:1> -> table (table's metatable is set to itself)
            // <SP:2> ~ <SP:top> -> args
            int top = lua_gettop(L);

            lua_pushvalue(L, lua_upvalueindex(1));
            lua_rawget(L, 1);           // get sub module metatable -> <sub_mt>

            lua_pushliteral(L, "__call");
            lua_rawget(L, -2);          // get call function -> <sub_mt> <call>

            lua_insert(L, 1);           // move <call> to top -> now <mt> is at index 2
            lua_replace(L, 2);          // replace original <mt> with <sub_mt>
            lua_call(L, top, 1);
            return 1;
        }

        public static int ErrorReadOnly(LuaState L)
        {
            return luaL_error(L, "property '{0}' is read-only", lua_tostring(L, lua_upvalueindex(1)));
        }

    }


}
