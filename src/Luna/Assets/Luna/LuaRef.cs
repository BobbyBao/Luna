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

    public struct LuaRef : IRefCount, IEquatable<LuaRef>, IComparable<LuaRef>, IEnumerable<TableKeyValuePair>
    {
        private readonly lua_State L;
        private readonly int _ref;

        public static readonly LuaRef Empty = new LuaRef();
        public static readonly LuaRef None = new LuaRef(LUA_NOREF);
        public static readonly LuaRef Nil = new LuaRef(LUA_REFNIL);
        
        public LuaRef(lua_State state, int index)
        {
            L = state;
            lua_pushvalue(L, index);
            _ref = luaL_ref(L, LUA_REGISTRYINDEX);
            Handle = 0;
            Handle = this.Alloc();
        }

        public LuaRef(lua_State state, string name)
        {
            L = state;
            PushGlobal(L, name);
            _ref = luaL_ref(L, LUA_REGISTRYINDEX);
            Handle = 0;
            Handle = this.Alloc();
        }

        public LuaRef(lua_State state)
        {
            L = state;
            _ref = luaL_ref(state, LUA_REGISTRYINDEX); 
            Handle = 0;
            Handle = this.Alloc();
        }

        private LuaRef(int luaRef)
        {
            L = IntPtr.Zero;
            _ref = luaRef;
            Handle = 0;
        }

        public void Dispose()
        {
            this.Release();
        }

        public uint Handle { get; set; }

        public void InternalRelease()
        {
            if (L != IntPtr.Zero)
            {
                luaL_unref(L, LUA_REGISTRYINDEX, _ref);
            }
        }

        public lua_State State => L;
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

        public static LuaRef CreateFunctionWith(lua_State L, LuaNativeFunction proc, params object[] upvalues)
        {
            PushArg(L, upvalues);
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

        public static LuaRef CreateTable(lua_State L, int narr = 0, int nrec = 0)
        {
            lua_createtable(L, narr, nrec);
            return PopFromStack(L);
        }

        public static LuaRef CreateTableWithMeta(lua_State L, string meta)
        {
            lua_newtable(L);
            Lua.PushGlobal(L, meta);
            lua_setmetatable(L, -2);
            return PopFromStack(L);
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

        public override int GetHashCode()
        {
            var hashCode = 626460037;
            hashCode = hashCode * -1521134295 + L.GetHashCode();
            hashCode = hashCode * -1521134295 + _ref.GetHashCode();
            return hashCode;
        }

        public int CompareTo(LuaRef r)
        {
            PushToStack();
            r.PushToStack();
            int d = lua_compare(L, -2, -1, LuaCompare.Equal)
                ? 0
                : (lua_compare(L, -2, -1, LuaCompare.LessThen) ? -1 : 1);
            lua_pop(L, 2);
            return d;
        }

        public bool Equals(LuaRef other)
        {
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

        public static bool operator <(in LuaRef l, in LuaRef r)
        {
            l.PushToStack();
            r.PushToStack();
            bool b = lua_compare(l.L, -2, -1, LuaCompare.LessThen) != 0;
            lua_pop(l.L, 2);
            return b;
        }

        public static bool operator <= (in LuaRef l, in LuaRef r)
        {
            l.PushToStack();
            r.PushToStack();
            bool b = lua_compare(l.L, -2, -1, LuaCompare.LessOrEqual) != 0;
            lua_pop(l.L, 2);
            return b;
        }

        public static bool operator ==(in LuaRef l, in LuaRef r) => l.Equals(r);
        public static bool operator !=(in LuaRef l, in LuaRef r) => !l.Equals(r);
        public static bool operator >(in LuaRef l, in LuaRef r) => !(l <= r);
        public static bool operator >=(in LuaRef l, in LuaRef r) => !(l < r);

        public static implicit operator bool(LuaRef luaRef)
        {
            return luaRef.L != IntPtr.Zero && luaRef._ref != LUA_NOREF && luaRef._ref != LUA_REFNIL;
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
            Lua.Push<T>(L, value);
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

        public LuaRef GetMetaTable()
        {
            LuaRef meta = LuaRef.None;
            PushToStack();
            if (lua_getmetatable(L, -1) != 0)
            {
                meta = PopFromStack(L);
            }
            lua_pop(L, 1);
            return meta;
        }

        public void SetMetaTable(LuaRef meta)
        {
            PushToStack();
            meta.PushToStack();
            lua_setmetatable(L, -2);
            lua_pop(L, 1);
        }

        public V RawGet<V, K>(K key)
        {
            PushToStack();
            Lua.Push<K>(L, key);
            lua_rawget(L, -2);
            V v = Lua.Get<V>(L, -1);
            lua_pop(L, 2);
            return v;
        }

        public LuaRef RawGet<K>(K key)
        {
            PushToStack();
            Lua.Push<K>(L, key);
            lua_rawget(L, -2);
            LuaRef v = Lua.Get<LuaRef>(L, -1);
            lua_pop(L, 2);
            return v;
        }

        public V RawGet<V, K>(K key, V def)
        {
            PushToStack();
            Lua.Push(L, key);
            lua_rawget(L, -2);
            V v = Lua.Opt<V>(L, -1, def);
            lua_pop(L, 2);
            return v;
        }

        public void RawSet<K, V>(K key, V value)
        {
            PushToStack();
            Lua.Push(L, key);
            Lua.Push(L, value);
            lua_rawset(L, -3);
            lua_pop(L, 1);
        }

        public V RawGetP<V>(IntPtr p)
        {
            PushToStack();
            lua_rawgetp(L, -1, p);
            V v = Lua.Get<V>(L, -1);
            lua_pop(L, 2);
            return v;
        }

        public LuaRef RawGet(IntPtr p)
        {
            return RawGetP<LuaRef>(p);
        }

        public V RawGet<V>(IntPtr p)
        {
            return RawGetP<V>(p);
        }

        public V RawGetP<V>(IntPtr p, V def)
        {
            PushToStack();
            lua_rawgetp(L, -1, p);
            V v = Lua.Opt<V>(L, -1, def);
            lua_pop(L, 2);
            return v;
        }

        public V RawGet<V>(IntPtr p, V def)
        {
            return RawGetP(p, def);
        }

        public void RawSetP<V>(IntPtr p, V value)
        {
            PushToStack();
            Lua.Push(L, value);
            lua_rawsetp(L, -2, p);
            lua_pop(L, 1);
        }

        public void RawSet<V>(IntPtr p, V value)
        {
            RawSetP(p, value);
        }

        public V RawGet<V>(int i)
        {
            PushToStack();
            lua_rawgeti(L, -1, i);
            V v = Lua.Get<V>(L, -1);
            lua_pop(L, 2);
            return v;
        }

        public V RawGet<V>(int i, V def)
        {
            PushToStack();
            lua_rawgeti(L, -1, i);
            V v = Lua.Opt<V>(L, -1, def);
            lua_pop(L, 2);
            return v;
        }

        public void RawSet<V>(int i, V value)
        {
            PushToStack();
            Lua.Push(L, value);
            lua_rawseti(L, -2, i);
            lua_pop(L, 1);
        }

        public int RawLen()
        {
            PushToStack();
            int n = (int)(lua_rawlen(L, -1));
            lua_pop(L, 1);
            return n;
        }

        public bool Has<K>(K key)
        {
            PushToStack();
            Lua.Push(L, key);
            lua_gettable(L, -2);
            bool ok = !lua_isnoneornil(L, -1);
            lua_pop(L, 2);
            return ok;
        }

        public LuaRef Get<K>(K key)
        {
            return Get<LuaRef, K>(key);
        }
         
        public V Get<V, K>(K key)
        {
            PushToStack();
            Lua.Push(L, key);
            lua_gettable(L, -2);
            V t = Lua.Get<V>(L, -1);
            lua_pop(L, 2);
            return t;
        }

        public V Get<V, K>(K key, V def)
        {
            PushToStack();
            Lua.Push(L, key);
            lua_gettable(L, -2);
            V t = Lua.Opt<V>(L, -1, def);
            lua_pop(L, 2);
            return t;
        }

        public void Set<K, V>(K key, V value)
        {
            PushToStack();
            Lua.Push(L, key);
            Lua.Push(L, value);
            lua_settable(L, -3);
            lua_pop(L, 1);
        }

        public void Remove<K>(K key)
        {
            PushToStack();
            Lua.Push(L, key);
            lua_pushnil(L);
            lua_settable(L, -3);
            lua_pop(L, 1);
        }

        public int Len()
        {
            PushToStack();
            int n = (int)(luaL_len(L, -1));
            lua_pop(L, 1);
            return n;
        }

        public LuaTableRef this[object key]
        {
            get
            {
                Lua.Push(L, key);
                return new LuaTableRef(L, _ref, luaL_ref(L, LUA_REGISTRYINDEX));
            }
        }

        public LuaTableRef this[int key]
        {
            get
            {
                Lua.Push(L, key);
                return new LuaTableRef(L, _ref, luaL_ref(L, LUA_REGISTRYINDEX));
            }
        }

        public LuaTableRef this[string key]
        {
            get
            {
                Lua.Push(L, key);
                return new LuaTableRef(L, _ref, luaL_ref(L, LUA_REGISTRYINDEX));
            }
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
            if(gcHandle.IsAllocated)
            {
                gcHandle.Free();
            }
            return 0;
        }

        static void Invoke(lua_State L, LuaRef f, params object[] args)
        {
            lua_pushcfunction(L, LuaException.traceback);
            f.PushToStack();
            PushArg(L, args);
            if (lua_pcall(L, args.Length, 1, -args.Length + 2) != (int)LuaStatus.OK)
            {
                lua_remove(L, -2);
                throw new LuaException(L);
            }
        }

        static R Invoke<R>(lua_State L, LuaRef f, params object[] args)
        {
            lua_pushcfunction(L, LuaException.traceback);
            f.PushToStack();
            PushArg(L, args);
            if (lua_pcall(L, args.Length, 1, -args.Length + 2) != (int)LuaStatus.OK)
            {
                lua_remove(L, -2);
                throw new LuaException(L);
            }
            R v = Lua.Get<R>(L, -1);
            lua_pop(L, 2);
            return v;
        }
     
        static void PushArg(lua_State L, params object[] args)
        {
            foreach (var obj in args)
            {
                Lua.Push(L, obj);
            }
        }
        
        public IEnumerator<TableKeyValuePair> GetEnumerator()
        {
            return new LuaTableEnumerator(L, _ref);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public override string ToString()
        {
            if (_ref == 0)
            {
                return "Empty";
            }

            LuaType t = Type;
            switch(t)
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
                    return "Table";
                case LuaType.Function:
                    return "Function";
                case LuaType.UserData:
                    return "UserData";
                case LuaType.Thread:
                    return "Thread";
                default:
                    return "nil";
            }

        }
    }

    public struct LuaTableRef : IRefCount
    {
        lua_State L;
        int _table;
        int _key;

        public LuaTableRef(lua_State state, int table, int key)
        {
            L = state;
            _table = table;
            _key = key;
            Handle = 0;
            Handle = this.Alloc();
        }

        public uint Handle { get; set; }

        public void Dispose()
        {
            this.Release();
        }

        public void InternalRelease()
        {
            luaL_unref(L, LUA_REGISTRYINDEX, _key);
        }

        public K Key<K>()
        {
            lua_rawgeti(L, LUA_REGISTRYINDEX, _key);
            return Lua.Pop<K>(L);
        }

        public void Set<V>(V value)
        {
            lua_rawgeti(L, LUA_REGISTRYINDEX, _table);
            lua_rawgeti(L, LUA_REGISTRYINDEX, _key);
            Lua.Push(L, value);
            lua_settable(L, -3);
            lua_pop(L, 1);
        }

        public T Value<T>()
        {
            lua_rawgeti(L, LUA_REGISTRYINDEX, _table);
            lua_rawgeti(L, LUA_REGISTRYINDEX, _key);
            lua_gettable(L, -2);
            T v = Lua.Get<T>(L, -1);
            lua_pop(L, 2);
            return v;
        }
    }

    public struct TableKeyValuePair
    {
        readonly int key;
        readonly int value;

        readonly lua_State L;

        public TableKeyValuePair(lua_State state, int k, int v)
        {
            L = state;
            key = k;
            value = v;
        }

        public LuaRef Key()
        {
            lua_rawgeti(L, LUA_REGISTRYINDEX, key);
            return Lua.Pop<LuaRef>(L);
        }

        public LuaRef Value()
        {
            lua_rawgeti(L, LUA_REGISTRYINDEX, value);
            return Lua.Pop<LuaRef>(L);
        }

        public K Key<K>()
        {
            lua_rawgeti(L, LUA_REGISTRYINDEX, key);
            return Lua.Pop<K>(L);
        }

        public V Value<V>()
        {
            lua_rawgeti(L, LUA_REGISTRYINDEX, value);
            return Lua.Pop<V>(L);
        }

    }

    public struct LuaTableEnumerator : IEnumerator<TableKeyValuePair>, IRefCount
    {
        lua_State L;
        int _table;
        int _key;
        int _value;

        public TableKeyValuePair Current => new TableKeyValuePair(L, _key, _value);
        object IEnumerator.Current => (TableKeyValuePair)Current;

        public LuaTableEnumerator(lua_State state, int table)
        {
            L = state;
            _table = table;
            _key = LUA_NOREF;
            _value = LUA_NOREF;
            Handle = 0;
            Handle = this.Alloc();
        }

        public bool MoveNext()
        {
            lua_rawgeti(L, LUA_REGISTRYINDEX, _table);
            lua_rawgeti(L, LUA_REGISTRYINDEX, _key);
            luaL_unref(L, LUA_REGISTRYINDEX, _key);
            luaL_unref(L, LUA_REGISTRYINDEX, _value);
            bool ret = false;
            if (lua_next(L, -2))
            {
                _value = luaL_ref(L, LUA_REGISTRYINDEX);
                _key = luaL_ref(L, LUA_REGISTRYINDEX);
                ret = true;
            }
            else
            {
                _value = LUA_NOREF;
                _key = LUA_NOREF;
                ret = false;
            }
            lua_pop(L, 1);
            return ret;
        }

        public void Reset()
        {
            luaL_unref(L, LUA_REGISTRYINDEX, _key);
            luaL_unref(L, LUA_REGISTRYINDEX, _value);
            _key = LUA_NOREF;
            _value = LUA_NOREF;
        }

        public uint Handle { get; set; }

        public void Dispose()
        {
            this.Release();
        }

        public void InternalRelease()
        {
            if (L != IntPtr.Zero)
            {
                luaL_unref(L, LUA_REGISTRYINDEX, _key);
                luaL_unref(L, LUA_REGISTRYINDEX, _value);
            }
        }

    }

}
