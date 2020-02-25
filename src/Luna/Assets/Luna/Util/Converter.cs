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
        static Converter()
        {
            RegisterAction<int>();
            RegisterAction<string>();
            RegisterAction<object>();

            RegisterFunc<int>();
            RegisterFunc<string>();
            RegisterFunc<object>();
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

        public static void Register<T>(Func<IntPtr, int, object> factory)
        {
            delegateFactory[typeof(T)] = factory;
        }

        public static void Register(Type type, Func<IntPtr, int, object> factory)
        {
            delegateFactory[type] = factory;
        }

        public static void RegisterAction()
        {
            delegateFactory[typeof(Action)] = ActionFactory.Create;
        }

        public static void RegisterAction<T1>()
        {
            delegateFactory[typeof(Action<T1>)] = ActionFactory<T1>.Create;
        }

        public static void RegisterAction<T1, T2>()
        {
            delegateFactory[typeof(Action<T1, T2>)] = ActionFactory<T1, T2>.Create;
        }

        public static void RegisterAction<T1, T2, T3>()
        {
            delegateFactory[typeof(Action<T1, T2, T3>)] = ActionFactory<T1, T2, T3>.Create;
        }

        public static void RegisterAction<T1, T2, T3, T4>()
        {
            delegateFactory[typeof(Action<T1, T2, T3, T4>)] = ActionFactory<T1, T2, T3, T4>.Create;
        }

        public static void RegisterFunc<R>()
        {
            delegateFactory[typeof(Func<R>)] = FuncFactory<R>.Create;
        }

        public static void RegisterFunc<T1,R>()
        {
            delegateFactory[typeof(Func<T1, R>)] = FuncFactory<T1, R>.Create;
        }

        public static void RegisterFunc<T1, T2, R>()
        {
            delegateFactory[typeof(Func<T1, T2, R>)] = FuncFactory<T1, T2, R>.Create;
        }

        public static void RegisterFunc<T1, T2, T3, R>()
        {
            delegateFactory[typeof(Func<T1, T2, T3, R>)] = FuncFactory<T1, T2, T3, R>.Create;
        }

        public static void RegisterFunc<T1, T2, T3, T4, R>()
        {
            delegateFactory[typeof(Func<T1, T2, T3, T4, R>)] = FuncFactory<T1, T2, T3, T4, R>.Create;
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
