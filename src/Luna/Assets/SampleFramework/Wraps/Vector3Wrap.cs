using System;
using SharpLuna;
using UnityEngine;
using static SharpLuna.Lua;

[WrapClass(typeof(Vector3))]
public class Vector3Wrap
{
	[AOT.MonoPInvokeCallback(typeof(LuaNativeFunction))]
	static int get_x(LuaState L)
    {
        ref var obj = ref SharpObject.GetValue<Vector3>(L, 1);
        Lua.Push(L, obj.x);
        return 1;
	}

	[AOT.MonoPInvokeCallback(typeof(LuaNativeFunction))]
	static int set_x(LuaState L)
    {
		ref var obj = ref SharpObject.GetValue<Vector3>(L, 1);
		float p1 = Lua.Get<float>(L, 2);
        obj.x = p1;
        return 0;
	}

}