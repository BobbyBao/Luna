using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpLuna
{

    [AttributeUsage(AttributeTargets.Class)]
    public sealed class WrapClassAttribute : Attribute
    {
        public Type Type { get; }
        public string Name { get; }
        public WrapClassAttribute(Type type, string name = null)
        {
            Type = type;
            Name = name;
        }

    }

}
