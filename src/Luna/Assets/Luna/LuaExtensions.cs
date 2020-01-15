
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;


namespace SharpLuna
{
    public unsafe static partial class Lua
    {        
        public static void Register(this LuaState L, string name, LuaNativeFunction function)
        {
            savedFn.TryAdd(function);
            lua_pushcfunction(L, function);
            lua_setglobal(L, name);
        }

        public static string ToString(this LuaState L, int index, bool callMetamethod = true)
        {
            var str = lua_tostring(L, index);

            if (callMetamethod)
            {
                lua_pop(L, 1);
            }

            return str;
        }

        public static unsafe void PushLightObject<T>(this LuaState L, T obj)
        {
            GCHandle gc = GCHandle.Alloc(obj, GCHandleType.Normal);
            lua_pushlightuserdata(L, GCHandle.ToIntPtr(gc));
        }

        public static T ToLightObject<T>(this LuaState L, int index, bool freeGCHandle = true)
        {
            if (lua_isnil(L, index) || !lua_islightuserdata(L, index))
                return default(T);

            IntPtr data = lua_touserdata(L, index);
            if (data == IntPtr.Zero)
                return default(T);

            var handle = GCHandle.FromIntPtr(data);
            if (!handle.IsAllocated)
                return default(T);

            var reference = (T)handle.Target;

            if (freeGCHandle)
                handle.Free();

            return reference;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Push<T>(this LuaState L, T v)
        {
            switch (v)
            {
                case bool bval:
                    lua_pushboolean(L, bval ? 1 : 0);
                    break;
                case sbyte ival:
                    lua_pushinteger(L, ival);
                    break;
                case byte ival:
                    lua_pushinteger(L, ival);
                    break;
                case short ival:
                    lua_pushinteger(L, ival);
                    break;
                case ushort ival:
                    lua_pushinteger(L, ival);
                    break;
                case int ival:
                    lua_pushinteger(L, ival);
                    break;
                case uint ival:
                    lua_pushinteger(L, ival);
                    break;
                case long lval:
                    lua_pushinteger(L, lval);
                    break;
                case ulong lval:
                    lua_pushinteger(L, (long)lval);
                    break;
                case float fval:
                    lua_pushnumber(L, fval);
                    break;
                case double dval:
                    lua_pushnumber(L, dval);
                    break;
                case string strval:
                    lua_pushstring(L, strval);
                    break;
                case LuaNativeFunction fn:
                    lua_pushcfunction(L, fn);
                    break;
                case LuaRef luaRef:
                    if (luaRef.IsValid)
                    {
                        luaRef.PushToStack();
                    }
                    else
                    {
                        lua_pushnil(L);
                    }
                    break;
                default:
                    if (SharpClass.IsRegistered<T>())
                    {
                        SharpObject.PushToStack(L, v);
                    }
                    else
                    {
                        PushLightObject(L, v);
                    }
                    break;
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static object GetObject(this LuaState L, int index)
        {
            LuaType type = lua_type(L, index);

            switch (type)
            {
                case LuaType.Number:
                    {
                        if (lua_isinteger(L, index))
                            return lua_tointeger(L, index);

                        return lua_tonumber(L, index);
                    }
                case LuaType.String:
                    return lua_tostring(L, index);
                case LuaType.Boolean:
                    return lua_toboolean(L, index);
                case LuaType.Table:
                    return Get<LuaRef>(L, index);
                case LuaType.Function:
                    return Get<LuaRef>(L, index);
                case LuaType.LightUserData:
                    return L.ToLightObject<object>(index);
                case LuaType.UserData:
                    return SharpObject.Get<object>(L, index);
                default:
                    return null;
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T Get<T>(this LuaState L, int index)
        {
            Type t = typeof(T);
            if (t == typeof(bool))
                return Convert.To<T>(luaL_checkinteger(L, index) == 0 ? false : true);
            else if (t == typeof(sbyte))
                return Convert.To<T>((sbyte)luaL_checkinteger(L, index));
            else if (t == typeof(byte))
                return Convert.To<T>((byte)luaL_checkinteger(L, index));
            else if (t == typeof(short))
                return Convert.To<T>((short)luaL_checkinteger(L, index));
            else if (t == typeof(ushort))
                return Convert.To<T>((ushort)luaL_checkinteger(L, index));
            else if (t == typeof(char))
                return Convert.To<T>((char)luaL_checkinteger(L, index));
            else if (t == typeof(int))
                return Convert.To<T>((int)luaL_checkinteger(L, index));
            else if (t == typeof(uint))
                return Convert.To<T>((uint)luaL_checkinteger(L, index));
            else if (t == typeof(long))
                return Convert.To<T>(luaL_checkinteger(L, index));
            else if (t == typeof(ulong))
                return Convert.To<T>((ulong)luaL_checkinteger(L, index));
            else if (t == typeof(float))
                return Convert.To<T>((float)luaL_checknumber(L, index));
            else if (t == typeof(double))
                return Convert.To<T>(luaL_checknumber(L, index));
            else if (t == typeof(string))
                return (T)(object)lua_checkstring(L, index);
            else if (t == typeof(LuaNativeFunction))
                 return (T)(object)lua_tocfunction(L, index).ToLuaFunction();
            else if (t == typeof(LuaRef))
            {
                if (lua_isnone(L, index))
                    return Convert.To<T>(LuaRef.None);
                else
                    return Convert.To<T>(new LuaRef(L, index));
            }
            else
            {
                return SharpObject.Get<T>(L, index);
            }

        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T Opt<T>(this LuaState L, int index, T def)
        {
            switch (def)
            {
                case bool bval:
                    return Convert.To<T>(luaL_optinteger(L, index, bval ? 1 : 0));
                case sbyte ival:
                    return Convert.To<T>((sbyte)luaL_optinteger(L, index, ival));
                case byte ival:
                    return Convert.To<T>((byte)luaL_optinteger(L, index, ival));
                case short ival:
                    return Convert.To<T>((short)luaL_optinteger(L, index, ival));
                case ushort ival:
                    return Convert.To<T>((ushort)luaL_optinteger(L, index, ival));
                case char ival:
                    return Convert.To<T>((char)luaL_optinteger(L, index, ival));
                case int ival:
                    return Convert.To<T>((int)luaL_optinteger(L, index, ival));
                case uint ival:
                    return Convert.To<T>((uint)luaL_optinteger(L, index, ival));
                case long ival:
                    return Convert.To<T>(luaL_optinteger(L, index, ival));
                case ulong ival:
                    return Convert.To<T>((ulong)luaL_optinteger(L, index, (long)ival));
                case float fval:
                    return Convert.To<T>((float)luaL_optnumber(L, index, fval));
                case double fval:
                    return Convert.To<T>(luaL_optnumber(L, index, fval));
                case string strval:
                    return (T)(object)luaL_optstring(L, index, strval);
                case LuaNativeFunction fn:
                    return lua_isnoneornil(L, index) ? def : (T)(object)lua_tocfunction(L, index).ToLuaFunction();
                case LuaRef luaRef:
                    return lua_isnone(L, index) ? def : Convert.To<T>(new LuaRef(L, index));

                default:
                    return lua_isnoneornil(L, index) ? def : SharpObject.Get<T>(L, index);
            }

        }

        public static T Pop<T>(this LuaState L)
        {
            T v = Get<T>(L, -1);
            lua_pop(L, 1);
            return v;
        }

        public static object[] PopValues(this LuaState L, int oldTop)
        {
            int newTop = lua_gettop(L);

            if (oldTop == newTop)
                return null;

            var returnValues = new List<object>();
            for (int i = oldTop + 1; i <= newTop; i++)
                returnValues.Add(GetObject(L, i));

            lua_settop(L, oldTop);
            return returnValues.ToArray();
        }

        public unsafe static void PushGlobal(this LuaState L, string name)
        {
            var p = Marshal.StringToHGlobalAnsi(name);
            PushGlobal(L, (byte*)p);
            Marshal.FreeHGlobal(p);
        }

        public static void PopToGlobal(this LuaState L, string name)
        {
            var p = Marshal.StringToHGlobalAnsi(name);
            PopToGlobal(L, (byte*)p);
            Marshal.FreeHGlobal(p);
        }

        static byte* strchr(byte* p, char ch)
        {
            if (p == null)
            {
                return null;
            }
            while (*p != 0)
            {
                if (*p == ch)
                {
                    return p;
                }
                p = p + 1;
            }
            //strchr for '\0' should succeed - the while loop terminates
            //*p == 0, but ch also == 0, so NULL terminator address is returned
            return (*p == ch) ? p : null;
        }

        private unsafe static void PushGlobal(this LuaState L, byte* name)
        {
            byte* p = strchr(name, '.');
            if (p != null)
            {
                lua_pushglobaltable(L);                 // <table>
                while (p != null)
                {
                    lua_pushlstring(L, name, p - name); // <table> <key>

                    lua_gettable(L, -2);                // <table> <table_value>
                    lua_remove(L, -2);                  // <table_value>
                    if (lua_isnoneornil(L, -1)) return;
                    name = p + 1;
                    p = strchr(name, '.');
                }
                lua_pushstring(L, name);                // <last_table> <key>
                lua_gettable(L, -2);                    // <last_table> <table_value>
                lua_remove(L, -2);                      // <table_value>
            }
            else
            {
                lua_getglobal(L, name);
            }
        }

        private static unsafe void PopToGlobal(this LuaState L, byte* name)
        {
            byte* p = strchr(name, '.');
            if (p != null)
            {
                lua_pushglobaltable(L);                 // <value> <table>
                while (p != null)
                {
                    lua_pushlstring(L, name, p - name); // <value> <table> <key>
                    lua_gettable(L, -2);                // <value> <table> <table_value>
                    lua_remove(L, -2);                  // <value> <table_value>
                    name = p + 1;
                    p = strchr(name, '.');
                }
                lua_pushstring(L, name);                // <value> <last_table> <name>
                lua_pushvalue(L, -3);                   // <value> <last_table> <name> <value>
                lua_settable(L, -3);                    // <value> <last_table>
                lua_pop(L, 2);
            }
            else
            {
                lua_setglobal(L, name);
            }
        }

        #region LUA_DEBUG
        public static bool GetInfo(this LuaState L, string what, IntPtr ar)
        {
            return lua_getinfo(L, what, ar) != 0;
        }

        public static bool GetInfo(this LuaState L, string what, ref LuaDebug ar)
        {
            IntPtr pDebug = Marshal.AllocHGlobal(Marshal.SizeOf(ar));
            bool ret = false;
            try
            {
                Marshal.StructureToPtr(ar, pDebug, false);
                ret = lua_getinfo(L, what, pDebug) != 0;
                ar = LuaDebug.FromIntPtr(pDebug);

            }
            finally
            {
                Marshal.FreeHGlobal(pDebug);
            }

            return ret;
        }

        public static string GetLocal(this LuaState L, IntPtr ar, int n)
        {
            IntPtr ptr = lua_getlocal(L, ar, n);
            return Marshal.PtrToStringAnsi(ptr);
        }

        public static string GetLocal(this LuaState L, LuaDebug ar, int n)
        {
            IntPtr pDebug = Marshal.AllocHGlobal(Marshal.SizeOf(ar));
            string ret = string.Empty;
            try
            {
                Marshal.StructureToPtr(ar, pDebug, false);
                ret = GetLocal(L, pDebug, n);
                ar = LuaDebug.FromIntPtr(pDebug);
            }
            finally
            {
                Marshal.FreeHGlobal(pDebug);
            }
            return ret;
        }

        public static string SetLocal(this LuaState L, IntPtr ar, int n)
        {
            IntPtr ptr = lua_setlocal(L, ar, n);
            return Marshal.PtrToStringAnsi(ptr);
        }

        public static string SetLocal(this LuaState L, LuaDebug ar, int n)
        {
            IntPtr pDebug = Marshal.AllocHGlobal(Marshal.SizeOf(ar));
            string ret = string.Empty;
            try
            {
                Marshal.StructureToPtr(ar, pDebug, false);
                ret = SetLocal(L, pDebug, n);
                ar = LuaDebug.FromIntPtr(pDebug);
            }
            finally
            {
                Marshal.FreeHGlobal(pDebug);
            }
            return ret;
        }

        public static int GetStack(this LuaState L, int level, ref LuaDebug ar)
        {
            IntPtr pDebug = Marshal.AllocHGlobal(Marshal.SizeOf(ar));
            int ret = 0;
            try
            {
                Marshal.StructureToPtr(ar, pDebug, false);

                ret = lua_getstack(L, level, pDebug);
                ar = LuaDebug.FromIntPtr(pDebug);

            }
            finally
            {
                Marshal.FreeHGlobal(pDebug);
            }
            return ret;
        }

        #endregion
    }
}
