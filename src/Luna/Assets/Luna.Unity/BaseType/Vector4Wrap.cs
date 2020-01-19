using System;
using SharpLuna;
using static SharpLuna.Lua;

[WrapClass(typeof(UnityEngine.Vector4))]
public class Vector4Wrap
{
	[AOT.MonoPInvokeCallback(typeof(LuaNativeFunction))]
	static int Constructor(IntPtr L)
	{
		int n = lua_gettop(L) - 1;
		UnityEngine.Vector4 obj = default;
		if(n == 0)
		{
			obj = new UnityEngine.Vector4();
		}
		else if(n == 2)
		{
			Get(L, 2, out float t1);
			Get(L, 3, out float t2);
			obj = new UnityEngine.Vector4(t1, t2);
		}
		else if(n == 3)
		{
			Get(L, 2, out float t1);
			Get(L, 3, out float t2);
			Get(L, 4, out float t3);
			obj = new UnityEngine.Vector4(t1, t2, t3);
		}
		else if(n == 4)
		{
			Get(L, 2, out float t1);
			Get(L, 3, out float t2);
			Get(L, 4, out float t3);
			Get(L, 5, out float t4);
			obj = new UnityEngine.Vector4(t1, t2, t3, t4);
		}
		Push(L, obj);
		return 1;
	}

	[AOT.MonoPInvokeCallback(typeof(LuaNativeFunction))]
	static int Get_x(IntPtr L)
	{
		ref var obj = ref SharpObject.GetValue<UnityEngine.Vector4>(L, 1);
		Push(L, obj.x);
		return 1;
	}

	[AOT.MonoPInvokeCallback(typeof(LuaNativeFunction))]
	static int Set_x(IntPtr L)
	{
		ref var obj = ref SharpObject.GetValue<UnityEngine.Vector4>(L, 1);
		Get(L, 2, out float p1);
		obj.x = p1;
		return 0;
	}

	[AOT.MonoPInvokeCallback(typeof(LuaNativeFunction))]
	static int Get_y(IntPtr L)
	{
		ref var obj = ref SharpObject.GetValue<UnityEngine.Vector4>(L, 1);
		Push(L, obj.y);
		return 1;
	}

	[AOT.MonoPInvokeCallback(typeof(LuaNativeFunction))]
	static int Set_y(IntPtr L)
	{
		ref var obj = ref SharpObject.GetValue<UnityEngine.Vector4>(L, 1);
		Get(L, 2, out float p1);
		obj.y = p1;
		return 0;
	}

	[AOT.MonoPInvokeCallback(typeof(LuaNativeFunction))]
	static int Get_z(IntPtr L)
	{
		ref var obj = ref SharpObject.GetValue<UnityEngine.Vector4>(L, 1);
		Push(L, obj.z);
		return 1;
	}

	[AOT.MonoPInvokeCallback(typeof(LuaNativeFunction))]
	static int Set_z(IntPtr L)
	{
		ref var obj = ref SharpObject.GetValue<UnityEngine.Vector4>(L, 1);
		Get(L, 2, out float p1);
		obj.z = p1;
		return 0;
	}

	[AOT.MonoPInvokeCallback(typeof(LuaNativeFunction))]
	static int Get_w(IntPtr L)
	{
		ref var obj = ref SharpObject.GetValue<UnityEngine.Vector4>(L, 1);
		Push(L, obj.w);
		return 1;
	}

	[AOT.MonoPInvokeCallback(typeof(LuaNativeFunction))]
	static int Set_w(IntPtr L)
	{
		ref var obj = ref SharpObject.GetValue<UnityEngine.Vector4>(L, 1);
		Get(L, 2, out float p1);
		obj.w = p1;
		return 0;
	}

	[AOT.MonoPInvokeCallback(typeof(LuaNativeFunction))]
	static int Get_normalized(IntPtr L)
	{
		ref var obj = ref SharpObject.GetValue<UnityEngine.Vector4>(L, 1);
		Push(L, obj.normalized);
		return 1;
	}

	[AOT.MonoPInvokeCallback(typeof(LuaNativeFunction))]
	static int Get_magnitude(IntPtr L)
	{
		ref var obj = ref SharpObject.GetValue<UnityEngine.Vector4>(L, 1);
		Push(L, obj.magnitude);
		return 1;
	}

	[AOT.MonoPInvokeCallback(typeof(LuaNativeFunction))]
	static int Get_sqrMagnitude(IntPtr L)
	{
		ref var obj = ref SharpObject.GetValue<UnityEngine.Vector4>(L, 1);
		Push(L, obj.sqrMagnitude);
		return 1;
	}

	[AOT.MonoPInvokeCallback(typeof(LuaNativeFunction))]
	static int Get_zero(IntPtr L)
	{
		Push(L, UnityEngine.Vector4.zero);
		return 1;
	}

	[AOT.MonoPInvokeCallback(typeof(LuaNativeFunction))]
	static int Get_one(IntPtr L)
	{
		Push(L, UnityEngine.Vector4.one);
		return 1;
	}

	[AOT.MonoPInvokeCallback(typeof(LuaNativeFunction))]
	static int Get_positiveInfinity(IntPtr L)
	{
		Push(L, UnityEngine.Vector4.positiveInfinity);
		return 1;
	}

	[AOT.MonoPInvokeCallback(typeof(LuaNativeFunction))]
	static int Get_negativeInfinity(IntPtr L)
	{
		Push(L, UnityEngine.Vector4.negativeInfinity);
		return 1;
	}

	[AOT.MonoPInvokeCallback(typeof(LuaNativeFunction))]
	static int Set(IntPtr L)
	{
		const int startStack = 2;
		ref var obj = ref SharpObject.GetValue<UnityEngine.Vector4>(L, 1);
		Get(L, 0 + startStack, out float t0);
		Get(L, 1 + startStack, out float t1);
		Get(L, 2 + startStack, out float t2);
		Get(L, 3 + startStack, out float t3);
		obj.Set(t0, t1, t2, t3);
		return 0;
	}

	[AOT.MonoPInvokeCallback(typeof(LuaNativeFunction))]
	static int Lerp(IntPtr L)
	{
		#if LUNA_SCRIPT
		const int startStack = 2;
		#else
		const int startStack = 1;
		#endif
		Get(L, 0 + startStack, out UnityEngine.Vector4 t0);
		Get(L, 1 + startStack, out UnityEngine.Vector4 t1);
		Get(L, 2 + startStack, out float t2);
		var ret = UnityEngine.Vector4.Lerp(t0, t1, t2);
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
		Get(L, 0 + startStack, out UnityEngine.Vector4 t0);
		Get(L, 1 + startStack, out UnityEngine.Vector4 t1);
		Get(L, 2 + startStack, out float t2);
		var ret = UnityEngine.Vector4.LerpUnclamped(t0, t1, t2);
		Push(L, ret);
		return 1;
	}

	[AOT.MonoPInvokeCallback(typeof(LuaNativeFunction))]
	static int MoveTowards(IntPtr L)
	{
		#if LUNA_SCRIPT
		const int startStack = 2;
		#else
		const int startStack = 1;
		#endif
		Get(L, 0 + startStack, out UnityEngine.Vector4 t0);
		Get(L, 1 + startStack, out UnityEngine.Vector4 t1);
		Get(L, 2 + startStack, out float t2);
		var ret = UnityEngine.Vector4.MoveTowards(t0, t1, t2);
		Push(L, ret);
		return 1;
	}

	[AOT.MonoPInvokeCallback(typeof(LuaNativeFunction))]
	static int Scale(IntPtr L)
	{
		int n = lua_gettop(L) - 1;
		if(n == 1)
		{
			const int startStack = 2;
			ref var obj = ref SharpObject.GetValue<UnityEngine.Vector4>(L, 1);
			Get(L, 0 + startStack, out UnityEngine.Vector4 t0);
			obj.Scale(t0);
			return 0;
		}
		else if(n == 2)
		{
			#if LUNA_SCRIPT
			const int startStack = 2;
			#else
			const int startStack = 1;
			#endif
			Get(L, 0 + startStack, out UnityEngine.Vector4 t0);
			Get(L, 1 + startStack, out UnityEngine.Vector4 t1);
			var ret = UnityEngine.Vector4.Scale(t0, t1);
			Push(L, ret);
			return 1;
		}
		return 0;
	}

	[AOT.MonoPInvokeCallback(typeof(LuaNativeFunction))]
	static int GetHashCode(IntPtr L)
	{
		ref var obj = ref SharpObject.GetValue<UnityEngine.Vector4>(L, 1);
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
			ref var obj = ref SharpObject.GetValue<UnityEngine.Vector4>(L, 1);
			Get(L, 0 + startStack, out object t0);
			var ret = obj.Equals(t0);
			Push(L, ret);
			return 1;
		}
		else if(n == 1)
		{
			const int startStack = 2;
			ref var obj = ref SharpObject.GetValue<UnityEngine.Vector4>(L, 1);
			Get(L, 0 + startStack, out UnityEngine.Vector4 t0);
			var ret = obj.Equals(t0);
			Push(L, ret);
			return 1;
		}
		return 0;
	}

	[AOT.MonoPInvokeCallback(typeof(LuaNativeFunction))]
	static int Normalize(IntPtr L)
	{
		int n = lua_gettop(L) - 1;
		if(n == 0)
		{
			ref var obj = ref SharpObject.GetValue<UnityEngine.Vector4>(L, 1);
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
			Get(L, 0 + startStack, out UnityEngine.Vector4 t0);
			var ret = UnityEngine.Vector4.Normalize(t0);
			Push(L, ret);
			return 1;
		}
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
		Get(L, 0 + startStack, out UnityEngine.Vector4 t0);
		Get(L, 1 + startStack, out UnityEngine.Vector4 t1);
		var ret = UnityEngine.Vector4.Dot(t0, t1);
		Push(L, ret);
		return 1;
	}

	[AOT.MonoPInvokeCallback(typeof(LuaNativeFunction))]
	static int Project(IntPtr L)
	{
		#if LUNA_SCRIPT
		const int startStack = 2;
		#else
		const int startStack = 1;
		#endif
		Get(L, 0 + startStack, out UnityEngine.Vector4 t0);
		Get(L, 1 + startStack, out UnityEngine.Vector4 t1);
		var ret = UnityEngine.Vector4.Project(t0, t1);
		Push(L, ret);
		return 1;
	}

	[AOT.MonoPInvokeCallback(typeof(LuaNativeFunction))]
	static int Distance(IntPtr L)
	{
		#if LUNA_SCRIPT
		const int startStack = 2;
		#else
		const int startStack = 1;
		#endif
		Get(L, 0 + startStack, out UnityEngine.Vector4 t0);
		Get(L, 1 + startStack, out UnityEngine.Vector4 t1);
		var ret = UnityEngine.Vector4.Distance(t0, t1);
		Push(L, ret);
		return 1;
	}

	[AOT.MonoPInvokeCallback(typeof(LuaNativeFunction))]
	static int Magnitude(IntPtr L)
	{
		#if LUNA_SCRIPT
		const int startStack = 2;
		#else
		const int startStack = 1;
		#endif
		Get(L, 0 + startStack, out UnityEngine.Vector4 t0);
		var ret = UnityEngine.Vector4.Magnitude(t0);
		Push(L, ret);
		return 1;
	}

	[AOT.MonoPInvokeCallback(typeof(LuaNativeFunction))]
	static int Min(IntPtr L)
	{
		#if LUNA_SCRIPT
		const int startStack = 2;
		#else
		const int startStack = 1;
		#endif
		Get(L, 0 + startStack, out UnityEngine.Vector4 t0);
		Get(L, 1 + startStack, out UnityEngine.Vector4 t1);
		var ret = UnityEngine.Vector4.Min(t0, t1);
		Push(L, ret);
		return 1;
	}

	[AOT.MonoPInvokeCallback(typeof(LuaNativeFunction))]
	static int Max(IntPtr L)
	{
		#if LUNA_SCRIPT
		const int startStack = 2;
		#else
		const int startStack = 1;
		#endif
		Get(L, 0 + startStack, out UnityEngine.Vector4 t0);
		Get(L, 1 + startStack, out UnityEngine.Vector4 t1);
		var ret = UnityEngine.Vector4.Max(t0, t1);
		Push(L, ret);
		return 1;
	}

	[AOT.MonoPInvokeCallback(typeof(LuaNativeFunction))]
	static int ToString(IntPtr L)
	{
		int n = lua_gettop(L) - 1;
		if(n == 0)
		{
			ref var obj = ref SharpObject.GetValue<UnityEngine.Vector4>(L, 1);
			var ret = obj.ToString();
			Push(L, ret);
			return 1;
		}
		else if(n == 1)
		{
			const int startStack = 2;
			ref var obj = ref SharpObject.GetValue<UnityEngine.Vector4>(L, 1);
			Get(L, 0 + startStack, out string t0);
			var ret = obj.ToString(t0);
			Push(L, ret);
			return 1;
		}
		return 0;
	}

	[AOT.MonoPInvokeCallback(typeof(LuaNativeFunction))]
	static int SqrMagnitude(IntPtr L)
	{
		int n = lua_gettop(L) - 1;
		if(n == 0)
		{
			ref var obj = ref SharpObject.GetValue<UnityEngine.Vector4>(L, 1);
			var ret = obj.SqrMagnitude();
			Push(L, ret);
			return 1;
		}
		else if(n == 1)
		{
			#if LUNA_SCRIPT
			const int startStack = 2;
			#else
			const int startStack = 1;
			#endif
			Get(L, 0 + startStack, out UnityEngine.Vector4 t0);
			var ret = UnityEngine.Vector4.SqrMagnitude(t0);
			Push(L, ret);
			return 1;
		}
		return 0;
	}

	[AOT.MonoPInvokeCallback(typeof(LuaNativeFunction))]
	static int GetType(IntPtr L)
	{
		ref var obj = ref SharpObject.GetValue<UnityEngine.Vector4>(L, 1);
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
		classWraper.RegProperty("normalized", Get_normalized);
		classWraper.RegProperty("magnitude", Get_magnitude);
		classWraper.RegProperty("sqrMagnitude", Get_sqrMagnitude);
		classWraper.RegProperty("zero", Get_zero);
		classWraper.RegProperty("one", Get_one);
		classWraper.RegProperty("positiveInfinity", Get_positiveInfinity);
		classWraper.RegProperty("negativeInfinity", Get_negativeInfinity);
		classWraper.RegFunction("Set", Set);
		classWraper.RegFunction("Lerp", Lerp);
		classWraper.RegFunction("LerpUnclamped", LerpUnclamped);
		classWraper.RegFunction("MoveTowards", MoveTowards);
		classWraper.RegFunction("Scale", Scale);
		classWraper.RegFunction("GetHashCode", GetHashCode);
		classWraper.RegFunction("Equals", Equals);
		classWraper.RegFunction("Normalize", Normalize);
		classWraper.RegFunction("Dot", Dot);
		classWraper.RegFunction("Project", Project);
		classWraper.RegFunction("Distance", Distance);
		classWraper.RegFunction("Magnitude", Magnitude);
		classWraper.RegFunction("Min", Min);
		classWraper.RegFunction("Max", Max);
		classWraper.RegFunction("ToString", ToString);
		classWraper.RegFunction("SqrMagnitude", SqrMagnitude);
		classWraper.RegFunction("GetType", GetType);
	}
}