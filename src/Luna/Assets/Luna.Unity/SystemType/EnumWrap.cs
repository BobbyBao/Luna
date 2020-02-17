using System;
using SharpLuna;
using System.Collections.Generic;
using static SharpLuna.Lua;

[WrapClass(typeof(System.Enum))]
public class EnumWrap
{
	[AOT.MonoPInvokeCallback(typeof(LuaNativeFunction))]
	static int Parse(IntPtr L)
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
			Get(L, 1 + startStack, out string t1);
			object ret = System.Enum.Parse(t0, t1);
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
			Get(L, 0 + startStack, out System.Type t0);
			Get(L, 1 + startStack, out string t1);
			Get(L, 2 + startStack, out bool t2);
			object ret = System.Enum.Parse(t0, t1, t2);
			Push(L, ret);
			return 1;
		}
		return 0;
	}

	[AOT.MonoPInvokeCallback(typeof(LuaNativeFunction))]
	static int GetUnderlyingType(IntPtr L)
	{
		#if LUNA_SCRIPT
		const int startStack = 2;
		#else
		const int startStack = 1;
		#endif
		Get(L, 0 + startStack, out System.Type t0);
		System.Type ret = System.Enum.GetUnderlyingType(t0);
		Push(L, ret);
		return 1;
	}

	[AOT.MonoPInvokeCallback(typeof(LuaNativeFunction))]
	static int GetValues(IntPtr L)
	{
		#if LUNA_SCRIPT
		const int startStack = 2;
		#else
		const int startStack = 1;
		#endif
		Get(L, 0 + startStack, out System.Type t0);
		System.Array ret = System.Enum.GetValues(t0);
		Push(L, ret);
		return 1;
	}

	[AOT.MonoPInvokeCallback(typeof(LuaNativeFunction))]
	static int GetName(IntPtr L)
	{
		#if LUNA_SCRIPT
		const int startStack = 2;
		#else
		const int startStack = 1;
		#endif
		Get(L, 0 + startStack, out System.Type t0);
		Get(L, 1 + startStack, out object t1);
		string ret = System.Enum.GetName(t0, t1);
		Push(L, ret);
		return 1;
	}

	[AOT.MonoPInvokeCallback(typeof(LuaNativeFunction))]
	static int GetNames(IntPtr L)
	{
		#if LUNA_SCRIPT
		const int startStack = 2;
		#else
		const int startStack = 1;
		#endif
		Get(L, 0 + startStack, out System.Type t0);
		string[] ret = System.Enum.GetNames(t0);
		Push(L, ret);
		return 1;
	}

	[AOT.MonoPInvokeCallback(typeof(LuaNativeFunction))]
	static int ToObject(IntPtr L)
	{
		int n = lua_gettop(L) - 1;
		if(n == 2 && CheckType<System.Type, object>(L, 1))
		{
			#if LUNA_SCRIPT
			const int startStack = 2;
			#else
			const int startStack = 1;
			#endif
			Get(L, 0 + startStack, out System.Type t0);
			Get(L, 1 + startStack, out object t1);
			object ret = System.Enum.ToObject(t0, t1);
			Push(L, ret);
			return 1;
		}
		else if(n == 2 && CheckType<System.Type, sbyte>(L, 1))
		{
			#if LUNA_SCRIPT
			const int startStack = 2;
			#else
			const int startStack = 1;
			#endif
			Get(L, 0 + startStack, out System.Type t0);
			Get(L, 1 + startStack, out sbyte t1);
			object ret = System.Enum.ToObject(t0, t1);
			Push(L, ret);
			return 1;
		}
		else if(n == 2 && CheckType<System.Type, short>(L, 1))
		{
			#if LUNA_SCRIPT
			const int startStack = 2;
			#else
			const int startStack = 1;
			#endif
			Get(L, 0 + startStack, out System.Type t0);
			Get(L, 1 + startStack, out short t1);
			object ret = System.Enum.ToObject(t0, t1);
			Push(L, ret);
			return 1;
		}
		else if(n == 2 && CheckType<System.Type, int>(L, 1))
		{
			#if LUNA_SCRIPT
			const int startStack = 2;
			#else
			const int startStack = 1;
			#endif
			Get(L, 0 + startStack, out System.Type t0);
			Get(L, 1 + startStack, out int t1);
			object ret = System.Enum.ToObject(t0, t1);
			Push(L, ret);
			return 1;
		}
		else if(n == 2 && CheckType<System.Type, byte>(L, 1))
		{
			#if LUNA_SCRIPT
			const int startStack = 2;
			#else
			const int startStack = 1;
			#endif
			Get(L, 0 + startStack, out System.Type t0);
			Get(L, 1 + startStack, out byte t1);
			object ret = System.Enum.ToObject(t0, t1);
			Push(L, ret);
			return 1;
		}
		else if(n == 2 && CheckType<System.Type, ushort>(L, 1))
		{
			#if LUNA_SCRIPT
			const int startStack = 2;
			#else
			const int startStack = 1;
			#endif
			Get(L, 0 + startStack, out System.Type t0);
			Get(L, 1 + startStack, out ushort t1);
			object ret = System.Enum.ToObject(t0, t1);
			Push(L, ret);
			return 1;
		}
		else if(n == 2 && CheckType<System.Type, uint>(L, 1))
		{
			#if LUNA_SCRIPT
			const int startStack = 2;
			#else
			const int startStack = 1;
			#endif
			Get(L, 0 + startStack, out System.Type t0);
			Get(L, 1 + startStack, out uint t1);
			object ret = System.Enum.ToObject(t0, t1);
			Push(L, ret);
			return 1;
		}
		else if(n == 2 && CheckType<System.Type, long>(L, 1))
		{
			#if LUNA_SCRIPT
			const int startStack = 2;
			#else
			const int startStack = 1;
			#endif
			Get(L, 0 + startStack, out System.Type t0);
			Get(L, 1 + startStack, out long t1);
			object ret = System.Enum.ToObject(t0, t1);
			Push(L, ret);
			return 1;
		}
		else if(n == 2 && CheckType<System.Type, ulong>(L, 1))
		{
			#if LUNA_SCRIPT
			const int startStack = 2;
			#else
			const int startStack = 1;
			#endif
			Get(L, 0 + startStack, out System.Type t0);
			Get(L, 1 + startStack, out ulong t1);
			object ret = System.Enum.ToObject(t0, t1);
			Push(L, ret);
			return 1;
		}
		return 0;
	}

	[AOT.MonoPInvokeCallback(typeof(LuaNativeFunction))]
	static int IsDefined(IntPtr L)
	{
		#if LUNA_SCRIPT
		const int startStack = 2;
		#else
		const int startStack = 1;
		#endif
		Get(L, 0 + startStack, out System.Type t0);
		Get(L, 1 + startStack, out object t1);
		bool ret = System.Enum.IsDefined(t0, t1);
		Push(L, ret);
		return 1;
	}

	[AOT.MonoPInvokeCallback(typeof(LuaNativeFunction))]
	static int Format(IntPtr L)
	{
		#if LUNA_SCRIPT
		const int startStack = 2;
		#else
		const int startStack = 1;
		#endif
		Get(L, 0 + startStack, out System.Type t0);
		Get(L, 1 + startStack, out object t1);
		Get(L, 2 + startStack, out string t2);
		string ret = System.Enum.Format(t0, t1, t2);
		Push(L, ret);
		return 1;
	}

	[AOT.MonoPInvokeCallback(typeof(LuaNativeFunction))]
	static int Equals(IntPtr L)
	{
		const int startStack = 2;
		var obj = SharpObject.Get<System.Enum>(L, 1);
		Get(L, 0 + startStack, out object t0);
		bool ret = obj.Equals(t0);
		Push(L, ret);
		return 1;
	}

	[AOT.MonoPInvokeCallback(typeof(LuaNativeFunction))]
	static int GetHashCode(IntPtr L)
	{
		var obj = SharpObject.Get<System.Enum>(L, 1);
		int ret = obj.GetHashCode();
		Push(L, ret);
		return 1;
	}

	[AOT.MonoPInvokeCallback(typeof(LuaNativeFunction))]
	static int ToString(IntPtr L)
	{
		int n = lua_gettop(L) - 1;
		if(n == 0)
		{
			var obj = SharpObject.Get<System.Enum>(L, 1);
			string ret = obj.ToString();
			Push(L, ret);
			return 1;
		}
		else if(n == 1)
		{
			const int startStack = 2;
			var obj = SharpObject.Get<System.Enum>(L, 1);
			Get(L, 0 + startStack, out string t0);
			string ret = obj.ToString(t0);
			Push(L, ret);
			return 1;
		}
		return 0;
	}

	[AOT.MonoPInvokeCallback(typeof(LuaNativeFunction))]
	static int CompareTo(IntPtr L)
	{
		const int startStack = 2;
		var obj = SharpObject.Get<System.Enum>(L, 1);
		Get(L, 0 + startStack, out object t0);
		int ret = obj.CompareTo(t0);
		Push(L, ret);
		return 1;
	}

	[AOT.MonoPInvokeCallback(typeof(LuaNativeFunction))]
	static int HasFlag(IntPtr L)
	{
		const int startStack = 2;
		var obj = SharpObject.Get<System.Enum>(L, 1);
		Get(L, 0 + startStack, out System.Enum t0);
		bool ret = obj.HasFlag(t0);
		Push(L, ret);
		return 1;
	}

	[AOT.MonoPInvokeCallback(typeof(LuaNativeFunction))]
	static int GetTypeCode(IntPtr L)
	{
		var obj = SharpObject.Get<System.Enum>(L, 1);
		System.TypeCode ret = obj.GetTypeCode();
		Push(L, ret);
		return 1;
	}

	[AOT.MonoPInvokeCallback(typeof(LuaNativeFunction))]
	static int GetType(IntPtr L)
	{
		var obj = SharpObject.Get<System.Enum>(L, 1);
		System.Type ret = obj.GetType();
		Push(L, ret);
		return 1;
	}

	public static void Register(ClassWraper classWraper)
	{
		classWraper.RegFunction("Parse", Parse);
		classWraper.RegFunction("GetUnderlyingType", GetUnderlyingType);
		classWraper.RegFunction("GetValues", GetValues);
		classWraper.RegFunction("GetName", GetName);
		classWraper.RegFunction("GetNames", GetNames);
		classWraper.RegFunction("ToObject", ToObject);
		classWraper.RegFunction("IsDefined", IsDefined);
		classWraper.RegFunction("Format", Format);
		classWraper.RegFunction("Equals", Equals);
		classWraper.RegFunction("GetHashCode", GetHashCode);
		classWraper.RegFunction("ToString", ToString);
		classWraper.RegFunction("CompareTo", CompareTo);
		classWraper.RegFunction("HasFlag", HasFlag);
		classWraper.RegFunction("GetTypeCode", GetTypeCode);
		classWraper.RegFunction("GetType", GetType);
	}
}