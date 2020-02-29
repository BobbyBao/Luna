#define SAVE_FUNC
using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Collections.Concurrent;

namespace SharpLuna
{
    using size_t = System.UIntPtr;
    using lua_State = System.IntPtr;
    using voidptr_t = System.IntPtr;
    using charptr_t = System.IntPtr;
    using lua_CFunction = System.IntPtr;
    using lua_Integer = System.Int64;
    using lua_Number = System.Double;

    using static Lua;

    public unsafe static partial class Lua
    {
        public const int LUAI_MAXSTACK = 1000000;
        public const int LUA_REGISTRYINDEX = (-LUAI_MAXSTACK - 1000);
        public const int LUA_RIDX_GLOBALS = 2;

        public const int LUA_NOREF = (-2);
        public const int LUA_REFNIL = (-1);
        
#if LUNA_SCRIPT
        public const int STATIC_STARTSTACK = 2;
#else
	    public const int STATIC_STARTSTACK = 1;
#endif
        internal static Encoding Encoding { get; set; } = Encoding.UTF8;
        internal static HashSet<object> savedFn = new HashSet<object>();

        static ConcurrentDictionary<IntPtr, List<IDisposable>> luaStates = new ConcurrentDictionary<IntPtr, List<IDisposable>>();

        public static lua_State newstate()
        {
            var L = luaL_newstate();
            luaStates.TryAdd(L, new List<IDisposable>());
            return L;
        }

        public static void closestate(this lua_State L)
        {
            if (!luaStates.TryRemove(L, out var state))
            {
                assert(false);
            }

            lua_close(L);
        }

        public static bool isactive(this lua_State L)
        {
            if (L == IntPtr.Zero)
            {
                return false;
            }

            return luaStates.ContainsKey(L);
        }


        public static void addref(this lua_State L, IDisposable r)
        {
            if (luaStates.TryGetValue(L, out var refs))
            {
                refs.Add(r);
            }
        }

        public static void unref(this lua_State L, IDisposable r)
        {
            if (luaStates.TryGetValue(L, out var refs))
            {
                refs.FastRemove(r);
            }
        }

        public static void unrefall(this lua_State L)
        {
            if (luaStates.TryGetValue(L, out var refs))
            {
                while (refs.Count != 0)
                {
                    var r = refs[refs.Count - 1];
                    r?.Dispose();
                }
            }
        }

        public static LuaNativeFunction lua_atpanic(lua_State L, LuaNativeFunction panicFunction)
        {
#if SAVE_FUNC
            savedFn.TryAdd(panicFunction);
#endif
            IntPtr newPanicPtr = panicFunction.ToFunctionPointer();
            return lua_atpanic(L, newPanicPtr).ToLuaFunction();
        }

        public static void lua_sethook(lua_State L, KyHookFunction hookFunction, LuaHookMask mask, int count)
        {
#if SAVE_FUNC
            savedFn.TryAdd(hookFunction);
#endif
            lua_sethook(L, hookFunction.ToFunctionPointer(), (int)mask, count);
        }

        public static LuaStatus luaL_loadbuffer(lua_State L, byte[] buff, string name = null, string mode = null) 
            => (LuaStatus)luaL_loadbufferx(L, buff, (UIntPtr)buff.Length, name, mode);

        public static LuaStatus lua_loadstring(lua_State L, string chunk, string name)
        {
            byte[] buffer = Encoding.GetBytes(chunk);
            return (LuaStatus)luaL_loadbufferx(L, buffer, (UIntPtr)buffer.Length, name, null);
        }

        public static string lua_typename(lua_State L, LuaType type)
        {
            return Marshal.PtrToStringAnsi(lua_typename(L, (int)type));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static lua_Number lua_tonumber(lua_State L, int i) => lua_tonumberx(L, (i), null);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static lua_Integer lua_tointeger(lua_State L, int i) => lua_tointegerx(L, (i), null);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void lua_pop(lua_State L, int n) => lua_settop(L, -(n) - 1);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void lua_newtable(lua_State L) => lua_createtable(L, 0, 0);

        public static void lua_register(lua_State L, string n, LuaNativeFunction f)
        {
#if SAVE_FUNC
            savedFn.TryAdd(f);
#endif
            lua_pushcfunction(L, (f));
            var p = Marshal.StringToHGlobalAnsi(n);
            lua_setglobal(L, (byte*)(p));
            Marshal.FreeHGlobal(p);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool lua_isfunction(lua_State L, int n) => (lua_type(L, (n)) == LuaType.Function);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool lua_istable(lua_State L, int n) => (lua_type(L, (n)) == LuaType.Table);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool lua_islightuserdata(lua_State L, int n) => (lua_type(L, (n)) == LuaType.LightUserData);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool lua_isnil(lua_State L, int n) => (lua_type(L, (n)) == LuaType.Nil);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool lua_isboolean(lua_State L, int n) => (lua_type(L, (n)) == LuaType.Boolean);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool lua_isthread(lua_State L, int n) => (lua_type(L, (n)) == LuaType.Thread);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool lua_isnone(lua_State L, int n) => (lua_type(L, (n)) == LuaType.None);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool lua_isnoneornil(lua_State L, int n) => (lua_type(L, (n)) <= 0);

        public static void lua_pushstring(lua_State L, string s)
        {
            byte[] buffer = Encoding.GetBytes(s);
            fixed(byte* p = &buffer[0])
            lua_pushlstring(L, (byte*)p, buffer.Length);
        }

        public static void lua_pushliteral(lua_State L, string s)
        {
            var p = Marshal.StringToHGlobalAnsi(s);
            lua_pushstring(L, (byte*)p);
            Marshal.FreeHGlobal(p);
        }

        public static void lua_pushcclosure(lua_State L, LuaNativeFunction function, int n)
        {
            //Delegate
            if(!function.Method.IsDefined(typeof(AOT.MonoPInvokeCallbackAttribute), false))
            {
                assert(false);
            }
#if SAVE_FUNC
            savedFn.TryAdd(function);
#endif
            lua_pushcclosure(L, function.ToFunctionPointer(), n);
        }

        public static void lua_pushcfunction(lua_State L, LuaNativeFunction function)
        {
#if SAVE_FUNC
            savedFn.TryAdd(function);
#endif
            lua_pushcclosure(L, function.ToFunctionPointer(), 0);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static voidptr_t lua_newuserdata(lua_State L, size_t size) => lua_newuserdatauv(L, size, 1);

        public static void lua_pushglobaltable(lua_State L) => lua_rawgeti(L, LUA_REGISTRYINDEX, LUA_RIDX_GLOBALS);

        public static string lua_checkstring(lua_State L, int i)
        {
            UIntPtr len;
            IntPtr buff = Lua.luaL_checklstring(L, i, out len);
            if (buff == IntPtr.Zero)
                return null;

            return Encoding.GetString((byte*)buff, (int)len);
        }

        public static string lua_tostring(lua_State L, int i)
        {
            size_t len;
            IntPtr buff = Lua.luaL_tolstring(L, i, &len);
            if (buff == IntPtr.Zero)
                return null;

            return Encoding.GetString((byte*)buff, (int)len);
        }

        public static string luaL_optstring(lua_State L, int n, string d) => Marshal.PtrToStringAnsi(luaL_optlstring(L, (n), (d), null));

        [Conditional("DEBUG")]
        public static void assert(bool condition) => Debug.Assert(condition);

        public static int luaL_error(lua_State L, string message, params object[] args)
        {
            return luaL_error(L, string.Format(message, args));
        }

        public static void lua_insert(lua_State L, int idx) => lua_rotate(L, (idx), 1);
        public static void lua_remove(lua_State L, int idx) { lua_rotate(L, (idx), -1); lua_pop(L, 1); }
        public static void lua_replace(lua_State L, int idx) { lua_copy(L, -1, (idx)); lua_pop(L, 1); }
        public static void lua_call(lua_State L, int n, int r) => lua_callk(L, (n), (r), IntPtr.Zero, IntPtr.Zero);
        public static LuaStatus lua_pcall(lua_State L, int n, int r, int f) => (LuaStatus)lua_pcallk(L, (n), (r), (f), IntPtr.Zero, IntPtr.Zero);
    
        public static LuaType lua_getglobal(lua_State L, string name)
        {
            var p = Marshal.StringToHGlobalAnsi(name);
            int ret = lua_getglobal(L, (byte*)p);
            Marshal.FreeHGlobal(p);
            return (LuaType)ret;
        }

        public static void lua_setglobal(lua_State L, string name)
        {
            var p = Marshal.StringToHGlobalAnsi(name);
            lua_setglobal(L, (byte*)p);
            Marshal.FreeHGlobal(p);
        }


    }
}
