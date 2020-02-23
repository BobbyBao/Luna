#define C_API

using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace SharpLuna
{
    using lua_State = System.IntPtr;
    using static Lua;

    public partial class SharpClass
    {
        protected static IntPtr ___type;
        protected static IntPtr ___super;
        protected static IntPtr ___getters;
        protected static IntPtr ___setters;
        protected static IntPtr ___get_indexed;
        protected static IntPtr ___set_indexed;

        public static void Init(lua_State L)
        {
#if C_API
            LunaData lunaData = new LunaData();
            luna_init(L, ref lunaData);
            ___type = lunaData.type;
            ___super = lunaData.super;
            ___getters = lunaData.getters;
            ___setters = lunaData.setters;
            ___get_indexed = lunaData.get_indexed;
            ___set_indexed = lunaData.set_indexed;
#else      
            ___type =  Marshal.StringToHGlobalAnsi("___type");
            ___super = Marshal.StringToHGlobalAnsi("___super");
            ___getters = Marshal.StringToHGlobalAnsi("___getters");
            ___setters = Marshal.StringToHGlobalAnsi("___setters");
            ___get_indexed = Marshal.StringToHGlobalAnsi("___get_indexed");
            ___set_indexed = Marshal.StringToHGlobalAnsi("___set_indexed");
#endif
        }

        public static bool CreateClass(ref LuaRef meta, LuaRef parent, string name, int classId)
        {
            LuaRef @ref = parent.RawGet<LuaRef, string>(name);
            if (@ref)
            {
                meta = @ref;
                return false;
            }

            var L = parent.State;
            string type_name = "class<" + GetFullName(parent, name) + ">";

            LuaRef typeClass = LuaRef.FromValue(L, classId);
            LuaRef cls = LuaRef.CreateTable(L);
            cls.SetMetaTable(cls);

            cls.RawSet("__index", (LuaNativeFunction)class_index);
            cls.RawSet("__newindex", (LuaNativeFunction)class_newindex);

            cls.RawSet(___getters, LuaRef.CreateTable(L));
            cls.RawSet(___setters, LuaRef.CreateTable(L));
            cls.RawSet(___type, type_name);

            LuaRef registry = new LuaRef(L, LUA_REGISTRYINDEX);
            registry.RawSet(typeClass, cls);
            parent.RawSet(name, cls);
            meta = cls;
            return true;
        }

        public static bool CreateClass(ref LuaRef meta, LuaRef parent, string name, int classId, int superClassID)
        {
            if (CreateClass(ref meta, parent, name, classId))
            {
                if (superClassID != 0)
                {
                    LuaRef registry = new LuaRef(parent.State, LUA_REGISTRYINDEX);
                    LuaRef super = registry.RawGet<LuaRef>(superClassID);
                    meta.RawSet(___super, super);
                }

                return true;
            }
            return false;
        }

        public static LuaRef create_module(lua_State L, LuaRef parentMeta, string name)
        {
            string type_name = "module<" + GetFullName(parentMeta, name) + ">";
            LuaRef module = LuaRef.CreateTable(L);
            module.SetMetaTable(module);

            module.RawSet("__index", (LuaNativeFunction)module_index);
            module.RawSet("__newindex", (LuaNativeFunction)module_newindex);
            module.RawSet(___getters, LuaRef.CreateTable(L));
            module.RawSet(___setters, LuaRef.CreateTable(L));
            module.RawSet(___type, type_name);
            module.RawSet("___parent", parentMeta);
            parentMeta.RawSet(name, module);
            return module;
        }

        //todo: use upvalue
        [AOT.MonoPInvokeCallback(typeof(LuaNativeFunction))]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int class_index(lua_State L)
        {
#if C_API
            return luna_class_index(L);
#else
            // <SP:1> -> table or userdata
            // <SP:2> -> key

            // get signature metatable -> <mt> <sign_mt>
            lua_getmetatable(L, 1);

            for (; ; )
            {
                // push metatable[key] -> <mt> <mt[key]>
                lua_pushvalue(L, 2);
#if DEBUG
                string key = lua_tostring(L, -1);
                lua_pop(L, 1);
#endif
                lua_rawget(L, -2);

                if (!lua_isnil(L, -1))
                {
                    // value is found
                    break;
                }

                // get metatable.getters -> <mt> <getters>
                lua_pop(L, 1);                  // pop nil

                if (lua_isnumber(L, 2))
                {
                    //lua_pushliteral(L, "___get_indexed");
                    lua_pushlightuserdata(L, ___get_indexed);
                    lua_rawget(L, -2);

                    if (!lua_isnil(L, -1))
                    {
                        assert(lua_iscfunction(L, -1));
                        lua_pushvalue(L, 1);
                        lua_pushvalue(L, 2);
                        lua_call(L, 2, 1);
                        break;
                    }
                    else
                    {
                        lua_pop(L, 1);
                    }
                }

                //lua_pushliteral(L, "___getters");
                lua_pushlightuserdata(L, ___getters);
                lua_rawget(L, -2);
                assert(lua_istable(L, -1));

                // get metatable.getters[key] -> <mt> <getters> <getters[key]>
                lua_pushvalue(L, 2);            // push key
                lua_rawget(L, -2);              // lookup key in getters

                // call the getter if it is function, or leave it as value
                if (!lua_isnil(L, -1))
                {
                    if (lua_iscfunction(L, -1))
                    {
                        if (lua_isuserdata(L, 1))
                        {
                            // if it is userdata, that means instance
                            lua_pushvalue(L, 1);    // push userdata as object param for member function
                            lua_call(L, 1, 1);
                        }
                        else
                        {
                            // otherwise, it is static (class getters)
                            assert(lua_istable(L, 1));
                            lua_call(L, 0, 1);
                        }
                    }
                    break;
                }

                // now try super metatable -> <mt> <super_mt>
                lua_pop(L, 2);                  // pop <getters> <getters[key]>
                //lua_pushliteral(L, "___super");
                lua_pushlightuserdata(L, ___super);
                lua_rawget(L, -2);

                if (lua_isnil(L, -1))
                {
                    // leave value on top -> <nil> or <extra_fields[key]>
                    break;
                }

                // yes, now continue with <super_mt>
                assert(lua_istable(L, -1));
                lua_remove(L, -2);              // pop <mt>
            }

            return 1;
#endif
        }

        [AOT.MonoPInvokeCallback(typeof(LuaNativeFunction))]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int class_newindex(lua_State L)
        {
#if C_API
            return luna_class_newindex(L);
#else
            // <SP:1> -> table or userdata
            // <SP:2> -> key
            // <SP:3> -> value

            // get signature metatable -> <mt> <sign_mt>
            lua_getmetatable(L, 1);

            for (; ; )
            {                
                if (lua_isnumber(L, 2))
                {
                    //lua_pushliteral(L, "___set_indexed");
                    lua_pushlightuserdata(L, ___set_indexed);
                    lua_rawget(L, -2);

                    if (!lua_isnil(L, -1))
                    {
                        assert(lua_iscfunction(L, -1));
                        lua_pushvalue(L, 1);
                        lua_pushvalue(L, 2);
                        lua_pushvalue(L, 3);
                        lua_call(L, 3, 0);
                        break;
                    }
                    else
                    {
                        lua_pop(L, 1);
                    }
                }

                // get setters subtable of metatable -> <mt> <setters>
                //lua_pushliteral(L, "___setters");
                lua_pushlightuserdata(L, ___setters);
                lua_rawget(L, -2);              // get __setters table
                assert(lua_istable(L, -1));

                // get setters[key] -> <mt> <setters> <setters[key]>
                lua_pushvalue(L, 2);            // push key arg2
                lua_rawget(L, -2);              // lookup key in setters

                if (lua_iscfunction(L, -1))
                {
                    // setter function found, now need to test whether it is object (== userdata)
                    int n = 1;
                    if (lua_isuserdata(L, 1))
                    {
                        lua_pushvalue(L, 1);    // push userdata as object param for member function
                        n++;
                    }
                    else
                    {
                        assert(lua_istable(L, 1));
                    }

                    lua_pushvalue(L, 3);        // push new value as arg
                    lua_call(L, n, 0);
                    break;
                }

                // now try super metatable -> <mt> <super_mt>
                assert(lua_isnil(L, -1));
                lua_pop(L, 2);                  // pop <setters> <setters[key]>
                //lua_pushliteral(L, "___super");
                lua_pushlightuserdata(L, ___super);
                lua_rawget(L, -2);

                // check if there is one
                if (lua_isnil(L, -1))
                {
                    // give up
                    //lua_pushliteral(L, "___type");
                    lua_pushlightuserdata(L, ___type);
                    lua_rawget(L, -3);
                    return luaL_error(L, "property '{0}.{1}' is not found or not writable",
                        luaL_optstring(L, -1, "<unknown>"), lua_tostring(L, 2));
                }

                // yes, now continue with <super_mt>
                assert(lua_istable(L, -1));
                lua_remove(L, -2);              // pop <mt>
            }

            return 0;
#endif
        }

        [AOT.MonoPInvokeCallback(typeof(LuaNativeFunction))]
        public static int module_index(lua_State L)
        {
#if C_API
            return luna_module_index(L);
#else
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
                //lua_pushliteral(L, "___getters");
                lua_pushlightuserdata(L, ___getters);
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
#endif
        }

        [AOT.MonoPInvokeCallback(typeof(LuaNativeFunction))]
        public static int module_newindex(lua_State L)
        {
#if C_API
            return luna_module_newindex(L);
#else
            // <SP:1> -> table
            // <SP:2> -> key
            // <SP:3> -> value

            // push metatable of table -> <mt>
            lua_getmetatable(L, 1);

            // get setters subtable of metatable -> <mt> <setters>
            //lua_pushliteral(L, "___setters");
            lua_pushlightuserdata(L, ___setters);
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
#endif
        }

        [AOT.MonoPInvokeCallback(typeof(LuaNativeFunction))]
        public static int ErrorReadOnly(lua_State L)
        {
            return luaL_error(L, "property '{0}' is read-only", lua_tostring(L, lua_upvalueindex(1)));
        }

    }

}
