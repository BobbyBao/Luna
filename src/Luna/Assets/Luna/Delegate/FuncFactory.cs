
using System;
using System.Reflection;

namespace SharpLuna
{
    using static Lua;
    using lua_State = IntPtr;

    public struct FuncFactory<R>
    {
        public static Func<R> Create(IntPtr L, int index)
        {
            lua_pushvalue(L, index);
            int luaref = luaL_ref(L, LUA_REGISTRYINDEX);
            return () =>
            {
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

    public struct FuncFactory<T1, R>
    {      
        public static Func<T1, R> Create(IntPtr L, int index)
        {
            lua_pushvalue(L, index);
            int luaref = luaL_ref(L, LUA_REGISTRYINDEX);
            return (t1) =>
            {
                int errFunc = pcall_prepare(L, errorFuncRef, luaref);
                PushT(L, t1);
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

    public struct FuncFactory<T1, T2, R>
    {      
        public static Func<T1, T2, R> Create(IntPtr L, int index)
        {
            lua_pushvalue(L, index);
            int luaref = luaL_ref(L, LUA_REGISTRYINDEX);
            return (t1, t2) =>
            {
                int errFunc = pcall_prepare(L, errorFuncRef, luaref);
                PushT(L, t1);
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

    public struct FuncFactory<T1, T2, T3, R>
    {      
        public static Func<T1, T2, T3, R> Create(IntPtr L, int index)
        {
            lua_pushvalue(L, index);
            int luaref = luaL_ref(L, LUA_REGISTRYINDEX);
            return (t1, t2, t3) =>
            {
                int errFunc = pcall_prepare(L, errorFuncRef, luaref);
                PushT(L, t1);
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

    public struct FuncFactory<T1, T2, T3, T4, R>
    {      
        public static Func<T1, T2, T3, T4, R> Create(IntPtr L, int index)
        {
            lua_pushvalue(L, index);
            int luaref = luaL_ref(L, LUA_REGISTRYINDEX);
            return (t1, t2, t3, t4) =>
            {
                int errFunc = pcall_prepare(L, errorFuncRef, luaref);
                PushT(L, t1);
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

    public struct FuncFactory<T1, T2, T3, T4, T5, R>
    {      
        public static Func<T1, T2, T3, T4, T5, R> Create(IntPtr L, int index)
        {
            lua_pushvalue(L, index);
            int luaref = luaL_ref(L, LUA_REGISTRYINDEX);
            return (t1, t2, t3, t4, t5) =>
            {
                int errFunc = pcall_prepare(L, errorFuncRef, luaref);
                PushT(L, t1);
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

    public struct FuncFactory<T1, T2, T3, T4, T5, T6, R>
    {      
        public static Func<T1, T2, T3, T4, T5, T6, R> Create(IntPtr L, int index)
        {
            lua_pushvalue(L, index);
            int luaref = luaL_ref(L, LUA_REGISTRYINDEX);
            return (t1, t2, t3, t4, t5, t6) =>
            {
                int errFunc = pcall_prepare(L, errorFuncRef, luaref);
                PushT(L, t1);
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

    public struct FuncFactory<T1, T2, T3, T4, T5, T6, T7, R>
    {      
        public static Func<T1, T2, T3, T4, T5, T6, T7, R> Create(IntPtr L, int index)
        {
            lua_pushvalue(L, index);
            int luaref = luaL_ref(L, LUA_REGISTRYINDEX);
            return (t1, t2, t3, t4, t5, t6, t7) =>
            {
                int errFunc = pcall_prepare(L, errorFuncRef, luaref);
                PushT(L, t1);
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

    public struct FuncFactory<T1, T2, T3, T4, T5, T6, T7, T8, R>
    {      
        public static Func<T1, T2, T3, T4, T5, T6, T7, T8, R> Create(IntPtr L, int index)
        {
            lua_pushvalue(L, index);
            int luaref = luaL_ref(L, LUA_REGISTRYINDEX);
            return (t1, t2, t3, t4, t5, t6, t7, t8) =>
            {
                int errFunc = pcall_prepare(L, errorFuncRef, luaref);
                PushT(L, t1);
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
