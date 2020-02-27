using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;

namespace SharpLuna
{
    using lua_State = System.IntPtr;
    using static Lua;

    public class SharpModule : SharpClass
    {
        protected Luna luna;

        static Dictionary<string, SharpModule> registeredModule;
        static Dictionary<Type, SharpClass> registeredClass = new Dictionary<Type, SharpClass>();
        static Dictionary<Type, string> classAlias = new Dictionary<Type, string>();

        //GlobalModule
        public SharpModule(Luna luna) : base(null, LuaRef.Globals(luna.State))
        {
            this.luna = luna;
            Name = "_G";
        }
        
        public SharpModule(SharpModule parent, LuaRef parentMeta, string name) : base(parent)
        {
            LuaRef luaref = parentMeta.RawGet(name) as LuaRef;
            if (luaref)
            {
                meta = luaref;
                return;
            }

            this.parent = parent;
            meta = create_module(parentMeta.State, parentMeta, name);
            Name = name;
        }

        public override Luna Luna
        {
            get
            {
                if (luna != null)
                    return luna;

                return base.Luna;
            }
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


        public SharpClass GetClass(Type classType, Type superClass = null)
        {
            if (registeredClass.TryGetValue(classType, out var bindClass))
            {
                return bindClass;
            }

            if (classType == superClass)
            {
                return CreateClass(classType, null, this);
            }

            var parentType = classType.BaseType;
            while (superClass == null)
            {
                if (parentType == null)
                {
                    break;
                }

                if (SharpModule.IsRegistered(parentType))
                {
                    superClass = parentType;
                    break;
                }

                if (parentType == typeof(object))
                {
                    break;
                }

                parentType = parentType.BaseType;
            }

            return CreateClass(classType, superClass, this);
        }

        [AOT.MonoPInvokeCallback(typeof(LuaNativeFunction))]
        static int Destructor(lua_State L)
        {
            SharpObject.Free(L, 1);
            return 0;
        }

        [AOT.MonoPInvokeCallback(typeof(LuaNativeFunction))]
        static int DestructorStruct(lua_State L)
        {
            SharpObject.FreeStruct(L, 1);
            return 0;
        }

        private SharpClass CreateClass(Type classType, Type superType, SharpClass module)
        {
            string name = GetTableName(classType);

            LuaNativeFunction dctor = null;
            if (classType.IsValueType)
            {
                if (!classType.IsUnManaged()) //未注册Wrap，用反射版api， UnmanagedType同Object
                {
                    dctor = Destructor;
                }

            }
            else
            {
                dctor = Destructor;
            }

            LuaRef meta = CreateClass(module.Meta, name, classType, superType, dctor);
            var bindClass = new SharpClass(module, meta);
            bindClass.Name = name;
            bindClass.SetClassType(classType);
            registeredClass.Add(classType, bindClass);
            return bindClass;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsRegistered<T>()
        {
            return IsRegistered(typeof(T));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsRegistered(Type t)
        {
            return registeredClass.ContainsKey(t);
        }

        public static void SetAlias(Type t, string alias)
        {
            classAlias.Add(t, alias);
        }

        public static string GetTableName(Type t)
        {
            if (classAlias.TryGetValue(t, out string alias))
            {
                return alias;
            }

            if (t.IsArray)
            {
                return t.Name.Replace("[]", "Array");
            }

            return t.Name;
        }

    };


}
