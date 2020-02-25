
using System;
using System.Reflection;

namespace SharpLuna
{
    using static Lua;
    using lua_State = IntPtr;

    public struct ActionFactory
    {
        public static Action Create(IntPtr L, int index)
        {
            lua_pushvalue(L, index);
            int luaref = luaL_ref(L, LUA_REGISTRYINDEX);
            return () =>
            {
                lua_pushcfunction(L, LuaException.traceback);
                lua_rawgeti(L, LUA_REGISTRYINDEX, luaref);
                if (lua_pcall(L, 0, 0, 2) != (int)LuaStatus.OK)
                {
                    lua_remove(L, -2);
                    throw new LuaException(L);
                }
                lua_pop(L, 1);
            };
        }

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

    public struct ActionFactory<T1>
    {
        public static Action<T1> Create(IntPtr L, int index)
        {
            lua_pushvalue(L, index);
            int luaref = luaL_ref(L, LUA_REGISTRYINDEX);
            return (t1) =>
            {
                lua_pushcfunction(L, LuaException.traceback);
                lua_rawgeti(L, LUA_REGISTRYINDEX, luaref);
                Push(L, t1);
                if (lua_pcall(L, 0, 0, -0 + 2) != (int)LuaStatus.OK)
                {
                    lua_remove(L, -2);
                    throw new LuaException(L);
                }
                lua_pop(L, 1);
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

    public struct ActionFactory<T1, T2>
    {
        public static Action<T1, T2> Create(IntPtr L, int index)
        {
            lua_pushvalue(L, index);
            int luaref = luaL_ref(L, LUA_REGISTRYINDEX);
            return (t1, t2) =>
            {
                lua_pushcfunction(L, LuaException.traceback);
                lua_rawgeti(L, LUA_REGISTRYINDEX, luaref);
                Push(L, t1);
                Push(L, t2);
                if (lua_pcall(L, 1, 0, -1 + 2) != (int)LuaStatus.OK)
                {
                    lua_remove(L, -2);
                    throw new LuaException(L);
                }
                lua_pop(L, 1);
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

    public struct ActionFactory<T1, T2, T3>
    {
        public static Action<T1, T2, T3> Create(IntPtr L, int index)
        {
            lua_pushvalue(L, index);
            int luaref = luaL_ref(L, LUA_REGISTRYINDEX);
            return (t1, t2, t3) =>
            {
                lua_pushcfunction(L, LuaException.traceback);
                lua_rawgeti(L, LUA_REGISTRYINDEX, luaref);
                Push(L, t1);
                Push(L, t2);
                Push(L, t3);
                if (lua_pcall(L, 2, 0, -2 + 2) != (int)LuaStatus.OK)
                {
                    lua_remove(L, -2);
                    throw new LuaException(L);
                }
                lua_pop(L, 1);
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

    public struct ActionFactory<T1, T2, T3, T4>
    {
        public static Action<T1, T2, T3, T4> Create(IntPtr L, int index)
        {
            lua_pushvalue(L, index);
            int luaref = luaL_ref(L, LUA_REGISTRYINDEX);
            return (t1, t2, t3, t4) =>
            {
                lua_pushcfunction(L, LuaException.traceback);
                lua_rawgeti(L, LUA_REGISTRYINDEX, luaref);
                Push(L, t1);
                Push(L, t2);
                Push(L, t3);
                Push(L, t4);
                if (lua_pcall(L, 3, 0, -3 + 2) != (int)LuaStatus.OK)
                {
                    lua_remove(L, -2);
                    throw new LuaException(L);
                }
                lua_pop(L, 1);
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

    public struct ActionFactory<T1, T2, T3, T4, T5>
    {
        public static Action<T1, T2, T3, T4, T5> Create(IntPtr L, int index)
        {
            lua_pushvalue(L, index);
            int luaref = luaL_ref(L, LUA_REGISTRYINDEX);
            return (t1, t2, t3, t4, t5) =>
            {
                lua_pushcfunction(L, LuaException.traceback);
                lua_rawgeti(L, LUA_REGISTRYINDEX, luaref);
                Push(L, t1);
                Push(L, t2);
                Push(L, t3);
                Push(L, t4);
                Push(L, t5);
                if (lua_pcall(L, 4, 0, -4 + 2) != (int)LuaStatus.OK)
                {
                    lua_remove(L, -2);
                    throw new LuaException(L);
                }
                lua_pop(L, 1);
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

    public struct ActionFactory<T1, T2, T3, T4, T5, T6>
    {
        public static Action<T1, T2, T3, T4, T5, T6> Create(IntPtr L, int index)
        {
            lua_pushvalue(L, index);
            int luaref = luaL_ref(L, LUA_REGISTRYINDEX);
            return (t1, t2, t3, t4, t5, t6) =>
            {
                lua_pushcfunction(L, LuaException.traceback);
                lua_rawgeti(L, LUA_REGISTRYINDEX, luaref);
                Push(L, t1);
                Push(L, t2);
                Push(L, t3);
                Push(L, t4);
                Push(L, t5);
                Push(L, t6);
                if (lua_pcall(L, 5, 0, -5 + 2) != (int)LuaStatus.OK)
                {
                    lua_remove(L, -2);
                    throw new LuaException(L);
                }
                lua_pop(L, 1);
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

    public struct ActionFactory<T1, T2, T3, T4, T5, T6, T7>
    {
        public static Action<T1, T2, T3, T4, T5, T6, T7> Create(IntPtr L, int index)
        {
            lua_pushvalue(L, index);
            int luaref = luaL_ref(L, LUA_REGISTRYINDEX);
            return (t1, t2, t3, t4, t5, t6, t7) =>
            {
                lua_pushcfunction(L, LuaException.traceback);
                lua_rawgeti(L, LUA_REGISTRYINDEX, luaref);
                Push(L, t1);
                Push(L, t2);
                Push(L, t3);
                Push(L, t4);
                Push(L, t5);
                Push(L, t6);
                Push(L, t7);
                if (lua_pcall(L, 6, 0, -6 + 2) != (int)LuaStatus.OK)
                {
                    lua_remove(L, -2);
                    throw new LuaException(L);
                }
                lua_pop(L, 1);
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

    public struct ActionFactory<T1, T2, T3, T4, T5, T6, T7, T8>
    {
        public static Action<T1, T2, T3, T4, T5, T6, T7, T8> Create(IntPtr L, int index)
        {
            lua_pushvalue(L, index);
            int luaref = luaL_ref(L, LUA_REGISTRYINDEX);
            return (t1, t2, t3, t4, t5, t6, t7, t8) =>
            {
                lua_pushcfunction(L, LuaException.traceback);
                lua_rawgeti(L, LUA_REGISTRYINDEX, luaref);
                Push(L, t1);
                Push(L, t2);
                Push(L, t3);
                Push(L, t4);
                Push(L, t5);
                Push(L, t6);
                Push(L, t7);
                Push(L, t8);
                if (lua_pcall(L, 7, 0, -7 + 2) != (int)LuaStatus.OK)
                {
                    lua_remove(L, -2);
                    throw new LuaException(L);
                }
                lua_pop(L, 1);
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
