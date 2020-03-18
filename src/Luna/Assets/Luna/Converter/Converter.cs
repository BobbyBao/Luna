#define IL2CPP

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;

namespace SharpLuna
{
    using static Lua;
    using lua_State = IntPtr;

    public partial class Converter
    {
        public Type type;
        public Func<IntPtr, int, object> getter;
        public Action<IntPtr, object> pusher;

        public Converter()
        {
        }

        public Converter(Type type, Func<IntPtr, int, object> getter)
        {
            this.type = type;
            this.getter = getter;
        }
    }

    public class TConverter<T> : Converter
    {
        public TConverter()
        {
        }

        public TConverter(Func<IntPtr, int, object> getter)
        {
            this.type = typeof(T);
            this.getter = getter;
        }

        public virtual T Get(IntPtr L, int index)
        {
            return (T)getter(L, index);
        }

        public virtual void Push(IntPtr L, T data)
        {
            pusher(L, data);
        }

    }

    public partial class Converter
    {
        static Dictionary<Type, Converter> converterFactory = new Dictionary<Type, Converter>();
        protected static Dictionary<IntPtr, Converter> converterCache = new Dictionary<IntPtr, Converter>();
        static Converter()
        {
            RegisterAction();
            RegisterAction<bool>();
            RegisterAction<int>();
            RegisterAction<string>();
            RegisterAction<object>();

            RegisterFunc<bool>();
            RegisterFunc<int>();
            RegisterFunc<string>();
            RegisterFunc<object>();
        }

        public static Converter GetConverter(Type type)
        {
            if (converterFactory.TryGetValue(type, out var f))
            {
                return f;
            }

            return null;
        }

        public static object Convert(Type type, LuaType luaType, IntPtr L, int index)
        {
            if (type.IsEnum)
            {
                assert(false);
                Get(L, index, out int v);
                return (object)v;
            }

            if (!converterFactory.TryGetValue(type, out var fac))
            {
                return null;
            }
              
            return fac.getter(L, index);            
        }

        public static T Convert<T>(LuaType luaType, IntPtr L, int index)
        {
            Type type = typeof(T);
            if (!converterFactory.TryGetValue(type, out var fac))
            {
                return default;
            }

            return ((TConverter<T>)fac).Get(L, index);
        }

        public static void Register<T>(Converter converter)
        {
            converterFactory[typeof(T)] = converter;
        }

        public static void RegUnmanagedConverter<T>(IntPtr L) where T : unmanaged
        {
            var c = new UnamanagedConverter<T>(L, typeof(T));
            converterFactory[typeof(T)] = c;
            System.Diagnostics.Debug.Assert(c.Size == Marshal.SizeOf<T>());
        }

        public static void RegDelegateFactory<T>(Func<IntPtr, int, T> factory) where T : class
        {
            converterFactory[typeof(T)] = new DelegateBridge<T>(factory);
        }

        public static void RegisterAction()
        {
            RegDelegateFactory(ActionFactory.Create);
        }

        public static void RegisterAction<T1>()
        {
            RegDelegateFactory(ActionFactory<T1>.Create);
        }

        public static void RegisterAction<T1, T2>()
        {
            RegDelegateFactory(ActionFactory<T1, T2>.Create);
        }

        public static void RegisterAction<T1, T2, T3>()
        {
            RegDelegateFactory(ActionFactory<T1, T2, T3>.Create);
        }

        public static void RegisterAction<T1, T2, T3, T4>()
        {
            RegDelegateFactory(ActionFactory<T1, T2, T3, T4>.Create);
        }

        public static void RegisterFunc<R>()
        {
            RegDelegateFactory(FuncFactory<R>.Create);
        }

        public static void RegisterFunc<T1, R>()
        {
            RegDelegateFactory(FuncFactory<T1, R>.Create);
        }

        public static void RegisterFunc<T1, T2, R>()
        {
            RegDelegateFactory(FuncFactory<T1, T2, R>.Create);
        }

        public static void RegisterFunc<T1, T2, T3, R>()
        {
            RegDelegateFactory(FuncFactory<T1, T2, T3, R>.Create);
        }

        public static void RegisterFunc<T1, T2, T3, T4, R>()
        {
            RegDelegateFactory(FuncFactory<T1, T2, T3, T4, R>.Create);
        }


#if IL2CPP
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T To<T>(object v)
        {
            return (T)v;
        }

#else

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T To<T>(bool v)
        {
            T val = default;
            __refvalue(__makeref(val), bool) = v;
            return val;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T To<T>(char v)
        {
            T val = default;
            __refvalue(__makeref(val), char) = v;
            return val;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T To<T>(sbyte v)
        {
            T val = default;
            __refvalue(__makeref(val), sbyte) = v;
            return val;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T To<T>(byte v)
        {
            T val = default;
            __refvalue(__makeref(val), byte) = v;
            return val;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T To<T>(short v)
        {
            T val = default;
            __refvalue(__makeref(val), short) = v;
            return val;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T To<T>(ushort v)
        {
            T val = default;
            __refvalue(__makeref(val), ushort) = v;
            return val;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T To<T>(int v)
        {
            T val = default;
            __refvalue(__makeref(val), int) = v;
            return val;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T To<T>(uint v)
        {
            T val = default;
            __refvalue(__makeref(val), uint) = v;
            return val;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T To<T>(long v)
        {
            T val = default;
            __refvalue(__makeref(val), long) = v;
            return val;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T To<T>(ulong v)
        {
            T val = default;
            __refvalue(__makeref(val), ulong) = v;
            return val;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T To<T>(float v)
        {
            T val = default;
            __refvalue(__makeref(val), float) = v;
            return val;
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T To<T>(double v)
        {
            T val = default;
            __refvalue(__makeref(val), double) = v;
            return val;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T To<T>(decimal v)
        {
            T val = default;
            __refvalue(__makeref(val), decimal) = v;
            return val;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T To<T>(DateTime v)
        {
            T val = default;
            __refvalue(__makeref(val), DateTime) = v;
            return val;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T To<T>(IntPtr v)
        {
            T val = default;
            __refvalue(__makeref(val), IntPtr) = v;
            return val;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T To<T>(UIntPtr v)
        {
            T val = default;
            __refvalue(__makeref(val), UIntPtr) = v;
            return val;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T To<T>(LuaRef v)
        {
            T val = default;
            __refvalue(__makeref(val), LuaRef) = v;
            return val;
        }
#endif


    }

}
