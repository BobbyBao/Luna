using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace SharpLuna
{
    using static Lua;
    using lua_State = IntPtr;

    public unsafe class UnamanagedConverter<T> : TConverter<T>
    {
        public int metaRef = -1;
        public int newRef = -1;
        public int unpackRef = -1;

        protected NativeBuffer buffer;
        public int Size => buffer.size;

        public UnamanagedConverter()
        {
        }

        public UnamanagedConverter(IntPtr L, Type unmanagedType)
        {
            this.type = unmanagedType;

            getter = _Get;
            pusher = _Push;

            int size;
            StructElement[] structElements = unmanagedType.GetLayout(out size);
            buffer = new NativeBuffer(structElements);

            InitRef(L);
        }

        protected void InitRef(IntPtr L)
        {
            lua_getglobal(L, type.Name);

            lua_getfield(L, -1, "unpack");
            unpackRef = luaL_ref(L, LUA_REGISTRYINDEX);

            lua_getfield(L, -1, "pack");
            newRef = luaL_ref(L, LUA_REGISTRYINDEX);

            //luaL_getmetafield(L, -1, "__call");            
            //newRef = luaL_ref(L, LUA_REGISTRYINDEX);

            metaRef = luaL_ref(L, LUA_REGISTRYINDEX);
        }

        object _Get(IntPtr L, int index)
        {
            byte* ptr = stackalloc byte[buffer.size];
            if (unpackRef == -1)
                LunaNative.luna_getstruct(L, index, (IntPtr)ptr, buffer.Addr, buffer.Count);
            else
                LunaNative.luna_unpackstruct(L, index, unpackRef, (IntPtr)ptr, buffer.Addr, buffer.Count);
            object boxed = Marshal.PtrToStructure((IntPtr)ptr, type);
            return boxed;
        }

        void _Push(IntPtr L, object data)
        {
            byte* ptr = stackalloc byte[buffer.size];
            Marshal.StructureToPtr(data, (IntPtr)ptr, false);
            if (newRef == -1)
                LunaNative.luna_pushstruct(L, metaRef, (IntPtr)ptr, buffer.Addr, buffer.Count);
            else
                LunaNative.luna_packstruct(L, newRef, (IntPtr)ptr, buffer.Addr, buffer.Count);
        }

        public override T Get(IntPtr L, int index)
        {
            T data = default;
            if (unpackRef == -1)
                LunaNative.luna_getstruct(L, index, (IntPtr)Unsafe.AsPointer(ref data), buffer.Addr, buffer.Count);
            else
                LunaNative.luna_unpackstruct(L, index, unpackRef, (IntPtr)Unsafe.AsPointer(ref data), buffer.Addr, buffer.Count);
            return data;

        }

        public override void Push(IntPtr L, T data)
        {
            if (newRef == -1)
                LunaNative.luna_pushstruct(L, metaRef, (IntPtr)Unsafe.AsPointer(ref data), buffer.Addr, buffer.Count);
            else
                LunaNative.luna_packstruct(L, newRef, (IntPtr)Unsafe.AsPointer(ref data), buffer.Addr, buffer.Count);

        }

    }

    public unsafe abstract class ValueTypeConverter<T> : UnamanagedConverter<T>
    {
        enum State
        {
            Init, Reading, Writing
        }

        State state;

        public ValueTypeConverter(IntPtr L)
        {
            this.type = typeof(T);

            getter = _Get;
            pusher = _Push;

            InitRef(L);

            state = State.Init;
            T obj = default;
            BuildStruct(ref obj);
        }

        protected abstract void BuildStruct(ref T obj);

        protected ValueTypeConverter<T> Transfer<K>(string key, ref K v)
        {
            if (state == State.Init)
            {
                buffer.Add(key, ref v);
            }
            else if (state == State.Reading)
            {
                buffer.Read(key, ref v);
            }
            else if (state == State.Writing)
            {
                buffer.Write(key, ref v);
            }

            return this;
        }

        object _Get(IntPtr L, int index)
        {
            return Get(L, index);
        }

        void _Push(IntPtr L, object data)
        {
            Push(L, (T)data);
        }

        public override T Get(IntPtr L, int index)
        {
            byte* ptr = stackalloc byte[buffer.size];
            if (unpackRef == -1)
                LunaNative.luna_getstruct(L, index, (IntPtr)ptr, buffer.Addr, buffer.Count);
            else
                LunaNative.luna_unpackstruct(L, index, unpackRef, (IntPtr)ptr, buffer.Addr, buffer.Count);

            buffer.Init(ptr);
            state = State.Reading;
            T obj = default;
            BuildStruct(ref obj);
            return (T)obj;
        }

        public override void Push(IntPtr L, T data)
        {
            byte* ptr = stackalloc byte[buffer.size];
            buffer.Init(ptr);
            state = State.Writing;
            BuildStruct(ref data);

            if (newRef == -1)
                LunaNative.luna_pushstruct(L, metaRef, (IntPtr)ptr, buffer.Addr, buffer.Count);
            else
                LunaNative.luna_packstruct(L, newRef, (IntPtr)ptr, buffer.Addr, buffer.Count);
        }

    }
    /*
    public struct Test11
    {
        public int test1;
        public float test2;
    }

    public class TestConverter : ValueTypeConverter<Test11>
    {
        public TestConverter(IntPtr L) : base(L)
        {
        }

        protected override void BuildStruct(ref Test11 obj)
        {
            Transfer("test1", ref obj.test1);
            Transfer("test2", ref obj.test2);
        }
    }*/

}
