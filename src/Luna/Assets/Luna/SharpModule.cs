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
        }
        
        public SharpModule(SharpClass parent, LuaRef parentMeta, string name) : base(parentMeta)
        {
            LuaRef @ref = parentMeta.RawGet(name);
            if (@ref)
            {
                meta = @ref;
                return;
            }

            this.parent = parent;
            meta = create_module(parentMeta.State, parentMeta, name);
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

    };


}
