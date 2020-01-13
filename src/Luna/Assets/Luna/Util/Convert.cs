#define UNBOXING

using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;

namespace SharpLuna
{
    /// <summary>
    /// struct类型转化成 T, 避免box
    /// </summary>
    public static class Convert
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T To<T>(this bool v)
        {
#if UNBOXING
            T val = default;
            __refvalue(__makeref(val), bool) = v;
            return val;
#else
            return (T)(object)v;
#endif
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T To<T>(this char v)
        {
#if UNBOXING
            T val = default;
            __refvalue(__makeref(val), char) = v;
            return val;
#else
            return (T)(object)v;
#endif
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T To<T>(this sbyte v)
        {
#if UNBOXING
            T val = default;
            __refvalue(__makeref(val), sbyte) = v;
            return val;
#else
            return (T)(object)v;
#endif
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T To<T>(this byte v)
        {
#if UNBOXING
            T val = default;
            __refvalue(__makeref(val), byte) = v;
            return val;
#else
            return (T)(object)v;
#endif
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T To<T>(this short v)
        {
#if UNBOXING
            T val = default;
            __refvalue(__makeref(val), short) = v;
            return val;
#else
            return (T)(object)v;
#endif
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T To<T>(this ushort v)
        {
#if UNBOXING
            T val = default;
            __refvalue(__makeref(val), ushort) = v;
            return val;
#else
            return (T)(object)v;
#endif
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T To<T>(this int v)
        {
#if UNBOXING
            T val = default;
            __refvalue(__makeref(val), int) = v;
            return val;
#else
            return (T)(object)v;
#endif
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T To<T>(this uint v)
        {
#if UNBOXING
            T val = default;
            __refvalue(__makeref(val), uint) = v;
            return val;
#else
            return (T)(object)v;
#endif
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T To<T>(this long v)
        {
#if UNBOXING
            T val = default;
            __refvalue(__makeref(val), long) = v;
            return val;
#else
            return (T)(object)v;
#endif
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T To<T>(this ulong v)
        {
#if UNBOXING
            T val = default;
            __refvalue(__makeref(val), ulong) = v;
            return val;
#else
            return (T)(object)v;
#endif
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T To<T>(this float v)
        {
#if UNBOXING
            T val = default;
            __refvalue(__makeref(val), float) = v;
            return val;
#else
            return (T)(object)v;
#endif
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T To<T>(this double v)
        {
#if UNBOXING
            T val = default;
            __refvalue(__makeref(val), double) = v;
            return val;
#else
            return (T)(object)v;
#endif
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T To<T>(this decimal v)
        {
#if UNBOXING
            T val = default;
            __refvalue(__makeref(val), decimal) = v;
            return val;
#else
            return (T)(object)v;
#endif
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T To<T>(this DateTime v)
        {
#if UNBOXING
            T val = default;
            __refvalue(__makeref(val), DateTime) = v;
            return val;
#else
            return (T)(object)v;
#endif
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T To<T>(this LuaRef v)
        {
#if UNBOXING
            T val = default;
            __refvalue(__makeref(val), LuaRef) = v;
            return val;
#else
            return (T)(object)v;
#endif
        }




    }
}
