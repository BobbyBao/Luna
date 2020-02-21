using SharpLuna;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    Dictionary<string, GameObject> m_uiMap = new Dictionary<string, GameObject>();
    Dictionary<string, bool> m_uiLoading = new Dictionary<string, bool>();
    static UIManager instance;
    public static UIManager Instance => instance;

    private void Awake()
    {
        instance = this;

    }

    public GameObject LoadPanel(string filePath)
    {
        var go = ResourceManager.LoadObject(filePath);       
        m_uiMap[filePath] = go;
        go.transform.SetParent(transform, false);
        return go;
    }

    public void OpenPanel(string filePath, LuaRef fn, LuaRef inst)
    {
        OpenPanel(filePath, (go)=>
        {
            fn.Call(inst, go);
        });
    }
      
    public void OpenPanel(string filePath, System.Action<GameObject> callback)
    {
        if (m_uiLoading.ContainsKey(filePath))
        {
            return;
        }

        GameObject ui;
        if (m_uiMap.TryGetValue(filePath, out ui))
        {
            if (ui != null)
            {
                ui.SetActive(true);

                if (callback != null)
                {
                    callback(ui);
                }

                return;
            }
        }

        m_uiLoading.Add(filePath, false);

        ResourceManager.LoadObject(filePath, (go) =>
        {
            m_uiMap[filePath] = go;

            go.transform.SetParent(transform, false);
            
            if (m_uiLoading[filePath])
            {
                //已经关闭
                m_uiLoading.Remove(filePath);
                go.SetActive(false);
            }
            else
            {
                m_uiLoading.Remove(filePath);
                go.SetActive(true);

                if (callback != null)
                {
                    callback(go);
                }

            }

        });
    }

    public void ClosePanel(string filePath)
    {
        bool cancel = false;
        if (m_uiLoading.TryGetValue(filePath, out cancel))
        {
            m_uiLoading[filePath] = true;
        }
        else
        {
            GameObject ui;
            if (m_uiMap.TryGetValue(filePath, out ui))
            {
                ui.SetActive(false);
                return;
            }

        }

    }

    public void ClearPanels()
    {
        var it = m_uiMap.GetEnumerator();
        while (it.MoveNext())
        {
            it.Current.Value.SetActive(false);
        }

        m_uiMap.Clear();
        
        m_uiLoading.Clear();

    }

    private void OnDestroy()
    {
        ClearPanels();
    }

}
