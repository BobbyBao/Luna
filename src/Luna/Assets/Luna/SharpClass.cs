using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;

namespace SharpLuna
{
    using lua_State = IntPtr;
    using static Lua;

    public partial class SharpClass : IDisposable
    {
        public string Name { get; set; }

        protected LuaRef meta;
        protected SharpClass parent;
        protected Type classType;

        protected Dictionary<string, MethodWraper> classInfo;

        static Dictionary<string, string> tagMethods = new Dictionary<string, string>
        {
            {"ToString","__tostring"},
            {"Dispose", "__close"},

            {"Equals","__eq"},
            {"op_Equality","__eq"},
            {"op_LessThan","__lt"},
            {"op_LessThanOrEqual","__le"},

            {"op_Addition","__add"},
            {"op_Subtraction","__sub"},
            {"op_Multiply","__mul"},
            {"op_Division","__div"},
            {"op_Modulus","__mod"},
            {"op_UnaryNegation","__unm"},

        };
        
        static bool IsTagMethod(string name, out string tag) => tagMethods.TryGetValue(name, out tag);
        public SharpClass(SharpClass parent)
        {
            this.parent = parent;
        }
        
        public SharpClass(SharpClass parent, LuaRef meta)
        {
            this.meta = meta;
            this.meta.CheckTable();
            this.parent = parent;
        }

        ~SharpClass()
        {
            meta.Dispose();
        }

        public void Dispose()
        {
            meta.Dispose();

            GC.SuppressFinalize(this);
        }

        public virtual Luna Luna
        {
            get { return parent.Luna;}
        }

        public lua_State State => meta.State;

        public LuaRef Meta => meta;

        public void SetClassType(Type type)
        {
            classType = type;
            classInfo = Luna.GetClassWrapper(classType);
        }
        
        public void SetGetter(string name, LuaRef getter)
        {
            if (name == "Item")
            {
                SetMemberFunction(___get_indexed, getter);
            }
            else
            {
                meta.RawGet(___getters).RawSet(name, getter);
            }
        }

        public void SetSetter(string name, LuaRef setter)
        {
            if(name == "Item")
            {
                SetMemberFunction(___set_indexed, setter);
            }
            else
            {
                meta.RawGet(___setters).RawSet(name, setter);
            }

        }

        public void SetReadOnly(string name)
        {
            LuaRef meta_class = meta;
            string full_name = GetMemberName(meta_class, name);
            LuaRef err = LuaRef.CreateFunctionWith(State, ErrorReadOnly, full_name);

            meta_class.RawGet(___setters).RawSet(name, err);
        }

        public void SetMemberFunction(IntPtr name, LuaRef proc)
        {
            meta.RawSet(name, proc);
        }

        public void OnRegClass()
        {
            var fields = classType.GetFields();
            foreach (var field in fields)
            {
                if (!field.IsPublic)
                    continue;
   
                if (field.IsLiteral)
                    RegConstant(field);                   
                else
                    RegField(field);
            }

            if (classInfo.TryGetValue("ctor", out var methodConfig))
            {
                meta.RawSet("__call", LuaRef.CreateFunction(State, methodConfig.func));                
            }
            else
            {
                var ctors = classType.GetConstructors();
                List<MethodBase> memberInfo = new List<MethodBase>();
                foreach (var constructorInfo in ctors)
                {
                    if (!constructorInfo.IsPublic)
                        continue;

                    if (!constructorInfo.ShouldExport())
                        continue;

                    var paramInfos = constructorInfo.GetParameters();
                    foreach (var info in paramInfos)
                    {
                        if (!info.ParameterType.ShouldExport())
                            continue;
                    }

                    memberInfo.Add(constructorInfo);
                }

                if(memberInfo.Count > 0)
                    RegMethod("__call", memberInfo.ToArray());
            }

            var props = classType.GetProperties(BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static | BindingFlags.DeclaredOnly);
            foreach (var p in props)
            {
                if (!p.ShouldExport())
                    continue;
                  
                RegProperty(p);
            }

            var methods = classType.GetMethods(BindingFlags.Public | BindingFlags.Static | BindingFlags.Instance | BindingFlags.DeclaredOnly);
            HashSet<string> registered = new HashSet<string>();
            foreach (var m in methods)
            {
                if (registered.Contains(m.Name))
                {
                    continue;
                }

                bool isAsync = m.GetCustomAttribute<LuaAsyncAttribute>() != null;

                if (classInfo.TryGetValue(m.Name, out methodConfig))
                {
                    if (methodConfig.func != null)
                    {
                        var fn = LuaRef.CreateFunction(State, methodConfig.func);
                        if (IsTagMethod(m.Name, out var tag))
                        {
                            meta.RawSet(tag, fn);
                        }
                        else
                        {
                            meta.RawSet(m.Name, fn);
                        }

                        if (isAsync)
                        {
                            LuaRef r = new LuaRef(State, "coroutine.__async");
                            fn = r.Call<LuaRef>(fn);

                            meta.RawSet("_async_" + m.Name, fn);

                        }

                        if (classType.IsArray)
                        {
                            if(m.Name == "Get")
                            {
                                SetMemberFunction(___get_indexed, fn);
                            }

                            if (m.Name == "Set")
                            {
                                SetMemberFunction(___set_indexed, fn);
                            }
                        }

                    }

                    registered.Add(m.Name);
                    continue;
                }

                if (!m.ShouldExport())
                {
                    continue;
                }

                var memberInfo = classType.GetMember(m.Name).Cast<MethodBase>().ToArray();               
                registered.Add(m.Name);
                if (memberInfo.Length > 0)
                {
                    RegMethod(m.Name, memberInfo, isAsync);

                }
            }
        }
        
        public SharpClass RegConstant(FieldInfo field)
        {
            var v = field.GetValue(null);
            if(field.FieldType.IsEnum)
            {
                LuaRef r = LuaRef.FromValue(State, (int)v);
                SetGetter(field.Name, r);
            }
            else
            {
                LuaRef r = LuaRef.FromValue(State, v);
                SetGetter(field.Name, r);
            }

            SetReadOnly(field.Name);
            return this;
        }

        public SharpClass RegField(FieldInfo fieldInfo)
        {
            if(classInfo.TryGetValue(fieldInfo.Name, out var methodConfig))
            {
                if (methodConfig.getter != null)
                {
                    var getter = LuaRef.CreateFunction(State, methodConfig.getter);
                    SetGetter(fieldInfo.Name, getter);
                }

                if (methodConfig.setter != null)
                {
                    var setter = LuaRef.CreateFunction(State, methodConfig.setter);
                    SetSetter(fieldInfo.Name, setter);
                }
                else
                {
                    SetReadOnly(fieldInfo.Name);
                }

                return this;
            }
          
            //Luna.LogWarning("注册反射版的field : " + fieldInfo.Name);
            if (fieldInfo.IsStatic)
            {
                var getter = LuaRef.CreateFunction(State, Field.StaticGetter, fieldInfo);
                SetGetter(fieldInfo.Name, getter);
                var setter = LuaRef.CreateFunction(State, Field.StaticSetter, fieldInfo);
                SetSetter(fieldInfo.Name, setter);
            }
            else
            {
                var getter = LuaRef.CreateFunction(State, Field.Getter, fieldInfo);
                SetGetter(fieldInfo.Name, getter);
                var setter = LuaRef.CreateFunction(State, Field.Setter, fieldInfo);
                SetSetter(fieldInfo.Name, setter);
            }

            return this;
        }
        
        public SharpClass RegProperty(PropertyInfo propertyInfo, bool isIndexer = false)
        {
            if (classInfo.TryGetValue(propertyInfo.Name, out var methodConfig))
            {
                if (methodConfig.getter != null)
                {
                    var getter = LuaRef.CreateFunction(State, methodConfig.getter);
                    SetGetter(propertyInfo.Name, getter);
                }

                if (methodConfig.setter != null)
                {
                    var setter = LuaRef.CreateFunction(State, methodConfig.setter);
                    SetSetter(propertyInfo.Name, setter);
                }
                else
                {
                    SetReadOnly(propertyInfo.Name);
                }

                return this;
            }
            
            if (propertyInfo.CanRead)
            {
                MethodInfo methodInfo = propertyInfo.GetGetMethod(false);
                if (methodInfo != null)
                {
#if true//IL2CPP
                    var fn = methodInfo.IsStatic ? (LuaNativeFunction)Property.StaticGetter : Property.Getter;
                    var getter = LuaRef.CreateFunction(State, fn, propertyInfo);
                    SetGetter(propertyInfo.Name, getter);
#else
                    var luaFun = RegMethod(methodInfo, true);
                    if (luaFun)
                    {
                        SetGetter(propertyInfo.Name, luaFun);
                    }
#endif
                }
            }

            if (propertyInfo.CanWrite)
            {
                MethodInfo methodInfo = propertyInfo.GetSetMethod(false);
                if (methodInfo != null)
                {
#if true//IL2CPP
                    var fn = methodInfo.IsStatic ? (LuaNativeFunction)Property.StaticSetter : Property.Setter;
                    var setter = LuaRef.CreateFunction(State, fn, propertyInfo);
                    SetSetter(propertyInfo.Name, setter);
#else
                    var luaFun = RegMethod(methodInfo, true);
                    if (luaFun)
                    {
                        SetSetter(propertyInfo.Name, luaFun);
                    }
#endif
                }
            }
            else
            {
                SetReadOnly(propertyInfo.Name);
            }

            return this;
        }

        public LuaRef RegMethod(MethodInfo methodInfo, bool isProp)
        {
#if LUNA_SCRIPT
            string callFnName = (methodInfo.IsStatic && !isProp) ? "StaticCall" : "Call";
#else
            string callFnName = "Call";
#endif
            if (DelegateCache.GetMethodDelegate(classType, methodInfo, callFnName, out LuaNativeFunction luaFunc, out Delegate del))
            {
                return LuaRef.CreateFunction(State, luaFunc, del);
            }

            return null;
        }


        public SharpClass RegMethod(string name, MethodBase[] methodInfo, bool isAsync = false)
        {             
            MethodReflection method = new MethodReflection(methodInfo);
#if !IL2CPP
            for(int i = 0; i < methodInfo.Length; i++)
            {
                var mi = methodInfo[i] as MethodInfo;
                if(mi != null)
                {
                    if(DelegateCache.GetMethodDelegate(classType, mi, "CallDel", out CallDel fn, out Delegate del))
                    {
                        method.luaFunc[i] = fn;
                        method.del[i] = del;
                    }
                }
             
            }
#endif
            //Luna.Log("反射方式实现");
            LuaRef luaFun = LuaRef.CreateFunction(State, MethodReflection.Call, method);
            if (IsTagMethod(name, out var tag))
            {
                meta.RawSet(tag, luaFun);
            }
            else
            {
                meta.RawSet(name, luaFun);
            }

            if (classType.IsArray)
            {
                if (name == "Get")
                {
                    SetMemberFunction(___get_indexed, luaFun);
                }

                if (name == "Set")
                {
                    SetMemberFunction(___set_indexed, luaFun);
                }
            }

            if (isAsync)
            {
                LuaRef r = new LuaRef(State, "coroutine.__async");
                luaFun = r.Call<LuaRef>(luaFun);

                meta.RawSet("_async_" + name, luaFun);

            }

            return this;
        }

    }

}
