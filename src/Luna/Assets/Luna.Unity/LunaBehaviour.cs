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

        public LuaRef ScriptInstance => scriptInstance;

        void Awake()
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
            //CallFunc("update");
        }

        protected virtual void OnEnable()
        {
            CallFunc("onEnable");
        }

        protected virtual void OnDisable()
        {
            CallFunc("onDisable");
        }

        void OnDestroy()
        {
            if(LunaClient.Luna != null)
            {

                CallFunc("onDestroy");

                scriptClass.Dispose();
                scriptInstance.Dispose();
            }
        }

    }
}
