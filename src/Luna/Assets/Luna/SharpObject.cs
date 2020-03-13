#define LUA_WEAKTABLE

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
        class ReferenceEqualsComparer : IEqualityComparer<object>
        {
            public new bool Equals(object o1, object o2)
            {
                return object.ReferenceEquals(o1, o2);
            }
            public int GetHashCode(object obj)
            {
                return System.Runtime.CompilerServices.RuntimeHelpers.GetHashCode(obj);
            }
        }

#if LUA_WEAKTABLE
        static FreeList<object> freeList = new FreeList<object>(1024);
        static Dictionary<object, int> obj2id = new Dictionary<object, int>(new ReferenceEqualsComparer());
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
            int id = freeList.Alloc(obj);
            *((int*)mem) = 0;
            *(((int*)mem) + 1) = id;

            obj2id[obj] = (int)id;

            LunaNative.CacheUserData(L, id, weakTableRef);
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
                Debug.LogWarning($"class not registered : {obj.GetType() }, id: {classId} obj: {obj}");
                lua_pop(L, 1);
                return;
            }
            //luaL_checktype(L, -1, (int)LuaType.Table);
#endif
            lua_setmetatable(L, -2);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static unsafe void AllocUnmanagedObject<T>(lua_State L, T obj)// where T : unmanaged
        {
            int classId = TypeID(obj);
            IntPtr mem = lua_newuserdata(L, (UIntPtr)Unsafe.SizeOf<T>() + 4);
            Unsafe.Write((void*)(mem + 4), obj);

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
        public static unsafe void AllocUnmanagedObject(lua_State L, object obj)
        {
            int classId = TypeID(obj);
            IntPtr mem = lua_newuserdata(L, (UIntPtr)Marshal.SizeOf(obj) + 4);
            Marshal.StructureToPtr(obj, mem + 4, false);

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
        public static void PushUnmanagedObject(lua_State L, in object obj) //where T : unmanaged
        {
            AllocUnmanagedObject(L, obj);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void PushUnmanagedObject<T>(lua_State L, in T obj) //where T : unmanaged
        {
            AllocUnmanagedObject(L, obj);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void PushValueToStack<T>(lua_State L, ref T obj) //where T : struct
        {
            if (typeof(T).IsUnManaged())
            {
                AllocUnmanagedObject(L, obj);
            }
            else
            {
                AllocObject(L, obj);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void PushToStack<T>(lua_State L, T obj)
        {
#if LUA_WEAKTABLE
            if (obj2id.TryGetValue(obj, out var key))
            {
                if (LunaNative.TryGetUserData(L, key, weakTableRef) == 1)
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
                if (LunaNative.TryGetUserData(L, key, weakTableRef) == 1)
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
            LuaType type = lua_type(L, index);
            if (type != LuaType.UserData)
            {
                Debug.LogError("error type : " + type);
                assert(false);
            }
               
            var ptr = lua_touserdata(L, index);            
            return ref Unsafe.AsRef<T>((void*)(ptr + 4));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public unsafe static object GetUnmanaged(lua_State L, int index, Type type)
        {
            var ptr = lua_touserdata(L, index);
            return Marshal.PtrToStructure(ptr + 4, type);
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
        public static object Get(lua_State L, int index)
        {
            var handle = GetHandler(L, index);
#if LUA_WEAKTABLE
            return freeList[(int)handle];
#else
            return GCHandle.FromIntPtr((IntPtr)handle).Target;
#endif
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
        static unsafe int GetHandler(lua_State L, int index)
        {
            var ptr = lua_touserdata(L, index);
            //var ptr = lua_topointer(L, index);
#if DEBUG
            if (ptr == IntPtr.Zero)
            {
                return 0;
            }
#endif
            return *(((int*)ptr) + 1);
            //return *((int*)ptr);
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
            //Debug.Log("gc : " + handle);

#if LUA_WEAKTABLE
            var obj = freeList[handle];
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

    }

}
