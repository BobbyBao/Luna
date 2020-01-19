
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
        [AOT.MonoPInvokeCallback(typeof(LuaNativeFunction))]
        public static int StaticCall(lua_State L) => Call(L);
        [AOT.MonoPInvokeCallback(typeof(LuaNativeFunction))]
        public static int Call(lua_State L)
        {         
            try
            {
                RefAction a = ToLightObject<RefAction>(L, lua_upvalueindex(1), false);
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
        [AOT.MonoPInvokeCallback(typeof(LuaNativeFunction))]
        public static int Call(lua_State L) => Call(L, 1);
        [AOT.MonoPInvokeCallback(typeof(LuaNativeFunction))]
        public static int StaticCall(lua_State L) => Call(L, 2);

        static int Call(lua_State L, int start)
        {
            try
            {
                var a = ToLightObject<RefAction<T1>>(L, lua_upvalueindex(1), false);
                a(ref SharpObject.GetValue<T1>(L, 0 + start));
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
        [AOT.MonoPInvokeCallback(typeof(LuaNativeFunction))]
        public static int Call(lua_State L) => Call(L, 1);
        [AOT.MonoPInvokeCallback(typeof(LuaNativeFunction))]
        public static int StaticCall(lua_State L) => Call(L, 2);

        static int Call(lua_State L, int start)
        {
            try
            {
                var a = ToLightObject<RefAction<T1, T2>>(L, lua_upvalueindex(1), false);
                Get(L, 1 + start, out T2 t2);
                a(ref SharpObject.GetValue<T1>(L, 0 + start), t2);
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
        [AOT.MonoPInvokeCallback(typeof(LuaNativeFunction))]
        public static int Call(lua_State L) => Call(L, 1);
        [AOT.MonoPInvokeCallback(typeof(LuaNativeFunction))]
        public static int StaticCall(lua_State L) => Call(L, 2);

        static int Call(lua_State L, int start)
        {
            try
            {
                var a = ToLightObject<RefAction<T1, T2, T3>>(L, lua_upvalueindex(1), false);
                Get(L, 1 + start, out T2 t2);
                Get(L, 2 + start, out T3 t3);
                a(ref SharpObject.GetValue<T1>(L, 0 + start), t2, t3);
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
        [AOT.MonoPInvokeCallback(typeof(LuaNativeFunction))]
        public static int Call(lua_State L) => Call(L, 1);
        [AOT.MonoPInvokeCallback(typeof(LuaNativeFunction))]
        public static int StaticCall(lua_State L) => Call(L, 2);

        static int Call(lua_State L, int start)
        {
            try
            {
                var a = ToLightObject<RefAction<T1, T2, T3, T4>>(L, lua_upvalueindex(1), false);
                Get(L, 1 + start, out T2 t2);
                Get(L, 2 + start, out T3 t3);
                Get(L, 3 + start, out T4 t4);
                a(ref SharpObject.GetValue<T1>(L, 0 + start), t2, t3, t4);
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
        [AOT.MonoPInvokeCallback(typeof(LuaNativeFunction))]
        public static int Call(lua_State L) => Call(L, 1);
        [AOT.MonoPInvokeCallback(typeof(LuaNativeFunction))]
        public static int StaticCall(lua_State L) => Call(L, 2);

        static int Call(lua_State L, int start)
        {
            try
            {
                var a = ToLightObject<RefAction<T1, T2, T3, T4, T5>>(L, lua_upvalueindex(1), false);
                Get(L, 1 + start, out T2 t2);
                Get(L, 2 + start, out T3 t3);
                Get(L, 3 + start, out T4 t4);
                Get(L, 4 + start, out T5 t5);
                a(ref SharpObject.GetValue<T1>(L, 0 + start), t2, t3, t4, t5);
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
        [AOT.MonoPInvokeCallback(typeof(LuaNativeFunction))]
        public static int Call(lua_State L) => Call(L, 1);
        [AOT.MonoPInvokeCallback(typeof(LuaNativeFunction))]
        public static int StaticCall(lua_State L) => Call(L, 2);

        static int Call(lua_State L, int start)
        {
            try
            {
                var a = ToLightObject<RefAction<T1, T2, T3, T4, T5, T6>>(L, lua_upvalueindex(1), false);
                Get(L, 1 + start, out T2 t2);
                Get(L, 2 + start, out T3 t3);
                Get(L, 3 + start, out T4 t4);
                Get(L, 4 + start, out T5 t5);
                Get(L, 5 + start, out T6 t6);
                a(ref SharpObject.GetValue<T1>(L, 0 + start), t2, t3, t4, t5, t6);
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
        [AOT.MonoPInvokeCallback(typeof(LuaNativeFunction))]
        public static int Call(lua_State L) => Call(L, 1);
        [AOT.MonoPInvokeCallback(typeof(LuaNativeFunction))]
        public static int StaticCall(lua_State L) => Call(L, 2);

        static int Call(lua_State L, int start)
        {
            try
            {
                var a = ToLightObject<RefAction<T1, T2, T3, T4, T5, T6, T7>>(L, lua_upvalueindex(1), false);
                Get(L, 1 + start, out T2 t2);
                Get(L, 2 + start, out T3 t3);
                Get(L, 3 + start, out T4 t4);
                Get(L, 4 + start, out T5 t5);
                Get(L, 5 + start, out T6 t6);
                Get(L, 6 + start, out T7 t7);
                a(ref SharpObject.GetValue<T1>(L, 0 + start), t2, t3, t4, t5, t6, t7);
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
        [AOT.MonoPInvokeCallback(typeof(LuaNativeFunction))]
        public static int Call(lua_State L) => Call(L, 1);
        [AOT.MonoPInvokeCallback(typeof(LuaNativeFunction))]
        public static int StaticCall(lua_State L) => Call(L, 2);

        static int Call(lua_State L, int start)
        {
            try
            {
                var a = ToLightObject<RefAction<T1, T2, T3, T4, T5, T6, T7, T8>>(L, lua_upvalueindex(1), false);
                Get(L, 1 + start, out T2 t2);
                Get(L, 2 + start, out T3 t3);
                Get(L, 3 + start, out T4 t4);
                Get(L, 4 + start, out T5 t5);
                Get(L, 5 + start, out T6 t6);
                Get(L, 6 + start, out T7 t7);
                Get(L, 7 + start, out T8 t8);
                a(ref SharpObject.GetValue<T1>(L, 0 + start), t2, t3, t4, t5, t6, t7, t8);
                return 0;
            }
            catch (Exception e)
            {
                return luaL_error(L, e.Message);
            }
        }
    }


}
