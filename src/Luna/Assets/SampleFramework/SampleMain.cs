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
        luaClient = gameObject.AddComponent<LunaClient>();
    }

    // Start is called before the first frame update
    IEnumerator Start()
    {
        var luna = LunaClient.Luna;

        //Load("UI/Panel");
                
        if (!string.IsNullOrWhiteSpace(startFile))
        {
            luaClient.Run(startFile); 
        }

        yield return null;
    }

    public void Load(string file)
    {
        var obj = Object.Instantiate(Resources.Load<GameObject>(file));
        obj.transform.SetParent(this.transform, false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
