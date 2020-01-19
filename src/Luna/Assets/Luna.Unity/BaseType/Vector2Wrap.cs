using System;
using SharpLuna;
using static SharpLuna.Lua;

[WrapClass(typeof(UnityEngine.Vector2))]
public class Vector2Wrap
{
	[AOT.MonoPInvokeCallback(typeof(LuaNativeFunction))]
	static int Constructor(IntPtr L)
	{
		int n = lua_gettop(L) - 1;
		UnityEngine.Vector2 obj = default;
		if(n == 0)
		{
			obj = new UnityEngine.Vector2();
		}
		else if(n == 2)
		{
			obj = new UnityEngine.Vector2(
				Lua.Get<float>(L, 2),
				Lua.Get<float>(L, 3)
			);
		}
		Lua.Push(L, obj);
		return 1;
	}

	[AOT.MonoPInvokeCallback(typeof(LuaNativeFunction))]
	static int Get_x(IntPtr L)
	{
		ref var obj = ref SharpObject.GetValue<UnityEngine.Vector2>(L, 1);
		Lua.Push(L, obj.x);
		return 1;
	}

	[AOT.MonoPInvokeCallback(typeof(LuaNativeFunction))]
	static int Set_x(IntPtr L)
	{
		ref var obj = ref SharpObject.GetValue<UnityEngine.Vector2>(L, 1);
		var p1 = Lua.Get<float>(L, 2);
		obj.x = p1;
		return 0;
	}

	[AOT.MonoPInvokeCallback(typeof(LuaNativeFunction))]
	static int Get_y(IntPtr L)
	{
		ref var obj = ref SharpObject.GetValue<UnityEngine.Vector2>(L, 1);
		Lua.Push(L, obj.y);
		return 1;
	}

	[AOT.MonoPInvokeCallback(typeof(LuaNativeFunction))]
	static int Set_y(IntPtr L)
	{
		ref var obj = ref SharpObject.GetValue<UnityEngine.Vector2>(L, 1);
		var p1 = Lua.Get<float>(L, 2);
		obj.y = p1;
		return 0;
	}

	[AOT.MonoPInvokeCallback(typeof(LuaNativeFunction))]
	static int Get_normalized(IntPtr L)
	{
		ref var obj = ref SharpObject.GetValue<UnityEngine.Vector2>(L, 1);
		Lua.Push(L, obj.normalized);
		return 1;
	}

	[AOT.MonoPInvokeCallback(typeof(LuaNativeFunction))]
	static int Get_magnitude(IntPtr L)
	{
		ref var obj = ref SharpObject.GetValue<UnityEngine.Vector2>(L, 1);
		Lua.Push(L, obj.magnitude);
		return 1;
	}

	[AOT.MonoPInvokeCallback(typeof(LuaNativeFunction))]
	static int Get_sqrMagnitude(IntPtr L)
	{
		ref var obj = ref SharpObject.GetValue<UnityEngine.Vector2>(L, 1);
		Lua.Push(L, obj.sqrMagnitude);
		return 1;
	}

	[AOT.MonoPInvokeCallback(typeof(LuaNativeFunction))]
	static int Get_zero(IntPtr L)
	{
		Lua.Push(L, UnityEngine.Vector2.zero);
		return 1;
	}

	[AOT.MonoPInvokeCallback(typeof(LuaNativeFunction))]
	static int Get_one(IntPtr L)
	{
		Lua.Push(L, UnityEngine.Vector2.one);
		return 1;
	}

	[AOT.MonoPInvokeCallback(typeof(LuaNativeFunction))]
	static int Get_up(IntPtr L)
	{
		Lua.Push(L, UnityEngine.Vector2.up);
		return 1;
	}

	[AOT.MonoPInvokeCallback(typeof(LuaNativeFunction))]
	static int Get_down(IntPtr L)
	{
		Lua.Push(L, UnityEngine.Vector2.down);
		return 1;
	}

	[AOT.MonoPInvokeCallback(typeof(LuaNativeFunction))]
	static int Get_left(IntPtr L)
	{
		Lua.Push(L, UnityEngine.Vector2.left);
		return 1;
	}

	[AOT.MonoPInvokeCallback(typeof(LuaNativeFunction))]
	static int Get_right(IntPtr L)
	{
		Lua.Push(L, UnityEngine.Vector2.right);
		return 1;
	}

	[AOT.MonoPInvokeCallback(typeof(LuaNativeFunction))]
	static int Get_positiveInfinity(IntPtr L)
	{
		Lua.Push(L, UnityEngine.Vector2.positiveInfinity);
		return 1;
	}

	[AOT.MonoPInvokeCallback(typeof(LuaNativeFunction))]
	static int Get_negativeInfinity(IntPtr L)
	{
		Lua.Push(L, UnityEngine.Vector2.negativeInfinity);
		return 1;
	}

	[AOT.MonoPInvokeCallback(typeof(LuaNativeFunction))]
	static int Set(IntPtr L)
	{
		const int startStack = 2;
		ref var obj = ref SharpObject.GetValue<UnityEngine.Vector2>(L, 1);
		obj.Set(
			Lua.Get<float>(L, 0 + startStack),
			Lua.Get<float>(L, 1 + startStack)
		);
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
		var ret = UnityEngine.Vector2.Lerp(
			Lua.Get<UnityEngine.Vector2>(L, 0 + startStack),
			Lua.Get<UnityEngine.Vector2>(L, 1 + startStack),
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
		var ret = UnityEngine.Vector2.LerpUnclamped(
			Lua.Get<UnityEngine.Vector2>(L, 0 + startStack),
			Lua.Get<UnityEngine.Vector2>(L, 1 + startStack),
			Lua.Get<float>(L, 2 + startStack)
		);
		Lua.Push(L, ret);
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
		var ret = UnityEngine.Vector2.MoveTowards(
			Lua.Get<UnityEngine.Vector2>(L, 0 + startStack),
			Lua.Get<UnityEngine.Vector2>(L, 1 + startStack),
			Lua.Get<float>(L, 2 + startStack)
		);
		Lua.Push(L, ret);
		return 1;
	}

	[AOT.MonoPInvokeCallback(typeof(LuaNativeFunction))]
	static int Scale(IntPtr L)
	{
		int n = lua_gettop(L) - 1;
		if(n == 1)
		{
			const int startStack = 2;
			ref var obj = ref SharpObject.GetValue<UnityEngine.Vector2>(L, 1);
			obj.Scale(
				Lua.Get<UnityEngine.Vector2>(L, 0 + startStack)
			);
			return 0;
		}
		else if(n == 2)
		{
			#if LUNA_SCRIPT
			const int startStack = 2;
			#else
			const int startStack = 1;
			#endif
			var ret = UnityEngine.Vector2.Scale(
				Lua.Get<UnityEngine.Vector2>(L, 0 + startStack),
				Lua.Get<UnityEngine.Vector2>(L, 1 + startStack)
			);
			Lua.Push(L, ret);
			return 1;
		}
		return 0;
	}

	[AOT.MonoPInvokeCallback(typeof(LuaNativeFunction))]
	static int Normalize(IntPtr L)
	{
		ref var obj = ref SharpObject.GetValue<UnityEngine.Vector2>(L, 1);
		obj.Normalize();
		return 0;
	}

	[AOT.MonoPInvokeCallback(typeof(LuaNativeFunction))]
	static int ToString(IntPtr L)
	{
		int n = lua_gettop(L) - 1;
		if(n == 0)
		{
			ref var obj = ref SharpObject.GetValue<UnityEngine.Vector2>(L, 1);
			var ret = obj.ToString();
			Lua.Push(L, ret);
			return 1;
		}
		else if(n == 1)
		{
			const int startStack = 2;
			ref var obj = ref SharpObject.GetValue<UnityEngine.Vector2>(L, 1);
			var ret = obj.ToString(
				Lua.Get<string>(L, 0 + startStack)
			);
			Lua.Push(L, ret);
			return 1;
		}
		return 0;
	}

	[AOT.MonoPInvokeCallback(typeof(LuaNativeFunction))]
	static int GetHashCode(IntPtr L)
	{
		ref var obj = ref SharpObject.GetValue<UnityEngine.Vector2>(L, 1);
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
			ref var obj = ref SharpObject.GetValue<UnityEngine.Vector2>(L, 1);
			var ret = obj.Equals(
				Lua.Get<object>(L, 0 + startStack)
			);
			Lua.Push(L, ret);
			return 1;
		}
		else if(n == 1)
		{
			const int startStack = 2;
			ref var obj = ref SharpObject.GetValue<UnityEngine.Vector2>(L, 1);
			var ret = obj.Equals(
				Lua.Get<UnityEngine.Vector2>(L, 0 + startStack)
			);
			Lua.Push(L, ret);
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
		var ret = UnityEngine.Vector2.Reflect(
			Lua.Get<UnityEngine.Vector2>(L, 0 + startStack),
			Lua.Get<UnityEngine.Vector2>(L, 1 + startStack)
		);
		Lua.Push(L, ret);
		return 1;
	}

	[AOT.MonoPInvokeCallback(typeof(LuaNativeFunction))]
	static int Perpendicular(IntPtr L)
	{
		#if LUNA_SCRIPT
		const int startStack = 2;
		#else
		const int startStack = 1;
		#endif
		var ret = UnityEngine.Vector2.Perpendicular(
			Lua.Get<UnityEngine.Vector2>(L, 0 + startStack)
		);
		Lua.Push(L, ret);
		return 1;
	}

	[AOT.MonoPInvokeCallback(typeof(LuaNativeFunction))]
	static int Dot(IntPtr L)
	{
		#if LUNA_SCRIPT
		const int startStack = 2;
		#else
		const int startStack = 1;
		#endif
		var ret = UnityEngine.Vector2.Dot(
			Lua.Get<UnityEngine.Vector2>(L, 0 + startStack),
			Lua.Get<UnityEngine.Vector2>(L, 1 + startStack)
		);
		Lua.Push(L, ret);
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
		var ret = UnityEngine.Vector2.Angle(
			Lua.Get<UnityEngine.Vector2>(L, 0 + startStack),
			Lua.Get<UnityEngine.Vector2>(L, 1 + startStack)
		);
		Lua.Push(L, ret);
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
		var ret = UnityEngine.Vector2.SignedAngle(
			Lua.Get<UnityEngine.Vector2>(L, 0 + startStack),
			Lua.Get<UnityEngine.Vector2>(L, 1 + startStack)
		);
		Lua.Push(L, ret);
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
		var ret = UnityEngine.Vector2.Distance(
			Lua.Get<UnityEngine.Vector2>(L, 0 + startStack),
			Lua.Get<UnityEngine.Vector2>(L, 1 + startStack)
		);
		Lua.Push(L, ret);
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
		var ret = UnityEngine.Vector2.ClampMagnitude(
			Lua.Get<UnityEngine.Vector2>(L, 0 + startStack),
			Lua.Get<float>(L, 1 + startStack)
		);
		Lua.Push(L, ret);
		return 1;
	}

	[AOT.MonoPInvokeCallback(typeof(LuaNativeFunction))]
	static int SqrMagnitude(IntPtr L)
	{
		int n = lua_gettop(L) - 1;
		if(n == 0)
		{
			ref var obj = ref SharpObject.GetValue<UnityEngine.Vector2>(L, 1);
			var ret = obj.SqrMagnitude();
			Lua.Push(L, ret);
			return 1;
		}
		else if(n == 1)
		{
			#if LUNA_SCRIPT
			const int startStack = 2;
			#else
			const int startStack = 1;
			#endif
			var ret = UnityEngine.Vector2.SqrMagnitude(
				Lua.Get<UnityEngine.Vector2>(L, 0 + startStack)
			);
			Lua.Push(L, ret);
			return 1;
		}
		return 0;
	}

	[AOT.MonoPInvokeCallback(typeof(LuaNativeFunction))]
	static int Min(IntPtr L)
	{
		#if LUNA_SCRIPT
		const int startStack = 2;
		#else
		const int startStack = 1;
		#endif
		var ret = UnityEngine.Vector2.Min(
			Lua.Get<UnityEngine.Vector2>(L, 0 + startStack),
			Lua.Get<UnityEngine.Vector2>(L, 1 + startStack)
		);
		Lua.Push(L, ret);
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
		var ret = UnityEngine.Vector2.Max(
			Lua.Get<UnityEngine.Vector2>(L, 0 + startStack),
			Lua.Get<UnityEngine.Vector2>(L, 1 + startStack)
		);
		Lua.Push(L, ret);
		return 1;
	}

	[AOT.MonoPInvokeCallback(typeof(LuaNativeFunction))]
	static int GetType(IntPtr L)
	{
		ref var obj = ref SharpObject.GetValue<UnityEngine.Vector2>(L, 1);
		var ret = obj.GetType();
		Lua.Push(L, ret);
		return 1;
	}

	public static void Register(ClassWraper classWraper)
	{
		classWraper.RegConstructor(Constructor);
		classWraper.RegField("x", Get_x, Set_x);
		classWraper.RegField("y", Get_y, Set_y);
		classWraper.RegProperty("normalized", Get_normalized);
		classWraper.RegProperty("magnitude", Get_magnitude);
		classWraper.RegProperty("sqrMagnitude", Get_sqrMagnitude);
		classWraper.RegProperty("zero", Get_zero);
		classWraper.RegProperty("one", Get_one);
		classWraper.RegProperty("up", Get_up);
		classWraper.RegProperty("down", Get_down);
		classWraper.RegProperty("left", Get_left);
		classWraper.RegProperty("right", Get_right);
		classWraper.RegProperty("positiveInfinity", Get_positiveInfinity);
		classWraper.RegProperty("negativeInfinity", Get_negativeInfinity);
		classWraper.RegFunction("Set", Set);
		classWraper.RegFunction("Lerp", Lerp);
		classWraper.RegFunction("LerpUnclamped", LerpUnclamped);
		classWraper.RegFunction("MoveTowards", MoveTowards);
		classWraper.RegFunction("Scale", Scale);
		classWraper.RegFunction("Normalize", Normalize);
		classWraper.RegFunction("ToString", ToString);
		classWraper.RegFunction("GetHashCode", GetHashCode);
		classWraper.RegFunction("Equals", Equals);
		classWraper.RegFunction("Reflect", Reflect);
		classWraper.RegFunction("Perpendicular", Perpendicular);
		classWraper.RegFunction("Dot", Dot);
		classWraper.RegFunction("Angle", Angle);
		classWraper.RegFunction("SignedAngle", SignedAngle);
		classWraper.RegFunction("Distance", Distance);
		classWraper.RegFunction("ClampMagnitude", ClampMagnitude);
		classWraper.RegFunction("SqrMagnitude", SqrMagnitude);
		classWraper.RegFunction("Min", Min);
		classWraper.RegFunction("Max", Max);
		classWraper.RegFunction("GetType", GetType);
	}
}