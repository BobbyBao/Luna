using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;

namespace SharpLuna
{
    public class FreeList<T> : FastList<T>
    {
        Stack<int> freeList = new Stack<int>();

        public FreeList(int size) : base(size)
        {
            Add(default);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public int Alloc(T obj)
        {
            if (freeList.Count > 0)
            {
                int id = freeList.Pop();
                items[id] = obj;
                return id;
            }

            int count = Count;
            Add(obj);
            return count;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Free(int id)
        {
            items[id] = default;
            freeList.Push(id);
        }
    }

}
