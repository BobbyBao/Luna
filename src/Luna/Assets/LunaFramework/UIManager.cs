using SharpLuna;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

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

    [LuaAsync]
    public static void ShowAlertBox(string message, string title, Action onFinished = null)
    {
        var alertPanel = GameObject.Find("Canvas").transform.Find("AlertBox");
        if (alertPanel == null)
        {
            alertPanel = ResourceManager.LoadObject("UI/AlertBox").transform;
            alertPanel.gameObject.name = "AlertBox";
            alertPanel.SetParent(instance.transform);
            alertPanel.localPosition = new Vector3(-6f, -6f, 0f);
        }

        alertPanel.Find("title").GetComponent<Text>().text = title;
        alertPanel.Find("message").GetComponent<Text>().text = message;

        var button = alertPanel.Find("alertBtn").GetComponent<Button>();
        UnityAction onclick = () =>
        {
            onFinished?.Invoke();
            button.onClick.RemoveAllListeners();
            alertPanel.gameObject.SetActive(false);
        };
        //防止消息框未关闭时多次被调用
        button.onClick.RemoveAllListeners();
        button.onClick.AddListener(onclick);
        alertPanel.gameObject.SetActive(true);
    }

    [LuaAsync]
    public static void ShowConfirmBox(string message, string title, Action<bool> onFinished = null)
    {
        var confirmPanel = GameObject.Find("Canvas").transform.Find("ConfirmBox");
        if (confirmPanel == null)
        {
            confirmPanel = ResourceManager.LoadObject("UI/ConfirmBox").transform;
            confirmPanel.gameObject.name = "ConfirmBox";
            confirmPanel.SetParent(instance.transform);
            confirmPanel.localPosition = new Vector3(-8f, -18f, 0f);
        }

        confirmPanel.Find("confirmTitle").GetComponent<Text>().text = title;
        confirmPanel.Find("conmessage").GetComponent<Text>().text = message;

        var confirmBtn = confirmPanel.Find("confirmBtn").GetComponent<Button>();
        var cancelBtn = confirmPanel.Find("cancelBtn").GetComponent<Button>();
        Action cleanup = () =>
        {
            confirmBtn.onClick.RemoveAllListeners();
            cancelBtn.onClick.RemoveAllListeners();
            confirmPanel.gameObject.SetActive(false);
        };

        UnityAction onconfirm = () =>
        {
            onFinished?.Invoke(true);            
            cleanup();
        };

        UnityAction oncancel = () =>
        {
            onFinished?.Invoke(false);
            cleanup();
        };

        //防止消息框未关闭时多次被调用
        confirmBtn.onClick.RemoveAllListeners();
        confirmBtn.onClick.AddListener(onconfirm);
        cancelBtn.onClick.RemoveAllListeners();
        cancelBtn.onClick.AddListener(oncancel);
        confirmPanel.gameObject.SetActive(true);
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
