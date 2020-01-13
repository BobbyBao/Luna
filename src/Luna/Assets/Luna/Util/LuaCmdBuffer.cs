using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;

namespace SharpLuna
{
    public enum LuaCmd : byte
    {
        Begin,

        End = 255,
    }

    public class LuaCmdBuffer : UnmanagedBuffer
    {
        public void Begin()
        {
            Write(LuaCmd.Begin);
        }

        public void End()
        {
            Write(LuaCmd.End);
        }

    }
}
