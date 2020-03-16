﻿using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;

namespace SharpLuna
{
    using static Lua;
    using lua_State = IntPtr;

    public delegate int CallDel(lua_State L, int start, Delegate del);

    public class MethodReflection
    {
        public string methodName;
        public MethodBase[] methodInfo;
        public ParameterInfo[][] parameters;
        public object[][] args;
        public CallDel[] luaFunc;
        public Delegate[] del;

        public MethodReflection(MethodBase[] methodInfo)
        {
            methodName = methodInfo[0].Name;
            this.methodInfo = methodInfo;
            parameters = new ParameterInfo[methodInfo.Length][];
            args = new object[methodInfo.Length][]; //非线程安全
            for (int i = 0; i < methodInfo.Length; i++)
            {
                parameters[i] = methodInfo[i].GetParameters();
                args[i] = new object[parameters[i].Length];
            }

            luaFunc = new CallDel[methodInfo.Length];
            del = new Delegate[methodInfo.Length];
        }

        public static LuaNativeFunction Call = _Call;
        [AOT.MonoPInvokeCallback(typeof(LuaNativeFunction))]
        static int _Call(lua_State L)
        {
            try
            {
                int n = lua_gettop(L);
                MethodBase methodInfo = null;
                ParameterInfo[] parameterInfo = null;
                object[] args = null;
                MethodReflection method = ToLightObject<MethodReflection>(L, lua_upvalueindex(1), false);
                CallDel callDel = null;
                Delegate del = null;
                for (int i = 0; i < method.methodInfo.Length; i++)
                {
                    var m = method.methodInfo[i];
                    var paramInfo = method.parameters[i];
                    int paramCount = n - 1;
#if !LUNA_SCRIPT              
                    if (m.IsStatic)
                    {
                        paramCount = n;
                    }
#endif
                    if (paramInfo.Length == paramCount)
                    {
                        //todo:CheckType
                        methodInfo = m;
                        parameterInfo = paramInfo;
                        args = method.args[i];
                        callDel = method.luaFunc[i];
                        del = method.del[i];
                        break;
                    }
                }

                if (callDel != null)
                {
                    return callDel(L, methodInfo.IsStatic ? 2 : 1, del);
                }

#if LUNA_SCRIPT
                int StackStart = 2;
#else
                int StackStart = methodInfo.IsStatic ? 1 : 2;
#endif

                for (int i = 0; i < args.Length; i++)
                {
                    args[i] = GetObject(L, i + StackStart, parameterInfo[i].ParameterType);
                }

                if (methodInfo.IsConstructor)
                {
                    object ret = ((ConstructorInfo)methodInfo).Invoke(args);
                    Array.Clear(args, 0, args.Length);
                    Push(L, ret);
                    return 1;
                }
                else
                {
                    object obj = methodInfo.IsStatic ? null : GetObject(L, 1, methodInfo.ReflectedType);
                    MethodInfo mi = methodInfo as MethodInfo;
                    object ret = methodInfo.Invoke(obj, args);
                    Array.Clear(args, 0, args.Length);

                    if (mi.ReturnType != typeof(void))
                    {
                        Push(L, ret);
                        return 1;
                    }
                    else
                    {
                        return 0;
                    }
                }


            }
            catch (Exception e)
            {
                return luaL_error(L, e.Message);
            }
        }

    }

    public struct Field
    {
        public static LuaNativeFunction StaticGetter = _StaticGetter;
        public static LuaNativeFunction StaticSetter = _StaticSetter;
        public static LuaNativeFunction Getter = _Getter;
        public static LuaNativeFunction Setter = _Setter;

        [AOT.MonoPInvokeCallback(typeof(LuaNativeFunction))]
        static int _StaticGetter(lua_State L)
        {
            try
            {
                FieldInfo fieldInfo = ToLightObject<FieldInfo>(L, lua_upvalueindex(1), false);
                object v = fieldInfo.GetValue(null);
                Push(L, v);
                return 1;
            }
            catch (Exception e)
            {
                return luaL_error(L, e.Message);
            }
        }

        [AOT.MonoPInvokeCallback(typeof(LuaNativeFunction))]
        static int _StaticSetter(lua_State L)
        {
            try
            {
                FieldInfo fieldInfo = ToLightObject<FieldInfo>(L, lua_upvalueindex(1), false);
                object v = GetObject(L, 1, fieldInfo.FieldType);
                fieldInfo.SetValue(null, v);
                return 0;
            }
            catch (Exception e)
            {
                return luaL_error(L, e.Message);
            }
        }

        [AOT.MonoPInvokeCallback(typeof(LuaNativeFunction))]
        static int _Getter(lua_State L)
        {
            try
            {
                FieldInfo fieldInfo = ToLightObject<FieldInfo>(L, lua_upvalueindex(1), false);
                object obj = GetObject(L, 1, fieldInfo.ReflectedType);
                object v = fieldInfo.GetValue(obj);
                Push(L, v);
                return 1;
            }
            catch (Exception e)
            {
                return luaL_error(L, e.Message);
            }
        }

        [AOT.MonoPInvokeCallback(typeof(LuaNativeFunction))]
        static int _Setter(lua_State L)
        {
            try
            {
                FieldInfo fieldInfo = ToLightObject<FieldInfo>(L, lua_upvalueindex(1), false);
                object obj = GetObject(L, 1, fieldInfo.ReflectedType);
                object v = GetObject(L, 2, fieldInfo.FieldType);
                fieldInfo.SetValue(obj, v);
                return 0;
            }
            catch (Exception e)
            {
                return luaL_error(L, e.Message);
            }
        }

    }

    public struct Property
    {
        public static LuaNativeFunction StaticGetter = _StaticGetter;
        public static LuaNativeFunction StaticSetter = _StaticSetter;
        public static LuaNativeFunction Getter = _Getter;
        public static LuaNativeFunction Setter = _Setter;

        [AOT.MonoPInvokeCallback(typeof(LuaNativeFunction))]
        static int _StaticGetter(lua_State L)
        {
            try
            {
                PropertyInfo propertyInfo = ToLightObject<PropertyInfo>(L, lua_upvalueindex(1), false);
                object v = propertyInfo.GetValue(null);
                Push(L, v);
                return 1;
            }
            catch (Exception e)
            {
                return luaL_error(L, e.Message);
            }
        }

        [AOT.MonoPInvokeCallback(typeof(LuaNativeFunction))]
        static int _StaticSetter(lua_State L)
        {
            try
            {
                PropertyInfo propertyInfo = ToLightObject<PropertyInfo>(L, lua_upvalueindex(1), false);
                object v = GetObject(L, 1, propertyInfo.PropertyType);
                propertyInfo.SetValue(null, v);
                return 0;
            }
            catch (Exception e)
            {
                return luaL_error(L, e.Message);
            }
        }

        [AOT.MonoPInvokeCallback(typeof(LuaNativeFunction))]
        static int _Getter(lua_State L)
        {
            try
            {
                PropertyInfo propertyInfo = ToLightObject<PropertyInfo>(L, lua_upvalueindex(1), false);
                int n = lua_gettop(L);
                object obj = GetObject(L, 1, propertyInfo.ReflectedType);
                if (n == 2)
                {
                    MethodInfo m = propertyInfo.GetGetMethod();
                    var paramType = m.GetParameters()[0].ParameterType;
                    object idx = GetObject(L, 2, paramType);
                    object v = propertyInfo.GetValue(obj, new[] { idx });
                    Push(L, v);
                }
                else
                {
                    object v = propertyInfo.GetValue(obj);
                    Push(L, v);
                }
                return 1;
            }
            catch (Exception e)
            {
                return luaL_error(L, e.Message);
            }
        }

        [AOT.MonoPInvokeCallback(typeof(LuaNativeFunction))]
        static int _Setter(lua_State L)
        {
            try
            {
                PropertyInfo propertyInfo = ToLightObject<PropertyInfo>(L, lua_upvalueindex(1), false);
                int n = lua_gettop(L);
                object obj = GetObject(L, 1, propertyInfo.ReflectedType);
                if(n == 3)
                {
                    MethodInfo m = propertyInfo.GetSetMethod();
                    var paramType = m.GetParameters()[0].ParameterType;
                    object idx = GetObject(L, 2, paramType);
                    object v = GetObject(L, 3, propertyInfo.PropertyType);
                    propertyInfo.SetValue(obj, v, new[] { idx });
                }
                else
                {
                    object v = GetObject(L, 2, propertyInfo.PropertyType);
                    propertyInfo.SetValue(obj, v);
                }
                return 0;
            }
            catch (Exception e)
            {
                return luaL_error(L, e.Message);
            }
        }

    }


}
