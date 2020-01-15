using System;
using SharpLuna;
using static SharpLuna.Lua;

[WrapClass(typeof(UnityEngine.Plane))]
public class PlaneWrap
{
	[AOT.MonoPInvokeCallback(typeof(LuaNativeFunction))]
	static int Constructor(LuaState L)
	{
		int n = lua_gettop(L);
		UnityEngine.Plane obj = default;
		if(n == 2)
			obj = new UnityEngine.Plane(
				Lua.Get<UnityEngine.Vector3>(L, 1),
				Lua.Get<UnityEngine.Vector3>(L, 2)
			);
		else if(n == 2)
			obj = new UnityEngine.Plane(
				Lua.Get<UnityEngine.Vector3>(L, 1),
				Lua.Get<float>(L, 2)
			);
		else if(n == 3)
			obj = new UnityEngine.Plane(
				Lua.Get<UnityEngine.Vector3>(L, 1),
				Lua.Get<UnityEngine.Vector3>(L, 2),
				Lua.Get<UnityEngine.Vector3>(L, 3)
			);
		Lua.Push(L, obj);
		return 1;
	}

	public static void Register(ClassWraper classWraper)
	{
		classWraper.RegFunction("ctor", Constructor);
	}
}