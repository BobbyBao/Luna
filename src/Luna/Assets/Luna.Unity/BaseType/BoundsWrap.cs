using System;
using SharpLuna;
using System.Collections.Generic;
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
		{
			obj = new UnityEngine.Bounds();
		}
		else if(n == 2)
		{
			Get(L, 2, out UnityEngine.Vector3 t1);
			Get(L, 3, out UnityEngine.Vector3 t2);
			obj = new UnityEngine.Bounds(t1, t2);
		}
		Push(L, obj);
		return 1;
	}

	[AOT.MonoPInvokeCallback(typeof(LuaNativeFunction))]
	static int Get_center(IntPtr L)
	{
		ref var obj = ref SharpObject.GetValue<UnityEngine.Bounds>(L, 1);
		Push(L, obj.center);
		return 1;
	}

	[AOT.MonoPInvokeCallback(typeof(LuaNativeFunction))]
	static int Set_center(IntPtr L)
	{
		ref var obj = ref SharpObject.GetValue<UnityEngine.Bounds>(L, 1);
		Get(L, 2, out UnityEngine.Vector3 p1);
		obj.center = p1;
		return 0;
	}

	[AOT.MonoPInvokeCallback(typeof(LuaNativeFunction))]
	static int Get_size(IntPtr L)
	{
		ref var obj = ref SharpObject.GetValue<UnityEngine.Bounds>(L, 1);
		Push(L, obj.size);
		return 1;
	}

	[AOT.MonoPInvokeCallback(typeof(LuaNativeFunction))]
	static int Set_size(IntPtr L)
	{
		ref var obj = ref SharpObject.GetValue<UnityEngine.Bounds>(L, 1);
		Get(L, 2, out UnityEngine.Vector3 p1);
		obj.size = p1;
		return 0;
	}

	[AOT.MonoPInvokeCallback(typeof(LuaNativeFunction))]
	static int Get_extents(IntPtr L)
	{
		ref var obj = ref SharpObject.GetValue<UnityEngine.Bounds>(L, 1);
		Push(L, obj.extents);
		return 1;
	}

	[AOT.MonoPInvokeCallback(typeof(LuaNativeFunction))]
	static int Set_extents(IntPtr L)
	{
		ref var obj = ref SharpObject.GetValue<UnityEngine.Bounds>(L, 1);
		Get(L, 2, out UnityEngine.Vector3 p1);
		obj.extents = p1;
		return 0;
	}

	[AOT.MonoPInvokeCallback(typeof(LuaNativeFunction))]
	static int Get_min(IntPtr L)
	{
		ref var obj = ref SharpObject.GetValue<UnityEngine.Bounds>(L, 1);
		Push(L, obj.min);
		return 1;
	}

	[AOT.MonoPInvokeCallback(typeof(LuaNativeFunction))]
	static int Set_min(IntPtr L)
	{
		ref var obj = ref SharpObject.GetValue<UnityEngine.Bounds>(L, 1);
		Get(L, 2, out UnityEngine.Vector3 p1);
		obj.min = p1;
		return 0;
	}

	[AOT.MonoPInvokeCallback(typeof(LuaNativeFunction))]
	static int Get_max(IntPtr L)
	{
		ref var obj = ref SharpObject.GetValue<UnityEngine.Bounds>(L, 1);
		Push(L, obj.max);
		return 1;
	}

	[AOT.MonoPInvokeCallback(typeof(LuaNativeFunction))]
	static int Set_max(IntPtr L)
	{
		ref var obj = ref SharpObject.GetValue<UnityEngine.Bounds>(L, 1);
		Get(L, 2, out UnityEngine.Vector3 p1);
		obj.max = p1;
		return 0;
	}

	[AOT.MonoPInvokeCallback(typeof(LuaNativeFunction))]
	static int GetHashCode(IntPtr L)
	{
		ref var obj = ref SharpObject.GetValue<UnityEngine.Bounds>(L, 1);
		int ret = obj.GetHashCode();
		Push(L, ret);
		return 1;
	}

	[AOT.MonoPInvokeCallback(typeof(LuaNativeFunction))]
	static int Equals(IntPtr L)
	{
		int n = lua_gettop(L) - 1;
		if(n == 1 && CheckType<object>(L, 1))
		{
			const int startStack = 2;
			ref var obj = ref SharpObject.GetValue<UnityEngine.Bounds>(L, 1);
			Get(L, 0 + startStack, out object t0);
			bool ret = obj.Equals(t0);
			Push(L, ret);
			return 1;
		}
		else if(n == 1 && CheckType<UnityEngine.Bounds>(L, 1))
		{
			const int startStack = 2;
			ref var obj = ref SharpObject.GetValue<UnityEngine.Bounds>(L, 1);
			Get(L, 0 + startStack, out UnityEngine.Bounds t0);
			bool ret = obj.Equals(t0);
			Push(L, ret);
			return 1;
		}
		return 0;
	}

	[AOT.MonoPInvokeCallback(typeof(LuaNativeFunction))]
	static int SetMinMax(IntPtr L)
	{
		const int startStack = 2;
		ref var obj = ref SharpObject.GetValue<UnityEngine.Bounds>(L, 1);
		Get(L, 0 + startStack, out UnityEngine.Vector3 t0);
		Get(L, 1 + startStack, out UnityEngine.Vector3 t1);
		obj.SetMinMax(t0, t1);
		return 0;
	}

	[AOT.MonoPInvokeCallback(typeof(LuaNativeFunction))]
	static int Encapsulate(IntPtr L)
	{
		int n = lua_gettop(L) - 1;
		if(n == 1 && CheckType<UnityEngine.Vector3>(L, 1))
		{
			const int startStack = 2;
			ref var obj = ref SharpObject.GetValue<UnityEngine.Bounds>(L, 1);
			Get(L, 0 + startStack, out UnityEngine.Vector3 t0);
			obj.Encapsulate(t0);
			return 0;
		}
		else if(n == 1 && CheckType<UnityEngine.Bounds>(L, 1))
		{
			const int startStack = 2;
			ref var obj = ref SharpObject.GetValue<UnityEngine.Bounds>(L, 1);
			Get(L, 0 + startStack, out UnityEngine.Bounds t0);
			obj.Encapsulate(t0);
			return 0;
		}
		return 0;
	}

	[AOT.MonoPInvokeCallback(typeof(LuaNativeFunction))]
	static int Expand(IntPtr L)
	{
		int n = lua_gettop(L) - 1;
		if(n == 1 && CheckType<float>(L, 1))
		{
			const int startStack = 2;
			ref var obj = ref SharpObject.GetValue<UnityEngine.Bounds>(L, 1);
			Get(L, 0 + startStack, out float t0);
			obj.Expand(t0);
			return 0;
		}
		else if(n == 1 && CheckType<UnityEngine.Vector3>(L, 1))
		{
			const int startStack = 2;
			ref var obj = ref SharpObject.GetValue<UnityEngine.Bounds>(L, 1);
			Get(L, 0 + startStack, out UnityEngine.Vector3 t0);
			obj.Expand(t0);
			return 0;
		}
		return 0;
	}

	[AOT.MonoPInvokeCallback(typeof(LuaNativeFunction))]
	static int Intersects(IntPtr L)
	{
		const int startStack = 2;
		ref var obj = ref SharpObject.GetValue<UnityEngine.Bounds>(L, 1);
		Get(L, 0 + startStack, out UnityEngine.Bounds t0);
		bool ret = obj.Intersects(t0);
		Push(L, ret);
		return 1;
	}

	[AOT.MonoPInvokeCallback(typeof(LuaNativeFunction))]
	static int IntersectRay(IntPtr L)
	{
		const int startStack = 2;
		ref var obj = ref SharpObject.GetValue<UnityEngine.Bounds>(L, 1);
		Get(L, 0 + startStack, out UnityEngine.Ray t0);
		bool ret = obj.IntersectRay(t0);
		Push(L, ret);
		return 1;
	}

	[AOT.MonoPInvokeCallback(typeof(LuaNativeFunction))]
	static int ToString(IntPtr L)
	{
		int n = lua_gettop(L) - 1;
		if(n == 0)
		{
			ref var obj = ref SharpObject.GetValue<UnityEngine.Bounds>(L, 1);
			string ret = obj.ToString();
			Push(L, ret);
			return 1;
		}
		else if(n == 1)
		{
			const int startStack = 2;
			ref var obj = ref SharpObject.GetValue<UnityEngine.Bounds>(L, 1);
			Get(L, 0 + startStack, out string t0);
			string ret = obj.ToString(t0);
			Push(L, ret);
			return 1;
		}
		return 0;
	}

	[AOT.MonoPInvokeCallback(typeof(LuaNativeFunction))]
	static int Contains(IntPtr L)
	{
		const int startStack = 2;
		ref var obj = ref SharpObject.GetValue<UnityEngine.Bounds>(L, 1);
		Get(L, 0 + startStack, out UnityEngine.Vector3 t0);
		bool ret = obj.Contains(t0);
		Push(L, ret);
		return 1;
	}

	[AOT.MonoPInvokeCallback(typeof(LuaNativeFunction))]
	static int SqrDistance(IntPtr L)
	{
		const int startStack = 2;
		ref var obj = ref SharpObject.GetValue<UnityEngine.Bounds>(L, 1);
		Get(L, 0 + startStack, out UnityEngine.Vector3 t0);
		float ret = obj.SqrDistance(t0);
		Push(L, ret);
		return 1;
	}

	[AOT.MonoPInvokeCallback(typeof(LuaNativeFunction))]
	static int ClosestPoint(IntPtr L)
	{
		const int startStack = 2;
		ref var obj = ref SharpObject.GetValue<UnityEngine.Bounds>(L, 1);
		Get(L, 0 + startStack, out UnityEngine.Vector3 t0);
		UnityEngine.Vector3 ret = obj.ClosestPoint(t0);
		Push(L, ret);
		return 1;
	}

	[AOT.MonoPInvokeCallback(typeof(LuaNativeFunction))]
	static int GetType(IntPtr L)
	{
		ref var obj = ref SharpObject.GetValue<UnityEngine.Bounds>(L, 1);
		System.Type ret = obj.GetType();
		Push(L, ret);
		return 1;
	}

	public static void Register(ClassWraper classWraper)
	{
		classWraper.RegConstructor(Constructor);
		classWraper.RegProperty("center", Get_center, Set_center);
		classWraper.RegProperty("size", Get_size, Set_size);
		classWraper.RegProperty("extents", Get_extents, Set_extents);
		classWraper.RegProperty("min", Get_min, Set_min);
		classWraper.RegProperty("max", Get_max, Set_max);
		classWraper.RegFunction("GetHashCode", GetHashCode);
		classWraper.RegFunction("Equals", Equals);
		classWraper.RegFunction("SetMinMax", SetMinMax);
		classWraper.RegFunction("Encapsulate", Encapsulate);
		classWraper.RegFunction("Expand", Expand);
		classWraper.RegFunction("Intersects", Intersects);
		classWraper.RegFunction("IntersectRay", IntersectRay);
		classWraper.RegFunction("ToString", ToString);
		classWraper.RegFunction("Contains", Contains);
		classWraper.RegFunction("SqrDistance", SqrDistance);
		classWraper.RegFunction("ClosestPoint", ClosestPoint);
		classWraper.RegFunction("GetType", GetType);
	}
}