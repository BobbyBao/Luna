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

    public struct Method
    {
        [AOT.MonoPInvokeCallback(typeof(LuaNativeFunction))]
        public static int StaticCall(lua_State L)
        {
            try
            {
                int n = lua_gettop(L);
                MethodInfo methodInfo = ToLightObject<MethodInfo>(L, lua_upvalueindex(1), false);
#if LUNA_SCRIPT
                const int StackStart = 2;
#else
                const int StackStart = 1;
#endif
                object[] args = new object[n - 1];
                for (int i = StackStart; i <= n; i++)
                {
                    args[i - StackStart] = GetObject(L, i);
                }

                object ret = methodInfo.Invoke(null, args);
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

        [AOT.MonoPInvokeCallback(typeof(LuaNativeFunction))]
        public static int Call(lua_State L)
        {
            try
            {
                int n = lua_gettop(L);
                MethodInfo methodInfo = ToLightObject<MethodInfo>(L, lua_upvalueindex(1), false);
                object obj = GetObject(L, 1);
                object[] args = new object[n - 1];
                for (int i = 2; i <= n; i++)
                {
                    args[i - 2] = GetObject(L, i);
                }

                object ret = methodInfo.Invoke(obj, args);
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
