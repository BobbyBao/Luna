using System;
using SharpLuna;
using static SharpLuna.Lua;

[WrapClass(typeof(UnityEngine.Plane))]
public class PlaneWrap
{
	[AOT.MonoPInvokeCallback(typeof(LuaNativeFunction))]
	static int Constructor(IntPtr L)
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

	[AOT.MonoPInvokeCallback(typeof(LuaNativeFunction))]
	static int Get_normal(IntPtr L)
	{
		ref var obj = ref SharpObject.GetValue<UnityEngine.Plane>(L, 1);
		Lua.Push(L, obj.normal);
		return 1;
	}

	[AOT.MonoPInvokeCallback(typeof(LuaNativeFunction))]
	static int Set_normal(IntPtr L)
	{
		ref var obj = ref SharpObject.GetValue<UnityEngine.Plane>(L, 1);
		var p1 = Lua.Get<UnityEngine.Vector3>(L, 2);
		obj.normal = p1;
		return 0;
	}

	[AOT.MonoPInvokeCallback(typeof(LuaNativeFunction))]
	static int Get_distance(IntPtr L)
	{
		ref var obj = ref SharpObject.GetValue<UnityEngine.Plane>(L, 1);
		Lua.Push(L, obj.distance);
		return 1;
	}

	[AOT.MonoPInvokeCallback(typeof(LuaNativeFunction))]
	static int Set_distance(IntPtr L)
	{
		ref var obj = ref SharpObject.GetValue<UnityEngine.Plane>(L, 1);
		var p1 = Lua.Get<float>(L, 2);
		obj.distance = p1;
		return 0;
	}

	[AOT.MonoPInvokeCallback(typeof(LuaNativeFunction))]
	static int Get_flipped(IntPtr L)
	{
		ref var obj = ref SharpObject.GetValue<UnityEngine.Plane>(L, 1);
		Lua.Push(L, obj.flipped);
		return 1;
	}

	public static void Register(ClassWraper classWraper)
	{
		classWraper.RegFunction("ctor", Constructor);
		classWraper.RegProp("normal", Get_normal, Set_normal);
		classWraper.RegProp("distance", Get_distance, Set_distance);
		classWraper.RegProp("flipped", Get_flipped);
	}
}