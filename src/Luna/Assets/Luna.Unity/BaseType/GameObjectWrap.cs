using System;
using SharpLuna;
using System.Collections.Generic;
using static SharpLuna.Lua;

[WrapClass(typeof(UnityEngine.GameObject))]
public class GameObjectWrap
{
	[AOT.MonoPInvokeCallback(typeof(LuaNativeFunction))]
	static int Constructor(IntPtr L)
	{
		int n = lua_gettop(L) - 1;
		UnityEngine.GameObject obj = default;
		if(n == 0)
		{
			obj = new UnityEngine.GameObject();
		}
		else if(n == 1)
		{
			Get(L, 2, out string t1);
			obj = new UnityEngine.GameObject(t1);
		}
		else if(n == 2)
		{
			Get(L, 2, out string t1);
			Get(L, 3, out System.Type[] t2);
			obj = new UnityEngine.GameObject(t1, t2);
		}
		Push(L, obj);
		return 1;
	}

	[AOT.MonoPInvokeCallback(typeof(LuaNativeFunction))]
	static int Get_transform(IntPtr L)
	{
		var obj = SharpObject.Get<UnityEngine.GameObject>(L, 1);
		Push(L, obj.transform);
		return 1;
	}

	[AOT.MonoPInvokeCallback(typeof(LuaNativeFunction))]
	static int Get_layer(IntPtr L)
	{
		var obj = SharpObject.Get<UnityEngine.GameObject>(L, 1);
		Push(L, obj.layer);
		return 1;
	}

	[AOT.MonoPInvokeCallback(typeof(LuaNativeFunction))]
	static int Set_layer(IntPtr L)
	{
		var obj = SharpObject.Get<UnityEngine.GameObject>(L, 1);
		Get(L, 2, out int p1);
		obj.layer = p1;
		return 0;
	}

	[AOT.MonoPInvokeCallback(typeof(LuaNativeFunction))]
	static int Get_activeSelf(IntPtr L)
	{
		var obj = SharpObject.Get<UnityEngine.GameObject>(L, 1);
		Push(L, obj.activeSelf);
		return 1;
	}

	[AOT.MonoPInvokeCallback(typeof(LuaNativeFunction))]
	static int Get_activeInHierarchy(IntPtr L)
	{
		var obj = SharpObject.Get<UnityEngine.GameObject>(L, 1);
		Push(L, obj.activeInHierarchy);
		return 1;
	}

	[AOT.MonoPInvokeCallback(typeof(LuaNativeFunction))]
	static int Get_isStatic(IntPtr L)
	{
		var obj = SharpObject.Get<UnityEngine.GameObject>(L, 1);
		Push(L, obj.isStatic);
		return 1;
	}

	[AOT.MonoPInvokeCallback(typeof(LuaNativeFunction))]
	static int Set_isStatic(IntPtr L)
	{
		var obj = SharpObject.Get<UnityEngine.GameObject>(L, 1);
		Get(L, 2, out bool p1);
		obj.isStatic = p1;
		return 0;
	}

	[AOT.MonoPInvokeCallback(typeof(LuaNativeFunction))]
	static int Get_tag(IntPtr L)
	{
		var obj = SharpObject.Get<UnityEngine.GameObject>(L, 1);
		Push(L, obj.tag);
		return 1;
	}

	[AOT.MonoPInvokeCallback(typeof(LuaNativeFunction))]
	static int Set_tag(IntPtr L)
	{
		var obj = SharpObject.Get<UnityEngine.GameObject>(L, 1);
		Get(L, 2, out string p1);
		obj.tag = p1;
		return 0;
	}

	[AOT.MonoPInvokeCallback(typeof(LuaNativeFunction))]
	static int Get_scene(IntPtr L)
	{
		var obj = SharpObject.Get<UnityEngine.GameObject>(L, 1);
		Push(L, obj.scene);
		return 1;
	}

	[AOT.MonoPInvokeCallback(typeof(LuaNativeFunction))]
	static int Get_gameObject(IntPtr L)
	{
		var obj = SharpObject.Get<UnityEngine.GameObject>(L, 1);
		Push(L, obj.gameObject);
		return 1;
	}

	[AOT.MonoPInvokeCallback(typeof(LuaNativeFunction))]
	static int Get_name(IntPtr L)
	{
		var obj = SharpObject.Get<UnityEngine.GameObject>(L, 1);
		Push(L, obj.name);
		return 1;
	}

	[AOT.MonoPInvokeCallback(typeof(LuaNativeFunction))]
	static int Set_name(IntPtr L)
	{
		var obj = SharpObject.Get<UnityEngine.GameObject>(L, 1);
		Get(L, 2, out string p1);
		obj.name = p1;
		return 0;
	}

	[AOT.MonoPInvokeCallback(typeof(LuaNativeFunction))]
	static int Get_hideFlags(IntPtr L)
	{
		var obj = SharpObject.Get<UnityEngine.GameObject>(L, 1);
		Push(L, obj.hideFlags);
		return 1;
	}

	[AOT.MonoPInvokeCallback(typeof(LuaNativeFunction))]
	static int Set_hideFlags(IntPtr L)
	{
		var obj = SharpObject.Get<UnityEngine.GameObject>(L, 1);
		Get(L, 2, out UnityEngine.HideFlags p1);
		obj.hideFlags = p1;
		return 0;
	}

	[AOT.MonoPInvokeCallback(typeof(LuaNativeFunction))]
	static int CreatePrimitive(IntPtr L)
	{
		#if LUNA_SCRIPT
		const int startStack = 2;
		#else
		const int startStack = 1;
		#endif
		Get(L, 0 + startStack, out UnityEngine.PrimitiveType t0);
		UnityEngine.GameObject ret = UnityEngine.GameObject.CreatePrimitive(t0);
		Push(L, ret);
		return 1;
	}

	[AOT.MonoPInvokeCallback(typeof(LuaNativeFunction))]
	static int GetComponent(IntPtr L)
	{
		int n = lua_gettop(L) - 1;
		if(n == 1 && CheckType<System.Type>(L, 1))
		{
			const int startStack = 2;
			var obj = SharpObject.Get<UnityEngine.GameObject>(L, 1);
			Get(L, 0 + startStack, out System.Type t0);
			UnityEngine.Component ret = obj.GetComponent(t0);
			Push(L, ret);
			return 1;
		}
		else if(n == 1 && CheckType<string>(L, 1))
		{
			const int startStack = 2;
			var obj = SharpObject.Get<UnityEngine.GameObject>(L, 1);
			Get(L, 0 + startStack, out string t0);
			UnityEngine.Component ret = obj.GetComponent(t0);
			Push(L, ret);
			return 1;
		}
		return 0;
	}

	[AOT.MonoPInvokeCallback(typeof(LuaNativeFunction))]
	static int GetComponentInChildren(IntPtr L)
	{
		int n = lua_gettop(L) - 1;
		if(n == 1 && CheckType<System.Type>(L, 1))
		{
			const int startStack = 2;
			var obj = SharpObject.Get<UnityEngine.GameObject>(L, 1);
			Get(L, 0 + startStack, out System.Type t0);
			UnityEngine.Component ret = obj.GetComponentInChildren(t0);
			Push(L, ret);
			return 1;
		}
		else if(n == 2 && CheckType<System.Type, bool>(L, 1))
		{
			const int startStack = 2;
			var obj = SharpObject.Get<UnityEngine.GameObject>(L, 1);
			Get(L, 0 + startStack, out System.Type t0);
			Get(L, 1 + startStack, out bool t1);
			UnityEngine.Component ret = obj.GetComponentInChildren(t0, t1);
			Push(L, ret);
			return 1;
		}
		return 0;
	}

	[AOT.MonoPInvokeCallback(typeof(LuaNativeFunction))]
	static int GetComponentInParent(IntPtr L)
	{
		const int startStack = 2;
		var obj = SharpObject.Get<UnityEngine.GameObject>(L, 1);
		Get(L, 0 + startStack, out System.Type t0);
		UnityEngine.Component ret = obj.GetComponentInParent(t0);
		Push(L, ret);
		return 1;
	}

	[AOT.MonoPInvokeCallback(typeof(LuaNativeFunction))]
	static int GetComponents(IntPtr L)
	{
		const int startStack = 2;
		var obj = SharpObject.Get<UnityEngine.GameObject>(L, 1);
		Get(L, 0 + startStack, out System.Type t0);
		UnityEngine.Component[] ret = obj.GetComponents(t0);
		Push(L, ret);
		return 1;
	}

	[AOT.MonoPInvokeCallback(typeof(LuaNativeFunction))]
	static int GetComponentsInChildren(IntPtr L)
	{
		int n = lua_gettop(L) - 1;
		if(n == 1 && CheckType<System.Type>(L, 1))
		{
			const int startStack = 2;
			var obj = SharpObject.Get<UnityEngine.GameObject>(L, 1);
			Get(L, 0 + startStack, out System.Type t0);
			UnityEngine.Component[] ret = obj.GetComponentsInChildren(t0);
			Push(L, ret);
			return 1;
		}
		else if(n == 2 && CheckType<System.Type, bool>(L, 1))
		{
			const int startStack = 2;
			var obj = SharpObject.Get<UnityEngine.GameObject>(L, 1);
			Get(L, 0 + startStack, out System.Type t0);
			Get(L, 1 + startStack, out bool t1);
			UnityEngine.Component[] ret = obj.GetComponentsInChildren(t0, t1);
			Push(L, ret);
			return 1;
		}
		return 0;
	}

	[AOT.MonoPInvokeCallback(typeof(LuaNativeFunction))]
	static int GetComponentsInParent(IntPtr L)
	{
		int n = lua_gettop(L) - 1;
		if(n == 1 && CheckType<System.Type>(L, 1))
		{
			const int startStack = 2;
			var obj = SharpObject.Get<UnityEngine.GameObject>(L, 1);
			Get(L, 0 + startStack, out System.Type t0);
			UnityEngine.Component[] ret = obj.GetComponentsInParent(t0);
			Push(L, ret);
			return 1;
		}
		else if(n == 2 && CheckType<System.Type, bool>(L, 1))
		{
			const int startStack = 2;
			var obj = SharpObject.Get<UnityEngine.GameObject>(L, 1);
			Get(L, 0 + startStack, out System.Type t0);
			Get(L, 1 + startStack, out bool t1);
			UnityEngine.Component[] ret = obj.GetComponentsInParent(t0, t1);
			Push(L, ret);
			return 1;
		}
		return 0;
	}

	[AOT.MonoPInvokeCallback(typeof(LuaNativeFunction))]
	static int FindWithTag(IntPtr L)
	{
		#if LUNA_SCRIPT
		const int startStack = 2;
		#else
		const int startStack = 1;
		#endif
		Get(L, 0 + startStack, out string t0);
		UnityEngine.GameObject ret = UnityEngine.GameObject.FindWithTag(t0);
		Push(L, ret);
		return 1;
	}

	[AOT.MonoPInvokeCallback(typeof(LuaNativeFunction))]
	static int SendMessageUpwards(IntPtr L)
	{
		int n = lua_gettop(L) - 1;
		if(n == 1 && CheckType<string>(L, 1))
		{
			const int startStack = 2;
			var obj = SharpObject.Get<UnityEngine.GameObject>(L, 1);
			Get(L, 0 + startStack, out string t0);
			obj.SendMessageUpwards(t0);
			return 0;
		}
		else if(n == 2 && CheckType<string, UnityEngine.SendMessageOptions>(L, 1))
		{
			const int startStack = 2;
			var obj = SharpObject.Get<UnityEngine.GameObject>(L, 1);
			Get(L, 0 + startStack, out string t0);
			Get(L, 1 + startStack, out UnityEngine.SendMessageOptions t1);
			obj.SendMessageUpwards(t0, t1);
			return 0;
		}
		else if(n == 2 && CheckType<string, object>(L, 1))
		{
			const int startStack = 2;
			var obj = SharpObject.Get<UnityEngine.GameObject>(L, 1);
			Get(L, 0 + startStack, out string t0);
			Get(L, 1 + startStack, out object t1);
			obj.SendMessageUpwards(t0, t1);
			return 0;
		}
		else if(n == 3 && CheckType<string, object, UnityEngine.SendMessageOptions>(L, 1))
		{
			const int startStack = 2;
			var obj = SharpObject.Get<UnityEngine.GameObject>(L, 1);
			Get(L, 0 + startStack, out string t0);
			Get(L, 1 + startStack, out object t1);
			Get(L, 2 + startStack, out UnityEngine.SendMessageOptions t2);
			obj.SendMessageUpwards(t0, t1, t2);
			return 0;
		}
		return 0;
	}

	[AOT.MonoPInvokeCallback(typeof(LuaNativeFunction))]
	static int SendMessage(IntPtr L)
	{
		int n = lua_gettop(L) - 1;
		if(n == 1 && CheckType<string>(L, 1))
		{
			const int startStack = 2;
			var obj = SharpObject.Get<UnityEngine.GameObject>(L, 1);
			Get(L, 0 + startStack, out string t0);
			obj.SendMessage(t0);
			return 0;
		}
		else if(n == 2 && CheckType<string, UnityEngine.SendMessageOptions>(L, 1))
		{
			const int startStack = 2;
			var obj = SharpObject.Get<UnityEngine.GameObject>(L, 1);
			Get(L, 0 + startStack, out string t0);
			Get(L, 1 + startStack, out UnityEngine.SendMessageOptions t1);
			obj.SendMessage(t0, t1);
			return 0;
		}
		else if(n == 2 && CheckType<string, object>(L, 1))
		{
			const int startStack = 2;
			var obj = SharpObject.Get<UnityEngine.GameObject>(L, 1);
			Get(L, 0 + startStack, out string t0);
			Get(L, 1 + startStack, out object t1);
			obj.SendMessage(t0, t1);
			return 0;
		}
		else if(n == 3 && CheckType<string, object, UnityEngine.SendMessageOptions>(L, 1))
		{
			const int startStack = 2;
			var obj = SharpObject.Get<UnityEngine.GameObject>(L, 1);
			Get(L, 0 + startStack, out string t0);
			Get(L, 1 + startStack, out object t1);
			Get(L, 2 + startStack, out UnityEngine.SendMessageOptions t2);
			obj.SendMessage(t0, t1, t2);
			return 0;
		}
		return 0;
	}

	[AOT.MonoPInvokeCallback(typeof(LuaNativeFunction))]
	static int BroadcastMessage(IntPtr L)
	{
		int n = lua_gettop(L) - 1;
		if(n == 1 && CheckType<string>(L, 1))
		{
			const int startStack = 2;
			var obj = SharpObject.Get<UnityEngine.GameObject>(L, 1);
			Get(L, 0 + startStack, out string t0);
			obj.BroadcastMessage(t0);
			return 0;
		}
		else if(n == 2 && CheckType<string, UnityEngine.SendMessageOptions>(L, 1))
		{
			const int startStack = 2;
			var obj = SharpObject.Get<UnityEngine.GameObject>(L, 1);
			Get(L, 0 + startStack, out string t0);
			Get(L, 1 + startStack, out UnityEngine.SendMessageOptions t1);
			obj.BroadcastMessage(t0, t1);
			return 0;
		}
		else if(n == 2 && CheckType<string, object>(L, 1))
		{
			const int startStack = 2;
			var obj = SharpObject.Get<UnityEngine.GameObject>(L, 1);
			Get(L, 0 + startStack, out string t0);
			Get(L, 1 + startStack, out object t1);
			obj.BroadcastMessage(t0, t1);
			return 0;
		}
		else if(n == 3 && CheckType<string, object, UnityEngine.SendMessageOptions>(L, 1))
		{
			const int startStack = 2;
			var obj = SharpObject.Get<UnityEngine.GameObject>(L, 1);
			Get(L, 0 + startStack, out string t0);
			Get(L, 1 + startStack, out object t1);
			Get(L, 2 + startStack, out UnityEngine.SendMessageOptions t2);
			obj.BroadcastMessage(t0, t1, t2);
			return 0;
		}
		return 0;
	}

	[AOT.MonoPInvokeCallback(typeof(LuaNativeFunction))]
	static int AddComponent(IntPtr L)
	{
		const int startStack = 2;
		var obj = SharpObject.Get<UnityEngine.GameObject>(L, 1);
		Get(L, 0 + startStack, out System.Type t0);
		UnityEngine.Component ret = obj.AddComponent(t0);
		Push(L, ret);
		return 1;
	}

	[AOT.MonoPInvokeCallback(typeof(LuaNativeFunction))]
	static int SetActive(IntPtr L)
	{
		const int startStack = 2;
		var obj = SharpObject.Get<UnityEngine.GameObject>(L, 1);
		Get(L, 0 + startStack, out bool t0);
		obj.SetActive(t0);
		return 0;
	}

	[AOT.MonoPInvokeCallback(typeof(LuaNativeFunction))]
	static int CompareTag(IntPtr L)
	{
		const int startStack = 2;
		var obj = SharpObject.Get<UnityEngine.GameObject>(L, 1);
		Get(L, 0 + startStack, out string t0);
		bool ret = obj.CompareTag(t0);
		Push(L, ret);
		return 1;
	}

	[AOT.MonoPInvokeCallback(typeof(LuaNativeFunction))]
	static int FindGameObjectWithTag(IntPtr L)
	{
		#if LUNA_SCRIPT
		const int startStack = 2;
		#else
		const int startStack = 1;
		#endif
		Get(L, 0 + startStack, out string t0);
		UnityEngine.GameObject ret = UnityEngine.GameObject.FindGameObjectWithTag(t0);
		Push(L, ret);
		return 1;
	}

	[AOT.MonoPInvokeCallback(typeof(LuaNativeFunction))]
	static int FindGameObjectsWithTag(IntPtr L)
	{
		#if LUNA_SCRIPT
		const int startStack = 2;
		#else
		const int startStack = 1;
		#endif
		Get(L, 0 + startStack, out string t0);
		UnityEngine.GameObject[] ret = UnityEngine.GameObject.FindGameObjectsWithTag(t0);
		Push(L, ret);
		return 1;
	}

	[AOT.MonoPInvokeCallback(typeof(LuaNativeFunction))]
	static int Find(IntPtr L)
	{
		#if LUNA_SCRIPT
		const int startStack = 2;
		#else
		const int startStack = 1;
		#endif
		Get(L, 0 + startStack, out string t0);
		UnityEngine.GameObject ret = UnityEngine.GameObject.Find(t0);
		Push(L, ret);
		return 1;
	}

	[AOT.MonoPInvokeCallback(typeof(LuaNativeFunction))]
	static int GetInstanceID(IntPtr L)
	{
		var obj = SharpObject.Get<UnityEngine.GameObject>(L, 1);
		int ret = obj.GetInstanceID();
		Push(L, ret);
		return 1;
	}

	[AOT.MonoPInvokeCallback(typeof(LuaNativeFunction))]
	static int GetHashCode(IntPtr L)
	{
		var obj = SharpObject.Get<UnityEngine.GameObject>(L, 1);
		int ret = obj.GetHashCode();
		Push(L, ret);
		return 1;
	}

	[AOT.MonoPInvokeCallback(typeof(LuaNativeFunction))]
	static int Equals(IntPtr L)
	{
		const int startStack = 2;
		var obj = SharpObject.Get<UnityEngine.GameObject>(L, 1);
		Get(L, 0 + startStack, out object t0);
		bool ret = obj.Equals(t0);
		Push(L, ret);
		return 1;
	}

	[AOT.MonoPInvokeCallback(typeof(LuaNativeFunction))]
	static int ToString(IntPtr L)
	{
		var obj = SharpObject.Get<UnityEngine.GameObject>(L, 1);
		string ret = obj.ToString();
		Push(L, ret);
		return 1;
	}

	[AOT.MonoPInvokeCallback(typeof(LuaNativeFunction))]
	static int GetType(IntPtr L)
	{
		var obj = SharpObject.Get<UnityEngine.GameObject>(L, 1);
		System.Type ret = obj.GetType();
		Push(L, ret);
		return 1;
	}

	public static void Register(ClassWraper classWraper)
	{
		classWraper.RegConstructor(Constructor);
		classWraper.RegProperty("transform", Get_transform);
		classWraper.RegProperty("layer", Get_layer, Set_layer);
		classWraper.RegProperty("activeSelf", Get_activeSelf);
		classWraper.RegProperty("activeInHierarchy", Get_activeInHierarchy);
		classWraper.RegProperty("isStatic", Get_isStatic, Set_isStatic);
		classWraper.RegProperty("tag", Get_tag, Set_tag);
		classWraper.RegProperty("scene", Get_scene);
		classWraper.RegProperty("gameObject", Get_gameObject);
		classWraper.RegProperty("name", Get_name, Set_name);
		classWraper.RegProperty("hideFlags", Get_hideFlags, Set_hideFlags);
		classWraper.RegFunction("CreatePrimitive", CreatePrimitive);
		classWraper.RegFunction("GetComponent", GetComponent);
		classWraper.RegFunction("GetComponentInChildren", GetComponentInChildren);
		classWraper.RegFunction("GetComponentInParent", GetComponentInParent);
		classWraper.RegFunction("GetComponents", GetComponents);
		classWraper.RegFunction("GetComponentsInChildren", GetComponentsInChildren);
		classWraper.RegFunction("GetComponentsInParent", GetComponentsInParent);
		classWraper.RegFunction("FindWithTag", FindWithTag);
		classWraper.RegFunction("SendMessageUpwards", SendMessageUpwards);
		classWraper.RegFunction("SendMessage", SendMessage);
		classWraper.RegFunction("BroadcastMessage", BroadcastMessage);
		classWraper.RegFunction("AddComponent", AddComponent);
		classWraper.RegFunction("SetActive", SetActive);
		classWraper.RegFunction("CompareTag", CompareTag);
		classWraper.RegFunction("FindGameObjectWithTag", FindGameObjectWithTag);
		classWraper.RegFunction("FindGameObjectsWithTag", FindGameObjectsWithTag);
		classWraper.RegFunction("Find", Find);
		classWraper.RegFunction("GetInstanceID", GetInstanceID);
		classWraper.RegFunction("GetHashCode", GetHashCode);
		classWraper.RegFunction("Equals", Equals);
		classWraper.RegFunction("ToString", ToString);
		classWraper.RegFunction("GetType", GetType);
	}
}