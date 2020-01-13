using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpLuna
{

    public class MethodConfig
    {
        public LuaNativeFunction func;
        public LuaNativeFunction getter;
        public LuaNativeFunction setter;
    }

    public class LunaConfig
    {
        public Dictionary<Type, Dictionary<string, MethodConfig>> wrapClassRegistry = new Dictionary<Type, Dictionary<string, MethodConfig>>();

        public Dictionary<string, MethodConfig> GetClassConfig(Type type)
        {
            if(wrapClassRegistry.TryGetValue(type, out var methodConfig))
            {
                return methodConfig;
            }

            methodConfig = new Dictionary<string, MethodConfig>();
            wrapClassRegistry.Add(type, methodConfig);
            return methodConfig;
        }

        public bool IsRegistered(Type type)
        {
            return wrapClassRegistry.ContainsKey(type);
        }
    }
}
