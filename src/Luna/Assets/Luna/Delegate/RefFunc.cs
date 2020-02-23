
using System;
using System.Reflection;

namespace SharpLuna
{
    using static Lua;
    using lua_State = IntPtr;

    public delegate R RefFunc<R>();
    public delegate R RefFunc<T1, R>(ref T1 t1) where T1 : struct;
    public delegate R RefFunc<T1, T2, R>(ref T1 t1, T2 t2) where T1 : struct;
    public delegate R RefFunc<T1, T2, T3, R>(ref T1 t1, T2 t2, T3 t3) where T1 : struct;
    public delegate R RefFunc<T1, T2, T3, T4, R>(ref T1 t1, T2 t2, T3 t3, T4 t4) where T1 : struct;
    public delegate R RefFunc<T1, T2, T3, T4, T5, R>(ref T1 t1, T2 t2, T3 t3, T4 t4, T5 t5) where T1 : struct;
    public delegate R RefFunc<T1, T2, T3, T4, T5, T6, R>(ref T1 t1, T2 t2, T3 t3, T4 t4, T5 t5, T6 t6) where T1 : struct;
    public delegate R RefFunc<T1, T2, T3, T4, T5, T6, T7, R>(ref T1 t1, T2 t2, T3 t3, T4 t4, T5 t5, T6 t6, T7 t7) where T1 : struct;
    public delegate R RefFunc<T1, T2, T3, T4, T5, T6, T7, T8, R>(ref T1 t1, T2 t2, T3 t3, T4 t4, T5 t5, T6 t6, T7 t7, T8 t8) where T1 : struct;

    public struct RefFuncCaller<R>
    {
        [AOT.MonoPInvokeCallback(typeof(LuaNativeFunction))]
        public static int StaticCall(lua_State L) => Call(L);
        [AOT.MonoPInvokeCallback(typeof(LuaNativeFunction))]
        public static int Call(lua_State L)
        {         
            try
            {
                var a = ToLightObject<RefFunc<R>>(L, lua_upvalueindex(1), false);
                var r = a();
                PushT(L, r);
                return 1;
            }
            catch (Exception e)
            {
                return luaL_error(L, e.Message);
            }
        }
    }

    public struct RefFuncCaller<T1, R> where T1 : struct
    {
        [AOT.MonoPInvokeCallback(typeof(LuaNativeFunction))]
        public static int Call(lua_State L) => Call(L, 1);
        [AOT.MonoPInvokeCallback(typeof(LuaNativeFunction))]
        public static int StaticCall(lua_State L) => Call(L, 2);

        static int Call(lua_State L, int start)
        {
            try
            {
                var a = ToLightObject<RefFunc<T1, R>>(L, lua_upvalueindex(1), false);
                var r = a(ref SharpObject.GetValue<T1>(L, 0 + start));
                PushT(L, r);
                return 1;
            }
            catch (Exception e)
            {
                return luaL_error(L, e.Message);
            }
        }
    }

    public struct RefFuncCaller<T1, T2, R> where T1 : struct
    {
        [AOT.MonoPInvokeCallback(typeof(LuaNativeFunction))]
        public static int Call(lua_State L) => Call(L, 1);
        [AOT.MonoPInvokeCallback(typeof(LuaNativeFunction))]
        public static int StaticCall(lua_State L) => Call(L, 2);

        static int Call(lua_State L, int start)
        {
            try
            {
                var a = ToLightObject<RefFunc<T1, T2, R>>(L, lua_upvalueindex(1), false);
                GetT(L, 1 + start, out T2 t2);
                var r = a(ref SharpObject.GetValue<T1>(L, 0 + start), t2);
                PushT(L, r);
                return 1;
            }
            catch (Exception e)
            {
                return luaL_error(L, e.Message);
            }
        }
    }

    public struct RefFuncCaller<T1, T2, T3, R> where T1 : struct
    {
        [AOT.MonoPInvokeCallback(typeof(LuaNativeFunction))]
        public static int Call(lua_State L) => Call(L, 1);
        [AOT.MonoPInvokeCallback(typeof(LuaNativeFunction))]
        public static int StaticCall(lua_State L) => Call(L, 2);

        static int Call(lua_State L, int start)
        {
            try
            {
                var a = ToLightObject<RefFunc<T1, T2, T3, R>>(L, lua_upvalueindex(1), false);
                GetT(L, 1 + start, out T2 t2);
                GetT(L, 2 + start, out T3 t3);
                var r = a(ref SharpObject.GetValue<T1>(L, 0 + start), t2, t3);
                PushT(L, r);
                return 1;
            }
            catch (Exception e)
            {
                return luaL_error(L, e.Message);
            }
        }
    }

    public struct RefFuncCaller<T1, T2, T3, T4, R> where T1 : struct
    {
        [AOT.MonoPInvokeCallback(typeof(LuaNativeFunction))]
        public static int Call(lua_State L) => Call(L, 1);
        [AOT.MonoPInvokeCallback(typeof(LuaNativeFunction))]
        public static int StaticCall(lua_State L) => Call(L, 2);

        static int Call(lua_State L, int start)
        {
            try
            {
                var a = ToLightObject<RefFunc<T1, T2, T3, T4, R>>(L, lua_upvalueindex(1), false);
                GetT(L, 1 + start, out T2 t2);
                GetT(L, 2 + start, out T3 t3);
                GetT(L, 3 + start, out T4 t4);
                var r = a(ref SharpObject.GetValue<T1>(L, 0 + start), t2, t3, t4);
                PushT(L, r);
                return 1;
            }
            catch (Exception e)
            {
                return luaL_error(L, e.Message);
            }
        }
    }

    public struct RefFuncCaller<T1, T2, T3, T4, T5, R> where T1 : struct
    {
        [AOT.MonoPInvokeCallback(typeof(LuaNativeFunction))]
        public static int Call(lua_State L) => Call(L, 1);
        [AOT.MonoPInvokeCallback(typeof(LuaNativeFunction))]
        public static int StaticCall(lua_State L) => Call(L, 2);

        static int Call(lua_State L, int start)
        {
            try
            {
                var a = ToLightObject<RefFunc<T1, T2, T3, T4, T5, R>>(L, lua_upvalueindex(1), false);
                GetT(L, 1 + start, out T2 t2);
                GetT(L, 2 + start, out T3 t3);
                GetT(L, 3 + start, out T4 t4);
                GetT(L, 4 + start, out T5 t5);
                var r = a(ref SharpObject.GetValue<T1>(L, 0 + start), t2, t3, t4, t5);
                PushT(L, r);
                return 1;
            }
            catch (Exception e)
            {
                return luaL_error(L, e.Message);
            }
        }
    }

    public struct RefFuncCaller<T1, T2, T3, T4, T5, T6, R> where T1 : struct
    {
        [AOT.MonoPInvokeCallback(typeof(LuaNativeFunction))]
        public static int Call(lua_State L) => Call(L, 1);
        [AOT.MonoPInvokeCallback(typeof(LuaNativeFunction))]
        public static int StaticCall(lua_State L) => Call(L, 2);

        static int Call(lua_State L, int start)
        {
            try
            {
                var a = ToLightObject<RefFunc<T1, T2, T3, T4, T5, T6, R>>(L, lua_upvalueindex(1), false);
                GetT(L, 1 + start, out T2 t2);
                GetT(L, 2 + start, out T3 t3);
                GetT(L, 3 + start, out T4 t4);
                GetT(L, 4 + start, out T5 t5);
                GetT(L, 5 + start, out T6 t6);
                var r = a(ref SharpObject.GetValue<T1>(L, 0 + start), t2, t3, t4, t5, t6);
                PushT(L, r);
                return 1;
            }
            catch (Exception e)
            {
                return luaL_error(L, e.Message);
            }
        }
    }

    public struct RefFuncCaller<T1, T2, T3, T4, T5, T6, T7, R> where T1 : struct
    {
        [AOT.MonoPInvokeCallback(typeof(LuaNativeFunction))]
        public static int Call(lua_State L) => Call(L, 1);
        [AOT.MonoPInvokeCallback(typeof(LuaNativeFunction))]
        public static int StaticCall(lua_State L) => Call(L, 2);

        static int Call(lua_State L, int start)
        {
            try
            {
                var a = ToLightObject<RefFunc<T1, T2, T3, T4, T5, T6, T7, R>>(L, lua_upvalueindex(1), false);
                GetT(L, 1 + start, out T2 t2);
                GetT(L, 2 + start, out T3 t3);
                GetT(L, 3 + start, out T4 t4);
                GetT(L, 4 + start, out T5 t5);
                GetT(L, 5 + start, out T6 t6);
                GetT(L, 6 + start, out T7 t7);
                var r = a(ref SharpObject.GetValue<T1>(L, 0 + start), t2, t3, t4, t5, t6, t7);
                PushT(L, r);
                return 1;
            }
            catch (Exception e)
            {
                return luaL_error(L, e.Message);
            }
        }
    }

    public struct RefFuncCaller<T1, T2, T3, T4, T5, T6, T7, T8, R> where T1 : struct
    {
        [AOT.MonoPInvokeCallback(typeof(LuaNativeFunction))]
        public static int Call(lua_State L) => Call(L, 1);
        [AOT.MonoPInvokeCallback(typeof(LuaNativeFunction))]
        public static int StaticCall(lua_State L) => Call(L, 2);

        static int Call(lua_State L, int start)
        {
            try
            {
                var a = ToLightObject<RefFunc<T1, T2, T3, T4, T5, T6, T7, T8, R>>(L, lua_upvalueindex(1), false);
                GetT(L, 1 + start, out T2 t2);
                GetT(L, 2 + start, out T3 t3);
                GetT(L, 3 + start, out T4 t4);
                GetT(L, 4 + start, out T5 t5);
                GetT(L, 5 + start, out T6 t6);
                GetT(L, 6 + start, out T7 t7);
                GetT(L, 7 + start, out T8 t8);
                var r = a(ref SharpObject.GetValue<T1>(L, 0 + start), t2, t3, t4, t5, t6, t7, t8);
                PushT(L, r);
                return 1;
            }
            catch (Exception e)
            {
                return luaL_error(L, e.Message);
            }
        }
    }


}
