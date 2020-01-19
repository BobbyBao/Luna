using System;
using SharpLuna;
using static SharpLuna.Lua;

[WrapClass(typeof(UnityEngine.Quaternion))]
public class QuaternionWrap
{
	[AOT.MonoPInvokeCallback(typeof(LuaNativeFunction))]
	static int Constructor(IntPtr L)
	{
		int n = lua_gettop(L) - 1;
		UnityEngine.Quaternion obj = default;
		if(n == 0)
		{
			obj = new UnityEngine.Quaternion();
		}
		else if(n == 4)
		{
			obj = new UnityEngine.Quaternion(
				Lua.Get<float>(L, 2),
				Lua.Get<float>(L, 3),
				Lua.Get<float>(L, 4),
				Lua.Get<float>(L, 5)
			);
		}
		Lua.Push(L, obj);
		return 1;
	}

	[AOT.MonoPInvokeCallback(typeof(LuaNativeFunction))]
	static int Get_x(IntPtr L)
	{
		ref var obj = ref SharpObject.GetValue<UnityEngine.Quaternion>(L, 1);
		Lua.Push(L, obj.x);
		return 1;
	}

	[AOT.MonoPInvokeCallback(typeof(LuaNativeFunction))]
	static int Set_x(IntPtr L)
	{
		ref var obj = ref SharpObject.GetValue<UnityEngine.Quaternion>(L, 1);
		var p1 = Lua.Get<float>(L, 2);
		obj.x = p1;
		return 0;
	}

	[AOT.MonoPInvokeCallback(typeof(LuaNativeFunction))]
	static int Get_y(IntPtr L)
	{
		ref var obj = ref SharpObject.GetValue<UnityEngine.Quaternion>(L, 1);
		Lua.Push(L, obj.y);
		return 1;
	}

	[AOT.MonoPInvokeCallback(typeof(LuaNativeFunction))]
	static int Set_y(IntPtr L)
	{
		ref var obj = ref SharpObject.GetValue<UnityEngine.Quaternion>(L, 1);
		var p1 = Lua.Get<float>(L, 2);
		obj.y = p1;
		return 0;
	}

	[AOT.MonoPInvokeCallback(typeof(LuaNativeFunction))]
	static int Get_z(IntPtr L)
	{
		ref var obj = ref SharpObject.GetValue<UnityEngine.Quaternion>(L, 1);
		Lua.Push(L, obj.z);
		return 1;
	}

	[AOT.MonoPInvokeCallback(typeof(LuaNativeFunction))]
	static int Set_z(IntPtr L)
	{
		ref var obj = ref SharpObject.GetValue<UnityEngine.Quaternion>(L, 1);
		var p1 = Lua.Get<float>(L, 2);
		obj.z = p1;
		return 0;
	}

	[AOT.MonoPInvokeCallback(typeof(LuaNativeFunction))]
	static int Get_w(IntPtr L)
	{
		ref var obj = ref SharpObject.GetValue<UnityEngine.Quaternion>(L, 1);
		Lua.Push(L, obj.w);
		return 1;
	}

	[AOT.MonoPInvokeCallback(typeof(LuaNativeFunction))]
	static int Set_w(IntPtr L)
	{
		ref var obj = ref SharpObject.GetValue<UnityEngine.Quaternion>(L, 1);
		var p1 = Lua.Get<float>(L, 2);
		obj.w = p1;
		return 0;
	}

	[AOT.MonoPInvokeCallback(typeof(LuaNativeFunction))]
	static int Get_identity(IntPtr L)
	{
		Lua.Push(L, UnityEngine.Quaternion.identity);
		return 1;
	}

	[AOT.MonoPInvokeCallback(typeof(LuaNativeFunction))]
	static int Get_eulerAngles(IntPtr L)
	{
		ref var obj = ref SharpObject.GetValue<UnityEngine.Quaternion>(L, 1);
		Lua.Push(L, obj.eulerAngles);
		return 1;
	}

	[AOT.MonoPInvokeCallback(typeof(LuaNativeFunction))]
	static int Set_eulerAngles(IntPtr L)
	{
		ref var obj = ref SharpObject.GetValue<UnityEngine.Quaternion>(L, 1);
		var p1 = Lua.Get<UnityEngine.Vector3>(L, 2);
		obj.eulerAngles = p1;
		return 0;
	}

	[AOT.MonoPInvokeCallback(typeof(LuaNativeFunction))]
	static int Get_normalized(IntPtr L)
	{
		ref var obj = ref SharpObject.GetValue<UnityEngine.Quaternion>(L, 1);
		Lua.Push(L, obj.normalized);
		return 1;
	}

	[AOT.MonoPInvokeCallback(typeof(LuaNativeFunction))]
	static int FromToRotation(IntPtr L)
	{
		#if LUNA_SCRIPT
		const int startStack = 2;
		#else
		const int startStack = 1;
		#endif
		var ret = UnityEngine.Quaternion.FromToRotation(
			Lua.Get<UnityEngine.Vector3>(L, 0 + startStack),
			Lua.Get<UnityEngine.Vector3>(L, 1 + startStack)
		);
		Lua.Push(L, ret);
		return 1;
	}

	[AOT.MonoPInvokeCallback(typeof(LuaNativeFunction))]
	static int Inverse(IntPtr L)
	{
		#if LUNA_SCRIPT
		const int startStack = 2;
		#else
		const int startStack = 1;
		#endif
		var ret = UnityEngine.Quaternion.Inverse(
			Lua.Get<UnityEngine.Quaternion>(L, 0 + startStack)
		);
		Lua.Push(L, ret);
		return 1;
	}

	[AOT.MonoPInvokeCallback(typeof(LuaNativeFunction))]
	static int Slerp(IntPtr L)
	{
		#if LUNA_SCRIPT
		const int startStack = 2;
		#else
		const int startStack = 1;
		#endif
		var ret = UnityEngine.Quaternion.Slerp(
			Lua.Get<UnityEngine.Quaternion>(L, 0 + startStack),
			Lua.Get<UnityEngine.Quaternion>(L, 1 + startStack),
			Lua.Get<float>(L, 2 + startStack)
		);
		Lua.Push(L, ret);
		return 1;
	}

	[AOT.MonoPInvokeCallback(typeof(LuaNativeFunction))]
	static int SlerpUnclamped(IntPtr L)
	{
		#if LUNA_SCRIPT
		const int startStack = 2;
		#else
		const int startStack = 1;
		#endif
		var ret = UnityEngine.Quaternion.SlerpUnclamped(
			Lua.Get<UnityEngine.Quaternion>(L, 0 + startStack),
			Lua.Get<UnityEngine.Quaternion>(L, 1 + startStack),
			Lua.Get<float>(L, 2 + startStack)
		);
		Lua.Push(L, ret);
		return 1;
	}

	[AOT.MonoPInvokeCallback(typeof(LuaNativeFunction))]
	static int Lerp(IntPtr L)
	{
		#if LUNA_SCRIPT
		const int startStack = 2;
		#else
		const int startStack = 1;
		#endif
		var ret = UnityEngine.Quaternion.Lerp(
			Lua.Get<UnityEngine.Quaternion>(L, 0 + startStack),
			Lua.Get<UnityEngine.Quaternion>(L, 1 + startStack),
			Lua.Get<float>(L, 2 + startStack)
		);
		Lua.Push(L, ret);
		return 1;
	}

	[AOT.MonoPInvokeCallback(typeof(LuaNativeFunction))]
	static int LerpUnclamped(IntPtr L)
	{
		#if LUNA_SCRIPT
		const int startStack = 2;
		#else
		const int startStack = 1;
		#endif
		var ret = UnityEngine.Quaternion.LerpUnclamped(
			Lua.Get<UnityEngine.Quaternion>(L, 0 + startStack),
			Lua.Get<UnityEngine.Quaternion>(L, 1 + startStack),
			Lua.Get<float>(L, 2 + startStack)
		);
		Lua.Push(L, ret);
		return 1;
	}

	[AOT.MonoPInvokeCallback(typeof(LuaNativeFunction))]
	static int AngleAxis(IntPtr L)
	{
		#if LUNA_SCRIPT
		const int startStack = 2;
		#else
		const int startStack = 1;
		#endif
		var ret = UnityEngine.Quaternion.AngleAxis(
			Lua.Get<float>(L, 0 + startStack),
			Lua.Get<UnityEngine.Vector3>(L, 1 + startStack)
		);
		Lua.Push(L, ret);
		return 1;
	}

	[AOT.MonoPInvokeCallback(typeof(LuaNativeFunction))]
	static int LookRotation(IntPtr L)
	{
		int n = lua_gettop(L) - 1;
		if(n == 1)
		{
			#if LUNA_SCRIPT
			const int startStack = 2;
			#else
			const int startStack = 1;
			#endif
			var ret = UnityEngine.Quaternion.LookRotation(
				Lua.Get<UnityEngine.Vector3>(L, 0 + startStack)
			);
			Lua.Push(L, ret);
			return 1;
		}
		else if(n == 2)
		{
			#if LUNA_SCRIPT
			const int startStack = 2;
			#else
			const int startStack = 1;
			#endif
			var ret = UnityEngine.Quaternion.LookRotation(
				Lua.Get<UnityEngine.Vector3>(L, 0 + startStack),
				Lua.Get<UnityEngine.Vector3>(L, 1 + startStack)
			);
			Lua.Push(L, ret);
			return 1;
		}
		return 0;
	}

	[AOT.MonoPInvokeCallback(typeof(LuaNativeFunction))]
	static int Set(IntPtr L)
	{
		const int startStack = 2;
		ref var obj = ref SharpObject.GetValue<UnityEngine.Quaternion>(L, 1);
		obj.Set(
			Lua.Get<float>(L, 0 + startStack),
			Lua.Get<float>(L, 1 + startStack),
			Lua.Get<float>(L, 2 + startStack),
			Lua.Get<float>(L, 3 + startStack)
		);
		return 0;
	}

	[AOT.MonoPInvokeCallback(typeof(LuaNativeFunction))]
	static int Dot(IntPtr L)
	{
		#if LUNA_SCRIPT
		const int startStack = 2;
		#else
		const int startStack = 1;
		#endif
		var ret = UnityEngine.Quaternion.Dot(
			Lua.Get<UnityEngine.Quaternion>(L, 0 + startStack),
			Lua.Get<UnityEngine.Quaternion>(L, 1 + startStack)
		);
		Lua.Push(L, ret);
		return 1;
	}

	[AOT.MonoPInvokeCallback(typeof(LuaNativeFunction))]
	static int SetLookRotation(IntPtr L)
	{
		int n = lua_gettop(L) - 1;
		if(n == 1)
		{
			const int startStack = 2;
			ref var obj = ref SharpObject.GetValue<UnityEngine.Quaternion>(L, 1);
			obj.SetLookRotation(
				Lua.Get<UnityEngine.Vector3>(L, 0 + startStack)
			);
			return 0;
		}
		else if(n == 2)
		{
			const int startStack = 2;
			ref var obj = ref SharpObject.GetValue<UnityEngine.Quaternion>(L, 1);
			obj.SetLookRotation(
				Lua.Get<UnityEngine.Vector3>(L, 0 + startStack),
				Lua.Get<UnityEngine.Vector3>(L, 1 + startStack)
			);
			return 0;
		}
		return 0;
	}

	[AOT.MonoPInvokeCallback(typeof(LuaNativeFunction))]
	static int Angle(IntPtr L)
	{
		#if LUNA_SCRIPT
		const int startStack = 2;
		#else
		const int startStack = 1;
		#endif
		var ret = UnityEngine.Quaternion.Angle(
			Lua.Get<UnityEngine.Quaternion>(L, 0 + startStack),
			Lua.Get<UnityEngine.Quaternion>(L, 1 + startStack)
		);
		Lua.Push(L, ret);
		return 1;
	}

	[AOT.MonoPInvokeCallback(typeof(LuaNativeFunction))]
	static int Euler(IntPtr L)
	{
		int n = lua_gettop(L) - 1;
		if(n == 1)
		{
			#if LUNA_SCRIPT
			const int startStack = 2;
			#else
			const int startStack = 1;
			#endif
			var ret = UnityEngine.Quaternion.Euler(
				Lua.Get<UnityEngine.Vector3>(L, 0 + startStack)
			);
			Lua.Push(L, ret);
			return 1;
		}
		else if(n == 3)
		{
			#if LUNA_SCRIPT
			const int startStack = 2;
			#else
			const int startStack = 1;
			#endif
			var ret = UnityEngine.Quaternion.Euler(
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
	static int SetFromToRotation(IntPtr L)
	{
		const int startStack = 2;
		ref var obj = ref SharpObject.GetValue<UnityEngine.Quaternion>(L, 1);
		obj.SetFromToRotation(
			Lua.Get<UnityEngine.Vector3>(L, 0 + startStack),
			Lua.Get<UnityEngine.Vector3>(L, 1 + startStack)
		);
		return 0;
	}

	[AOT.MonoPInvokeCallback(typeof(LuaNativeFunction))]
	static int RotateTowards(IntPtr L)
	{
		#if LUNA_SCRIPT
		const int startStack = 2;
		#else
		const int startStack = 1;
		#endif
		var ret = UnityEngine.Quaternion.RotateTowards(
			Lua.Get<UnityEngine.Quaternion>(L, 0 + startStack),
			Lua.Get<UnityEngine.Quaternion>(L, 1 + startStack),
			Lua.Get<float>(L, 2 + startStack)
		);
		Lua.Push(L, ret);
		return 1;
	}

	[AOT.MonoPInvokeCallback(typeof(LuaNativeFunction))]
	static int Normalize(IntPtr L)
	{
		int n = lua_gettop(L) - 1;
		if(n == 0)
		{
			ref var obj = ref SharpObject.GetValue<UnityEngine.Quaternion>(L, 1);
			obj.Normalize();
			return 0;
		}
		else if(n == 1)
		{
			#if LUNA_SCRIPT
			const int startStack = 2;
			#else
			const int startStack = 1;
			#endif
			var ret = UnityEngine.Quaternion.Normalize(
				Lua.Get<UnityEngine.Quaternion>(L, 0 + startStack)
			);
			Lua.Push(L, ret);
			return 1;
		}
		return 0;
	}

	[AOT.MonoPInvokeCallback(typeof(LuaNativeFunction))]
	static int GetHashCode(IntPtr L)
	{
		ref var obj = ref SharpObject.GetValue<UnityEngine.Quaternion>(L, 1);
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
			ref var obj = ref SharpObject.GetValue<UnityEngine.Quaternion>(L, 1);
			var ret = obj.Equals(
				Lua.Get<object>(L, 0 + startStack)
			);
			Lua.Push(L, ret);
			return 1;
		}
		else if(n == 1)
		{
			const int startStack = 2;
			ref var obj = ref SharpObject.GetValue<UnityEngine.Quaternion>(L, 1);
			var ret = obj.Equals(
				Lua.Get<UnityEngine.Quaternion>(L, 0 + startStack)
			);
			Lua.Push(L, ret);
			return 1;
		}
		return 0;
	}

	[AOT.MonoPInvokeCallback(typeof(LuaNativeFunction))]
	static int ToString(IntPtr L)
	{
		int n = lua_gettop(L) - 1;
		if(n == 0)
		{
			ref var obj = ref SharpObject.GetValue<UnityEngine.Quaternion>(L, 1);
			var ret = obj.ToString();
			Lua.Push(L, ret);
			return 1;
		}
		else if(n == 1)
		{
			const int startStack = 2;
			ref var obj = ref SharpObject.GetValue<UnityEngine.Quaternion>(L, 1);
			var ret = obj.ToString(
				Lua.Get<string>(L, 0 + startStack)
			);
			Lua.Push(L, ret);
			return 1;
		}
		return 0;
	}

	[AOT.MonoPInvokeCallback(typeof(LuaNativeFunction))]
	static int GetType(IntPtr L)
	{
		ref var obj = ref SharpObject.GetValue<UnityEngine.Quaternion>(L, 1);
		var ret = obj.GetType();
		Lua.Push(L, ret);
		return 1;
	}

	public static void Register(ClassWraper classWraper)
	{
		classWraper.RegConstructor(Constructor);
		classWraper.RegField("x", Get_x, Set_x);
		classWraper.RegField("y", Get_y, Set_y);
		classWraper.RegField("z", Get_z, Set_z);
		classWraper.RegField("w", Get_w, Set_w);
		classWraper.RegProperty("identity", Get_identity);
		classWraper.RegProperty("eulerAngles", Get_eulerAngles, Set_eulerAngles);
		classWraper.RegProperty("normalized", Get_normalized);
		classWraper.RegFunction("FromToRotation", FromToRotation);
		classWraper.RegFunction("Inverse", Inverse);
		classWraper.RegFunction("Slerp", Slerp);
		classWraper.RegFunction("SlerpUnclamped", SlerpUnclamped);
		classWraper.RegFunction("Lerp", Lerp);
		classWraper.RegFunction("LerpUnclamped", LerpUnclamped);
		classWraper.RegFunction("AngleAxis", AngleAxis);
		classWraper.RegFunction("LookRotation", LookRotation);
		classWraper.RegFunction("Set", Set);
		classWraper.RegFunction("Dot", Dot);
		classWraper.RegFunction("SetLookRotation", SetLookRotation);
		classWraper.RegFunction("Angle", Angle);
		classWraper.RegFunction("Euler", Euler);
		classWraper.RegFunction("SetFromToRotation", SetFromToRotation);
		classWraper.RegFunction("RotateTowards", RotateTowards);
		classWraper.RegFunction("Normalize", Normalize);
		classWraper.RegFunction("GetHashCode", GetHashCode);
		classWraper.RegFunction("Equals", Equals);
		classWraper.RegFunction("ToString", ToString);
		classWraper.RegFunction("GetType", GetType);
	}
}