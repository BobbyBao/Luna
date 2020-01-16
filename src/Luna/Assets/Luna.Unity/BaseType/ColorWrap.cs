using System;
using SharpLuna;
using static SharpLuna.Lua;

[WrapClass(typeof(UnityEngine.Color))]
public class ColorWrap
{
	[AOT.MonoPInvokeCallback(typeof(LuaNativeFunction))]
	static int Constructor(IntPtr L)
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
	static int Get_r(IntPtr L)
	{
		ref var obj = ref SharpObject.GetValue<UnityEngine.Color>(L, 1);
		Lua.Push(L, obj.r);
		return 1;
	}

	[AOT.MonoPInvokeCallback(typeof(LuaNativeFunction))]
	static int Set_r(IntPtr L)
	{
		ref var obj = ref SharpObject.GetValue<UnityEngine.Color>(L, 1);
		var p1 = Lua.Get<float>(L, 2);
		obj.r = p1;
		return 0;
	}

	[AOT.MonoPInvokeCallback(typeof(LuaNativeFunction))]
	static int Get_g(IntPtr L)
	{
		ref var obj = ref SharpObject.GetValue<UnityEngine.Color>(L, 1);
		Lua.Push(L, obj.g);
		return 1;
	}

	[AOT.MonoPInvokeCallback(typeof(LuaNativeFunction))]
	static int Set_g(IntPtr L)
	{
		ref var obj = ref SharpObject.GetValue<UnityEngine.Color>(L, 1);
		var p1 = Lua.Get<float>(L, 2);
		obj.g = p1;
		return 0;
	}

	[AOT.MonoPInvokeCallback(typeof(LuaNativeFunction))]
	static int Get_b(IntPtr L)
	{
		ref var obj = ref SharpObject.GetValue<UnityEngine.Color>(L, 1);
		Lua.Push(L, obj.b);
		return 1;
	}

	[AOT.MonoPInvokeCallback(typeof(LuaNativeFunction))]
	static int Set_b(IntPtr L)
	{
		ref var obj = ref SharpObject.GetValue<UnityEngine.Color>(L, 1);
		var p1 = Lua.Get<float>(L, 2);
		obj.b = p1;
		return 0;
	}

	[AOT.MonoPInvokeCallback(typeof(LuaNativeFunction))]
	static int Get_a(IntPtr L)
	{
		ref var obj = ref SharpObject.GetValue<UnityEngine.Color>(L, 1);
		Lua.Push(L, obj.a);
		return 1;
	}

	[AOT.MonoPInvokeCallback(typeof(LuaNativeFunction))]
	static int Set_a(IntPtr L)
	{
		ref var obj = ref SharpObject.GetValue<UnityEngine.Color>(L, 1);
		var p1 = Lua.Get<float>(L, 2);
		obj.a = p1;
		return 0;
	}

	[AOT.MonoPInvokeCallback(typeof(LuaNativeFunction))]
	static int Get_red(IntPtr L)
	{
		ref var obj = ref SharpObject.GetValue<UnityEngine.Color>(L, 1);
		Lua.Push(L, UnityEngine.Color.red);
		return 1;
	}

	[AOT.MonoPInvokeCallback(typeof(LuaNativeFunction))]
	static int Get_green(IntPtr L)
	{
		ref var obj = ref SharpObject.GetValue<UnityEngine.Color>(L, 1);
		Lua.Push(L, UnityEngine.Color.green);
		return 1;
	}

	[AOT.MonoPInvokeCallback(typeof(LuaNativeFunction))]
	static int Get_blue(IntPtr L)
	{
		ref var obj = ref SharpObject.GetValue<UnityEngine.Color>(L, 1);
		Lua.Push(L, UnityEngine.Color.blue);
		return 1;
	}

	[AOT.MonoPInvokeCallback(typeof(LuaNativeFunction))]
	static int Get_white(IntPtr L)
	{
		ref var obj = ref SharpObject.GetValue<UnityEngine.Color>(L, 1);
		Lua.Push(L, UnityEngine.Color.white);
		return 1;
	}

	[AOT.MonoPInvokeCallback(typeof(LuaNativeFunction))]
	static int Get_black(IntPtr L)
	{
		ref var obj = ref SharpObject.GetValue<UnityEngine.Color>(L, 1);
		Lua.Push(L, UnityEngine.Color.black);
		return 1;
	}

	[AOT.MonoPInvokeCallback(typeof(LuaNativeFunction))]
	static int Get_yellow(IntPtr L)
	{
		ref var obj = ref SharpObject.GetValue<UnityEngine.Color>(L, 1);
		Lua.Push(L, UnityEngine.Color.yellow);
		return 1;
	}

	[AOT.MonoPInvokeCallback(typeof(LuaNativeFunction))]
	static int Get_cyan(IntPtr L)
	{
		ref var obj = ref SharpObject.GetValue<UnityEngine.Color>(L, 1);
		Lua.Push(L, UnityEngine.Color.cyan);
		return 1;
	}

	[AOT.MonoPInvokeCallback(typeof(LuaNativeFunction))]
	static int Get_magenta(IntPtr L)
	{
		ref var obj = ref SharpObject.GetValue<UnityEngine.Color>(L, 1);
		Lua.Push(L, UnityEngine.Color.magenta);
		return 1;
	}

	[AOT.MonoPInvokeCallback(typeof(LuaNativeFunction))]
	static int Get_gray(IntPtr L)
	{
		ref var obj = ref SharpObject.GetValue<UnityEngine.Color>(L, 1);
		Lua.Push(L, UnityEngine.Color.gray);
		return 1;
	}

	[AOT.MonoPInvokeCallback(typeof(LuaNativeFunction))]
	static int Get_grey(IntPtr L)
	{
		ref var obj = ref SharpObject.GetValue<UnityEngine.Color>(L, 1);
		Lua.Push(L, UnityEngine.Color.grey);
		return 1;
	}

	[AOT.MonoPInvokeCallback(typeof(LuaNativeFunction))]
	static int Get_clear(IntPtr L)
	{
		ref var obj = ref SharpObject.GetValue<UnityEngine.Color>(L, 1);
		Lua.Push(L, UnityEngine.Color.clear);
		return 1;
	}

	[AOT.MonoPInvokeCallback(typeof(LuaNativeFunction))]
	static int Get_grayscale(IntPtr L)
	{
		ref var obj = ref SharpObject.GetValue<UnityEngine.Color>(L, 1);
		Lua.Push(L, obj.grayscale);
		return 1;
	}

	[AOT.MonoPInvokeCallback(typeof(LuaNativeFunction))]
	static int Get_linear(IntPtr L)
	{
		ref var obj = ref SharpObject.GetValue<UnityEngine.Color>(L, 1);
		Lua.Push(L, obj.linear);
		return 1;
	}

	[AOT.MonoPInvokeCallback(typeof(LuaNativeFunction))]
	static int Get_gamma(IntPtr L)
	{
		ref var obj = ref SharpObject.GetValue<UnityEngine.Color>(L, 1);
		Lua.Push(L, obj.gamma);
		return 1;
	}

	[AOT.MonoPInvokeCallback(typeof(LuaNativeFunction))]
	static int Get_maxColorComponent(IntPtr L)
	{
		ref var obj = ref SharpObject.GetValue<UnityEngine.Color>(L, 1);
		Lua.Push(L, obj.maxColorComponent);
		return 1;
	}

	public static void Register(ClassWraper classWraper)
	{
		classWraper.RegFunction("ctor", Constructor);
		classWraper.RegField("r", Get_r, Set_r);
		classWraper.RegField("g", Get_g, Set_g);
		classWraper.RegField("b", Get_b, Set_b);
		classWraper.RegField("a", Get_a, Set_a);
		classWraper.RegProp("red", Get_red);
		classWraper.RegProp("green", Get_green);
		classWraper.RegProp("blue", Get_blue);
		classWraper.RegProp("white", Get_white);
		classWraper.RegProp("black", Get_black);
		classWraper.RegProp("yellow", Get_yellow);
		classWraper.RegProp("cyan", Get_cyan);
		classWraper.RegProp("magenta", Get_magenta);
		classWraper.RegProp("gray", Get_gray);
		classWraper.RegProp("grey", Get_grey);
		classWraper.RegProp("clear", Get_clear);
		classWraper.RegProp("grayscale", Get_grayscale);
		classWraper.RegProp("linear", Get_linear);
		classWraper.RegProp("gamma", Get_gamma);
		classWraper.RegProp("maxColorComponent", Get_maxColorComponent);
	}
}