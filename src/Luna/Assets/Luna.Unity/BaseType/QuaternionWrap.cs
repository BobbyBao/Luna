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
			Get(L, 2, out float t1);
			Get(L, 3, out float t2);
			Get(L, 4, out float t3);
			Get(L, 5, out float t4);
			obj = new UnityEngine.Quaternion(t1, t2, t3, t4);
		}
		Push(L, obj);
		return 1;
	}

	[AOT.MonoPInvokeCallback(typeof(LuaNativeFunction))]
	static int Get_x(IntPtr L)
	{
		ref var obj = ref SharpObject.GetValue<UnityEngine.Quaternion>(L, 1);
		Push(L, obj.x);
		return 1;
	}

	[AOT.MonoPInvokeCallback(typeof(LuaNativeFunction))]
	static int Set_x(IntPtr L)
	{
		ref var obj = ref SharpObject.GetValue<UnityEngine.Quaternion>(L, 1);
		Get(L, 2, out float p1);
		obj.x = p1;
		return 0;
	}

	[AOT.MonoPInvokeCallback(typeof(LuaNativeFunction))]
	static int Get_y(IntPtr L)
	{
		ref var obj = ref SharpObject.GetValue<UnityEngine.Quaternion>(L, 1);
		Push(L, obj.y);
		return 1;
	}

	[AOT.MonoPInvokeCallback(typeof(LuaNativeFunction))]
	static int Set_y(IntPtr L)
	{
		ref var obj = ref SharpObject.GetValue<UnityEngine.Quaternion>(L, 1);
		Get(L, 2, out float p1);
		obj.y = p1;
		return 0;
	}

	[AOT.MonoPInvokeCallback(typeof(LuaNativeFunction))]
	static int Get_z(IntPtr L)
	{
		ref var obj = ref SharpObject.GetValue<UnityEngine.Quaternion>(L, 1);
		Push(L, obj.z);
		return 1;
	}

	[AOT.MonoPInvokeCallback(typeof(LuaNativeFunction))]
	static int Set_z(IntPtr L)
	{
		ref var obj = ref SharpObject.GetValue<UnityEngine.Quaternion>(L, 1);
		Get(L, 2, out float p1);
		obj.z = p1;
		return 0;
	}

	[AOT.MonoPInvokeCallback(typeof(LuaNativeFunction))]
	static int Get_w(IntPtr L)
	{
		ref var obj = ref SharpObject.GetValue<UnityEngine.Quaternion>(L, 1);
		Push(L, obj.w);
		return 1;
	}

	[AOT.MonoPInvokeCallback(typeof(LuaNativeFunction))]
	static int Set_w(IntPtr L)
	{
		ref var obj = ref SharpObject.GetValue<UnityEngine.Quaternion>(L, 1);
		Get(L, 2, out float p1);
		obj.w = p1;
		return 0;
	}

	[AOT.MonoPInvokeCallback(typeof(LuaNativeFunction))]
	static int Get_identity(IntPtr L)
	{
		Push(L, UnityEngine.Quaternion.identity);
		return 1;
	}

	[AOT.MonoPInvokeCallback(typeof(LuaNativeFunction))]
	static int Get_eulerAngles(IntPtr L)
	{
		ref var obj = ref SharpObject.GetValue<UnityEngine.Quaternion>(L, 1);
		Push(L, obj.eulerAngles);
		return 1;
	}

	[AOT.MonoPInvokeCallback(typeof(LuaNativeFunction))]
	static int Set_eulerAngles(IntPtr L)
	{
		ref var obj = ref SharpObject.GetValue<UnityEngine.Quaternion>(L, 1);
		Get(L, 2, out UnityEngine.Vector3 p1);
		obj.eulerAngles = p1;
		return 0;
	}

	[AOT.MonoPInvokeCallback(typeof(LuaNativeFunction))]
	static int Get_normalized(IntPtr L)
	{
		ref var obj = ref SharpObject.GetValue<UnityEngine.Quaternion>(L, 1);
		Push(L, obj.normalized);
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
		Get(L, 0 + startStack, out UnityEngine.Vector3 t0);
		Get(L, 1 + startStack, out UnityEngine.Vector3 t1);
		var ret = UnityEngine.Quaternion.FromToRotation(t0, t1);
		Push(L, ret);
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
		Get(L, 0 + startStack, out UnityEngine.Quaternion t0);
		var ret = UnityEngine.Quaternion.Inverse(t0);
		Push(L, ret);
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
		Get(L, 0 + startStack, out UnityEngine.Quaternion t0);
		Get(L, 1 + startStack, out UnityEngine.Quaternion t1);
		Get(L, 2 + startStack, out float t2);
		var ret = UnityEngine.Quaternion.Slerp(t0, t1, t2);
		Push(L, ret);
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
		Get(L, 0 + startStack, out UnityEngine.Quaternion t0);
		Get(L, 1 + startStack, out UnityEngine.Quaternion t1);
		Get(L, 2 + startStack, out float t2);
		var ret = UnityEngine.Quaternion.SlerpUnclamped(t0, t1, t2);
		Push(L, ret);
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
		Get(L, 0 + startStack, out UnityEngine.Quaternion t0);
		Get(L, 1 + startStack, out UnityEngine.Quaternion t1);
		Get(L, 2 + startStack, out float t2);
		var ret = UnityEngine.Quaternion.Lerp(t0, t1, t2);
		Push(L, ret);
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
		Get(L, 0 + startStack, out UnityEngine.Quaternion t0);
		Get(L, 1 + startStack, out UnityEngine.Quaternion t1);
		Get(L, 2 + startStack, out float t2);
		var ret = UnityEngine.Quaternion.LerpUnclamped(t0, t1, t2);
		Push(L, ret);
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
		Get(L, 0 + startStack, out float t0);
		Get(L, 1 + startStack, out UnityEngine.Vector3 t1);
		var ret = UnityEngine.Quaternion.AngleAxis(t0, t1);
		Push(L, ret);
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
			Get(L, 0 + startStack, out UnityEngine.Vector3 t0);
			var ret = UnityEngine.Quaternion.LookRotation(t0);
			Push(L, ret);
			return 1;
		}
		else if(n == 2)
		{
			#if LUNA_SCRIPT
			const int startStack = 2;
			#else
			const int startStack = 1;
			#endif
			Get(L, 0 + startStack, out UnityEngine.Vector3 t0);
			Get(L, 1 + startStack, out UnityEngine.Vector3 t1);
			var ret = UnityEngine.Quaternion.LookRotation(t0, t1);
			Push(L, ret);
			return 1;
		}
		return 0;
	}

	[AOT.MonoPInvokeCallback(typeof(LuaNativeFunction))]
	static int Set(IntPtr L)
	{
		const int startStack = 2;
		ref var obj = ref SharpObject.GetValue<UnityEngine.Quaternion>(L, 1);
		Get(L, 0 + startStack, out float t0);
		Get(L, 1 + startStack, out float t1);
		Get(L, 2 + startStack, out float t2);
		Get(L, 3 + startStack, out float t3);
		obj.Set(t0, t1, t2, t3);
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
		Get(L, 0 + startStack, out UnityEngine.Quaternion t0);
		Get(L, 1 + startStack, out UnityEngine.Quaternion t1);
		var ret = UnityEngine.Quaternion.Dot(t0, t1);
		Push(L, ret);
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
			Get(L, 0 + startStack, out UnityEngine.Vector3 t0);
			obj.SetLookRotation(t0);
			return 0;
		}
		else if(n == 2)
		{
			const int startStack = 2;
			ref var obj = ref SharpObject.GetValue<UnityEngine.Quaternion>(L, 1);
			Get(L, 0 + startStack, out UnityEngine.Vector3 t0);
			Get(L, 1 + startStack, out UnityEngine.Vector3 t1);
			obj.SetLookRotation(t0, t1);
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
		Get(L, 0 + startStack, out UnityEngine.Quaternion t0);
		Get(L, 1 + startStack, out UnityEngine.Quaternion t1);
		var ret = UnityEngine.Quaternion.Angle(t0, t1);
		Push(L, ret);
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
			Get(L, 0 + startStack, out UnityEngine.Vector3 t0);
			var ret = UnityEngine.Quaternion.Euler(t0);
			Push(L, ret);
			return 1;
		}
		else if(n == 3)
		{
			#if LUNA_SCRIPT
			const int startStack = 2;
			#else
			const int startStack = 1;
			#endif
			Get(L, 0 + startStack, out float t0);
			Get(L, 1 + startStack, out float t1);
			Get(L, 2 + startStack, out float t2);
			var ret = UnityEngine.Quaternion.Euler(t0, t1, t2);
			Push(L, ret);
			return 1;
		}
		return 0;
	}

	[AOT.MonoPInvokeCallback(typeof(LuaNativeFunction))]
	static int SetFromToRotation(IntPtr L)
	{
		const int startStack = 2;
		ref var obj = ref SharpObject.GetValue<UnityEngine.Quaternion>(L, 1);
		Get(L, 0 + startStack, out UnityEngine.Vector3 t0);
		Get(L, 1 + startStack, out UnityEngine.Vector3 t1);
		obj.SetFromToRotation(t0, t1);
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
		Get(L, 0 + startStack, out UnityEngine.Quaternion t0);
		Get(L, 1 + startStack, out UnityEngine.Quaternion t1);
		Get(L, 2 + startStack, out float t2);
		var ret = UnityEngine.Quaternion.RotateTowards(t0, t1, t2);
		Push(L, ret);
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
			Get(L, 0 + startStack, out UnityEngine.Quaternion t0);
			var ret = UnityEngine.Quaternion.Normalize(t0);
			Push(L, ret);
			return 1;
		}
		return 0;
	}

	[AOT.MonoPInvokeCallback(typeof(LuaNativeFunction))]
	static int GetHashCode(IntPtr L)
	{
		ref var obj = ref SharpObject.GetValue<UnityEngine.Quaternion>(L, 1);
		var ret = obj.GetHashCode();
		Push(L, ret);
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
			Get(L, 0 + startStack, out object t0);
			var ret = obj.Equals(t0);
			Push(L, ret);
			return 1;
		}
		else if(n == 1)
		{
			const int startStack = 2;
			ref var obj = ref SharpObject.GetValue<UnityEngine.Quaternion>(L, 1);
			Get(L, 0 + startStack, out UnityEngine.Quaternion t0);
			var ret = obj.Equals(t0);
			Push(L, ret);
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
			Push(L, ret);
			return 1;
		}
		else if(n == 1)
		{
			const int startStack = 2;
			ref var obj = ref SharpObject.GetValue<UnityEngine.Quaternion>(L, 1);
			Get(L, 0 + startStack, out string t0);
			var ret = obj.ToString(t0);
			Push(L, ret);
			return 1;
		}
		return 0;
	}

	[AOT.MonoPInvokeCallback(typeof(LuaNativeFunction))]
	static int GetType(IntPtr L)
	{
		ref var obj = ref SharpObject.GetValue<UnityEngine.Quaternion>(L, 1);
		var ret = obj.GetType();
		Push(L, ret);
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