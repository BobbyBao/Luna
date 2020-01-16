
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
        public static int StaticCall(lua_State L) => Call(L);
        public static int Call(lua_State L)
        {         
            try
            {
                var a = Lua.ToLightObject<RefFunc<R>>(L, lua_upvalueindex(1), false);
                var r = a();
                Lua.Push(L, r);
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
        public static int Call(lua_State L) => Call(L, 1);
        public static int StaticCall(lua_State L) => Call(L, 2);

        static int Call(lua_State L, int start)
        {
            try
            {
                var a = Lua.ToLightObject<RefFunc<T1, R>>(L, lua_upvalueindex(1), false);
                var r = a(
                    ref SharpObject.GetValue<T1>(L, 0 + start)
                );
                Lua.Push(L, r);
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
        public static int Call(lua_State L) => Call(L, 1);
        public static int StaticCall(lua_State L) => Call(L, 2);

        static int Call(lua_State L, int start)
        {
            try
            {
                var a = Lua.ToLightObject<RefFunc<T1, T2, R>>(L, lua_upvalueindex(1), false);
                var r = a(
                    ref SharpObject.GetValue<T1>(L, 0 + start),
                    Lua.Get<T2>(L, 1 + start)
                );
                Lua.Push(L, r);
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
        public static int Call(lua_State L) => Call(L, 1);
        public static int StaticCall(lua_State L) => Call(L, 2);

        static int Call(lua_State L, int start)
        {
            try
            {
                var a = Lua.ToLightObject<RefFunc<T1, T2, T3, R>>(L, lua_upvalueindex(1), false);
                var r = a(
                    ref SharpObject.GetValue<T1>(L, 0 + start),
                    Lua.Get<T2>(L, 1 + start),
                    Lua.Get<T3>(L, 2 + start)
                );
                Lua.Push(L, r);
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
        public static int Call(lua_State L) => Call(L, 1);
        public static int StaticCall(lua_State L) => Call(L, 2);

        static int Call(lua_State L, int start)
        {
            try
            {
                var a = Lua.ToLightObject<RefFunc<T1, T2, T3, T4, R>>(L, lua_upvalueindex(1), false);
                var r = a(
                    ref SharpObject.GetValue<T1>(L, 0 + start),
                    Lua.Get<T2>(L, 1 + start),
                    Lua.Get<T3>(L, 2 + start),
                    Lua.Get<T4>(L, 3 + start)
                );
                Lua.Push(L, r);
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
        public static int Call(lua_State L) => Call(L, 1);
        public static int StaticCall(lua_State L) => Call(L, 2);

        static int Call(lua_State L, int start)
        {
            try
            {
                var a = Lua.ToLightObject<RefFunc<T1, T2, T3, T4, T5, R>>(L, lua_upvalueindex(1), false);
                var r = a(
                    ref SharpObject.GetValue<T1>(L, 0 + start),
                    Lua.Get<T2>(L, 1 + start),
                    Lua.Get<T3>(L, 2 + start),
                    Lua.Get<T4>(L, 3 + start),
                    Lua.Get<T5>(L, 4 + start)
                );
                Lua.Push(L, r);
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
        public static int Call(lua_State L) => Call(L, 1);
        public static int StaticCall(lua_State L) => Call(L, 2);

        static int Call(lua_State L, int start)
        {
            try
            {
                var a = Lua.ToLightObject<RefFunc<T1, T2, T3, T4, T5, T6, R>>(L, lua_upvalueindex(1), false);
                var r = a(
                    ref SharpObject.GetValue<T1>(L, 0 + start),
                    Lua.Get<T2>(L, 1 + start),
                    Lua.Get<T3>(L, 2 + start),
                    Lua.Get<T4>(L, 3 + start),
                    Lua.Get<T5>(L, 4 + start),
                    Lua.Get<T6>(L, 5 + start)
                );
                Lua.Push(L, r);
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
        public static int Call(lua_State L) => Call(L, 1);
        public static int StaticCall(lua_State L) => Call(L, 2);

        static int Call(lua_State L, int start)
        {
            try
            {
                var a = Lua.ToLightObject<RefFunc<T1, T2, T3, T4, T5, T6, T7, R>>(L, lua_upvalueindex(1), false);
                var r = a(
                    ref SharpObject.GetValue<T1>(L, 0 + start),
                    Lua.Get<T2>(L, 1 + start),
                    Lua.Get<T3>(L, 2 + start),
                    Lua.Get<T4>(L, 3 + start),
                    Lua.Get<T5>(L, 4 + start),
                    Lua.Get<T6>(L, 5 + start),
                    Lua.Get<T7>(L, 6 + start)
                );
                Lua.Push(L, r);
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
        public static int Call(lua_State L) => Call(L, 1);
        public static int StaticCall(lua_State L) => Call(L, 2);

        static int Call(lua_State L, int start)
        {
            try
            {
                var a = Lua.ToLightObject<RefFunc<T1, T2, T3, T4, T5, T6, T7, T8, R>>(L, lua_upvalueindex(1), false);
                var r = a(
                    ref SharpObject.GetValue<T1>(L, 0 + start),
                    Lua.Get<T2>(L, 1 + start),
                    Lua.Get<T3>(L, 2 + start),
                    Lua.Get<T4>(L, 3 + start),
                    Lua.Get<T5>(L, 4 + start),
                    Lua.Get<T6>(L, 5 + start),
                    Lua.Get<T7>(L, 6 + start),
                    Lua.Get<T8>(L, 7 + start)
                );
                Lua.Push(L, r);
                return 1;
            }
            catch (Exception e)
            {
                return luaL_error(L, e.Message);
            }
        }
    }


}
