using System;
using SharpLuna;
using static SharpLuna.Lua;

[WrapClass(typeof(UnityEngine.Ray))]
public class RayWrap
{
	[AOT.MonoPInvokeCallback(typeof(LuaNativeFunction))]
	static int Constructor(IntPtr L)
	{
		int n = lua_gettop(L) - 1;
		UnityEngine.Ray obj = default;
		if(n == 0)
			obj = new UnityEngine.Ray();
		else if(n == 2)
			obj = new UnityEngine.Ray(
				Lua.Get<UnityEngine.Vector3>(L, 2),
				Lua.Get<UnityEngine.Vector3>(L, 3)
			);
		Lua.Push(L, obj);
		return 1;
	}

	[AOT.MonoPInvokeCallback(typeof(LuaNativeFunction))]
	static int Get_origin(IntPtr L)
	{
		ref var obj = ref SharpObject.GetValue<UnityEngine.Ray>(L, 1);
		Lua.Push(L, obj.origin);
		return 1;
	}

	[AOT.MonoPInvokeCallback(typeof(LuaNativeFunction))]
	static int Set_origin(IntPtr L)
	{
		ref var obj = ref SharpObject.GetValue<UnityEngine.Ray>(L, 1);
		var p1 = Lua.Get<UnityEngine.Vector3>(L, 2);
		obj.origin = p1;
		return 0;
	}

	[AOT.MonoPInvokeCallback(typeof(LuaNativeFunction))]
	static int Get_direction(IntPtr L)
	{
		ref var obj = ref SharpObject.GetValue<UnityEngine.Ray>(L, 1);
		Lua.Push(L, obj.direction);
		return 1;
	}

	[AOT.MonoPInvokeCallback(typeof(LuaNativeFunction))]
	static int Set_direction(IntPtr L)
	{
		ref var obj = ref SharpObject.GetValue<UnityEngine.Ray>(L, 1);
		var p1 = Lua.Get<UnityEngine.Vector3>(L, 2);
		obj.direction = p1;
		return 0;
	}

	public static void Register(ClassWraper classWraper)
	{
		classWraper.RegConstructor(Constructor);
		classWraper.RegProperty("origin", Get_origin, Set_origin);
		classWraper.RegProperty("direction", Get_direction, Set_direction);
	}
}