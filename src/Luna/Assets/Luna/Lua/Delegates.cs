using System.Runtime.InteropServices;
using System.Security;

using size_t = System.UIntPtr;
using lua_State = System.IntPtr;
using voidptr_t = System.IntPtr;
using charptr_t = System.IntPtr;
using lua_KContext = System.IntPtr;
using lua_Debug = System.IntPtr;
using System;

namespace SharpLuna
{
    /// <summary>
    /// Type for C# callbacks
    /// In order to communicate properly with Lua, a C function must use the following protocol, which defines the way parameters and results are passed: a C function receives its arguments from Lua in its stack in direct order (the first argument is pushed first). So, when the function starts, lua_gettop(L) returns the number of arguments received by the function. The first argument (if any) is at index 1 and its last argument is at index lua_gettop(L). To return values to Lua, a C function just pushes them onto the stack, in direct order (the first result is pushed first), and returns the number of results. Any other value in the stack below the results will be properly discarded by Lua. Like a Lua function, a C function called by Lua can also return many results. 
    /// </summary>
    /// <param name="luaState"></param>
    /// <returns></returns>
    [SuppressUnmanagedCodeSecurity]
    [UnmanagedFunctionPointer (CallingConvention.Cdecl)]
    public delegate int LuaNativeFunction(LuaState luaState);

    /// <summary>
    /// Type for debugging hook functions callbacks. 
    /// </summary>
    /// <param name="luaState"></param>
    /// <param name="ar"></param>
    [SuppressUnmanagedCodeSecurity]
    [UnmanagedFunctionPointer (CallingConvention.Cdecl)]
    public delegate void KyHookFunction (LuaState luaState, lua_Debug ar);

    /// <summary>
    /// Type for continuation functions 
    /// </summary>
    /// <param name="L"></param>
    /// <param name="status"></param>
    /// <param name="ctx"></param>
    /// <returns></returns>
    [SuppressUnmanagedCodeSecurity]
    [UnmanagedFunctionPointer (CallingConvention.Cdecl)]
    public delegate int LuaKFunction (LuaState L, int status, lua_KContext ctx);

    /// <summary>
    /// The reader function used by lua_load. Every time it needs another piece of the chunk, lua_load calls the reader, passing along its data parameter. The reader must return a pointer to a block of memory with a new piece of the chunk and set size to the block size
    /// </summary>
    /// <param name="L"></param>
    /// <param name="ud"></param>
    /// <param name="sz"></param>
    /// <returns></returns>
    [SuppressUnmanagedCodeSecurity]
    [UnmanagedFunctionPointer (CallingConvention.Cdecl)]
    public delegate charptr_t LuaReader (LuaState L, voidptr_t ud, ref size_t sz);

    /// <summary>
    /// 
    /// </summary>
    /// <param name="L"></param>
    /// <param name="p"></param>
    /// <param name="size"></param>
    /// <param name="ud"></param>
    /// <returns></returns>
    [SuppressUnmanagedCodeSecurity]
    [UnmanagedFunctionPointer (CallingConvention.Cdecl)]
    public delegate int LuaWriter (LuaState L, voidptr_t p, size_t size, voidptr_t ud);

    /// <summary>
    /// The type of the memory-allocation function used by Lua states. The allocator function must provide a functionality similar to realloc
    /// </summary>
    /// <param name="ud"></param>
    /// <param name="ptr"></param>
    /// <param name="osize"></param>
    /// <param name="nsize"></param>
    /// <returns></returns>
    [SuppressUnmanagedCodeSecurity]
    [UnmanagedFunctionPointer (CallingConvention.Cdecl)]
    public delegate voidptr_t LuaAlloc (voidptr_t ud, voidptr_t ptr, size_t osize, size_t nsize);

    static class DelegateExtensions
    {
        public static LuaNativeFunction ToLuaFunction(this IntPtr ptr)
        {
            if (ptr == IntPtr.Zero)
                return null;
#if NETFRAMEWORK
            return (LuaFunction) Marshal.GetDelegateForFunctionPointer(ptr, typeof(LuaFunction));
#else
            return Marshal.GetDelegateForFunctionPointer<LuaNativeFunction>(ptr);
#endif
        }

        public static IntPtr ToFunctionPointer(this LuaNativeFunction d)
        {
            if (d == null)
                return IntPtr.Zero;

#if NETFRAMEWORK
            return Marshal.GetFunctionPointerForDelegate(d);
#else
            return Marshal.GetFunctionPointerForDelegate<LuaNativeFunction>(d);
#endif
        }

        public static KyHookFunction ToLuaHookFunction(this IntPtr ptr)
        {
            if (ptr == IntPtr.Zero)
                return null;

#if NETFRAMEWORK
            return (LuaHookFunction) Marshal.GetDelegateForFunctionPointer(ptr, typeof(LuaHookFunction));
#else
            return Marshal.GetDelegateForFunctionPointer<KyHookFunction>(ptr);
#endif
        }

        public static IntPtr ToFunctionPointer(this KyHookFunction d)
        {
            if (d == null)
                return IntPtr.Zero;

#if NETFRAMEWORK
            return Marshal.GetFunctionPointerForDelegate(d);
#else
            return Marshal.GetFunctionPointerForDelegate<KyHookFunction>(d);
#endif
        }

        public static LuaKFunction ToLuaKFunction(this IntPtr ptr)
        {
            if (ptr == IntPtr.Zero)
                return null;
#if NETFRAMEWORK
            return (LuaKFunction) Marshal.GetDelegateForFunctionPointer(ptr, typeof(LuaKFunction));
#else
            return Marshal.GetDelegateForFunctionPointer<LuaKFunction>(ptr);
#endif
        }

        public static IntPtr ToFunctionPointer(this LuaKFunction d)
        {
            if (d == null)
                return IntPtr.Zero;

#if NETFRAMEWORK
            return Marshal.GetFunctionPointerForDelegate(d);
#else
            return Marshal.GetFunctionPointerForDelegate<LuaKFunction>(d);
#endif
        }

        public static LuaReader ToLuaReader(this IntPtr ptr)
        {
            if (ptr == IntPtr.Zero)
                return null;
#if NETFRAMEWORK
            return (LuaReader) Marshal.GetDelegateForFunctionPointer(ptr, typeof(LuaReader));
#else
            return Marshal.GetDelegateForFunctionPointer<LuaReader>(ptr);
#endif
        }

        public static IntPtr ToFunctionPointer(this LuaReader d)
        {
            if (d == null)
                return IntPtr.Zero;

#if NETFRAMEWORK
            return Marshal.GetFunctionPointerForDelegate(d);
#else
            return Marshal.GetFunctionPointerForDelegate<LuaReader>(d);
#endif
        }

        public static LuaWriter ToLuaWriter(this IntPtr ptr)
        {
            if (ptr == IntPtr.Zero)
                return null;
#if NETFRAMEWORK
            return (LuaWriter) Marshal.GetDelegateForFunctionPointer(ptr, typeof(LuaWriter));
#else
            return Marshal.GetDelegateForFunctionPointer<LuaWriter>(ptr);
#endif
        }

        public static IntPtr ToFunctionPointer(this LuaWriter d)
        {
            if (d == null)
                return IntPtr.Zero;

#if NETFRAMEWORK
            return Marshal.GetFunctionPointerForDelegate(d);
#else
            return Marshal.GetFunctionPointerForDelegate<LuaWriter>(d);
#endif
        }

        public static LuaAlloc ToLuaAlloc(this IntPtr ptr)
        {
            if (ptr == IntPtr.Zero)
                return null;
#if NETFRAMEWORK
            return (LuaAlloc) Marshal.GetDelegateForFunctionPointer(ptr, typeof(LuaAlloc));
#else
            return Marshal.GetDelegateForFunctionPointer<LuaAlloc>(ptr);
#endif
        }

        public static IntPtr ToFunctionPointer(this LuaAlloc d)
        {
            if (d == null)
                return IntPtr.Zero;

#if NETFRAMEWORK
            return Marshal.GetFunctionPointerForDelegate(d);
#else
            return Marshal.GetFunctionPointerForDelegate<LuaAlloc>(d);
#endif
        }
    }
}
