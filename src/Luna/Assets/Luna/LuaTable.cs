using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace SharpLuna
{
    using lua_State = System.IntPtr;
    using static Lua;

    public partial class LuaRef : IEnumerable<TableKeyValuePair>
    {
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

        public object this[int index]
        {
            get
            {
                return Get(index);
            }

            set
            {
                Set(index, value);
            }
        }

        public object this[string index]
        {
            get
            {
                return Get(index);
            }

            set
            {
                Set(index, value);
            }
        }

        public object this[object index]
        {
            get
            {
                return Get(index);
            }

            set
            {
                Set(index, value);
            }
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
        
        public IEnumerator<TableKeyValuePair> GetEnumerator()
        {
            return new LuaTableEnumerator(L, _ref);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
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

    public class LuaTableEnumerator : IEnumerator<TableKeyValuePair>
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
            L.AddRef(this);
        }

        ~LuaTableEnumerator()
        {
            if (L.IsActive())
            {
                luaL_unref(L, LUA_REGISTRYINDEX, _key);
                luaL_unref(L, LUA_REGISTRYINDEX, _value);
                L.RemoveRef(this);
            }
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

        public void Dispose()
        {
            if (L != IntPtr.Zero)
            {
                luaL_unref(L, LUA_REGISTRYINDEX, _key);
                luaL_unref(L, LUA_REGISTRYINDEX, _value);
                L.RemoveRef(this);
            }

            GC.SuppressFinalize(this);
        }

    }
}
