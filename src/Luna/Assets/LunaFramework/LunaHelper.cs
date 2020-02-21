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

        public static Component AddComponent(GameObject go, string type, string nameSpace)
        {
            return UnityHelper.AddComponent(go, type, null);
        }

        public static Component AddComponent(GameObject go, string type)
        {
            return UnityHelper.AddComponent(go, type, null);
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

        public static Component FindComponent(GameObject go, string name, string type)
        {
            return go.FindComponent(name, type);
        }

        public static void AddChild(GameObject parent, GameObject child)
        {
            child.transform.SetParent(parent.transform, false);
        }

        private static void CacheLuaFunction(object obj, LuaFunction luaFunc)
        {
            List<LuaFunction> functions = null;
            if (actions.TryGetValue(obj, out functions))
            {
                functions.Add(luaFunc);
            }
            else
            {
                functions = new List<LuaFunction>();
                functions.Add(luaFunc);
                actions.Add(obj, functions);
            }
        }

        public static void AddEventTrigger(GameObject go, EventTriggerType eventTriggerType, LuaFunction luaFunc, LuaRef self)
        {
            if (go == null || luaFunc == null)
                return;

            CacheLuaFunction(go, luaFunc);

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

        public static void RemoveEventTrigger(GameObject go, EventTriggerType eventTriggerType, LuaFunction luaFunc)
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
                RemoveAction(go, luaFunc);
            }
        }

        public static void RemoveAction(GameObject go, LuaFunction luaFunc)
        {
            List<LuaFunction> luafuncs = null;
            if (actions.TryGetValue(go, out luafuncs))
            {
                if (luaFunc == null)
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

        public static void AddClick(GameObject go, LuaFunction luaFunc, LuaRef self)
        {
            CacheLuaFunction(go, luaFunc);
            go.GetComponent<Button>().onClick.AddListener(() => luaFunc.Call(self, go));
        }

        public static void ButtonListener(Button btn, LuaFunction luaFunc, LuaRef self)
        {
            CacheLuaFunction(btn, luaFunc);
            btn.onClick.AddListener(() => luaFunc.Call(self));
        }

        public static void DropdownListener(Dropdown dropdown, LuaFunction func, LuaRef self)
        {
            CacheLuaFunction(dropdown, func);
            dropdown.onValueChanged.RemoveAllListeners();
            dropdown.onValueChanged.AddListener((val) =>
            {
                func.Call(self, val);
            });
        }

        public static void SliderListener(Slider slider, LuaFunction func, LuaRef self)
        {
            CacheLuaFunction(slider, func);
            slider.onValueChanged.RemoveAllListeners();
            slider.onValueChanged.AddListener((val) =>
            {
                func.Call(self, val);
            });
        }

        public static void ToggleListener(Toggle toggle, LuaFunction func, LuaRef self)
        {
            CacheLuaFunction(toggle, func);
            toggle.onValueChanged.RemoveAllListeners();
            toggle.onValueChanged.AddListener((val) =>
            {
                func.Call(self, val);
            });
        }

        public static void InputFieldListener(InputField inputfield, LuaFunction func, LuaRef self)
        {
            CacheLuaFunction(inputfield, func);
            inputfield.onValueChanged.RemoveAllListeners();
            inputfield.onValueChanged.AddListener((val) =>
            {
                func.Call(self, val);
            });
        }

        public static void ScrollbarListener(Scrollbar scrollbar, LuaFunction func, LuaRef self)
        {
            CacheLuaFunction(scrollbar, func);
            scrollbar.onValueChanged.RemoveAllListeners();
            scrollbar.onValueChanged.AddListener((val) =>
            {
                func.Call(self, val);
            });
        }

        public static void ScrollRectListener(ScrollRect sr, LuaFunction func, LuaRef self)
        {
            CacheLuaFunction(sr, func);
            sr.onValueChanged.AddListener((vc2) =>
            {
                func.Call(self, vc2);
            });
        }

        public static void InputFieldEndEdit(InputField inputfield, LuaFunction func, LuaRef self)
        {
            CacheLuaFunction(inputfield, func);
            inputfield.onEndEdit.RemoveAllListeners();
            inputfield.onEndEdit.AddListener((val) =>
            {
                func.Call(self, val);
            });
        }


    }
}
