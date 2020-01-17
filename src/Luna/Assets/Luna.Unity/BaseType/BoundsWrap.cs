using System;
using SharpLuna;
using static SharpLuna.Lua;

[WrapClass(typeof(UnityEngine.Bounds))]
public class BoundsWrap
{
	[AOT.MonoPInvokeCallback(typeof(LuaNativeFunction))]
	static int Constructor(IntPtr L)
	{
		int n = lua_gettop(L) - 1;
		UnityEngine.Bounds obj = default;
		if(n == 0)
			obj = new UnityEngine.Bounds();
		else if(n == 2)
			obj = new UnityEngine.Bounds(
				Lua.Get<UnityEngine.Vector3>(L, 2),
				Lua.Get<UnityEngine.Vector3>(L, 3)
			);
		Lua.Push(L, obj);
		return 1;
	}

	[AOT.MonoPInvokeCallback(typeof(LuaNativeFunction))]
	static int Get_center(IntPtr L)
	{
		ref var obj = ref SharpObject.GetValue<UnityEngine.Bounds>(L, 1);
		Lua.Push(L, obj.center);
		return 1;
	}

	[AOT.MonoPInvokeCallback(typeof(LuaNativeFunction))]
	static int Set_center(IntPtr L)
	{
		ref var obj = ref SharpObject.GetValue<UnityEngine.Bounds>(L, 1);
		var p1 = Lua.Get<UnityEngine.Vector3>(L, 2);
		obj.center = p1;
		return 0;
	}

	[AOT.MonoPInvokeCallback(typeof(LuaNativeFunction))]
	static int Get_size(IntPtr L)
	{
		ref var obj = ref SharpObject.GetValue<UnityEngine.Bounds>(L, 1);
		Lua.Push(L, obj.size);
		return 1;
	}

	[AOT.MonoPInvokeCallback(typeof(LuaNativeFunction))]
	static int Set_size(IntPtr L)
	{
		ref var obj = ref SharpObject.GetValue<UnityEngine.Bounds>(L, 1);
		var p1 = Lua.Get<UnityEngine.Vector3>(L, 2);
		obj.size = p1;
		return 0;
	}

	[AOT.MonoPInvokeCallback(typeof(LuaNativeFunction))]
	static int Get_extents(IntPtr L)
	{
		ref var obj = ref SharpObject.GetValue<UnityEngine.Bounds>(L, 1);
		Lua.Push(L, obj.extents);
		return 1;
	}

	[AOT.MonoPInvokeCallback(typeof(LuaNativeFunction))]
	static int Set_extents(IntPtr L)
	{
		ref var obj = ref SharpObject.GetValue<UnityEngine.Bounds>(L, 1);
		var p1 = Lua.Get<UnityEngine.Vector3>(L, 2);
		obj.extents = p1;
		return 0;
	}

	[AOT.MonoPInvokeCallback(typeof(LuaNativeFunction))]
	static int Get_min(IntPtr L)
	{
		ref var obj = ref SharpObject.GetValue<UnityEngine.Bounds>(L, 1);
		Lua.Push(L, obj.min);
		return 1;
	}

	[AOT.MonoPInvokeCallback(typeof(LuaNativeFunction))]
	static int Set_min(IntPtr L)
	{
		ref var obj = ref SharpObject.GetValue<UnityEngine.Bounds>(L, 1);
		var p1 = Lua.Get<UnityEngine.Vector3>(L, 2);
		obj.min = p1;
		return 0;
	}

	[AOT.MonoPInvokeCallback(typeof(LuaNativeFunction))]
	static int Get_max(IntPtr L)
	{
		ref var obj = ref SharpObject.GetValue<UnityEngine.Bounds>(L, 1);
		Lua.Push(L, obj.max);
		return 1;
	}

	[AOT.MonoPInvokeCallback(typeof(LuaNativeFunction))]
	static int Set_max(IntPtr L)
	{
		ref var obj = ref SharpObject.GetValue<UnityEngine.Bounds>(L, 1);
		var p1 = Lua.Get<UnityEngine.Vector3>(L, 2);
		obj.max = p1;
		return 0;
	}

	public static void Register(ClassWraper classWraper)
	{
		classWraper.RegConstructor(Constructor);
		classWraper.RegProperty("center", Get_center, Set_center);
		classWraper.RegProperty("size", Get_size, Set_size);
		classWraper.RegProperty("extents", Get_extents, Set_extents);
		classWraper.RegProperty("min", Get_min, Set_min);
		classWraper.RegProperty("max", Get_max, Set_max);
	}
}