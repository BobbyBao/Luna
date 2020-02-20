using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

using LuaFunction = SharpLuna.LuaRef;

namespace SharpLuna.Unity
{
    public class LunaHelper
    {
        private static Dictionary<object, List<LuaRef>> actions = new Dictionary<object, List<LuaRef>>();


        private static Assembly assembly_Current;
        private static Assembly assembly_UnityEngine;
        private static Assembly assembly_UnityEngineUI;
        private static Assembly assembly_Code;

        public static Component AddComponent(GameObject go, string typeName, string nameSpace)
        {
            Type type = null;
            if (string.IsNullOrEmpty(nameSpace))
            {
                type = Type.GetType(typeName);
                if (type == null)
                {
                    type = Type.GetType("UnityEngine." + typeName);
                    if (type == null)
                        type = Type.GetType("UnityEngine.UI" + typeName);
                }
            }
            else
            {
                type = Type.GetType(nameSpace + "." + typeName);
            }

            if (type == null)
            {
                if (assembly_Current == null)
                    assembly_Current = Assembly.GetExecutingAssembly();
                type = TryGetType(assembly_Current, typeName, nameSpace);
                if (type == null)
                {
                    if (assembly_UnityEngine == null)
                        assembly_UnityEngine = Assembly.Load("UnityEngine");
                    type = TryGetType(assembly_UnityEngine, typeName, nameSpace);
                    if (type == null)
                    {
                        if (assembly_UnityEngineUI == null)
                            assembly_UnityEngineUI = Assembly.Load("UnityEngine.UI");
                        type = TryGetType(assembly_UnityEngineUI, typeName, nameSpace);
                    }
                }
                if (type == null)
                {
                    Debug.LogError("我尽力了~~~取不到这个类型：" + typeName);
                    return null;
                }
                else
                {
                    return go.AddComponent(type);
                }
            }
            else
            {
                return go.AddComponent(type);
            }
        }

        public static Component AddComponent(GameObject go, string type)
        {
            return AddComponent(go, type, null);
        }

        private static Type TryGetType(Assembly assembly, string typeName, string nameSpace)
        {
            if (string.IsNullOrEmpty(nameSpace))
            {
                Type type = null;
                type = assembly.GetType(typeName);
                if (type == null)
                    type = assembly.GetType("UnityEngine." + typeName);
                if (type == null)
                    type = assembly.GetType("UnityEngine.UI." + typeName);
                return type;
            }
            else
            {
                string fullName = nameSpace + "." + typeName;
                return assembly.GetType(fullName);
            }
        }

        public static Component GetOrAddComponent(GameObject go, string type, string nameSpace)
        {
            Component c = go.GetComponent(type);
            if (c == null)
            {
                c = AddComponent(go, type, nameSpace);
            }

            return c;
        }

        public static Component GetOrAddComponent(GameObject go, string type)
        {
            return GetOrAddComponent(go, type, null);
        }

        public static GameObject FindChild(GameObject go, string name)
        {
            return go.FindChild(name);
        }

        public static void AddChild(GameObject parent, GameObject child)
        {
            child.transform.SetParent(parent.transform, false);
        }

        public static void SetLayer(GameObject go, string layerName)
        {
            SetLayer(go, LayerMask.NameToLayer(layerName));
        }

        public static void SetLayer(GameObject go, int layer)
        {
            go.layer = layer;

            UnityHelper.RecurseAll<int>(go.transform, (t, l) =>
            {
                t.gameObject.layer = l;
            }, layer);
        }

        private static void CacheLuaFunction(object go, LuaFunction luaFunc)
        {
            List<LuaFunction> functions = null;
            if (actions.TryGetValue(go, out functions))
            {
                functions.Add(luaFunc);
            }
            else
            {
                functions = new List<LuaFunction>();
                functions.Add(luaFunc);
                actions.Add(go, functions);
            }
        }

        public static void AddClick(LuaRef self, GameObject go, LuaFunction luaFunc)
        {
            CacheLuaFunction(self, luaFunc);

            go.GetComponent<Button>().onClick.AddListener(() => luaFunc.Call(self, go));
        }

        public static void AddEventTrigger(LuaRef self, GameObject go, EventTriggerType eventTriggerType, LuaFunction luaFunc)
        {
            if (go == null || luaFunc == null)
                return;

            CacheLuaFunction(self, luaFunc);

            UnityAction<BaseEventData> click = (data) =>
            {
                luaFunc.Call(self, go, data);
            };

            EventTrigger.Entry eventTrigger = new EventTrigger.Entry();
            eventTrigger.eventID = eventTriggerType;
            eventTrigger.callback.AddListener(click);

            EventTrigger trigger = go.GetComponent<EventTrigger>();
            if (trigger == null)
            {
                trigger = go.AddComponent<EventTrigger>();
            }

            for (int i = 0; i < trigger.triggers.Count; ++i)
            {
                if (trigger.triggers[i].eventID == eventTriggerType)
                {
                    return;
                }
            }

            trigger.triggers.Add(eventTrigger);
        }

        public static void RemoveEventTrigger(LuaRef self, GameObject go, EventTriggerType eventTriggerType, LuaFunction luaFunc)
        {
            if (go == null)
                return;

            EventTrigger trigger = go.GetComponent<EventTrigger>();
            if (trigger == null)
            {
                return;
            }

            EventTrigger.Entry eventTrigger = null;
            for (int i = 0; i < trigger.triggers.Count; ++i)
            {
                if (trigger.triggers[i].eventID == eventTriggerType)
                {
                    eventTrigger = trigger.triggers[i];
                    break;
                }
            }

            if (eventTrigger != null)
            {
                eventTrigger.callback.RemoveAllListeners();
                trigger.triggers.Remove(eventTrigger);
                RemoveAction(self, luaFunc);
            }
        }

        public static void RemoveAction(LuaRef self, LuaFunction luaFunc)
        {
            List<LuaFunction> luafuncs = null;
            if (actions.TryGetValue(self, out luafuncs))
            {
                if(luaFunc == null)
                {
                    for (int i = 0; i < luafuncs.Count; i++)
                    {
                        luafuncs[i].Dispose();                            
                    }

                    luafuncs.Clear();
                }
                else
                {
                    for (int i = 0; i < luafuncs.Count; i++)
                    {
                        if (luaFunc == luafuncs[i])
                        {
                            luaFunc.Dispose();
                            luafuncs.RemoveAt(i);
                            break;
                        }
                    }
                }

            }
        }

        public static void ClearActions()
        {
            foreach (var it in actions)
            {
                for (int i = 0; i < it.Value.Count; i++)
                {
                    if (it.Value[i] != null)
                    {
                        it.Value[i].Dispose();
                    }
                }
            }

            actions.Clear();
        }

    }
}
