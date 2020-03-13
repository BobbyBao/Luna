using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using UnityEngine;

namespace SharpLuna
{
    using static Lua;
    public struct DelegateCache
    {
        static Dictionary<MethodInfo, Delegate> cache = new Dictionary<MethodInfo, Delegate>();

        public static Delegate Get<T>(MethodInfo methodInfo)
        {
            return Get(typeof(T), methodInfo);
        }

        public static Delegate Get(Type type, MethodInfo methodInfo)
        {
            if (cache.TryGetValue(methodInfo, out var del))
            {
                return del;
            }

            try
            {
                del = Delegate.CreateDelegate(type, methodInfo);
            }
            catch// (System.Exception ex)
            {
                //Debug.LogError(ex.Message);
                //Debug.LogError(type.ToString());
                //Debug.LogError(methodInfo.ReflectedType.ToString() + ", " + methodInfo.ToString());
                return null;
            }
            cache.Add(methodInfo, del);
            return del;
        }
        
        readonly static (Type, Type)[] actionType =
        {
            (typeof(Action), typeof(ActionFactory)),
            (typeof(Action<>), typeof(ActionFactory<>)),
            (typeof(Action<,>), typeof(ActionFactory<,>)),
            (typeof(Action<,,>), typeof(ActionFactory<,,>)),
            (typeof(Action<,,,>), typeof(ActionFactory<,,,>)),
            (typeof(Action<,,,,>), typeof(ActionFactory<,,,,>)),
            (typeof(Action<,,,,,>), typeof(ActionFactory<,,,,,>)),
            (typeof(Action<,,,,,,>), typeof(ActionFactory<,,,,,,>)),
            (typeof(Action<,,,,,,,>), typeof(ActionFactory<,,,,,,,>)),
        };

        readonly static (Type, Type)[] refActionType =
        {
            (typeof(RefAction), typeof(RefActionFactory)),
            (typeof(RefAction<>), typeof(RefActionFactory<>)),
            (typeof(RefAction<,>), typeof(RefActionFactory<,>)),
            (typeof(RefAction<,,>), typeof(RefActionFactory<,,>)),
            (typeof(RefAction<,,,>), typeof(RefActionFactory<,,,>)),
            (typeof(RefAction<,,,,>), typeof(RefActionFactory<,,,,>)),
            (typeof(RefAction<,,,,,>), typeof(RefActionFactory<,,,,,>)),
            (typeof(RefAction<,,,,,,>), typeof(RefActionFactory<,,,,,,>)),
            (typeof(RefAction<,,,,,,,>), typeof(RefActionFactory<,,,,,,,>)),
        };

        readonly static (Type, Type)[] funcType =
        {
            (typeof(Func<>), typeof(FuncFactory<>)),
            (typeof(Func<,>), typeof(FuncFactory<,>)),
            (typeof(Func<,,>), typeof(FuncFactory<,,>)),
            (typeof(Func<,,,>), typeof(FuncFactory<,,,>)),
            (typeof(Func<,,,,>), typeof(FuncFactory<,,,,>)),
            (typeof(Func<,,,,,>), typeof(FuncFactory<,,,,,>)),
            (typeof(Func<,,,,,,>), typeof(FuncFactory<,,,,,,>)),
            (typeof(Func<,,,,,,,>), typeof(FuncFactory<,,,,,,,>)),
            (typeof(Func<,,,,,,,,>), typeof(FuncFactory<,,,,,,,,>)),
        };

        readonly static (Type, Type)[] refFuncType =
        {
            (typeof(RefFunc<>), typeof(RefFuncFactory<>)),
            (typeof(RefFunc<,>), typeof(RefFuncFactory<,>)),
            (typeof(RefFunc<,,>), typeof(RefFuncFactory<,,>)),
            (typeof(RefFunc<,,,>), typeof(RefFuncFactory<,,,>)),
            (typeof(RefFunc<,,,,>), typeof(RefFuncFactory<,,,,>)),
            (typeof(RefFunc<,,,,,>), typeof(RefFuncFactory<,,,,,>)),
            (typeof(RefFunc<,,,,,,>), typeof(RefFuncFactory<,,,,,,>)),
            (typeof(RefFunc<,,,,,,,>), typeof(RefFuncFactory<,,,,,,,>)),
            (typeof(RefFunc<,,,,,,,,>), typeof(RefFuncFactory<,,,,,,,,>)),
        };

        public static bool GetMethodDelegate<T>(Type classType, MethodInfo methodInfo, string callFnName, out T luaFunc, out Delegate del) where T : Delegate
        {
            if (methodInfo.CallingConvention == CallingConventions.VarArgs)
            {
                Debug.Log("不支持可变参数类型:" + methodInfo.ToString());
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
                if (!info.ParameterType.ShouldExport())
                {
                    luaFunc = null;
                    del = null;
                    return false;
                }

                paramTypes.Add(info.ParameterType);
            }

            if (typeOfResult == typeof(void))
            {
                var typeArray = paramTypes.ToArray();
                return RegAction(classType, methodInfo, typeArray, callFnName, out luaFunc, out del);
            }
            else
            {
                if (!typeOfResult.ShouldExport())
                {
                    luaFunc = null;
                    del = null;
                    return false;
                }

                paramTypes.Add(typeOfResult);
                var typeArray = paramTypes.ToArray();
                return RegFunc(classType, methodInfo, typeArray, callFnName, out luaFunc, out del);
            }

        }

        static CallDel actionCallDel = ActionFactory.CallDel;
        static LuaNativeFunction actionCall = ActionFactory.Call;

        static bool RegAction<T>(Type classType, MethodInfo methodInfo, Type[] typeArray, string callFnName, out T luaFunc, out Delegate del) where T : Delegate
        {
            Type funcDelegateType = null;
            Type callerType = null;

            if (typeArray.Length == 0)
            {
                funcDelegateType = typeof(Action);
                del = DelegateCache.Get(funcDelegateType, methodInfo);
                if (typeof(T) == typeof(CallDel))
                {
                    luaFunc = (T)(object)actionCallDel;
                }
                else
                {
                    luaFunc = (T)(object)actionCall;
                }
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
                    refActionType[typeArray.Length] : actionType[typeArray.Length];

                    funcDelegateType = funcGenType.MakeGenericType(typeArray);
                    callerType = callerGenType.MakeGenericType(typeArray);
                }
                catch (Exception e)
                {
                    Debug.Log(e);
                    luaFunc = null;
                    del = null;
                    return false;
                }
            }

            del = DelegateCache.Get(funcDelegateType, methodInfo);
            MethodInfo CallInnerDelegateMethod = callerType.GetMethod(callFnName, BindingFlags.Static | BindingFlags.Public);
            luaFunc = (T)DelegateCache.Get(typeof(T), CallInnerDelegateMethod);
            return true;
        }

        static bool RegFunc<T>(Type classType, MethodInfo methodInfo, Type[] typeArray, string callFnName, out T luaFunc, out Delegate del) where T : Delegate
        {
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
                    refFuncType[typeArray.Length - 1] : funcType[typeArray.Length - 1];

                    funcDelegateType = funcGenType.MakeGenericType(typeArray);
                    callerType = callerGenType.MakeGenericType(typeArray);
                }
                catch (Exception e)
                {
                    Debug.Log(e);
                    luaFunc = null;
                    del = null;
                    return false;
                }
            }

            del = DelegateCache.Get(funcDelegateType, methodInfo);
            MethodInfo CallInnerDelegateMethod = callerType.GetMethod(callFnName, BindingFlags.Static | BindingFlags.Public);
            luaFunc = (T)DelegateCache.Get(typeof(T), CallInnerDelegateMethod);
            return true;

        }

    }

}
