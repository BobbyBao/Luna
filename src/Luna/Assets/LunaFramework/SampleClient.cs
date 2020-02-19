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
        typeof(LunaBehaviour),
        typeof(ResourceMananger),
    };

    // Start is called before the first frame update
    protected override void OnInit()
    {          
        gameObject.AddComponent<ResourceMananger>();
        
        this.AddModule(customTypes);

    }

    // Start is called before the first frame update
    protected override IEnumerator OnStart()
    {

        var luna = LunaClient.Luna;

        yield return null;
    }

}
