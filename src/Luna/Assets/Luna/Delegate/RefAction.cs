
using System;
using System.Reflection;

namespace SharpLuna
{
    using static Lua;
    using lua_State = IntPtr;

    public delegate void RefAction();
    public delegate void RefAction<T1>(ref T1 t1) where T1 : struct;
    public delegate void RefAction<T1, T2>(ref T1 t1, T2 t2) where T1 : struct;
    public delegate void RefAction<T1, T2, T3>(ref T1 t1, T2 t2, T3 t3) where T1 : struct;
    public delegate void RefAction<T1, T2, T3, T4>(ref T1 t1, T2 t2, T3 t3, T4 t4) where T1 : struct;
    public delegate void RefAction<T1, T2, T3, T4, T5>(ref T1 t1, T2 t2, T3 t3, T4 t4, T5 t5) where T1 : struct;
    public delegate void RefAction<T1, T2, T3, T4, T5, T6>(ref T1 t1, T2 t2, T3 t3, T4 t4, T5 t5, T6 t6) where T1 : struct;
    public delegate void RefAction<T1, T2, T3, T4, T5, T6, T7>(ref T1 t1, T2 t2, T3 t3, T4 t4, T5 t5, T6 t6, T7 t7) where T1 : struct;
    public delegate void RefAction<T1, T2, T3, T4, T5, T6, T7, T8>(ref T1 t1, T2 t2, T3 t3, T4 t4, T5 t5, T6 t6, T7 t7, T8 t8) where T1 : struct;

    public struct RefActionCaller
    {
        public static int StaticCall(lua_State L) => Call(L);
        public static int Call(lua_State L)
        {         
            try
            {
                RefAction a = Lua.ToLightObject<RefAction>(L, lua_upvalueindex(1), false);
                a();
                return 0;
            }
            catch (Exception e)
            {
                return luaL_error(L, e.Message);
            }
        }
    }

    public struct RefActionCaller<T1> where T1 : struct
    {
        public static int Call(lua_State L) => Call(L, 1);
        public static int StaticCall(lua_State L) => Call(L, 2);

        static int Call(lua_State L, int start)
        {
            try
            {
                var a = Lua.ToLightObject<RefAction<T1>>(L, lua_upvalueindex(1), false);
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

    public struct RefActionCaller<T1, T2> where T1 : struct
    {
        public static int Call(lua_State L) => Call(L, 1);
        public static int StaticCall(lua_State L) => Call(L, 2);

        static int Call(lua_State L, int start)
        {
            try
            {
                var a = Lua.ToLightObject<RefAction<T1, T2>>(L, lua_upvalueindex(1), false);
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

    public struct RefActionCaller<T1, T2, T3> where T1 : struct
    {
        public static int Call(lua_State L) => Call(L, 1);
        public static int StaticCall(lua_State L) => Call(L, 2);

        static int Call(lua_State L, int start)
        {
            try
            {
                var a = Lua.ToLightObject<RefAction<T1, T2, T3>>(L, lua_upvalueindex(1), false);
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

    public struct RefActionCaller<T1, T2, T3, T4> where T1 : struct
    {
        public static int Call(lua_State L) => Call(L, 1);
        public static int StaticCall(lua_State L) => Call(L, 2);

        static int Call(lua_State L, int start)
        {
            try
            {
                var a = Lua.ToLightObject<RefAction<T1, T2, T3, T4>>(L, lua_upvalueindex(1), false);
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

    public struct RefActionCaller<T1, T2, T3, T4, T5> where T1 : struct
    {
        public static int Call(lua_State L) => Call(L, 1);
        public static int StaticCall(lua_State L) => Call(L, 2);

        static int Call(lua_State L, int start)
        {
            try
            {
                var a = Lua.ToLightObject<RefAction<T1, T2, T3, T4, T5>>(L, lua_upvalueindex(1), false);
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

    public struct RefActionCaller<T1, T2, T3, T4, T5, T6> where T1 : struct
    {
        public static int Call(lua_State L) => Call(L, 1);
        public static int StaticCall(lua_State L) => Call(L, 2);

        static int Call(lua_State L, int start)
        {
            try
            {
                var a = Lua.ToLightObject<RefAction<T1, T2, T3, T4, T5, T6>>(L, lua_upvalueindex(1), false);
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

    public struct RefActionCaller<T1, T2, T3, T4, T5, T6, T7> where T1 : struct
    {
        public static int Call(lua_State L) => Call(L, 1);
        public static int StaticCall(lua_State L) => Call(L, 2);

        static int Call(lua_State L, int start)
        {
            try
            {
                var a = Lua.ToLightObject<RefAction<T1, T2, T3, T4, T5, T6, T7>>(L, lua_upvalueindex(1), false);
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

    public struct RefActionCaller<T1, T2, T3, T4, T5, T6, T7, T8> where T1 : struct
    {
        public static int Call(lua_State L) => Call(L, 1);
        public static int StaticCall(lua_State L) => Call(L, 2);

        static int Call(lua_State L, int start)
        {
            try
            {
                var a = Lua.ToLightObject<RefAction<T1, T2, T3, T4, T5, T6, T7, T8>>(L, lua_upvalueindex(1), false);
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
