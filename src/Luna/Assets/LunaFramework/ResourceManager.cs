﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class ResourceManager : MonoBehaviour
{
    static IAssetLoader assetLoader;
    static ResourceManager instance;
    private void Awake()
    {
        instance = this;      
        assetLoader = new ResourcesLoader();
    }

    private void OnDestroy()
    {
        assetLoader?.Dispose();
        instance = null;
    }

    public static GameObject LoadObject(string filePath)
    {
        var go = Load<GameObject>(filePath);
        return GameObject.Instantiate(go);
    }

    public static void LoadObject(string filePath, System.Action<GameObject> finishLoad)
    {
        LoadAsync<GameObject>(filePath, (go) =>
        {
            finishLoad(GameObject.Instantiate(go));
        });
    }

    public static void Free(GameObject obj)
    {
        GameObject.Destroy(obj);
    }

    public static T Load<T>(string filePath) where T : UnityEngine.Object
    {
        return assetLoader.Load<T>(filePath);
    }

    public static void LoadAsync<T>(string filePath, System.Action<T> finishLoad) where T : UnityEngine.Object
    {
        instance.StartCoroutine(assetLoader.Load<T>(filePath, finishLoad));
    }

    public static void LoadScene(string sceneName, Action<AsyncOperation> loadCallback)
    {
        LoadScene(sceneName, loadCallback, false);
    }

    public static void LoadScene(string sceneName, Action<AsyncOperation> loadCallback, bool addtive)
    {
        instance.StartCoroutine(assetLoader.LoadScene(sceneName, loadCallback, addtive ? UnityEngine.SceneManagement.LoadSceneMode.Additive : UnityEngine.SceneManagement.LoadSceneMode.Single));
    }

}
