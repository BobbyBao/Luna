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
		{
			obj = new UnityEngine.Bounds();
		}
		else if(n == 2)
		{
			obj = new UnityEngine.Bounds(
				Lua.Get<UnityEngine.Vector3>(L, 2),
				Lua.Get<UnityEngine.Vector3>(L, 3)
			);
		}
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

	[AOT.MonoPInvokeCallback(typeof(LuaNativeFunction))]
	static int GetHashCode(IntPtr L)
	{
		ref var obj = ref SharpObject.GetValue<UnityEngine.Bounds>(L, 1);
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
			ref var obj = ref SharpObject.GetValue<UnityEngine.Bounds>(L, 1);
			var ret = obj.Equals(
				Lua.Get<object>(L, 0 + startStack)
			);
			Lua.Push(L, ret);
			return 1;
		}
		else if(n == 1)
		{
			const int startStack = 2;
			ref var obj = ref SharpObject.GetValue<UnityEngine.Bounds>(L, 1);
			var ret = obj.Equals(
				Lua.Get<UnityEngine.Bounds>(L, 0 + startStack)
			);
			Lua.Push(L, ret);
			return 1;
		}
		return 0;
	}

	[AOT.MonoPInvokeCallback(typeof(LuaNativeFunction))]
	static int SetMinMax(IntPtr L)
	{
		const int startStack = 2;
		ref var obj = ref SharpObject.GetValue<UnityEngine.Bounds>(L, 1);
		obj.SetMinMax(
			Lua.Get<UnityEngine.Vector3>(L, 0 + startStack),
			Lua.Get<UnityEngine.Vector3>(L, 1 + startStack)
		);
		return 0;
	}

	[AOT.MonoPInvokeCallback(typeof(LuaNativeFunction))]
	static int Encapsulate(IntPtr L)
	{
		int n = lua_gettop(L) - 1;
		if(n == 1)
		{
			const int startStack = 2;
			ref var obj = ref SharpObject.GetValue<UnityEngine.Bounds>(L, 1);
			obj.Encapsulate(
				Lua.Get<UnityEngine.Vector3>(L, 0 + startStack)
			);
			return 0;
		}
		else if(n == 1)
		{
			const int startStack = 2;
			ref var obj = ref SharpObject.GetValue<UnityEngine.Bounds>(L, 1);
			obj.Encapsulate(
				Lua.Get<UnityEngine.Bounds>(L, 0 + startStack)
			);
			return 0;
		}
		return 0;
	}

	[AOT.MonoPInvokeCallback(typeof(LuaNativeFunction))]
	static int Expand(IntPtr L)
	{
		int n = lua_gettop(L) - 1;
		if(n == 1)
		{
			const int startStack = 2;
			ref var obj = ref SharpObject.GetValue<UnityEngine.Bounds>(L, 1);
			obj.Expand(
				Lua.Get<float>(L, 0 + startStack)
			);
			return 0;
		}
		else if(n == 1)
		{
			const int startStack = 2;
			ref var obj = ref SharpObject.GetValue<UnityEngine.Bounds>(L, 1);
			obj.Expand(
				Lua.Get<UnityEngine.Vector3>(L, 0 + startStack)
			);
			return 0;
		}
		return 0;
	}

	[AOT.MonoPInvokeCallback(typeof(LuaNativeFunction))]
	static int Intersects(IntPtr L)
	{
		const int startStack = 2;
		ref var obj = ref SharpObject.GetValue<UnityEngine.Bounds>(L, 1);
		var ret = obj.Intersects(
			Lua.Get<UnityEngine.Bounds>(L, 0 + startStack)
		);
		Lua.Push(L, ret);
		return 1;
	}

	[AOT.MonoPInvokeCallback(typeof(LuaNativeFunction))]
	static int IntersectRay(IntPtr L)
	{
		const int startStack = 2;
		ref var obj = ref SharpObject.GetValue<UnityEngine.Bounds>(L, 1);
		var ret = obj.IntersectRay(
			Lua.Get<UnityEngine.Ray>(L, 0 + startStack)
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
			ref var obj = ref SharpObject.GetValue<UnityEngine.Bounds>(L, 1);
			var ret = obj.ToString();
			Lua.Push(L, ret);
			return 1;
		}
		else if(n == 1)
		{
			const int startStack = 2;
			ref var obj = ref SharpObject.GetValue<UnityEngine.Bounds>(L, 1);
			var ret = obj.ToString(
				Lua.Get<string>(L, 0 + startStack)
			);
			Lua.Push(L, ret);
			return 1;
		}
		return 0;
	}

	[AOT.MonoPInvokeCallback(typeof(LuaNativeFunction))]
	static int Contains(IntPtr L)
	{
		const int startStack = 2;
		ref var obj = ref SharpObject.GetValue<UnityEngine.Bounds>(L, 1);
		var ret = obj.Contains(
			Lua.Get<UnityEngine.Vector3>(L, 0 + startStack)
		);
		Lua.Push(L, ret);
		return 1;
	}

	[AOT.MonoPInvokeCallback(typeof(LuaNativeFunction))]
	static int SqrDistance(IntPtr L)
	{
		const int startStack = 2;
		ref var obj = ref SharpObject.GetValue<UnityEngine.Bounds>(L, 1);
		var ret = obj.SqrDistance(
			Lua.Get<UnityEngine.Vector3>(L, 0 + startStack)
		);
		Lua.Push(L, ret);
		return 1;
	}

	[AOT.MonoPInvokeCallback(typeof(LuaNativeFunction))]
	static int ClosestPoint(IntPtr L)
	{
		const int startStack = 2;
		ref var obj = ref SharpObject.GetValue<UnityEngine.Bounds>(L, 1);
		var ret = obj.ClosestPoint(
			Lua.Get<UnityEngine.Vector3>(L, 0 + startStack)
		);
		Lua.Push(L, ret);
		return 1;
	}

	[AOT.MonoPInvokeCallback(typeof(LuaNativeFunction))]
	static int GetType(IntPtr L)
	{
		ref var obj = ref SharpObject.GetValue<UnityEngine.Bounds>(L, 1);
		var ret = obj.GetType();
		Lua.Push(L, ret);
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