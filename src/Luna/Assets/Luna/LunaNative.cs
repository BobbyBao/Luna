#define C_API

using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace SharpLuna
{
    using lua_State = System.IntPtr;
    using static Lua;

    public static unsafe class LunaNative
    {
        public static IntPtr ___type;
        public static IntPtr ___super;
        public static IntPtr ___getters;
        public static IntPtr ___setters;
        public static IntPtr ___get_indexed;
        public static IntPtr ___set_indexed;

        static LuaNativeFunction class_index = _class_index;
        static LuaNativeFunction class_newindex = _class_newindex;
        static LuaNativeFunction module_index = _module_index;
        static LuaNativeFunction module_newindex = _module_newindex;
        public static LuaNativeFunction ErrorReadOnly = _ErrorReadOnly;

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

        public static string GetFullName(LuaRef parent, string name)
        {
            string full_name = parent.RawGet(___type, "");
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
            string full_name = parent.RawGet(___type, "<unknown>");
            full_name += '.';
            full_name += name;
            return full_name;
        }

        public static LuaRef create_class(lua_State L, LuaRef parentModule, string name, Type classType, LuaNativeFunction dctor)
        {
            int moduleRef = parentModule.Ref;
            int classId = SharpObject.TypeID(classType);
            
            //Debug.LogWarning($"Register type : {classType }, id: {classId}");
#if C_API
            int metaRef = luna_create_class(L, moduleRef, name, classId, dctor.ToFunctionPointer());
            var meta = new LuaRef(metaRef, L);
            meta.RawSet("type", classType);
            return meta;
#else
            string fullName = GetFullName(parentModule, name);
            
            lua_createtable(L, 0, 0);
            lua_pushvalue(L, -1);
            lua_setmetatable(L, -2);

            lua_pushstring(L, "__index");
            lua_pushcfunction(L, (LuaNativeFunction)class_index);
            lua_rawset(L, -3);

            lua_pushstring(L, "__newindex");
            lua_pushcfunction(L, (LuaNativeFunction)class_newindex);
            lua_rawset(L, -3);
   
            lua_createtable(L, 0, 0);
            lua_rawsetp(L, -2, ___getters);

            lua_createtable(L, 0, 0);
            lua_rawsetp(L, -2, ___setters);

            lua_pushstring(L, fullName);
            lua_rawsetp(L, -2, ___type);
  
            if (dctor != null)
            {
                lua_pushstring(L, "__gc");
                lua_pushcfunction(L, dctor);
                lua_rawset(L, -3);
            }

            int metaRef = luaL_ref(L, LUA_REGISTRYINDEX);

            if (classId != 0)
            {
                lua_pushvalue(L, LUA_REGISTRYINDEX);
                lua_rawgeti(L, LUA_REGISTRYINDEX, metaRef);
                lua_rawseti(L, -2, classId);
                lua_pop(L, 1);
            }

            lua_rawgeti(L, LUA_REGISTRYINDEX, moduleRef);
            lua_pushstring(L, name);
            lua_rawgeti(L, LUA_REGISTRYINDEX, metaRef);
            lua_rawset(L, -3);
            lua_pop(L, 1);
            return new LuaRef(metaRef, L);
            /*
            LuaRef meta = LuaRef.CreateTable(L);            
            meta.SetMetaTable(meta);

            meta.RawSet("__index", (LuaNativeFunction)class_index);
            meta.RawSet("__newindex", (LuaNativeFunction)class_newindex);
            meta.RawSet(___getters, LuaRef.CreateTable(L));
            meta.RawSet(___setters, LuaRef.CreateTable(L));
            meta.RawSet(___type, fullName);

            if (dctor != null)
                meta.RawSet("__gc", dctor);

            if(classId != 0)
            {
                LuaRef registry = LuaRef.Registry(L);
                registry.RawSet(classId, meta);
            }

            parentModule.RawSet(name, meta);
            return meta;*/
#endif

        }

        public static LuaRef create_module(lua_State L, LuaRef parentModule, string name)
        {
            int moduleRef = parentModule.Ref;
#if C_API
            int metaRef;
            metaRef = luna_create_module(L, moduleRef, name);
            return new LuaRef(metaRef, L);
#else
            string fullName = GetFullName(parentModule, name);

            LuaRef meta = LuaRef.CreateTable(L);
            meta.SetMetaTable(meta);

            meta.RawSet("__index", (LuaNativeFunction)module_index);
            meta.RawSet("__newindex", (LuaNativeFunction)module_newindex);
            meta.RawSet(___getters, LuaRef.CreateTable(L));
            meta.RawSet(___setters, LuaRef.CreateTable(L));
            meta.RawSet(___type, fullName);
            parentModule.RawSet(name, meta);
            return meta;
#endif
        }

        //todo: use upvalue
        [AOT.MonoPInvokeCallback(typeof(LuaNativeFunction))]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static int _class_index(lua_State L)
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
        static int _class_newindex(lua_State L)
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
        public static int _module_index(lua_State L)
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
        public static int _module_newindex(lua_State L)
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
        static int _ErrorReadOnly(lua_State L)
        {
            return luaL_error(L, "property '{0}' is read-only", lua_tostring(L, lua_upvalueindex(1)));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int TryGetUserData(lua_State L, int key, int cache_ref)
        {
#if C_API
            return luna_try_getuserdata(L, key, cache_ref);
#else
            lua_rawgeti(L, LUA_REGISTRYINDEX, cache_ref);
            lua_rawgeti(L, -1, key);
            if (!lua_isnil(L, -1))
            {
                lua_remove(L, -2);
                return 1;
            }
            lua_pop(L, 2);
            return 0;
#endif
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CacheUserData(lua_State L, int key, int cache_ref)
        {
#if C_API
            luna_cacheuserdata(L, key, cache_ref);
#else
            lua_rawgeti(L, LUA_REGISTRYINDEX, cache_ref);
            lua_pushvalue(L, -2);
            lua_rawseti(L, -2, key);
            lua_pop(L, 1);
#endif
        }


        [AOT.MonoPInvokeCallback(typeof(LuaNativeFunction))]
        static int c_lua_gettable(lua_State L)
        {
            lua_gettable(L, 1);
            return 1;
        }

        public static LuaStatus luna_pgettable(lua_State L, int idx)
        {
            int top = lua_gettop(L);
            idx = lua_absindex(L, idx);
            lua_pushcfunction(L, c_lua_gettable);
            lua_pushvalue(L, idx);
            lua_pushvalue(L, top);
            lua_remove(L, top);
            return lua_pcall(L, 2, 1, 0);
        }

#if C_API
        [DllImport(LuaLibraryName, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern void luna_pushstruct(lua_State L, int metaRef, IntPtr data, StructElement* layout, int count);
       
        [DllImport(LuaLibraryName, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern void luna_getstruct(lua_State L, int idx, IntPtr data, StructElement* layout, int count);

        [DllImport(LuaLibraryName, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern void luna_packstruct(lua_State L, int newFn, IntPtr data, StructElement* layout, int count);
        [DllImport(LuaLibraryName, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern void luna_unpackstruct(lua_State L, int index, int unpackFn, IntPtr data, StructElement* layout, int count);
#else
        public static void luna_pushstruct(lua_State L, int metaRef, IntPtr data, StructElement* layout, int count)
        {
            lua_newtable(L);
            for (int i = 0; i < count; i++)
            {
                StructElement* e = layout + i;
                switch (e->type)
                {
                    case TypeCode.Boolean:
                        lua_pushboolean(L, *(bool*)(data + e->offset) ? 1 : 0);
                        break;
                    case TypeCode.Int32:
                        lua_pushnumber(L, *(int*)(data + e->offset));
                        break;
                    case TypeCode.Single:
                        lua_pushnumber(L, *(float*)(data + e->offset));
                        break;
                    case TypeCode.Double:
                        lua_pushnumber(L, *(double*)(data + e->offset));
                        break;
                    case TypeCode.String:
                        UIntPtr l;
                        lua_pushlstring(L, *(byte**)(data + e->offset), 0);
                        break;
                }

                lua_setfield(L, -2, e->name);
            }

            lua_rawgeti(L, LUA_REGISTRYINDEX, metaRef);
            lua_setmetatable(L, -2);
        }
        
        public static void luna_getstruct(lua_State L, int idx, IntPtr data, StructElement* layout, int count)
        {
            lua_pushvalue(L, idx);
            for(int i = 0; i < count; i++)
            {
                StructElement* e = layout + i;                
                lua_getfield(L, -1, e->name);
                switch(e->type)
                {
                    case TypeCode.Boolean:
                        *(bool*)(data + e->offset) = (lua_toboolean(L, -1) != 0);
                        break;
                    case TypeCode.Int32:
                        *(int*)(data + e->offset) = (int)lua_tonumber(L, -1);
                        break;
                    case TypeCode.Single:
                        *(float*)(data + e->offset) = (float)lua_tonumber(L, -1);
                        break;
                    case TypeCode.Double:
                        *(double*)(data + e->offset) = lua_tonumber(L, -1);
                        break;
                    case TypeCode.String:
                        UIntPtr l;
                        *(IntPtr*)(data + e->offset) = luaL_tolstring(L, -1, &l);
                        break;
                }

                lua_pop(L, 1);
            }
            lua_pop(L, 1);
        }
#endif
    }

}
