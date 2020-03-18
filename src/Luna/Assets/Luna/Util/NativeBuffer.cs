using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;

namespace SharpLuna
{
    public unsafe struct NativeBuffer
    {
        public StructElement[] layout;
        int count;
        public int size;
        int pos;
        public byte* data;

        public NativeBuffer(StructElement[] layout)
        {
            this.layout = new StructElement[layout.Length];
            Array.Copy(layout, 0, this.layout, 0, layout.Length);
           
            this.size = 0;
            count = 0;
            foreach (var e in layout)
            {
                count++;
                this.size += e.size;
            }
            pos = 0;
            data = null;
        }

        public StructElement* Addr => (StructElement*)Unsafe.AsPointer(ref layout[0]);
        public int Count => count;

        public void Init(byte* data)
        {
            this.data = data;
            this.pos = 0;
        }

        public void Add<T>(string key, ref T v)
        {
            var e = new StructElement
            {
                name = Marshal.StringToHGlobalAnsi(key),
                type = TypeExtensions.GetTypeCode(typeof(T)),
                offset = (short)size,
                size = (short)TypeExtensions.GetSize(typeof(T)),
            };

            Add(e);
        }

        public void Read<T>(string key, ref T v)
        {
            Debug.Assert(pos < count);
            ref var e = ref layout[pos++];
            if(typeof(T) == typeof(string))
            {
                int len = *(int*)(data + e.offset);
                byte* str = *(byte**)(data + e.offset + 4);
                v = (T)(object)Marshal.PtrToStringAnsi((IntPtr)str, len);
            }
            else
            {
                v = Unsafe.Read<T>(data + e.offset);
            }         
        }

        public void Write<T>(string key, ref T v)
        {
            Debug.Assert(pos < count);
            ref var e = ref layout[pos++];
            if (v is string str)
            {
                var p = Marshal.StringToHGlobalAnsi(str);
                Unsafe.Write(data + e.offset + 4, p);
                int len = 0;
                while (*(byte*)p != 0) { len++; p += 1; }
                Unsafe.Write(data + e.offset, len);
            }
            else
            {
                Unsafe.Write(data + e.offset, v);
            }
        }

        public void Add(in StructElement e)
        {
            if (this.layout == null)
                this.layout = new StructElement[4];

            if (count == this.layout.Length)
            {
                Array.Resize(ref layout, this.layout.Length + 4);
            }

            this.layout[count++] = e;
            size += e.size;
        }


    }

}
