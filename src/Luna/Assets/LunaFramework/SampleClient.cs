using SharpLuna;
using SharpLuna.Unity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SampleClient : LunaClient
{
    [LunaExport]
    static ModuleInfo customTypes = new ModuleInfo
    {
        typeof(ResourceManager),
        typeof(LunaBehaviour),
        typeof(LunaHelper),
        typeof(UIManager),
        //typeof(UIPanel),
    };

    protected override void OnInit()
    {          
        gameObject.AddComponent<ResourceManager>();
        gameObject.AddComponent<UIManager>();

        this.AddModule(customTypes);

    }

    protected override IEnumerator OnStart()
    {

        var luna = LunaClient.Luna;

        yield return null;
    }

}
