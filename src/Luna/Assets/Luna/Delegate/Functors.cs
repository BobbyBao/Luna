using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;

namespace SharpLuna
{
    using static Lua;
    using lua_State = IntPtr;

    public struct ConstantVariable
    {
        public static int Call(lua_State L)
        {
            lua_pushvalue(L, lua_upvalueindex(1));
            return 1;
        }
    }

    public struct ClassConstructor<T> where T : new()
    {
        public static int Call(lua_State L)
        {
            try
            {
                int n = lua_gettop(L);
                if (n == 1)
                {
                    var v = new T();
                    Lua.Push(L, v);
                    return 1;
                }
                
                return CallConstructor(L, n);
            }
            catch (Exception e)
            {
                return luaL_error(L, e.Message);
            }
        }

        static int CallConstructor(lua_State L, int n)
        {
            try
            {
                ConstructorInfo fn = Lua.ToLightObject<ConstructorInfo>(L, lua_upvalueindex(1), false);
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
    }

    public struct ClassDestructor<T>
    {
        public static int Call(lua_State L)
        {
            try
            {
                SharpObject.Free<T>(L, 1);
                return 0;
            }
            catch (Exception e)
            {
                return luaL_error(L, e.Message);
            }
        }
    }

    public struct PropertyCaller<T1>
    {
        public static int Getter(lua_State L)
        {
            var a = Lua.ToLightObject<Func<T1>>(L, lua_upvalueindex(2), false);
            var r = a();
            Lua.Push(L, r);
            return 1;
        }

        public static int Setter(lua_State L)
        {
            var a = Lua.ToLightObject<Action<T1>>(L, lua_upvalueindex(3), false);
            a(
                Lua.Get<T1>(L, 1)
            );
            return 0;
        }

    }

    public struct PropertyCaller<T1, T2>
    {
        public static int Getter(lua_State L)
        {
            var a = Lua.ToLightObject<Func<T1, T2>>(L, lua_upvalueindex(2), false);
            var p1 = SharpObject.Get<T1>(L, 1);
            var r = a(p1);
            Lua.Push(L, r);
            return 1;
        }

        public static int Setter(lua_State L, int start)
        {
            var a = Lua.ToLightObject<Action<T1, T2>>(L, lua_upvalueindex(3), false);
            a(
                Lua.Get<T1>(L, 1), Lua.Get<T2>(L, 2)
            );
            return 0;
        }

    }

    //低效率版本的field
    public struct FieldDelegate<V>
    {
        public static Func<V> Getter(FieldInfo fieldInfo)
        {
            return delegate ()
            {
                return (V)fieldInfo.GetValue(null);
            };
        }

        public static Action<V> Setter(FieldInfo fieldInfo)
        {
            return delegate (V v)
            {
                fieldInfo.SetValue(null, v);
            };
        }
    }

    public struct FieldDelegate<T, V>
    {
        public static Func<T,V> Getter(FieldInfo fieldInfo)
        {
            return delegate (T obj)
            {
                return (V)fieldInfo.GetValue(obj);
            };
        }

        public static Action<T, V> Setter(FieldInfo fieldInfo)
        {
            return delegate (T obj, V v)
            {
                fieldInfo.SetValue(obj, v);
            };
        }
    }


}
