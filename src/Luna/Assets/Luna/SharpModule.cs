using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace SharpLuna
{
    using lua_State = System.IntPtr;
    using static Lua;

    public class SharpModule : SharpClass
    {
        static Dictionary<string, SharpModule> registeredModule;
        protected Luna luna;

        public override Luna Luna
        {
            get
            {               
                if(luna != null)   
                    return luna;

                return base.Luna;
            }
        }

        //GlobalModule
        public SharpModule(Luna luna) : base(LuaRef.Globals(luna.State))
        {
            this.luna = luna;
            Name = "_G";
        }
        
        public SharpModule(SharpClass parent, LuaRef parentMeta, string name) : base(parentMeta)
        {
            LuaRef luaref = parentMeta.RawGet(name) as LuaRef;
            if (luaref)
            {
                meta = luaref;
                return;
            }

            this.parent = parent;
            meta = create_meta(parentMeta, name, 0, null);
            Name = name;
        }

        public SharpModule GetModule(string name)
        {
            if (registeredModule == null)
            {
                registeredModule = new Dictionary<string, SharpModule>();
            }

            if (registeredModule.TryGetValue(name, out var module))
            {
                return module;
            }

            module = new SharpModule(this, meta, name);
            registeredModule.Add(name, module);
            return module;
        }

        public SharpClass RegClass<T>()
        {
            return RegClass(typeof(T));
        }

        public SharpClass RegClass<T, SUPER>()
        {
            return RegClass(typeof(T), typeof(SUPER));
        }

        public SharpClass RegClass(Type classType, Type superType = null)
        {
            var cls = GetClass(classType, superType);
            cls.OnRegClass();
            return this;
        }
    };


}
