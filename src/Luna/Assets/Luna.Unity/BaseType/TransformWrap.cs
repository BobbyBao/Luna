using System;
using SharpLuna;
using static SharpLuna.Lua;

[WrapClass(typeof(UnityEngine.Transform))]
public class TransformWrap
{
	[AOT.MonoPInvokeCallback(typeof(LuaNativeFunction))]
	static int Get_position(IntPtr L)
	{
		var obj = SharpObject.Get<UnityEngine.Transform>(L, 1);
		Lua.Push(L, obj.position);
		return 1;
	}

	[AOT.MonoPInvokeCallback(typeof(LuaNativeFunction))]
	static int Set_position(IntPtr L)
	{
		var obj = SharpObject.Get<UnityEngine.Transform>(L, 1);
		var p1 = Lua.Get<UnityEngine.Vector3>(L, 2);
		obj.position = p1;
		return 0;
	}

	[AOT.MonoPInvokeCallback(typeof(LuaNativeFunction))]
	static int Get_localPosition(IntPtr L)
	{
		var obj = SharpObject.Get<UnityEngine.Transform>(L, 1);
		Lua.Push(L, obj.localPosition);
		return 1;
	}

	[AOT.MonoPInvokeCallback(typeof(LuaNativeFunction))]
	static int Set_localPosition(IntPtr L)
	{
		var obj = SharpObject.Get<UnityEngine.Transform>(L, 1);
		var p1 = Lua.Get<UnityEngine.Vector3>(L, 2);
		obj.localPosition = p1;
		return 0;
	}

	[AOT.MonoPInvokeCallback(typeof(LuaNativeFunction))]
	static int Get_eulerAngles(IntPtr L)
	{
		var obj = SharpObject.Get<UnityEngine.Transform>(L, 1);
		Lua.Push(L, obj.eulerAngles);
		return 1;
	}

	[AOT.MonoPInvokeCallback(typeof(LuaNativeFunction))]
	static int Set_eulerAngles(IntPtr L)
	{
		var obj = SharpObject.Get<UnityEngine.Transform>(L, 1);
		var p1 = Lua.Get<UnityEngine.Vector3>(L, 2);
		obj.eulerAngles = p1;
		return 0;
	}

	[AOT.MonoPInvokeCallback(typeof(LuaNativeFunction))]
	static int Get_localEulerAngles(IntPtr L)
	{
		var obj = SharpObject.Get<UnityEngine.Transform>(L, 1);
		Lua.Push(L, obj.localEulerAngles);
		return 1;
	}

	[AOT.MonoPInvokeCallback(typeof(LuaNativeFunction))]
	static int Set_localEulerAngles(IntPtr L)
	{
		var obj = SharpObject.Get<UnityEngine.Transform>(L, 1);
		var p1 = Lua.Get<UnityEngine.Vector3>(L, 2);
		obj.localEulerAngles = p1;
		return 0;
	}

	[AOT.MonoPInvokeCallback(typeof(LuaNativeFunction))]
	static int Get_right(IntPtr L)
	{
		var obj = SharpObject.Get<UnityEngine.Transform>(L, 1);
		Lua.Push(L, obj.right);
		return 1;
	}

	[AOT.MonoPInvokeCallback(typeof(LuaNativeFunction))]
	static int Set_right(IntPtr L)
	{
		var obj = SharpObject.Get<UnityEngine.Transform>(L, 1);
		var p1 = Lua.Get<UnityEngine.Vector3>(L, 2);
		obj.right = p1;
		return 0;
	}

	[AOT.MonoPInvokeCallback(typeof(LuaNativeFunction))]
	static int Get_up(IntPtr L)
	{
		var obj = SharpObject.Get<UnityEngine.Transform>(L, 1);
		Lua.Push(L, obj.up);
		return 1;
	}

	[AOT.MonoPInvokeCallback(typeof(LuaNativeFunction))]
	static int Set_up(IntPtr L)
	{
		var obj = SharpObject.Get<UnityEngine.Transform>(L, 1);
		var p1 = Lua.Get<UnityEngine.Vector3>(L, 2);
		obj.up = p1;
		return 0;
	}

	[AOT.MonoPInvokeCallback(typeof(LuaNativeFunction))]
	static int Get_forward(IntPtr L)
	{
		var obj = SharpObject.Get<UnityEngine.Transform>(L, 1);
		Lua.Push(L, obj.forward);
		return 1;
	}

	[AOT.MonoPInvokeCallback(typeof(LuaNativeFunction))]
	static int Set_forward(IntPtr L)
	{
		var obj = SharpObject.Get<UnityEngine.Transform>(L, 1);
		var p1 = Lua.Get<UnityEngine.Vector3>(L, 2);
		obj.forward = p1;
		return 0;
	}

	[AOT.MonoPInvokeCallback(typeof(LuaNativeFunction))]
	static int Get_rotation(IntPtr L)
	{
		var obj = SharpObject.Get<UnityEngine.Transform>(L, 1);
		Lua.Push(L, obj.rotation);
		return 1;
	}

	[AOT.MonoPInvokeCallback(typeof(LuaNativeFunction))]
	static int Set_rotation(IntPtr L)
	{
		var obj = SharpObject.Get<UnityEngine.Transform>(L, 1);
		var p1 = Lua.Get<UnityEngine.Quaternion>(L, 2);
		obj.rotation = p1;
		return 0;
	}

	[AOT.MonoPInvokeCallback(typeof(LuaNativeFunction))]
	static int Get_localRotation(IntPtr L)
	{
		var obj = SharpObject.Get<UnityEngine.Transform>(L, 1);
		Lua.Push(L, obj.localRotation);
		return 1;
	}

	[AOT.MonoPInvokeCallback(typeof(LuaNativeFunction))]
	static int Set_localRotation(IntPtr L)
	{
		var obj = SharpObject.Get<UnityEngine.Transform>(L, 1);
		var p1 = Lua.Get<UnityEngine.Quaternion>(L, 2);
		obj.localRotation = p1;
		return 0;
	}

	[AOT.MonoPInvokeCallback(typeof(LuaNativeFunction))]
	static int Get_localScale(IntPtr L)
	{
		var obj = SharpObject.Get<UnityEngine.Transform>(L, 1);
		Lua.Push(L, obj.localScale);
		return 1;
	}

	[AOT.MonoPInvokeCallback(typeof(LuaNativeFunction))]
	static int Set_localScale(IntPtr L)
	{
		var obj = SharpObject.Get<UnityEngine.Transform>(L, 1);
		var p1 = Lua.Get<UnityEngine.Vector3>(L, 2);
		obj.localScale = p1;
		return 0;
	}

	[AOT.MonoPInvokeCallback(typeof(LuaNativeFunction))]
	static int Get_parent(IntPtr L)
	{
		var obj = SharpObject.Get<UnityEngine.Transform>(L, 1);
		Lua.Push(L, obj.parent);
		return 1;
	}

	[AOT.MonoPInvokeCallback(typeof(LuaNativeFunction))]
	static int Set_parent(IntPtr L)
	{
		var obj = SharpObject.Get<UnityEngine.Transform>(L, 1);
		var p1 = Lua.Get<UnityEngine.Transform>(L, 2);
		obj.parent = p1;
		return 0;
	}

	[AOT.MonoPInvokeCallback(typeof(LuaNativeFunction))]
	static int Get_worldToLocalMatrix(IntPtr L)
	{
		var obj = SharpObject.Get<UnityEngine.Transform>(L, 1);
		Lua.Push(L, obj.worldToLocalMatrix);
		return 1;
	}

	[AOT.MonoPInvokeCallback(typeof(LuaNativeFunction))]
	static int Get_localToWorldMatrix(IntPtr L)
	{
		var obj = SharpObject.Get<UnityEngine.Transform>(L, 1);
		Lua.Push(L, obj.localToWorldMatrix);
		return 1;
	}

	[AOT.MonoPInvokeCallback(typeof(LuaNativeFunction))]
	static int Get_root(IntPtr L)
	{
		var obj = SharpObject.Get<UnityEngine.Transform>(L, 1);
		Lua.Push(L, obj.root);
		return 1;
	}

	[AOT.MonoPInvokeCallback(typeof(LuaNativeFunction))]
	static int Get_childCount(IntPtr L)
	{
		var obj = SharpObject.Get<UnityEngine.Transform>(L, 1);
		Lua.Push(L, obj.childCount);
		return 1;
	}

	[AOT.MonoPInvokeCallback(typeof(LuaNativeFunction))]
	static int Get_lossyScale(IntPtr L)
	{
		var obj = SharpObject.Get<UnityEngine.Transform>(L, 1);
		Lua.Push(L, obj.lossyScale);
		return 1;
	}

	[AOT.MonoPInvokeCallback(typeof(LuaNativeFunction))]
	static int Get_hasChanged(IntPtr L)
	{
		var obj = SharpObject.Get<UnityEngine.Transform>(L, 1);
		Lua.Push(L, obj.hasChanged);
		return 1;
	}

	[AOT.MonoPInvokeCallback(typeof(LuaNativeFunction))]
	static int Set_hasChanged(IntPtr L)
	{
		var obj = SharpObject.Get<UnityEngine.Transform>(L, 1);
		var p1 = Lua.Get<bool>(L, 2);
		obj.hasChanged = p1;
		return 0;
	}

	[AOT.MonoPInvokeCallback(typeof(LuaNativeFunction))]
	static int Get_hierarchyCapacity(IntPtr L)
	{
		var obj = SharpObject.Get<UnityEngine.Transform>(L, 1);
		Lua.Push(L, obj.hierarchyCapacity);
		return 1;
	}

	[AOT.MonoPInvokeCallback(typeof(LuaNativeFunction))]
	static int Set_hierarchyCapacity(IntPtr L)
	{
		var obj = SharpObject.Get<UnityEngine.Transform>(L, 1);
		var p1 = Lua.Get<int>(L, 2);
		obj.hierarchyCapacity = p1;
		return 0;
	}

	[AOT.MonoPInvokeCallback(typeof(LuaNativeFunction))]
	static int Get_hierarchyCount(IntPtr L)
	{
		var obj = SharpObject.Get<UnityEngine.Transform>(L, 1);
		Lua.Push(L, obj.hierarchyCount);
		return 1;
	}

	[AOT.MonoPInvokeCallback(typeof(LuaNativeFunction))]
	static int Get_transform(IntPtr L)
	{
		var obj = SharpObject.Get<UnityEngine.Transform>(L, 1);
		Lua.Push(L, obj.transform);
		return 1;
	}

	[AOT.MonoPInvokeCallback(typeof(LuaNativeFunction))]
	static int Get_gameObject(IntPtr L)
	{
		var obj = SharpObject.Get<UnityEngine.Transform>(L, 1);
		Lua.Push(L, obj.gameObject);
		return 1;
	}

	[AOT.MonoPInvokeCallback(typeof(LuaNativeFunction))]
	static int Get_tag(IntPtr L)
	{
		var obj = SharpObject.Get<UnityEngine.Transform>(L, 1);
		Lua.Push(L, obj.tag);
		return 1;
	}

	[AOT.MonoPInvokeCallback(typeof(LuaNativeFunction))]
	static int Set_tag(IntPtr L)
	{
		var obj = SharpObject.Get<UnityEngine.Transform>(L, 1);
		var p1 = Lua.Get<string>(L, 2);
		obj.tag = p1;
		return 0;
	}

	[AOT.MonoPInvokeCallback(typeof(LuaNativeFunction))]
	static int Get_name(IntPtr L)
	{
		var obj = SharpObject.Get<UnityEngine.Transform>(L, 1);
		Lua.Push(L, obj.name);
		return 1;
	}

	[AOT.MonoPInvokeCallback(typeof(LuaNativeFunction))]
	static int Set_name(IntPtr L)
	{
		var obj = SharpObject.Get<UnityEngine.Transform>(L, 1);
		var p1 = Lua.Get<string>(L, 2);
		obj.name = p1;
		return 0;
	}

	[AOT.MonoPInvokeCallback(typeof(LuaNativeFunction))]
	static int Get_hideFlags(IntPtr L)
	{
		var obj = SharpObject.Get<UnityEngine.Transform>(L, 1);
		Lua.Push(L, obj.hideFlags);
		return 1;
	}

	[AOT.MonoPInvokeCallback(typeof(LuaNativeFunction))]
	static int Set_hideFlags(IntPtr L)
	{
		var obj = SharpObject.Get<UnityEngine.Transform>(L, 1);
		var p1 = Lua.Get<UnityEngine.HideFlags>(L, 2);
		obj.hideFlags = p1;
		return 0;
	}

	public static void Register(ClassWraper classWraper)
	{
		classWraper.RegProp("position", Get_position, Set_position);
		classWraper.RegProp("localPosition", Get_localPosition, Set_localPosition);
		classWraper.RegProp("eulerAngles", Get_eulerAngles, Set_eulerAngles);
		classWraper.RegProp("localEulerAngles", Get_localEulerAngles, Set_localEulerAngles);
		classWraper.RegProp("right", Get_right, Set_right);
		classWraper.RegProp("up", Get_up, Set_up);
		classWraper.RegProp("forward", Get_forward, Set_forward);
		classWraper.RegProp("rotation", Get_rotation, Set_rotation);
		classWraper.RegProp("localRotation", Get_localRotation, Set_localRotation);
		classWraper.RegProp("localScale", Get_localScale, Set_localScale);
		classWraper.RegProp("parent", Get_parent, Set_parent);
		classWraper.RegProp("worldToLocalMatrix", Get_worldToLocalMatrix);
		classWraper.RegProp("localToWorldMatrix", Get_localToWorldMatrix);
		classWraper.RegProp("root", Get_root);
		classWraper.RegProp("childCount", Get_childCount);
		classWraper.RegProp("lossyScale", Get_lossyScale);
		classWraper.RegProp("hasChanged", Get_hasChanged, Set_hasChanged);
		classWraper.RegProp("hierarchyCapacity", Get_hierarchyCapacity, Set_hierarchyCapacity);
		classWraper.RegProp("hierarchyCount", Get_hierarchyCount);
		classWraper.RegProp("transform", Get_transform);
		classWraper.RegProp("gameObject", Get_gameObject);
		classWraper.RegProp("tag", Get_tag, Set_tag);
		classWraper.RegProp("name", Get_name, Set_name);
		classWraper.RegProp("hideFlags", Get_hideFlags, Set_hideFlags);
	}
}