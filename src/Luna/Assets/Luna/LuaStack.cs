
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;


namespace SharpLuna
{
    using lua_State = IntPtr;

    public unsafe static partial class Lua
    {
        public unsafe static void GetOrCreate(lua_State L, string name)
        {
            var p = Marshal.StringToHGlobalAnsi(name);
            GetGlobal(L, (byte*)p, true);
            Marshal.FreeHGlobal(p);
        }

        public unsafe static void GetGlobal(lua_State L, string name)
        {
            var p = Marshal.StringToHGlobalAnsi(name);
            GetGlobal(L, (byte*)p, false);
            Marshal.FreeHGlobal(p);
        }

        public static void SetGlobal(lua_State L, string name)
        {
            var p = Marshal.StringToHGlobalAnsi(name);
            SetGlobal(L, (byte*)p);
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

        private unsafe static void GetGlobal(lua_State L, byte* name, bool create)
        {
            byte* p = strchr(name, '.');
            if (p != null)
            {
                lua_pushglobaltable(L);                 // <table>
                while (p != null)
                {
                    lua_pushlstring(L, name, p - name); // <table> <key>

                    lua_gettable(L, -2);                // <table> <table_value>
                    
                    if (lua_isnoneornil(L, -1))
                    {
                        if (!create)
                        {
                            lua_remove(L, -2);
                            return;
                        }

                        lua_pop(L, 1);
                        lua_pushlstring(L, name, p - name);
                        lua_newtable(L);
                        lua_settable(L, -3);
                        lua_pushlstring(L, name, p - name); // <table> <key>
                        lua_gettable(L, -2);                // <table> <table_value>                        
                    }
                                        
                    lua_remove(L, -2);                  // <table_value>
                    
                    name = p + 1;
                    p = strchr(name, '.');
                }
                lua_pushstring(L, name);                // <last_table> <key>
                lua_gettable(L, -2);                    // <last_table> <table_value>
               
                if (create && lua_isnoneornil(L, -1))
                {
                    lua_pop(L, 1);
                    lua_pushstring(L, name);
                    lua_newtable(L);
                    lua_settable(L, -3);
                    lua_pushstring(L, name);            // <table> <key>
                    lua_gettable(L, -2);                // <table> <table_value>
                }

                lua_remove(L, -2);                      // <table_value>
            }
            else
            {
                lua_getglobal(L, name);

                if (create && lua_isnoneornil(L, -1))
                {
                    lua_pop(L, 1);
                    lua_newtable(L);
                    lua_setglobal(L, name);
                    lua_getglobal(L, name);                // <table> <table_value>
                                                        //return;
                }
            }
        }

        private static unsafe void SetGlobal(lua_State L, byte* name)
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

        public static unsafe void PushLightObject<T>(lua_State L, T obj)
        {
            GCHandle gc = GCHandle.Alloc(obj, GCHandleType.Normal);
            lua_pushlightuserdata(L, GCHandle.ToIntPtr(gc));
        }

        public static T ToLightObject<T>(lua_State L, int index, bool freeGCHandle = true)
        {
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

        public static void PushArgs(lua_State L, params object[] args)
        {
            foreach (var obj in args)
            {
                Lua.Push(L, obj);
            }
        }

        public static T Pop<T>(lua_State L)
        {
            T v = Get<T>(L, -1);
            lua_pop(L, 1);
            return v;
        }

        public static object[] PopValues(lua_State L, int oldTop)
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

        public static object[] PopValues(lua_State L, int oldTop, Type[] returnTypes)
        {
            int newTop = lua_gettop(L);

            if (oldTop == newTop)
                return null;

            var returnValues = new List<object>();
            for (int i = oldTop + 1; i <= newTop; i++)
                returnValues.Add(GetObject(L, i, returnTypes[i]));

            lua_settop(L, oldTop);
            return returnValues.ToArray();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Push(lua_State L, object obj)
        {
            Type t = obj.GetType();
            if (t.IsPrimitive)
            {
                if (t == typeof(bool))
                    lua_pushboolean(L, ((bool)obj) ? 1 : 0);
                else if (t == typeof(long))
                    lua_pushinteger(L, (long)obj);
                else if (t == typeof(ulong))
                    lua_pushinteger(L, (long)(ulong)obj);
                else if (t == typeof(sbyte))
                    lua_pushinteger(L, (long)(sbyte)obj);
                else if (t == typeof(byte))
                    lua_pushnumber(L, (double)(byte)obj);
                else if (t == typeof(short))
                    lua_pushnumber(L, (double)(short)obj);
                else if (t == typeof(ushort))
                    lua_pushnumber(L, (double)(ushort)obj);
                else if (t == typeof(char))
                    lua_pushnumber(L, (double)(char)obj);
                else if (t == typeof(int))
                    lua_pushnumber(L, (double)(int)obj);
                else if (t == typeof(uint))
                    lua_pushnumber(L, (double)(uint)obj);
                else if (t == typeof(float))
                    lua_pushnumber(L, (double)(float)obj);
                else if (t == typeof(double))
                    lua_pushnumber(L, (double)obj);
                else
                    throw new Exception("未知类型");
            }
            else if (t == typeof(IntPtr))
                lua_pushinteger(L, (long)(IntPtr)obj);
            else if (t == typeof(UIntPtr))
                lua_pushinteger(L, (long)(UIntPtr)obj);
            else if (t == typeof(string))
                lua_pushstring(L, (string)obj);
            else if (t == typeof(LuaNativeFunction))
                lua_pushcfunction(L, (LuaNativeFunction)obj);
            else if (t == typeof(byte[]))
                Push(L, (byte[])obj);
            else if (t == typeof(LuaByteBuffer))
                Push(L, (LuaByteBuffer)obj);           
            else if (t == typeof(LuaRef))
            {
                var luaRef = (LuaRef)obj;
                if (luaRef.IsValid)
                {
                    luaRef.PushToStack();
                }
                else
                {
                    lua_pushnil(L);
                }
            }
            else
            {
                if (t.IsEnum)
                {
                    lua_pushinteger(L, (int)(object)obj);
                    return;
                }
                else if (t.IsValueType)
                {
                    if(t.IsUnManaged())
                    {
                        SharpObject.PushUnmanagedObject(L, obj);
                        return;
                    }

                }
                
                SharpObject.PushToStack(L, obj);
            }

        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Push(lua_State L, bool v) => lua_pushboolean(L, v ? 1 : 0);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Push(lua_State L, long v) => lua_pushinteger(L, v);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Push(lua_State L, ulong v) => lua_pushinteger(L, (long)v);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Push(lua_State L, IntPtr v) => lua_pushinteger(L, (long)v);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Push(lua_State L, UIntPtr v) => lua_pushinteger(L, (long)v);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Push(lua_State L, sbyte v) => lua_pushnumber(L, v);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Push(lua_State L, byte v) => lua_pushnumber(L, v);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Push(lua_State L, short v) => lua_pushnumber(L, v);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Push(lua_State L, ushort v) => lua_pushnumber(L, v);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Push(lua_State L, int v) => lua_pushnumber(L, v);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Push(lua_State L, uint v) => lua_pushnumber(L, v);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Push(lua_State L, float v) => lua_pushnumber(L, v);
  
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Push(lua_State L, double v) => lua_pushnumber(L, v);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Push(lua_State L, string v) => lua_pushstring(L, v);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Push(lua_State L, LuaNativeFunction v) => lua_pushcfunction(L, v);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Push<T>(lua_State L, T v) => SharpObject.PushToStack(L, v);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Push<T>(lua_State L, ref T v) where T : struct => SharpObject.PushValueToStack(L, ref v);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void PushT<T>(lua_State L, T v)
        {
            switch (v)
            {
                case bool bval:
                    lua_pushboolean(L, bval ? 1 : 0);
                    break;
                case long lval:
                    lua_pushinteger(L, lval);
                    break;
                case ulong lval:
                    lua_pushinteger(L, (long)lval);
                    break;
                case IntPtr lval:
                    lua_pushinteger(L, (long)lval);
                    break;
                case UIntPtr lval:
                    lua_pushinteger(L, (long)lval);
                    break;
                case sbyte ival:
                    lua_pushnumber(L, ival);
                    break;
                case byte ival:
                    lua_pushnumber(L, ival);
                    break;
                case short ival:
                    lua_pushnumber(L, ival);
                    break;
                case ushort ival:
                    lua_pushnumber(L, ival);
                    break;
                case int ival:
                    lua_pushnumber(L, ival);
                    break;
                case uint ival:
                    lua_pushnumber(L, ival);
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
                case byte[] bytes:
                    Push(L, bytes);
                    break;
                case LuaByteBuffer bytes:
                    Push(L, bytes);
                    break;
                default:
                    Type t = v.GetType();
                    if (t.IsEnum)
                    {
                        lua_pushinteger(L, (int)(object)v);
                    }
                    else if (t.IsValueType)
                    {
                        if (t.IsUnManaged())
                        {
                            SharpObject.PushUnmanagedObject(L, v);
                            return;
                        }

                    }

                    SharpObject.PushToStack(L, v);
                    break;
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static object GetObject(lua_State L, int index)
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
                    return (bool)(lua_toboolean(L, index) != 0);
                case LuaType.Table:
                    {
                        Get(L, index, out LuaRef luaref);
                        return luaref;
                    }
                case LuaType.Function:
                    {
                        Get(L, index, out LuaRef luaref);
                        return luaref;
                    }
                case LuaType.LightUserData:
                    return ToLightObject<object>(L, index);
                case LuaType.UserData:
                    return SharpObject.Get<object>(L, index);
                default:
                    return null;
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static object GetObject(lua_State L, int index, Type objtype)
        {
            if (objtype == typeof(float))
            {
                return (float)lua_tonumber(L, index);
            }
            if (objtype == typeof(double))
            {
                return lua_tonumber(L, index);
            }
            else if (objtype == typeof(int))
            {
                return (int)lua_tonumber(L, index);
            }
            else if (objtype == typeof(uint))
            {
                return (uint)lua_tonumber(L, index);
            }
            else if (objtype == typeof(short))
            {
                return (short)lua_tonumber(L, index);
            }
            else if (objtype == typeof(ushort))
            {
                return (ushort)lua_tonumber(L, index);
            }
            else if (objtype == typeof(sbyte))
            {
                return (sbyte)lua_tonumber(L, index);
            }
            else if (objtype == typeof(byte))
            {
                return (byte)lua_tonumber(L, index);
            }
            else if (objtype == typeof(string))
            {
                return lua_tostring(L, index);
            }
            else if (objtype == typeof(bool))
            {
                return (bool)(lua_toboolean(L, index) != 0);
            }
            else if (objtype == typeof(IntPtr))
            {
                return (IntPtr)lua_tonumber(L, index);
            }
            else if (objtype == typeof(UIntPtr))
            {
                return (uint)lua_tonumber(L, index);
            }
            else if (objtype == typeof(LuaNativeFunction))
            {
                return lua_tocfunction(L, index).ToLuaFunction();
            }
            else if (objtype == typeof(byte[]))
            {
                Get(L, index, out byte[] v);
                return v;
            }
            else if (objtype == typeof(LuaByteBuffer))
            {
                Get(L, index, out LuaByteBuffer v);
                return v;
            }
            else if (objtype == typeof(LuaRef))
            {
                LuaType type = lua_type(L, index);
                var obj = Converter.Convert(objtype, type, L, index);
                if (obj != null)
                    return obj;
                Get(L, index, out LuaRef luaref);
                return luaref;
            }
            else if (objtype == typeof(object))
            {
                return GetObject(L, index);
            }
            else
            {
                if (objtype.IsEnum)
                {
                    return (int)lua_tonumber(L, index);
                }
                if (objtype.IsValueType)
                {
                    if (objtype.IsUnManaged())
                    {
                        return SharpObject.GetUnmanaged(L, index, objtype);
                    }
                }

                return SharpObject.Get(L, index, objtype);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Get(lua_State L, int index, out bool v) => v = lua_toboolean(L, index) != 0;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Get(lua_State L, int index, out long v) => v = (long)luaL_checkinteger(L, index);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Get(lua_State L, int index, out ulong v) => v = (ulong)luaL_checkinteger(L, index);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Get(lua_State L, int index, out IntPtr v) => v = (IntPtr)luaL_checkinteger(L, index);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Get(lua_State L, int index, out UIntPtr v) => v = (UIntPtr)luaL_checkinteger(L, index);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Get(lua_State L, int index, out int v) => v = (int)luaL_checknumber(L, index);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Get(lua_State L, int index, out uint v) => v = (uint)luaL_checkinteger(L, index);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Get(lua_State L, int index, out short v) => v = (short)luaL_checkinteger(L, index);
 
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Get(lua_State L, int index, out ushort v) => v = (ushort)luaL_checkinteger(L, index);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Get(lua_State L, int index, out char v) => v = (char)luaL_checkinteger(L, index);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Get(lua_State L, int index, out sbyte v) => v = (sbyte)luaL_checkinteger(L, index);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Get(lua_State L, int index, out byte v) => v = (byte)luaL_checkinteger(L, index);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Get(lua_State L, int index, out float v) => v = (float)luaL_checknumber(L, index);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Get(lua_State L, int index, out double v) => v = (double)luaL_checknumber(L, index);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Get(lua_State L, int index, out string v) => v = lua_checkstring(L, index);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Get(lua_State L, int index, out LuaNativeFunction v) => v = lua_tocfunction(L, index).ToLuaFunction();

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Get(lua_State L, int index, out LuaRef v)
        {
            if (lua_isnone(L, index))
                v = LuaRef.None;
            else
                v = new LuaRef(L, index);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Get(lua_State L, int index, out object v) => v = SharpObject.Get<object>(L, index);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Get<T>(lua_State L, int index, out T v) => v = SharpObject.Get<T>(L, index);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void GetT<T>(lua_State L, int index, out T v) => v = Get<T>(L, index);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T Get<T>(lua_State L, int index)
        {
            Type t = typeof(T);

            if (t.IsPrimitive)
            {
                if (t == typeof(bool))
                    return Converter.To<T>((bool)(lua_toboolean(L, index) != 0));
                else if (t == typeof(char))
                    return Converter.To<T>((char)luaL_checkinteger(L, index));
                else if (t == typeof(sbyte))
                    return Converter.To<T>((sbyte)luaL_checkinteger(L, index));
                else if (t == typeof(byte))
                    return Converter.To<T>((byte)luaL_checkinteger(L, index));
                else if (t == typeof(short))
                    return Converter.To<T>((short)luaL_checkinteger(L, index));
                else if (t == typeof(ushort))
                    return Converter.To<T>((ushort)luaL_checkinteger(L, index));
                else if (t == typeof(int))
                    return Converter.To<T>((int)luaL_checkinteger(L, index));
                else if (t == typeof(uint))
                    return Converter.To<T>((uint)luaL_checkinteger(L, index));
                else if (t == typeof(long))
                    return Converter.To<T>((long)luaL_checkinteger(L, index));
                else if (t == typeof(ulong))
                    return Converter.To<T>((ulong)luaL_checkinteger(L, index));
                else if (t == typeof(float))
                    return Converter.To<T>((float)luaL_checknumber(L, index));
                else if (t == typeof(double))
                    return Converter.To<T>(luaL_checknumber(L, index));
                else
                    throw new Exception("Error type");
            }
            else if (t == typeof(IntPtr))
                return Converter.To<T>((IntPtr)luaL_checkinteger(L, index));
            else if (t == typeof(UIntPtr))
                return Converter.To<T>((UIntPtr)luaL_checkinteger(L, index));
            else if (t == typeof(string))
                return (T)(object)lua_checkstring(L, index);
            else if (t == typeof(LuaNativeFunction))
                return (T)(object)lua_tocfunction(L, index).ToLuaFunction();
            else if (t == typeof(LuaByteBuffer))
            {
                Get(L, index, out LuaByteBuffer buffer);
                return (T)(object)buffer;
            }
            else if (t == typeof(byte[]))
            {
                Get(L, index, out byte[] buffer);
                return (T)(object)buffer;
            }
            else if (t == typeof(LuaRef))
            {
                if (lua_isnone(L, index))
                    return (T)(object)null;// Convert.To<T>(LuaRef.None);
                else
                    return Converter.To<T>(new LuaRef(L, index));
            }
            else
            {
                if (t == typeof(object))
                {
                    return (T)GetObject(L, index);
                }
                else if (t.IsEnum)
                {
                    if(lua_type(L, index) == LuaType.Number)
                        return Converter.To<T>((int)luaL_checkinteger(L, index));
                }
                else if (t.IsValueType)
                {
                    if (t.IsUnManaged())
                    {
                        return SharpObject.GetUnmanaged<T>(L, index);
                    }
                }

                return SharpObject.Get<T>(L, index);
            }

        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T Opt<T>(lua_State L, int index, T def)
        {
            switch (def)
            {
                case bool bval:
                    return Converter.To<T>(luaL_optinteger(L, index, bval ? 1 : 0));
                case sbyte ival:
                    return Converter.To<T>((sbyte)luaL_optinteger(L, index, ival));
                case byte ival:
                    return Converter.To<T>((byte)luaL_optinteger(L, index, ival));
                case short ival:
                    return Converter.To<T>((short)luaL_optinteger(L, index, ival));
                case ushort ival:
                    return Converter.To<T>((ushort)luaL_optinteger(L, index, ival));
                case char ival:
                    return Converter.To<T>((char)luaL_optinteger(L, index, ival));
                case int ival:
                    return Converter.To<T>((int)luaL_optinteger(L, index, ival));
                case uint ival:
                    return Converter.To<T>((uint)luaL_optinteger(L, index, ival));
                case long ival:
                    return Converter.To<T>(luaL_optinteger(L, index, ival));
                case ulong ival:
                    return Converter.To<T>((ulong)luaL_optinteger(L, index, (long)ival));
                case IntPtr ival:
                    return Converter.To<T>((IntPtr)luaL_optinteger(L, index, (long)ival));
                case UIntPtr ival:
                    return Converter.To<T>((UIntPtr)luaL_optinteger(L, index, (long)ival));
                case float fval:
                    return Converter.To<T>((float)luaL_optnumber(L, index, fval));
                case double fval:
                    return Converter.To<T>(luaL_optnumber(L, index, fval));
                case string strval:
                    return (T)(object)luaL_optstring(L, index, strval);
                case LuaNativeFunction fn:
                    return lua_isnoneornil(L, index) ? def : (T)(object)lua_tocfunction(L, index).ToLuaFunction();
                case LuaByteBuffer v:
                    {
                        if (lua_isnoneornil(L, index))
                            return def;
                        Get(L, index, out LuaByteBuffer buffer);
                        return (T)(object)buffer;
                    }
                case byte[] v:
                    {
                        if (lua_isnoneornil(L, index))
                            return def;
                        Get(L, index, out byte[] buffer);
                        return (T)(object)buffer;
                    }
                case LuaRef luaRef:
                    return lua_isnone(L, index) ? def : Converter.To<T>(new LuaRef(L, index));
                default:
                    {
                        if (typeof(T).IsEnum)
                        {
                            return Converter.To<T>((int)luaL_optinteger(L, index, (long)(object)def));
                        }

                        if (lua_isnoneornil(L, index)) return def;

                        if (typeof(T).IsUnManaged())
                        {
                            return SharpObject.GetUnmanaged<T>(L, index);
                        }

                        return SharpObject.Get<T>(L, index);
                    }
            }

        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool CheckType(lua_State L, int index, Type t)
        {
            LuaType luaType = lua_type(L, index);

            if (t == typeof(bool))
                return luaType == LuaType.Boolean;
            else if (t == typeof(string))
                return luaType == LuaType.String;
            else if (t.IsPrimitive)
                return luaType == LuaType.Number;
            else if (t == typeof(LuaNativeFunction))
                return luaType == LuaType.Function;
            else if (t == typeof(IntPtr))
                return luaType == LuaType.Number;
            else if (t == typeof(UIntPtr))
                return luaType == LuaType.Number;
            else if (t == typeof(LuaRef))
                return luaType == LuaType.Table || luaType == LuaType.Function;
            else if (t == typeof(LuaByteBuffer))
                return luaType == LuaType.String;
            else
            {
                return luaType == LuaType.UserData;
            }

        }

        public static bool CheckType(lua_State L, int index, Type[] types)
        {
            for (int i = 0; i < types.Length; i++)
            {
                if (!CheckType(L, index, types[i]))
                {
                    return false;
                }
            }

            return true;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool CheckType<T>(lua_State L, int index)
        {
            return CheckType(L, index, typeof(T));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool CheckType<T1, T2>(lua_State L, int index)
        {
            return CheckType(L, index, typeof(T1)) && CheckType(L, index + 1, typeof(T2));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool CheckType<T1, T2, T3>(lua_State L, int index)
        {
            return CheckType(L, index, typeof(T1)) && CheckType(L, index + 1, typeof(T2)) && CheckType(L, index + 2, typeof(T3));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool CheckType<T1, T2, T3, T4>(lua_State L, int index)
        {
            return CheckType(L, index, typeof(T1)) && CheckType(L, index + 1, typeof(T2)) && CheckType(L, index + 2, typeof(T3)) && CheckType(L, index + 3, typeof(T4));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool CheckType<T1, T2, T3, T4, T5>(lua_State L, int index)
        {
            return CheckType(L, index, typeof(T1)) && CheckType(L, index + 1, typeof(T2)) && CheckType(L, index + 2, typeof(T3)) && CheckType(L, index + 3, typeof(T4))
                && CheckType(L, index + 4, typeof(T5));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool CheckType<T1, T2, T3, T4, T5, T6>(lua_State L, int index)
        {
            return CheckType(L, index, typeof(T1)) && CheckType(L, index + 1, typeof(T2)) && CheckType(L, index + 2, typeof(T3)) && CheckType(L, index + 3, typeof(T4))
                && CheckType(L, index + 4, typeof(T5)) && CheckType(L, index + 5, typeof(T6));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool CheckType<T1, T2, T3, T4, T5, T6, T7>(lua_State L, int index)
        {
            return CheckType(L, index, typeof(T1)) && CheckType(L, index + 1, typeof(T2)) && CheckType(L, index + 2, typeof(T3)) && CheckType(L, index + 3, typeof(T4))
                && CheckType(L, index + 4, typeof(T5)) && CheckType(L, index + 5, typeof(T6)) && CheckType(L, index + 6, typeof(T7));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool CheckType<T1, T2, T3, T4, T5, T6, T7, T8>(lua_State L, int index)
        {
            return CheckType(L, index, typeof(T1)) && CheckType(L, index + 1, typeof(T2)) && CheckType(L, index + 2, typeof(T3)) && CheckType(L, index + 3, typeof(T4))
                && CheckType(L, index + 4, typeof(T5)) && CheckType(L, index + 5, typeof(T6)) && CheckType(L, index + 6, typeof(T7)) && CheckType(L, index + 7, typeof(T8));
        }


        #region LUA_DEBUG
        public static bool GetInfo(lua_State L, string what, IntPtr ar)
        {
            return lua_getinfo(L, what, ar) != 0;
        }

        public static bool GetInfo(lua_State L, string what, ref LuaDebug ar)
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

        public static string GetLocal(lua_State L, IntPtr ar, int n)
        {
            IntPtr ptr = lua_getlocal(L, ar, n);
            return Marshal.PtrToStringAnsi(ptr);
        }

        public static string GetLocal(lua_State L, LuaDebug ar, int n)
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

        public static string SetLocal(lua_State L, IntPtr ar, int n)
        {
            IntPtr ptr = lua_setlocal(L, ar, n);
            return Marshal.PtrToStringAnsi(ptr);
        }

        public static string SetLocal(lua_State L, LuaDebug ar, int n)
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

        public static int GetStack(lua_State L, int level, ref LuaDebug ar)
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
