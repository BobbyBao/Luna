using System;
using SharpLuna;
using static SharpLuna.Lua;

[WrapClass(typeof(UnityEngine.Vector3))]
public class Vector3Wrap
{
	[AOT.MonoPInvokeCallback(typeof(LuaNativeFunction))]
	static int Constructor(IntPtr L)
	{
		int n = lua_gettop(L);
		UnityEngine.Vector3 obj = default;
		if(n == 3)
			obj = new UnityEngine.Vector3(
				Lua.Get<float>(L, 1),
				Lua.Get<float>(L, 2),
				Lua.Get<float>(L, 3)
			);
		else if(n == 2)
			obj = new UnityEngine.Vector3(
				Lua.Get<float>(L, 1),
				Lua.Get<float>(L, 2)
			);
		Lua.Push(L, obj);
		return 1;
	}

	[AOT.MonoPInvokeCallback(typeof(LuaNativeFunction))]
	static int Get_x(IntPtr L)
	{
		ref var obj = ref SharpObject.GetValue<UnityEngine.Vector3>(L, 1);
		Lua.Push(L, obj.x);
		return 1;
	}

	[AOT.MonoPInvokeCallback(typeof(LuaNativeFunction))]
	static int Set_x(IntPtr L)
	{
		ref var obj = ref SharpObject.GetValue<UnityEngine.Vector3>(L, 1);
		var p1 = Lua.Get<float>(L, 2);
		obj.x = p1;
		return 0;
	}

	[AOT.MonoPInvokeCallback(typeof(LuaNativeFunction))]
	static int Get_y(IntPtr L)
	{
		ref var obj = ref SharpObject.GetValue<UnityEngine.Vector3>(L, 1);
		Lua.Push(L, obj.y);
		return 1;
	}

	[AOT.MonoPInvokeCallback(typeof(LuaNativeFunction))]
	static int Set_y(IntPtr L)
	{
		ref var obj = ref SharpObject.GetValue<UnityEngine.Vector3>(L, 1);
		var p1 = Lua.Get<float>(L, 2);
		obj.y = p1;
		return 0;
	}

	[AOT.MonoPInvokeCallback(typeof(LuaNativeFunction))]
	static int Get_z(IntPtr L)
	{
		ref var obj = ref SharpObject.GetValue<UnityEngine.Vector3>(L, 1);
		Lua.Push(L, obj.z);
		return 1;
	}

	[AOT.MonoPInvokeCallback(typeof(LuaNativeFunction))]
	static int Set_z(IntPtr L)
	{
		ref var obj = ref SharpObject.GetValue<UnityEngine.Vector3>(L, 1);
		var p1 = Lua.Get<float>(L, 2);
		obj.z = p1;
		return 0;
	}

	[AOT.MonoPInvokeCallback(typeof(LuaNativeFunction))]
	static int Get_normalized(IntPtr L)
	{
		ref var obj = ref SharpObject.GetValue<UnityEngine.Vector3>(L, 1);
		Lua.Push(L, obj.normalized);
		return 1;
	}

	[AOT.MonoPInvokeCallback(typeof(LuaNativeFunction))]
	static int Get_magnitude(IntPtr L)
	{
		ref var obj = ref SharpObject.GetValue<UnityEngine.Vector3>(L, 1);
		Lua.Push(L, obj.magnitude);
		return 1;
	}

	[AOT.MonoPInvokeCallback(typeof(LuaNativeFunction))]
	static int Get_sqrMagnitude(IntPtr L)
	{
		ref var obj = ref SharpObject.GetValue<UnityEngine.Vector3>(L, 1);
		Lua.Push(L, obj.sqrMagnitude);
		return 1;
	}

	[AOT.MonoPInvokeCallback(typeof(LuaNativeFunction))]
	static int Get_zero(IntPtr L)
	{
		ref var obj = ref SharpObject.GetValue<UnityEngine.Vector3>(L, 1);
		Lua.Push(L, UnityEngine.Vector3.zero);
		return 1;
	}

	[AOT.MonoPInvokeCallback(typeof(LuaNativeFunction))]
	static int Get_one(IntPtr L)
	{
		ref var obj = ref SharpObject.GetValue<UnityEngine.Vector3>(L, 1);
		Lua.Push(L, UnityEngine.Vector3.one);
		return 1;
	}

	[AOT.MonoPInvokeCallback(typeof(LuaNativeFunction))]
	static int Get_forward(IntPtr L)
	{
		ref var obj = ref SharpObject.GetValue<UnityEngine.Vector3>(L, 1);
		Lua.Push(L, UnityEngine.Vector3.forward);
		return 1;
	}

	[AOT.MonoPInvokeCallback(typeof(LuaNativeFunction))]
	static int Get_back(IntPtr L)
	{
		ref var obj = ref SharpObject.GetValue<UnityEngine.Vector3>(L, 1);
		Lua.Push(L, UnityEngine.Vector3.back);
		return 1;
	}

	[AOT.MonoPInvokeCallback(typeof(LuaNativeFunction))]
	static int Get_up(IntPtr L)
	{
		ref var obj = ref SharpObject.GetValue<UnityEngine.Vector3>(L, 1);
		Lua.Push(L, UnityEngine.Vector3.up);
		return 1;
	}

	[AOT.MonoPInvokeCallback(typeof(LuaNativeFunction))]
	static int Get_down(IntPtr L)
	{
		ref var obj = ref SharpObject.GetValue<UnityEngine.Vector3>(L, 1);
		Lua.Push(L, UnityEngine.Vector3.down);
		return 1;
	}

	[AOT.MonoPInvokeCallback(typeof(LuaNativeFunction))]
	static int Get_left(IntPtr L)
	{
		ref var obj = ref SharpObject.GetValue<UnityEngine.Vector3>(L, 1);
		Lua.Push(L, UnityEngine.Vector3.left);
		return 1;
	}

	[AOT.MonoPInvokeCallback(typeof(LuaNativeFunction))]
	static int Get_right(IntPtr L)
	{
		ref var obj = ref SharpObject.GetValue<UnityEngine.Vector3>(L, 1);
		Lua.Push(L, UnityEngine.Vector3.right);
		return 1;
	}

	[AOT.MonoPInvokeCallback(typeof(LuaNativeFunction))]
	static int Get_positiveInfinity(IntPtr L)
	{
		ref var obj = ref SharpObject.GetValue<UnityEngine.Vector3>(L, 1);
		Lua.Push(L, UnityEngine.Vector3.positiveInfinity);
		return 1;
	}

	[AOT.MonoPInvokeCallback(typeof(LuaNativeFunction))]
	static int Get_negativeInfinity(IntPtr L)
	{
		ref var obj = ref SharpObject.GetValue<UnityEngine.Vector3>(L, 1);
		Lua.Push(L, UnityEngine.Vector3.negativeInfinity);
		return 1;
	}

	[AOT.MonoPInvokeCallback(typeof(LuaNativeFunction))]
	static int Get_fwd(IntPtr L)
	{
		ref var obj = ref SharpObject.GetValue<UnityEngine.Vector3>(L, 1);
		Lua.Push(L, UnityEngine.Vector3.fwd);
		return 1;
	}

	public static void Register(ClassWraper classWraper)
	{
		classWraper.RegFunction("ctor", Constructor);
		classWraper.RegField("x", Get_x, Set_x);
		classWraper.RegField("y", Get_y, Set_y);
		classWraper.RegField("z", Get_z, Set_z);
		classWraper.RegProp("normalized", Get_normalized);
		classWraper.RegProp("magnitude", Get_magnitude);
		classWraper.RegProp("sqrMagnitude", Get_sqrMagnitude);
		classWraper.RegProp("zero", Get_zero);
		classWraper.RegProp("one", Get_one);
		classWraper.RegProp("forward", Get_forward);
		classWraper.RegProp("back", Get_back);
		classWraper.RegProp("up", Get_up);
		classWraper.RegProp("down", Get_down);
		classWraper.RegProp("left", Get_left);
		classWraper.RegProp("right", Get_right);
		classWraper.RegProp("positiveInfinity", Get_positiveInfinity);
		classWraper.RegProp("negativeInfinity", Get_negativeInfinity);
		classWraper.RegProp("fwd", Get_fwd);
	}
}