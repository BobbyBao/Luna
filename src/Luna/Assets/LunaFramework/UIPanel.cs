using SharpLuna;
using SharpLuna.Unity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

using LuaFunction = SharpLuna.LuaRef;

public class UIPanel : LunaBehaviour
{
    private Dictionary<GameObject, List<LuaRef>> actions = new Dictionary<GameObject, List<LuaRef>>();

    protected override void Awake()
    {
        base.Awake();

    }

    protected override void OnDestroy()
    {
        base.OnDestroy();

        ClearActions();
    }

    /// <summary>
    /// 暂存lua方法
    /// </summary>
    /// <param name="go"></param>
    /// <param name="luaFunc"></param>
    private void CacheLuaFunction(GameObject go, LuaFunction luaFunc)
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

    public void AddClick(GameObject go, LuaFunction luaFunc)
    {
        if (go == null || luaFunc == null) return;
        CacheLuaFunction(go, luaFunc);

        go.GetComponent<Button>().onClick.AddListener(() => luaFunc.Call(ScriptInstance, go));
    }

    public void AddEventTrigger(GameObject go, EventTriggerType eventTriggerType, LuaFunction luaFunc)
    {
        if (go == null || luaFunc == null)
            return;

        CacheLuaFunction(go, luaFunc);

        UnityAction<BaseEventData> click = (data) =>
        {
            luaFunc.Call(ScriptInstance, go, data);
        };

        EventTrigger.Entry eventTrigger = new EventTrigger.Entry();
        eventTrigger.eventID = eventTriggerType;
        eventTrigger.callback.AddListener(click);

        EventTrigger trigger = gameObject.GetComponent<EventTrigger>();
        if (trigger == null)
        {
            trigger = gameObject.AddComponent<EventTrigger>();
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

    public void RemoveEventTrigger(GameObject go, EventTriggerType eventTriggerType, LuaFunction luaFunc)
    {
        if (go == null)
            return;

        EventTrigger trigger = gameObject.GetComponent<EventTrigger>();
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

    public void RemoveAction(GameObject go, LuaFunction luaFunc)
    {
        if (go == null) 
            return;

        List<LuaFunction> luafuncs = null;
        if (actions.TryGetValue(go, out luafuncs))
        {
            for (int i = 0; i < luafuncs.Count; i++)
            {
                if(luaFunc == luafuncs[i])
                {
                    luaFunc.Dispose();
                    luafuncs.RemoveAt(i);
                    break;
                }
            }
        }
    }

    public void ClearActions()
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

