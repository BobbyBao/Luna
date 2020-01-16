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
        
        public SharpModule(SharpClass parent, LuaRef meta, string name) : base(meta)
        {
            LuaRef @ref = meta.RawGet(name);
            if (@ref)
            {
                m_meta = @ref;
                return;
            }

            this.parent = parent;

            lua_State L = meta.State;
            string type_name = "module<" + GetFullName(meta, name) + ">";
            LuaRef module = LuaRef.CreateTable(L);
            module.SetMetaTable(module);

#if LUNA_SCRIPT
            module.RawSet("__index", (LuaNativeFunction)module_index);
            module.RawSet("__newindex", (LuaNativeFunction)module_newindex);

#else
            module.RawSet("__index", (LuaNativeFunction)luna_module_index);
            module.RawSet("__newindex", (LuaNativeFunction)luna_module_newindex);
#endif
            module.RawSet(___getters, LuaRef.CreateTable(L));
            module.RawSet(___setters, LuaRef.CreateTable(L));
            module.RawSet(___type, type_name);
            module.RawSet("___parent", meta);
            meta.RawSet(name, module);
            m_meta = module;
        }

        public SharpModule CreateModule(string name)
        {
            if (registeredModule == null)
            {
                registeredModule = new Dictionary<string, SharpModule>();
            }

            if (registeredModule.TryGetValue(name, out var module))
            {
                return module;
            }

            module = new SharpModule(this, m_meta, name);
            registeredModule.Add(name, module);
            return module;
        }

    };


}
