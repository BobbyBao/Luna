using SharpLuna;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace SharpLuna.Unity
{
    public class LunaBehaviour : MonoBehaviour
    {
        public string luaPath;

        string className;
        LuaRef scriptClass;
        LuaRef scriptInstance;

        LuaRef onEnableFn;
        LuaRef onDisableFn;
        LuaRef updateFn;

        public LuaRef ScriptInstance => scriptInstance;

        protected virtual void Awake()
        {
            if(!string.IsNullOrEmpty(luaPath))
            {
                var luna = LunaClient.Luna;
                var L = luna.State;
                int index = luaPath.LastIndexOf(".");
                if (index != -1)
                {
                    className = luaPath.Remove(index);
                }
                else
                {
                    className = luaPath;
                }

                index = className.LastIndexOf("/");
                if(index != -1)
                {
                    className = className.Substring(index + 1);
                }
                
                scriptClass = luna.GetGlobal(className);
                if (!scriptClass)
                {
                    luna.DoFile(luaPath);
                    scriptClass = luna.GetGlobal(className);
                    if (!scriptClass)
                    {
                        return;
                    }
                }

                var metaTable = scriptClass.GetMetaTable();             
                if(metaTable)
                {                  
                    var ctor = metaTable.RawGet("__call");
                    scriptInstance = ctor.Call<LuaRef>(metaTable);
                    metaTable.Dispose();
                }
                else
                {
                    Debug.Log("GetMetaTable failed : " + className);
                }

                scriptInstance["gameObject"] = gameObject;
                scriptInstance["transform"] = gameObject.transform;
                scriptInstance["behaviour"] = this;

                onEnableFn = scriptClass.Get("onEnable");
                onDisableFn = scriptClass.Get("onDisable");
                updateFn = scriptClass.Get("update");

                CallFunc("awake");
            }
        }

        protected void CallFunc(string name)
        {
            if(scriptInstance)
            {
                var fn = scriptClass.Get(name);
                if (fn)
                {
                    fn.Call(scriptInstance);
                    fn.Dispose();
                }
            }
        }

        void Start()
        {
            CallFunc("start");
        }

        void Update()
        {
            if(updateFn)
                updateFn.Call(scriptInstance);
        }

        protected virtual void OnEnable()
        {
            if (onEnableFn)
                onEnableFn.Call(scriptInstance);
        }

        protected virtual void OnDisable()
        {
            if (onDisableFn)
                onDisableFn.Call(scriptInstance);
        }

        protected virtual void OnDestroy()
        {
            if(LunaClient.Luna != null)
            {
                CallFunc("onDestroy");
                
                updateFn?.Dispose();
                onEnableFn?.Dispose();
                onDisableFn?.Dispose();

                scriptClass.Dispose();
                scriptInstance.Dispose();
            }
        }

    }
}
