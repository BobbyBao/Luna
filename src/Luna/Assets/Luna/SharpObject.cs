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

        static FreeList<object> freeList = new FreeList<object>(1024);
        static Dictionary<object, int> obj2id = new Dictionary<object, int>(new ReferenceEqualsComparer());
        static int weakTableRef;

        public static void Init(lua_State L)
        {
            lua_newtable(L);
            lua_newtable(L);
            lua_pushstring(L, "__mode");
            lua_pushstring(L, "v");
            lua_rawset(L, -3);
            lua_setmetatable(L, -2);
            weakTableRef = luaL_ref(L, LUA_REGISTRYINDEX);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int TypeID<T>(T obj) => obj.GetType().GetHashCode();

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int TypeID<T>() => typeof(T).GetHashCode();

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int TypeID(Type type) => type?.GetHashCode() ?? 0;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static unsafe void PushUnmanaged<T>(lua_State L, T obj)// where T : unmanaged
        {
            int classId = TypeID(obj);
            IntPtr mem = lua_newuserdata(L, (UIntPtr)Unsafe.SizeOf<T>() + 4);
            Unsafe.Write((void*)(mem + 4), obj);

            lua_rawgeti(L, LUA_REGISTRYINDEX, classId);
            
            if (!lua_istable(L, -1))
            {
                Debug.LogWarning($"class not registered : {obj.GetType() }, id: {classId} obj: {obj}");
                lua_pop(L, 1);
                return;
            }
#if DEBUG || UNITY_EDITOR
            //luaL_checktype(L, -1, (int)LuaType.Table);
#endif
            lua_setmetatable(L, -2);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static unsafe void PushUnmanaged(lua_State L, object obj)
        {
            int classId = TypeID(obj);
            IntPtr mem = lua_newuserdata(L, (UIntPtr)Marshal.SizeOf(obj) + 4);
            Marshal.StructureToPtr(obj, mem + 4, false);

            lua_rawgeti(L, LUA_REGISTRYINDEX, classId);
           
            if (!lua_istable(L, -1))
            {
                Debug.LogWarning($"class not registered : {obj.GetType() }, id: {classId} obj: {obj}");
                lua_pop(L, 1);
                return;
            }

#if DEBUG || UNITY_EDITOR
            //luaL_checktype(L, -1, (int)LuaType.Table);
#endif
            lua_setmetatable(L, -2);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void PushValueType(lua_State L, object obj)
        {
            var converter = Converter.GetConverter(obj.GetType());
            if (converter != null)
            {
                converter.pusher(L, obj);
                return;
            }

            if (obj.GetType().IsUnManaged())
            {
                PushUnmanaged(L, obj);
            }
            else
            {
                PushObject(L, obj);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void PushValueType<T>(lua_State L, ref T obj)
        {
            var converter = Converter.GetConverter(obj.GetType());
            if (converter != null)
            {
                ((TConverter<T>)converter).Push(L, obj);
                return;
            }

            if (typeof(T).IsUnManaged())
            {
                PushUnmanaged(L, obj);
            }
            else
            {
                PushObject(L, obj);
            }
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public unsafe static void PushObject(lua_State L, object obj)
        {
            if (obj2id.TryGetValue(obj, out var key))
            {
                if (LunaNative.TryGetUserData(L, key, weakTableRef) == 1)
                {
                    return;
                }
            }

            int classId = TypeID(obj);

            IntPtr mem = lua_newuserdata(L, (UIntPtr)sizeof(long));
            int id = freeList.Alloc(obj);
            *(int*)mem = 0;
            *(int*)(mem + 4) = id;

            obj2id[obj] = (int)id;

            LunaNative.CacheUserData(L, id, weakTableRef);

            lua_rawgeti(L, LUA_REGISTRYINDEX, classId);

            if (!lua_istable(L, -1))
            {
                Debug.LogWarning($"class not registered : {obj.GetType() }, id: {classId} obj: {obj}");
                lua_pop(L, 1);
                return;
            }

            lua_setmetatable(L, -2);
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
        public unsafe static object GetUnmanaged(lua_State L, int index, Type t)
        {
            LuaType type = lua_type(L, index);
            if (type != LuaType.UserData)
            {
                return Converter.Convert(t, type, L, index);
            }

            var ptr = lua_touserdata(L, index);
            return Marshal.PtrToStructure(ptr + 4, t);
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
                return ref Unsafe.Unbox<T>(freeList[(int)handle]);
            }

        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public unsafe static object GetValue(lua_State L, int index, Type valueType)
        {
            if (valueType.IsUnManaged())
            {
                return GetUnmanaged(L, index, valueType);
            }
            else
            {
                var handle = GetHandler(L, index);
                return freeList[(int)handle];
            }

        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static object Get(lua_State L, int index, Type t)
        {
            var converter = Converter.GetConverter(t);
            if (converter != null)
            {
                return converter.getter(L, index);                
            }

            var handle = GetHandler(L, index);
            return freeList[(int)handle];
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T Get<T>(lua_State L, int index)
        {
            var converter = Converter.GetConverter(typeof(T));
            if (converter != null)
            {
                return ((TConverter<T>)converter).Get(L, index);
            }

            var handle = GetHandler(L, index);
            return (T)freeList[(int)handle];
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static unsafe int GetHandler(lua_State L, int index)
        {
            var ptr = lua_touserdata(L, index);
#if DEBUG
            if (ptr == IntPtr.Zero)
            {                
                return 0;
            }
#endif
            return *(int*)(ptr + 4);            
        }


        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void FreeStruct(lua_State L, int index)
        {
            var handle = GetHandler(L, index);
            var obj = freeList[(int)handle];
            obj2id.Remove(obj);
            freeList.Free((int)handle);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Free(lua_State L, int index)
        {
            var handle = GetHandler(L, index);
            var obj = freeList[handle];
            obj2id.Remove(obj);
            freeList.Free((int)handle);
        }


    }

}
