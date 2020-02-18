using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

public interface IAssetLoader : IDisposable
{
    T Load<T>(string filePath) where T : UnityEngine.Object;
    IEnumerator Load<T>(string filePath, System.Action<T> finishLoad) where T : UnityEngine.Object;
    IEnumerator LoadScene(string sceneName, Action<AsyncOperation> loadCallback, UnityEngine.SceneManagement.LoadSceneMode mode);
}

public class ResourcesLoader : IAssetLoader
{
    public T Load<T>(string filePath) where T : UnityEngine.Object
    {
        return Resources.Load<T>(filePath);
    }

    public IEnumerator Load<T>(string filePath, Action<T> finishLoad) where T : UnityEngine.Object
    {
        ResourceRequest request = Resources.LoadAsync<T>(filePath);
        if (request != null)
        {
            while (!request.isDone)
            {
                yield return null;
            }

            T res = request.asset as T;
            if (res == null)
            {
                Debug.LogError("filePath : " + filePath);
            }

            finishLoad(res);
        }

    }

    public IEnumerator LoadScene(string sceneName, Action<AsyncOperation> loadCallback, LoadSceneMode mode)
    {
        AsyncOperation op = UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(sceneName, mode);
        while (!op.isDone)
        {
            loadCallback?.Invoke(op);
            yield return null;
        }

        loadCallback?.Invoke(op);
    }

    public void Dispose()
    {
    }
}

//todo:AB加载管理
public class AssetBundleLoader : IAssetLoader
{
    public T Load<T>(string filePath) where T : UnityEngine.Object
    {
        throw new NotImplementedException();
    }

    public IEnumerator Load<T>(string filePath, Action<T> finishLoad) where T : UnityEngine.Object
    {
        throw new NotImplementedException();
    }

    public IEnumerator LoadScene(string sceneName, Action<AsyncOperation> loadCallback, LoadSceneMode mode)
    {
        throw new NotImplementedException();
    }

    public void Dispose()
    {
        throw new NotImplementedException();
    }

}