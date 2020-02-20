using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class UnityHelper
{
    public static GameObject FindChild(this GameObject go, string name)
    {
        Transform t = GetChild(go, name);
        if (t == null)
        {
            //Debugger.LogError("Not Found on " + go.name +"/" + name);
            return null;
        }
        else
        {
            return t.gameObject;
        }
        //return t == null ? null : t.gameObject;
    }

    public static T FindChildComponent<T>(this GameObject go, string name) where T : Component
    {
        Transform t = GetChild(go, name);
        if (t == null)
        {
            //Debugger.LogError("Not Found on " + go.name +"/" + name);
            return null;
        }
        else
        {
            return t.GetComponent<T>();
        }
        //return t == null ? null : t.gameObject;
    }

    public static Transform GetChild(this GameObject go, string name)
    {
        Transform child = go.transform.Find(name);
        if (child != null)
        {
            return child;
        }

        return GetChildRecurse(go.transform, name);
    }

    public static Transform GetChild(this Component comp, string name)
    {
        Transform tr = comp.transform.Find(name);
        if (tr != null)
        {
            return tr;
        }

        foreach (Transform child in comp.transform)
        {
            Transform t = GetChildRecurse(child, name);
            if (t != null)
            {
                return t;
            }
        }

        return null;
    }

    public static Transform GetChildRecurse(Transform tr, string name)
    {
        if (tr.name == name)
        {
            return tr;
        }

        for (int i = 0; i < tr.childCount; ++i)
        {
            Transform child = tr.GetChild(i);
            Transform t = GetChildRecurse(child, name);
            if (t != null)
            {
                return t;
            }
        }

        return null;
    }

    public static void SetLayer(this GameObject go, int layer)
    {
        SetLayer(go.transform, layer);
    }

    public static void SetLayer(this Transform t, int layer)
    {
        t.gameObject.layer = layer;
        for (int i = 0; i < t.childCount; ++i)
        {
            Transform child = t.GetChild(i);
            child.gameObject.layer = layer;
            SetLayer(child, layer);
        }
    }

    public static void SetLayer(this Transform t, int layer, int exceptLayerMask)
    {
        int oLayerMask = 1 << t.gameObject.layer;
        if ((oLayerMask & exceptLayerMask) == 0)
            t.gameObject.layer = layer;
        for (int i = 0; i < t.childCount; ++i)
        {
            Transform child = t.GetChild(i);
            SetLayer(child, layer, exceptLayerMask);
        }
    }

    public static void RecurseAll<T>(Transform trans, Action<Transform, T> func, T param)
    {
        func(trans, param);

        for (int i = 0; i < trans.childCount; i++)
        {
            Transform child = trans.GetChild(i);
            RecurseAll(child, func, param);
        }
    }

    public static Transform RecurseFirst<T>(Transform trans, Func<Transform, T, bool> func, T param)
    {
        if (func(trans, param))
        {
            return trans;
        }

        for (int i = 0; i < trans.childCount; i++)
        {
            Transform child = trans.GetChild(i);
            Transform t = RecurseFirst(child, func, param);
            if (t != null)
            {
                return t;
            }
        }

        return null;
    }
    
}

