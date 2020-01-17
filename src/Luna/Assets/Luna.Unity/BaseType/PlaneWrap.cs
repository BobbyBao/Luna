using System;
using SharpLuna;
using static SharpLuna.Lua;

[WrapClass(typeof(UnityEngine.Plane))]
public class PlaneWrap
{
	[AOT.MonoPInvokeCallback(typeof(LuaNativeFunction))]
	static int Constructor(IntPtr L)
	{
		int n = lua_gettop(L) - 1;
		UnityEngine.Plane obj = default;
		if(n == 0)
			obj = new UnityEngine.Plane();
		else if(n == 2)
			obj = new UnityEngine.Plane(
				Lua.Get<UnityEngine.Vector3>(L, 2),
				Lua.Get<UnityEngine.Vector3>(L, 3)
			);
		else if(n == 2)
			obj = new UnityEngine.Plane(
				Lua.Get<UnityEngine.Vector3>(L, 2),
				Lua.Get<float>(L, 3)
			);
		else if(n == 3)
			obj = new UnityEngine.Plane(
				Lua.Get<UnityEngine.Vector3>(L, 2),
				Lua.Get<UnityEngine.Vector3>(L, 3),
				Lua.Get<UnityEngine.Vector3>(L, 4)
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
		classWraper.RegConstructor(Constructor);
		classWraper.RegProperty("normal", Get_normal, Set_normal);
		classWraper.RegProperty("distance", Get_distance, Set_distance);
		classWraper.RegProperty("flipped", Get_flipped);
	}
}