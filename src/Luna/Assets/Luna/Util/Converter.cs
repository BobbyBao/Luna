#define IL2CPP

using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;

namespace SharpLuna
{
    using static Lua;
    using lua_State = IntPtr;

    public static class Converter
    {
        static Dictionary<Type, Func<IntPtr, int, object>> delegateFactory = new Dictionary<Type, Func<IntPtr, int, object>>();

        static Dictionary<Type, Func<IntPtr, int, object>> customConverter = new Dictionary<Type, Func<IntPtr, int, object>>();

        static Converter()
        {
            Register<Action<string>>(CreateActionString);
            Register<Action<int>>(CreateActionInt);
        }


        public static Func<IntPtr, int, object> GetFactory(Type type)
        {
            if (delegateFactory.TryGetValue(type, out var f))
            {
                return f;
            }

            return null;
        }

        public static object Convert(Type type, IntPtr L, int index)
        {
            if (!delegateFactory.TryGetValue(type, out var fac))
            {
                return null;
            }

            return fac(L, index);
        }

        public static void Register<T>(Func<IntPtr, int, object> factory) where T : Delegate
        {
            delegateFactory[typeof(T)] = factory;
        }

        public static void Register(Type type, Func<IntPtr, int, object> factory)
        {
            delegateFactory[type] = factory;
        }

        static Action<string> CreateActionString(IntPtr L, int index)
        {
            lua_pushvalue(L, index);
            int luaref = luaL_ref(L, LUA_REGISTRYINDEX);
            return (data) =>
            {
                lua_pushcfunction(L, LuaException.traceback);
                lua_rawgeti(L, LUA_REGISTRYINDEX, luaref);
                Push(L, data);
                if (lua_pcall(L, 1, 0, -1 + 2) != (int)LuaStatus.OK)
                {
                    lua_remove(L, -2);
                    throw new LuaException(L);
                }
                lua_pop(L, 1);
            };
        }

        static Action<int> CreateActionInt(IntPtr L, int index)
        {
            lua_pushvalue(L, index);
            int luaref = luaL_ref(L, LUA_REGISTRYINDEX);
            return (data) =>
            {
                lua_pushcfunction(L, LuaException.traceback);
                lua_rawgeti(L, LUA_REGISTRYINDEX, luaref);
                Push(L, data);
                if (lua_pcall(L, 1, 0, -1 + 2) != (int)LuaStatus.OK)
                {
                    lua_remove(L, -2);
                    throw new LuaException(L);
                }
                lua_pop(L, 1);
            };
        }
#if IL2CPP
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T To<T>(this object v)
        {
            return (T)(object)v;
        }

#else

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T To<T>(this bool v)
        {
            T val = default;
            __refvalue(__makeref(val), bool) = v;
            return val;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T To<T>(this char v)
        {
            T val = default;
            __refvalue(__makeref(val), char) = v;
            return val;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T To<T>(this sbyte v)
        {
            T val = default;
            __refvalue(__makeref(val), sbyte) = v;
            return val;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T To<T>(this byte v)
        {
            T val = default;
            __refvalue(__makeref(val), byte) = v;
            return val;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T To<T>(this short v)
        {
            T val = default;
            __refvalue(__makeref(val), short) = v;
            return val;
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T To<T>(this ushort v)
        {
            T val = default;
            __refvalue(__makeref(val), ushort) = v;
            return val;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T To<T>(this int v)
        {
            T val = default;
            __refvalue(__makeref(val), int) = v;
            return val;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T To<T>(this uint v)
        {
            T val = default;
            __refvalue(__makeref(val), uint) = v;
            return val;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T To<T>(this long v)
        {
            T val = default;
            __refvalue(__makeref(val), long) = v;
            return val;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T To<T>(this ulong v)
        {
            T val = default;
            __refvalue(__makeref(val), ulong) = v;
            return val;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T To<T>(this float v)
        {
            T val = default;
            __refvalue(__makeref(val), float) = v;
            return val;
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T To<T>(this double v)
        {
            T val = default;
            __refvalue(__makeref(val), double) = v;
            return val;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T To<T>(this decimal v)
        {
            T val = default;
            __refvalue(__makeref(val), decimal) = v;
            return val;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T To<T>(this DateTime v)
        {
            T val = default;
            __refvalue(__makeref(val), DateTime) = v;
            return val;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T To<T>(this IntPtr v)
        {
            T val = default;
            __refvalue(__makeref(val), IntPtr) = v;
            return val;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T To<T>(this UIntPtr v)
        {
            T val = default;
            __refvalue(__makeref(val), UIntPtr) = v;
            return val;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T To<T>(this LuaRef v)
        {
            T val = default;
            __refvalue(__makeref(val), LuaRef) = v;
            return val;
        }
#endif



    }
}
