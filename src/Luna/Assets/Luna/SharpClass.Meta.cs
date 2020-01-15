//#define LUAINTF_EXTRA_LUA_FIELDS

using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace SharpLuna
{
    using lua_State = System.IntPtr;
    using static Lua;

    public partial class SharpClass
    {
        static Dictionary<Type, SharpClass> registeredClass = new Dictionary<Type, SharpClass>();
        static Dictionary<Type, string> classAlias = new Dictionary<Type, string>();

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsRegistered<T>()
        {
            return IsRegistered(typeof(T));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsRegistered(Type t)
        {
            return registeredClass.ContainsKey(t);
        }

        public static void SetAlias(Type t, string alias)
        {
            classAlias.Add(t, alias);
        }

        public static string GetTableName(Type t)
        {
            if (classAlias.TryGetValue(t, out string alias))
            {
                return alias;
            }

            return t.Name;
        }

        public static string GetFullName(LuaRef parent, string name)
        {
            string full_name = parent.Get("___type", "");
            if (!string.IsNullOrEmpty(full_name))
            {
                int pos = full_name.IndexOf('<');
                if (pos != -1) full_name.Remove(0, pos + 1);
                pos = full_name.LastIndexOf('>');
                if (pos != -1) full_name.Remove(pos);
                full_name += '.';
            }
            full_name += name;
            return full_name;
        }

        public static string GetMemberName(LuaRef parent, string name)
        {
            string full_name = parent.RawGet("___type", "<unknown>");
            full_name += '.';
            full_name += name;
            return full_name;
        }

        protected static bool BuildMetaTable(ref LuaRef meta, LuaRef parent, string name, IntPtr clazz_id)
        {
            LuaRef @ref = parent.RawGet<LuaRef, string>(name);
            if (@ref)
            {
                meta = @ref;
                return false;
            }

            var L = parent.State;
            string type_name = "class<" + GetFullName(parent, name) + ">";

            LuaRef type_clazz = LuaRef.FromPtr(L, clazz_id);
            LuaRef clazz = LuaRef.CreateTable(L);
            clazz.SetMetaTable(clazz);
            clazz.RawSet("__index", (LuaNativeFunction)BindClassMetaMethod.Index);
            clazz.RawSet("__newindex", (LuaNativeFunction)BindClassMetaMethod.NewIndex);
            clazz.RawSet("___getters", LuaRef.CreateTable(L));
            clazz.RawSet("___setters", LuaRef.CreateTable(L));

            clazz.RawSet("___type", type_name);

            LuaRef registry = new LuaRef(L, LUA_REGISTRYINDEX);
            registry.RawSet(type_clazz, clazz);
            parent.RawSet(name, clazz);
            meta = clazz;
            return true;
        }

        protected static bool BuildMetaTable(ref LuaRef meta, LuaRef parent, string name,
            IntPtr clazz_id, IntPtr super_static_id)
        {
            if (BuildMetaTable(ref meta, parent, name, clazz_id))
            {
                if (super_static_id != IntPtr.Zero)
                {
                    LuaRef registry = new LuaRef(parent.State, LUA_REGISTRYINDEX);
                    LuaRef super = registry.RawGetP<LuaRef>(super_static_id);
                    meta.RawSet("___super", super);
                }

                return true;
            }
            return false;
        }

        public static SharpClass Bind<T>(SharpClass parentMeta, string name = null)
        {
            Type classType = typeof(T);
            if(registeredClass.TryGetValue(classType, out var bindClass))
            {
                return bindClass;
            }

            if (name == null)
            {
                name = classType.Name;
            }

            LuaRef meta = LuaRef.None;
            if (BuildMetaTable(ref meta, parentMeta.Meta, name, SharpObject.Signature<T>()))
            {
                meta.RawSet("__gc", (LuaNativeFunction)ClassDestructor<T>.Call);
            }

            bindClass = new SharpClass(meta);
            bindClass.parent = parentMeta;
            bindClass.SetClassType(classType);
            registeredClass.Add(classType, bindClass);
            return bindClass;
        }

        public static SharpClass Extend<T, SUPER>(SharpClass parentMeta)
        {
            Type classType = typeof(T);
            if (registeredClass.TryGetValue(classType, out var bindClass))
            {
                return bindClass;
            }

            string name = classType.Name;
            LuaRef meta = LuaRef.None;
            if (BuildMetaTable(ref meta, parentMeta.Meta, name, SharpObject.Signature<T>(), SharpObject.Signature<SUPER>()))
            {
                meta.RawSet("__gc", (LuaNativeFunction)ClassDestructor<T>.Call);
            }

            bindClass = new SharpClass(meta);
            bindClass.parent = parentMeta;
            bindClass.SetClassType(classType);
            registeredClass.Add(classType, bindClass);
            return bindClass;
        }

        public static SharpClass Extend(Type classType, Type superType, SharpClass parentMeta)
        {
            if (registeredClass.TryGetValue(classType, out var bindClass))
            {
                return bindClass;
            }

            string name = classType.Name;
            LuaRef meta = LuaRef.None;
            if (BuildMetaTable(ref meta, parentMeta.Meta, name, SharpObject.Signature(classType), SharpObject.Signature(superType)))
            {
                try
                {
                    var callerType = typeof(ClassDestructor<>).MakeGenericType(classType);
                    MethodInfo CallInnerDelegateMethod = callerType.GetMethod("Call", BindingFlags.Static | BindingFlags.Public);
                    var luaFunc = (LuaNativeFunction)DelegateCache.Get(typeof(LuaNativeFunction), CallInnerDelegateMethod);

                    meta.RawSet("__gc", luaFunc);
                }
                catch
                {
                }
            }

            bindClass = new SharpClass(meta);
            bindClass.parent = parentMeta;
            bindClass.SetClassType(classType);
            registeredClass.Add(classType, bindClass);
            return bindClass;
        }



    }

    public class BindClassMetaMethod
    {
        public static int Index(LuaState L)
        {
            // <SP:1> -> table or userdata
            // <SP:2> -> key

            // get signature metatable -> <mt> <sign_mt>
            lua_getmetatable(L, 1);

            for (; ; )
            {
                // push metatable[key] -> <mt> <mt[key]>
                lua_pushvalue(L, 2);
#if DEBUG
                //string key = lua_tostring(L, -1);
                //lua_pop(L, 1);
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
                    lua_pushliteral(L, "___get_indexed");
                    lua_rawget(L, -2);

                    if (!lua_isnil(L, -1))
                    {
                        //assert(lua_iscfunction(L, -1));
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

                lua_pushliteral(L, "___getters");
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
                            //assert(lua_istable(L, 1));
                            lua_call(L, 0, 1);
                        }
                    }
                    break;
                }

                // now try super metatable -> <mt> <super_mt>
                lua_pop(L, 2);                  // pop <getters> <getters[key]>
                lua_pushliteral(L, "___super");
                lua_rawget(L, -2);

                if (lua_isnil(L, -1))
                {

#if LUAINTF_EXTRA_LUA_FIELDS
                    if (lua_isuserdata(L, 1))
                    {
                        lua_getuservalue(L, 1);
                        if (!lua_isnil(L, -1))
                        {
                            // get extra_fields[key] -> <mt> <nil> <extra_fields> <extra_fields[key]>
                            lua_pushvalue(L, 2);    // push key
                            lua_rawget(L, -2);      // lookup key in extra fields
                        }
                    }
#endif

                    // leave value on top -> <nil> or <extra_fields[key]>
                    break;
                }

                // yes, now continue with <super_mt>
                assert(lua_istable(L, -1));
                lua_remove(L, -2);              // pop <mt>
            }

            return 1;
        }

        public static int NewIndex(LuaState L)
        {
            // <SP:1> -> table or userdata
            // <SP:2> -> key
            // <SP:3> -> value

            // get signature metatable -> <mt> <sign_mt>
            lua_getmetatable(L, 1);

            for (; ; )
            {
                if (lua_isnumber(L, 2))
                {
                    lua_pushliteral(L, "___set_indexed");
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
                lua_pushliteral(L, "___setters");
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
                lua_pushliteral(L, "___super");
                lua_rawget(L, -2);

                // check if there is one
                if (lua_isnil(L, -1))
                {

#if LUAINTF_EXTRA_LUA_FIELDS
                    if (lua_isuserdata(L, 1))
                    {
                        // set instance fields
                        lua_pushliteral(L, "___const");
                        lua_rawget(L, -3);
                        if (!lua_rawequal(L, -1, -3))
                        {
                            // set field only if not const
                            lua_getuservalue(L, 1);
                            if (lua_isnil(L, -1))
                            {
                                lua_newtable(L);
                                lua_pushvalue(L, 2);
                                lua_pushvalue(L, 3);
                                lua_rawset(L, -3);
                                lua_setuservalue(L, 1);
                            }
                            else
                            {
                                lua_pushvalue(L, 2);
                                lua_pushvalue(L, 3);
                                lua_rawset(L, -3);
                            }
                            break;
                        }
                        lua_pop(L, 1);
                    }
                    else
                    {
                        // set class fields
                        lua_pushvalue(L, 2);
                        lua_pushvalue(L, 3);
                        lua_rawset(L, 1);
                        break;
                    }
#endif

                    // give up
                    lua_pushliteral(L, "___type");
                    lua_rawget(L, -3);
                    return luaL_error(L, "property '{0}.{1}' is not found or not writable",
                        luaL_optstring(L, -1, "<unknown>"), lua_tostring(L, 2));
                }

                // yes, now continue with <super_mt>
                assert(lua_istable(L, -1));
                lua_remove(L, -2);              // pop <mt>
            }

            return 0;
        }

        public static int ErrorReadOnly(LuaState L)
        {
            return luaL_error(L, "property '{0}' is read-only", lua_tostring(L, lua_upvalueindex(1)));
        }

        public static int ErrorConstMismatch(LuaState L)
        {
            return luaL_error(L, "member function '{0}' can not be access by const object", lua_tostring(L, lua_upvalueindex(1)));
        }

    }

}
