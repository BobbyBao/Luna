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

            try
            {
                del = Delegate.CreateDelegate(type, methodInfo);
            }
            catch (System.Exception ex)
            {
                Luna.LogError(ex.Message);
                Luna.LogError(type.ToString());
                Luna.LogError(methodInfo.ReflectedType.ToString() + ", " + methodInfo.ToString());
                return null;
            }
            cache.Add(methodInfo, del);
            return del;
        }

        public static Delegate Get(Type type, Type targetType, MethodInfo methodInfo)
        {
            if (cache.TryGetValue(methodInfo, out var del))
            {
                return del;
            }

            try
            {
                del = Delegate.CreateDelegate(type, methodInfo);
            }
            catch (System.Exception ex)
            {
                Luna.LogError(ex.Message);
                Luna.LogError(type.ToString());
                Luna.LogError(methodInfo.ReflectedType.ToString() + ", " + methodInfo.ToString());
                return null;
            }
            cache.Add(methodInfo, del);
            return del;
        }
        
    }

}
