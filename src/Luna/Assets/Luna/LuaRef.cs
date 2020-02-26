using SharpLuna;
using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Reflection;
using System.Collections;

namespace SharpLuna
{
    using lua_State = System.IntPtr;
    using static Lua;

    public partial class LuaRef : IEquatable<LuaRef>, IComparable<LuaRef>, IDisposable
    {
        private readonly lua_State L;
        private readonly int _ref;

        //public static readonly LuaRef Empty = new LuaRef(0);
        public static readonly LuaRef None = new LuaRef(LUA_NOREF, IntPtr.Zero);
        public static readonly LuaRef Nil = new LuaRef(LUA_REFNIL, IntPtr.Zero);

        public LuaRef(lua_State state, int index)
        {
            L = state;
            lua_pushvalue(L, index);
            _ref = luaL_ref(L, LUA_REGISTRYINDEX);
            state.AddRef(this);
        }

        public LuaRef(lua_State state, string name)
        {
            L = state;
            PushGlobal(L, name);
            _ref = luaL_ref(L, LUA_REGISTRYINDEX);
            state.AddRef(this);
        }

        public LuaRef(lua_State state)
        {
            L = state;
            _ref = luaL_ref(state, LUA_REGISTRYINDEX);
            state.AddRef(this);
        }

        public LuaRef(int luaRef, IntPtr state)
        {
            L = state;
            _ref = luaRef;

            if(state != IntPtr.Zero)
            {
                state.AddRef(this);
            }
        }

        ~LuaRef()
        {
            if (L.IsActive())
            {
                if (_ref != LUA_NOREF)
                {
                    luaL_unref(L, LUA_REGISTRYINDEX, _ref);
                    L.RemoveRef(this);
                }
            }
        }

        public void Dispose()
        {
            if (L.IsActive())
            {
                if (_ref != LUA_NOREF)
                {
                    luaL_unref(L, LUA_REGISTRYINDEX, _ref);
                    L.RemoveRef(this);
                }
            }

            GC.SuppressFinalize(this);
        }
        
        public lua_State State => L;
        public int Ref => _ref;
        public bool IsValid => _ref != LUA_NOREF;
        public bool IsTable => Type == LuaType.Table;
        public bool IsFunction => Type == LuaType.Function;

        public LuaType Type
        {
            get
            {
                if (_ref == LUA_NOREF)
                {
                    return LuaType.None;
                }
                else if (_ref == LUA_REFNIL)
                {
                    return LuaType.Nil;
                }
                else
                {
                    PushToStack();
                    LuaType t = lua_type(L, -1);
                    lua_pop(L, 1);
                    return (LuaType)(t);
                }
            }
        }

        public string TypeName
        {
            get
            {
                PushToStack();
                var s = lua_typename(L, lua_type(L, -1));
                lua_pop(L, 1);
                return s;
            }
        }

        public override int GetHashCode()
        {
            PushToStack();
            var pointer = lua_topointer(L, -1);
            lua_pop(L, 1);
            return pointer.ToInt32();
        }

        public int CompareTo(LuaRef r)
        {
            PushToStack();
            r.PushToStack();
            int d = lua_compare(L, -2, -1, LuaCompare.Equal)
                ? 0 : (lua_compare(L, -2, -1, LuaCompare.LessThen) ? -1 : 1);
            lua_pop(L, 2);
            return d;
        }

        public bool Equals(LuaRef other)
        {
            if(other == null)
            {
                return this == null;
            }

            PushToStack();
            other.PushToStack();
            bool b = lua_compare(L, -2, -1, LuaCompare.Equal) != 0;
            lua_pop(L, 2);
            return b;
        }

        public override bool Equals(object obj)
        {
            if (obj is LuaRef luaRef)
            {
                return this.Equals(luaRef);
            }
            return false;
        }

        public static bool operator < (LuaRef l, LuaRef r)
        {
            l.PushToStack();
            r.PushToStack();
            bool b = lua_compare(l.L, -2, -1, LuaCompare.LessThen) != 0;
            lua_pop(l.L, 2);
            return b;
        }

        public static bool operator <= (LuaRef l, LuaRef r)
        {
            l.PushToStack();
            r.PushToStack();
            bool b = lua_compare(l.L, -2, -1, LuaCompare.LessOrEqual) != 0;
            lua_pop(l.L, 2);
            return b;
        }

        public static bool operator > (LuaRef l, LuaRef r) => !(l <= r);
        public static bool operator >= (LuaRef l, LuaRef r) => !(l < r);

        public static implicit operator bool(LuaRef luaRef)
        {
            return luaRef != null && Lua.IsActive(luaRef.L) && luaRef._ref != LUA_NOREF && luaRef._ref != LUA_REFNIL;
        }

        public override string ToString()
        {
            if (_ref == 0)
            {
                return "Empty";
            }

            LuaType t = Type;
            switch (t)
            {
                case LuaType.Boolean:
                    return ToValue<bool>().ToString();
                case LuaType.LightUserData:
                    try
                    {
                        return Marshal.PtrToStringAnsi(ToPtr());
                    }
                    catch
                    {
                        return ToPtr().ToString();
                    }
                case LuaType.Number:
                    return ToValue<double>().ToString();
                case LuaType.String:
                    return ToValue<string>();
                case LuaType.Table:
                    return "Table:" + GetHashCode();
                case LuaType.Function:
                    return "Function:" + GetHashCode();
                case LuaType.UserData:
                    return "UserData:" + GetHashCode();
                case LuaType.Thread:
                    return "Thread:" + GetHashCode();
                default:
                    return "nil";
            }

        }

        public static LuaRef Registry(lua_State L)
        {
            return new LuaRef(L, LUA_REGISTRYINDEX);
        }

        public static LuaRef Globals(lua_State L)
        {
            lua_pushglobaltable(L);
            return PopFromStack(L);
        }

        public LuaRef CheckTable()
        {
            return CheckType(LuaType.Table);
        }

        public LuaRef CheckFunction()
        {
            return CheckType(LuaType.Function);
        }

        public LuaRef CheckType(LuaType type)
        {
            PushToStack();
            luaL_checktype(L, -1, (int)(type));
            lua_pop(L, 1);
            return this;
        }

        public void PushToStack()
        {
            lua_rawgeti(L, LUA_REGISTRYINDEX, _ref);
        }

        public static LuaRef PopFromStack(lua_State L)
        {
            return new LuaRef(L);
        }

        public T ToValue<T>()
        {
            PushToStack();
            return Lua.Pop<T>(L);
        }

        public static LuaRef FromValue<T>(lua_State L, T value)
        {
            Lua.PushT<T>(L, value);
            return PopFromStack(L);
        }

        public IntPtr ToPtr()
        {
            PushToStack();
            IntPtr ptr = lua_touserdata(L, -1);
            lua_pop(L, 1);
            return ptr;
        }

        public static LuaRef FromPtr(lua_State L, IntPtr ptr)
        {
            lua_pushlightuserdata(L, ptr);
            return PopFromStack(L);
        }

        public static LuaRef CreateUserData(lua_State L, int userdata_size, out IntPtr out_userdata)
        {
            IntPtr userdata = lua_newuserdata(L, (UIntPtr)userdata_size);
            out_userdata = userdata;
            return PopFromStack(L);
        }

        public static LuaRef CreateUserDataFrom<T>(lua_State L, T obj) where T : class
        {
            PushUserData(L, obj);
            return PopFromStack(L);
        }

        static unsafe void PushUserData<T>(lua_State L, T obj)
        {
            IntPtr userdata = lua_newuserdata(L, (UIntPtr)sizeof(IntPtr));
            GCHandle gc = GCHandle.Alloc(obj);
            Unsafe.Write((void*)userdata, GCHandle.ToIntPtr(gc));

            lua_pushcfunction(L, DestructUserData);
            lua_setfield(L, -2, "__gc");
            lua_setmetatable(L, -2);
        }

        static unsafe int DestructUserData(lua_State L)
        {
            IntPtr obj = lua_touserdata(L, 1);
            IntPtr handle = Unsafe.Read<IntPtr>((void*)obj);
            var gcHandle = GCHandle.FromIntPtr(handle);
            if (gcHandle.IsAllocated)
            {
                gcHandle.Free();
            }
            return 0;
        }

        public static LuaRef CreateFunctionWith(lua_State L, LuaNativeFunction proc, params object[] upvalues)
        {
            PushArgs(L, upvalues);
            lua_pushcclosure(L, proc, upvalues.Length);
            return PopFromStack(L);
        }

        public static LuaRef CreateFunction(lua_State L, LuaNativeFunction proc, object obj)
        {
            GCHandle gC = GCHandle.Alloc(obj);
            lua_pushlightuserdata(L, GCHandle.ToIntPtr(gC));
            lua_pushcclosure(L, proc, 1);
            return PopFromStack(L);
        }

        public static LuaRef CreateFunction(lua_State L, LuaNativeFunction proc)
        {
            lua_pushcclosure(L, proc, 0);
            return PopFromStack(L);
        }

        public static LuaRef CreateFunctionUnmanaged<T>(lua_State L, LuaNativeFunction proc, T valueObj)
        {
            PushUserData<T>(L, valueObj);
            lua_pushcclosure(L, proc, 1);
            return PopFromStack(L);
        }

        public void Call(params object[] args)
        {
            Invoke(L, this, args);
        }

        public void Dispatch(string func, params object[] args)
        {
            Invoke(L, Get<LuaRef, string>(func), args);
        }

        public R Call<R>(params object[] args)
        {           
            return Invoke<R>(L, this, args);
        }

        public R Dispatch<R>(string func, params object[] args)
        {
            return Invoke<R>(L, Get<LuaRef, string>(func), args);
        }
        
        static void Invoke(lua_State L, LuaRef f, params object[] args)
        {
            lua_pushcfunction(L, LuaException.traceback);
            f.PushToStack();
            PushArgs(L, args);
            if (lua_pcall(L, args.Length, 0, -args.Length + 2) != (int)LuaStatus.OK)
            {
                lua_remove(L, -2);
                throw new LuaException(L);
            }
            lua_pop(L, 1);
        }

        static R Invoke<R>(lua_State L, LuaRef f, params object[] args)
        {
            lua_pushcfunction(L, LuaException.traceback);
            f.PushToStack();
            PushArgs(L, args);
            if (lua_pcall(L, args.Length, 1, -args.Length + 2) != (int)LuaStatus.OK)
            {
                lua_remove(L, -2);
                throw new LuaException(L);
            }
            R v = Lua.Get<R>(L, -1);
            lua_pop(L, 2);
            return v;
        }
     
        
    }


}
