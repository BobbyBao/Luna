using System;

namespace SharpLuna
{
    using static Lua;
    /// <summary>
    /// Exceptions thrown by the Lua runtime
    /// </summary>
    [Serializable]
	public class LuaException : Exception
	{
		public LuaException (string message) : base(message)
		{
		}

		public LuaException (string message, Exception innerException) : base(message, innerException)
		{
		}

        public LuaException(IntPtr L) 
            : base((lua_gettop(L) > 0) ? lua_tostring(L, -1) : "unknown error")
        {
        }

        public static int traceback(IntPtr L)
        {
            if (!lua_isstring(L, 1)) return 1;

            lua_getglobal(L, "debug");
            if (!lua_istable(L, -1))
            {
                lua_pop(L, 1);
                return 1;
            }
            lua_getfield(L, -1, "traceback");
            if (!lua_isfunction(L, -1))
            {
                lua_pop(L, 2);
                return 1;
            }
            lua_pushvalue(L, 1);    // pass error message
            lua_call(L, 1, 1);      // call debug.traceback
            return 1;
        }
    }
}