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
		{
			obj = new UnityEngine.Plane();
		}
		else if(n == 2)
		{
			obj = new UnityEngine.Plane(
				Lua.Get<UnityEngine.Vector3>(L, 2),
				Lua.Get<UnityEngine.Vector3>(L, 3)
			);
		}
		else if(n == 2)
		{
			obj = new UnityEngine.Plane(
				Lua.Get<UnityEngine.Vector3>(L, 2),
				Lua.Get<float>(L, 3)
			);
		}
		else if(n == 3)
		{
			obj = new UnityEngine.Plane(
				Lua.Get<UnityEngine.Vector3>(L, 2),
				Lua.Get<UnityEngine.Vector3>(L, 3),
				Lua.Get<UnityEngine.Vector3>(L, 4)
			);
		}
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

	[AOT.MonoPInvokeCallback(typeof(LuaNativeFunction))]
	static int SetNormalAndPosition(IntPtr L)
	{
		const int startStack = 2;
		ref var obj = ref SharpObject.GetValue<UnityEngine.Plane>(L, 1);
		obj.SetNormalAndPosition(
			Lua.Get<UnityEngine.Vector3>(L, 0 + startStack),
			Lua.Get<UnityEngine.Vector3>(L, 1 + startStack)
		);
		return 0;
	}

	[AOT.MonoPInvokeCallback(typeof(LuaNativeFunction))]
	static int Set3Points(IntPtr L)
	{
		const int startStack = 2;
		ref var obj = ref SharpObject.GetValue<UnityEngine.Plane>(L, 1);
		obj.Set3Points(
			Lua.Get<UnityEngine.Vector3>(L, 0 + startStack),
			Lua.Get<UnityEngine.Vector3>(L, 1 + startStack),
			Lua.Get<UnityEngine.Vector3>(L, 2 + startStack)
		);
		return 0;
	}

	[AOT.MonoPInvokeCallback(typeof(LuaNativeFunction))]
	static int Flip(IntPtr L)
	{
		ref var obj = ref SharpObject.GetValue<UnityEngine.Plane>(L, 1);
		obj.Flip();
		return 0;
	}

	[AOT.MonoPInvokeCallback(typeof(LuaNativeFunction))]
	static int Translate(IntPtr L)
	{
		int n = lua_gettop(L) - 1;
		if(n == 1)
		{
			const int startStack = 2;
			ref var obj = ref SharpObject.GetValue<UnityEngine.Plane>(L, 1);
			obj.Translate(
				Lua.Get<UnityEngine.Vector3>(L, 0 + startStack)
			);
			return 0;
		}
		else if(n == 2)
		{
			#if LUNA_SCRIPT
			const int startStack = 2;
			#else
			const int startStack = 1;
			#endif
			var ret = UnityEngine.Plane.Translate(
				Lua.Get<UnityEngine.Plane>(L, 0 + startStack),
				Lua.Get<UnityEngine.Vector3>(L, 1 + startStack)
			);
			Lua.Push(L, ret);
			return 1;
		}
		return 0;
	}

	[AOT.MonoPInvokeCallback(typeof(LuaNativeFunction))]
	static int ClosestPointOnPlane(IntPtr L)
	{
		const int startStack = 2;
		ref var obj = ref SharpObject.GetValue<UnityEngine.Plane>(L, 1);
		var ret = obj.ClosestPointOnPlane(
			Lua.Get<UnityEngine.Vector3>(L, 0 + startStack)
		);
		Lua.Push(L, ret);
		return 1;
	}

	[AOT.MonoPInvokeCallback(typeof(LuaNativeFunction))]
	static int GetDistanceToPoint(IntPtr L)
	{
		const int startStack = 2;
		ref var obj = ref SharpObject.GetValue<UnityEngine.Plane>(L, 1);
		var ret = obj.GetDistanceToPoint(
			Lua.Get<UnityEngine.Vector3>(L, 0 + startStack)
		);
		Lua.Push(L, ret);
		return 1;
	}

	[AOT.MonoPInvokeCallback(typeof(LuaNativeFunction))]
	static int GetSide(IntPtr L)
	{
		const int startStack = 2;
		ref var obj = ref SharpObject.GetValue<UnityEngine.Plane>(L, 1);
		var ret = obj.GetSide(
			Lua.Get<UnityEngine.Vector3>(L, 0 + startStack)
		);
		Lua.Push(L, ret);
		return 1;
	}

	[AOT.MonoPInvokeCallback(typeof(LuaNativeFunction))]
	static int SameSide(IntPtr L)
	{
		const int startStack = 2;
		ref var obj = ref SharpObject.GetValue<UnityEngine.Plane>(L, 1);
		var ret = obj.SameSide(
			Lua.Get<UnityEngine.Vector3>(L, 0 + startStack),
			Lua.Get<UnityEngine.Vector3>(L, 1 + startStack)
		);
		Lua.Push(L, ret);
		return 1;
	}

	[AOT.MonoPInvokeCallback(typeof(LuaNativeFunction))]
	static int ToString(IntPtr L)
	{
		int n = lua_gettop(L) - 1;
		if(n == 0)
		{
			ref var obj = ref SharpObject.GetValue<UnityEngine.Plane>(L, 1);
			var ret = obj.ToString();
			Lua.Push(L, ret);
			return 1;
		}
		else if(n == 1)
		{
			const int startStack = 2;
			ref var obj = ref SharpObject.GetValue<UnityEngine.Plane>(L, 1);
			var ret = obj.ToString(
				Lua.Get<string>(L, 0 + startStack)
			);
			Lua.Push(L, ret);
			return 1;
		}
		return 0;
	}

	[AOT.MonoPInvokeCallback(typeof(LuaNativeFunction))]
	static int Equals(IntPtr L)
	{
		const int startStack = 2;
		ref var obj = ref SharpObject.GetValue<UnityEngine.Plane>(L, 1);
		var ret = obj.Equals(
			Lua.Get<object>(L, 0 + startStack)
		);
		Lua.Push(L, ret);
		return 1;
	}

	[AOT.MonoPInvokeCallback(typeof(LuaNativeFunction))]
	static int GetHashCode(IntPtr L)
	{
		ref var obj = ref SharpObject.GetValue<UnityEngine.Plane>(L, 1);
		var ret = obj.GetHashCode();
		Lua.Push(L, ret);
		return 1;
	}

	[AOT.MonoPInvokeCallback(typeof(LuaNativeFunction))]
	static int GetType(IntPtr L)
	{
		ref var obj = ref SharpObject.GetValue<UnityEngine.Plane>(L, 1);
		var ret = obj.GetType();
		Lua.Push(L, ret);
		return 1;
	}

	public static void Register(ClassWraper classWraper)
	{
		classWraper.RegConstructor(Constructor);
		classWraper.RegProperty("normal", Get_normal, Set_normal);
		classWraper.RegProperty("distance", Get_distance, Set_distance);
		classWraper.RegProperty("flipped", Get_flipped);
		classWraper.RegFunction("SetNormalAndPosition", SetNormalAndPosition);
		classWraper.RegFunction("Set3Points", Set3Points);
		classWraper.RegFunction("Flip", Flip);
		classWraper.RegFunction("Translate", Translate);
		classWraper.RegFunction("ClosestPointOnPlane", ClosestPointOnPlane);
		classWraper.RegFunction("GetDistanceToPoint", GetDistanceToPoint);
		classWraper.RegFunction("GetSide", GetSide);
		classWraper.RegFunction("SameSide", SameSide);
		classWraper.RegFunction("ToString", ToString);
		classWraper.RegFunction("Equals", Equals);
		classWraper.RegFunction("GetHashCode", GetHashCode);
		classWraper.RegFunction("GetType", GetType);
	}
}