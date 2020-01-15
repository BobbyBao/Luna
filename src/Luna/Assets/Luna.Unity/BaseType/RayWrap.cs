using System;
using SharpLuna;
using static SharpLuna.Lua;

[WrapClass(typeof(UnityEngine.Ray))]
public class RayWrap
{
	[AOT.MonoPInvokeCallback(typeof(LuaNativeFunction))]
	static int Constructor(LuaState L)
	{
		int n = lua_gettop(L);
		UnityEngine.Ray obj = default;
		if(n == 2)
			obj = new UnityEngine.Ray(
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