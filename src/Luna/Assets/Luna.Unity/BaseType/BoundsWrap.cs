using System;
using SharpLuna;
using static SharpLuna.Lua;

[WrapClass(typeof(UnityEngine.Bounds))]
public class BoundsWrap
{
	[AOT.MonoPInvokeCallback(typeof(LuaNativeFunction))]
	static int Constructor(LuaState L)
	{
		int n = lua_gettop(L);
		UnityEngine.Bounds obj = default;
		if(n == 2)
			obj = new UnityEngine.Bounds(
				Lua.Get<UnityEngine.Vector3>(L, 1),
				Lua.Get<UnityEngine.Vector3>(L, 2)
			);
		Lua.Push(L, obj);
		return 1;
	}

	public static void Register(ClassWraper classWraper)
	{
		classWraper.RegFunction("ctor", Constructor);
	}
}