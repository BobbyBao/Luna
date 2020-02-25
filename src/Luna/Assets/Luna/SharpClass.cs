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
        public string Name { get; protected set; }

        protected LuaRef meta;
        protected SharpClass parent;
        protected Type classType;

        protected Dictionary<string, MethodWraper> classInfo;

        static Dictionary<Type, SharpClass> registeredClass = new Dictionary<Type, SharpClass>();
        static Dictionary<Type, string> classAlias = new Dictionary<Type, string>();

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

        protected SharpClass(LuaRef meta)
        {
            this.meta = meta;
            this.meta.CheckTable();
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

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsRegistered<T>()
        {
            return IsRegistered(typeof(T));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsRegistered(Type t)
        {
            return registeredClass.ContainsKey(t);
        }

        public static void SetAlias(Type t, string alias)
        {
            classAlias.Add(t, alias);
        }

        public static string GetTableName(Type t)
        {
            if (classAlias.TryGetValue(t, out string alias))
            {
                return alias;
            }

            return t.Name;
        }

        [AOT.MonoPInvokeCallback(typeof(LuaNativeFunction))]
        static int Destructor(lua_State L)
        {
            SharpObject.Free(L, 1);
            return 0;
        }
       
        public static SharpClass Extend(Type classType, Type superType, SharpClass module)
        {
            if (registeredClass.TryGetValue(classType, out var bindClass))
            {
                return bindClass;
            }

            string name = GetTableName(classType);

            LuaNativeFunction dctor = null;
            if(!classType.IsUnManaged() 
                || IsRegistered(classType) //未注册Wrap，用反射版api， UnmanagedType同Object
                )
            {
                dctor = Destructor;
            }

            LuaRef meta = CreateClass(module.Meta, name, SharpObject.TypeID(classType), SharpObject.TypeID(superType), dctor);           
            bindClass = new SharpClass(meta);
            bindClass.parent = module;
            bindClass.Name = name;
            bindClass.SetClassType(classType);
            registeredClass.Add(classType, bindClass);
            return bindClass;
        }

        protected void SetClassType(Type type)
        {
            classType = type;
            classInfo = Luna.GetClassWrapper(classType);
        }
        
        public SharpClass GetClass(Type classType, Type superClass = null)
        {
            if (classType == superClass)
            {
                return SharpClass.Extend(classType, null, this);
            }

            var parentType = classType.BaseType;
 

            while (superClass == null)
            {           
                if(parentType == null)
                {
                    break;
                }

                if (SharpClass.IsRegistered(parentType))
                {
                    superClass = parentType;
                    break;
                }

                if (parentType == typeof(object))
                {
                    break;
                }

                parentType = parentType.BaseType;
            }

            return SharpClass.Extend(classType, superClass, this);
        }
        
        public void SetGetter(string name, LuaRef getter)
        {
            meta.RawGet(___getters).RawSet(name, getter);
        }

        public void SetSetter(string name, LuaRef setter)
        {
            LuaRef meta_class = meta;
            meta_class.RawGet(___setters).RawSet(name, setter);
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
                {
                    continue;
                }

                if (field.IsLiteral)
                {
                    RegConstant(field);                   
                }
                else
                {
                    RegField(field);
                }

            }

            bool wrapCtor = false;

            {
                if (classInfo.TryGetValue("ctor", out var methodConfig))
                {
                    if (methodConfig.func != null)
                    {
                        meta.RawSet("__call", LuaRef.CreateFunction(State, methodConfig.func));
                        wrapCtor = true;
                    }

                }
            }     

            if (!wrapCtor)
            {
                var ctors = classType.GetConstructors();
                List<MethodBase> memberInfo = new List<MethodBase>();
                foreach (var constructorInfo in ctors)
                {
                    if (!constructorInfo.IsPublic)
                    {
                        continue;
                    }

                    if (!constructorInfo.ShouldExport())
                    {
                        continue;
                    }

                    var paramInfos = constructorInfo.GetParameters();
                    foreach (var info in paramInfos)
                    {
                        if (!info.ParameterType.ShouldExport())
                        {
                            continue;
                        }
                    }

                    memberInfo.Add(constructorInfo);
                }

                if(memberInfo.Count > 0)
                    RegMethod("__call", memberInfo.ToArray());
            }

            var props = classType.GetProperties();
            foreach (var p in props)
            {
                if (!p.ShouldExport())
                {
                    continue;
                }

                if (p.Name == "Item")
                {
                    RegIndexer(p);
                }
                else
                {
                    RegProperty(p);
                }
            }

            var methods = classType.GetMethods(BindingFlags.Public | BindingFlags.Static | BindingFlags.Instance | BindingFlags.DeclaredOnly);
            HashSet<string> registered = new HashSet<string>();
            foreach (var m in methods)
            {
                if (registered.Contains(m.Name))
                {
                    continue;
                }

                if (classInfo.TryGetValue(m.Name, out var methodConfig))
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
                    RegMethod(m.Name, memberInfo);
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
        
        public SharpClass RegProperty(PropertyInfo propertyInfo)
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
            /*
            if (propertyInfo.CanRead)
            {
                MethodInfo methodInfo = propertyInfo.GetGetMethod(false);
                if (methodInfo != null)
                {
                    var luaFun = RegMethod(methodInfo, true); 
                    if (luaFun)
                    {
                        SetGetter(propertyInfo.Name, luaFun);
                    }
                }
            }

            if (propertyInfo.CanWrite)
            {
                MethodInfo methodInfo = propertyInfo.GetSetMethod(false);
                if (methodInfo != null)
                {
                    var luaFun = RegMethod(methodInfo, true);
                    if (luaFun)
                    {
                        SetSetter(propertyInfo.Name, luaFun);
                    }
                }
            }
            else
            {
                SetReadOnly(propertyInfo.Name);
            }
            */
            //todo: Reflection

            return this;
        }

        public SharpClass RegIndexer(PropertyInfo propertyInfo)
        {
            if (classInfo.TryGetValue(propertyInfo.Name, out var methodConfig))
            {
                if (methodConfig.getter != null)
                {
                    var luaFun = LuaRef.CreateFunction(State, methodConfig.getter);
                    if (luaFun)
                    {
                        SetMemberFunction(___get_indexed, luaFun);
                    }
                }

                if (methodConfig.setter != null)
                {
                    var luaFun = LuaRef.CreateFunction(State, methodConfig.setter);
                    if (luaFun)
                    {
                        SetMemberFunction(___set_indexed, luaFun);
                    }
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
                    var luaFun = RegMethod(methodInfo, true);
                    if(luaFun)
                    {
                        SetMemberFunction(___get_indexed, luaFun);
                    }
                    
                }
            }

            if (propertyInfo.CanWrite)
            {
                MethodInfo methodInfo = propertyInfo.GetSetMethod(false);
                if (methodInfo != null)
                {
                    var luaFun = RegMethod(methodInfo, true);
                    if (luaFun)
                    {
                        SetMemberFunction(___set_indexed, luaFun);
                    }
                }
            }
            else
            {
                SetReadOnly(propertyInfo.Name);
            }

            return this;
        }


        public SharpClass RegMethod(string name, MethodBase[] methodInfo)
        {             
            MethodWrap method = new MethodWrap(methodInfo);

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
            LuaRef luaFun = LuaRef.CreateFunction(State, MethodWrap.Call, method);
            if (IsTagMethod(name, out var tag))
            {
                meta.RawSet(tag, luaFun);
            }
            else
            {
                meta.RawSet(name, luaFun);
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


    }

}
