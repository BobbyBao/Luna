#define LUA_WEAKTABLE
#define C_API

using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using UnityEngine;

namespace SharpLuna
{
    using static Lua;
    using lua_State = IntPtr;

    public class SharpObject
    {
#if LUA_WEAKTABLE
        static FreeList<object> freeList = new FreeList<object>(1024);
        static Dictionary<object, int> obj2id = new Dictionary<object, int>();
        static int weakTableRef;
#else       
        static ConditionalWeakTable<object, UserDataRef> objectUserData = new ConditionalWeakTable<object, UserDataRef>();
#endif

        static Dictionary<Type, FreeList<int>> delegateBridge = new Dictionary<Type, FreeList<int>>();

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
        public static int TypeID<T>(T obj) => obj.GetType().GetHashCode();

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int TypeID<T>() => typeof(T).GetHashCode();

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int TypeID(Type type) => type?.GetHashCode() ?? 0;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static unsafe void AllocObject<T>(lua_State L, T obj)
        {
            int classId = TypeID(obj);

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

#if DEBUG || UNITY_EDITOR

            if(!lua_istable(L, -1))
            {
                Debug.LogWarning($"class not registered : {obj.GetType() }, obj: {obj}");
                lua_pop(L, 1);
                return;
            }
            //luaL_checktype(L, -1, (int)LuaType.Table);
#endif
            lua_setmetatable(L, -2);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static unsafe void AllocUnmanagedObject<T>(lua_State L, T obj) where T : unmanaged
        {
            int classId = TypeID(obj);
            IntPtr mem = lua_newuserdata(L, (UIntPtr)Unsafe.SizeOf<T>());
            Unsafe.Write((void*)mem, obj);

            lua_rawgeti(L, LUA_REGISTRYINDEX, classId);

#if DEBUG || UNITY_EDITOR

            if (!lua_istable(L, -1))
            {
                Debug.LogWarning($"class not registered : {obj.GetType() }, obj: {obj}");
                lua_pop(L, 1);
                return;
            }
            //luaL_checktype(L, -1, (int)LuaType.Table);
#endif
            lua_setmetatable(L, -2);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void PushUnmanagedObject<T>(lua_State L, in T obj) where T : unmanaged
        {
            AllocUnmanagedObject(L, obj);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void PushValueToStack<T>(lua_State L, ref T obj) where T : struct
        {
            AllocObject(L, obj);
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
            AllocObject(L, obj);
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
            AllocObject(L, obj);
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
            if (typeof(T).IsUnManaged())
            {
                return ref GetUnmanaged<T>(L, index);
            }
            else
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
                    return null;
                }

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
            LuaType type = lua_type(L, index);

            if (type != LuaType.UserData)
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
        public static void FreeStruct(lua_State L, int index)
        {
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

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Free(lua_State L, int index)
        {
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


#if !LUA_WEAKTABLE
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

    }

}
