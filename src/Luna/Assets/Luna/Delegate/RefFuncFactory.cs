
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

    public struct RefFuncFactory<R>
    {
        public static RefFunc<R> Create(IntPtr L, int index)
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
                Get(L, -1, out R ret);                
                lua_settop(L, errFunc - 1);
                return ret;
            };
        }

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

        public static int CallDel(lua_State L, int start, Delegate del)
        {
            var a = (RefFunc<R>)del;
            var r = a();
            PushT(L, r);
            return 1;
        }
    }

    public struct RefFuncFactory<T1, R> where T1 : struct
    {
        public static RefFunc<T1, R> Create(IntPtr L, int index)
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
                Get(L, -1, out R ret);                
                lua_settop(L, errFunc - 1);
                return ret;
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
                var a = ToLightObject<RefFunc<T1, R>>(L, lua_upvalueindex(1), false);
                return CallDel(L, start, a);
            }
            catch (Exception e)
            {
                return luaL_error(L, e.Message);
            }
        }
        
        public static int CallDel(lua_State L, int start, Delegate del)
        {
            var a = (RefFunc<T1, R>)del;
            var r = a(ref SharpObject.GetValue<T1>(L, 0 + start));
            PushT(L, r);
            return 1;
        }
    }

    public struct RefFuncFactory<T1, T2, R> where T1 : struct
    {
        public static RefFunc<T1, T2, R> Create(IntPtr L, int index)
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
                Get(L, -1, out R ret);                
                lua_settop(L, errFunc - 1);
                return ret;
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
                var a = ToLightObject<RefFunc<T1, T2, R>>(L, lua_upvalueindex(1), false);
                return CallDel(L, start, a);
            }
            catch (Exception e)
            {
                return luaL_error(L, e.Message);
            }
        }
        
        public static int CallDel(lua_State L, int start, Delegate del)
        {
            var a = (RefFunc<T1, T2, R>)del;
            GetT(L, 1 + start, out T2 t2);
            var r = a(ref SharpObject.GetValue<T1>(L, 0 + start), t2);
            PushT(L, r);
            return 1;
        }
    }

    public struct RefFuncFactory<T1, T2, T3, R> where T1 : struct
    {
        public static RefFunc<T1, T2, T3, R> Create(IntPtr L, int index)
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
                Get(L, -1, out R ret);                
                lua_settop(L, errFunc - 1);
                return ret;
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
                var a = ToLightObject<RefFunc<T1, T2, T3, R>>(L, lua_upvalueindex(1), false);
                return CallDel(L, start, a);
            }
            catch (Exception e)
            {
                return luaL_error(L, e.Message);
            }
        }
        
        public static int CallDel(lua_State L, int start, Delegate del)
        {
            var a = (RefFunc<T1, T2, T3, R>)del;
            GetT(L, 1 + start, out T2 t2);
            GetT(L, 2 + start, out T3 t3);
            var r = a(ref SharpObject.GetValue<T1>(L, 0 + start), t2, t3);
            PushT(L, r);
            return 1;
        }
    }

    public struct RefFuncFactory<T1, T2, T3, T4, R> where T1 : struct
    {
        public static RefFunc<T1, T2, T3, T4, R> Create(IntPtr L, int index)
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
                Get(L, -1, out R ret);                
                lua_settop(L, errFunc - 1);
                return ret;
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
                var a = ToLightObject<RefFunc<T1, T2, T3, T4, R>>(L, lua_upvalueindex(1), false);
                return CallDel(L, start, a);
            }
            catch (Exception e)
            {
                return luaL_error(L, e.Message);
            }
        }
        
        public static int CallDel(lua_State L, int start, Delegate del)
        {
            var a = (RefFunc<T1, T2, T3, T4, R>)del;
            GetT(L, 1 + start, out T2 t2);
            GetT(L, 2 + start, out T3 t3);
            GetT(L, 3 + start, out T4 t4);
            var r = a(ref SharpObject.GetValue<T1>(L, 0 + start), t2, t3, t4);
            PushT(L, r);
            return 1;
        }
    }

    public struct RefFuncFactory<T1, T2, T3, T4, T5, R> where T1 : struct
    {
        public static RefFunc<T1, T2, T3, T4, T5, R> Create(IntPtr L, int index)
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
                Get(L, -1, out R ret);                
                lua_settop(L, errFunc - 1);
                return ret;
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
                var a = ToLightObject<RefFunc<T1, T2, T3, T4, T5, R>>(L, lua_upvalueindex(1), false);
                return CallDel(L, start, a);
            }
            catch (Exception e)
            {
                return luaL_error(L, e.Message);
            }
        }
        
        public static int CallDel(lua_State L, int start, Delegate del)
        {
            var a = (RefFunc<T1, T2, T3, T4, T5, R>)del;
            GetT(L, 1 + start, out T2 t2);
            GetT(L, 2 + start, out T3 t3);
            GetT(L, 3 + start, out T4 t4);
            GetT(L, 4 + start, out T5 t5);
            var r = a(ref SharpObject.GetValue<T1>(L, 0 + start), t2, t3, t4, t5);
            PushT(L, r);
            return 1;
        }
    }

    public struct RefFuncFactory<T1, T2, T3, T4, T5, T6, R> where T1 : struct
    {
        public static RefFunc<T1, T2, T3, T4, T5, T6, R> Create(IntPtr L, int index)
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
                Get(L, -1, out R ret);                
                lua_settop(L, errFunc - 1);
                return ret;
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
                var a = ToLightObject<RefFunc<T1, T2, T3, T4, T5, T6, R>>(L, lua_upvalueindex(1), false);
                return CallDel(L, start, a);
            }
            catch (Exception e)
            {
                return luaL_error(L, e.Message);
            }
        }
        
        public static int CallDel(lua_State L, int start, Delegate del)
        {
            var a = (RefFunc<T1, T2, T3, T4, T5, T6, R>)del;
            GetT(L, 1 + start, out T2 t2);
            GetT(L, 2 + start, out T3 t3);
            GetT(L, 3 + start, out T4 t4);
            GetT(L, 4 + start, out T5 t5);
            GetT(L, 5 + start, out T6 t6);
            var r = a(ref SharpObject.GetValue<T1>(L, 0 + start), t2, t3, t4, t5, t6);
            PushT(L, r);
            return 1;
        }
    }

    public struct RefFuncFactory<T1, T2, T3, T4, T5, T6, T7, R> where T1 : struct
    {
        public static RefFunc<T1, T2, T3, T4, T5, T6, T7, R> Create(IntPtr L, int index)
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
                Get(L, -1, out R ret);                
                lua_settop(L, errFunc - 1);
                return ret;
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
                var a = ToLightObject<RefFunc<T1, T2, T3, T4, T5, T6, T7, R>>(L, lua_upvalueindex(1), false);
                return CallDel(L, start, a);
            }
            catch (Exception e)
            {
                return luaL_error(L, e.Message);
            }
        }
        
        public static int CallDel(lua_State L, int start, Delegate del)
        {
            var a = (RefFunc<T1, T2, T3, T4, T5, T6, T7, R>)del;
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
    }

    public struct RefFuncFactory<T1, T2, T3, T4, T5, T6, T7, T8, R> where T1 : struct
    {
        public static RefFunc<T1, T2, T3, T4, T5, T6, T7, T8, R> Create(IntPtr L, int index)
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
                Get(L, -1, out R ret);                
                lua_settop(L, errFunc - 1);
                return ret;
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
                var a = ToLightObject<RefFunc<T1, T2, T3, T4, T5, T6, T7, T8, R>>(L, lua_upvalueindex(1), false);
                return CallDel(L, start, a);
            }
            catch (Exception e)
            {
                return luaL_error(L, e.Message);
            }
        }
        
        public static int CallDel(lua_State L, int start, Delegate del)
        {
            var a = (RefFunc<T1, T2, T3, T4, T5, T6, T7, T8, R>)del;
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
    }


}
