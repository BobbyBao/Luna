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

    public class SharpObject
    {
        struct SignatureHolder<T>
        {
            public readonly static IntPtr value = (IntPtr)typeof(T).GetHashCode();//MetadataToken;//FullName.GetHashCode();
        }
#if FREE_LIST
#if WEAK_FREELIST
        static WeakFreeList<object> freeList = new WeakFreeList<object>(1024);
#else
        static FreeList<object> freeList = new FreeList<object>(1024);
#endif
#endif
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static IntPtr Signature<T>(T obj)
        {
            return (IntPtr)obj.GetType().GetHashCode();// MetadataToken;//FullName.GetHashCode();
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

            return (IntPtr)type.GetHashCode();//MetadataToken;//FullName.GetHashCode();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void PushToStack<T>(IntPtr L) where T : new()
        {
            T obj = new T();
            AllocObject(L, Signature<T>(), obj);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void PushToStack<T>(IntPtr L, T obj)
        {
            AllocObject(L, Signature<T>(), obj);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void PushToStack<T>(IntPtr L, object obj)
        {
            AllocObject(L, Signature(obj), obj);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static unsafe IntPtr AllocValueObject<T>(IntPtr L, IntPtr classId, T obj)
        {
            IntPtr mem = lua_newuserdata(L, (UIntPtr)Unsafe.SizeOf<T>());
            Unsafe.Write((void*)mem, obj);

            lua_rawgetp(L, LUA_REGISTRYINDEX, classId);
            luaL_checktype(L, -1, (int)LuaType.Table);
            lua_setmetatable(L, -2);
            return mem;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static unsafe IntPtr AllocObject<T>(IntPtr L, IntPtr classId, T obj)
        {
            //             if (typeof(T).IsUnManaged())
            //             {
            //                 return AllocValueObject(L, classId, obj);
            //             }

            IntPtr mem = lua_newuserdata(L, (UIntPtr)sizeof(long));
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

            lua_rawgetp(L, LUA_REGISTRYINDEX, classId);
            luaL_checktype(L, -1, (int)LuaType.Table);
            lua_setmetatable(L, -2);
            return mem;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public unsafe static ref T GetUnmanaged<T>(IntPtr L, int index)
        {
#if DEBUG
            var ptr = GetUserData(L, index, Signature<T>(), true, true);
#else
            var ptr = lua_touserdata(L, index);
#endif
            return ref Unsafe.AsRef<T>((void*)ptr);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public unsafe static ref T GetValue<T>(IntPtr L, int index) where T : struct
        {
//             if (typeof(T).IsUnManaged())
//             {
//                 return ref GetUnmanaged<T>(L, index);
//             }
//             else
            {
                var handle = GetHandler<T>(L, index, false, true);
#if FREE_LIST
                return ref Unsafe.Unbox<T>(freeList[(int)handle]);
#else
                return ref Unsafe.Unbox<T>(GCHandle.FromIntPtr(handle).Target);
#endif
            }

        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T Get<T>(IntPtr L, int index)
        {
//             if (typeof(T).IsUnManaged())
//             {
//                 return GetUnmanaged<T>(L, index);
//             }

            var handle = GetHandler<T>(L, index, false, true);
            if (handle == IntPtr.Zero)
            {
                return default;
            }
#if FREE_LIST
            return (T)freeList[(int)handle];
#else
            return (T)GCHandle.FromIntPtr(handle).Target;
#endif
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static unsafe IntPtr GetHandler<T>(IntPtr L, int index, bool is_exact, bool raise_error)
        {
#if DEBUG
            var ptr = GetUserData(L, index, Signature<T>(), is_exact, raise_error);
#else
            var ptr = lua_touserdata(L, index);
#endif
            if (ptr == IntPtr.Zero)
            {
                return IntPtr.Zero;
            }

            return *((IntPtr*)ptr);
        }

        public static void Free<T>(IntPtr L, int index)
        {
            //             if (typeof(T).IsUnManaged())
            //             {
            //                 return;
            //             }

            var handle = GetHandler<T>(L, index, false, true);
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

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static IntPtr GetUserData(IntPtr L, int index, IntPtr class_id, bool is_exact, bool raise_error)
        {
#if DEBUG
            if (!lua_isuserdata(L, index))
            {
                if (raise_error)
                {
                    luaL_error(L, "expect userdata, got %s", lua_typename(L, lua_type(L, index)));
                }
                return IntPtr.Zero;
            }
#endif

#if false
            // <SP: index> = <obj>
            index = lua_absindex(L, index);

            // get registry base class metatable -> <base_mt>
            lua_rawgetp(L, LUA_REGISTRYINDEX, class_id);

            // report error if no metatable
            if (!lua_istable(L, -1))
            {
                if (raise_error)
                {
                    luaL_error(L, "unknown class, you need to register this class first by using LuaBinding");
                }
                else
                {
                    lua_pop(L, 1);
                }
                return IntPtr.Zero;
            }

            // get the object metatable -> <base_mt> <obj_mt>
            lua_getmetatable(L, index);

            for (; ; )
            {
                // check if <obj_mt> and <base_mt> are equal
                if (lua_rawequal(L, -1, -2))
                {
                    // matched, return this object
                    lua_pop(L, 2);
                    break;
                }

                // give up if exact match is needed
                if (is_exact)
                {
                    if (raise_error)
                    {
                        TypeMismatchError(L, index);
                    }
                    else
                    {
                        lua_pop(L, 2);
                    }
                    return IntPtr.Zero;
                }

                // now try super class -> <base_mt> <obj_mt> <obj_super_mt>
                lua_pushliteral(L, "___super");
                lua_rawget(L, -2);

                if (lua_isnil(L, -1))
                {
                    // no super class
                    if (raise_error)
                    {
                        lua_pop(L, 1); // pop nil
                        TypeMismatchError(L, index);
                    }
                    else
                    {
                        lua_pop(L, 3);
                    }
                    return IntPtr.Zero;
                }
                else
                {
                    // continue with <obj_super_mt> -> <base_mt> <obj_super_mt>
                    lua_remove(L, -2);
                }
            }
#endif
            return lua_touserdata(L, index);
        }
    }
}
