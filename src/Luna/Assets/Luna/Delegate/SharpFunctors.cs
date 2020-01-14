using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;

namespace SharpLuna
{
    using static Lua;

    struct ConstantVariable
    {
        public static int Call(LuaState L)
        {
            lua_pushvalue(L, lua_upvalueindex(1));
            return 1;
        }
    }

    struct ClassConstructor<T> where T : new()
    {
        public static int Call(LuaState L)
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

        static int CallConstructor(LuaState L, int n)
        {
            try
            {
                ConstructorInfo fn = L.ToLightObject<ConstructorInfo>(lua_upvalueindex(1), false);
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

    struct ClassDestructor<T>
    {
        public static int Call(LuaState L)
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

    struct FieldDelegate<V>
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

    struct FieldDelegate<T, V>
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

    // 通用调用
    struct ActionCaller
    {
        public static int StaticCall(LuaState L) => Call(L);
        public static int Call(LuaState L)
        {
            try
            {
                Action a = L.ToLightObject<Action>(lua_upvalueindex(1), false);
                a();
                return 0;
            }
            catch (Exception e)
            {
                return luaL_error(L, e.Message);
            }
        }
    }

    struct ActionCaller<P1>
    {
        public static int Call(LuaState L) => Call(L, 0);
        public static int StaticCall(LuaState L) => Call(L, 1);

        static int Call(LuaState L, int start)
        {
            try
            {
                Action<P1> a = L.ToLightObject<Action<P1>>(lua_upvalueindex(1), false);
                P1 obj = Lua.Get<P1>(L, 1 + start);
                a(obj);
                return 0;
            }
            catch (Exception e)
            {
                return luaL_error(L, e.Message);
            }
        }
    }

    struct ActionCaller<P1, P2>
    {
        public static int Call(LuaState L) => Call(L, 0);
        public static int StaticCall(LuaState L) => Call(L, 1);

        static int Call(LuaState L, int start)
        {
            try
            {
                Action<P1, P2> a = L.ToLightObject<Action<P1, P2>>(lua_upvalueindex(1), false);
                var p1 = Lua.Get<P1>(L, 1 + start);
                var p2 = Lua.Get<P2>(L, 2 + start);
                a(p1, p2);
                return 0;
            }
            catch (Exception e)
            {
                return luaL_error(L, e.Message);
            }
        }
    }

    struct ActionCaller<P1, P2, P3>
    {
        public static int Call(LuaState L) => Call(L, 0);
        public static int StaticCall(LuaState L) => Call(L, 1);
        static int Call(LuaState L, int start)
        {
            try
            {
                Action<P1, P2, P3> a = L.ToLightObject<Action<P1, P2, P3>>(lua_upvalueindex(1), false);
                var p1 = Lua.Get<P1>(L, 1 + start);
                var p2 = Lua.Get<P2>(L, 2 + start);
                var p3 = Lua.Get<P3>(L, 3 + start);
                a(p1, p2, p3);
                return 0;
            }
            catch (Exception e)
            {
                return luaL_error(L, e.Message);
            }
        }
    }

    struct ActionCaller<P1, P2, P3, P4>
    {
        public static int Call(LuaState L) => Call(L, 0);
        public static int StaticCall(LuaState L) => Call(L, 1);

        static int Call(LuaState L, int start)
        {
            try
            {
                Action<P1, P2, P3, P4> a = L.ToLightObject<Action<P1, P2, P3, P4>>(lua_upvalueindex(1), false);
                var p1 = Lua.Get<P1>(L, 1 + start);
                var p2 = Lua.Get<P2>(L, 2 + start);
                var p3 = Lua.Get<P3>(L, 3 + start);
                var p4 = Lua.Get<P4>(L, 4 + start);
                a(p1, p2, p3, p4);
                return 0;
            }
            catch (Exception e)
            {
                return luaL_error(L, e.Message);
            }
        }
    }

    struct ActionCaller<P1, P2, P3, P4, P5>
    {
        public static int Call(LuaState L) => Call(L, 0);
        public static int StaticCall(LuaState L) => Call(L, 1);

        static int Call(LuaState L, int start)
        {
            try
            {
                Action<P1, P2, P3, P4, P5> a = L.ToLightObject<Action<P1, P2, P3, P4, P5>>(lua_upvalueindex(1), false);
                var p1 = Lua.Get<P1>(L, 1 + start);
                var p2 = Lua.Get<P2>(L, 2 + start);
                var p3 = Lua.Get<P3>(L, 3 + start);
                var p4 = Lua.Get<P4>(L, 4 + start);
                var p5 = Lua.Get<P5>(L, 5 + start);
                a(p1, p2, p3, p4, p5);
                return 0;
            }
            catch (Exception e)
            {
                return luaL_error(L, e.Message);
            }
        }
    }

    struct FuncCaller<R>
    {
        public static int StaticCall(LuaState L) => Call(L);
        public static int Call(LuaState L)
        {
            try
            {
                Func<R> fn = L.ToLightObject<Func<R>>(lua_upvalueindex(1), false);              
                var r = fn();
                Lua.Push(L, r);
                return 1;
            }
            catch (Exception e)
            {
                return luaL_error(L, e.Message);
            }
        }
    }

    struct FuncCaller<P1, R>
    {
        public static int Call(LuaState L) => Call(L, 0);
        public static int StaticCall(LuaState L) => Call(L, 1);
        static int Call(LuaState L, int start)
        {
            try
            {
                Func<P1, R> fn = L.ToLightObject<Func<P1, R>>(lua_upvalueindex(1), false);
                var p1 = Lua.Get<P1>(L, 1 + start);
                var r = fn(p1);
                Lua.Push(L, r);
                return 1;
            }
            catch (Exception e)
            {
                return luaL_error(L, e.Message);
            }
        }
    }

    struct FuncCaller<P1, P2, R>
    {
        public static int Call(LuaState L) => Call(L, 0);
        public static int StaticCall(LuaState L) => Call(L, 1);
        static int Call(LuaState L, int start)
        {
            try
            {
                Func<P1, P2, R> fn = L.ToLightObject<Func<P1, P2, R>>(lua_upvalueindex(1), false);
                var p1 = Lua.Get<P1>(L, 1 + start);
                var p2 = Lua.Get<P2>(L, 2 + start);
                var r = fn(p1, p2);
                Lua.Push(L, r);
                return 1;
            }
            catch (Exception e)
            {
                return luaL_error(L, e.Message);
            }
        }
    }

    struct FuncCaller<P1, P2, P3, R>
    {
        public static int Call(LuaState L) => Call(L, 0);
        public static int StaticCall(LuaState L) => Call(L, 1);
        static int Call(LuaState L, int start)
        {
            try
            {
                Func<P1, P2, P3, R> fn = L.ToLightObject<Func<P1, P2, P3, R>>(lua_upvalueindex(1), false);
                var p1 = Lua.Get<P1>(L, 1 + start);
                var p2 = Lua.Get<P2>(L, 2 + start);
                var p3 = Lua.Get<P3>(L, 3 + start);
                var r = fn(p1, p2, p3);
                Lua.Push(L, r);
                return 1;
            }
            catch (Exception e)
            {
                return luaL_error(L, e.Message);
            }
        }
    }

    struct FuncCaller<P1, P2, P3, P4, R>
    {
        public static int Call(LuaState L) => Call(L, 0);
        public static int StaticCall(LuaState L) => Call(L, 1);
        static int Call(LuaState L, int start)
        {
            try
            {
                Func<P1, P2, P3, P4, R> fn = L.ToLightObject<Func<P1, P2, P3, P4, R>>(lua_upvalueindex(1), false);
                var p1 = Lua.Get<P1>(L, 1 + start);
                var p2 = Lua.Get<P2>(L, 2 + start);
                var p3 = Lua.Get<P3>(L, 3 + start);
                var p4 = Lua.Get<P4>(L, 4 + start);
                var r = fn(p1, p2, p3, p4);
                Lua.Push(L, r);
                return 1;
            }
            catch (Exception e)
            {
                return luaL_error(L, e.Message);
            }
        }
    }

    struct FuncCaller<P1, P2, P3, P4, P5, R>
    {
        public static int Call(LuaState L) => Call(L, 0);
        public static int StaticCall(LuaState L) => Call(L, 1);
        static int Call(LuaState L, int start)
        {
            try
            {
                Func<P1, P2, P3, P4, P5, R> fn = L.ToLightObject<Func<P1, P2, P3, P4, P5, R>>(lua_upvalueindex(1), false);
                var p1 = Lua.Get<P1>(L, 1 + start);
                var p2 = Lua.Get<P2>(L, 2 + start);
                var p3 = Lua.Get<P3>(L, 3 + start);
                var p4 = Lua.Get<P4>(L, 4 + start);
                var p5 = Lua.Get<P5>(L, 5 + start);
                var r = fn(p1, p2, p3, p4, p5);
                Lua.Push(L, r);
                return 1;
            }
            catch (Exception e)
            {
                return luaL_error(L, e.Message);
            }
        }
    }


}
