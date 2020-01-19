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
		if(n == 0)
		{
			obj = new UnityEngine.GameObject();
		}
		else if(n == 1)
		{
			obj = new UnityEngine.GameObject(
				Lua.Get<string>(L, 2)
			);
		}
		else if(n == 2)
		{
			obj = new UnityEngine.GameObject(
				Lua.Get<string>(L, 2),
				Lua.Get<System.Type[]>(L, 3)
			);
		}
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

	[AOT.MonoPInvokeCallback(typeof(LuaNativeFunction))]
	static int CreatePrimitive(IntPtr L)
	{
		#if LUNA_SCRIPT
		const int startStack = 2;
		#else
		const int startStack = 1;
		#endif
		var ret = UnityEngine.GameObject.CreatePrimitive(
			Lua.Get<UnityEngine.PrimitiveType>(L, 0 + startStack)
		);
		Lua.Push(L, ret);
		return 1;
	}

	[AOT.MonoPInvokeCallback(typeof(LuaNativeFunction))]
	static int GetComponent(IntPtr L)
	{
		int n = lua_gettop(L) - 1;
		if(n == 1)
		{
			const int startStack = 2;
			var obj = SharpObject.Get<UnityEngine.GameObject>(L, 1);
			var ret = obj.GetComponent(
				Lua.Get<System.Type>(L, 0 + startStack)
			);
			Lua.Push(L, ret);
			return 1;
		}
		else if(n == 1)
		{
			const int startStack = 2;
			var obj = SharpObject.Get<UnityEngine.GameObject>(L, 1);
			var ret = obj.GetComponent(
				Lua.Get<string>(L, 0 + startStack)
			);
			Lua.Push(L, ret);
			return 1;
		}
		return 0;
	}

	[AOT.MonoPInvokeCallback(typeof(LuaNativeFunction))]
	static int GetComponentInChildren(IntPtr L)
	{
		int n = lua_gettop(L) - 1;
		if(n == 1)
		{
			const int startStack = 2;
			var obj = SharpObject.Get<UnityEngine.GameObject>(L, 1);
			var ret = obj.GetComponentInChildren(
				Lua.Get<System.Type>(L, 0 + startStack)
			);
			Lua.Push(L, ret);
			return 1;
		}
		else if(n == 2)
		{
			const int startStack = 2;
			var obj = SharpObject.Get<UnityEngine.GameObject>(L, 1);
			var ret = obj.GetComponentInChildren(
				Lua.Get<System.Type>(L, 0 + startStack),
				Lua.Get<bool>(L, 1 + startStack)
			);
			Lua.Push(L, ret);
			return 1;
		}
		return 0;
	}

	[AOT.MonoPInvokeCallback(typeof(LuaNativeFunction))]
	static int GetComponentInParent(IntPtr L)
	{
		const int startStack = 2;
		var obj = SharpObject.Get<UnityEngine.GameObject>(L, 1);
		var ret = obj.GetComponentInParent(
			Lua.Get<System.Type>(L, 0 + startStack)
		);
		Lua.Push(L, ret);
		return 1;
	}

	[AOT.MonoPInvokeCallback(typeof(LuaNativeFunction))]
	static int GetComponents(IntPtr L)
	{
		const int startStack = 2;
		var obj = SharpObject.Get<UnityEngine.GameObject>(L, 1);
		var ret = obj.GetComponents(
			Lua.Get<System.Type>(L, 0 + startStack)
		);
		Lua.Push(L, ret);
		return 1;
	}

	[AOT.MonoPInvokeCallback(typeof(LuaNativeFunction))]
	static int GetComponentsInChildren(IntPtr L)
	{
		int n = lua_gettop(L) - 1;
		if(n == 1)
		{
			const int startStack = 2;
			var obj = SharpObject.Get<UnityEngine.GameObject>(L, 1);
			var ret = obj.GetComponentsInChildren(
				Lua.Get<System.Type>(L, 0 + startStack)
			);
			Lua.Push(L, ret);
			return 1;
		}
		else if(n == 2)
		{
			const int startStack = 2;
			var obj = SharpObject.Get<UnityEngine.GameObject>(L, 1);
			var ret = obj.GetComponentsInChildren(
				Lua.Get<System.Type>(L, 0 + startStack),
				Lua.Get<bool>(L, 1 + startStack)
			);
			Lua.Push(L, ret);
			return 1;
		}
		return 0;
	}

	[AOT.MonoPInvokeCallback(typeof(LuaNativeFunction))]
	static int GetComponentsInParent(IntPtr L)
	{
		int n = lua_gettop(L) - 1;
		if(n == 1)
		{
			const int startStack = 2;
			var obj = SharpObject.Get<UnityEngine.GameObject>(L, 1);
			var ret = obj.GetComponentsInParent(
				Lua.Get<System.Type>(L, 0 + startStack)
			);
			Lua.Push(L, ret);
			return 1;
		}
		else if(n == 2)
		{
			const int startStack = 2;
			var obj = SharpObject.Get<UnityEngine.GameObject>(L, 1);
			var ret = obj.GetComponentsInParent(
				Lua.Get<System.Type>(L, 0 + startStack),
				Lua.Get<bool>(L, 1 + startStack)
			);
			Lua.Push(L, ret);
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
		var ret = UnityEngine.GameObject.FindWithTag(
			Lua.Get<string>(L, 0 + startStack)
		);
		Lua.Push(L, ret);
		return 1;
	}

	[AOT.MonoPInvokeCallback(typeof(LuaNativeFunction))]
	static int SendMessageUpwards(IntPtr L)
	{
		int n = lua_gettop(L) - 1;
		if(n == 1)
		{
			const int startStack = 2;
			var obj = SharpObject.Get<UnityEngine.GameObject>(L, 1);
			obj.SendMessageUpwards(
				Lua.Get<string>(L, 0 + startStack)
			);
			return 0;
		}
		else if(n == 2)
		{
			const int startStack = 2;
			var obj = SharpObject.Get<UnityEngine.GameObject>(L, 1);
			obj.SendMessageUpwards(
				Lua.Get<string>(L, 0 + startStack),
				Lua.Get<UnityEngine.SendMessageOptions>(L, 1 + startStack)
			);
			return 0;
		}
		else if(n == 2)
		{
			const int startStack = 2;
			var obj = SharpObject.Get<UnityEngine.GameObject>(L, 1);
			obj.SendMessageUpwards(
				Lua.Get<string>(L, 0 + startStack),
				Lua.Get<object>(L, 1 + startStack)
			);
			return 0;
		}
		else if(n == 3)
		{
			const int startStack = 2;
			var obj = SharpObject.Get<UnityEngine.GameObject>(L, 1);
			obj.SendMessageUpwards(
				Lua.Get<string>(L, 0 + startStack),
				Lua.Get<object>(L, 1 + startStack),
				Lua.Get<UnityEngine.SendMessageOptions>(L, 2 + startStack)
			);
			return 0;
		}
		return 0;
	}

	[AOT.MonoPInvokeCallback(typeof(LuaNativeFunction))]
	static int SendMessage(IntPtr L)
	{
		int n = lua_gettop(L) - 1;
		if(n == 1)
		{
			const int startStack = 2;
			var obj = SharpObject.Get<UnityEngine.GameObject>(L, 1);
			obj.SendMessage(
				Lua.Get<string>(L, 0 + startStack)
			);
			return 0;
		}
		else if(n == 2)
		{
			const int startStack = 2;
			var obj = SharpObject.Get<UnityEngine.GameObject>(L, 1);
			obj.SendMessage(
				Lua.Get<string>(L, 0 + startStack),
				Lua.Get<UnityEngine.SendMessageOptions>(L, 1 + startStack)
			);
			return 0;
		}
		else if(n == 2)
		{
			const int startStack = 2;
			var obj = SharpObject.Get<UnityEngine.GameObject>(L, 1);
			obj.SendMessage(
				Lua.Get<string>(L, 0 + startStack),
				Lua.Get<object>(L, 1 + startStack)
			);
			return 0;
		}
		else if(n == 3)
		{
			const int startStack = 2;
			var obj = SharpObject.Get<UnityEngine.GameObject>(L, 1);
			obj.SendMessage(
				Lua.Get<string>(L, 0 + startStack),
				Lua.Get<object>(L, 1 + startStack),
				Lua.Get<UnityEngine.SendMessageOptions>(L, 2 + startStack)
			);
			return 0;
		}
		return 0;
	}

	[AOT.MonoPInvokeCallback(typeof(LuaNativeFunction))]
	static int BroadcastMessage(IntPtr L)
	{
		int n = lua_gettop(L) - 1;
		if(n == 1)
		{
			const int startStack = 2;
			var obj = SharpObject.Get<UnityEngine.GameObject>(L, 1);
			obj.BroadcastMessage(
				Lua.Get<string>(L, 0 + startStack)
			);
			return 0;
		}
		else if(n == 2)
		{
			const int startStack = 2;
			var obj = SharpObject.Get<UnityEngine.GameObject>(L, 1);
			obj.BroadcastMessage(
				Lua.Get<string>(L, 0 + startStack),
				Lua.Get<UnityEngine.SendMessageOptions>(L, 1 + startStack)
			);
			return 0;
		}
		else if(n == 2)
		{
			const int startStack = 2;
			var obj = SharpObject.Get<UnityEngine.GameObject>(L, 1);
			obj.BroadcastMessage(
				Lua.Get<string>(L, 0 + startStack),
				Lua.Get<object>(L, 1 + startStack)
			);
			return 0;
		}
		else if(n == 3)
		{
			const int startStack = 2;
			var obj = SharpObject.Get<UnityEngine.GameObject>(L, 1);
			obj.BroadcastMessage(
				Lua.Get<string>(L, 0 + startStack),
				Lua.Get<object>(L, 1 + startStack),
				Lua.Get<UnityEngine.SendMessageOptions>(L, 2 + startStack)
			);
			return 0;
		}
		return 0;
	}

	[AOT.MonoPInvokeCallback(typeof(LuaNativeFunction))]
	static int AddComponent(IntPtr L)
	{
		const int startStack = 2;
		var obj = SharpObject.Get<UnityEngine.GameObject>(L, 1);
		var ret = obj.AddComponent(
			Lua.Get<System.Type>(L, 0 + startStack)
		);
		Lua.Push(L, ret);
		return 1;
	}

	[AOT.MonoPInvokeCallback(typeof(LuaNativeFunction))]
	static int SetActive(IntPtr L)
	{
		const int startStack = 2;
		var obj = SharpObject.Get<UnityEngine.GameObject>(L, 1);
		obj.SetActive(
			Lua.Get<bool>(L, 0 + startStack)
		);
		return 0;
	}

	[AOT.MonoPInvokeCallback(typeof(LuaNativeFunction))]
	static int CompareTag(IntPtr L)
	{
		const int startStack = 2;
		var obj = SharpObject.Get<UnityEngine.GameObject>(L, 1);
		var ret = obj.CompareTag(
			Lua.Get<string>(L, 0 + startStack)
		);
		Lua.Push(L, ret);
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
		var ret = UnityEngine.GameObject.FindGameObjectWithTag(
			Lua.Get<string>(L, 0 + startStack)
		);
		Lua.Push(L, ret);
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
		var ret = UnityEngine.GameObject.FindGameObjectsWithTag(
			Lua.Get<string>(L, 0 + startStack)
		);
		Lua.Push(L, ret);
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
		var ret = UnityEngine.GameObject.Find(
			Lua.Get<string>(L, 0 + startStack)
		);
		Lua.Push(L, ret);
		return 1;
	}

	[AOT.MonoPInvokeCallback(typeof(LuaNativeFunction))]
	static int GetInstanceID(IntPtr L)
	{
		var obj = SharpObject.Get<UnityEngine.GameObject>(L, 1);
		var ret = obj.GetInstanceID();
		Lua.Push(L, ret);
		return 1;
	}

	[AOT.MonoPInvokeCallback(typeof(LuaNativeFunction))]
	static int GetHashCode(IntPtr L)
	{
		var obj = SharpObject.Get<UnityEngine.GameObject>(L, 1);
		var ret = obj.GetHashCode();
		Lua.Push(L, ret);
		return 1;
	}

	[AOT.MonoPInvokeCallback(typeof(LuaNativeFunction))]
	static int Equals(IntPtr L)
	{
		const int startStack = 2;
		var obj = SharpObject.Get<UnityEngine.GameObject>(L, 1);
		var ret = obj.Equals(
			Lua.Get<object>(L, 0 + startStack)
		);
		Lua.Push(L, ret);
		return 1;
	}

	[AOT.MonoPInvokeCallback(typeof(LuaNativeFunction))]
	static int ToString(IntPtr L)
	{
		var obj = SharpObject.Get<UnityEngine.GameObject>(L, 1);
		var ret = obj.ToString();
		Lua.Push(L, ret);
		return 1;
	}

	[AOT.MonoPInvokeCallback(typeof(LuaNativeFunction))]
	static int GetType(IntPtr L)
	{
		var obj = SharpObject.Get<UnityEngine.GameObject>(L, 1);
		var ret = obj.GetType();
		Lua.Push(L, ret);
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