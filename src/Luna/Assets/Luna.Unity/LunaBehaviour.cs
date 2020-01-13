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
        LuaRef luaClass;
        LuaRef luaInstance;
        void Awake()
        {
            if(!string.IsNullOrEmpty(luaPath))
            {
                var luna = LunaClient.Luna;
                var L = luna.State;

                luna.DoFile(luaPath);

                int index = luaPath.LastIndexOf(".");
                className = luaPath.Remove(index);
                index = className.LastIndexOf("/");
                if(index != -1)
                {
                    className = className.Substring(index + 1);
                }
                
                luaClass = LuaRef.Globals(L).RawGet(className);
                luaClass.AddRef();

                var metaTable = luaClass.GetMetaTable();             
                if(metaTable)
                {                  
                    var ctor = metaTable.RawGet("__call");
                    luaInstance = ctor.Call<LuaRef>(luaClass);
                    luaInstance.AddRef();
                    metaTable.Dispose();
                }
                else
                {
                    Debug.Log("GetMetaTable failed : " + className);
                }

                CallFunc("Awake");
                
            }
        }

        protected void CallFunc(string name)
        {
            if(luaInstance)
            {
                var fn = luaClass.Get(name);
                if (fn)
                {
                    fn.Call(luaInstance);
                    fn.Dispose();
                }
            }
        }

        void Start()
        {
            CallFunc("Start");
        }

        void Update()
        {
            //CallLuaFunc("Update");
        }

        protected virtual void OnEnable()
        {
            CallFunc("OnEnable");
        }

        protected virtual void OnDisable()
        {
            CallFunc("OnDisable");
        }

        void OnDestroy()
        {
            if(LunaClient.Luna != null)
            {

                CallFunc("OnDestroy");

                luaClass.Dispose();
                luaInstance.Dispose();
            }
        }

    }
}
