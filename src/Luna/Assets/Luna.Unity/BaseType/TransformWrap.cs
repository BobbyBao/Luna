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

	[AOT.MonoPInvokeCallback(typeof(LuaNativeFunction))]
	static int SetParent(IntPtr L)
	{
		int n = lua_gettop(L) - 1;
		if(n == 1)
		{
			const int startStack = 2;
			var obj = SharpObject.Get<UnityEngine.Transform>(L, 1);
			obj.SetParent(
				Lua.Get<UnityEngine.Transform>(L, 0 + startStack)
			);
			return 0;
		}
		else if(n == 2)
		{
			const int startStack = 2;
			var obj = SharpObject.Get<UnityEngine.Transform>(L, 1);
			obj.SetParent(
				Lua.Get<UnityEngine.Transform>(L, 0 + startStack),
				Lua.Get<bool>(L, 1 + startStack)
			);
			return 0;
		}
		return 0;
	}

	[AOT.MonoPInvokeCallback(typeof(LuaNativeFunction))]
	static int SetPositionAndRotation(IntPtr L)
	{
		const int startStack = 2;
		var obj = SharpObject.Get<UnityEngine.Transform>(L, 1);
		obj.SetPositionAndRotation(
			Lua.Get<UnityEngine.Vector3>(L, 0 + startStack),
			Lua.Get<UnityEngine.Quaternion>(L, 1 + startStack)
		);
		return 0;
	}

	[AOT.MonoPInvokeCallback(typeof(LuaNativeFunction))]
	static int Translate(IntPtr L)
	{
		int n = lua_gettop(L) - 1;
		if(n == 1)
		{
			const int startStack = 2;
			var obj = SharpObject.Get<UnityEngine.Transform>(L, 1);
			obj.Translate(
				Lua.Get<UnityEngine.Vector3>(L, 0 + startStack)
			);
			return 0;
		}
		else if(n == 2)
		{
			const int startStack = 2;
			var obj = SharpObject.Get<UnityEngine.Transform>(L, 1);
			obj.Translate(
				Lua.Get<UnityEngine.Vector3>(L, 0 + startStack),
				Lua.Get<UnityEngine.Space>(L, 1 + startStack)
			);
			return 0;
		}
		else if(n == 2)
		{
			const int startStack = 2;
			var obj = SharpObject.Get<UnityEngine.Transform>(L, 1);
			obj.Translate(
				Lua.Get<UnityEngine.Vector3>(L, 0 + startStack),
				Lua.Get<UnityEngine.Transform>(L, 1 + startStack)
			);
			return 0;
		}
		else if(n == 3)
		{
			const int startStack = 2;
			var obj = SharpObject.Get<UnityEngine.Transform>(L, 1);
			obj.Translate(
				Lua.Get<float>(L, 0 + startStack),
				Lua.Get<float>(L, 1 + startStack),
				Lua.Get<float>(L, 2 + startStack)
			);
			return 0;
		}
		else if(n == 4)
		{
			const int startStack = 2;
			var obj = SharpObject.Get<UnityEngine.Transform>(L, 1);
			obj.Translate(
				Lua.Get<float>(L, 0 + startStack),
				Lua.Get<float>(L, 1 + startStack),
				Lua.Get<float>(L, 2 + startStack),
				Lua.Get<UnityEngine.Space>(L, 3 + startStack)
			);
			return 0;
		}
		else if(n == 4)
		{
			const int startStack = 2;
			var obj = SharpObject.Get<UnityEngine.Transform>(L, 1);
			obj.Translate(
				Lua.Get<float>(L, 0 + startStack),
				Lua.Get<float>(L, 1 + startStack),
				Lua.Get<float>(L, 2 + startStack),
				Lua.Get<UnityEngine.Transform>(L, 3 + startStack)
			);
			return 0;
		}
		return 0;
	}

	[AOT.MonoPInvokeCallback(typeof(LuaNativeFunction))]
	static int Rotate(IntPtr L)
	{
		int n = lua_gettop(L) - 1;
		if(n == 1)
		{
			const int startStack = 2;
			var obj = SharpObject.Get<UnityEngine.Transform>(L, 1);
			obj.Rotate(
				Lua.Get<UnityEngine.Vector3>(L, 0 + startStack)
			);
			return 0;
		}
		else if(n == 2)
		{
			const int startStack = 2;
			var obj = SharpObject.Get<UnityEngine.Transform>(L, 1);
			obj.Rotate(
				Lua.Get<UnityEngine.Vector3>(L, 0 + startStack),
				Lua.Get<UnityEngine.Space>(L, 1 + startStack)
			);
			return 0;
		}
		else if(n == 2)
		{
			const int startStack = 2;
			var obj = SharpObject.Get<UnityEngine.Transform>(L, 1);
			obj.Rotate(
				Lua.Get<UnityEngine.Vector3>(L, 0 + startStack),
				Lua.Get<float>(L, 1 + startStack)
			);
			return 0;
		}
		else if(n == 3)
		{
			const int startStack = 2;
			var obj = SharpObject.Get<UnityEngine.Transform>(L, 1);
			obj.Rotate(
				Lua.Get<float>(L, 0 + startStack),
				Lua.Get<float>(L, 1 + startStack),
				Lua.Get<float>(L, 2 + startStack)
			);
			return 0;
		}
		else if(n == 3)
		{
			const int startStack = 2;
			var obj = SharpObject.Get<UnityEngine.Transform>(L, 1);
			obj.Rotate(
				Lua.Get<UnityEngine.Vector3>(L, 0 + startStack),
				Lua.Get<float>(L, 1 + startStack),
				Lua.Get<UnityEngine.Space>(L, 2 + startStack)
			);
			return 0;
		}
		else if(n == 4)
		{
			const int startStack = 2;
			var obj = SharpObject.Get<UnityEngine.Transform>(L, 1);
			obj.Rotate(
				Lua.Get<float>(L, 0 + startStack),
				Lua.Get<float>(L, 1 + startStack),
				Lua.Get<float>(L, 2 + startStack),
				Lua.Get<UnityEngine.Space>(L, 3 + startStack)
			);
			return 0;
		}
		return 0;
	}

	[AOT.MonoPInvokeCallback(typeof(LuaNativeFunction))]
	static int RotateAround(IntPtr L)
	{
		const int startStack = 2;
		var obj = SharpObject.Get<UnityEngine.Transform>(L, 1);
		obj.RotateAround(
			Lua.Get<UnityEngine.Vector3>(L, 0 + startStack),
			Lua.Get<UnityEngine.Vector3>(L, 1 + startStack),
			Lua.Get<float>(L, 2 + startStack)
		);
		return 0;
	}

	[AOT.MonoPInvokeCallback(typeof(LuaNativeFunction))]
	static int LookAt(IntPtr L)
	{
		int n = lua_gettop(L) - 1;
		if(n == 1)
		{
			const int startStack = 2;
			var obj = SharpObject.Get<UnityEngine.Transform>(L, 1);
			obj.LookAt(
				Lua.Get<UnityEngine.Transform>(L, 0 + startStack)
			);
			return 0;
		}
		else if(n == 1)
		{
			const int startStack = 2;
			var obj = SharpObject.Get<UnityEngine.Transform>(L, 1);
			obj.LookAt(
				Lua.Get<UnityEngine.Vector3>(L, 0 + startStack)
			);
			return 0;
		}
		else if(n == 2)
		{
			const int startStack = 2;
			var obj = SharpObject.Get<UnityEngine.Transform>(L, 1);
			obj.LookAt(
				Lua.Get<UnityEngine.Transform>(L, 0 + startStack),
				Lua.Get<UnityEngine.Vector3>(L, 1 + startStack)
			);
			return 0;
		}
		else if(n == 2)
		{
			const int startStack = 2;
			var obj = SharpObject.Get<UnityEngine.Transform>(L, 1);
			obj.LookAt(
				Lua.Get<UnityEngine.Vector3>(L, 0 + startStack),
				Lua.Get<UnityEngine.Vector3>(L, 1 + startStack)
			);
			return 0;
		}
		return 0;
	}

	[AOT.MonoPInvokeCallback(typeof(LuaNativeFunction))]
	static int TransformDirection(IntPtr L)
	{
		int n = lua_gettop(L) - 1;
		if(n == 1)
		{
			const int startStack = 2;
			var obj = SharpObject.Get<UnityEngine.Transform>(L, 1);
			var ret = obj.TransformDirection(
				Lua.Get<UnityEngine.Vector3>(L, 0 + startStack)
			);
			Lua.Push(L, ret);
			return 1;
		}
		else if(n == 3)
		{
			const int startStack = 2;
			var obj = SharpObject.Get<UnityEngine.Transform>(L, 1);
			var ret = obj.TransformDirection(
				Lua.Get<float>(L, 0 + startStack),
				Lua.Get<float>(L, 1 + startStack),
				Lua.Get<float>(L, 2 + startStack)
			);
			Lua.Push(L, ret);
			return 1;
		}
		return 0;
	}

	[AOT.MonoPInvokeCallback(typeof(LuaNativeFunction))]
	static int InverseTransformDirection(IntPtr L)
	{
		int n = lua_gettop(L) - 1;
		if(n == 1)
		{
			const int startStack = 2;
			var obj = SharpObject.Get<UnityEngine.Transform>(L, 1);
			var ret = obj.InverseTransformDirection(
				Lua.Get<UnityEngine.Vector3>(L, 0 + startStack)
			);
			Lua.Push(L, ret);
			return 1;
		}
		else if(n == 3)
		{
			const int startStack = 2;
			var obj = SharpObject.Get<UnityEngine.Transform>(L, 1);
			var ret = obj.InverseTransformDirection(
				Lua.Get<float>(L, 0 + startStack),
				Lua.Get<float>(L, 1 + startStack),
				Lua.Get<float>(L, 2 + startStack)
			);
			Lua.Push(L, ret);
			return 1;
		}
		return 0;
	}

	[AOT.MonoPInvokeCallback(typeof(LuaNativeFunction))]
	static int TransformVector(IntPtr L)
	{
		int n = lua_gettop(L) - 1;
		if(n == 1)
		{
			const int startStack = 2;
			var obj = SharpObject.Get<UnityEngine.Transform>(L, 1);
			var ret = obj.TransformVector(
				Lua.Get<UnityEngine.Vector3>(L, 0 + startStack)
			);
			Lua.Push(L, ret);
			return 1;
		}
		else if(n == 3)
		{
			const int startStack = 2;
			var obj = SharpObject.Get<UnityEngine.Transform>(L, 1);
			var ret = obj.TransformVector(
				Lua.Get<float>(L, 0 + startStack),
				Lua.Get<float>(L, 1 + startStack),
				Lua.Get<float>(L, 2 + startStack)
			);
			Lua.Push(L, ret);
			return 1;
		}
		return 0;
	}

	[AOT.MonoPInvokeCallback(typeof(LuaNativeFunction))]
	static int InverseTransformVector(IntPtr L)
	{
		int n = lua_gettop(L) - 1;
		if(n == 1)
		{
			const int startStack = 2;
			var obj = SharpObject.Get<UnityEngine.Transform>(L, 1);
			var ret = obj.InverseTransformVector(
				Lua.Get<UnityEngine.Vector3>(L, 0 + startStack)
			);
			Lua.Push(L, ret);
			return 1;
		}
		else if(n == 3)
		{
			const int startStack = 2;
			var obj = SharpObject.Get<UnityEngine.Transform>(L, 1);
			var ret = obj.InverseTransformVector(
				Lua.Get<float>(L, 0 + startStack),
				Lua.Get<float>(L, 1 + startStack),
				Lua.Get<float>(L, 2 + startStack)
			);
			Lua.Push(L, ret);
			return 1;
		}
		return 0;
	}

	[AOT.MonoPInvokeCallback(typeof(LuaNativeFunction))]
	static int TransformPoint(IntPtr L)
	{
		int n = lua_gettop(L) - 1;
		if(n == 1)
		{
			const int startStack = 2;
			var obj = SharpObject.Get<UnityEngine.Transform>(L, 1);
			var ret = obj.TransformPoint(
				Lua.Get<UnityEngine.Vector3>(L, 0 + startStack)
			);
			Lua.Push(L, ret);
			return 1;
		}
		else if(n == 3)
		{
			const int startStack = 2;
			var obj = SharpObject.Get<UnityEngine.Transform>(L, 1);
			var ret = obj.TransformPoint(
				Lua.Get<float>(L, 0 + startStack),
				Lua.Get<float>(L, 1 + startStack),
				Lua.Get<float>(L, 2 + startStack)
			);
			Lua.Push(L, ret);
			return 1;
		}
		return 0;
	}

	[AOT.MonoPInvokeCallback(typeof(LuaNativeFunction))]
	static int InverseTransformPoint(IntPtr L)
	{
		int n = lua_gettop(L) - 1;
		if(n == 1)
		{
			const int startStack = 2;
			var obj = SharpObject.Get<UnityEngine.Transform>(L, 1);
			var ret = obj.InverseTransformPoint(
				Lua.Get<UnityEngine.Vector3>(L, 0 + startStack)
			);
			Lua.Push(L, ret);
			return 1;
		}
		else if(n == 3)
		{
			const int startStack = 2;
			var obj = SharpObject.Get<UnityEngine.Transform>(L, 1);
			var ret = obj.InverseTransformPoint(
				Lua.Get<float>(L, 0 + startStack),
				Lua.Get<float>(L, 1 + startStack),
				Lua.Get<float>(L, 2 + startStack)
			);
			Lua.Push(L, ret);
			return 1;
		}
		return 0;
	}

	[AOT.MonoPInvokeCallback(typeof(LuaNativeFunction))]
	static int DetachChildren(IntPtr L)
	{
		var obj = SharpObject.Get<UnityEngine.Transform>(L, 1);
		obj.DetachChildren();
		return 0;
	}

	[AOT.MonoPInvokeCallback(typeof(LuaNativeFunction))]
	static int SetAsFirstSibling(IntPtr L)
	{
		var obj = SharpObject.Get<UnityEngine.Transform>(L, 1);
		obj.SetAsFirstSibling();
		return 0;
	}

	[AOT.MonoPInvokeCallback(typeof(LuaNativeFunction))]
	static int SetAsLastSibling(IntPtr L)
	{
		var obj = SharpObject.Get<UnityEngine.Transform>(L, 1);
		obj.SetAsLastSibling();
		return 0;
	}

	[AOT.MonoPInvokeCallback(typeof(LuaNativeFunction))]
	static int SetSiblingIndex(IntPtr L)
	{
		const int startStack = 2;
		var obj = SharpObject.Get<UnityEngine.Transform>(L, 1);
		obj.SetSiblingIndex(
			Lua.Get<int>(L, 0 + startStack)
		);
		return 0;
	}

	[AOT.MonoPInvokeCallback(typeof(LuaNativeFunction))]
	static int GetSiblingIndex(IntPtr L)
	{
		var obj = SharpObject.Get<UnityEngine.Transform>(L, 1);
		var ret = obj.GetSiblingIndex();
		Lua.Push(L, ret);
		return 1;
	}

	[AOT.MonoPInvokeCallback(typeof(LuaNativeFunction))]
	static int Find(IntPtr L)
	{
		const int startStack = 2;
		var obj = SharpObject.Get<UnityEngine.Transform>(L, 1);
		var ret = obj.Find(
			Lua.Get<string>(L, 0 + startStack)
		);
		Lua.Push(L, ret);
		return 1;
	}

	[AOT.MonoPInvokeCallback(typeof(LuaNativeFunction))]
	static int IsChildOf(IntPtr L)
	{
		const int startStack = 2;
		var obj = SharpObject.Get<UnityEngine.Transform>(L, 1);
		var ret = obj.IsChildOf(
			Lua.Get<UnityEngine.Transform>(L, 0 + startStack)
		);
		Lua.Push(L, ret);
		return 1;
	}

	[AOT.MonoPInvokeCallback(typeof(LuaNativeFunction))]
	static int GetEnumerator(IntPtr L)
	{
		var obj = SharpObject.Get<UnityEngine.Transform>(L, 1);
		var ret = obj.GetEnumerator();
		Lua.Push(L, ret);
		return 1;
	}

	[AOT.MonoPInvokeCallback(typeof(LuaNativeFunction))]
	static int GetChild(IntPtr L)
	{
		const int startStack = 2;
		var obj = SharpObject.Get<UnityEngine.Transform>(L, 1);
		var ret = obj.GetChild(
			Lua.Get<int>(L, 0 + startStack)
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
			var obj = SharpObject.Get<UnityEngine.Transform>(L, 1);
			var ret = obj.GetComponent(
				Lua.Get<System.Type>(L, 0 + startStack)
			);
			Lua.Push(L, ret);
			return 1;
		}
		else if(n == 1)
		{
			const int startStack = 2;
			var obj = SharpObject.Get<UnityEngine.Transform>(L, 1);
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
			var obj = SharpObject.Get<UnityEngine.Transform>(L, 1);
			var ret = obj.GetComponentInChildren(
				Lua.Get<System.Type>(L, 0 + startStack)
			);
			Lua.Push(L, ret);
			return 1;
		}
		else if(n == 2)
		{
			const int startStack = 2;
			var obj = SharpObject.Get<UnityEngine.Transform>(L, 1);
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
	static int GetComponentsInChildren(IntPtr L)
	{
		int n = lua_gettop(L) - 1;
		if(n == 1)
		{
			const int startStack = 2;
			var obj = SharpObject.Get<UnityEngine.Transform>(L, 1);
			var ret = obj.GetComponentsInChildren(
				Lua.Get<System.Type>(L, 0 + startStack)
			);
			Lua.Push(L, ret);
			return 1;
		}
		else if(n == 2)
		{
			const int startStack = 2;
			var obj = SharpObject.Get<UnityEngine.Transform>(L, 1);
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
	static int GetComponentInParent(IntPtr L)
	{
		const int startStack = 2;
		var obj = SharpObject.Get<UnityEngine.Transform>(L, 1);
		var ret = obj.GetComponentInParent(
			Lua.Get<System.Type>(L, 0 + startStack)
		);
		Lua.Push(L, ret);
		return 1;
	}

	[AOT.MonoPInvokeCallback(typeof(LuaNativeFunction))]
	static int GetComponentsInParent(IntPtr L)
	{
		int n = lua_gettop(L) - 1;
		if(n == 1)
		{
			const int startStack = 2;
			var obj = SharpObject.Get<UnityEngine.Transform>(L, 1);
			var ret = obj.GetComponentsInParent(
				Lua.Get<System.Type>(L, 0 + startStack)
			);
			Lua.Push(L, ret);
			return 1;
		}
		else if(n == 2)
		{
			const int startStack = 2;
			var obj = SharpObject.Get<UnityEngine.Transform>(L, 1);
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
	static int GetComponents(IntPtr L)
	{
		const int startStack = 2;
		var obj = SharpObject.Get<UnityEngine.Transform>(L, 1);
		var ret = obj.GetComponents(
			Lua.Get<System.Type>(L, 0 + startStack)
		);
		Lua.Push(L, ret);
		return 1;
	}

	[AOT.MonoPInvokeCallback(typeof(LuaNativeFunction))]
	static int CompareTag(IntPtr L)
	{
		const int startStack = 2;
		var obj = SharpObject.Get<UnityEngine.Transform>(L, 1);
		var ret = obj.CompareTag(
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
			var obj = SharpObject.Get<UnityEngine.Transform>(L, 1);
			obj.SendMessageUpwards(
				Lua.Get<string>(L, 0 + startStack)
			);
			return 0;
		}
		else if(n == 2)
		{
			const int startStack = 2;
			var obj = SharpObject.Get<UnityEngine.Transform>(L, 1);
			obj.SendMessageUpwards(
				Lua.Get<string>(L, 0 + startStack),
				Lua.Get<object>(L, 1 + startStack)
			);
			return 0;
		}
		else if(n == 2)
		{
			const int startStack = 2;
			var obj = SharpObject.Get<UnityEngine.Transform>(L, 1);
			obj.SendMessageUpwards(
				Lua.Get<string>(L, 0 + startStack),
				Lua.Get<UnityEngine.SendMessageOptions>(L, 1 + startStack)
			);
			return 0;
		}
		else if(n == 3)
		{
			const int startStack = 2;
			var obj = SharpObject.Get<UnityEngine.Transform>(L, 1);
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
			var obj = SharpObject.Get<UnityEngine.Transform>(L, 1);
			obj.SendMessage(
				Lua.Get<string>(L, 0 + startStack)
			);
			return 0;
		}
		else if(n == 2)
		{
			const int startStack = 2;
			var obj = SharpObject.Get<UnityEngine.Transform>(L, 1);
			obj.SendMessage(
				Lua.Get<string>(L, 0 + startStack),
				Lua.Get<object>(L, 1 + startStack)
			);
			return 0;
		}
		else if(n == 2)
		{
			const int startStack = 2;
			var obj = SharpObject.Get<UnityEngine.Transform>(L, 1);
			obj.SendMessage(
				Lua.Get<string>(L, 0 + startStack),
				Lua.Get<UnityEngine.SendMessageOptions>(L, 1 + startStack)
			);
			return 0;
		}
		else if(n == 3)
		{
			const int startStack = 2;
			var obj = SharpObject.Get<UnityEngine.Transform>(L, 1);
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
			var obj = SharpObject.Get<UnityEngine.Transform>(L, 1);
			obj.BroadcastMessage(
				Lua.Get<string>(L, 0 + startStack)
			);
			return 0;
		}
		else if(n == 2)
		{
			const int startStack = 2;
			var obj = SharpObject.Get<UnityEngine.Transform>(L, 1);
			obj.BroadcastMessage(
				Lua.Get<string>(L, 0 + startStack),
				Lua.Get<object>(L, 1 + startStack)
			);
			return 0;
		}
		else if(n == 2)
		{
			const int startStack = 2;
			var obj = SharpObject.Get<UnityEngine.Transform>(L, 1);
			obj.BroadcastMessage(
				Lua.Get<string>(L, 0 + startStack),
				Lua.Get<UnityEngine.SendMessageOptions>(L, 1 + startStack)
			);
			return 0;
		}
		else if(n == 3)
		{
			const int startStack = 2;
			var obj = SharpObject.Get<UnityEngine.Transform>(L, 1);
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
	static int GetInstanceID(IntPtr L)
	{
		var obj = SharpObject.Get<UnityEngine.Transform>(L, 1);
		var ret = obj.GetInstanceID();
		Lua.Push(L, ret);
		return 1;
	}

	[AOT.MonoPInvokeCallback(typeof(LuaNativeFunction))]
	static int GetHashCode(IntPtr L)
	{
		var obj = SharpObject.Get<UnityEngine.Transform>(L, 1);
		var ret = obj.GetHashCode();
		Lua.Push(L, ret);
		return 1;
	}

	[AOT.MonoPInvokeCallback(typeof(LuaNativeFunction))]
	static int Equals(IntPtr L)
	{
		const int startStack = 2;
		var obj = SharpObject.Get<UnityEngine.Transform>(L, 1);
		var ret = obj.Equals(
			Lua.Get<object>(L, 0 + startStack)
		);
		Lua.Push(L, ret);
		return 1;
	}

	[AOT.MonoPInvokeCallback(typeof(LuaNativeFunction))]
	static int ToString(IntPtr L)
	{
		var obj = SharpObject.Get<UnityEngine.Transform>(L, 1);
		var ret = obj.ToString();
		Lua.Push(L, ret);
		return 1;
	}

	[AOT.MonoPInvokeCallback(typeof(LuaNativeFunction))]
	static int GetType(IntPtr L)
	{
		var obj = SharpObject.Get<UnityEngine.Transform>(L, 1);
		var ret = obj.GetType();
		Lua.Push(L, ret);
		return 1;
	}

	public static void Register(ClassWraper classWraper)
	{
		classWraper.RegProperty("position", Get_position, Set_position);
		classWraper.RegProperty("localPosition", Get_localPosition, Set_localPosition);
		classWraper.RegProperty("eulerAngles", Get_eulerAngles, Set_eulerAngles);
		classWraper.RegProperty("localEulerAngles", Get_localEulerAngles, Set_localEulerAngles);
		classWraper.RegProperty("right", Get_right, Set_right);
		classWraper.RegProperty("up", Get_up, Set_up);
		classWraper.RegProperty("forward", Get_forward, Set_forward);
		classWraper.RegProperty("rotation", Get_rotation, Set_rotation);
		classWraper.RegProperty("localRotation", Get_localRotation, Set_localRotation);
		classWraper.RegProperty("localScale", Get_localScale, Set_localScale);
		classWraper.RegProperty("parent", Get_parent, Set_parent);
		classWraper.RegProperty("worldToLocalMatrix", Get_worldToLocalMatrix);
		classWraper.RegProperty("localToWorldMatrix", Get_localToWorldMatrix);
		classWraper.RegProperty("root", Get_root);
		classWraper.RegProperty("childCount", Get_childCount);
		classWraper.RegProperty("lossyScale", Get_lossyScale);
		classWraper.RegProperty("hasChanged", Get_hasChanged, Set_hasChanged);
		classWraper.RegProperty("hierarchyCapacity", Get_hierarchyCapacity, Set_hierarchyCapacity);
		classWraper.RegProperty("hierarchyCount", Get_hierarchyCount);
		classWraper.RegProperty("transform", Get_transform);
		classWraper.RegProperty("gameObject", Get_gameObject);
		classWraper.RegProperty("tag", Get_tag, Set_tag);
		classWraper.RegProperty("name", Get_name, Set_name);
		classWraper.RegProperty("hideFlags", Get_hideFlags, Set_hideFlags);
		classWraper.RegFunction("SetParent", SetParent);
		classWraper.RegFunction("SetPositionAndRotation", SetPositionAndRotation);
		classWraper.RegFunction("Translate", Translate);
		classWraper.RegFunction("Rotate", Rotate);
		classWraper.RegFunction("RotateAround", RotateAround);
		classWraper.RegFunction("LookAt", LookAt);
		classWraper.RegFunction("TransformDirection", TransformDirection);
		classWraper.RegFunction("InverseTransformDirection", InverseTransformDirection);
		classWraper.RegFunction("TransformVector", TransformVector);
		classWraper.RegFunction("InverseTransformVector", InverseTransformVector);
		classWraper.RegFunction("TransformPoint", TransformPoint);
		classWraper.RegFunction("InverseTransformPoint", InverseTransformPoint);
		classWraper.RegFunction("DetachChildren", DetachChildren);
		classWraper.RegFunction("SetAsFirstSibling", SetAsFirstSibling);
		classWraper.RegFunction("SetAsLastSibling", SetAsLastSibling);
		classWraper.RegFunction("SetSiblingIndex", SetSiblingIndex);
		classWraper.RegFunction("GetSiblingIndex", GetSiblingIndex);
		classWraper.RegFunction("Find", Find);
		classWraper.RegFunction("IsChildOf", IsChildOf);
		classWraper.RegFunction("GetEnumerator", GetEnumerator);
		classWraper.RegFunction("GetChild", GetChild);
		classWraper.RegFunction("GetComponent", GetComponent);
		classWraper.RegFunction("GetComponentInChildren", GetComponentInChildren);
		classWraper.RegFunction("GetComponentsInChildren", GetComponentsInChildren);
		classWraper.RegFunction("GetComponentInParent", GetComponentInParent);
		classWraper.RegFunction("GetComponentsInParent", GetComponentsInParent);
		classWraper.RegFunction("GetComponents", GetComponents);
		classWraper.RegFunction("CompareTag", CompareTag);
		classWraper.RegFunction("SendMessageUpwards", SendMessageUpwards);
		classWraper.RegFunction("SendMessage", SendMessage);
		classWraper.RegFunction("BroadcastMessage", BroadcastMessage);
		classWraper.RegFunction("GetInstanceID", GetInstanceID);
		classWraper.RegFunction("GetHashCode", GetHashCode);
		classWraper.RegFunction("Equals", Equals);
		classWraper.RegFunction("ToString", ToString);
		classWraper.RegFunction("GetType", GetType);
	}
}