using System;
using SharpLuna;
using static SharpLuna.Lua;

[WrapClass(typeof(UnityEngine.Color))]
public class ColorWrap
{
	[AOT.MonoPInvokeCallback(typeof(LuaNativeFunction))]
	static int Constructor(IntPtr L)
	{
		int n = lua_gettop(L) - 1;
		UnityEngine.Color obj = default;
		if(n == 0)
		{
			obj = new UnityEngine.Color();
		}
		else if(n == 3)
		{
			obj = new UnityEngine.Color(
				Lua.Get<float>(L, 2),
				Lua.Get<float>(L, 3),
				Lua.Get<float>(L, 4)
			);
		}
		else if(n == 4)
		{
			obj = new UnityEngine.Color(
				Lua.Get<float>(L, 2),
				Lua.Get<float>(L, 3),
				Lua.Get<float>(L, 4),
				Lua.Get<float>(L, 5)
			);
		}
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
		Lua.Push(L, UnityEngine.Color.red);
		return 1;
	}

	[AOT.MonoPInvokeCallback(typeof(LuaNativeFunction))]
	static int Get_green(IntPtr L)
	{
		Lua.Push(L, UnityEngine.Color.green);
		return 1;
	}

	[AOT.MonoPInvokeCallback(typeof(LuaNativeFunction))]
	static int Get_blue(IntPtr L)
	{
		Lua.Push(L, UnityEngine.Color.blue);
		return 1;
	}

	[AOT.MonoPInvokeCallback(typeof(LuaNativeFunction))]
	static int Get_white(IntPtr L)
	{
		Lua.Push(L, UnityEngine.Color.white);
		return 1;
	}

	[AOT.MonoPInvokeCallback(typeof(LuaNativeFunction))]
	static int Get_black(IntPtr L)
	{
		Lua.Push(L, UnityEngine.Color.black);
		return 1;
	}

	[AOT.MonoPInvokeCallback(typeof(LuaNativeFunction))]
	static int Get_yellow(IntPtr L)
	{
		Lua.Push(L, UnityEngine.Color.yellow);
		return 1;
	}

	[AOT.MonoPInvokeCallback(typeof(LuaNativeFunction))]
	static int Get_cyan(IntPtr L)
	{
		Lua.Push(L, UnityEngine.Color.cyan);
		return 1;
	}

	[AOT.MonoPInvokeCallback(typeof(LuaNativeFunction))]
	static int Get_magenta(IntPtr L)
	{
		Lua.Push(L, UnityEngine.Color.magenta);
		return 1;
	}

	[AOT.MonoPInvokeCallback(typeof(LuaNativeFunction))]
	static int Get_gray(IntPtr L)
	{
		Lua.Push(L, UnityEngine.Color.gray);
		return 1;
	}

	[AOT.MonoPInvokeCallback(typeof(LuaNativeFunction))]
	static int Get_grey(IntPtr L)
	{
		Lua.Push(L, UnityEngine.Color.grey);
		return 1;
	}

	[AOT.MonoPInvokeCallback(typeof(LuaNativeFunction))]
	static int Get_clear(IntPtr L)
	{
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

	[AOT.MonoPInvokeCallback(typeof(LuaNativeFunction))]
	static int ToString(IntPtr L)
	{
		int n = lua_gettop(L) - 1;
		if(n == 0)
		{
			ref var obj = ref SharpObject.GetValue<UnityEngine.Color>(L, 1);
			var ret = obj.ToString();
			Lua.Push(L, ret);
			return 1;
		}
		else if(n == 1)
		{
			const int startStack = 2;
			ref var obj = ref SharpObject.GetValue<UnityEngine.Color>(L, 1);
			var ret = obj.ToString(
				Lua.Get<string>(L, 0 + startStack)
			);
			Lua.Push(L, ret);
			return 1;
		}
		return 0;
	}

	[AOT.MonoPInvokeCallback(typeof(LuaNativeFunction))]
	static int GetHashCode(IntPtr L)
	{
		ref var obj = ref SharpObject.GetValue<UnityEngine.Color>(L, 1);
		var ret = obj.GetHashCode();
		Lua.Push(L, ret);
		return 1;
	}

	[AOT.MonoPInvokeCallback(typeof(LuaNativeFunction))]
	static int Equals(IntPtr L)
	{
		int n = lua_gettop(L) - 1;
		if(n == 1)
		{
			const int startStack = 2;
			ref var obj = ref SharpObject.GetValue<UnityEngine.Color>(L, 1);
			var ret = obj.Equals(
				Lua.Get<object>(L, 0 + startStack)
			);
			Lua.Push(L, ret);
			return 1;
		}
		else if(n == 1)
		{
			const int startStack = 2;
			ref var obj = ref SharpObject.GetValue<UnityEngine.Color>(L, 1);
			var ret = obj.Equals(
				Lua.Get<UnityEngine.Color>(L, 0 + startStack)
			);
			Lua.Push(L, ret);
			return 1;
		}
		return 0;
	}

	[AOT.MonoPInvokeCallback(typeof(LuaNativeFunction))]
	static int Lerp(IntPtr L)
	{
		#if LUNA_SCRIPT
		const int startStack = 2;
		#else
		const int startStack = 1;
		#endif
		var ret = UnityEngine.Color.Lerp(
			Lua.Get<UnityEngine.Color>(L, 0 + startStack),
			Lua.Get<UnityEngine.Color>(L, 1 + startStack),
			Lua.Get<float>(L, 2 + startStack)
		);
		Lua.Push(L, ret);
		return 1;
	}

	[AOT.MonoPInvokeCallback(typeof(LuaNativeFunction))]
	static int LerpUnclamped(IntPtr L)
	{
		#if LUNA_SCRIPT
		const int startStack = 2;
		#else
		const int startStack = 1;
		#endif
		var ret = UnityEngine.Color.LerpUnclamped(
			Lua.Get<UnityEngine.Color>(L, 0 + startStack),
			Lua.Get<UnityEngine.Color>(L, 1 + startStack),
			Lua.Get<float>(L, 2 + startStack)
		);
		Lua.Push(L, ret);
		return 1;
	}

	[AOT.MonoPInvokeCallback(typeof(LuaNativeFunction))]
	static int HSVToRGB(IntPtr L)
	{
		int n = lua_gettop(L) - 1;
		if(n == 3)
		{
			#if LUNA_SCRIPT
			const int startStack = 2;
			#else
			const int startStack = 1;
			#endif
			var ret = UnityEngine.Color.HSVToRGB(
				Lua.Get<float>(L, 0 + startStack),
				Lua.Get<float>(L, 1 + startStack),
				Lua.Get<float>(L, 2 + startStack)
			);
			Lua.Push(L, ret);
			return 1;
		}
		else if(n == 4)
		{
			#if LUNA_SCRIPT
			const int startStack = 2;
			#else
			const int startStack = 1;
			#endif
			var ret = UnityEngine.Color.HSVToRGB(
				Lua.Get<float>(L, 0 + startStack),
				Lua.Get<float>(L, 1 + startStack),
				Lua.Get<float>(L, 2 + startStack),
				Lua.Get<bool>(L, 3 + startStack)
			);
			Lua.Push(L, ret);
			return 1;
		}
		return 0;
	}

	[AOT.MonoPInvokeCallback(typeof(LuaNativeFunction))]
	static int GetType(IntPtr L)
	{
		ref var obj = ref SharpObject.GetValue<UnityEngine.Color>(L, 1);
		var ret = obj.GetType();
		Lua.Push(L, ret);
		return 1;
	}

	public static void Register(ClassWraper classWraper)
	{
		classWraper.RegConstructor(Constructor);
		classWraper.RegField("r", Get_r, Set_r);
		classWraper.RegField("g", Get_g, Set_g);
		classWraper.RegField("b", Get_b, Set_b);
		classWraper.RegField("a", Get_a, Set_a);
		classWraper.RegProperty("red", Get_red);
		classWraper.RegProperty("green", Get_green);
		classWraper.RegProperty("blue", Get_blue);
		classWraper.RegProperty("white", Get_white);
		classWraper.RegProperty("black", Get_black);
		classWraper.RegProperty("yellow", Get_yellow);
		classWraper.RegProperty("cyan", Get_cyan);
		classWraper.RegProperty("magenta", Get_magenta);
		classWraper.RegProperty("gray", Get_gray);
		classWraper.RegProperty("grey", Get_grey);
		classWraper.RegProperty("clear", Get_clear);
		classWraper.RegProperty("grayscale", Get_grayscale);
		classWraper.RegProperty("linear", Get_linear);
		classWraper.RegProperty("gamma", Get_gamma);
		classWraper.RegProperty("maxColorComponent", Get_maxColorComponent);
		classWraper.RegFunction("ToString", ToString);
		classWraper.RegFunction("GetHashCode", GetHashCode);
		classWraper.RegFunction("Equals", Equals);
		classWraper.RegFunction("Lerp", Lerp);
		classWraper.RegFunction("LerpUnclamped", LerpUnclamped);
		classWraper.RegFunction("HSVToRGB", HSVToRGB);
		classWraper.RegFunction("GetType", GetType);
	}
}