using System;
using SharpLuna;
using static SharpLuna.Lua;

[WrapClass(typeof(UnityEngine.Color))]
public class ColorWrap
{
	[AOT.MonoPInvokeCallback(typeof(LuaNativeFunction))]
	static int Constructor(LuaState L)
	{
		int n = lua_gettop(L);
		UnityEngine.Color obj = default;
		if(n == 4)
			obj = new UnityEngine.Color(
				Lua.Get<float>(L, 1),
				Lua.Get<float>(L, 2),
				Lua.Get<float>(L, 3),
				Lua.Get<float>(L, 4)
			);
		else if(n == 3)
			obj = new UnityEngine.Color(
				Lua.Get<float>(L, 1),
				Lua.Get<float>(L, 2),
				Lua.Get<float>(L, 3)
			);
		Lua.Push(L, obj);
		return 1;
	}

	[AOT.MonoPInvokeCallback(typeof(LuaNativeFunction))]
	static int Get_r(LuaState L)
	{
		ref var obj = ref SharpObject.GetValue<UnityEngine.Color>(L, 1);
		Lua.Push(L, obj.r);
		return 1;
	}

	[AOT.MonoPInvokeCallback(typeof(LuaNativeFunction))]
	static int Set_r(LuaState L)
	{
		ref var obj = ref SharpObject.GetValue<UnityEngine.Color>(L, 1);
		var p1 = Lua.Get<float>(L, 2);
		obj.r = p1;
		return 0;
	}

	[AOT.MonoPInvokeCallback(typeof(LuaNativeFunction))]
	static int Get_g(LuaState L)
	{
		ref var obj = ref SharpObject.GetValue<UnityEngine.Color>(L, 1);
		Lua.Push(L, obj.g);
		return 1;
	}

	[AOT.MonoPInvokeCallback(typeof(LuaNativeFunction))]
	static int Set_g(LuaState L)
	{
		ref var obj = ref SharpObject.GetValue<UnityEngine.Color>(L, 1);
		var p1 = Lua.Get<float>(L, 2);
		obj.g = p1;
		return 0;
	}

	[AOT.MonoPInvokeCallback(typeof(LuaNativeFunction))]
	static int Get_b(LuaState L)
	{
		ref var obj = ref SharpObject.GetValue<UnityEngine.Color>(L, 1);
		Lua.Push(L, obj.b);
		return 1;
	}

	[AOT.MonoPInvokeCallback(typeof(LuaNativeFunction))]
	static int Set_b(LuaState L)
	{
		ref var obj = ref SharpObject.GetValue<UnityEngine.Color>(L, 1);
		var p1 = Lua.Get<float>(L, 2);
		obj.b = p1;
		return 0;
	}

	[AOT.MonoPInvokeCallback(typeof(LuaNativeFunction))]
	static int Get_a(LuaState L)
	{
		ref var obj = ref SharpObject.GetValue<UnityEngine.Color>(L, 1);
		Lua.Push(L, obj.a);
		return 1;
	}

	[AOT.MonoPInvokeCallback(typeof(LuaNativeFunction))]
	static int Set_a(LuaState L)
	{
		ref var obj = ref SharpObject.GetValue<UnityEngine.Color>(L, 1);
		var p1 = Lua.Get<float>(L, 2);
		obj.a = p1;
		return 0;
	}

	public static void Register(ClassWraper classWraper)
	{
		classWraper.RegFunction("ctor", Constructor);
		classWraper.RegField("r", Get_r, Set_r);
		classWraper.RegField("g", Get_g, Set_g);
		classWraper.RegField("b", Get_b, Set_b);
		classWraper.RegField("a", Get_a, Set_a);
	}
}