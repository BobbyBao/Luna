using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace SharpLuna
{
    using lua_State = IntPtr;


    public struct LuaByteBuffer
    {
        public byte[] buffer;
        public int Length { get; }

        public LuaByteBuffer(IntPtr source, int len)
        {
            buffer = new byte[len];
            Length = len;
            Marshal.Copy(source, buffer, 0, len);
        }

        public LuaByteBuffer(byte[] buf)
        {
            buffer = buf;
            Length = buf.Length;
        }

        public LuaByteBuffer(byte[] buf, int len)
            : this()
        {
            buffer = buf;
            Length = len;
        }

        public LuaByteBuffer(System.IO.MemoryStream stream)
            : this()
        {
            buffer = stream.GetBuffer();
            Length = (int)stream.Length;
        }

        public static implicit operator LuaByteBuffer(System.IO.MemoryStream stream)
        {
            return new LuaByteBuffer(stream);
        }

    }

    public static partial class Lua
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public unsafe static void Push(lua_State L, in LuaByteBuffer v)
        {
            fixed (byte* p = &v.buffer[0])
                lua_pushlstring(L, p, v.Length);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public unsafe static void Push(lua_State L, byte[] v)
        {
            fixed (byte* p = &v[0])
                lua_pushlstring(L, p, v.Length);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Get(lua_State L, int index, out LuaByteBuffer v)
        {
            IntPtr buf = luaL_checklstring(L, index, out var sz);
            v = new LuaByteBuffer(buf, (int)sz);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Get(lua_State L, int index, out byte[] v)
        {
            IntPtr buf = luaL_checklstring(L, index, out var sz);
            v = new byte[(int)sz];          
            Marshal.Copy(buf, v, 0, (int)sz);
        }
    }

}
