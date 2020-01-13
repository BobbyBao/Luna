using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

#if UNITY_IOS || UNITY_2018_1_OR_NEWER

#else

namespace AOT
{
    [AttributeUsage(AttributeTargets.Method)]
    public class MonoPInvokeCallbackAttribute : Attribute
    {
        public Type Type { get; }
        public MonoPInvokeCallbackAttribute(Type type)
        {
            Type = type;
        }
    }
}

#endif
