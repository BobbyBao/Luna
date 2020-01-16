using System;
using SharpLuna;
using static SharpLuna.Lua;

[WrapClass(typeof(UnityEngine.RaycastHit))]
public class RaycastHitWrap
{
	[AOT.MonoPInvokeCallback(typeof(LuaNativeFunction))]
	static int Get_collider(IntPtr L)
	{
		ref var obj = ref SharpObject.GetValue<UnityEngine.RaycastHit>(L, 1);
		Lua.Push(L, obj.collider);
		return 1;
	}

	[AOT.MonoPInvokeCallback(typeof(LuaNativeFunction))]
	static int Get_point(IntPtr L)
	{
		ref var obj = ref SharpObject.GetValue<UnityEngine.RaycastHit>(L, 1);
		Lua.Push(L, obj.point);
		return 1;
	}

	[AOT.MonoPInvokeCallback(typeof(LuaNativeFunction))]
	static int Set_point(IntPtr L)
	{
		ref var obj = ref SharpObject.GetValue<UnityEngine.RaycastHit>(L, 1);
		var p1 = Lua.Get<UnityEngine.Vector3>(L, 2);
		obj.point = p1;
		return 0;
	}

	[AOT.MonoPInvokeCallback(typeof(LuaNativeFunction))]
	static int Get_normal(IntPtr L)
	{
		ref var obj = ref SharpObject.GetValue<UnityEngine.RaycastHit>(L, 1);
		Lua.Push(L, obj.normal);
		return 1;
	}

	[AOT.MonoPInvokeCallback(typeof(LuaNativeFunction))]
	static int Set_normal(IntPtr L)
	{
		ref var obj = ref SharpObject.GetValue<UnityEngine.RaycastHit>(L, 1);
		var p1 = Lua.Get<UnityEngine.Vector3>(L, 2);
		obj.normal = p1;
		return 0;
	}

	[AOT.MonoPInvokeCallback(typeof(LuaNativeFunction))]
	static int Get_barycentricCoordinate(IntPtr L)
	{
		ref var obj = ref SharpObject.GetValue<UnityEngine.RaycastHit>(L, 1);
		Lua.Push(L, obj.barycentricCoordinate);
		return 1;
	}

	[AOT.MonoPInvokeCallback(typeof(LuaNativeFunction))]
	static int Set_barycentricCoordinate(IntPtr L)
	{
		ref var obj = ref SharpObject.GetValue<UnityEngine.RaycastHit>(L, 1);
		var p1 = Lua.Get<UnityEngine.Vector3>(L, 2);
		obj.barycentricCoordinate = p1;
		return 0;
	}

	[AOT.MonoPInvokeCallback(typeof(LuaNativeFunction))]
	static int Get_distance(IntPtr L)
	{
		ref var obj = ref SharpObject.GetValue<UnityEngine.RaycastHit>(L, 1);
		Lua.Push(L, obj.distance);
		return 1;
	}

	[AOT.MonoPInvokeCallback(typeof(LuaNativeFunction))]
	static int Set_distance(IntPtr L)
	{
		ref var obj = ref SharpObject.GetValue<UnityEngine.RaycastHit>(L, 1);
		var p1 = Lua.Get<float>(L, 2);
		obj.distance = p1;
		return 0;
	}

	[AOT.MonoPInvokeCallback(typeof(LuaNativeFunction))]
	static int Get_triangleIndex(IntPtr L)
	{
		ref var obj = ref SharpObject.GetValue<UnityEngine.RaycastHit>(L, 1);
		Lua.Push(L, obj.triangleIndex);
		return 1;
	}

	[AOT.MonoPInvokeCallback(typeof(LuaNativeFunction))]
	static int Get_textureCoord(IntPtr L)
	{
		ref var obj = ref SharpObject.GetValue<UnityEngine.RaycastHit>(L, 1);
		Lua.Push(L, obj.textureCoord);
		return 1;
	}

	[AOT.MonoPInvokeCallback(typeof(LuaNativeFunction))]
	static int Get_textureCoord2(IntPtr L)
	{
		ref var obj = ref SharpObject.GetValue<UnityEngine.RaycastHit>(L, 1);
		Lua.Push(L, obj.textureCoord2);
		return 1;
	}

	[AOT.MonoPInvokeCallback(typeof(LuaNativeFunction))]
	static int Get_textureCoord1(IntPtr L)
	{
		ref var obj = ref SharpObject.GetValue<UnityEngine.RaycastHit>(L, 1);
		Lua.Push(L, obj.textureCoord1);
		return 1;
	}

	[AOT.MonoPInvokeCallback(typeof(LuaNativeFunction))]
	static int Get_transform(IntPtr L)
	{
		ref var obj = ref SharpObject.GetValue<UnityEngine.RaycastHit>(L, 1);
		Lua.Push(L, obj.transform);
		return 1;
	}

	[AOT.MonoPInvokeCallback(typeof(LuaNativeFunction))]
	static int Get_rigidbody(IntPtr L)
	{
		ref var obj = ref SharpObject.GetValue<UnityEngine.RaycastHit>(L, 1);
		Lua.Push(L, obj.rigidbody);
		return 1;
	}

	[AOT.MonoPInvokeCallback(typeof(LuaNativeFunction))]
	static int Get_lightmapCoord(IntPtr L)
	{
		ref var obj = ref SharpObject.GetValue<UnityEngine.RaycastHit>(L, 1);
		Lua.Push(L, obj.lightmapCoord);
		return 1;
	}

	public static void Register(ClassWraper classWraper)
	{
		classWraper.RegProp("collider", Get_collider);
		classWraper.RegProp("point", Get_point, Set_point);
		classWraper.RegProp("normal", Get_normal, Set_normal);
		classWraper.RegProp("barycentricCoordinate", Get_barycentricCoordinate, Set_barycentricCoordinate);
		classWraper.RegProp("distance", Get_distance, Set_distance);
		classWraper.RegProp("triangleIndex", Get_triangleIndex);
		classWraper.RegProp("textureCoord", Get_textureCoord);
		classWraper.RegProp("textureCoord2", Get_textureCoord2);
		classWraper.RegProp("textureCoord1", Get_textureCoord1);
		classWraper.RegProp("transform", Get_transform);
		classWraper.RegProp("rigidbody", Get_rigidbody);
		classWraper.RegProp("lightmapCoord", Get_lightmapCoord);
	}
}