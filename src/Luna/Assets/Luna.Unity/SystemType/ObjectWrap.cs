using System;
using SharpLuna;
using System.Collections.Generic;
using static SharpLuna.Lua;

[WrapClass(typeof(System.Object))]
public class ObjectWrap
{
	[AOT.MonoPInvokeCallback(typeof(LuaNativeFunction))]
	static int Constructor(IntPtr L)
	{
		int n = lua_gettop(L) - 1;
		System.Object obj = default;
		if(n == 0)
		{
			obj = new System.Object();
		}
		Push(L, obj);
		return 1;
	}

	[AOT.MonoPInvokeCallback(typeof(LuaNativeFunction))]
	static int Equals(IntPtr L)
	{
		int n = lua_gettop(L) - 1;
		if(n == 1)
		{
			const int startStack = 2;
			var obj = SharpObject.Get<System.Object>(L, 1);
			Get(L, 0 + startStack, out object t0);
			bool ret = obj.Equals(t0);
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
			Get(L, 0 + startStack, out object t0);
			Get(L, 1 + startStack, out object t1);
			bool ret = System.Object.Equals(t0, t1);
			Push(L, ret);
			return 1;
		}
		return 0;
	}

	[AOT.MonoPInvokeCallback(typeof(LuaNativeFunction))]
	static int GetHashCode(IntPtr L)
	{
		var obj = SharpObject.Get<System.Object>(L, 1);
		int ret = obj.GetHashCode();
		Push(L, ret);
		return 1;
	}

	[AOT.MonoPInvokeCallback(typeof(LuaNativeFunction))]
	static int GetType(IntPtr L)
	{
		var obj = SharpObject.Get<System.Object>(L, 1);
		System.Type ret = obj.GetType();
		Push(L, ret);
		return 1;
	}

	[AOT.MonoPInvokeCallback(typeof(LuaNativeFunction))]
	static int ToString(IntPtr L)
	{
		var obj = SharpObject.Get<System.Object>(L, 1);
		string ret = obj.ToString();
		Push(L, ret);
		return 1;
	}

	[AOT.MonoPInvokeCallback(typeof(LuaNativeFunction))]
	static int ReferenceEquals(IntPtr L)
	{
		#if LUNA_SCRIPT
		const int startStack = 2;
		#else
		const int startStack = 1;
		#endif
		Get(L, 0 + startStack, out object t0);
		Get(L, 1 + startStack, out object t1);
		bool ret = System.Object.ReferenceEquals(t0, t1);
		Push(L, ret);
		return 1;
	}

	public static void Register(ClassWraper classWraper)
	{
		classWraper.RegConstructor(Constructor);
		classWraper.RegFunction("Equals", Equals);
		classWraper.RegFunction("GetHashCode", GetHashCode);
		classWraper.RegFunction("GetType", GetType);
		classWraper.RegFunction("ToString", ToString);
		classWraper.RegFunction("ReferenceEquals", ReferenceEquals);
	}
}