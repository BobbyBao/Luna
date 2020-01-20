using System;
using SharpLuna;
using System.Collections.Generic;
using static SharpLuna.Lua;

[WrapClass(typeof(UnityEngine.RaycastHit))]
public class RaycastHitWrap
{
	[AOT.MonoPInvokeCallback(typeof(LuaNativeFunction))]
	static int Get_collider(IntPtr L)
	{
		ref var obj = ref SharpObject.GetValue<UnityEngine.RaycastHit>(L, 1);
		Push(L, obj.collider);
		return 1;
	}

	[AOT.MonoPInvokeCallback(typeof(LuaNativeFunction))]
	static int Get_point(IntPtr L)
	{
		ref var obj = ref SharpObject.GetValue<UnityEngine.RaycastHit>(L, 1);
		Push(L, obj.point);
		return 1;
	}

	[AOT.MonoPInvokeCallback(typeof(LuaNativeFunction))]
	static int Set_point(IntPtr L)
	{
		ref var obj = ref SharpObject.GetValue<UnityEngine.RaycastHit>(L, 1);
		Get(L, 2, out UnityEngine.Vector3 p1);
		obj.point = p1;
		return 0;
	}

	[AOT.MonoPInvokeCallback(typeof(LuaNativeFunction))]
	static int Get_normal(IntPtr L)
	{
		ref var obj = ref SharpObject.GetValue<UnityEngine.RaycastHit>(L, 1);
		Push(L, obj.normal);
		return 1;
	}

	[AOT.MonoPInvokeCallback(typeof(LuaNativeFunction))]
	static int Set_normal(IntPtr L)
	{
		ref var obj = ref SharpObject.GetValue<UnityEngine.RaycastHit>(L, 1);
		Get(L, 2, out UnityEngine.Vector3 p1);
		obj.normal = p1;
		return 0;
	}

	[AOT.MonoPInvokeCallback(typeof(LuaNativeFunction))]
	static int Get_barycentricCoordinate(IntPtr L)
	{
		ref var obj = ref SharpObject.GetValue<UnityEngine.RaycastHit>(L, 1);
		Push(L, obj.barycentricCoordinate);
		return 1;
	}

	[AOT.MonoPInvokeCallback(typeof(LuaNativeFunction))]
	static int Set_barycentricCoordinate(IntPtr L)
	{
		ref var obj = ref SharpObject.GetValue<UnityEngine.RaycastHit>(L, 1);
		Get(L, 2, out UnityEngine.Vector3 p1);
		obj.barycentricCoordinate = p1;
		return 0;
	}

	[AOT.MonoPInvokeCallback(typeof(LuaNativeFunction))]
	static int Get_distance(IntPtr L)
	{
		ref var obj = ref SharpObject.GetValue<UnityEngine.RaycastHit>(L, 1);
		Push(L, obj.distance);
		return 1;
	}

	[AOT.MonoPInvokeCallback(typeof(LuaNativeFunction))]
	static int Set_distance(IntPtr L)
	{
		ref var obj = ref SharpObject.GetValue<UnityEngine.RaycastHit>(L, 1);
		Get(L, 2, out float p1);
		obj.distance = p1;
		return 0;
	}

	[AOT.MonoPInvokeCallback(typeof(LuaNativeFunction))]
	static int Get_triangleIndex(IntPtr L)
	{
		ref var obj = ref SharpObject.GetValue<UnityEngine.RaycastHit>(L, 1);
		Push(L, obj.triangleIndex);
		return 1;
	}

	[AOT.MonoPInvokeCallback(typeof(LuaNativeFunction))]
	static int Get_textureCoord(IntPtr L)
	{
		ref var obj = ref SharpObject.GetValue<UnityEngine.RaycastHit>(L, 1);
		Push(L, obj.textureCoord);
		return 1;
	}

	[AOT.MonoPInvokeCallback(typeof(LuaNativeFunction))]
	static int Get_textureCoord2(IntPtr L)
	{
		ref var obj = ref SharpObject.GetValue<UnityEngine.RaycastHit>(L, 1);
		Push(L, obj.textureCoord2);
		return 1;
	}

	[AOT.MonoPInvokeCallback(typeof(LuaNativeFunction))]
	static int Get_transform(IntPtr L)
	{
		ref var obj = ref SharpObject.GetValue<UnityEngine.RaycastHit>(L, 1);
		Push(L, obj.transform);
		return 1;
	}

	[AOT.MonoPInvokeCallback(typeof(LuaNativeFunction))]
	static int Get_rigidbody(IntPtr L)
	{
		ref var obj = ref SharpObject.GetValue<UnityEngine.RaycastHit>(L, 1);
		Push(L, obj.rigidbody);
		return 1;
	}

	[AOT.MonoPInvokeCallback(typeof(LuaNativeFunction))]
	static int Get_lightmapCoord(IntPtr L)
	{
		ref var obj = ref SharpObject.GetValue<UnityEngine.RaycastHit>(L, 1);
		Push(L, obj.lightmapCoord);
		return 1;
	}

	[AOT.MonoPInvokeCallback(typeof(LuaNativeFunction))]
	static int Equals(IntPtr L)
	{
		const int startStack = 2;
		ref var obj = ref SharpObject.GetValue<UnityEngine.RaycastHit>(L, 1);
		Get(L, 0 + startStack, out object t0);
		bool ret = obj.Equals(t0);
		Push(L, ret);
		return 1;
	}

	[AOT.MonoPInvokeCallback(typeof(LuaNativeFunction))]
	static int GetHashCode(IntPtr L)
	{
		ref var obj = ref SharpObject.GetValue<UnityEngine.RaycastHit>(L, 1);
		int ret = obj.GetHashCode();
		Push(L, ret);
		return 1;
	}

	[AOT.MonoPInvokeCallback(typeof(LuaNativeFunction))]
	static int ToString(IntPtr L)
	{
		ref var obj = ref SharpObject.GetValue<UnityEngine.RaycastHit>(L, 1);
		string ret = obj.ToString();
		Push(L, ret);
		return 1;
	}

	[AOT.MonoPInvokeCallback(typeof(LuaNativeFunction))]
	static int GetType(IntPtr L)
	{
		ref var obj = ref SharpObject.GetValue<UnityEngine.RaycastHit>(L, 1);
		System.Type ret = obj.GetType();
		Push(L, ret);
		return 1;
	}

	public static void Register(ClassWraper classWraper)
	{
		classWraper.RegProperty("collider", Get_collider);
		classWraper.RegProperty("point", Get_point, Set_point);
		classWraper.RegProperty("normal", Get_normal, Set_normal);
		classWraper.RegProperty("barycentricCoordinate", Get_barycentricCoordinate, Set_barycentricCoordinate);
		classWraper.RegProperty("distance", Get_distance, Set_distance);
		classWraper.RegProperty("triangleIndex", Get_triangleIndex);
		classWraper.RegProperty("textureCoord", Get_textureCoord);
		classWraper.RegProperty("textureCoord2", Get_textureCoord2);
		classWraper.RegProperty("transform", Get_transform);
		classWraper.RegProperty("rigidbody", Get_rigidbody);
		classWraper.RegProperty("lightmapCoord", Get_lightmapCoord);
		classWraper.RegFunction("Equals", Equals);
		classWraper.RegFunction("GetHashCode", GetHashCode);
		classWraper.RegFunction("ToString", ToString);
		classWraper.RegFunction("GetType", GetType);
	}
}