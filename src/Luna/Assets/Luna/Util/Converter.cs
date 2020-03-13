#define IL2CPP

using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;

namespace SharpLuna
{
    using static Lua;
    using lua_State = IntPtr;
    public class CustomConverter
    {
        public Type type;
        public Func<IntPtr, int, object> getter;
        public Action<IntPtr, object> pusher;

        public CustomConverter()
        {
        }

        public CustomConverter(Type type, Func<IntPtr, int, object> getter)
        {
            this.type = type;
            this.getter = getter;
        }

        public virtual object Get(IntPtr L, int index)
        {
            return getter(L, index);
        }

        public virtual void Push(IntPtr L, object data)
        {            
            pusher(L, data);
        }

        public virtual T Get<T>(IntPtr L, int index)
        {
            return (T)getter(L, index);
        }

        public virtual void Push<T>(IntPtr L, T data)
        {
            pusher(L, data);
        }
    }

    public unsafe class StructConverter : CustomConverter
    {
        public int metaRef = -1;
        public int newRef = -1;
        public int unpackRef = -1;
        public int size;
        public StructElement[] structElements;

        public StructConverter(IntPtr L, Type unmanagedType)
        {
            this.type = unmanagedType;
            structElements = unmanagedType.GetLayout(out size);

            lua_getglobal(L, type.Name);

            lua_getfield(L, -1, "unpack");
            unpackRef = luaL_ref(L, LUA_REGISTRYINDEX);

            lua_getfield(L, -1, "pack");
            newRef = luaL_ref(L, LUA_REGISTRYINDEX);

            //luaL_getmetafield(L, -1, "__call");            
            //newRef = luaL_ref(L, LUA_REGISTRYINDEX);

            metaRef = luaL_ref(L, LUA_REGISTRYINDEX);
        }

        public override object Get(IntPtr L, int index)
        {
            if(getter != null)
            {
                return getter(L, index);
            }

            IntPtr ptr = Marshal.AllocHGlobal(size);

            if (unpackRef == -1)
                LunaNative.luna_getstruct(L, index, ptr, (StructElement*)Unsafe.AsPointer(ref structElements[0]), structElements.Length);
            else
                LunaNative.luna_unpackstruct(L, index, unpackRef, ptr, (StructElement*)Unsafe.AsPointer(ref structElements[0]), structElements.Length);
            object boxed = Marshal.PtrToStructure(ptr, type);
            Marshal.FreeHGlobal(ptr);
            return boxed;
        }

        public override void Push(IntPtr L, object data)
        {
            if (pusher != null)
            {
                pusher(L, data);
                return;
            }

            IntPtr ptr = Marshal.AllocHGlobal(size);
            Marshal.StructureToPtr(data, ptr, false);
            if(newRef == -1)
                LunaNative.luna_pushstruct(L, metaRef, ptr, (StructElement*)Unsafe.AsPointer(ref structElements[0]), structElements.Length);
            else
                LunaNative.luna_packstruct(L, newRef, ptr, (StructElement*)Unsafe.AsPointer(ref structElements[0]), structElements.Length);
            Marshal.FreeHGlobal(ptr);
        }

        public override T Get<T>(IntPtr L, int index)
        {
            T data = default;
            if (unpackRef == -1)
                LunaNative.luna_getstruct(L, index, (IntPtr)Unsafe.AsPointer(ref data), (StructElement*)Unsafe.AsPointer(ref structElements[0]), structElements.Length);
            else
                LunaNative.luna_unpackstruct(L, index, unpackRef, (IntPtr)Unsafe.AsPointer(ref data), (StructElement*)Unsafe.AsPointer(ref structElements[0]), structElements.Length);
            return data;

        }

        public override void Push<T>(IntPtr L, T data)
        {
            if (newRef == -1)
                LunaNative.luna_pushstruct(L, metaRef, (IntPtr)Unsafe.AsPointer(ref data), (StructElement*)Unsafe.AsPointer(ref structElements[0]), structElements.Length);
            else
                LunaNative.luna_packstruct(L, newRef, (IntPtr)Unsafe.AsPointer(ref data), (StructElement*)Unsafe.AsPointer(ref structElements[0]), structElements.Length);

        }

    }

    public static class Converter
    {
        static Dictionary<Type, CustomConverter> converterFactory = new Dictionary<Type, CustomConverter>();
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
        
        public static CustomConverter GetConverter(Type type)
        {
            if (converterFactory.TryGetValue(type, out var f))
            {
                return f;
            }

            return null;
        }

        public static void RegStructConverter<T>(IntPtr L) where T : unmanaged
        {
            var c = new StructConverter(L, typeof(T));
            converterFactory[typeof(T)] = c;
            Debug.Assert(c.size == Marshal.SizeOf<T>());
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

            return fac.Get(L, index);
        }

        public static T Convert<T>( LuaType luaType, IntPtr L, int index)
        {
            Type type = typeof(T);
            if (!converterFactory.TryGetValue(type, out var fac))
            {
                return default;
            }

            return fac.Get<T>(L, index);
        }

        public static void Register<T>(Func<IntPtr, int, object> factory)
        {
            converterFactory[typeof(T)] = new CustomConverter(typeof(T), factory);
        }

        public static void Register(Type type, Func<IntPtr, int, object> factory)
        {
            converterFactory[type] = new CustomConverter(type, factory);
        }

        public static void RegisterAction()
        {
            Register(typeof(Action), ActionFactory.Create);
        }

        public static void RegisterAction<T1>()
        {
            Register(typeof(Action<T1>), ActionFactory<T1>.Create);
        }

        public static void RegisterAction<T1, T2>()
        {
            Register(typeof(Action<T1, T2>), ActionFactory<T1, T2>.Create);
        }

        public static void RegisterAction<T1, T2, T3>()
        {
            Register(typeof(Action<T1, T2, T3>), ActionFactory<T1, T2, T3>.Create);
        }

        public static void RegisterAction<T1, T2, T3, T4>()
        {
            Register(typeof(Action<T1, T2, T3, T4>), ActionFactory<T1, T2, T3, T4>.Create);
        }

        public static void RegisterFunc<R>()
        {
            Register(typeof(Func<R>), FuncFactory<R>.Create);
        }

        public static void RegisterFunc<T1,R>()
        {
            Register(typeof(Func<T1, R>), FuncFactory<T1, R>.Create);
        }

        public static void RegisterFunc<T1, T2, R>()
        {
            Register(typeof(Func<T1, T2, R>), FuncFactory<T1, T2, R>.Create);
        }

        public static void RegisterFunc<T1, T2, T3, R>()
        {
            Register(typeof(Func<T1, T2, T3, R>), FuncFactory<T1, T2, T3, R>.Create);
        }

        public static void RegisterFunc<T1, T2, T3, T4, R>()
        {
            Register(typeof(Func<T1, T2, T3, T4, R>), FuncFactory<T1, T2, T3, T4, R>.Create);
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
