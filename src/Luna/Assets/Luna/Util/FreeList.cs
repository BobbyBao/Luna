using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;

namespace SharpLuna
{
    public class FreeList<T> : List<T>
    {
        Stack<int> freeList = new Stack<int>();

        public FreeList(int size) : base(size)
        {
            Add(default);
        }

        public int Alloc(T obj)
        {
            if (freeList.Count > 0)
            {
                int id = freeList.Pop();
                this[id] = obj;
                return id;
            }

            int count = Count;
            Add(obj);
            return count;
        }

        public void Free(int id)
        {
            this[id] = default;
            freeList.Push(id);
        }
    }

}
