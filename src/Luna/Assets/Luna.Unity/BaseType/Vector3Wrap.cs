using System;
using SharpLuna;
using System.Collections.Generic;
using static SharpLuna.Lua;

[WrapClass(typeof(UnityEngine.Vector3))]
public class Vector3Wrap
{
	[AOT.MonoPInvokeCallback(typeof(LuaNativeFunction))]
	static int Constructor(IntPtr L)
	{
		int n = lua_gettop(L) - 1;
		UnityEngine.Vector3 obj = default;
		if(n == 0)
		{
			obj = new UnityEngine.Vector3();
		}
		else if(n == 2)
		{
			Get(L, 2, out float t1);
			Get(L, 3, out float t2);
			obj = new UnityEngine.Vector3(t1, t2);
		}
		else if(n == 3)
		{
			Get(L, 2, out float t1);
			Get(L, 3, out float t2);
			Get(L, 4, out float t3);
			obj = new UnityEngine.Vector3(t1, t2, t3);
		}
		Push(L, obj);
		return 1;
	}

	[AOT.MonoPInvokeCallback(typeof(LuaNativeFunction))]
	static int Get_x(IntPtr L)
	{
		ref var obj = ref SharpObject.GetValue<UnityEngine.Vector3>(L, 1);
		Push(L, obj.x);
		return 1;
	}

	[AOT.MonoPInvokeCallback(typeof(LuaNativeFunction))]
	static int Set_x(IntPtr L)
	{
		ref var obj = ref SharpObject.GetValue<UnityEngine.Vector3>(L, 1);
		Get(L, 2, out float p1);
		obj.x = p1;
		return 0;
	}

	[AOT.MonoPInvokeCallback(typeof(LuaNativeFunction))]
	static int Get_y(IntPtr L)
	{
		ref var obj = ref SharpObject.GetValue<UnityEngine.Vector3>(L, 1);
		Push(L, obj.y);
		return 1;
	}

	[AOT.MonoPInvokeCallback(typeof(LuaNativeFunction))]
	static int Set_y(IntPtr L)
	{
		ref var obj = ref SharpObject.GetValue<UnityEngine.Vector3>(L, 1);
		Get(L, 2, out float p1);
		obj.y = p1;
		return 0;
	}

	[AOT.MonoPInvokeCallback(typeof(LuaNativeFunction))]
	static int Get_z(IntPtr L)
	{
		ref var obj = ref SharpObject.GetValue<UnityEngine.Vector3>(L, 1);
		Push(L, obj.z);
		return 1;
	}

	[AOT.MonoPInvokeCallback(typeof(LuaNativeFunction))]
	static int Set_z(IntPtr L)
	{
		ref var obj = ref SharpObject.GetValue<UnityEngine.Vector3>(L, 1);
		Get(L, 2, out float p1);
		obj.z = p1;
		return 0;
	}

	[AOT.MonoPInvokeCallback(typeof(LuaNativeFunction))]
	static int Get_normalized(IntPtr L)
	{
		ref var obj = ref SharpObject.GetValue<UnityEngine.Vector3>(L, 1);
		Push(L, obj.normalized);
		return 1;
	}

	[AOT.MonoPInvokeCallback(typeof(LuaNativeFunction))]
	static int Get_magnitude(IntPtr L)
	{
		ref var obj = ref SharpObject.GetValue<UnityEngine.Vector3>(L, 1);
		Push(L, obj.magnitude);
		return 1;
	}

	[AOT.MonoPInvokeCallback(typeof(LuaNativeFunction))]
	static int Get_sqrMagnitude(IntPtr L)
	{
		ref var obj = ref SharpObject.GetValue<UnityEngine.Vector3>(L, 1);
		Push(L, obj.sqrMagnitude);
		return 1;
	}

	[AOT.MonoPInvokeCallback(typeof(LuaNativeFunction))]
	static int Get_zero(IntPtr L)
	{
		Push(L, UnityEngine.Vector3.zero);
		return 1;
	}

	[AOT.MonoPInvokeCallback(typeof(LuaNativeFunction))]
	static int Get_one(IntPtr L)
	{
		Push(L, UnityEngine.Vector3.one);
		return 1;
	}

	[AOT.MonoPInvokeCallback(typeof(LuaNativeFunction))]
	static int Get_forward(IntPtr L)
	{
		Push(L, UnityEngine.Vector3.forward);
		return 1;
	}

	[AOT.MonoPInvokeCallback(typeof(LuaNativeFunction))]
	static int Get_back(IntPtr L)
	{
		Push(L, UnityEngine.Vector3.back);
		return 1;
	}

	[AOT.MonoPInvokeCallback(typeof(LuaNativeFunction))]
	static int Get_up(IntPtr L)
	{
		Push(L, UnityEngine.Vector3.up);
		return 1;
	}

	[AOT.MonoPInvokeCallback(typeof(LuaNativeFunction))]
	static int Get_down(IntPtr L)
	{
		Push(L, UnityEngine.Vector3.down);
		return 1;
	}

	[AOT.MonoPInvokeCallback(typeof(LuaNativeFunction))]
	static int Get_left(IntPtr L)
	{
		Push(L, UnityEngine.Vector3.left);
		return 1;
	}

	[AOT.MonoPInvokeCallback(typeof(LuaNativeFunction))]
	static int Get_right(IntPtr L)
	{
		Push(L, UnityEngine.Vector3.right);
		return 1;
	}

	[AOT.MonoPInvokeCallback(typeof(LuaNativeFunction))]
	static int Get_positiveInfinity(IntPtr L)
	{
		Push(L, UnityEngine.Vector3.positiveInfinity);
		return 1;
	}

	[AOT.MonoPInvokeCallback(typeof(LuaNativeFunction))]
	static int Get_negativeInfinity(IntPtr L)
	{
		Push(L, UnityEngine.Vector3.negativeInfinity);
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
		Get(L, 0 + startStack, out UnityEngine.Vector3 t0);
		Get(L, 1 + startStack, out UnityEngine.Vector3 t1);
		Get(L, 2 + startStack, out float t2);
		UnityEngine.Vector3 ret = UnityEngine.Vector3.Slerp(t0, t1, t2);
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
		Get(L, 0 + startStack, out UnityEngine.Vector3 t0);
		Get(L, 1 + startStack, out UnityEngine.Vector3 t1);
		Get(L, 2 + startStack, out float t2);
		UnityEngine.Vector3 ret = UnityEngine.Vector3.SlerpUnclamped(t0, t1, t2);
		Push(L, ret);
		return 1;
	}

	[AOT.MonoPInvokeCallback(typeof(LuaNativeFunction))]
	static int RotateTowards(IntPtr L)
	{
		#if LUNA_SCRIPT
		const int startStack = 2;
		#else
		const int startStack = 1;
		#endif
		Get(L, 0 + startStack, out UnityEngine.Vector3 t0);
		Get(L, 1 + startStack, out UnityEngine.Vector3 t1);
		Get(L, 2 + startStack, out float t2);
		Get(L, 3 + startStack, out float t3);
		UnityEngine.Vector3 ret = UnityEngine.Vector3.RotateTowards(t0, t1, t2, t3);
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
		Get(L, 0 + startStack, out UnityEngine.Vector3 t0);
		Get(L, 1 + startStack, out UnityEngine.Vector3 t1);
		Get(L, 2 + startStack, out float t2);
		UnityEngine.Vector3 ret = UnityEngine.Vector3.Lerp(t0, t1, t2);
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
		Get(L, 0 + startStack, out UnityEngine.Vector3 t0);
		Get(L, 1 + startStack, out UnityEngine.Vector3 t1);
		Get(L, 2 + startStack, out float t2);
		UnityEngine.Vector3 ret = UnityEngine.Vector3.LerpUnclamped(t0, t1, t2);
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
		Get(L, 0 + startStack, out UnityEngine.Vector3 t0);
		Get(L, 1 + startStack, out UnityEngine.Vector3 t1);
		Get(L, 2 + startStack, out float t2);
		UnityEngine.Vector3 ret = UnityEngine.Vector3.MoveTowards(t0, t1, t2);
		Push(L, ret);
		return 1;
	}

	[AOT.MonoPInvokeCallback(typeof(LuaNativeFunction))]
	static int Set(IntPtr L)
	{
		const int startStack = 2;
		ref var obj = ref SharpObject.GetValue<UnityEngine.Vector3>(L, 1);
		Get(L, 0 + startStack, out float t0);
		Get(L, 1 + startStack, out float t1);
		Get(L, 2 + startStack, out float t2);
		obj.Set(t0, t1, t2);
		return 0;
	}

	[AOT.MonoPInvokeCallback(typeof(LuaNativeFunction))]
	static int Scale(IntPtr L)
	{
		int n = lua_gettop(L) - 1;
		if(n == 1)
		{
			const int startStack = 2;
			ref var obj = ref SharpObject.GetValue<UnityEngine.Vector3>(L, 1);
			Get(L, 0 + startStack, out UnityEngine.Vector3 t0);
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
			Get(L, 0 + startStack, out UnityEngine.Vector3 t0);
			Get(L, 1 + startStack, out UnityEngine.Vector3 t1);
			UnityEngine.Vector3 ret = UnityEngine.Vector3.Scale(t0, t1);
			Push(L, ret);
			return 1;
		}
		return 0;
	}

	[AOT.MonoPInvokeCallback(typeof(LuaNativeFunction))]
	static int Cross(IntPtr L)
	{
		#if LUNA_SCRIPT
		const int startStack = 2;
		#else
		const int startStack = 1;
		#endif
		Get(L, 0 + startStack, out UnityEngine.Vector3 t0);
		Get(L, 1 + startStack, out UnityEngine.Vector3 t1);
		UnityEngine.Vector3 ret = UnityEngine.Vector3.Cross(t0, t1);
		Push(L, ret);
		return 1;
	}

	[AOT.MonoPInvokeCallback(typeof(LuaNativeFunction))]
	static int GetHashCode(IntPtr L)
	{
		ref var obj = ref SharpObject.GetValue<UnityEngine.Vector3>(L, 1);
		int ret = obj.GetHashCode();
		Push(L, ret);
		return 1;
	}

	[AOT.MonoPInvokeCallback(typeof(LuaNativeFunction))]
	static int Equals(IntPtr L)
	{
		int n = lua_gettop(L) - 1;
		if(n == 1 && CheckType<object>(L, 1))
		{
			const int startStack = 2;
			ref var obj = ref SharpObject.GetValue<UnityEngine.Vector3>(L, 1);
			Get(L, 0 + startStack, out object t0);
			bool ret = obj.Equals(t0);
			Push(L, ret);
			return 1;
		}
		else if(n == 1 && CheckType<UnityEngine.Vector3>(L, 1))
		{
			const int startStack = 2;
			ref var obj = ref SharpObject.GetValue<UnityEngine.Vector3>(L, 1);
			Get(L, 0 + startStack, out UnityEngine.Vector3 t0);
			bool ret = obj.Equals(t0);
			Push(L, ret);
			return 1;
		}
		return 0;
	}

	[AOT.MonoPInvokeCallback(typeof(LuaNativeFunction))]
	static int Reflect(IntPtr L)
	{
		#if LUNA_SCRIPT
		const int startStack = 2;
		#else
		const int startStack = 1;
		#endif
		Get(L, 0 + startStack, out UnityEngine.Vector3 t0);
		Get(L, 1 + startStack, out UnityEngine.Vector3 t1);
		UnityEngine.Vector3 ret = UnityEngine.Vector3.Reflect(t0, t1);
		Push(L, ret);
		return 1;
	}

	[AOT.MonoPInvokeCallback(typeof(LuaNativeFunction))]
	static int Normalize(IntPtr L)
	{
		int n = lua_gettop(L) - 1;
		if(n == 0)
		{
			ref var obj = ref SharpObject.GetValue<UnityEngine.Vector3>(L, 1);
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
			Get(L, 0 + startStack, out UnityEngine.Vector3 t0);
			UnityEngine.Vector3 ret = UnityEngine.Vector3.Normalize(t0);
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
		Get(L, 0 + startStack, out UnityEngine.Vector3 t0);
		Get(L, 1 + startStack, out UnityEngine.Vector3 t1);
		float ret = UnityEngine.Vector3.Dot(t0, t1);
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
		Get(L, 0 + startStack, out UnityEngine.Vector3 t0);
		Get(L, 1 + startStack, out UnityEngine.Vector3 t1);
		UnityEngine.Vector3 ret = UnityEngine.Vector3.Project(t0, t1);
		Push(L, ret);
		return 1;
	}

	[AOT.MonoPInvokeCallback(typeof(LuaNativeFunction))]
	static int ProjectOnPlane(IntPtr L)
	{
		#if LUNA_SCRIPT
		const int startStack = 2;
		#else
		const int startStack = 1;
		#endif
		Get(L, 0 + startStack, out UnityEngine.Vector3 t0);
		Get(L, 1 + startStack, out UnityEngine.Vector3 t1);
		UnityEngine.Vector3 ret = UnityEngine.Vector3.ProjectOnPlane(t0, t1);
		Push(L, ret);
		return 1;
	}

	[AOT.MonoPInvokeCallback(typeof(LuaNativeFunction))]
	static int Angle(IntPtr L)
	{
		#if LUNA_SCRIPT
		const int startStack = 2;
		#else
		const int startStack = 1;
		#endif
		Get(L, 0 + startStack, out UnityEngine.Vector3 t0);
		Get(L, 1 + startStack, out UnityEngine.Vector3 t1);
		float ret = UnityEngine.Vector3.Angle(t0, t1);
		Push(L, ret);
		return 1;
	}

	[AOT.MonoPInvokeCallback(typeof(LuaNativeFunction))]
	static int SignedAngle(IntPtr L)
	{
		#if LUNA_SCRIPT
		const int startStack = 2;
		#else
		const int startStack = 1;
		#endif
		Get(L, 0 + startStack, out UnityEngine.Vector3 t0);
		Get(L, 1 + startStack, out UnityEngine.Vector3 t1);
		Get(L, 2 + startStack, out UnityEngine.Vector3 t2);
		float ret = UnityEngine.Vector3.SignedAngle(t0, t1, t2);
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
		Get(L, 0 + startStack, out UnityEngine.Vector3 t0);
		Get(L, 1 + startStack, out UnityEngine.Vector3 t1);
		float ret = UnityEngine.Vector3.Distance(t0, t1);
		Push(L, ret);
		return 1;
	}

	[AOT.MonoPInvokeCallback(typeof(LuaNativeFunction))]
	static int ClampMagnitude(IntPtr L)
	{
		#if LUNA_SCRIPT
		const int startStack = 2;
		#else
		const int startStack = 1;
		#endif
		Get(L, 0 + startStack, out UnityEngine.Vector3 t0);
		Get(L, 1 + startStack, out float t1);
		UnityEngine.Vector3 ret = UnityEngine.Vector3.ClampMagnitude(t0, t1);
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
		Get(L, 0 + startStack, out UnityEngine.Vector3 t0);
		float ret = UnityEngine.Vector3.Magnitude(t0);
		Push(L, ret);
		return 1;
	}

	[AOT.MonoPInvokeCallback(typeof(LuaNativeFunction))]
	static int SqrMagnitude(IntPtr L)
	{
		#if LUNA_SCRIPT
		const int startStack = 2;
		#else
		const int startStack = 1;
		#endif
		Get(L, 0 + startStack, out UnityEngine.Vector3 t0);
		float ret = UnityEngine.Vector3.SqrMagnitude(t0);
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
		Get(L, 0 + startStack, out UnityEngine.Vector3 t0);
		Get(L, 1 + startStack, out UnityEngine.Vector3 t1);
		UnityEngine.Vector3 ret = UnityEngine.Vector3.Min(t0, t1);
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
		Get(L, 0 + startStack, out UnityEngine.Vector3 t0);
		Get(L, 1 + startStack, out UnityEngine.Vector3 t1);
		UnityEngine.Vector3 ret = UnityEngine.Vector3.Max(t0, t1);
		Push(L, ret);
		return 1;
	}

	[AOT.MonoPInvokeCallback(typeof(LuaNativeFunction))]
	static int ToString(IntPtr L)
	{
		int n = lua_gettop(L) - 1;
		if(n == 0)
		{
			ref var obj = ref SharpObject.GetValue<UnityEngine.Vector3>(L, 1);
			string ret = obj.ToString();
			Push(L, ret);
			return 1;
		}
		else if(n == 1)
		{
			const int startStack = 2;
			ref var obj = ref SharpObject.GetValue<UnityEngine.Vector3>(L, 1);
			Get(L, 0 + startStack, out string t0);
			string ret = obj.ToString(t0);
			Push(L, ret);
			return 1;
		}
		return 0;
	}

	[AOT.MonoPInvokeCallback(typeof(LuaNativeFunction))]
	static int GetType(IntPtr L)
	{
		ref var obj = ref SharpObject.GetValue<UnityEngine.Vector3>(L, 1);
		System.Type ret = obj.GetType();
		Push(L, ret);
		return 1;
	}

	public static void Register(ClassWraper classWraper)
	{
		classWraper.RegConstructor(Constructor);
		classWraper.RegField("x", Get_x, Set_x);
		classWraper.RegField("y", Get_y, Set_y);
		classWraper.RegField("z", Get_z, Set_z);
		classWraper.RegProperty("normalized", Get_normalized);
		classWraper.RegProperty("magnitude", Get_magnitude);
		classWraper.RegProperty("sqrMagnitude", Get_sqrMagnitude);
		classWraper.RegProperty("zero", Get_zero);
		classWraper.RegProperty("one", Get_one);
		classWraper.RegProperty("forward", Get_forward);
		classWraper.RegProperty("back", Get_back);
		classWraper.RegProperty("up", Get_up);
		classWraper.RegProperty("down", Get_down);
		classWraper.RegProperty("left", Get_left);
		classWraper.RegProperty("right", Get_right);
		classWraper.RegProperty("positiveInfinity", Get_positiveInfinity);
		classWraper.RegProperty("negativeInfinity", Get_negativeInfinity);
		classWraper.RegFunction("Slerp", Slerp);
		classWraper.RegFunction("SlerpUnclamped", SlerpUnclamped);
		classWraper.RegFunction("RotateTowards", RotateTowards);
		classWraper.RegFunction("Lerp", Lerp);
		classWraper.RegFunction("LerpUnclamped", LerpUnclamped);
		classWraper.RegFunction("MoveTowards", MoveTowards);
		classWraper.RegFunction("Set", Set);
		classWraper.RegFunction("Scale", Scale);
		classWraper.RegFunction("Cross", Cross);
		classWraper.RegFunction("GetHashCode", GetHashCode);
		classWraper.RegFunction("Equals", Equals);
		classWraper.RegFunction("Reflect", Reflect);
		classWraper.RegFunction("Normalize", Normalize);
		classWraper.RegFunction("Dot", Dot);
		classWraper.RegFunction("Project", Project);
		classWraper.RegFunction("ProjectOnPlane", ProjectOnPlane);
		classWraper.RegFunction("Angle", Angle);
		classWraper.RegFunction("SignedAngle", SignedAngle);
		classWraper.RegFunction("Distance", Distance);
		classWraper.RegFunction("ClampMagnitude", ClampMagnitude);
		classWraper.RegFunction("Magnitude", Magnitude);
		classWraper.RegFunction("SqrMagnitude", SqrMagnitude);
		classWraper.RegFunction("Min", Min);
		classWraper.RegFunction("Max", Max);
		classWraper.RegFunction("ToString", ToString);
		classWraper.RegFunction("GetType", GetType);
	}
}