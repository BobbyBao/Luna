
using System;
using System.Reflection;

namespace SharpLuna
{
    using static Lua;

    public struct FuncCaller<R>
    {
        public static int StaticCall(LuaState L) => Call(L);
        public static int Call(LuaState L)
        {         
            try
            {
                var a = L.ToLightObject<Func<R>>(lua_upvalueindex(1), false);
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

    public struct FuncCaller<T1, R>
    {
        public static int Call(LuaState L) => Call(L, 1);
        public static int StaticCall(LuaState L) => Call(L, 2);

        static int Call(LuaState L, int start)
        {
            try
            {
                var a = L.ToLightObject<Func<T1, R>>(lua_upvalueindex(1), false);
                var r = a(
                    Lua.Get<T1>(L, 0 + start)
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

    public struct FuncCaller<T1, T2, R>
    {
        public static int Call(LuaState L) => Call(L, 1);
        public static int StaticCall(LuaState L) => Call(L, 2);

        static int Call(LuaState L, int start)
        {
            try
            {
                var a = L.ToLightObject<Func<T1, T2, R>>(lua_upvalueindex(1), false);
                var r = a(
                    Lua.Get<T1>(L, 0 + start),
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

    public struct FuncCaller<T1, T2, T3, R>
    {
        public static int Call(LuaState L) => Call(L, 1);
        public static int StaticCall(LuaState L) => Call(L, 2);

        static int Call(LuaState L, int start)
        {
            try
            {
                var a = L.ToLightObject<Func<T1, T2, T3, R>>(lua_upvalueindex(1), false);
                var r = a(
                    Lua.Get<T1>(L, 0 + start),
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

    public struct FuncCaller<T1, T2, T3, T4, R>
    {
        public static int Call(LuaState L) => Call(L, 1);
        public static int StaticCall(LuaState L) => Call(L, 2);

        static int Call(LuaState L, int start)
        {
            try
            {
                var a = L.ToLightObject<Func<T1, T2, T3, T4, R>>(lua_upvalueindex(1), false);
                var r = a(
                    Lua.Get<T1>(L, 0 + start),
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

    public struct FuncCaller<T1, T2, T3, T4, T5, R>
    {
        public static int Call(LuaState L) => Call(L, 1);
        public static int StaticCall(LuaState L) => Call(L, 2);

        static int Call(LuaState L, int start)
        {
            try
            {
                var a = L.ToLightObject<Func<T1, T2, T3, T4, T5, R>>(lua_upvalueindex(1), false);
                var r = a(
                    Lua.Get<T1>(L, 0 + start),
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


}
