
using System;
using System.Reflection;

namespace SharpLuna
{
    using static Lua;

    public delegate void RefAction();
    public delegate void RefAction<T1>(ref T1 t1);
    public delegate void RefAction<T1, T2>(ref T1 t1, T2 t2);
    public delegate void RefAction<T1, T2, T3>(ref T1 t1, T2 t2, T3 t3);
    public delegate void RefAction<T1, T2, T3, T4>(ref T1 t1, T2 t2, T3 t3, T4 t4);
    public delegate void RefAction<T1, T2, T3, T4, T5>(ref T1 t1, T2 t2, T3 t3, T4 t4, T5 t5);
    public delegate void RefAction<T1, T2, T3, T4, T5, T6>(ref T1 t1, T2 t2, T3 t3, T4 t4, T5 t5, T6 t6);
    public delegate void RefAction<T1, T2, T3, T4, T5, T6, T7>(ref T1 t1, T2 t2, T3 t3, T4 t4, T5 t5, T6 t6, T7 t7);
    public delegate void RefAction<T1, T2, T3, T4, T5, T6, T7, T8>(ref T1 t1, T2 t2, T3 t3, T4 t4, T5 t5, T6 t6, T7 t7, T8 t8);

    public struct RefActionCaller
    {
        public static int StaticCall(LuaState L) => Call(L);
        public static int Call(LuaState L)
        {         
            try
            {
                RefAction a = L.ToLightObject<RefAction>(lua_upvalueindex(1), false);
                a();
                return 0;
            }
            catch (Exception e)
            {
                return luaL_error(L, e.Message);
            }
        }
    }

    public struct RefActionCaller<T1>
    {
        public static int Call(LuaState L) => Call(L, 1);
        public static int StaticCall(LuaState L) => Call(L, 2);

        static int Call(LuaState L, int start)
        {
            try
            {
                var a = L.ToLightObject<RefAction<T1>>(lua_upvalueindex(1), false);
                a(
                    ref SharpObject.GetValue<T1>(L, 0 + start)
                );
                return 0;
            }
            catch (Exception e)
            {
                return luaL_error(L, e.Message);
            }
        }
    }

    public struct RefActionCaller<T1, T2>
    {
        public static int Call(LuaState L) => Call(L, 1);
        public static int StaticCall(LuaState L) => Call(L, 2);

        static int Call(LuaState L, int start)
        {
            try
            {
                var a = L.ToLightObject<RefAction<T1, T2>>(lua_upvalueindex(1), false);
                a(
                    ref SharpObject.GetValue<T1>(L, 0 + start),
                    Lua.Get<T2>(L, 1 + start)
                );
                return 0;
            }
            catch (Exception e)
            {
                return luaL_error(L, e.Message);
            }
        }
    }

    public struct RefActionCaller<T1, T2, T3>
    {
        public static int Call(LuaState L) => Call(L, 1);
        public static int StaticCall(LuaState L) => Call(L, 2);

        static int Call(LuaState L, int start)
        {
            try
            {
                var a = L.ToLightObject<RefAction<T1, T2, T3>>(lua_upvalueindex(1), false);
                a(
                    ref SharpObject.GetValue<T1>(L, 0 + start),
                    Lua.Get<T2>(L, 1 + start),
                    Lua.Get<T3>(L, 2 + start)
                );
                return 0;
            }
            catch (Exception e)
            {
                return luaL_error(L, e.Message);
            }
        }
    }

    public struct RefActionCaller<T1, T2, T3, T4>
    {
        public static int Call(LuaState L) => Call(L, 1);
        public static int StaticCall(LuaState L) => Call(L, 2);

        static int Call(LuaState L, int start)
        {
            try
            {
                var a = L.ToLightObject<RefAction<T1, T2, T3, T4>>(lua_upvalueindex(1), false);
                a(
                    ref SharpObject.GetValue<T1>(L, 0 + start),
                    Lua.Get<T2>(L, 1 + start),
                    Lua.Get<T3>(L, 2 + start),
                    Lua.Get<T4>(L, 3 + start)
                );
                return 0;
            }
            catch (Exception e)
            {
                return luaL_error(L, e.Message);
            }
        }
    }

    public struct RefActionCaller<T1, T2, T3, T4, T5>
    {
        public static int Call(LuaState L) => Call(L, 1);
        public static int StaticCall(LuaState L) => Call(L, 2);

        static int Call(LuaState L, int start)
        {
            try
            {
                var a = L.ToLightObject<RefAction<T1, T2, T3, T4, T5>>(lua_upvalueindex(1), false);
                a(
                    ref SharpObject.GetValue<T1>(L, 0 + start),
                    Lua.Get<T2>(L, 1 + start),
                    Lua.Get<T3>(L, 2 + start),
                    Lua.Get<T4>(L, 3 + start),
                    Lua.Get<T5>(L, 4 + start)
                );
                return 0;
            }
            catch (Exception e)
            {
                return luaL_error(L, e.Message);
            }
        }
    }

    public struct RefActionCaller<T1, T2, T3, T4, T5, T6>
    {
        public static int Call(LuaState L) => Call(L, 1);
        public static int StaticCall(LuaState L) => Call(L, 2);

        static int Call(LuaState L, int start)
        {
            try
            {
                var a = L.ToLightObject<RefAction<T1, T2, T3, T4, T5, T6>>(lua_upvalueindex(1), false);
                a(
                    ref SharpObject.GetValue<T1>(L, 0 + start),
                    Lua.Get<T2>(L, 1 + start),
                    Lua.Get<T3>(L, 2 + start),
                    Lua.Get<T4>(L, 3 + start),
                    Lua.Get<T5>(L, 4 + start),
                    Lua.Get<T6>(L, 5 + start)
                );
                return 0;
            }
            catch (Exception e)
            {
                return luaL_error(L, e.Message);
            }
        }
    }

    public struct RefActionCaller<T1, T2, T3, T4, T5, T6, T7>
    {
        public static int Call(LuaState L) => Call(L, 1);
        public static int StaticCall(LuaState L) => Call(L, 2);

        static int Call(LuaState L, int start)
        {
            try
            {
                var a = L.ToLightObject<RefAction<T1, T2, T3, T4, T5, T6, T7>>(lua_upvalueindex(1), false);
                a(
                    ref SharpObject.GetValue<T1>(L, 0 + start),
                    Lua.Get<T2>(L, 1 + start),
                    Lua.Get<T3>(L, 2 + start),
                    Lua.Get<T4>(L, 3 + start),
                    Lua.Get<T5>(L, 4 + start),
                    Lua.Get<T6>(L, 5 + start),
                    Lua.Get<T7>(L, 6 + start)
                );
                return 0;
            }
            catch (Exception e)
            {
                return luaL_error(L, e.Message);
            }
        }
    }

    public struct RefActionCaller<T1, T2, T3, T4, T5, T6, T7, T8>
    {
        public static int Call(LuaState L) => Call(L, 1);
        public static int StaticCall(LuaState L) => Call(L, 2);

        static int Call(LuaState L, int start)
        {
            try
            {
                var a = L.ToLightObject<RefAction<T1, T2, T3, T4, T5, T6, T7, T8>>(lua_upvalueindex(1), false);
                a(
                    ref SharpObject.GetValue<T1>(L, 0 + start),
                    Lua.Get<T2>(L, 1 + start),
                    Lua.Get<T3>(L, 2 + start),
                    Lua.Get<T4>(L, 3 + start),
                    Lua.Get<T5>(L, 4 + start),
                    Lua.Get<T6>(L, 5 + start),
                    Lua.Get<T7>(L, 6 + start),
                    Lua.Get<T8>(L, 7 + start)
                );
                return 0;
            }
            catch (Exception e)
            {
                return luaL_error(L, e.Message);
            }
        }
    }


}
