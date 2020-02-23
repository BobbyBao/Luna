﻿
using System;
using System.Reflection;

namespace SharpLuna
{
    using static Lua;
    using lua_State = IntPtr;

    public struct FuncCaller<R>
    {
        [AOT.MonoPInvokeCallback(typeof(LuaNativeFunction))]
        public static int StaticCall(lua_State L) => Call(L);
        [AOT.MonoPInvokeCallback(typeof(LuaNativeFunction))]
        public static int Call(lua_State L)
        {         
            try
            {
                var a = ToLightObject<Func<R>>(L, lua_upvalueindex(1), false);
                var r = a();
                PushT(L, r);
                return 1;
            }
            catch (Exception e)
            {
                return luaL_error(L, e.Message);
            }
        }

        public static int CallDel(lua_State L, int start, Delegate del)
        {
            Func<R> a = (Func<R>)del;
            var r = a();
            PushT(L, r);
            return 1;
        }
    }

    public struct FuncCaller<T1, R>
    {
        [AOT.MonoPInvokeCallback(typeof(LuaNativeFunction))]
        public static int Call(lua_State L) => Call(L, 1);
        [AOT.MonoPInvokeCallback(typeof(LuaNativeFunction))]
        public static int StaticCall(lua_State L) => Call(L, 2);

        static int Call(lua_State L, int start)
        {
            try
            {
                var a = ToLightObject<Func<T1, R>>(L, lua_upvalueindex(1), false);
                return CallDel(L, start, a);
            }
            catch (Exception e)
            {
                return luaL_error(L, e.Message);
            }
        }

        public static int CallDel(lua_State L, int start, Delegate del)
        {
            var a = (Func<T1, R>)del;
            GetT(L, 0 + start, out T1 t1);
            var r = a(t1);
            PushT(L, r);
            return 1;
        }
    }

    public struct FuncCaller<T1, T2, R>
    {
        [AOT.MonoPInvokeCallback(typeof(LuaNativeFunction))]
        public static int Call(lua_State L) => Call(L, 1);
        [AOT.MonoPInvokeCallback(typeof(LuaNativeFunction))]
        public static int StaticCall(lua_State L) => Call(L, 2);

        static int Call(lua_State L, int start)
        {
            try
            {
                var a = ToLightObject<Func<T1, T2, R>>(L, lua_upvalueindex(1), false);
                return CallDel(L, start, a);
            }
            catch (Exception e)
            {
                return luaL_error(L, e.Message);
            }
        }

        public static int CallDel(lua_State L, int start, Delegate del)
        {
            var a = (Func<T1, T2, R>)del;
            GetT(L, 0 + start, out T1 t1);
            GetT(L, 1 + start, out T2 t2);
            var r = a(t1, t2);
            PushT(L, r);
            return 1;
        }
    }

    public struct FuncCaller<T1, T2, T3, R>
    {
        [AOT.MonoPInvokeCallback(typeof(LuaNativeFunction))]
        public static int Call(lua_State L) => Call(L, 1);
        [AOT.MonoPInvokeCallback(typeof(LuaNativeFunction))]
        public static int StaticCall(lua_State L) => Call(L, 2);

        static int Call(lua_State L, int start)
        {
            try
            {
                var a = ToLightObject<Func<T1, T2, T3, R>>(L, lua_upvalueindex(1), false);
                return CallDel(L, start, a);
            }
            catch (Exception e)
            {
                return luaL_error(L, e.Message);
            }
        }

        public static int CallDel(lua_State L, int start, Delegate del)
        {
            var a = (Func<T1, T2, T3, R>)del;
            GetT(L, 0 + start, out T1 t1);
            GetT(L, 1 + start, out T2 t2);
            GetT(L, 2 + start, out T3 t3);
            var r = a(t1, t2, t3);
            PushT(L, r);
            return 1;
        }
    }

    public struct FuncCaller<T1, T2, T3, T4, R>
    {
        [AOT.MonoPInvokeCallback(typeof(LuaNativeFunction))]
        public static int Call(lua_State L) => Call(L, 1);
        [AOT.MonoPInvokeCallback(typeof(LuaNativeFunction))]
        public static int StaticCall(lua_State L) => Call(L, 2);

        static int Call(lua_State L, int start)
        {
            try
            {
                var a = ToLightObject<Func<T1, T2, T3, T4, R>>(L, lua_upvalueindex(1), false);
                return CallDel(L, start, a);
            }
            catch (Exception e)
            {
                return luaL_error(L, e.Message);
            }
        }

        public static int CallDel(lua_State L, int start, Delegate del)
        {
            var a = (Func<T1, T2, T3, T4, R>)del;
            GetT(L, 0 + start, out T1 t1);
            GetT(L, 1 + start, out T2 t2);
            GetT(L, 2 + start, out T3 t3);
            GetT(L, 3 + start, out T4 t4);
            var r = a(t1, t2, t3, t4);
            PushT(L, r);
            return 1;
        }
    }

    public struct FuncCaller<T1, T2, T3, T4, T5, R>
    {
        [AOT.MonoPInvokeCallback(typeof(LuaNativeFunction))]
        public static int Call(lua_State L) => Call(L, 1);
        [AOT.MonoPInvokeCallback(typeof(LuaNativeFunction))]
        public static int StaticCall(lua_State L) => Call(L, 2);

        static int Call(lua_State L, int start)
        {
            try
            {
                var a = ToLightObject<Func<T1, T2, T3, T4, T5, R>>(L, lua_upvalueindex(1), false);
                return CallDel(L, start, a);
            }
            catch (Exception e)
            {
                return luaL_error(L, e.Message);
            }
        }

        public static int CallDel(lua_State L, int start, Delegate del)
        {
            var a = (Func<T1, T2, T3, T4, T5, R>)del;
            GetT(L, 0 + start, out T1 t1);
            GetT(L, 1 + start, out T2 t2);
            GetT(L, 2 + start, out T3 t3);
            GetT(L, 3 + start, out T4 t4);
            GetT(L, 4 + start, out T5 t5);
            var r = a(t1, t2, t3, t4, t5);
            PushT(L, r);
            return 1;
        }
    }

    public struct FuncCaller<T1, T2, T3, T4, T5, T6, R>
    {
        [AOT.MonoPInvokeCallback(typeof(LuaNativeFunction))]
        public static int Call(lua_State L) => Call(L, 1);
        [AOT.MonoPInvokeCallback(typeof(LuaNativeFunction))]
        public static int StaticCall(lua_State L) => Call(L, 2);

        static int Call(lua_State L, int start)
        {
            try
            {
                var a = ToLightObject<Func<T1, T2, T3, T4, T5, T6, R>>(L, lua_upvalueindex(1), false);
                return CallDel(L, start, a);
            }
            catch (Exception e)
            {
                return luaL_error(L, e.Message);
            }
        }

        public static int CallDel(lua_State L, int start, Delegate del)
        {
            var a = (Func<T1, T2, T3, T4, T5, T6, R>)del;
            GetT(L, 0 + start, out T1 t1);
            GetT(L, 1 + start, out T2 t2);
            GetT(L, 2 + start, out T3 t3);
            GetT(L, 3 + start, out T4 t4);
            GetT(L, 4 + start, out T5 t5);
            GetT(L, 5 + start, out T6 t6);
            var r = a(t1, t2, t3, t4, t5, t6);
            PushT(L, r);
            return 1;
        }
    }

    public struct FuncCaller<T1, T2, T3, T4, T5, T6, T7, R>
    {
        [AOT.MonoPInvokeCallback(typeof(LuaNativeFunction))]
        public static int Call(lua_State L) => Call(L, 1);
        [AOT.MonoPInvokeCallback(typeof(LuaNativeFunction))]
        public static int StaticCall(lua_State L) => Call(L, 2);

        static int Call(lua_State L, int start)
        {
            try
            {
                var a = ToLightObject<Func<T1, T2, T3, T4, T5, T6, T7, R>>(L, lua_upvalueindex(1), false);
                return CallDel(L, start, a);
            }
            catch (Exception e)
            {
                return luaL_error(L, e.Message);
            }
        }

        public static int CallDel(lua_State L, int start, Delegate del)
        {
            var a = (Func<T1, T2, T3, T4, T5, T6, T7, R>)del;
            GetT(L, 0 + start, out T1 t1);
            GetT(L, 1 + start, out T2 t2);
            GetT(L, 2 + start, out T3 t3);
            GetT(L, 3 + start, out T4 t4);
            GetT(L, 4 + start, out T5 t5);
            GetT(L, 5 + start, out T6 t6);
            GetT(L, 6 + start, out T7 t7);
            var r = a(t1, t2, t3, t4, t5, t6, t7);
            PushT(L, r);
            return 1;
        }
    }

    public struct FuncCaller<T1, T2, T3, T4, T5, T6, T7, T8, R>
    {
        [AOT.MonoPInvokeCallback(typeof(LuaNativeFunction))]
        public static int Call(lua_State L) => Call(L, 1);
        [AOT.MonoPInvokeCallback(typeof(LuaNativeFunction))]
        public static int StaticCall(lua_State L) => Call(L, 2);

        static int Call(lua_State L, int start)
        {
            try
            {
                var a = ToLightObject<Func<T1, T2, T3, T4, T5, T6, T7, T8, R>>(L, lua_upvalueindex(1), false);
                return CallDel(L, start, a);
            }
            catch (Exception e)
            {
                return luaL_error(L, e.Message);
            }
        }

        public static int CallDel(lua_State L, int start, Delegate del)
        {
            var a = (Func<T1, T2, T3, T4, T5, T6, T7, T8, R>)del;
            GetT(L, 0 + start, out T1 t1);
            GetT(L, 1 + start, out T2 t2);
            GetT(L, 2 + start, out T3 t3);
            GetT(L, 3 + start, out T4 t4);
            GetT(L, 4 + start, out T5 t5);
            GetT(L, 5 + start, out T6 t6);
            GetT(L, 6 + start, out T7 t7);
            GetT(L, 7 + start, out T8 t8);
            var r = a(t1, t2, t3, t4, t5, t6, t7, t8);
            PushT(L, r);
            return 1;
        }
    }


}
