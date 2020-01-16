using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;


namespace SharpLuna
{
    [DebuggerDisplay("Count = {Count}")]
    public class FastList<T>
    {
        protected T[] items;
        protected int size;

        private const int defaultCapacity = 4;
        public static readonly T[] Empty = new T[0];

        public FastList()
        {
            items = Empty;
        }

        public FastList(int capacity)
        {
            items = new T[capacity];
        }

        public ref T this[int index]
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get
            {
                return ref items[index];
            }
        }
        public int Count => size;
        public int Capacity => items.Length;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Add(T item)
        {
            if (size == items.Length)
            {
                EnsureCapacity(size + 1);
            }

            items[size++] = item;
        }

        public void Resize(int sz)
        {
            EnsureCapacity(sz);
            size = sz;
        }

        public void Clear()
        {
            Clear(false);
        }

        public bool Contains(T item)
        {
            if (item == null)
            {
                for (int j = 0; j < size; j++)
                {
                    if (items[j] == null)
                    {
                        return true;
                    }
                }
                return false;
            }

            EqualityComparer<T> comparer = EqualityComparer<T>.Default;
            for (int i = 0; i < size; i++)
            {
                if (comparer.Equals(items[i], item))
                {
                    return true;
                }
            }

            return false;
        }

        public void CopyTo(T[] array, int arrayIndex)
        {
            Array.Copy(items, 0, array, arrayIndex, size);
        }

        public int IndexOf(T item)
        {
            return Array.IndexOf(items, item, 0, size);
        }

        public void Insert(int index, T item)
        {
            if (size == items.Length)
            {
                EnsureCapacity(size + 1);
            }
            if (index < size)
            {
                Array.Copy(items, index, items, index + 1, size - index);
            }
            items[index] = item;
            size++;
        }

        public bool Remove(T item)
        {
            int index = IndexOf(item);
            if (index >= 0)
            {
                RemoveAt(index);
                return true;
            }
            return false;
        }

        public bool FastRemove(T item)
        {
            int index = IndexOf(item);
            if(index >= 0)
            {
                FastRemove(index);
                return true;
            }
            return false;
        }

        public void RemoveAt(int index)
        {
            if (index < 0 || index >= size) throw new ArgumentOutOfRangeException(nameof(index));
            size--;
            if (index < size)
            {
                Array.Copy(items, index + 1, items, index, size - index);
            }
            items[size] = default(T);
        }

        public void FastRemove(int index)
        {
            if(index < 0 || index >= size) throw new ArgumentOutOfRangeException(nameof(index));
            size--;
            
            if(index < size)
            {
                items[index] = items[size];
            }
            items[size] = default(T);
        }

        public void Clear(bool fastClear)
        {
            if (!fastClear && size > 0)
            {
                Array.Clear(items, 0, size);
            }
            size = 0;
        }

        public void EnsureCapacity(int min)
        {
            if (items.Length < min)
            {
                int num = (items.Length == 0) ? defaultCapacity : (items.Length*2);
                if (num < min)
                {
                    num = min;
                }

                if (num != items.Length)
                {
                    if (num > 0)
                    {
                        var destinationArray = new T[num];
                        if (size > 0)
                        {
                            Array.Copy(items, 0, destinationArray, 0, size);
                        }
                        items = destinationArray;
                    }
                    else
                    {
                        items = Empty;
                    }
                }
            }
        }


    }
}
