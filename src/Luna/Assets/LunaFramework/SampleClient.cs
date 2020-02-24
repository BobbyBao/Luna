using SharpLuna;
using SharpLuna.Unity;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using static SharpLuna.Lua;

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

    static System.Action<GameObject> CreateActionGameObject(IntPtr L, int index)
    {
        lua_pushvalue(L, index);
        int luaref = luaL_ref(L, LUA_REGISTRYINDEX);
        return (data) =>
        {
            lua_pushcfunction(L, LuaException.traceback);
            lua_rawgeti(L, LUA_REGISTRYINDEX, luaref);
            Push(L, data);
            if (lua_pcall(L, 1, 0, -1 + 2) != (int)LuaStatus.OK)
            {
                lua_remove(L, -2);
                throw new LuaException(L);
            }
            lua_pop(L, 1);
        };
    }
}
