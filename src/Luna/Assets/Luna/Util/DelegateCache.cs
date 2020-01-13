using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace SharpLuna
{
    using static Lua;
    internal struct DelegateCache
    {
        static Dictionary<MethodInfo, Delegate> cache = new Dictionary<MethodInfo, Delegate>();
        public static Delegate Get<T>(MethodInfo methodInfo)
        {
            return Get(typeof(T), methodInfo);
        }

        public static Delegate Get(Type type, MethodInfo methodInfo)
        {
            if (cache.TryGetValue(methodInfo, out var del))
            {
                return del;
            }

            del = Delegate.CreateDelegate(type, methodInfo);
            cache.Add(methodInfo, del);
            return del;
        }

        public static Delegate Get(Type type, Type targetType, MethodInfo methodInfo)
        {
            if (cache.TryGetValue(methodInfo, out var del))
            {
                return del;
            }

            del = Delegate.CreateDelegate(type, targetType, methodInfo);
            cache.Add(methodInfo, del);
            return del;
        }
        
    }

}
