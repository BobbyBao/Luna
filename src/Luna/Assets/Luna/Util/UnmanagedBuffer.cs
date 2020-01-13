using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;

namespace SharpLuna
{
    public unsafe class UnmanagedBuffer
    {
        byte* buffer = null;
        int size = 0;
        int cap = 0;


        public UnmanagedBuffer(int cap = 1024)
        {
            this.cap = cap;
            buffer = Alloc(cap);
        }

        ~UnmanagedBuffer()
        {
            Free(buffer);
        }

        public void Write<V>(V v) where V : unmanaged
        {
            EnsureCapacity(size + sizeof(V));
            Unsafe.Write(buffer, v);
            size += sizeof(V);
        }

        //TODO: POOL
        public static byte* Alloc(int size)
        {
            return (byte*)Marshal.AllocHGlobal(size);
        }

        //TODO: POOL
        public static void Free(byte* buf)
        {
            Marshal.FreeHGlobal((IntPtr)buf);
        }

        void EnsureCapacity(int min)
        {
            if (cap < min)
            {
                int num = (cap * 2);
                if (num < min)
                {
                    num = min;
                }

                if (num != cap)
                {
                    if (num > 0)
                    {
                        var destinationArray = Alloc(num);
                        if (size > 0)
                        {
                            Unsafe.CopyBlock(destinationArray, buffer, (uint)size);
                        }

                        Free(buffer);
                        buffer = destinationArray;
                    }
                    else
                    {
                        buffer = null;
                    }
                }
            }
        }
    }
}
