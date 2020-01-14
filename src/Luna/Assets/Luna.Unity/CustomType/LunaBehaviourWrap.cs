using System;
using SharpLuna;
using static SharpLuna.Lua;

[WrapClass(typeof(SharpLuna.Unity.LunaBehaviour))]
public class LunaBehaviourWrap
{
	[AOT.MonoPInvokeCallback(typeof(LuaNativeFunction))]
	static int Constructor(LuaState L)
	{
		int n = lua_gettop(L);
		SharpLuna.Unity.LunaBehaviour obj = default;
		if(n == 0)
			obj = new SharpLuna.Unity.LunaBehaviour();
		Lua.Push(L, obj);
		return 1;
	}

	[AOT.MonoPInvokeCallback(typeof(LuaNativeFunction))]
	static int Get_luaPath(LuaState L)
	{
		var obj = SharpObject.Get<SharpLuna.Unity.LunaBehaviour>(L, 1);
		Lua.Push(L, obj.luaPath);
		return 1;
	}

	[AOT.MonoPInvokeCallback(typeof(LuaNativeFunction))]
	static int Set_luaPath(LuaState L)
	{
		var obj = SharpObject.Get<SharpLuna.Unity.LunaBehaviour>(L, 1);
		var p1 = Lua.Get<string>(L, 2);
		obj.luaPath = p1;
		return 0;
	}

	public static void Register(ClassWraper classWraper)
	{
		classWraper.RegFunction("ctor", Constructor);
		classWraper.RegField("luaPath", Get_luaPath, Set_luaPath);
	}
}