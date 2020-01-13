using System;
using System.Collections.Generic;
using System.Text;

namespace SharpLuna
{
    public enum LuaCompare : int
    {
        /// <summary>
        ///  compares for equality (==)
        /// </summary>
        Equal = 0,
        /// <summary>
        ///  compares for less than 
        /// </summary>
        LessThen = 1,
        /// <summary>
        /// compares for less or equal 
        /// </summary>
        LessOrEqual = 2
    }

    /// <summary>
    /// Garbage Collector operations
    /// </summary>
    public enum LuaGC
    {
        /// <summary>
        ///  Stops the garbage collector. 
        /// </summary>
        Stop = 0,
        /// <summary>
        /// Restarts the garbage collector. 
        /// </summary>
        Restart = 1,
        /// <summary>
        /// Performs a full garbage-collection cycle. 
        /// </summary>
        Collect = 2,
        /// <summary>
        ///  Returns the current amount of memory (in Kbytes) in use by Lua. 
        /// </summary>
        Count = 3,
        /// <summary>
        ///  Returns the remainder of dividing the current amount of bytes of memory in use by Lua by 1024
        /// </summary>
        Countb = 4,
        /// <summary>
        ///  Performs an incremental step of garbage collection. 
        /// </summary>
        Step = 5,
        /// <summary>
        /// Sets data as the new value for the pause of the collector (see §2.5) and returns the previous value
        /// </summary>
        SetPause = 6,
        /// <summary>
        /// sets data as the new value for the step multiplier of the collector
        /// </summary>
        SetStepMultiplier = 7,
        /// <summary>
        ///  returns a boolean that tells whether the collector is running
        /// </summary>
        IsRunning = 9,
    }

    /// <summary>
    /// Whenever a hook is called, its ar argument has its field event set to the specific event that triggered the hook
    /// </summary>
    public enum LuaHookEvent
    {
        /// <summary>
        /// The call hook: is called when the interpreter calls a function. The hook is called just after Lua enters the new function, before the function gets its arguments. 
        /// </summary>
        Call = 0,
        /// <summary>
        /// The return hook: is called when the interpreter returns from a function. The hook is called just before Lua leaves the function. There is no standard way to access the values to be returned by the function. 
        /// </summary>
        Return = 1,
        /// <summary>
        /// The line hook: is called when the interpreter is about to start the execution of a new line of code, or when it jumps back in the code (even to the same line). (This event only happens while Lua is executing a Lua function.) 
        /// </summary>
        Line = 2,
        /// <summary>
        ///  The count hook: is called after the interpreter executes every count instructions. (This event only happens while Lua is executing a Lua function.) 
        /// </summary>
        Count = 3,
        /// <summary>
        /// Tail Call
        /// </summary>
        TailCall = 4,
    }  

    /// <summary>     
    /// Lua Hook Event Masks
    /// </summary>
    [Flags]
    public enum LuaHookMask
    {
        /// <summary>
        /// Disabled hook
        /// </summary>
        Disabled = 0,
        /// <summary>
        /// The call hook: is called when the interpreter calls a function. The hook is called just after Lua enters the new function, before the function gets its arguments. 
        /// </summary>
        Call = 1 << LuaHookEvent.Call,
        /// <summary>
        /// The return hook: is called when the interpreter returns from a function. The hook is called just before Lua leaves the function. There is no standard way to access the values to be returned by the function. 
        /// </summary>
        Return = 1 << LuaHookEvent.Return,
        /// <summary>
        /// The line hook: is called when the interpreter is about to start the execution of a new line of code, or when it jumps back in the code (even to the same line). (This event only happens while Lua is executing a Lua function.) 
        /// </summary>
        Line = 1 << LuaHookEvent.Line,
        /// <summary>
        ///  The count hook: is called after the interpreter executes every count instructions. (This event only happens while Lua is executing a Lua function.) 
        /// </summary>
        Count = 1 << LuaHookEvent.Count,
    }

    /// <summary>
    /// Operation value used by Arith method
    /// </summary>
    public enum LuaOperation
    {
        /// <summary>
        /// adition(+)
        /// </summary>
        Add = 0,
        /// <summary>
        ///  substraction (-)
        /// </summary>
        Sub = 1,
        /// <summary>
        /// Multiplication (*)
        /// </summary>
        Mul = 2,

        /// <summary>
        /// Modulo (%)
        /// </summary>
        Mod = 3,

        /// <summary>
        /// Exponentiation (^)
        /// </summary>
        Pow = 4,
        /// <summary>
        ///  performs float division (/)
        /// </summary>
        Div = 5,
        /// <summary>
        ///  performs floor division (//)
        /// </summary>
        Idiv = 6,
        /// <summary>
        /// performs bitwise AND
        /// </summary>
        Band = 7,
        /// <summary>
        /// performs bitwise OR (|)
        /// </summary>
        Bor = 8,
        /// <summary>
        /// performs bitwise exclusive OR (~)
        /// </summary>
        Bxor = 9,
        /// <summary>
        /// performs left shift 
        /// </summary>
        Shl = 10,
        /// <summary>
        ///  performs right shift
        /// </summary>
        Shr = 11,
        /// <summary>
        ///  performs mathematical negation (unary -)
        /// </summary>
        Unm = 12,
        /// <summary>
        /// performs bitwise NOT (~)
        /// </summary>
        Bnot = 13,
    }

    /// <summary>
    /// Enum for pseudo-index used by registry table
    /// </summary>
    public enum LuaRegistry
    {
        /* LUAI_MAXSTACK		1000000 */
        /// <summary>
        /// pseudo-index used by registry table
        /// </summary>
        Index = -1000000 - 1000
    }

    /// <summary>
    /// Registry index 
    /// </summary>
    public enum LuaRegistryIndex
    {
        /// <summary>
        ///  At this index the registry has the main thread of the state.
        /// </summary>
        MainThread = 1,
        /// <summary>
        /// At this index the registry has the global environment. 
        /// </summary>
        Globals = 2,
    }

    /// <summary>
    /// Lua Load/Call status return
    /// </summary>
    public enum LuaStatus
    {
        /// <summary>
        ///  success
        /// </summary>
        OK = 0,
        /// <summary>
        /// Yield
        /// </summary>
        Yield = 1,
        /// <summary>
        /// a runtime error. 
        /// </summary>
        ErrRun = 2,
        /// <summary>
        /// syntax error during precompilation
        /// </summary>
        ErrSyntax = 3,
        /// <summary>
        ///  memory allocation error. For such errors, Lua does not call the message handler. 
        /// </summary>
        ErrMem = 4,
        /// <summary>
        ///  error while running a __gc metamethod. For such errors, Lua does not call the message handler
        /// </summary>
        ErrGCMM = 5,
        /// <summary>
        ///  error while running the message handler. 
        /// </summary>
        ErrErr = 6,
    }

    /// <summary>
    /// Lua types
    /// </summary>
    public enum LuaType : int
    {
        /// <summary>
        /// 
        /// </summary>
        None = -1,
        /// <summary>
        /// LUA_TNIL
        /// </summary>
        Nil = 0,
        /// <summary>
        /// LUA_TBOOLEAN
        /// </summary>
        Boolean = 1,
        /// <summary>
        /// LUA_TLIGHTUSERDATA
        /// </summary>
        LightUserData = 2,
        /// <summary>
        /// LUA_TNUMBER
        /// </summary>
        Number = 3,
        /// <summary>
        /// LUA_TSTRING
        /// </summary>
        String = 4,
        /// <summary>
        /// LUA_TTABLE
        /// </summary>
        Table = 5,
        /// <summary>
        /// LUA_TFUNCTION
        /// </summary>
        Function = 6,
        /// <summary>
        /// LUA_TUSERDATA
        /// </summary>
        UserData = 7,
        /// <summary>
        /// LUA_TTHREAD
        /// </summary>
        /// //
        Thread = 8,
    }
}
