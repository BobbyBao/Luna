﻿using System;
using System.Collections.Generic;
using System.Text;

namespace SharpLuna
{
    /// <summary>
    /// 智能引用计数，主要用于避免struct类型的资源泄露
    /// </summary>
    public interface IRefCount : IDisposable
    {
        uint Handle { get; set; }
        void InternalRelease();
    }

    public unsafe static class RefCountHelper
    {
        struct RefInfo
        {
            public IRefCount refCount;
            public ushort gen;
            public ushort refs;
        }

        static FastList<RefInfo> refInfos = new FastList<RefInfo>();
        static List<ushort> freeList = new List<ushort>();
        static int current = 0;
        public static int LiveCount => refInfos.Count - freeList.Count;
        public static int FreeCount => freeList.Count;

        /// <summary>
        /// struct构造的时候调用
        /// </summary>
        /// <param name="self"></param>
        /// <returns></returns>
        public static uint Alloc(this IRefCount self)
        {
            if (freeList.Count > 0)
            {
                ushort idx;
                idx = freeList[freeList.Count - 1];
                freeList.RemoveAt(freeList.Count - 1);
                ref var info = ref refInfos[idx];
                info.refCount = self;
                info.refs = 1;
                info.refCount.Handle = (uint)((info.gen << 16) | idx);
                return info.refCount.Handle;
            }
            else
            {
                int idx = refInfos.Count;
                if (idx >= ushort.MaxValue)
                {
                    return 0;
                }

                self.Handle = (uint)idx;
                refInfos.Add(new RefInfo { refCount = self, refs = 1 } );
                return (uint)idx;
            }
            
        }

        /// <summary>
        /// 用于主动持有资源的引用计数
        /// </summary>
        /// <param name="self"></param>
        /// <returns></returns>
        public static int AddRef(this IRefCount self)
        {
            ushort gen = (ushort)(self.Handle >> 16);
            ushort idx = (ushort)(self.Handle & 0xffff);
            ref var info = ref refInfos[idx];
            if(info.gen != gen)
            {
                return 0;
            }

            return ++info.refs;
        }

        public static void Release(this IRefCount self)
        {
            ushort gen = (ushort)(self.Handle >> 16);
            ushort idx = (ushort)(self.Handle & 0xffff);
            ref var info = ref refInfos[idx];
            if (info.gen != gen)
            {
                return;
            }

            if(info.refs > 0)
            {
                if (--info.refs == 0)
                {
                    self.InternalRelease();
                    info.gen++;
                    freeList.Add(idx);
                }
            }
         
        }

        /// <summary>
        /// 忘记Release的资源（比如局部变量，没有进行Dispose），定期进行清理
        /// </summary>
        public static void Collect()
        {
            for (int i = 0; i < refInfos.Count; i++)
            {
                ref var info = ref refInfos[i];
                if (info.refs == 1)
                {
                    info.refCount.Release();
                }
            }
        }

        public static void CollectFrame(int count = 8)
        {
            for (int i = 0; i < count; i++)
            {
                ref var info = ref refInfos[current++ % refInfos.Count];
                if (info.refs == 1)
                {
                    info.refCount.Release();
                }
            }
        }

        /// <summary>
        /// 程序结束进行统一清理
        /// </summary>
        public static void Clear()
        {
            for (int i = 0; i < refInfos.Count; i++)
            {
                ref var info = ref refInfos[i];
                if (info.refs > 0)
                {
                    info.refCount.InternalRelease();
                }
            }

            refInfos.Clear();
            freeList.Clear();
        }

    }
}
