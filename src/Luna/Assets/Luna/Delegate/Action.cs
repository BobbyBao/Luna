﻿
using System;
using System.Reflection;

namespace SharpLuna
{
    using static Lua;
    using lua_State = IntPtr;

    public struct ActionCaller
    {
        [AOT.MonoPInvokeCallback(typeof(LuaNativeFunction))]
        public static int StaticCall(lua_State L) => Call(L);
        [AOT.MonoPInvokeCallback(typeof(LuaNativeFunction))]
        public static int Call(lua_State L)
        {         
            try
            {
                Action a = ToLightObject<Action>(L, lua_upvalueindex(1), false);
                a();
                return 0;
            }
            catch (Exception e)
            {
                return luaL_error(L, e.Message);
            }
        }

        public static int CallDel(lua_State L, int start, Delegate del)
        {
            Action a = (Action)del;
            a();
            return 0;
        }
    }

    public struct ActionCaller<T1>
    {
        [AOT.MonoPInvokeCallback(typeof(LuaNativeFunction))]
        public static int Call(lua_State L) => Call(L, 1);
        [AOT.MonoPInvokeCallback(typeof(LuaNativeFunction))]
        public static int StaticCall(lua_State L) => Call(L, 2);

        static int Call(lua_State L, int start)
        {
            try
            {
                var a = ToLightObject<Action<T1>>(L, lua_upvalueindex(1), false);
                return CallDel(L, start, a);
            }
            catch (Exception e)
            {
                return luaL_error(L, e.Message);
            }
        }
        
        public static int CallDel(lua_State L, int start, Delegate del)
        {
            var a = (Action<T1>)del;
            GetT(L, 0 + start, out T1 t1);
            a(t1);
            return 0;
        }
    }

    public struct ActionCaller<T1, T2>
    {
        [AOT.MonoPInvokeCallback(typeof(LuaNativeFunction))]
        public static int Call(lua_State L) => Call(L, 1);
        [AOT.MonoPInvokeCallback(typeof(LuaNativeFunction))]
        public static int StaticCall(lua_State L) => Call(L, 2);

        static int Call(lua_State L, int start)
        {
            try
            {
                var a = ToLightObject<Action<T1, T2>>(L, lua_upvalueindex(1), false);
                return CallDel(L, start, a);
            }
            catch (Exception e)
            {
                return luaL_error(L, e.Message);
            }
        }
        
        public static int CallDel(lua_State L, int start, Delegate del)
        {
            var a = (Action<T1, T2>)del;
            GetT(L, 0 + start, out T1 t1);
            GetT(L, 1 + start, out T2 t2);
            a(t1, t2);
            return 0;
        }
    }

    public struct ActionCaller<T1, T2, T3>
    {
        [AOT.MonoPInvokeCallback(typeof(LuaNativeFunction))]
        public static int Call(lua_State L) => Call(L, 1);
        [AOT.MonoPInvokeCallback(typeof(LuaNativeFunction))]
        public static int StaticCall(lua_State L) => Call(L, 2);

        static int Call(lua_State L, int start)
        {
            try
            {
                var a = ToLightObject<Action<T1, T2, T3>>(L, lua_upvalueindex(1), false);
                return CallDel(L, start, a);
            }
            catch (Exception e)
            {
                return luaL_error(L, e.Message);
            }
        }
        
        public static int CallDel(lua_State L, int start, Delegate del)
        {
            var a = (Action<T1, T2, T3>)del;
            GetT(L, 0 + start, out T1 t1);
            GetT(L, 1 + start, out T2 t2);
            GetT(L, 2 + start, out T3 t3);
            a(t1, t2, t3);
            return 0;
        }
    }

    public struct ActionCaller<T1, T2, T3, T4>
    {
        [AOT.MonoPInvokeCallback(typeof(LuaNativeFunction))]
        public static int Call(lua_State L) => Call(L, 1);
        [AOT.MonoPInvokeCallback(typeof(LuaNativeFunction))]
        public static int StaticCall(lua_State L) => Call(L, 2);

        static int Call(lua_State L, int start)
        {
            try
            {
                var a = ToLightObject<Action<T1, T2, T3, T4>>(L, lua_upvalueindex(1), false);
                return CallDel(L, start, a);
            }
            catch (Exception e)
            {
                return luaL_error(L, e.Message);
            }
        }
        
        public static int CallDel(lua_State L, int start, Delegate del)
        {
            var a = (Action<T1, T2, T3, T4>)del;
            GetT(L, 0 + start, out T1 t1);
            GetT(L, 1 + start, out T2 t2);
            GetT(L, 2 + start, out T3 t3);
            GetT(L, 3 + start, out T4 t4);
            a(t1, t2, t3, t4);
            return 0;
        }
    }

    public struct ActionCaller<T1, T2, T3, T4, T5>
    {
        [AOT.MonoPInvokeCallback(typeof(LuaNativeFunction))]
        public static int Call(lua_State L) => Call(L, 1);
        [AOT.MonoPInvokeCallback(typeof(LuaNativeFunction))]
        public static int StaticCall(lua_State L) => Call(L, 2);

        static int Call(lua_State L, int start)
        {
            try
            {
                var a = ToLightObject<Action<T1, T2, T3, T4, T5>>(L, lua_upvalueindex(1), false);
                return CallDel(L, start, a);
            }
            catch (Exception e)
            {
                return luaL_error(L, e.Message);
            }
        }
        
        public static int CallDel(lua_State L, int start, Delegate del)
        {
            var a = (Action<T1, T2, T3, T4, T5>)del;
            GetT(L, 0 + start, out T1 t1);
            GetT(L, 1 + start, out T2 t2);
            GetT(L, 2 + start, out T3 t3);
            GetT(L, 3 + start, out T4 t4);
            GetT(L, 4 + start, out T5 t5);
            a(t1, t2, t3, t4, t5);
            return 0;
        }
    }

    public struct ActionCaller<T1, T2, T3, T4, T5, T6>
    {
        [AOT.MonoPInvokeCallback(typeof(LuaNativeFunction))]
        public static int Call(lua_State L) => Call(L, 1);
        [AOT.MonoPInvokeCallback(typeof(LuaNativeFunction))]
        public static int StaticCall(lua_State L) => Call(L, 2);

        static int Call(lua_State L, int start)
        {
            try
            {
                var a = ToLightObject<Action<T1, T2, T3, T4, T5, T6>>(L, lua_upvalueindex(1), false);
                return CallDel(L, start, a);
            }
            catch (Exception e)
            {
                return luaL_error(L, e.Message);
            }
        }
        
        public static int CallDel(lua_State L, int start, Delegate del)
        {
            var a = (Action<T1, T2, T3, T4, T5, T6>)del;
            GetT(L, 0 + start, out T1 t1);
            GetT(L, 1 + start, out T2 t2);
            GetT(L, 2 + start, out T3 t3);
            GetT(L, 3 + start, out T4 t4);
            GetT(L, 4 + start, out T5 t5);
            GetT(L, 5 + start, out T6 t6);
            a(t1, t2, t3, t4, t5, t6);
            return 0;
        }
    }

    public struct ActionCaller<T1, T2, T3, T4, T5, T6, T7>
    {
        [AOT.MonoPInvokeCallback(typeof(LuaNativeFunction))]
        public static int Call(lua_State L) => Call(L, 1);
        [AOT.MonoPInvokeCallback(typeof(LuaNativeFunction))]
        public static int StaticCall(lua_State L) => Call(L, 2);

        static int Call(lua_State L, int start)
        {
            try
            {
                var a = ToLightObject<Action<T1, T2, T3, T4, T5, T6, T7>>(L, lua_upvalueindex(1), false);
                return CallDel(L, start, a);
            }
            catch (Exception e)
            {
                return luaL_error(L, e.Message);
            }
        }
        
        public static int CallDel(lua_State L, int start, Delegate del)
        {
            var a = (Action<T1, T2, T3, T4, T5, T6, T7>)del;
            GetT(L, 0 + start, out T1 t1);
            GetT(L, 1 + start, out T2 t2);
            GetT(L, 2 + start, out T3 t3);
            GetT(L, 3 + start, out T4 t4);
            GetT(L, 4 + start, out T5 t5);
            GetT(L, 5 + start, out T6 t6);
            GetT(L, 6 + start, out T7 t7);
            a(t1, t2, t3, t4, t5, t6, t7);
            return 0;
        }
    }

    public struct ActionCaller<T1, T2, T3, T4, T5, T6, T7, T8>
    {
        [AOT.MonoPInvokeCallback(typeof(LuaNativeFunction))]
        public static int Call(lua_State L) => Call(L, 1);
        [AOT.MonoPInvokeCallback(typeof(LuaNativeFunction))]
        public static int StaticCall(lua_State L) => Call(L, 2);

        static int Call(lua_State L, int start)
        {
            try
            {
                var a = ToLightObject<Action<T1, T2, T3, T4, T5, T6, T7, T8>>(L, lua_upvalueindex(1), false);
                return CallDel(L, start, a);
            }
            catch (Exception e)
            {
                return luaL_error(L, e.Message);
            }
        }
        
        public static int CallDel(lua_State L, int start, Delegate del)
        {
            var a = (Action<T1, T2, T3, T4, T5, T6, T7, T8>)del;
            GetT(L, 0 + start, out T1 t1);
            GetT(L, 1 + start, out T2 t2);
            GetT(L, 2 + start, out T3 t3);
            GetT(L, 3 + start, out T4 t4);
            GetT(L, 4 + start, out T5 t5);
            GetT(L, 5 + start, out T6 t6);
            GetT(L, 6 + start, out T7 t7);
            GetT(L, 7 + start, out T8 t8);
            a(t1, t2, t3, t4, t5, t6, t7, t8);
            return 0;
        }
    }


}
