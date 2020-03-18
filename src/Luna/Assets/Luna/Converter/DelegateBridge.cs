using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpLuna
{
    using static Lua;
    using lua_State = IntPtr;

    public class DelegateBridge<T> : TConverter<T> where T : class
    {
        Dictionary<IntPtr, WeakReference<T>> delegateCache = new Dictionary<IntPtr, WeakReference<T>>(); 
        Func<IntPtr, int, T> fnCreate;
        public DelegateBridge(Func<IntPtr, int, T> create) 
        {
            this.type = typeof(T);
            this.getter = Create;
            this.fnCreate = create;
        }

        object Create(IntPtr L, int index)
        {
            var type = lua_type(L, index);
            var ptr = lua_topointer(L, index);
            if(delegateCache.TryGetValue(ptr, out var del))
            {
                return del;
            }

            //lua_pushvalue(L, index);
            //int luaref = luaL_ref(L, LUA_REGISTRYINDEX);

            var d = fnCreate(L, index);
            delegateCache[ptr] = new WeakReference<T>(d);
            return d;
        }
    }
}
