
using System;
using System.Reflection;

namespace SharpLuna
{
    using static Lua;
    using lua_State = IntPtr;

    public struct FuncCaller<R>
    {
        public static int StaticCall(lua_State L) => Call(L);
        public static int Call(lua_State L)
        {         
            try
            {
                var a = ToLightObject<Func<R>>(L, lua_upvalueindex(1), false);
                var r = a();
                Push(L, r);
                return 1;
            }
            catch (Exception e)
            {
                return luaL_error(L, e.Message);
            }
        }
    }

    public struct FuncCaller<T1, R>
    {
        public static int Call(lua_State L) => Call(L, 1);
        public static int StaticCall(lua_State L) => Call(L, 2);

        static int Call(lua_State L, int start)
        {
            try
            {
                var a = ToLightObject<Func<T1, R>>(L, lua_upvalueindex(1), false);
                Get(L, 0 + start, out T1 t1);
                var r = a(t1);
                Push(L, r);
                return 1;
            }
            catch (Exception e)
            {
                return luaL_error(L, e.Message);
            }
        }
    }

    public struct FuncCaller<T1, T2, R>
    {
        public static int Call(lua_State L) => Call(L, 1);
        public static int StaticCall(lua_State L) => Call(L, 2);

        static int Call(lua_State L, int start)
        {
            try
            {
                var a = ToLightObject<Func<T1, T2, R>>(L, lua_upvalueindex(1), false);
                Get(L, 0 + start, out T1 t1);
                Get(L, 1 + start, out T2 t2);
                var r = a(t1, t2);
                Push(L, r);
                return 1;
            }
            catch (Exception e)
            {
                return luaL_error(L, e.Message);
            }
        }
    }

    public struct FuncCaller<T1, T2, T3, R>
    {
        public static int Call(lua_State L) => Call(L, 1);
        public static int StaticCall(lua_State L) => Call(L, 2);

        static int Call(lua_State L, int start)
        {
            try
            {
                var a = ToLightObject<Func<T1, T2, T3, R>>(L, lua_upvalueindex(1), false);
                Get(L, 0 + start, out T1 t1);
                Get(L, 1 + start, out T2 t2);
                Get(L, 2 + start, out T3 t3);
                var r = a(t1, t2, t3);
                Push(L, r);
                return 1;
            }
            catch (Exception e)
            {
                return luaL_error(L, e.Message);
            }
        }
    }

    public struct FuncCaller<T1, T2, T3, T4, R>
    {
        public static int Call(lua_State L) => Call(L, 1);
        public static int StaticCall(lua_State L) => Call(L, 2);

        static int Call(lua_State L, int start)
        {
            try
            {
                var a = ToLightObject<Func<T1, T2, T3, T4, R>>(L, lua_upvalueindex(1), false);
                Get(L, 0 + start, out T1 t1);
                Get(L, 1 + start, out T2 t2);
                Get(L, 2 + start, out T3 t3);
                Get(L, 3 + start, out T4 t4);
                var r = a(t1, t2, t3, t4);
                Push(L, r);
                return 1;
            }
            catch (Exception e)
            {
                return luaL_error(L, e.Message);
            }
        }
    }

    public struct FuncCaller<T1, T2, T3, T4, T5, R>
    {
        public static int Call(lua_State L) => Call(L, 1);
        public static int StaticCall(lua_State L) => Call(L, 2);

        static int Call(lua_State L, int start)
        {
            try
            {
                var a = ToLightObject<Func<T1, T2, T3, T4, T5, R>>(L, lua_upvalueindex(1), false);
                Get(L, 0 + start, out T1 t1);
                Get(L, 1 + start, out T2 t2);
                Get(L, 2 + start, out T3 t3);
                Get(L, 3 + start, out T4 t4);
                Get(L, 4 + start, out T5 t5);
                var r = a(t1, t2, t3, t4, t5);
                Push(L, r);
                return 1;
            }
            catch (Exception e)
            {
                return luaL_error(L, e.Message);
            }
        }
    }

    public struct FuncCaller<T1, T2, T3, T4, T5, T6, R>
    {
        public static int Call(lua_State L) => Call(L, 1);
        public static int StaticCall(lua_State L) => Call(L, 2);

        static int Call(lua_State L, int start)
        {
            try
            {
                var a = ToLightObject<Func<T1, T2, T3, T4, T5, T6, R>>(L, lua_upvalueindex(1), false);
                Get(L, 0 + start, out T1 t1);
                Get(L, 1 + start, out T2 t2);
                Get(L, 2 + start, out T3 t3);
                Get(L, 3 + start, out T4 t4);
                Get(L, 4 + start, out T5 t5);
                Get(L, 5 + start, out T6 t6);
                var r = a(t1, t2, t3, t4, t5, t6);
                Push(L, r);
                return 1;
            }
            catch (Exception e)
            {
                return luaL_error(L, e.Message);
            }
        }
    }

    public struct FuncCaller<T1, T2, T3, T4, T5, T6, T7, R>
    {
        public static int Call(lua_State L) => Call(L, 1);
        public static int StaticCall(lua_State L) => Call(L, 2);

        static int Call(lua_State L, int start)
        {
            try
            {
                var a = ToLightObject<Func<T1, T2, T3, T4, T5, T6, T7, R>>(L, lua_upvalueindex(1), false);
                Get(L, 0 + start, out T1 t1);
                Get(L, 1 + start, out T2 t2);
                Get(L, 2 + start, out T3 t3);
                Get(L, 3 + start, out T4 t4);
                Get(L, 4 + start, out T5 t5);
                Get(L, 5 + start, out T6 t6);
                Get(L, 6 + start, out T7 t7);
                var r = a(t1, t2, t3, t4, t5, t6, t7);
                Push(L, r);
                return 1;
            }
            catch (Exception e)
            {
                return luaL_error(L, e.Message);
            }
        }
    }

    public struct FuncCaller<T1, T2, T3, T4, T5, T6, T7, T8, R>
    {
        public static int Call(lua_State L) => Call(L, 1);
        public static int StaticCall(lua_State L) => Call(L, 2);

        static int Call(lua_State L, int start)
        {
            try
            {
                var a = ToLightObject<Func<T1, T2, T3, T4, T5, T6, T7, T8, R>>(L, lua_upvalueindex(1), false);
                Get(L, 0 + start, out T1 t1);
                Get(L, 1 + start, out T2 t2);
                Get(L, 2 + start, out T3 t3);
                Get(L, 3 + start, out T4 t4);
                Get(L, 4 + start, out T5 t5);
                Get(L, 5 + start, out T6 t6);
                Get(L, 6 + start, out T7 t7);
                Get(L, 7 + start, out T8 t8);
                var r = a(t1, t2, t3, t4, t5, t6, t7, t8);
                Push(L, r);
                return 1;
            }
            catch (Exception e)
            {
                return luaL_error(L, e.Message);
            }
        }
    }


}
