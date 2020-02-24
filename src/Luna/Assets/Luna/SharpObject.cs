#define LUA_WEAKTABLE
#define C_API

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
            public readonly static int value = typeof(T).GetHashCode();
        }

#if LUA_WEAKTABLE
        static FreeList<object> freeList = new FreeList<object>(1024);
        static Dictionary<object, int> obj2id = new Dictionary<object, int>();
        static int weakTableRef;
#else
        class UserDataRef : IDisposable
        {
            public int Ref { get; }

            private lua_State L;
            public UserDataRef(lua_State l, int r)
            {
                L = l;
                Ref = r;
            }

            ~UserDataRef()
            {
                if (L != IntPtr.Zero)
                {
                    if (isactive(L))
                    {
                        luaL_unref(L, LUA_REGISTRYINDEX, Ref);
                        L = IntPtr.Zero;
                    }
                }
            }

            public void Dispose()
            {
                if (L != IntPtr.Zero)
                {
                    luaL_unref(L, LUA_REGISTRYINDEX, Ref);
                    L = IntPtr.Zero;
                }
                GC.SuppressFinalize(this);
            }
        }

        static ConditionalWeakTable<object, UserDataRef> objectUserData = new ConditionalWeakTable<object, UserDataRef>();

#endif

        static Dictionary<LuaRef, Delegate> delegateBridge = new Dictionary<LuaRef, Delegate>();

        public static void Init(lua_State L)
        {
#if LUA_WEAKTABLE
            lua_newtable(L);
            lua_newtable(L);
            lua_pushstring(L, "__mode");
            lua_pushstring(L, "v");
            lua_rawset(L, -3);
            lua_setmetatable(L, -2);
            weakTableRef = luaL_ref(L, LUA_REGISTRYINDEX);
#endif
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int Signature<T>(T obj)
        {
            return obj.GetType().GetHashCode();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int Signature<T>()
        {
            return SignatureHolder<T>.value;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int Signature(Type type)
        {
            if (type == null) return 0;

            return type.GetHashCode();
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
#if LUA_WEAKTABLE
            if (obj2id.TryGetValue(obj, out var key))
            {
                if (TryGetUserData(L, key, weakTableRef) == 1)
                {
                    return;
                }
            }
#else
            if (objectUserData.TryGetValue(obj, out var userRef))
            {
                lua_rawgeti(L, LUA_REGISTRYINDEX, userRef.Ref);
                return;
            }
#endif
            AllocObject(L, Signature(obj), obj);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void PushToStack<T>(lua_State L, object obj)
        {
#if LUA_WEAKTABLE
            if (obj2id.TryGetValue(obj, out var key))
            {
                if (TryGetUserData(L, key, weakTableRef) == 1)
                {
                    return;
                }
            }
#else
            if (objectUserData.TryGetValue(obj, out var userRef))
            {
                lua_rawgeti(L, LUA_REGISTRYINDEX, userRef.Ref);
                return;
            }
#endif
            AllocObject(L, Signature(obj), obj);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static unsafe void AllocValueObject<T>(lua_State L, int classId, T obj)
        {
            IntPtr mem = lua_newuserdata(L, (UIntPtr)Unsafe.SizeOf<T>());
            Unsafe.Write((void*)mem, obj);

            lua_rawgeti(L, LUA_REGISTRYINDEX, classId);
            luaL_checktype(L, -1, (int)LuaType.Table);
            lua_setmetatable(L, -2);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static unsafe void AllocObject<T>(lua_State L, int classId, T obj)
        {
            //             if (typeof(T).IsUnManaged())
            //             {
            //                 return AllocValueObject(L, classId, obj);
            //             }

            IntPtr mem = lua_newuserdata(L, (UIntPtr)sizeof(long));
       
#if LUA_WEAKTABLE
            long id = freeList.Alloc(obj);
            *((long*)mem) = id;
            obj2id.Add(obj, (int)id);

            CacheUserData(L, id, weakTableRef);
#else
            GCHandle gc = GCHandle.Alloc(obj, GCHandleType.Weak);
            Unsafe.Write((void*)mem, GCHandle.ToIntPtr(gc));

            int userRef = luaL_ref(L, LUA_REGISTRYINDEX);
            lua_rawgeti(L, LUA_REGISTRYINDEX, userRef);
            UserDataRef userDataRef = new UserDataRef(L, userRef);
            objectUserData.Add(obj, userDataRef);
#endif
            lua_rawgeti(L, LUA_REGISTRYINDEX, classId);
#if DEBUG
            luaL_checktype(L, -1, (int)LuaType.Table);
#endif
            lua_setmetatable(L, -2);
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
                var handle = GetHandler(L, index);
#if LUA_WEAKTABLE
                return ref Unsafe.Unbox<T>(freeList[(int)handle]);
#else
                return ref Unsafe.Unbox<T>(GCHandle.FromIntPtr((IntPtr)handle).Target);
#endif
            }

        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static object Get(lua_State L, int index, Type t)
        {
            LuaType type = lua_type(L, index);
            if (type != LuaType.UserData)
            {
                if(t == null)
                {
                    //                    
                    return null;
                }

                //Lua.Get(L, index, out LuaRef func);
                return Converter.Convert(t, L, index);
            }

            var handle = GetHandler(L, index);
#if LUA_WEAKTABLE
            return freeList[(int)handle];
#else
            return GCHandle.FromIntPtr((IntPtr)handle).Target;
#endif
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T Get<T>(lua_State L, int index)
        {
            //             if (typeof(T).IsUnManaged())
            //             {
            //                 return GetUnmanaged<T>(L, index);
            //             }
         
            LuaType type = lua_type(L, index);
            if (type == LuaType.Function)
            {
                return (T)Converter.Convert(typeof(T), L, index);
            }
            
            var handle = GetHandler(L, index);
#if LUA_WEAKTABLE
            return (T)freeList[(int)handle];
#else
            return (T)GCHandle.FromIntPtr((IntPtr)handle).Target;
#endif
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static unsafe long GetHandler(lua_State L, int index)
        {
            var ptr = lua_touserdata(L, index);
            //var ptr = lua_topointer(L, index);
#if DEBUG
            if (ptr == IntPtr.Zero)
            {
                return 0;
            }
#endif
            return *((long*)ptr);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Free(lua_State L, int index)
        {
            //             if (typeof(T).IsUnManaged())
            //             {
            //                 return;
            //             }

            var handle = GetHandler(L, index);
#if LUA_WEAKTABLE
            var obj = freeList[(int)handle];
            obj2id.Remove(obj);
            freeList.Free((int)handle);
#else
            GCHandle gCHandle = GCHandle.FromIntPtr((IntPtr)handle);
            if (gCHandle.IsAllocated)
            {
                gCHandle.Free();
            }
#endif
        }

#if LUA_WEAKTABLE
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static int TryGetUserData(lua_State L, long key, int cache_ref)
        {
#if C_API
            return luna_try_getuserdata(L, key, cache_ref);
#else
            lua_rawgeti(L, LUA_REGISTRYINDEX, cache_ref);
            lua_rawgeti(L, -1, key);
            if (!lua_isnil(L, -1))
            {
                lua_remove(L, -2);
                return 1;
            }
            lua_pop(L, 2);
            return 0;
#endif
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static void CacheUserData(lua_State L, long key, int cache_ref)
        {
#if C_API
            luna_cacheuserdata(L, key, cache_ref);
#else
            lua_rawgeti(L, LUA_REGISTRYINDEX, cache_ref);
            lua_pushvalue(L, -2);
            lua_rawseti(L, -2, key);
            lua_pop(L, 1);
#endif
        }
#endif
        }
    }
