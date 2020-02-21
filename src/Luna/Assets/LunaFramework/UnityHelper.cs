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

    public static GameObject FindChild(this GameObject go, string name)
    {
        Transform t = GetChild(go, name);
        return t?.gameObject;
    }

    public static T FindComponent<T>(this GameObject go, string name) where T : Component
    {
        Transform t = GetChild(go, name);        
        return t?.GetComponent<T>();        
    }

    public static Component FindComponent(this GameObject go, string name, string type)
    {
        Transform t = GetChild(go, name);
        return t?.GetComponent(type);
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

