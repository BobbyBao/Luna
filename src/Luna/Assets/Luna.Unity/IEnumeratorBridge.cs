using System;

namespace SharpLuna
{

    using static Lua;
    using lua_State = System.IntPtr;

    public class IEnumeratorBridge : LuaRef, System.Collections.IEnumerator
    {
        public static System.Collections.IEnumerator Create(lua_State L, int index)
        {
            return new IEnumeratorBridge(L, index);
        }

        public IEnumeratorBridge(lua_State L, int index) : base(L, index)
        {
        }

        bool System.Collections.IEnumerator.MoveNext()
        {
            int err_func = load_error_func(L, errorFuncRef);

            lua_getref(L, _ref);
            lua_pushstring(L, "moveNext");

            if (LuaStatus.OK != xlua_pgettable(L, -2))
            {
                ThrowExceptionFromError(L, err_func - 1);
            }

            if (!lua_isfunction(L, -1))
            {
                lua_pushstring(L, "no such function MoveNext");
                ThrowExceptionFromError(L, err_func - 1);
            }

            lua_pushvalue(L, -2);
            lua_remove(L, -3);

            var __gen_error = lua_pcall(L, 1, 1, err_func);
            if (__gen_error != 0)
                ThrowExceptionFromError(L, err_func - 1);

            int __gen_ret = lua_toboolean(L, err_func + 1);
            lua_settop(L, err_func - 1);
            return __gen_ret != 0;
        }

        [AOT.MonoPInvokeCallback(typeof(LuaNativeFunction))]
        static int c_lua_gettable(lua_State L)
        {
            lua_gettable(L, 1);
            return 1;
        }

        static LuaStatus xlua_pgettable(lua_State L, int idx)
        {
            int top = lua_gettop(L);
            idx = lua_absindex(L, idx);
            lua_pushcfunction(L, c_lua_gettable);
            lua_pushvalue(L, idx);
            lua_pushvalue(L, top);
            lua_remove(L, top);
            return lua_pcall(L, 2, 1, 0);
        }

        void System.Collections.IEnumerator.Reset()
        {
            int err_func = load_error_func(L, errorFuncRef);
            
            lua_getref(L, _ref);
            lua_pushstring(L, "reset");

            if(LuaStatus.OK != xlua_pgettable(L, -2))
            {
                ThrowExceptionFromError(L, err_func - 1);
            }

            if (!lua_isfunction(L, -1))
            {
                lua_pushstring(L, "no such function Reset");
                ThrowExceptionFromError(L, err_func - 1);
            }

            lua_pushvalue(L, -2);
            lua_remove(L, -3);

            var __gen_error = lua_pcall(L, 1, 0, err_func);
            if (__gen_error != 0)
                ThrowExceptionFromError(L, err_func - 1);

            lua_settop(L, err_func - 1);
        }



        object System.Collections.IEnumerator.Current
        {

            get
            {
                int oldTop = lua_gettop(L);
                lua_getref(L, _ref);
                lua_pushstring(L, "current");

                if (LuaStatus.OK != xlua_pgettable(L, -2))
                {
                    ThrowExceptionFromError(L, oldTop);
                }

                object __gen_ret = GetObject(L, -1);
                lua_pop(L, 2);
                return __gen_ret;
            }


        }





    }


}