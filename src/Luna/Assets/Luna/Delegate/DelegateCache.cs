﻿using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace SharpLuna
{
    using static Lua;
    public struct DelegateCache
    {
        static Dictionary<MethodInfo, Delegate> cache = new Dictionary<MethodInfo, Delegate>();

        public readonly static (Type, Type)[] actionType = 
        {
            (typeof(Action), typeof(ActionCaller)),
            (typeof(Action<>), typeof(ActionCaller<>)),
            (typeof(Action<,>), typeof(ActionCaller<,>)),
            (typeof(Action<,,>), typeof(ActionCaller<,,>)),
            (typeof(Action<,,,>), typeof(ActionCaller<,,,>)),
            (typeof(Action<,,,,>), typeof(ActionCaller<,,,,>)),
            (typeof(Action<,,,,,>), typeof(ActionCaller<,,,,,>)),
            (typeof(Action<,,,,,,>), typeof(ActionCaller<,,,,,,>)),
            (typeof(Action<,,,,,,,>), typeof(ActionCaller<,,,,,,,>)),
        };

        public readonly static (Type, Type)[] refActionType =
        {
            (typeof(RefAction), typeof(RefActionCaller)),
            (typeof(RefAction<>), typeof(RefActionCaller<>)),
            (typeof(RefAction<,>), typeof(RefActionCaller<,>)),
            (typeof(RefAction<,,>), typeof(RefActionCaller<,,>)),
            (typeof(RefAction<,,,>), typeof(RefActionCaller<,,,>)),
            (typeof(RefAction<,,,,>), typeof(RefActionCaller<,,,,>)),
            (typeof(RefAction<,,,,,>), typeof(RefActionCaller<,,,,,>)),
            (typeof(RefAction<,,,,,,>), typeof(RefActionCaller<,,,,,,>)),
            (typeof(RefAction<,,,,,,,>), typeof(RefActionCaller<,,,,,,,>)),
        };

        public readonly static (Type, Type)[] funcType =
        {
            (typeof(Func<>), typeof(FuncCaller<>)),
            (typeof(Func<,>), typeof(FuncCaller<,>)),
            (typeof(Func<,,>), typeof(FuncCaller<,,>)),
            (typeof(Func<,,,>), typeof(FuncCaller<,,,>)),
            (typeof(Func<,,,,>), typeof(FuncCaller<,,,,>)),
            (typeof(Func<,,,,,>), typeof(FuncCaller<,,,,,>)),
            (typeof(Func<,,,,,,>), typeof(FuncCaller<,,,,,,>)),
            (typeof(Func<,,,,,,,>), typeof(FuncCaller<,,,,,,,>)),
            (typeof(Func<,,,,,,,,>), typeof(FuncCaller<,,,,,,,,>)),
        };

        public readonly static (Type, Type)[] refFuncType =
        {
            (typeof(RefFunc<>), typeof(RefFuncCaller<>)),
            (typeof(RefFunc<,>), typeof(RefFuncCaller<,>)),
            (typeof(RefFunc<,,>), typeof(RefFuncCaller<,,>)),
            (typeof(RefFunc<,,,>), typeof(RefFuncCaller<,,,>)),
            (typeof(RefFunc<,,,,>), typeof(RefFuncCaller<,,,,>)),
            (typeof(RefFunc<,,,,,>), typeof(RefFuncCaller<,,,,,>)),
            (typeof(RefFunc<,,,,,,>), typeof(RefFuncCaller<,,,,,,>)),
            (typeof(RefFunc<,,,,,,,>), typeof(RefFuncCaller<,,,,,,,>)),
            (typeof(RefFunc<,,,,,,,,>), typeof(RefFuncCaller<,,,,,,,,>)),
        };

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
            catch (System.Exception ex)
            {
                Luna.LogError(ex.Message);
                Luna.LogError(type.ToString());
                Luna.LogError(methodInfo.ReflectedType.ToString() + ", " + methodInfo.ToString());
                return null;
            }
            cache.Add(methodInfo, del);
            return del;
        }

        public static Delegate Get(Type type, Type targetType, MethodInfo methodInfo)
        {
            if (cache.TryGetValue(methodInfo, out var del))
            {
                return del;
            }

            try
            {
                del = Delegate.CreateDelegate(type, methodInfo);
            }
            catch (System.Exception ex)
            {
                Luna.LogError(ex.Message);
                Luna.LogError(type.ToString());
                Luna.LogError(methodInfo.ReflectedType.ToString() + ", " + methodInfo.ToString());
                return null;
            }
            cache.Add(methodInfo, del);
            return del;
        }
        
    }

}
