using System;
using System.Collections.Generic;
using System.Text;

namespace SharpLuna
{
    public static class BindHelper
    {
        static Stack<SharpClass> stack = new Stack<SharpClass>();
        static GlobalModule global;
        public static GlobalModule Begin(GlobalModule g)
        {
            global = g;
            stack.Clear();
            stack.Push(g);
            return g;
        }

        public static void End()
        {
            stack.Clear();
        }

        public static SharpClass Current => stack.Count > 0 ? stack.Peek() : null;
        public static SharpClass Pop() { return stack.Count > 0 ? stack.Pop() : null; }
        
        public static SharpClass Class<T>()
        {
            var cls = Current.BeginClass<T>();
            stack.Push(cls);
            return cls;
        }

        public static SharpModule Module(string name)
        {
            var m = global.BeginModule(name);
            stack.Push(m);
            return m;
        }

        public static SharpClass Constant<V>(string name, V v) => Current.AddConstant<V>(name, v);
        public static SharpClass Enum<V>() => Current.AddEnum<V>();
        public static SharpClass Variable<V>(string name, Func<V> getter, Action<V> setter = null) => Current.AddVar<V>(name, getter, setter);
        public static SharpClass Variable<T, V>(string name, Func<T, V> getter, Action<T, V> setter = null) => Current.AddVar<T, V>(name, getter, setter);
        public static SharpClass Property<T, V>(string name) => Current.AddProperty<T, V>(name);

        public static SharpClass Constructor<T>() where T : new() => Current.AddConstructor<T>();

        public static SharpClass Method<T>(string name) => Current.AddMethod<T>(name);
        public static SharpClass Method<T, P1>(string name) => Current.AddMethod<T, P1>(name);
        public static SharpClass Method<T, P1, P2>(string name) => Current.AddMethod<T, P1, P2>(name);

        public static SharpClass MethodR<T, R>(string name) => Current.AddMethodR<T, R>(name);
        public static SharpClass MethodR<T, P1, R>(string name) => Current.AddMethodR<T, P1, R>(name);
        public static SharpClass MethodR<T, P1, P2, R>(string name) => Current.AddMethodR<T, P1, P2, R>(name);

    }
}
