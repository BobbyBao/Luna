using System;
using SharpLuna;
using static SharpLuna.Lua;

[WrapClass(typeof(UnityEngine.LayerMask))]
public class LayerMaskWrap
{
	[AOT.MonoPInvokeCallback(typeof(LuaNativeFunction))]
	static int Get_value(IntPtr L)
	{
		ref var obj = ref SharpObject.GetValue<UnityEngine.LayerMask>(L, 1);
		Lua.Push(L, obj.value);
		return 1;
	}

	[AOT.MonoPInvokeCallback(typeof(LuaNativeFunction))]
	static int Set_value(IntPtr L)
	{
		ref var obj = ref SharpObject.GetValue<UnityEngine.LayerMask>(L, 1);
		var p1 = Lua.Get<int>(L, 2);
		obj.value = p1;
		return 0;
	}

	[AOT.MonoPInvokeCallback(typeof(LuaNativeFunction))]
	static int LayerToName(IntPtr L)
	{
		#if LUNA_SCRIPT
		const int startStack = 2;
		#else
		const int startStack = 1;
		#endif
		var ret = UnityEngine.LayerMask.LayerToName(
			Lua.Get<int>(L, 0 + startStack)
		);
		Lua.Push(L, ret);
		return 1;
	}

	[AOT.MonoPInvokeCallback(typeof(LuaNativeFunction))]
	static int NameToLayer(IntPtr L)
	{
		#if LUNA_SCRIPT
		const int startStack = 2;
		#else
		const int startStack = 1;
		#endif
		var ret = UnityEngine.LayerMask.NameToLayer(
			Lua.Get<string>(L, 0 + startStack)
		);
		Lua.Push(L, ret);
		return 1;
	}

	[AOT.MonoPInvokeCallback(typeof(LuaNativeFunction))]
	static int GetMask(IntPtr L)
	{
		#if LUNA_SCRIPT
		const int startStack = 2;
		#else
		const int startStack = 1;
		#endif
		var ret = UnityEngine.LayerMask.GetMask(
			Lua.Get<System.String[]>(L, 0 + startStack)
		);
		Lua.Push(L, ret);
		return 1;
	}

	[AOT.MonoPInvokeCallback(typeof(LuaNativeFunction))]
	static int Equals(IntPtr L)
	{
		const int startStack = 2;
		ref var obj = ref SharpObject.GetValue<UnityEngine.LayerMask>(L, 1);
		var ret = obj.Equals(
			Lua.Get<object>(L, 0 + startStack)
		);
		Lua.Push(L, ret);
		return 1;
	}

	[AOT.MonoPInvokeCallback(typeof(LuaNativeFunction))]
	static int GetHashCode(IntPtr L)
	{
		ref var obj = ref SharpObject.GetValue<UnityEngine.LayerMask>(L, 1);
		var ret = obj.GetHashCode();
		Lua.Push(L, ret);
		return 1;
	}

	[AOT.MonoPInvokeCallback(typeof(LuaNativeFunction))]
	static int ToString(IntPtr L)
	{
		ref var obj = ref SharpObject.GetValue<UnityEngine.LayerMask>(L, 1);
		var ret = obj.ToString();
		Lua.Push(L, ret);
		return 1;
	}

	[AOT.MonoPInvokeCallback(typeof(LuaNativeFunction))]
	static int GetType(IntPtr L)
	{
		ref var obj = ref SharpObject.GetValue<UnityEngine.LayerMask>(L, 1);
		var ret = obj.GetType();
		Lua.Push(L, ret);
		return 1;
	}

	public static void Register(ClassWraper classWraper)
	{
		classWraper.RegProperty("value", Get_value, Set_value);
		classWraper.RegFunction("LayerToName", LayerToName);
		classWraper.RegFunction("NameToLayer", NameToLayer);
		classWraper.RegFunction("GetMask", GetMask);
		classWraper.RegFunction("Equals", Equals);
		classWraper.RegFunction("GetHashCode", GetHashCode);
		classWraper.RegFunction("ToString", ToString);
		classWraper.RegFunction("GetType", GetType);
	}
}