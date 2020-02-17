using System;
using SharpLuna;
using System.Collections.Generic;
using static SharpLuna.Lua;

[WrapClass(typeof(System.Delegate))]
public class DelegateWrap
{
	[AOT.MonoPInvokeCallback(typeof(LuaNativeFunction))]
	static int Get_Method(IntPtr L)
	{
		var obj = SharpObject.Get<System.Delegate>(L, 1);
		Push(L, obj.Method);
		return 1;
	}

	[AOT.MonoPInvokeCallback(typeof(LuaNativeFunction))]
	static int Get_Target(IntPtr L)
	{
		var obj = SharpObject.Get<System.Delegate>(L, 1);
		Push(L, obj.Target);
		return 1;
	}

	[AOT.MonoPInvokeCallback(typeof(LuaNativeFunction))]
	static int CreateDelegate(IntPtr L)
	{
		int n = lua_gettop(L) - 1;
		if(n == 2)
		{
			#if LUNA_SCRIPT
			const int startStack = 2;
			#else
			const int startStack = 1;
			#endif
			Get(L, 0 + startStack, out System.Type t0);
			Get(L, 1 + startStack, out System.Reflection.MethodInfo t1);
			System.Delegate ret = System.Delegate.CreateDelegate(t0, t1);
			Push(L, ret);
			return 1;
		}
		else if(n == 3 && CheckType<System.Type, object, System.Reflection.MethodInfo>(L, 1))
		{
			#if LUNA_SCRIPT
			const int startStack = 2;
			#else
			const int startStack = 1;
			#endif
			Get(L, 0 + startStack, out System.Type t0);
			Get(L, 1 + startStack, out object t1);
			Get(L, 2 + startStack, out System.Reflection.MethodInfo t2);
			System.Delegate ret = System.Delegate.CreateDelegate(t0, t1, t2);
			Push(L, ret);
			return 1;
		}
		else if(n == 3 && CheckType<System.Type, System.Reflection.MethodInfo, bool>(L, 1))
		{
			#if LUNA_SCRIPT
			const int startStack = 2;
			#else
			const int startStack = 1;
			#endif
			Get(L, 0 + startStack, out System.Type t0);
			Get(L, 1 + startStack, out System.Reflection.MethodInfo t1);
			Get(L, 2 + startStack, out bool t2);
			System.Delegate ret = System.Delegate.CreateDelegate(t0, t1, t2);
			Push(L, ret);
			return 1;
		}
		else if(n == 3 && CheckType<System.Type, object, string>(L, 1))
		{
			#if LUNA_SCRIPT
			const int startStack = 2;
			#else
			const int startStack = 1;
			#endif
			Get(L, 0 + startStack, out System.Type t0);
			Get(L, 1 + startStack, out object t1);
			Get(L, 2 + startStack, out string t2);
			System.Delegate ret = System.Delegate.CreateDelegate(t0, t1, t2);
			Push(L, ret);
			return 1;
		}
		else if(n == 3 && CheckType<System.Type, System.Type, string>(L, 1))
		{
			#if LUNA_SCRIPT
			const int startStack = 2;
			#else
			const int startStack = 1;
			#endif
			Get(L, 0 + startStack, out System.Type t0);
			Get(L, 1 + startStack, out System.Type t1);
			Get(L, 2 + startStack, out string t2);
			System.Delegate ret = System.Delegate.CreateDelegate(t0, t1, t2);
			Push(L, ret);
			return 1;
		}
		else if(n == 4 && CheckType<System.Type, object, System.Reflection.MethodInfo, bool>(L, 1))
		{
			#if LUNA_SCRIPT
			const int startStack = 2;
			#else
			const int startStack = 1;
			#endif
			Get(L, 0 + startStack, out System.Type t0);
			Get(L, 1 + startStack, out object t1);
			Get(L, 2 + startStack, out System.Reflection.MethodInfo t2);
			Get(L, 3 + startStack, out bool t3);
			System.Delegate ret = System.Delegate.CreateDelegate(t0, t1, t2, t3);
			Push(L, ret);
			return 1;
		}
		else if(n == 4 && CheckType<System.Type, System.Type, string, bool>(L, 1))
		{
			#if LUNA_SCRIPT
			const int startStack = 2;
			#else
			const int startStack = 1;
			#endif
			Get(L, 0 + startStack, out System.Type t0);
			Get(L, 1 + startStack, out System.Type t1);
			Get(L, 2 + startStack, out string t2);
			Get(L, 3 + startStack, out bool t3);
			System.Delegate ret = System.Delegate.CreateDelegate(t0, t1, t2, t3);
			Push(L, ret);
			return 1;
		}
		else if(n == 4 && CheckType<System.Type, object, string, bool>(L, 1))
		{
			#if LUNA_SCRIPT
			const int startStack = 2;
			#else
			const int startStack = 1;
			#endif
			Get(L, 0 + startStack, out System.Type t0);
			Get(L, 1 + startStack, out object t1);
			Get(L, 2 + startStack, out string t2);
			Get(L, 3 + startStack, out bool t3);
			System.Delegate ret = System.Delegate.CreateDelegate(t0, t1, t2, t3);
			Push(L, ret);
			return 1;
		}
		else if(n == 5 && CheckType<System.Type, System.Type, string, bool, bool>(L, 1))
		{
			#if LUNA_SCRIPT
			const int startStack = 2;
			#else
			const int startStack = 1;
			#endif
			Get(L, 0 + startStack, out System.Type t0);
			Get(L, 1 + startStack, out System.Type t1);
			Get(L, 2 + startStack, out string t2);
			Get(L, 3 + startStack, out bool t3);
			Get(L, 4 + startStack, out bool t4);
			System.Delegate ret = System.Delegate.CreateDelegate(t0, t1, t2, t3, t4);
			Push(L, ret);
			return 1;
		}
		else if(n == 5 && CheckType<System.Type, object, string, bool, bool>(L, 1))
		{
			#if LUNA_SCRIPT
			const int startStack = 2;
			#else
			const int startStack = 1;
			#endif
			Get(L, 0 + startStack, out System.Type t0);
			Get(L, 1 + startStack, out object t1);
			Get(L, 2 + startStack, out string t2);
			Get(L, 3 + startStack, out bool t3);
			Get(L, 4 + startStack, out bool t4);
			System.Delegate ret = System.Delegate.CreateDelegate(t0, t1, t2, t3, t4);
			Push(L, ret);
			return 1;
		}
		return 0;
	}

	[AOT.MonoPInvokeCallback(typeof(LuaNativeFunction))]
	static int DynamicInvoke(IntPtr L)
	{
		const int startStack = 2;
		var obj = SharpObject.Get<System.Delegate>(L, 1);
		Get(L, 0 + startStack, out object[] t0);
		object ret = obj.DynamicInvoke(t0);
		Push(L, ret);
		return 1;
	}

	[AOT.MonoPInvokeCallback(typeof(LuaNativeFunction))]
	static int Clone(IntPtr L)
	{
		var obj = SharpObject.Get<System.Delegate>(L, 1);
		object ret = obj.Clone();
		Push(L, ret);
		return 1;
	}

	[AOT.MonoPInvokeCallback(typeof(LuaNativeFunction))]
	static int Equals(IntPtr L)
	{
		const int startStack = 2;
		var obj = SharpObject.Get<System.Delegate>(L, 1);
		Get(L, 0 + startStack, out object t0);
		bool ret = obj.Equals(t0);
		Push(L, ret);
		return 1;
	}

	[AOT.MonoPInvokeCallback(typeof(LuaNativeFunction))]
	static int GetHashCode(IntPtr L)
	{
		var obj = SharpObject.Get<System.Delegate>(L, 1);
		int ret = obj.GetHashCode();
		Push(L, ret);
		return 1;
	}

	[AOT.MonoPInvokeCallback(typeof(LuaNativeFunction))]
	static int GetObjectData(IntPtr L)
	{
		const int startStack = 2;
		var obj = SharpObject.Get<System.Delegate>(L, 1);
		Get(L, 0 + startStack, out System.Runtime.Serialization.SerializationInfo t0);
		Get(L, 1 + startStack, out System.Runtime.Serialization.StreamingContext t1);
		obj.GetObjectData(t0, t1);
		return 0;
	}

	[AOT.MonoPInvokeCallback(typeof(LuaNativeFunction))]
	static int GetInvocationList(IntPtr L)
	{
		var obj = SharpObject.Get<System.Delegate>(L, 1);
		System.Delegate[] ret = obj.GetInvocationList();
		Push(L, ret);
		return 1;
	}

	[AOT.MonoPInvokeCallback(typeof(LuaNativeFunction))]
	static int Combine(IntPtr L)
	{
		int n = lua_gettop(L) - 1;
		if(n == 1)
		{
			#if LUNA_SCRIPT
			const int startStack = 2;
			#else
			const int startStack = 1;
			#endif
			Get(L, 0 + startStack, out System.Delegate[] t0);
			System.Delegate ret = System.Delegate.Combine(t0);
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
			Get(L, 0 + startStack, out System.Delegate t0);
			Get(L, 1 + startStack, out System.Delegate t1);
			System.Delegate ret = System.Delegate.Combine(t0, t1);
			Push(L, ret);
			return 1;
		}
		return 0;
	}

	[AOT.MonoPInvokeCallback(typeof(LuaNativeFunction))]
	static int Remove(IntPtr L)
	{
		#if LUNA_SCRIPT
		const int startStack = 2;
		#else
		const int startStack = 1;
		#endif
		Get(L, 0 + startStack, out System.Delegate t0);
		Get(L, 1 + startStack, out System.Delegate t1);
		System.Delegate ret = System.Delegate.Remove(t0, t1);
		Push(L, ret);
		return 1;
	}

	[AOT.MonoPInvokeCallback(typeof(LuaNativeFunction))]
	static int RemoveAll(IntPtr L)
	{
		#if LUNA_SCRIPT
		const int startStack = 2;
		#else
		const int startStack = 1;
		#endif
		Get(L, 0 + startStack, out System.Delegate t0);
		Get(L, 1 + startStack, out System.Delegate t1);
		System.Delegate ret = System.Delegate.RemoveAll(t0, t1);
		Push(L, ret);
		return 1;
	}

	[AOT.MonoPInvokeCallback(typeof(LuaNativeFunction))]
	static int GetType(IntPtr L)
	{
		var obj = SharpObject.Get<System.Delegate>(L, 1);
		System.Type ret = obj.GetType();
		Push(L, ret);
		return 1;
	}

	[AOT.MonoPInvokeCallback(typeof(LuaNativeFunction))]
	static int ToString(IntPtr L)
	{
		var obj = SharpObject.Get<System.Delegate>(L, 1);
		string ret = obj.ToString();
		Push(L, ret);
		return 1;
	}

	public static void Register(ClassWraper classWraper)
	{
		classWraper.RegProperty("Method", Get_Method);
		classWraper.RegProperty("Target", Get_Target);
		classWraper.RegFunction("CreateDelegate", CreateDelegate);
		classWraper.RegFunction("DynamicInvoke", DynamicInvoke);
		classWraper.RegFunction("Clone", Clone);
		classWraper.RegFunction("Equals", Equals);
		classWraper.RegFunction("GetHashCode", GetHashCode);
		classWraper.RegFunction("GetObjectData", GetObjectData);
		classWraper.RegFunction("GetInvocationList", GetInvocationList);
		classWraper.RegFunction("Combine", Combine);
		classWraper.RegFunction("Remove", Remove);
		classWraper.RegFunction("RemoveAll", RemoveAll);
		classWraper.RegFunction("GetType", GetType);
		classWraper.RegFunction("ToString", ToString);
	}
}