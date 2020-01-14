//#define METATABLE_EXTEND

using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace SharpLuna
{
    using static Lua;

    public partial class SharpClass : IDisposable
    {
        protected LuaRef m_meta;
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

        protected SharpClass(LuaRef meta)
        {
            m_meta = meta;
            m_meta.AddRef();
            m_meta.CheckTable();
        }

        ~SharpClass()
        {
            m_meta.Release();
        }

        public void Dispose()
        {
            m_meta.Release();

            GC.SuppressFinalize(this);
        }

        public virtual Luna Luna
        {
            get {return parent.Luna;}
        }

        public LuaState State => m_meta.State;

        public LuaRef Meta => m_meta;

        protected void SetClassType(Type type)
        {
            classType = type;

            classInfo = Luna.GetClassWrapper(classType);

        }

        public SharpClass this[params object[] obj]
        {
            get
            {
                if (BindHelper.Current == this)
                {
                    BindHelper.Pop();
                }
                return this;
            }
        }


        public SharpClass BeginRoot<SUB>()
        {
            return SharpClass.Bind<SUB>(this);
        }

        public SharpClass BeginClass<SUB>()
        {
            if (typeof(SUB) == typeof(object))
            {
                return SharpClass.Bind<SUB>(this);
            }

            return SharpClass.Extend<SUB, object>(this);
        }

        public SharpClass BeginClass<SUB, SUPER>()
        {
            if (typeof(SUB) == typeof(SUPER))
            {
                return SharpClass.Bind<SUB>(this);
            }

            return SharpClass.Extend<SUB, SUPER>(this);
        }

        public SharpClass BeginClass(Type classType, Type superClass = null)
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

        public SharpClass EndClass()
        {
            return parent;
        }

        public SharpModule BeginModule(string name)
        {
            return new SharpModule(m_meta, name);
        }

        public SharpClass EndModule()
        {
            return parent;
        }

        public void SetGetter(string name, LuaRef getter)
        {
            m_meta.RawGet("___getters").RawSet(name, getter);
        }

        public void SetSetter(string name, LuaRef setter)
        {
            LuaRef meta_class = m_meta;
            string full_name = GetMemberName(meta_class, name);
            LuaRef err = LuaRef.CreateFunctionWith(State, BindClassMetaMethod.ErrorConstMismatch, full_name);

            meta_class.RawGet("___setters").RawSet(name, setter);
        }

        public void SetReadOnly(string name)
        {
            LuaRef meta_class = m_meta;
            string full_name = GetMemberName(meta_class, name);
            LuaRef err = LuaRef.CreateFunctionWith(State, BindClassMetaMethod.ErrorReadOnly, full_name);

            meta_class.RawGet("___setters").RawSet(name, err);
        }

        public void SetMemberFunction(string name, LuaRef proc)
        {
            LuaRef meta_class = m_meta;
            meta_class.RawSet(name, proc);
        }

        #region 自动绑定
        public SharpClass RegClass<T>()
        {
            return RegClass(typeof(T));
        }

        public SharpClass RegClass<T, SUPER>()
        {
            return RegClass(typeof(T), typeof(SUPER));
        }

        public SharpClass RegClass(Type classType, Type superType = null)
        {
            var cls = BeginClass(classType, superType);
            cls.OnRegClass();
            return this;
        }

        protected void OnRegClass()
        {
            OnRegClass(classType);
        }

        protected void OnRegClass(Type type)
        {
            var fields = type.GetFields();
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

            var ctors = type.GetConstructors();
            foreach (var ctor in ctors)
            {
                if (!ctor.IsPublic)
                {
                    continue;
                }

                if (ctor.IsDefined(typeof(LuaHideAttribute)))
                {
                    continue;
                }

                RegConstructor(type, ctor);
            }

            var props = type.GetProperties();
            foreach (var p in props)
            {
                if (p.IsDefined(typeof(LuaHideAttribute)))
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

            var methods = type.GetMethods(BindingFlags.Public | BindingFlags.Static | BindingFlags.Instance | BindingFlags.DeclaredOnly);
            foreach (var m in methods)
            {
                if (m.IsSpecialName)
                {
                    continue;
                }

                if (!m.IsPublic)
                {
                    continue;
                }

                if (m.IsGenericMethod)
                {
                    continue;
                }

                if (m.IsDefined(typeof(LuaHideAttribute)))
                {
                    continue;
                }

                RegMethod(m);
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

        public SharpClass RegConstructor(Type type, ConstructorInfo constructorInfo)
        {
            var paramInfos = constructorInfo.GetParameters();
            var callerType = typeof(ClassConstructor<>).MakeGenericType(type);
            MethodInfo CallInnerDelegateMethod = callerType.GetMethod("Call", BindingFlags.Static | BindingFlags.Public);
            var luaFunc = (LuaNativeFunction)DelegateCache.Get(typeof(LuaNativeFunction), CallInnerDelegateMethod);

            if (paramInfos.Length == 0)
            {
                m_meta.RawSet("__call", LuaRef.CreateFunction(State, luaFunc));
            }
            else
            {
                m_meta.RawSet("__call", LuaRef.CreateFunction(State, luaFunc, constructorInfo));
            }

            return this;
        }

        public SharpClass RegField(FieldInfo fieldInfo)
        {
            if (classInfo != null)
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
            }
            
            if(fieldInfo.IsStatic)
            {
                var fieldDelType = typeof(FieldDelegate<>).MakeGenericType(fieldInfo.FieldType);
               
                var getMethodInfo = fieldDelType.GetMethod("Getter", BindingFlags.Static | BindingFlags.Public);
                var getDel = (Delegate)getMethodInfo.Invoke(null, new[] { fieldInfo });
                var getCallerType = typeof(FuncCaller<>).MakeGenericType(fieldInfo.FieldType);
                var getMethodCaller = getCallerType.GetMethod("Call", BindingFlags.Static | BindingFlags.Public);
                var getLuaDel = (LuaNativeFunction)DelegateCache.Get(typeof(LuaNativeFunction), getMethodCaller);
                var getter = LuaRef.CreateFunction(State, getLuaDel, getDel);
                SetGetter(fieldInfo.Name, getter);

                var setMethodInfo = fieldDelType.GetMethod("Setter", BindingFlags.Static | BindingFlags.Public);
                var setDel = (Delegate)setMethodInfo.Invoke(null, new[] { fieldInfo });
                var setCallerType = typeof(ActionCaller<>).MakeGenericType(fieldInfo.FieldType);
                var setMethodCaller = setCallerType.GetMethod("Call", BindingFlags.Static | BindingFlags.Public);
                var setLuaDel = (LuaNativeFunction)DelegateCache.Get(typeof(LuaNativeFunction), setMethodCaller);
                var setter = LuaRef.CreateFunction(State, setLuaDel, setDel);
                SetSetter(fieldInfo.Name, setter);
            }
            else
            {
                var fieldDelType = typeof(FieldDelegate<,>).MakeGenericType(fieldInfo.ReflectedType, fieldInfo.FieldType);

                var getMethodInfo = fieldDelType.GetMethod("Getter", BindingFlags.Static | BindingFlags.Public);
                var getDel = (Delegate)getMethodInfo.Invoke(null, new[] { fieldInfo });
                var getCallerType = typeof(FuncCaller<,>).MakeGenericType(fieldInfo.ReflectedType, fieldInfo.FieldType);
                var getMethodCaller = getCallerType.GetMethod("Call", BindingFlags.Static | BindingFlags.Public);
                var getLuaDel = (LuaNativeFunction)DelegateCache.Get(typeof(LuaNativeFunction), getMethodCaller);
                var getter = LuaRef.CreateFunction(State, getLuaDel, getDel);
                SetGetter(fieldInfo.Name, getter);

                var setMethodInfo = fieldDelType.GetMethod("Setter", BindingFlags.Static | BindingFlags.Public);
                var setDel = (Delegate)setMethodInfo.Invoke(null, new[] { fieldInfo });
                var setCallerType = typeof(ActionCaller<,>).MakeGenericType(fieldInfo.ReflectedType, fieldInfo.FieldType);
                var setMethodCaller = setCallerType.GetMethod("Call", BindingFlags.Static | BindingFlags.Public);
                var setLuaDel = (LuaNativeFunction)DelegateCache.Get(typeof(LuaNativeFunction), setMethodCaller);
                var setter = LuaRef.CreateFunction(State, setLuaDel, setDel);
                SetSetter(fieldInfo.Name, setter);
            }


            return this;
        }
        
        public SharpClass RegProperty(string name)
        {
            return RegProperty(classType, name);
        }

        public SharpClass RegProperty<T>(string name)
        {
            return RegProperty(typeof(T), name);
        }

        public SharpClass RegProperty(Type classType, string name)
        {
            PropertyInfo propertyInfo = classType.GetProperty(name);

            return RegProperty(propertyInfo);
        }

        public SharpClass RegProperty(PropertyInfo propertyInfo)
        {
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

            return this;
        }
        public SharpClass RegIndexer(PropertyInfo propertyInfo)
        {
            if (propertyInfo.CanRead)
            {
                MethodInfo methodInfo = propertyInfo.GetGetMethod(false);
                if (methodInfo != null)
                {
                    var luaFun = RegMethod(methodInfo, true);
                    if(luaFun)
                    {
                        SetMemberFunction("___get_indexed", luaFun);
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
                        SetMemberFunction("___set_indexed", luaFun);
                    }
                }
            }
            else
            {
                SetReadOnly(propertyInfo.Name);
            }

            return this;
        }

        public SharpClass RegMethod(string name)
        {
            return RegMethod(classType, name);
        }

        public SharpClass RegMethod<T>(string name)
        {
            return RegMethod(typeof(T), name);
        }

        public SharpClass RegMethod(Type classType, string name)
        {
            MethodInfo methodInfo = classType.GetMethod(name);
            return RegMethod(methodInfo);
        }

        public SharpClass RegMethod(MethodInfo methodInfo)
        {
            var luaFun = RegMethod(methodInfo, false);
            if (luaFun)
            {
                if (IsTagMethod(methodInfo.Name, out var tag))
                {
                    m_meta.RawSet(tag, luaFun);
                }
                else
                {
                    m_meta.RawSet(methodInfo.Name, luaFun);
                }
            }
            return this;
        }

        public LuaRef RegMethod(MethodInfo methodInfo, bool isProp)
        {
            Type typeOfResult = methodInfo.ReturnType;
            var paramInfo = methodInfo.GetParameters();
            List<Type> paramTypes = new List<Type>();

            if (!methodInfo.IsStatic)
            {
                System.Diagnostics.Debug.Assert(classType != null);

                paramTypes.Add(classType);
            }

            foreach (var info in paramInfo)
            {
                if(info.ParameterType.IsByRef)
                {
                    Luna.Log("不支持引用类型参数:" + methodInfo.ToString());
                    return LuaRef.Empty;
                }

                paramTypes.Add(info.ParameterType);
            }

            if (typeOfResult == typeof(void))
            {
                var typeArray = paramTypes.ToArray();
                return RegAction(methodInfo, typeArray, isProp);
            }
            else
            {
                paramTypes.Add(typeOfResult);
                var typeArray = paramTypes.ToArray();
                return RegFunc(methodInfo, typeArray, isProp);
            }

        }

        LuaRef RegAction(MethodInfo methodInfo, Type[] typeArray, bool isProp = false)
        {
#if LUNA_SCRIPT
            string callFnName = (methodInfo.IsStatic && !isProp) ? "StaticCall" : "Call";
#else
            string callFnName = "Call";
#endif
            Type funcDelegateType = null;
            Type callerType = null;
            LuaNativeFunction luaFunc = null;
            Delegate del = null;

            if (typeArray.Length == 0)
            {
                funcDelegateType = typeof(Action);

                del = DelegateCache.Get(funcDelegateType, methodInfo);
                luaFunc = ActionCaller.Call;
                return LuaRef.CreateFunction(State, luaFunc, del);
            }
            else if (typeArray.Length > DelegateCache.actionType.Length)
            {
                return LuaRef.Empty;
            }
            else
            {
                (var funcGenType, var callerGenType) = (classType.IsValueType && !methodInfo.IsStatic) ?
                    DelegateCache.refActionType[typeArray.Length] 
                    : DelegateCache.actionType[typeArray.Length];

                funcDelegateType = funcGenType.MakeGenericType(typeArray);
                callerType = callerGenType.MakeGenericType(typeArray);
            }
            
            del = DelegateCache.Get(funcDelegateType, methodInfo);

            if(del == null)
            {
                del = DelegateCache.Get(funcDelegateType, typeArray[0], methodInfo);
            }

            MethodInfo CallInnerDelegateMethod = callerType.GetMethod(callFnName, BindingFlags.Static | BindingFlags.Public);
            luaFunc = (LuaNativeFunction)DelegateCache.Get(typeof(LuaNativeFunction), CallInnerDelegateMethod);
            return LuaRef.CreateFunction(State, luaFunc, del);
        }

        LuaRef RegFunc(MethodInfo methodInfo, Type[] typeArray, bool isProp = false)
        {
#if LUNA_SCRIPT
            string callFnName = (methodInfo.IsStatic && !isProp) ? "StaticCall" : "Call";
#else
            string callFnName = "Call";
#endif
            Type funcDelegateType = null;
            Type callerType = null;
            LuaNativeFunction luaFunc = null;
            Delegate del = null;

            if (typeArray.Length >= DelegateCache.funcType.Length)
            {
                return LuaRef.Empty;
            }
            else
            {
                (var funcGenType, var callerGenType) = (classType.IsValueType && !methodInfo.IsStatic) ?
                    DelegateCache.refFuncType[typeArray.Length - 1]
                    : DelegateCache.funcType[typeArray.Length - 1];

                funcDelegateType = funcGenType.MakeGenericType(typeArray);
                callerType = callerGenType.MakeGenericType(typeArray);
            }
            
            del = DelegateCache.Get(funcDelegateType, methodInfo);
            if (del == null)
            {
                del = DelegateCache.Get(funcDelegateType, typeArray[0], methodInfo);
            }

            MethodInfo CallInnerDelegateMethod = callerType.GetMethod(callFnName, BindingFlags.Static | BindingFlags.Public);
            luaFunc = (LuaNativeFunction)DelegateCache.Get(typeof(LuaNativeFunction), CallInnerDelegateMethod);

            return LuaRef.CreateFunction(State, luaFunc, del);

        }

#endregion

#region 显式绑定

        public SharpClass AddConstant<T>(string name)
        {
            var v = typeof(T).GetField(name).GetValue(null);
            LuaRef r = LuaRef.FromValue(State, v);
            SetGetter(name, r);
            SetReadOnly(name);
            return this;
        }

        public SharpClass AddConstant<V>(string name, V v)
        {
            LuaRef r = LuaRef.FromValue(State, v);
            SetGetter(name, r);
            SetReadOnly(name);
            return this;
        }

        public SharpClass AddEnum<V>()
        {
            var t = typeof(V);
            if (t.IsEnum)
            {
                OnRegClass(typeof(V));
            }

            return this;
        }

        //静态变量
        public SharpClass AddVar<V>(string name, Func<V> getter, Action<V> setter = null)
        {
            SetGetter(name, LuaRef.CreateFunction(State, FuncCaller<V>.Call, getter));

            if (setter != null)
            {
                SetSetter(name, LuaRef.CreateFunction(State, ActionCaller<V>.Call, setter));
            }
            else
            {
                SetReadOnly(name);
            }

            return this;
        }

        //成员变量
        public SharpClass AddVar<T, V>(string name, Func<T, V> getter, Action<T, V> setter = null)
        {
            SetGetter(name, LuaRef.CreateFunction(State, FuncCaller<T, V>.Call, getter));

            if (setter != null)
            {
                SetSetter(name, LuaRef.CreateFunction(State, ActionCaller<T, V>.Call, setter));
            }
            else
            {
                SetReadOnly(name);
            }

            return this;
        }

        public SharpClass AddProperty<T, V>(string name)
        {
            var info = typeof(T).GetProperty(name);
            if (info.CanRead)
            {
                var getter = info.GetGetMethod(false);

                if (getter.IsStatic)
                {
                    var fn = DelegateCache.Get(typeof(Func<V>), getter);
                    SetGetter(name, LuaRef.CreateFunction(State, FuncCaller<V>.Call, fn));
                }
                else
                {
                    var fn = DelegateCache.Get(typeof(Func<T, V>), getter);
                    SetGetter(name, LuaRef.CreateFunction(State, FuncCaller<T, V>.Call, fn));
                }
            }

            if (info.CanWrite)
            {
                var setter = info.GetSetMethod(false);

                if (setter.IsStatic)
                {
                    var fn = DelegateCache.Get(typeof(Action<V>), setter);
                    SetSetter(name, LuaRef.CreateFunction(State, ActionCaller<V>.Call, fn));
                }
                else
                {
                    var fn = DelegateCache.Get(typeof(Action<T, V>), setter);
                    SetSetter(name, LuaRef.CreateFunction(State, ActionCaller<T, V>.Call, fn));
                }
            }
            else
            {
                SetReadOnly(name);
            }

            return this;
        }
        
        public SharpClass AddIndexer<T, K, V>(K key, V val)
        {
            return this;
        }

        public SharpClass AddConstructor<T>() where T : new()
        {
            m_meta.RawSet("__call", LuaRef.CreateFunction(State, ClassConstructor<T>.Call));
            return this;
        }

        public SharpClass AddStaticFn<T>(string name)
        {
            MethodInfo methodInfo = typeof(T).GetMethod(name);
            var fn = DelegateCache.Get<Action>(methodInfo);
            m_meta.RawSet(name, LuaRef.CreateFunction(State, ActionCaller.Call, fn));
            return this;
        }

        public SharpClass AddStaticFn<T, P1>(string name)
        {
            MethodInfo methodInfo = typeof(T).GetMethod(name);
            var fn = DelegateCache.Get<Action<P1>>(methodInfo);
#if LUNA_SCRIPT
            m_meta.RawSet(name, LuaRef.CreateFunction(State, ActionCaller<P1>.StaticCall, fn));
#else
            m_meta.RawSet(name, LuaRef.CreateFunction(State, ActionCaller<P1>.Call, fn));
#endif
            return this;
        }

        public SharpClass AddStaticFn<T, P1, P2>(string name)
        {
            MethodInfo methodInfo = typeof(T).GetMethod(name);
            var fn = DelegateCache.Get<Action<P1, P2>>(methodInfo);
#if LUNA_SCRIPT
            m_meta.RawSet(name, LuaRef.CreateFunction(State, ActionCaller<P1, P2>.StaticCall, fn));
#else
            m_meta.RawSet(name, LuaRef.CreateFunction(State, ActionCaller<P1, P2>.Call, fn));
#endif
            return this;
        }

        public SharpClass AddStaticFnR<T, R>(string name)
        {
            MethodInfo methodInfo = typeof(T).GetMethod(name);
            var fn = DelegateCache.Get<Func<R>>(methodInfo);
            m_meta.RawSet(name, LuaRef.CreateFunction(State, FuncCaller<R>.Call, fn));
            return this;
        }

        public SharpClass AddStaticFnR<T, P1, R>(string name)
        {
            MethodInfo methodInfo = typeof(T).GetMethod(name);
            var fn = DelegateCache.Get<Func<P1, R>>(methodInfo);
#if LUNA_SCRIPT
            m_meta.RawSet(name, LuaRef.CreateFunction(State, FuncCaller<P1, R>.StaticCall, fn));
#else
            m_meta.RawSet(name, LuaRef.CreateFunction(State, FuncCaller<P1, R>.Call, fn));
#endif
            return this;
        }

        public SharpClass AddStaticFnR<T, P1, P2, R>(string name)
        {
            MethodInfo methodInfo = typeof(T).GetMethod(name);
            var fn = DelegateCache.Get<Func<P1, P2, R>>(methodInfo);
#if LUNA_SCRIPT
            m_meta.RawSet(name, LuaRef.CreateFunction(State, FuncCaller<P1, P2, R>.StaticCall, fn));
#else
            m_meta.RawSet(name, LuaRef.CreateFunction(State, FuncCaller<P1, P2, R>.Call, fn));
#endif
            return this;
        }

        public SharpClass AddMethod<T>(string name)
        {
            MethodInfo methodInfo = typeof(T).GetMethod(name);
            if (methodInfo.IsStatic)
            {
                return AddStaticFn<T>(name);
            }

            var fn = DelegateCache.Get<Action<T>>(methodInfo);
            m_meta.RawSet(name, LuaRef.CreateFunction(State, ActionCaller<T>.Call, fn));
            return this;
        }

        public SharpClass AddMethod<T, P1>(string name)
        {
            MethodInfo methodInfo = typeof(T).GetMethod(name);
            if (methodInfo.IsStatic)
            {
                return AddStaticFn<T, P1>(name);
            }

            var fn = DelegateCache.Get<Action<T, P1>>(methodInfo);
            m_meta.RawSet(name, LuaRef.CreateFunction(State, ActionCaller<T, P1>.Call, fn));
            return this;
        }

        public SharpClass AddMethod<T, P1, P2>(string name)
        {
            MethodInfo methodInfo = typeof(T).GetMethod(name);
            if (methodInfo.IsStatic)
            {
                return AddStaticFn<T, P1, P2>(name);
            }

            var fn = DelegateCache.Get<Action<T, P1, P2>>(methodInfo);
            m_meta.RawSet(name, LuaRef.CreateFunction(State, ActionCaller<T, P1, P2>.Call, fn));
            return this;
        }

        public SharpClass AddMethodR<T, R>(string name)
        {
            MethodInfo methodInfo = typeof(T).GetMethod(name);
            if (methodInfo.IsStatic)
            {
                return AddStaticFnR<T, R>(name);
            }

            var fn = DelegateCache.Get<Func<T, R>>(methodInfo);
            m_meta.RawSet(name, LuaRef.CreateFunction(State, FuncCaller<T, R>.Call, fn));
            return this;
        }

        public SharpClass AddMethodR<T, P1, R>(string name)
        {
            MethodInfo methodInfo = typeof(T).GetMethod(name);
            if (methodInfo.IsStatic)
            {
                return AddStaticFnR<T, P1, R>(name);
            }

            var fn = DelegateCache.Get<Func<T, P1, R>>(methodInfo);
            m_meta.RawSet(name, LuaRef.CreateFunction(State, FuncCaller<T, P1, R>.Call, fn));
            return this;
        }

        public SharpClass AddMethodR<T, P1, P2, R>(string name)
        {
            MethodInfo methodInfo = typeof(T).GetMethod(name);
            if (methodInfo.IsStatic)
            {
                return AddStaticFnR<T, P1, P2, R>(name);
            }
            var fn = DelegateCache.Get<Func<T, P1, P2, R>>(methodInfo);
            m_meta.RawSet(name, LuaRef.CreateFunction(State, FuncCaller<T, P1, P2, R>.Call, fn));
            return this;
        }

#endregion

    }

}
