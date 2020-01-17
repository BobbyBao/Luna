//#define METATABLE_EXTEND

using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace SharpLuna
{
    using lua_State = IntPtr;
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
            get { return parent.Luna;}
        }

        public lua_State State => m_meta.State;

        public LuaRef Meta => m_meta;

        protected void SetClassType(Type type)
        {
            classType = type;

            classInfo = Luna.GetClassWrapper(classType);

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
        
        public void SetGetter(string name, LuaRef getter)
        {
            m_meta.RawGet(___getters).RawSet(name, getter);
        }

        public void SetSetter(string name, LuaRef setter)
        {
            LuaRef meta_class = m_meta;
            meta_class.RawGet(___setters).RawSet(name, setter);
        }

        public void SetReadOnly(string name)
        {
            LuaRef meta_class = m_meta;
            string full_name = GetMemberName(meta_class, name);
            LuaRef err = LuaRef.CreateFunctionWith(State, ErrorReadOnly, full_name);

            meta_class.RawGet(___setters).RawSet(name, err);
        }

        public void SetMemberFunction(IntPtr name, LuaRef proc)
        {
            m_meta.RawSet(name, proc);
        }

        public void SetMemberFunction(string name, LuaRef proc)
        {
            m_meta.RawSet(name, proc);
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
            if (classInfo != null)
            {
                if (classInfo.TryGetValue("ctor", out var methodConfig))
                {
                    if (methodConfig.func != null)
                    {
                        m_meta.RawSet("__call", LuaRef.CreateFunction(State, methodConfig.func));
                        wrapCtor = true;
                    }

                }
            }

            if (!wrapCtor)
            {
                var ctors = classType.GetConstructors();
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

                    RegConstructor(classType, ctor);
                }
            }


            var props = classType.GetProperties();
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

            var methods = classType.GetMethods(BindingFlags.Public | BindingFlags.Static | BindingFlags.Instance | BindingFlags.DeclaredOnly);
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
            foreach (var info in paramInfos)
            {
                if (info.ParameterType.IsByRef || info.ParameterType.IsPointer)
                {
                    return this;
                }
            }

            try
            {
                if (paramInfos.Length == 0)
                {
                    var callerType = typeof(ClassConstructor<>).MakeGenericType(type);
                    MethodInfo CallInnerDelegateMethod = callerType.GetMethod("Call", BindingFlags.Static | BindingFlags.Public);
                    var luaFunc = (LuaNativeFunction)DelegateCache.Get(typeof(LuaNativeFunction), CallInnerDelegateMethod);

                    m_meta.RawSet("__call", LuaRef.CreateFunction(State, luaFunc));
                }
                else
                {
                    m_meta.RawSet("__call", LuaRef.CreateFunction(State, (LuaNativeFunction)CallConstructor, constructorInfo));
                }
            }
            catch(Exception e)
            {
                Luna.Log(e.Message);
            }

            return this;
        }

        static int CallConstructor(lua_State L)
        {
            try
            {
                int n = lua_gettop(L);
                ConstructorInfo fn = ToLightObject<ConstructorInfo>(L, lua_upvalueindex(1), false);
                //忽略self
                object[] args = new object[n - 1];
                for (int i = 2; i <= n; i++)
                {
                    args[i - 2] = Lua.GetObject(L, i);
                }

                object ret = fn.Invoke(args);
                Lua.Push(L, ret);
                return 1;

            }
            catch (Exception e)
            {
                return luaL_error(L, "%s", e.Message);
            }
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
                var getDel = DelegateCache.GetInvokeer(getMethodInfo, fieldInfo);// (Delegate)getMethodInfo.Invoke(null, new[] { fieldInfo });
                var getCallerType = typeof(FuncCaller<>).MakeGenericType(fieldInfo.FieldType);
                var getMethodCaller = getCallerType.GetMethod("Call", BindingFlags.Static | BindingFlags.Public);
                var getLuaDel = (LuaNativeFunction)DelegateCache.Get(typeof(LuaNativeFunction), getMethodCaller);
                var getter = LuaRef.CreateFunction(State, getLuaDel, getDel);
                SetGetter(fieldInfo.Name, getter);

                var setMethodInfo = fieldDelType.GetMethod("Setter", BindingFlags.Static | BindingFlags.Public);
                var setDel = DelegateCache.GetInvokeer(getMethodInfo, fieldInfo);// (Delegate)setMethodInfo.Invoke(null, new[] { fieldInfo });
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
                var getDel = DelegateCache.GetInvokeer(getMethodInfo, fieldInfo);//(Delegate)getMethodInfo.Invoke(null, new[] { fieldInfo });
                var getCallerType = typeof(FuncCaller<,>).MakeGenericType(fieldInfo.ReflectedType, fieldInfo.FieldType);
                var getMethodCaller = getCallerType.GetMethod("Call", BindingFlags.Static | BindingFlags.Public);
                var getLuaDel = (LuaNativeFunction)DelegateCache.Get(typeof(LuaNativeFunction), getMethodCaller);
                var getter = LuaRef.CreateFunction(State, getLuaDel, getDel);
                SetGetter(fieldInfo.Name, getter);

                var setMethodInfo = fieldDelType.GetMethod("Setter", BindingFlags.Static | BindingFlags.Public);
                var setDel = DelegateCache.GetInvokeer(setMethodInfo, fieldInfo);//(Delegate)setMethodInfo.Invoke(null, new[] { fieldInfo });
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
            if (classInfo != null)
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
            }

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
            if (classInfo != null)
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
            if (classInfo != null)
            {
                if (classInfo.TryGetValue(name, out var methodConfig))
                {
                    if (methodConfig.getter != null)
                    {
                        var luaFun = LuaRef.CreateFunction(State, methodConfig.func);
                        if (IsTagMethod(name, out var tag))
                        {
                            m_meta.RawSet(tag, luaFun);
                        }
                        else
                        {
                            m_meta.RawSet(name, luaFun);
                        }
                    }

                    return this;
                }
            }

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

        //todo:同名函数处理
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

            if(methodInfo.CallingConvention == CallingConventions.VarArgs)
            {
                Luna.Log("不支持可变参数类型:" + methodInfo.ToString());
                return LuaRef.Empty;
            }

            foreach (var info in paramInfo)
            {
                if (info.ParameterType.IsGenericType)
                {
                    Luna.Log("不支持泛型参数:" + methodInfo.ToString());
                    return LuaRef.Empty;
                }

                if (info.ParameterType.IsByRef || info.ParameterType.IsPointer)
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
                if (typeOfResult.IsGenericType)
                {
                    Luna.Log("不支持泛型参数:" + methodInfo.ToString());
                    return LuaRef.Empty;
                }

                if (typeOfResult.IsByRef || typeOfResult.IsPointer)
                {
                    Luna.Log("不支持引用类型参数:" + methodInfo.ToString());
                    return LuaRef.Empty;
                }

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
                try
                {
                    (var funcGenType, var callerGenType) = (classType.IsValueType && !methodInfo.IsStatic) ?
                    DelegateCache.refActionType[typeArray.Length] 
                    : DelegateCache.actionType[typeArray.Length];

                    funcDelegateType = funcGenType.MakeGenericType(typeArray);
                    callerType = callerGenType.MakeGenericType(typeArray);
                }
                catch (Exception e)
                {
                    Luna.Log(e);
                    return LuaRef.Empty;
                }
            }
            
            del = DelegateCache.Get(funcDelegateType, methodInfo);
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
                try
                {
                    (var funcGenType, var callerGenType) = (classType.IsValueType && !methodInfo.IsStatic) ?
                    DelegateCache.refFuncType[typeArray.Length - 1]
                    : DelegateCache.funcType[typeArray.Length - 1];

                    funcDelegateType = funcGenType.MakeGenericType(typeArray);
                    callerType = callerGenType.MakeGenericType(typeArray);
                }
                catch (Exception e)
                {
                    Luna.Log(e);
                    return LuaRef.Empty;
                }
            }
            
            del = DelegateCache.Get(funcDelegateType, methodInfo);
            MethodInfo CallInnerDelegateMethod = callerType.GetMethod(callFnName, BindingFlags.Static | BindingFlags.Public);
            luaFunc = (LuaNativeFunction)DelegateCache.Get(typeof(LuaNativeFunction), CallInnerDelegateMethod);

            return LuaRef.CreateFunction(State, luaFunc, del);

        }

#endregion


    }

}
