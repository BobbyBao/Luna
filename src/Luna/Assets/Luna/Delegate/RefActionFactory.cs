
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

    public struct RefActionFactory
    {
        public static RefAction Create(IntPtr L, int index)
        {
            lua_pushvalue(L, index);
            int luaref = luaL_ref(L, LUA_REGISTRYINDEX);
            return () =>
            {
                L = mainState;
                int errFunc = pcall_prepare(L, errorFuncRef, luaref);
                if (lua_pcall(L, 0, 0, errFunc) != (int)LuaStatus.OK)
                {
                    ThrowExceptionFromError(L, errFunc - 1);
                }
                lua_settop(L, errFunc - 1);
            };
        }

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

        public static int CallDel(lua_State L, int start, Delegate del)
        {
            RefAction a = (RefAction)del;
            a();
            return 0;
        }
    }

    public struct RefActionFactory<T1> where T1 : struct
    {        
        public static RefAction<T1> Create(IntPtr L, int index)
        {
            lua_pushvalue(L, index);
            int luaref = luaL_ref(L, LUA_REGISTRYINDEX);
            return (ref T1 t1) =>
            {
                L = mainState;
                int errFunc = pcall_prepare(L, errorFuncRef, luaref);
                Push(L, ref t1);
                if (lua_pcall(L, 1, 0, errFunc) != (int)LuaStatus.OK)
                {
                    ThrowExceptionFromError(L, errFunc - 1);
                }
                lua_settop(L, errFunc - 1);
            };
        }

        [AOT.MonoPInvokeCallback(typeof(LuaNativeFunction))]
        public static int Call(lua_State L) => Call(L, 1);
        [AOT.MonoPInvokeCallback(typeof(LuaNativeFunction))]
        public static int StaticCall(lua_State L) => Call(L, 2);

        static int Call(lua_State L, int start)
        {
            try
            {
                var a = ToLightObject<RefAction<T1>>(L, lua_upvalueindex(1), false);
                return CallDel(L, start, a);
            }
            catch (Exception e)
            {
                return luaL_error(L, e.Message);
            }
        }
        
        public static int CallDel(lua_State L, int start, Delegate del)
        {
            var a = (RefAction<T1>)del;
            GetT(L, 0 + start, out T1 t1);
            a(ref SharpObject.GetValue<T1>(L, 0 + start));
            return 0;
        }
    }

    public struct RefActionFactory<T1, T2> where T1 : struct
    {        
        public static RefAction<T1, T2> Create(IntPtr L, int index)
        {
            lua_pushvalue(L, index);
            int luaref = luaL_ref(L, LUA_REGISTRYINDEX);
            return (ref T1 t1, T2 t2) =>
            {
                L = mainState;
                int errFunc = pcall_prepare(L, errorFuncRef, luaref);
                Push(L, ref t1);
                PushT(L, t2);
                if (lua_pcall(L, 2, 0, errFunc) != (int)LuaStatus.OK)
                {
                    ThrowExceptionFromError(L, errFunc - 1);
                }
                lua_settop(L, errFunc - 1);
            };
        }

        [AOT.MonoPInvokeCallback(typeof(LuaNativeFunction))]
        public static int Call(lua_State L) => Call(L, 1);
        [AOT.MonoPInvokeCallback(typeof(LuaNativeFunction))]
        public static int StaticCall(lua_State L) => Call(L, 2);

        static int Call(lua_State L, int start)
        {
            try
            {
                var a = ToLightObject<RefAction<T1, T2>>(L, lua_upvalueindex(1), false);
                return CallDel(L, start, a);
            }
            catch (Exception e)
            {
                return luaL_error(L, e.Message);
            }
        }
        
        public static int CallDel(lua_State L, int start, Delegate del)
        {
            var a = (RefAction<T1, T2>)del;
            GetT(L, 0 + start, out T1 t1);
            GetT(L, 1 + start, out T2 t2);
            a(ref SharpObject.GetValue<T1>(L, 0 + start), t2);
            return 0;
        }
    }

    public struct RefActionFactory<T1, T2, T3> where T1 : struct
    {        
        public static RefAction<T1, T2, T3> Create(IntPtr L, int index)
        {
            lua_pushvalue(L, index);
            int luaref = luaL_ref(L, LUA_REGISTRYINDEX);
            return (ref T1 t1, T2 t2, T3 t3) =>
            {
                L = mainState;
                int errFunc = pcall_prepare(L, errorFuncRef, luaref);
                Push(L, ref t1);
                PushT(L, t2);
                PushT(L, t3);
                if (lua_pcall(L, 3, 0, errFunc) != (int)LuaStatus.OK)
                {
                    ThrowExceptionFromError(L, errFunc - 1);
                }
                lua_settop(L, errFunc - 1);
            };
        }

        [AOT.MonoPInvokeCallback(typeof(LuaNativeFunction))]
        public static int Call(lua_State L) => Call(L, 1);
        [AOT.MonoPInvokeCallback(typeof(LuaNativeFunction))]
        public static int StaticCall(lua_State L) => Call(L, 2);

        static int Call(lua_State L, int start)
        {
            try
            {
                var a = ToLightObject<RefAction<T1, T2, T3>>(L, lua_upvalueindex(1), false);
                return CallDel(L, start, a);
            }
            catch (Exception e)
            {
                return luaL_error(L, e.Message);
            }
        }
        
        public static int CallDel(lua_State L, int start, Delegate del)
        {
            var a = (RefAction<T1, T2, T3>)del;
            GetT(L, 0 + start, out T1 t1);
            GetT(L, 1 + start, out T2 t2);
            GetT(L, 2 + start, out T3 t3);
            a(ref SharpObject.GetValue<T1>(L, 0 + start), t2, t3);
            return 0;
        }
    }

    public struct RefActionFactory<T1, T2, T3, T4> where T1 : struct
    {        
        public static RefAction<T1, T2, T3, T4> Create(IntPtr L, int index)
        {
            lua_pushvalue(L, index);
            int luaref = luaL_ref(L, LUA_REGISTRYINDEX);
            return (ref T1 t1, T2 t2, T3 t3, T4 t4) =>
            {
                L = mainState;
                int errFunc = pcall_prepare(L, errorFuncRef, luaref);
                Push(L, ref t1);
                PushT(L, t2);
                PushT(L, t3);
                PushT(L, t4);
                if (lua_pcall(L, 4, 0, errFunc) != (int)LuaStatus.OK)
                {
                    ThrowExceptionFromError(L, errFunc - 1);
                }
                lua_settop(L, errFunc - 1);
            };
        }

        [AOT.MonoPInvokeCallback(typeof(LuaNativeFunction))]
        public static int Call(lua_State L) => Call(L, 1);
        [AOT.MonoPInvokeCallback(typeof(LuaNativeFunction))]
        public static int StaticCall(lua_State L) => Call(L, 2);

        static int Call(lua_State L, int start)
        {
            try
            {
                var a = ToLightObject<RefAction<T1, T2, T3, T4>>(L, lua_upvalueindex(1), false);
                return CallDel(L, start, a);
            }
            catch (Exception e)
            {
                return luaL_error(L, e.Message);
            }
        }
        
        public static int CallDel(lua_State L, int start, Delegate del)
        {
            var a = (RefAction<T1, T2, T3, T4>)del;
            GetT(L, 0 + start, out T1 t1);
            GetT(L, 1 + start, out T2 t2);
            GetT(L, 2 + start, out T3 t3);
            GetT(L, 3 + start, out T4 t4);
            a(ref SharpObject.GetValue<T1>(L, 0 + start), t2, t3, t4);
            return 0;
        }
    }

    public struct RefActionFactory<T1, T2, T3, T4, T5> where T1 : struct
    {        
        public static RefAction<T1, T2, T3, T4, T5> Create(IntPtr L, int index)
        {
            lua_pushvalue(L, index);
            int luaref = luaL_ref(L, LUA_REGISTRYINDEX);
            return (ref T1 t1, T2 t2, T3 t3, T4 t4, T5 t5) =>
            {
                L = mainState;
                int errFunc = pcall_prepare(L, errorFuncRef, luaref);
                Push(L, ref t1);
                PushT(L, t2);
                PushT(L, t3);
                PushT(L, t4);
                PushT(L, t5);
                if (lua_pcall(L, 5, 0, errFunc) != (int)LuaStatus.OK)
                {
                    ThrowExceptionFromError(L, errFunc - 1);
                }
                lua_settop(L, errFunc - 1);
            };
        }

        [AOT.MonoPInvokeCallback(typeof(LuaNativeFunction))]
        public static int Call(lua_State L) => Call(L, 1);
        [AOT.MonoPInvokeCallback(typeof(LuaNativeFunction))]
        public static int StaticCall(lua_State L) => Call(L, 2);

        static int Call(lua_State L, int start)
        {
            try
            {
                var a = ToLightObject<RefAction<T1, T2, T3, T4, T5>>(L, lua_upvalueindex(1), false);
                return CallDel(L, start, a);
            }
            catch (Exception e)
            {
                return luaL_error(L, e.Message);
            }
        }
        
        public static int CallDel(lua_State L, int start, Delegate del)
        {
            var a = (RefAction<T1, T2, T3, T4, T5>)del;
            GetT(L, 0 + start, out T1 t1);
            GetT(L, 1 + start, out T2 t2);
            GetT(L, 2 + start, out T3 t3);
            GetT(L, 3 + start, out T4 t4);
            GetT(L, 4 + start, out T5 t5);
            a(ref SharpObject.GetValue<T1>(L, 0 + start), t2, t3, t4, t5);
            return 0;
        }
    }

    public struct RefActionFactory<T1, T2, T3, T4, T5, T6> where T1 : struct
    {        
        public static RefAction<T1, T2, T3, T4, T5, T6> Create(IntPtr L, int index)
        {
            lua_pushvalue(L, index);
            int luaref = luaL_ref(L, LUA_REGISTRYINDEX);
            return (ref T1 t1, T2 t2, T3 t3, T4 t4, T5 t5, T6 t6) =>
            {
                L = mainState;
                int errFunc = pcall_prepare(L, errorFuncRef, luaref);
                Push(L, ref t1);
                PushT(L, t2);
                PushT(L, t3);
                PushT(L, t4);
                PushT(L, t5);
                PushT(L, t6);
                if (lua_pcall(L, 6, 0, errFunc) != (int)LuaStatus.OK)
                {
                    ThrowExceptionFromError(L, errFunc - 1);
                }
                lua_settop(L, errFunc - 1);
            };
        }

        [AOT.MonoPInvokeCallback(typeof(LuaNativeFunction))]
        public static int Call(lua_State L) => Call(L, 1);
        [AOT.MonoPInvokeCallback(typeof(LuaNativeFunction))]
        public static int StaticCall(lua_State L) => Call(L, 2);

        static int Call(lua_State L, int start)
        {
            try
            {
                var a = ToLightObject<RefAction<T1, T2, T3, T4, T5, T6>>(L, lua_upvalueindex(1), false);
                return CallDel(L, start, a);
            }
            catch (Exception e)
            {
                return luaL_error(L, e.Message);
            }
        }
        
        public static int CallDel(lua_State L, int start, Delegate del)
        {
            var a = (RefAction<T1, T2, T3, T4, T5, T6>)del;
            GetT(L, 0 + start, out T1 t1);
            GetT(L, 1 + start, out T2 t2);
            GetT(L, 2 + start, out T3 t3);
            GetT(L, 3 + start, out T4 t4);
            GetT(L, 4 + start, out T5 t5);
            GetT(L, 5 + start, out T6 t6);
            a(ref SharpObject.GetValue<T1>(L, 0 + start), t2, t3, t4, t5, t6);
            return 0;
        }
    }

    public struct RefActionFactory<T1, T2, T3, T4, T5, T6, T7> where T1 : struct
    {        
        public static RefAction<T1, T2, T3, T4, T5, T6, T7> Create(IntPtr L, int index)
        {
            lua_pushvalue(L, index);
            int luaref = luaL_ref(L, LUA_REGISTRYINDEX);
            return (ref T1 t1, T2 t2, T3 t3, T4 t4, T5 t5, T6 t6, T7 t7) =>
            {
                L = mainState;
                int errFunc = pcall_prepare(L, errorFuncRef, luaref);
                Push(L, ref t1);
                PushT(L, t2);
                PushT(L, t3);
                PushT(L, t4);
                PushT(L, t5);
                PushT(L, t6);
                PushT(L, t7);
                if (lua_pcall(L, 7, 0, errFunc) != (int)LuaStatus.OK)
                {
                    ThrowExceptionFromError(L, errFunc - 1);
                }
                lua_settop(L, errFunc - 1);
            };
        }

        [AOT.MonoPInvokeCallback(typeof(LuaNativeFunction))]
        public static int Call(lua_State L) => Call(L, 1);
        [AOT.MonoPInvokeCallback(typeof(LuaNativeFunction))]
        public static int StaticCall(lua_State L) => Call(L, 2);

        static int Call(lua_State L, int start)
        {
            try
            {
                var a = ToLightObject<RefAction<T1, T2, T3, T4, T5, T6, T7>>(L, lua_upvalueindex(1), false);
                return CallDel(L, start, a);
            }
            catch (Exception e)
            {
                return luaL_error(L, e.Message);
            }
        }
        
        public static int CallDel(lua_State L, int start, Delegate del)
        {
            var a = (RefAction<T1, T2, T3, T4, T5, T6, T7>)del;
            GetT(L, 0 + start, out T1 t1);
            GetT(L, 1 + start, out T2 t2);
            GetT(L, 2 + start, out T3 t3);
            GetT(L, 3 + start, out T4 t4);
            GetT(L, 4 + start, out T5 t5);
            GetT(L, 5 + start, out T6 t6);
            GetT(L, 6 + start, out T7 t7);
            a(ref SharpObject.GetValue<T1>(L, 0 + start), t2, t3, t4, t5, t6, t7);
            return 0;
        }
    }

    public struct RefActionFactory<T1, T2, T3, T4, T5, T6, T7, T8> where T1 : struct
    {        
        public static RefAction<T1, T2, T3, T4, T5, T6, T7, T8> Create(IntPtr L, int index)
        {
            lua_pushvalue(L, index);
            int luaref = luaL_ref(L, LUA_REGISTRYINDEX);
            return (ref T1 t1, T2 t2, T3 t3, T4 t4, T5 t5, T6 t6, T7 t7, T8 t8) =>
            {
                L = mainState;
                int errFunc = pcall_prepare(L, errorFuncRef, luaref);
                Push(L, ref t1);
                PushT(L, t2);
                PushT(L, t3);
                PushT(L, t4);
                PushT(L, t5);
                PushT(L, t6);
                PushT(L, t7);
                PushT(L, t8);
                if (lua_pcall(L, 8, 0, errFunc) != (int)LuaStatus.OK)
                {
                    ThrowExceptionFromError(L, errFunc - 1);
                }
                lua_settop(L, errFunc - 1);
            };
        }

        [AOT.MonoPInvokeCallback(typeof(LuaNativeFunction))]
        public static int Call(lua_State L) => Call(L, 1);
        [AOT.MonoPInvokeCallback(typeof(LuaNativeFunction))]
        public static int StaticCall(lua_State L) => Call(L, 2);

        static int Call(lua_State L, int start)
        {
            try
            {
                var a = ToLightObject<RefAction<T1, T2, T3, T4, T5, T6, T7, T8>>(L, lua_upvalueindex(1), false);
                return CallDel(L, start, a);
            }
            catch (Exception e)
            {
                return luaL_error(L, e.Message);
            }
        }
        
        public static int CallDel(lua_State L, int start, Delegate del)
        {
            var a = (RefAction<T1, T2, T3, T4, T5, T6, T7, T8>)del;
            GetT(L, 0 + start, out T1 t1);
            GetT(L, 1 + start, out T2 t2);
            GetT(L, 2 + start, out T3 t3);
            GetT(L, 3 + start, out T4 t4);
            GetT(L, 4 + start, out T5 t5);
            GetT(L, 5 + start, out T6 t6);
            GetT(L, 6 + start, out T7 t7);
            GetT(L, 7 + start, out T8 t8);
            a(ref SharpObject.GetValue<T1>(L, 0 + start), t2, t3, t4, t5, t6, t7, t8);
            return 0;
        }
    }


}
