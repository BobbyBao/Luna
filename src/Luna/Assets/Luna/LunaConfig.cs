using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpLuna
{

    public class LunaConfig
    {
        Dictionary<Type, ClassWraper> classWrapers = new Dictionary<Type, ClassWraper>();

        public ClassWraper GetClassWrapper(Type type)
        {
            if(classWrapers.TryGetValue(type, out var classWraper))
            {
                return classWraper;
            }

            classWraper = new ClassWraper();
            classWrapers.Add(type, classWraper);
            return classWraper;
        }

        public bool IsRegistered(Type type)
        {
            return classWrapers.ContainsKey(type);
        }
    }

    public class MethodWraper
    {
        public LuaNativeFunction func;
        public LuaNativeFunction getter;
        public LuaNativeFunction setter;
    }

    public class ClassWraper : Dictionary<string, MethodWraper>
    {
        public void RegField(string name, LuaNativeFunction getter, LuaNativeFunction setter) => RegProp(name, getter, setter);

        public void RegProp(string name, LuaNativeFunction getter, LuaNativeFunction setter)
        {
            if (!TryGetValue(name, out var methodWraper))
            {
                methodWraper = new MethodWraper();
                Add(name, methodWraper);
            }

            methodWraper.getter = getter;
            methodWraper.setter = setter;
        }

        public void RegConstructor(LuaNativeFunction func) => RegFunction("ctor", func);

        public void RegFunction(string name, LuaNativeFunction func)
        {
            if (!TryGetValue(name, out var methodWraper))
            {
                methodWraper = new MethodWraper();
                Add(name, methodWraper);
            }

            methodWraper.func = func;
        }
    }

}
