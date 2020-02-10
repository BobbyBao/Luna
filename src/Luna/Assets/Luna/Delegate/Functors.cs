using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;

namespace SharpLuna
{
    using static Lua;
    using lua_State = IntPtr;

    public struct Constructor
    {
        [AOT.MonoPInvokeCallback(typeof(LuaNativeFunction))]
        public static int Call(lua_State L)
        {
            try
            {
                int n = lua_gettop(L);
                ConstructorInfo fn = ToLightObject<ConstructorInfo>(L, lua_upvalueindex(1), false);
                //忽略self
                object[] args = new object[n - 1];
                for (int i = 2; i <= n; i++)
                {
                    args[i - 2] = GetObject(L, i);
                }

                object ret = fn.Invoke(args);
                Push(L, ret);
                return 1;

            }
            catch (Exception e)
            {
                return luaL_error(L, e.Message);
            }
        }

    }

    public struct Constructor<T> where T : new()
    {
        [AOT.MonoPInvokeCallback(typeof(LuaNativeFunction))]
        public static int Call(lua_State L)
        {
            try
            {
                int n = lua_gettop(L);
                if (n == 1)
                {
                    var v = new T();
                    Push(L, v);
                    return 1;
                }
                lua_pushnil(L);
                return 1;
            }
            catch (Exception e)
            {
                return luaL_error(L, e.Message);
            }
        }

    }

    public struct Destructor
    {
        [AOT.MonoPInvokeCallback(typeof(LuaNativeFunction))]
        public static int Call(lua_State L)
        {
            SharpObject.Free(L, 1);
            return 0;
        }
    }

    public struct Field
    {
        [AOT.MonoPInvokeCallback(typeof(LuaNativeFunction))]
        public static int StaticGetter(lua_State L)
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
        public static int StaticSetter(lua_State L)
        {
            try
            {
                FieldInfo fieldInfo = ToLightObject<FieldInfo>(L, lua_upvalueindex(1), false);
                object v = GetObject(L, 1);
                fieldInfo.SetValue(null, v);
                return 0;
            }
            catch (Exception e)
            {
                return luaL_error(L, e.Message);
            }
        }

        [AOT.MonoPInvokeCallback(typeof(LuaNativeFunction))]
        public static int Getter(lua_State L)
        {
            try
            {
                FieldInfo fieldInfo = ToLightObject<FieldInfo>(L, lua_upvalueindex(1), false);
                object obj = GetObject(L, 1);
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
        public static int Setter(lua_State L)
        {
            try
            {
                FieldInfo fieldInfo = ToLightObject<FieldInfo>(L, lua_upvalueindex(1), false);
                object obj = GetObject(L, 1);
                object v = GetObject(L, 2);
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
        [AOT.MonoPInvokeCallback(typeof(LuaNativeFunction))]
        public static int StaticGetter(lua_State L)
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
        public static int StaticSetter(lua_State L)
        {
            try
            {
                PropertyInfo propertyInfo = ToLightObject<PropertyInfo>(L, lua_upvalueindex(1), false);
                object v = GetObject(L, 1);
                propertyInfo.SetValue(null, v);
                return 0;
            }
            catch (Exception e)
            {
                return luaL_error(L, e.Message);
            }
        }

        [AOT.MonoPInvokeCallback(typeof(LuaNativeFunction))]
        public static int Getter(lua_State L)
        {
            try
            {
                PropertyInfo propertyInfo = ToLightObject<PropertyInfo>(L, lua_upvalueindex(1), false);
                object obj = GetObject(L, 1);
                object v = propertyInfo.GetValue(obj);
                Push(L, v);
                return 1;
            }
            catch (Exception e)
            {
                return luaL_error(L, e.Message);
            }
        }

        [AOT.MonoPInvokeCallback(typeof(LuaNativeFunction))]
        public static int Setter(lua_State L)
        {
            try
            {
                PropertyInfo propertyInfo = ToLightObject<PropertyInfo>(L, lua_upvalueindex(1), false);
                object obj = GetObject(L, 1);
                object v = GetObject(L, 2);
                propertyInfo.SetValue(obj, v);
                return 0;
            }
            catch (Exception e)
            {
                return luaL_error(L, e.Message);
            }
        }

    }

    public class Method
    {
        public string methodName;
        public MethodInfo[] methodInfo;
        public ParameterInfo[][] parameters;
        public object[][] args;
        public Method(MethodInfo[] methodInfo)
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
        }

        [AOT.MonoPInvokeCallback(typeof(LuaNativeFunction))]
        public static int Call(lua_State L)
        {
            try
            {
                int n = lua_gettop(L);
                MethodInfo methodInfo = null;
                ParameterInfo[] parameterInfo = null;
                object[] args = null;
                Method method = ToLightObject<Method>(L, lua_upvalueindex(1), false);
                for(int i = 0; i < method.methodInfo.Length; i++)
                {
                    var m = method.methodInfo[i];
                    var paramInfo = method.parameters[i];
                    if (m.IsStatic)
                    {
#if LUNA_SCRIPT
                        if (paramInfo.Length == n - 1)
                        {
                            methodInfo = m; 
                            parameterInfo = paramInfo;
                            args = method.args[i];
                            break;
                        }
#else                        
                        if (paramInfo.Length == n)
                        {
                            methodInfo = m;
                            parameterInfo = paramInfo;
                            args = method.args[i];
                            break;
                        }
#endif
                    }
                    else
                    {
                        if (paramInfo.Length == n - 1)
                        {
                            methodInfo = m;
                            parameterInfo = paramInfo;
                            args = method.args[i];
                            break;
                        }

                    }
            
                }
#if LUNA_SCRIPT
                int StackStart = 2;
#else
                int StackStart = methodInfo.IsStatic ? 1 : 2;
#endif
                object obj = methodInfo.IsStatic ? null : GetObject(L, 1);
                //object[] args = new object[parameterInfo.Length];
                for (int i = 0; i < args.Length; i++)
                {
                    args[i] = GetObject(L, i + StackStart, parameterInfo[i].ParameterType);
                }

                object ret = methodInfo.Invoke(obj, args);
                Array.Clear(args, 0, args.Length);
                if (methodInfo.ReturnType != typeof(void))
                {
                    Push(L, ret);
                    return 1;
                }
                else
                {
                    return 0;
                }

            }
            catch (Exception e)
            {
                return luaL_error(L, e.Message);
            }
        }

    }

}
