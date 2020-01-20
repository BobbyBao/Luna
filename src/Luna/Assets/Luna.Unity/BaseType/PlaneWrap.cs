using System;
using SharpLuna;
using System.Collections.Generic;
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
			Get(L, 2, out UnityEngine.Vector3 t1);
			Get(L, 3, out UnityEngine.Vector3 t2);
			obj = new UnityEngine.Plane(t1, t2);
		}
		else if(n == 2)
		{
			Get(L, 2, out UnityEngine.Vector3 t1);
			Get(L, 3, out float t2);
			obj = new UnityEngine.Plane(t1, t2);
		}
		else if(n == 3)
		{
			Get(L, 2, out UnityEngine.Vector3 t1);
			Get(L, 3, out UnityEngine.Vector3 t2);
			Get(L, 4, out UnityEngine.Vector3 t3);
			obj = new UnityEngine.Plane(t1, t2, t3);
		}
		Push(L, obj);
		return 1;
	}

	[AOT.MonoPInvokeCallback(typeof(LuaNativeFunction))]
	static int Get_normal(IntPtr L)
	{
		ref var obj = ref SharpObject.GetValue<UnityEngine.Plane>(L, 1);
		Push(L, obj.normal);
		return 1;
	}

	[AOT.MonoPInvokeCallback(typeof(LuaNativeFunction))]
	static int Set_normal(IntPtr L)
	{
		ref var obj = ref SharpObject.GetValue<UnityEngine.Plane>(L, 1);
		Get(L, 2, out UnityEngine.Vector3 p1);
		obj.normal = p1;
		return 0;
	}

	[AOT.MonoPInvokeCallback(typeof(LuaNativeFunction))]
	static int Get_distance(IntPtr L)
	{
		ref var obj = ref SharpObject.GetValue<UnityEngine.Plane>(L, 1);
		Push(L, obj.distance);
		return 1;
	}

	[AOT.MonoPInvokeCallback(typeof(LuaNativeFunction))]
	static int Set_distance(IntPtr L)
	{
		ref var obj = ref SharpObject.GetValue<UnityEngine.Plane>(L, 1);
		Get(L, 2, out float p1);
		obj.distance = p1;
		return 0;
	}

	[AOT.MonoPInvokeCallback(typeof(LuaNativeFunction))]
	static int Get_flipped(IntPtr L)
	{
		ref var obj = ref SharpObject.GetValue<UnityEngine.Plane>(L, 1);
		Push(L, obj.flipped);
		return 1;
	}

	[AOT.MonoPInvokeCallback(typeof(LuaNativeFunction))]
	static int SetNormalAndPosition(IntPtr L)
	{
		const int startStack = 2;
		ref var obj = ref SharpObject.GetValue<UnityEngine.Plane>(L, 1);
		Get(L, 0 + startStack, out UnityEngine.Vector3 t0);
		Get(L, 1 + startStack, out UnityEngine.Vector3 t1);
		obj.SetNormalAndPosition(t0, t1);
		return 0;
	}

	[AOT.MonoPInvokeCallback(typeof(LuaNativeFunction))]
	static int Set3Points(IntPtr L)
	{
		const int startStack = 2;
		ref var obj = ref SharpObject.GetValue<UnityEngine.Plane>(L, 1);
		Get(L, 0 + startStack, out UnityEngine.Vector3 t0);
		Get(L, 1 + startStack, out UnityEngine.Vector3 t1);
		Get(L, 2 + startStack, out UnityEngine.Vector3 t2);
		obj.Set3Points(t0, t1, t2);
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
			Get(L, 0 + startStack, out UnityEngine.Vector3 t0);
			obj.Translate(t0);
			return 0;
		}
		else if(n == 2)
		{
			#if LUNA_SCRIPT
			const int startStack = 2;
			#else
			const int startStack = 1;
			#endif
			Get(L, 0 + startStack, out UnityEngine.Plane t0);
			Get(L, 1 + startStack, out UnityEngine.Vector3 t1);
			UnityEngine.Plane ret = UnityEngine.Plane.Translate(t0, t1);
			Push(L, ret);
			return 1;
		}
		return 0;
	}

	[AOT.MonoPInvokeCallback(typeof(LuaNativeFunction))]
	static int ClosestPointOnPlane(IntPtr L)
	{
		const int startStack = 2;
		ref var obj = ref SharpObject.GetValue<UnityEngine.Plane>(L, 1);
		Get(L, 0 + startStack, out UnityEngine.Vector3 t0);
		UnityEngine.Vector3 ret = obj.ClosestPointOnPlane(t0);
		Push(L, ret);
		return 1;
	}

	[AOT.MonoPInvokeCallback(typeof(LuaNativeFunction))]
	static int GetDistanceToPoint(IntPtr L)
	{
		const int startStack = 2;
		ref var obj = ref SharpObject.GetValue<UnityEngine.Plane>(L, 1);
		Get(L, 0 + startStack, out UnityEngine.Vector3 t0);
		float ret = obj.GetDistanceToPoint(t0);
		Push(L, ret);
		return 1;
	}

	[AOT.MonoPInvokeCallback(typeof(LuaNativeFunction))]
	static int GetSide(IntPtr L)
	{
		const int startStack = 2;
		ref var obj = ref SharpObject.GetValue<UnityEngine.Plane>(L, 1);
		Get(L, 0 + startStack, out UnityEngine.Vector3 t0);
		bool ret = obj.GetSide(t0);
		Push(L, ret);
		return 1;
	}

	[AOT.MonoPInvokeCallback(typeof(LuaNativeFunction))]
	static int SameSide(IntPtr L)
	{
		const int startStack = 2;
		ref var obj = ref SharpObject.GetValue<UnityEngine.Plane>(L, 1);
		Get(L, 0 + startStack, out UnityEngine.Vector3 t0);
		Get(L, 1 + startStack, out UnityEngine.Vector3 t1);
		bool ret = obj.SameSide(t0, t1);
		Push(L, ret);
		return 1;
	}

	[AOT.MonoPInvokeCallback(typeof(LuaNativeFunction))]
	static int ToString(IntPtr L)
	{
		int n = lua_gettop(L) - 1;
		if(n == 0)
		{
			ref var obj = ref SharpObject.GetValue<UnityEngine.Plane>(L, 1);
			string ret = obj.ToString();
			Push(L, ret);
			return 1;
		}
		else if(n == 1)
		{
			const int startStack = 2;
			ref var obj = ref SharpObject.GetValue<UnityEngine.Plane>(L, 1);
			Get(L, 0 + startStack, out string t0);
			string ret = obj.ToString(t0);
			Push(L, ret);
			return 1;
		}
		return 0;
	}

	[AOT.MonoPInvokeCallback(typeof(LuaNativeFunction))]
	static int Equals(IntPtr L)
	{
		const int startStack = 2;
		ref var obj = ref SharpObject.GetValue<UnityEngine.Plane>(L, 1);
		Get(L, 0 + startStack, out object t0);
		bool ret = obj.Equals(t0);
		Push(L, ret);
		return 1;
	}

	[AOT.MonoPInvokeCallback(typeof(LuaNativeFunction))]
	static int GetHashCode(IntPtr L)
	{
		ref var obj = ref SharpObject.GetValue<UnityEngine.Plane>(L, 1);
		int ret = obj.GetHashCode();
		Push(L, ret);
		return 1;
	}

	[AOT.MonoPInvokeCallback(typeof(LuaNativeFunction))]
	static int GetType(IntPtr L)
	{
		ref var obj = ref SharpObject.GetValue<UnityEngine.Plane>(L, 1);
		System.Type ret = obj.GetType();
		Push(L, ret);
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