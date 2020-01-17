#define MEM_TRICK
#define FREE_LIST
//#define WEAK_FREELIST

using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;

namespace SharpLuna
{
    using static Lua;
    using lua_State = IntPtr;

    public class SharpObject
    {
        struct SignatureHolder<T>
        {
            public readonly static IntPtr value = (IntPtr)typeof(T).GetHashCode();//MetadataToken;//FullName.GetHashCode();
        }

        class UserDataRef : IDisposable
        {
            private readonly lua_State L;
            public int Ref { get; }
            public UserDataRef(lua_State l, int r)
            {
                L = l;
                Ref = r;
            }

            ~UserDataRef()
            {
                luaL_unref(L, LUA_REGISTRYINDEX, Ref);
            }

            public void Dispose()
            {
                luaL_unref(L, LUA_REGISTRYINDEX, Ref);
                GC.SuppressFinalize(this);
            }
        }

#if FREE_LIST
#if WEAK_FREELIST
        static WeakFreeList<object> freeList = new WeakFreeList<object>(1024);
#else
        static FreeList<object> freeList = new FreeList<object>(1024);
#endif
#endif
        static ConditionalWeakTable<object, UserDataRef> objectUserData = new ConditionalWeakTable<object, UserDataRef>();

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static IntPtr Signature<T>(T obj)
        {
            return (IntPtr)obj.GetType().GetHashCode();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static IntPtr Signature<T>()
        {
            return SignatureHolder<T>.value;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static IntPtr Signature(Type type)
        {
            if (type == null) return IntPtr.Zero;

            return (IntPtr)type.GetHashCode();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void PushToStack<T>(lua_State L) where T : new()
        {
            T obj = new T();
            AllocObject(L, Signature<T>(), obj);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void PushToStack<T>(lua_State L, T obj)
        {
            if (objectUserData.TryGetValue(obj, out var userRef))
            {
                lua_rawgeti(L, LUA_REGISTRYINDEX, userRef.Ref);
                return;
            }

            AllocObject(L, Signature<T>(), obj);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void PushToStack<T>(lua_State L, object obj)
        {
            if (objectUserData.TryGetValue(obj, out var userRef))
            {
                lua_rawgeti(L, LUA_REGISTRYINDEX, userRef.Ref);
                return;
            }

            AllocObject(L, Signature(obj), obj);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static unsafe IntPtr AllocValueObject<T>(lua_State L, IntPtr classId, T obj)
        {
            IntPtr mem = lua_newuserdata(L, (UIntPtr)Unsafe.SizeOf<T>());
            Unsafe.Write((void*)mem, obj);

            lua_rawgetp(L, LUA_REGISTRYINDEX, classId);
            luaL_checktype(L, -1, (int)LuaType.Table);
            lua_setmetatable(L, -2);
            return mem;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static unsafe void AllocObject<T>(lua_State L, IntPtr classId, T obj)
        {
            //             if (typeof(T).IsUnManaged())
            //             {
            //                 return AllocValueObject(L, classId, obj);
            //             }

            IntPtr mem = lua_newuserdata(L, (UIntPtr)sizeof(long));
            lua_rawgetp(L, LUA_REGISTRYINDEX, classId);
            luaL_checktype(L, -1, (int)LuaType.Table);
            lua_setmetatable(L, -2);

            int userRef = luaL_ref(L, LUA_REGISTRYINDEX);
            lua_rawgeti(L, LUA_REGISTRYINDEX, userRef);

#if FREE_LIST
            long id = freeList.Alloc(obj);
            *((long*)mem) = id;
#else
            //Unity的Object由UnityEngine管理，采用弱引用的方式，防止C#和Lua相互引用导致无法gc
#if UNITY_2018_1_OR_NEWER
            GCHandleType handleType = obj is UnityEngine.Object ? GCHandleType.Weak : GCHandleType.Normal;
#else
            GCHandleType handleType = GCHandleType.Normal;
#endif

            GCHandle gc = GCHandle.Alloc(obj, handleType);
            Unsafe.Write((void*)mem, GCHandle.ToIntPtr(gc));
#endif

            UserDataRef userDataRef = new UserDataRef(L, userRef);          
            objectUserData.Add(obj, userDataRef);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public unsafe static ref T GetUnmanaged<T>(lua_State L, int index)
        {
            var ptr = lua_touserdata(L, index);
            return ref Unsafe.AsRef<T>((void*)ptr);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public unsafe static ref T GetValue<T>(lua_State L, int index) where T : struct
        {
//             if (typeof(T).IsUnManaged())
//             {
//                 return ref GetUnmanaged<T>(L, index);
//             }
//             else
            {
                var handle = GetHandler<T>(L, index);
#if FREE_LIST
                return ref Unsafe.Unbox<T>(freeList[(int)handle]);
#else
                return ref Unsafe.Unbox<T>(GCHandle.FromIntPtr(handle).Target);
#endif
            }

        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T Get<T>(lua_State L, int index)
        {
//             if (typeof(T).IsUnManaged())
//             {
//                 return GetUnmanaged<T>(L, index);
//             }

            var handle = GetHandler<T>(L, index);
#if FREE_LIST
            return (T)freeList[(int)handle];
#else
            return (T)GCHandle.FromIntPtr(handle).Target;
#endif
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static unsafe long GetHandler<T>(lua_State L, int index)
        {
            var ptr = lua_touserdata(L, index);
#if DEBUG
            if (ptr == IntPtr.Zero)
            {
                return 0;
            }
#endif
            return *((long*)ptr);
        }

        public static void Free<T>(lua_State L, int index)
        {
            //             if (typeof(T).IsUnManaged())
            //             {
            //                 return;
            //             }

            var handle = GetHandler<T>(L, index);
#if FREE_LIST
            freeList.Free((int)handle);
#else
            GCHandle gCHandle = GCHandle.FromIntPtr(handle);
            if (gCHandle.IsAllocated)
            {
                gCHandle.Free();
            }
#endif
        }

    }
}
