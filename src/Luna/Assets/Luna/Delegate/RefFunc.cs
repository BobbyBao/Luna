
using System;
using System.Reflection;

namespace SharpLuna
{
    using static Lua;

    public delegate R RefFunc<R>();
    public delegate R RefFunc<T1, R>(ref T1 t1);
    public delegate R RefFunc<T1, T2, R>(ref T1 t1, T2 t2);
    public delegate R RefFunc<T1, T2, T3, R>(ref T1 t1, T2 t2, T3 t3);
    public delegate R RefFunc<T1, T2, T3, T4, R>(ref T1 t1, T2 t2, T3 t3, T4 t4);
    public delegate R RefFunc<T1, T2, T3, T4, T5, R>(ref T1 t1, T2 t2, T3 t3, T4 t4, T5 t5);
    public delegate R RefFunc<T1, T2, T3, T4, T5, T6, R>(ref T1 t1, T2 t2, T3 t3, T4 t4, T5 t5, T6 t6);
    public delegate R RefFunc<T1, T2, T3, T4, T5, T6, T7, R>(ref T1 t1, T2 t2, T3 t3, T4 t4, T5 t5, T6 t6, T7 t7);
    public delegate R RefFunc<T1, T2, T3, T4, T5, T6, T7, T8, R>(ref T1 t1, T2 t2, T3 t3, T4 t4, T5 t5, T6 t6, T7 t7, T8 t8);

    public struct RefFuncCaller<R>
    {
        public static int StaticCall(LuaState L) => Call(L);
        public static int Call(LuaState L)
        {         
            try
            {
                var a = L.ToLightObject<RefFunc<R>>(lua_upvalueindex(1), false);
                var r = a();
                Lua.Push(L, r);
                return 1;
            }
            catch (Exception e)
            {
                return luaL_error(L, e.Message);
            }
        }
    }

    public struct RefFuncCaller<T1, R>
    {
        public static int Call(LuaState L) => Call(L, 1);
        public static int StaticCall(LuaState L) => Call(L, 2);

        static int Call(LuaState L, int start)
        {
            try
            {
                var a = L.ToLightObject<RefFunc<T1, R>>(lua_upvalueindex(1), false);
                var r = a(
                    ref SharpObject.GetValue<T1>(L, 0 + start)
                );
                Lua.Push(L, r);
                return 1;
            }
            catch (Exception e)
            {
                return luaL_error(L, e.Message);
            }
        }
    }

    public struct RefFuncCaller<T1, T2, R>
    {
        public static int Call(LuaState L) => Call(L, 1);
        public static int StaticCall(LuaState L) => Call(L, 2);

        static int Call(LuaState L, int start)
        {
            try
            {
                var a = L.ToLightObject<RefFunc<T1, T2, R>>(lua_upvalueindex(1), false);
                var r = a(
                    ref SharpObject.GetValue<T1>(L, 0 + start),
                    Lua.Get<T2>(L, 1 + start)
                );
                Lua.Push(L, r);
                return 1;
            }
            catch (Exception e)
            {
                return luaL_error(L, e.Message);
            }
        }
    }

    public struct RefFuncCaller<T1, T2, T3, R>
    {
        public static int Call(LuaState L) => Call(L, 1);
        public static int StaticCall(LuaState L) => Call(L, 2);

        static int Call(LuaState L, int start)
        {
            try
            {
                var a = L.ToLightObject<RefFunc<T1, T2, T3, R>>(lua_upvalueindex(1), false);
                var r = a(
                    ref SharpObject.GetValue<T1>(L, 0 + start),
                    Lua.Get<T2>(L, 1 + start),
                    Lua.Get<T3>(L, 2 + start)
                );
                Lua.Push(L, r);
                return 1;
            }
            catch (Exception e)
            {
                return luaL_error(L, e.Message);
            }
        }
    }

    public struct RefFuncCaller<T1, T2, T3, T4, R>
    {
        public static int Call(LuaState L) => Call(L, 1);
        public static int StaticCall(LuaState L) => Call(L, 2);

        static int Call(LuaState L, int start)
        {
            try
            {
                var a = L.ToLightObject<RefFunc<T1, T2, T3, T4, R>>(lua_upvalueindex(1), false);
                var r = a(
                    ref SharpObject.GetValue<T1>(L, 0 + start),
                    Lua.Get<T2>(L, 1 + start),
                    Lua.Get<T3>(L, 2 + start),
                    Lua.Get<T4>(L, 3 + start)
                );
                Lua.Push(L, r);
                return 1;
            }
            catch (Exception e)
            {
                return luaL_error(L, e.Message);
            }
        }
    }

    public struct RefFuncCaller<T1, T2, T3, T4, T5, R>
    {
        public static int Call(LuaState L) => Call(L, 1);
        public static int StaticCall(LuaState L) => Call(L, 2);

        static int Call(LuaState L, int start)
        {
            try
            {
                var a = L.ToLightObject<RefFunc<T1, T2, T3, T4, T5, R>>(lua_upvalueindex(1), false);
                var r = a(
                    ref SharpObject.GetValue<T1>(L, 0 + start),
                    Lua.Get<T2>(L, 1 + start),
                    Lua.Get<T3>(L, 2 + start),
                    Lua.Get<T4>(L, 3 + start),
                    Lua.Get<T5>(L, 4 + start)
                );
                Lua.Push(L, r);
                return 1;
            }
            catch (Exception e)
            {
                return luaL_error(L, e.Message);
            }
        }
    }

    public struct RefFuncCaller<T1, T2, T3, T4, T5, T6, R>
    {
        public static int Call(LuaState L) => Call(L, 1);
        public static int StaticCall(LuaState L) => Call(L, 2);

        static int Call(LuaState L, int start)
        {
            try
            {
                var a = L.ToLightObject<RefFunc<T1, T2, T3, T4, T5, T6, R>>(lua_upvalueindex(1), false);
                var r = a(
                    ref SharpObject.GetValue<T1>(L, 0 + start),
                    Lua.Get<T2>(L, 1 + start),
                    Lua.Get<T3>(L, 2 + start),
                    Lua.Get<T4>(L, 3 + start),
                    Lua.Get<T5>(L, 4 + start),
                    Lua.Get<T6>(L, 5 + start)
                );
                Lua.Push(L, r);
                return 1;
            }
            catch (Exception e)
            {
                return luaL_error(L, e.Message);
            }
        }
    }

    public struct RefFuncCaller<T1, T2, T3, T4, T5, T6, T7, R>
    {
        public static int Call(LuaState L) => Call(L, 1);
        public static int StaticCall(LuaState L) => Call(L, 2);

        static int Call(LuaState L, int start)
        {
            try
            {
                var a = L.ToLightObject<RefFunc<T1, T2, T3, T4, T5, T6, T7, R>>(lua_upvalueindex(1), false);
                var r = a(
                    ref SharpObject.GetValue<T1>(L, 0 + start),
                    Lua.Get<T2>(L, 1 + start),
                    Lua.Get<T3>(L, 2 + start),
                    Lua.Get<T4>(L, 3 + start),
                    Lua.Get<T5>(L, 4 + start),
                    Lua.Get<T6>(L, 5 + start),
                    Lua.Get<T7>(L, 6 + start)
                );
                Lua.Push(L, r);
                return 1;
            }
            catch (Exception e)
            {
                return luaL_error(L, e.Message);
            }
        }
    }

    public struct RefFuncCaller<T1, T2, T3, T4, T5, T6, T7, T8, R>
    {
        public static int Call(LuaState L) => Call(L, 1);
        public static int StaticCall(LuaState L) => Call(L, 2);

        static int Call(LuaState L, int start)
        {
            try
            {
                var a = L.ToLightObject<RefFunc<T1, T2, T3, T4, T5, T6, T7, T8, R>>(lua_upvalueindex(1), false);
                var r = a(
                    ref SharpObject.GetValue<T1>(L, 0 + start),
                    Lua.Get<T2>(L, 1 + start),
                    Lua.Get<T3>(L, 2 + start),
                    Lua.Get<T4>(L, 3 + start),
                    Lua.Get<T5>(L, 4 + start),
                    Lua.Get<T6>(L, 5 + start),
                    Lua.Get<T7>(L, 6 + start),
                    Lua.Get<T8>(L, 7 + start)
                );
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
