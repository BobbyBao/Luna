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

        public static string GetFullName(LuaRef parent, string name)
        {
            string full_name = parent.Get(___type, "");
            if (!string.IsNullOrEmpty(full_name))
            {
                int pos = full_name.IndexOf('<');
                if (pos != -1) full_name.Remove(0, pos + 1);
                pos = full_name.LastIndexOf('>');
                if (pos != -1) full_name.Remove(pos);
                full_name += '.';
            }
            full_name += name;
            return full_name;
        }

        public static string GetMemberName(LuaRef parent, string name)
        {
            string full_name = parent.RawGet(___type, "<unknown>");
            full_name += '.';
            full_name += name;
            return full_name;
        }

        public static SharpClass Bind<T>(SharpClass parentMeta)
        {
            Type classType = typeof(T);
            if (registeredClass.TryGetValue(classType, out var bindClass))
            {
                return bindClass;
            }

            var name = GetTableName(classType);
            LuaRef meta = LuaRef.None;
            if (CreateClass(ref meta, parentMeta.Meta, name, SharpObject.Signature<T>()))
            {
                meta.RawSet("__gc", (LuaNativeFunction)Destructor.Call);
            }

            bindClass = new SharpClass(meta);
            bindClass.parent = parentMeta;
            bindClass.SetClassType(classType);
            registeredClass.Add(classType, bindClass);
            return bindClass;
        }

        public static SharpClass Extend<T, SUPER>(SharpClass parent)
        {
            Type classType = typeof(T);
            if (registeredClass.TryGetValue(classType, out var bindClass))
            {
                return bindClass;
            }

            string name = GetTableName(classType);
            LuaRef meta = LuaRef.None;
            if (CreateClass(ref meta, parent.Meta, name, SharpObject.Signature<T>(), SharpObject.Signature<SUPER>()))
            {
                meta.RawSet("__gc", (LuaNativeFunction)Destructor.Call);
            }

            bindClass = new SharpClass(meta);
            bindClass.parent = parent;
            bindClass.SetClassType(classType);
            registeredClass.Add(classType, bindClass);
            return bindClass;
        }

        public static SharpClass Extend(Type classType, Type superType, SharpClass parent)
        {
            if (registeredClass.TryGetValue(classType, out var bindClass))
            {
                return bindClass;
            }

            string name = GetTableName(classType);
            LuaRef meta = LuaRef.None;
            if (CreateClass(ref meta, parent.Meta, name, SharpObject.Signature(classType), SharpObject.Signature(superType)))
            {                
                meta.RawSet("__gc", (LuaNativeFunction)Destructor.Call);                
            }

            bindClass = new SharpClass(meta);
            bindClass.parent = parent;
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

        public void SetMemberFunction(string name, LuaRef proc)
        {
            meta.RawSet(name, proc);
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
            var cls = GetClass(classType, superType);
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
                    //RegConstructor(classType, ctor);
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

       /*
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
                    var callerType = typeof(Constructor<>).MakeGenericType(type);
                    MethodInfo CallInnerDelegateMethod = callerType.GetMethod("Call", BindingFlags.Static | BindingFlags.Public);
                    var luaFunc = (LuaNativeFunction)DelegateCache.Get(typeof(LuaNativeFunction), CallInnerDelegateMethod);
                    meta.RawSet("__call", LuaRef.CreateFunction(State, luaFunc));
                    return this;
                }
            }
            catch(Exception e)
            {
                Luna.Log(e.Message);
            }

            Luna.LogWarning("注册反射版的Constructor : " + type.Name);

            meta.RawSet("__call", LuaRef.CreateFunction(State, Constructor.Call, constructorInfo));
            return this;
        }*/

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
            LuaRef luaFun = null;
            /*
            if (methodInfo.Length == 1)
            {                  
                luaFun = RegMethod(methodInfo[0], false);          
            }
            else
            {
                //todo同名函数处理
                //Luna.Log($"{name}存在同名函数" );
            }*/

            if (!luaFun)
            {
                //Luna.Log("退化成反射方式实现");
                Method method = new Method(methodInfo);
                luaFun = LuaRef.CreateFunction(State, Method.Call, method);// methodInfo);
            }

            if (luaFun)
            {
                if (IsTagMethod(name, out var tag))
                {
                    meta.RawSet(tag, luaFun);
                }
                else
                {
                    meta.RawSet(name, luaFun);
                }

            }

            return this;
        }

        public LuaRef RegMethod(MethodInfo methodInfo, bool isProp)
        {
            if (RegMethod(methodInfo, isProp, out LuaNativeFunction luaFunc, out Delegate del))
            {
                return LuaRef.CreateFunction(State, luaFunc, del);
            }

            return null;
        }

        public bool RegMethod(MethodInfo methodInfo, bool isProp, out LuaNativeFunction luaFunc, out Delegate del)
        {
            if (methodInfo.CallingConvention == CallingConventions.VarArgs)
            {
                Luna.Log("不支持可变参数类型:" + methodInfo.ToString());
                luaFunc = null;
                del = null;
                return false;
            }

            Type typeOfResult = methodInfo.ReturnType;
            var paramInfo = methodInfo.GetParameters();
            List<Type> paramTypes = new List<Type>();
            if (!methodInfo.IsStatic)
            {
                paramTypes.Add(classType);
            }

            foreach (var info in paramInfo)
            {
                if (info.ParameterType.IsGenericType)
                {
                    Luna.Log("不支持泛型参数:" + methodInfo.ToString());
                    luaFunc = null;
                    del = null;
                    return false;
                }

                if (info.ParameterType.IsByRef || info.ParameterType.IsPointer)
                {
                    Luna.Log("不支持引用类型参数:" + methodInfo.ToString());
                    luaFunc = null;
                    del = null;
                    return false;
                }

                paramTypes.Add(info.ParameterType);
            }

            if (typeOfResult == typeof(void))
            {
                var typeArray = paramTypes.ToArray();
                return RegAction(methodInfo, typeArray, isProp, out luaFunc, out del);
            }
            else
            {
                if (typeOfResult.IsGenericType)
                {
                    Luna.Log("不支持泛型参数:" + methodInfo.ToString());
                    luaFunc = null;
                    del = null;
                    return false;
                }

                if (typeOfResult.IsByRef || typeOfResult.IsPointer)
                {
                    Luna.Log("不支持引用类型参数:" + methodInfo.ToString());
                    luaFunc = null;
                    del = null;
                    return false;
                }

                paramTypes.Add(typeOfResult);
                var typeArray = paramTypes.ToArray();
                return RegFunc(methodInfo, typeArray, isProp, out luaFunc, out del);
            }

        }

        bool RegAction(MethodInfo methodInfo, Type[] typeArray, bool isProp, out LuaNativeFunction luaFunc, out Delegate del)
        {
#if LUNA_SCRIPT
            string callFnName = (methodInfo.IsStatic && !isProp) ? "StaticCall" : "Call";
#else
            string callFnName = "Call";
#endif
            Type funcDelegateType = null;
            Type callerType = null;
            if (typeArray.Length == 0)
            {
                funcDelegateType = typeof(Action);
                del = DelegateCache.Get(funcDelegateType, methodInfo);
                luaFunc = ActionCaller.Call;
                return true;
            }
            else if (typeArray.Length > DelegateCache.actionType.Length)
            {
                luaFunc = null;
                del = null;
                return false;
            }
            else
            {
                try
                {
                    (var funcGenType, var callerGenType) = (classType.IsValueType && !methodInfo.IsStatic) ?
                    DelegateCache.refActionType[typeArray.Length] : DelegateCache.actionType[typeArray.Length];

                    funcDelegateType = funcGenType.MakeGenericType(typeArray);
                    callerType = callerGenType.MakeGenericType(typeArray);
                }
                catch (Exception e)
                {
                    Luna.Log(e);
                    luaFunc = null;
                    del = null;
                    return false;
                }
            }
            
            del = DelegateCache.Get(funcDelegateType, methodInfo);
            MethodInfo CallInnerDelegateMethod = callerType.GetMethod(callFnName, BindingFlags.Static | BindingFlags.Public);
            luaFunc = (LuaNativeFunction)DelegateCache.Get(typeof(LuaNativeFunction), CallInnerDelegateMethod);
            return true;
        }

        bool RegFunc(MethodInfo methodInfo, Type[] typeArray, bool isProp, out LuaNativeFunction luaFunc, out Delegate del)
        {
#if LUNA_SCRIPT
            string callFnName = (methodInfo.IsStatic && !isProp) ? "StaticCall" : "Call";
#else
            string callFnName = "Call";
#endif
            Type funcDelegateType = null;
            Type callerType = null;
            if (typeArray.Length >= DelegateCache.funcType.Length)
            {
                luaFunc = null;
                del = null;
                return false;
            }
            else
            {
                try
                {
                    (var funcGenType, var callerGenType) = (classType.IsValueType && !methodInfo.IsStatic) ?
                    DelegateCache.refFuncType[typeArray.Length - 1] : DelegateCache.funcType[typeArray.Length - 1];

                    funcDelegateType = funcGenType.MakeGenericType(typeArray);
                    callerType = callerGenType.MakeGenericType(typeArray);
                }
                catch (Exception e)
                {
                    Luna.Log(e);
                    luaFunc = null;
                    del = null;
                    return false;
                }
            }
            
            del = DelegateCache.Get(funcDelegateType, methodInfo);
            MethodInfo CallInnerDelegateMethod = callerType.GetMethod(callFnName, BindingFlags.Static | BindingFlags.Public);
            luaFunc = (LuaNativeFunction)DelegateCache.Get(typeof(LuaNativeFunction), CallInnerDelegateMethod);
            return true;

        }

#endregion


    }

}
