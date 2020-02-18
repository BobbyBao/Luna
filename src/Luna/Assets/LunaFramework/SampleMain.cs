using SharpLuna.Unity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SampleMain : MonoBehaviour
{
    static SampleMain instance = null;
    public static SampleMain Instance => instance;

    public string startFile;
    LunaClient luaClient;
    // Start is called before the first frame update
    void Awake()
    {
        instance = this;
        DontDestroyOnLoad(gameObject);      
        
        gameObject.AddComponent<ResourceMananger>();
        luaClient = gameObject.AddComponent<LunaClient>();

    }

    // Start is called before the first frame update
    IEnumerator Start()
    {
        var luna = LunaClient.Luna;

        if (!string.IsNullOrWhiteSpace(startFile))
        {
            luaClient.Run(startFile); 
        }

        yield return null;
    }

    // Update is called once per frame
    void Update()
    {        
    }
}
