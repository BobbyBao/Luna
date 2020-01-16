using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;

namespace SharpLuna
{
    public class FreeList<T>
    {
        public FastList<T> active;
        Stack<int> freeList = new Stack<int>();

        public FreeList(int size)
        {
            active = new FastList<T>(size);
            active.Add(default);
        }

        public ref T this[int index]
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get
            {
                return ref active[index];
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public int Alloc(T obj)
        {
            if (freeList.Count > 0)
            {
                int id = freeList.Pop();
                active[id] = obj;
                return id;
            }

            int count = active.Count;
            active.Add(obj);
            return count;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Free(int id)
        {
            active[id] = default;
            freeList.Push(id);
        }
    }

    public class WeakFreeList<T> where T : class
    {
        public FastList<WeakReference<T>> active;
        Stack<int> freeList = new Stack<int>();

        public WeakFreeList(int size)
        {
            active = new FastList<WeakReference<T>>(size);
            active.Add(default);
        }

        public T this[int index]
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get
            {
                if(active[index].TryGetTarget(out T ret))
                {
                    return ret;
                }
                return default;
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public int Alloc(T obj)
        {
            if (freeList.Count > 0)
            {
                int id = freeList.Pop();
                active[id].SetTarget(obj);
                return id;
            }

            int count = active.Count;
            active.Add(new WeakReference<T>(obj));
            return count;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Free(int id)
        {
            freeList.Push(id);
        }
    }
}
