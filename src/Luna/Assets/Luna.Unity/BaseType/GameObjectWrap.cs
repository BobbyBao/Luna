using System;
using SharpLuna;
using static SharpLuna.Lua;

[WrapClass(typeof(UnityEngine.GameObject))]
public class GameObjectWrap
{
	[AOT.MonoPInvokeCallback(typeof(LuaNativeFunction))]
	static int Constructor(IntPtr L)
	{
		int n = lua_gettop(L) - 1;
		UnityEngine.GameObject obj = default;
		if(n == 1)
			obj = new UnityEngine.GameObject(
				Lua.Get<string>(L, 2)
			);
		else if(n == 0)
			obj = new UnityEngine.GameObject();
		else if(n == 2)
			obj = new UnityEngine.GameObject(
				Lua.Get<string>(L, 2),
				Lua.Get<System.Type[]>(L, 3)
			);
		Lua.Push(L, obj);
		return 1;
	}

	[AOT.MonoPInvokeCallback(typeof(LuaNativeFunction))]
	static int Get_transform(IntPtr L)
	{
		var obj = SharpObject.Get<UnityEngine.GameObject>(L, 1);
		Lua.Push(L, obj.transform);
		return 1;
	}

	[AOT.MonoPInvokeCallback(typeof(LuaNativeFunction))]
	static int Get_layer(IntPtr L)
	{
		var obj = SharpObject.Get<UnityEngine.GameObject>(L, 1);
		Lua.Push(L, obj.layer);
		return 1;
	}

	[AOT.MonoPInvokeCallback(typeof(LuaNativeFunction))]
	static int Set_layer(IntPtr L)
	{
		var obj = SharpObject.Get<UnityEngine.GameObject>(L, 1);
		var p1 = Lua.Get<int>(L, 2);
		obj.layer = p1;
		return 0;
	}

	[AOT.MonoPInvokeCallback(typeof(LuaNativeFunction))]
	static int Get_activeSelf(IntPtr L)
	{
		var obj = SharpObject.Get<UnityEngine.GameObject>(L, 1);
		Lua.Push(L, obj.activeSelf);
		return 1;
	}

	[AOT.MonoPInvokeCallback(typeof(LuaNativeFunction))]
	static int Get_activeInHierarchy(IntPtr L)
	{
		var obj = SharpObject.Get<UnityEngine.GameObject>(L, 1);
		Lua.Push(L, obj.activeInHierarchy);
		return 1;
	}

	[AOT.MonoPInvokeCallback(typeof(LuaNativeFunction))]
	static int Get_isStatic(IntPtr L)
	{
		var obj = SharpObject.Get<UnityEngine.GameObject>(L, 1);
		Lua.Push(L, obj.isStatic);
		return 1;
	}

	[AOT.MonoPInvokeCallback(typeof(LuaNativeFunction))]
	static int Set_isStatic(IntPtr L)
	{
		var obj = SharpObject.Get<UnityEngine.GameObject>(L, 1);
		var p1 = Lua.Get<bool>(L, 2);
		obj.isStatic = p1;
		return 0;
	}

	[AOT.MonoPInvokeCallback(typeof(LuaNativeFunction))]
	static int Get_tag(IntPtr L)
	{
		var obj = SharpObject.Get<UnityEngine.GameObject>(L, 1);
		Lua.Push(L, obj.tag);
		return 1;
	}

	[AOT.MonoPInvokeCallback(typeof(LuaNativeFunction))]
	static int Set_tag(IntPtr L)
	{
		var obj = SharpObject.Get<UnityEngine.GameObject>(L, 1);
		var p1 = Lua.Get<string>(L, 2);
		obj.tag = p1;
		return 0;
	}

	[AOT.MonoPInvokeCallback(typeof(LuaNativeFunction))]
	static int Get_scene(IntPtr L)
	{
		var obj = SharpObject.Get<UnityEngine.GameObject>(L, 1);
		Lua.Push(L, obj.scene);
		return 1;
	}

	[AOT.MonoPInvokeCallback(typeof(LuaNativeFunction))]
	static int Get_gameObject(IntPtr L)
	{
		var obj = SharpObject.Get<UnityEngine.GameObject>(L, 1);
		Lua.Push(L, obj.gameObject);
		return 1;
	}

	[AOT.MonoPInvokeCallback(typeof(LuaNativeFunction))]
	static int Get_name(IntPtr L)
	{
		var obj = SharpObject.Get<UnityEngine.GameObject>(L, 1);
		Lua.Push(L, obj.name);
		return 1;
	}

	[AOT.MonoPInvokeCallback(typeof(LuaNativeFunction))]
	static int Set_name(IntPtr L)
	{
		var obj = SharpObject.Get<UnityEngine.GameObject>(L, 1);
		var p1 = Lua.Get<string>(L, 2);
		obj.name = p1;
		return 0;
	}

	[AOT.MonoPInvokeCallback(typeof(LuaNativeFunction))]
	static int Get_hideFlags(IntPtr L)
	{
		var obj = SharpObject.Get<UnityEngine.GameObject>(L, 1);
		Lua.Push(L, obj.hideFlags);
		return 1;
	}

	[AOT.MonoPInvokeCallback(typeof(LuaNativeFunction))]
	static int Set_hideFlags(IntPtr L)
	{
		var obj = SharpObject.Get<UnityEngine.GameObject>(L, 1);
		var p1 = Lua.Get<UnityEngine.HideFlags>(L, 2);
		obj.hideFlags = p1;
		return 0;
	}

	public static void Register(ClassWraper classWraper)
	{
		classWraper.RegConstructor(Constructor);
		classWraper.RegProp("transform", Get_transform);
		classWraper.RegProp("layer", Get_layer, Set_layer);
		classWraper.RegProp("activeSelf", Get_activeSelf);
		classWraper.RegProp("activeInHierarchy", Get_activeInHierarchy);
		classWraper.RegProp("isStatic", Get_isStatic, Set_isStatic);
		classWraper.RegProp("tag", Get_tag, Set_tag);
		classWraper.RegProp("scene", Get_scene);
		classWraper.RegProp("gameObject", Get_gameObject);
		classWraper.RegProp("name", Get_name, Set_name);
		classWraper.RegProp("hideFlags", Get_hideFlags, Set_hideFlags);
	}
}