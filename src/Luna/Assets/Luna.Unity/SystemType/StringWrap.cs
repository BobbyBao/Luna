using System;
using SharpLuna;
using System.Collections.Generic;
using static SharpLuna.Lua;

[WrapClass(typeof(System.String))]
public class StringWrap
{
	[AOT.MonoPInvokeCallback(typeof(LuaNativeFunction))]
	static int Constructor(IntPtr L)
	{
		int n = lua_gettop(L) - 1;
		System.String obj = default;
		if(n == 1)
		{
			Get(L, 2, out char[] t1);
			obj = new System.String(t1);
		}
		else if(n == 2)
		{
			Get(L, 2, out char t1);
			Get(L, 3, out int t2);
			obj = new System.String(t1, t2);
		}
		else if(n == 3)
		{
			Get(L, 2, out char[] t1);
			Get(L, 3, out int t2);
			Get(L, 4, out int t3);
			obj = new System.String(t1, t2, t3);
		}
		Push(L, obj);
		return 1;
	}

	[AOT.MonoPInvokeCallback(typeof(LuaNativeFunction))]
	static int Get_Empty(IntPtr L)
	{
		Push(L, System.String.Empty);
		return 1;
	}

	[AOT.MonoPInvokeCallback(typeof(LuaNativeFunction))]
	static int Get_Length(IntPtr L)
	{
		var obj = SharpObject.Get<System.String>(L, 1);
		Push(L, obj.Length);
		return 1;
	}

	[AOT.MonoPInvokeCallback(typeof(LuaNativeFunction))]
	static int Join(IntPtr L)
	{
		int n = lua_gettop(L) - 1;
		if(n == 2 && CheckType<string, string[]>(L, 1))
		{
			#if LUNA_SCRIPT
			const int startStack = 2;
			#else
			const int startStack = 1;
			#endif
			Get(L, 0 + startStack, out string t0);
			Get(L, 1 + startStack, out string[] t1);
			string ret = System.String.Join(t0, t1);
			Push(L, ret);
			return 1;
		}
		else if(n == 2 && CheckType<string, object[]>(L, 1))
		{
			#if LUNA_SCRIPT
			const int startStack = 2;
			#else
			const int startStack = 1;
			#endif
			Get(L, 0 + startStack, out string t0);
			Get(L, 1 + startStack, out object[] t1);
			string ret = System.String.Join(t0, t1);
			Push(L, ret);
			return 1;
		}
		else if(n == 4)
		{
			#if LUNA_SCRIPT
			const int startStack = 2;
			#else
			const int startStack = 1;
			#endif
			Get(L, 0 + startStack, out string t0);
			Get(L, 1 + startStack, out string[] t1);
			Get(L, 2 + startStack, out int t2);
			Get(L, 3 + startStack, out int t3);
			string ret = System.String.Join(t0, t1, t2, t3);
			Push(L, ret);
			return 1;
		}
		return 0;
	}

	[AOT.MonoPInvokeCallback(typeof(LuaNativeFunction))]
	static int Equals(IntPtr L)
	{
		int n = lua_gettop(L) - 1;
		if(n == 1 && CheckType<object>(L, 1))
		{
			const int startStack = 2;
			var obj = SharpObject.Get<System.String>(L, 1);
			Get(L, 0 + startStack, out object t0);
			bool ret = obj.Equals(t0);
			Push(L, ret);
			return 1;
		}
		else if(n == 1 && CheckType<string>(L, 1))
		{
			const int startStack = 2;
			var obj = SharpObject.Get<System.String>(L, 1);
			Get(L, 0 + startStack, out string t0);
			bool ret = obj.Equals(t0);
			Push(L, ret);
			return 1;
		}
		else if(n == 2 && CheckType<string, System.StringComparison>(L, 1))
		{
			const int startStack = 2;
			var obj = SharpObject.Get<System.String>(L, 1);
			Get(L, 0 + startStack, out string t0);
			Get(L, 1 + startStack, out System.StringComparison t1);
			bool ret = obj.Equals(t0, t1);
			Push(L, ret);
			return 1;
		}
		else if(n == 2 && CheckType<string, string>(L, 1))
		{
			#if LUNA_SCRIPT
			const int startStack = 2;
			#else
			const int startStack = 1;
			#endif
			Get(L, 0 + startStack, out string t0);
			Get(L, 1 + startStack, out string t1);
			bool ret = System.String.Equals(t0, t1);
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
			Get(L, 0 + startStack, out string t0);
			Get(L, 1 + startStack, out string t1);
			Get(L, 2 + startStack, out System.StringComparison t2);
			bool ret = System.String.Equals(t0, t1, t2);
			Push(L, ret);
			return 1;
		}
		return 0;
	}

	[AOT.MonoPInvokeCallback(typeof(LuaNativeFunction))]
	static int CopyTo(IntPtr L)
	{
		const int startStack = 2;
		var obj = SharpObject.Get<System.String>(L, 1);
		Get(L, 0 + startStack, out int t0);
		Get(L, 1 + startStack, out char[] t1);
		Get(L, 2 + startStack, out int t2);
		Get(L, 3 + startStack, out int t3);
		obj.CopyTo(t0, t1, t2, t3);
		return 0;
	}

	[AOT.MonoPInvokeCallback(typeof(LuaNativeFunction))]
	static int ToCharArray(IntPtr L)
	{
		int n = lua_gettop(L) - 1;
		if(n == 0)
		{
			var obj = SharpObject.Get<System.String>(L, 1);
			char[] ret = obj.ToCharArray();
			Push(L, ret);
			return 1;
		}
		else if(n == 2)
		{
			const int startStack = 2;
			var obj = SharpObject.Get<System.String>(L, 1);
			Get(L, 0 + startStack, out int t0);
			Get(L, 1 + startStack, out int t1);
			char[] ret = obj.ToCharArray(t0, t1);
			Push(L, ret);
			return 1;
		}
		return 0;
	}

	[AOT.MonoPInvokeCallback(typeof(LuaNativeFunction))]
	static int IsNullOrEmpty(IntPtr L)
	{
		#if LUNA_SCRIPT
		const int startStack = 2;
		#else
		const int startStack = 1;
		#endif
		Get(L, 0 + startStack, out string t0);
		bool ret = System.String.IsNullOrEmpty(t0);
		Push(L, ret);
		return 1;
	}

	[AOT.MonoPInvokeCallback(typeof(LuaNativeFunction))]
	static int IsNullOrWhiteSpace(IntPtr L)
	{
		#if LUNA_SCRIPT
		const int startStack = 2;
		#else
		const int startStack = 1;
		#endif
		Get(L, 0 + startStack, out string t0);
		bool ret = System.String.IsNullOrWhiteSpace(t0);
		Push(L, ret);
		return 1;
	}

	[AOT.MonoPInvokeCallback(typeof(LuaNativeFunction))]
	static int GetHashCode(IntPtr L)
	{
		var obj = SharpObject.Get<System.String>(L, 1);
		int ret = obj.GetHashCode();
		Push(L, ret);
		return 1;
	}

	[AOT.MonoPInvokeCallback(typeof(LuaNativeFunction))]
	static int Split(IntPtr L)
	{
		int n = lua_gettop(L) - 1;
		if(n == 1)
		{
			const int startStack = 2;
			var obj = SharpObject.Get<System.String>(L, 1);
			Get(L, 0 + startStack, out char[] t0);
			string[] ret = obj.Split(t0);
			Push(L, ret);
			return 1;
		}
		else if(n == 2 && CheckType<char[], int>(L, 1))
		{
			const int startStack = 2;
			var obj = SharpObject.Get<System.String>(L, 1);
			Get(L, 0 + startStack, out char[] t0);
			Get(L, 1 + startStack, out int t1);
			string[] ret = obj.Split(t0, t1);
			Push(L, ret);
			return 1;
		}
		else if(n == 2 && CheckType<char[], System.StringSplitOptions>(L, 1))
		{
			const int startStack = 2;
			var obj = SharpObject.Get<System.String>(L, 1);
			Get(L, 0 + startStack, out char[] t0);
			Get(L, 1 + startStack, out System.StringSplitOptions t1);
			string[] ret = obj.Split(t0, t1);
			Push(L, ret);
			return 1;
		}
		else if(n == 2 && CheckType<string[], System.StringSplitOptions>(L, 1))
		{
			const int startStack = 2;
			var obj = SharpObject.Get<System.String>(L, 1);
			Get(L, 0 + startStack, out string[] t0);
			Get(L, 1 + startStack, out System.StringSplitOptions t1);
			string[] ret = obj.Split(t0, t1);
			Push(L, ret);
			return 1;
		}
		else if(n == 3 && CheckType<char[], int, System.StringSplitOptions>(L, 1))
		{
			const int startStack = 2;
			var obj = SharpObject.Get<System.String>(L, 1);
			Get(L, 0 + startStack, out char[] t0);
			Get(L, 1 + startStack, out int t1);
			Get(L, 2 + startStack, out System.StringSplitOptions t2);
			string[] ret = obj.Split(t0, t1, t2);
			Push(L, ret);
			return 1;
		}
		else if(n == 3 && CheckType<string[], int, System.StringSplitOptions>(L, 1))
		{
			const int startStack = 2;
			var obj = SharpObject.Get<System.String>(L, 1);
			Get(L, 0 + startStack, out string[] t0);
			Get(L, 1 + startStack, out int t1);
			Get(L, 2 + startStack, out System.StringSplitOptions t2);
			string[] ret = obj.Split(t0, t1, t2);
			Push(L, ret);
			return 1;
		}
		return 0;
	}

	[AOT.MonoPInvokeCallback(typeof(LuaNativeFunction))]
	static int Substring(IntPtr L)
	{
		int n = lua_gettop(L) - 1;
		if(n == 1)
		{
			const int startStack = 2;
			var obj = SharpObject.Get<System.String>(L, 1);
			Get(L, 0 + startStack, out int t0);
			string ret = obj.Substring(t0);
			Push(L, ret);
			return 1;
		}
		else if(n == 2)
		{
			const int startStack = 2;
			var obj = SharpObject.Get<System.String>(L, 1);
			Get(L, 0 + startStack, out int t0);
			Get(L, 1 + startStack, out int t1);
			string ret = obj.Substring(t0, t1);
			Push(L, ret);
			return 1;
		}
		return 0;
	}

	[AOT.MonoPInvokeCallback(typeof(LuaNativeFunction))]
	static int Trim(IntPtr L)
	{
		int n = lua_gettop(L) - 1;
		if(n == 0)
		{
			var obj = SharpObject.Get<System.String>(L, 1);
			string ret = obj.Trim();
			Push(L, ret);
			return 1;
		}
		else if(n == 1)
		{
			const int startStack = 2;
			var obj = SharpObject.Get<System.String>(L, 1);
			Get(L, 0 + startStack, out char[] t0);
			string ret = obj.Trim(t0);
			Push(L, ret);
			return 1;
		}
		return 0;
	}

	[AOT.MonoPInvokeCallback(typeof(LuaNativeFunction))]
	static int TrimStart(IntPtr L)
	{
		const int startStack = 2;
		var obj = SharpObject.Get<System.String>(L, 1);
		Get(L, 0 + startStack, out char[] t0);
		string ret = obj.TrimStart(t0);
		Push(L, ret);
		return 1;
	}

	[AOT.MonoPInvokeCallback(typeof(LuaNativeFunction))]
	static int TrimEnd(IntPtr L)
	{
		const int startStack = 2;
		var obj = SharpObject.Get<System.String>(L, 1);
		Get(L, 0 + startStack, out char[] t0);
		string ret = obj.TrimEnd(t0);
		Push(L, ret);
		return 1;
	}

	[AOT.MonoPInvokeCallback(typeof(LuaNativeFunction))]
	static int IsNormalized(IntPtr L)
	{
		int n = lua_gettop(L) - 1;
		if(n == 0)
		{
			var obj = SharpObject.Get<System.String>(L, 1);
			bool ret = obj.IsNormalized();
			Push(L, ret);
			return 1;
		}
		else if(n == 1)
		{
			const int startStack = 2;
			var obj = SharpObject.Get<System.String>(L, 1);
			Get(L, 0 + startStack, out System.Text.NormalizationForm t0);
			bool ret = obj.IsNormalized(t0);
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
			var obj = SharpObject.Get<System.String>(L, 1);
			string ret = obj.Normalize();
			Push(L, ret);
			return 1;
		}
		else if(n == 1)
		{
			const int startStack = 2;
			var obj = SharpObject.Get<System.String>(L, 1);
			Get(L, 0 + startStack, out System.Text.NormalizationForm t0);
			string ret = obj.Normalize(t0);
			Push(L, ret);
			return 1;
		}
		return 0;
	}

	[AOT.MonoPInvokeCallback(typeof(LuaNativeFunction))]
	static int Compare(IntPtr L)
	{
		int n = lua_gettop(L) - 1;
		if(n == 2)
		{
			#if LUNA_SCRIPT
			const int startStack = 2;
			#else
			const int startStack = 1;
			#endif
			Get(L, 0 + startStack, out string t0);
			Get(L, 1 + startStack, out string t1);
			int ret = System.String.Compare(t0, t1);
			Push(L, ret);
			return 1;
		}
		else if(n == 3 && CheckType<string, string, bool>(L, 1))
		{
			#if LUNA_SCRIPT
			const int startStack = 2;
			#else
			const int startStack = 1;
			#endif
			Get(L, 0 + startStack, out string t0);
			Get(L, 1 + startStack, out string t1);
			Get(L, 2 + startStack, out bool t2);
			int ret = System.String.Compare(t0, t1, t2);
			Push(L, ret);
			return 1;
		}
		else if(n == 3 && CheckType<string, string, System.StringComparison>(L, 1))
		{
			#if LUNA_SCRIPT
			const int startStack = 2;
			#else
			const int startStack = 1;
			#endif
			Get(L, 0 + startStack, out string t0);
			Get(L, 1 + startStack, out string t1);
			Get(L, 2 + startStack, out System.StringComparison t2);
			int ret = System.String.Compare(t0, t1, t2);
			Push(L, ret);
			return 1;
		}
		else if(n == 4 && CheckType<string, string, System.Globalization.CultureInfo, System.Globalization.CompareOptions>(L, 1))
		{
			#if LUNA_SCRIPT
			const int startStack = 2;
			#else
			const int startStack = 1;
			#endif
			Get(L, 0 + startStack, out string t0);
			Get(L, 1 + startStack, out string t1);
			Get(L, 2 + startStack, out System.Globalization.CultureInfo t2);
			Get(L, 3 + startStack, out System.Globalization.CompareOptions t3);
			int ret = System.String.Compare(t0, t1, t2, t3);
			Push(L, ret);
			return 1;
		}
		else if(n == 4 && CheckType<string, string, bool, System.Globalization.CultureInfo>(L, 1))
		{
			#if LUNA_SCRIPT
			const int startStack = 2;
			#else
			const int startStack = 1;
			#endif
			Get(L, 0 + startStack, out string t0);
			Get(L, 1 + startStack, out string t1);
			Get(L, 2 + startStack, out bool t2);
			Get(L, 3 + startStack, out System.Globalization.CultureInfo t3);
			int ret = System.String.Compare(t0, t1, t2, t3);
			Push(L, ret);
			return 1;
		}
		else if(n == 5)
		{
			#if LUNA_SCRIPT
			const int startStack = 2;
			#else
			const int startStack = 1;
			#endif
			Get(L, 0 + startStack, out string t0);
			Get(L, 1 + startStack, out int t1);
			Get(L, 2 + startStack, out string t2);
			Get(L, 3 + startStack, out int t3);
			Get(L, 4 + startStack, out int t4);
			int ret = System.String.Compare(t0, t1, t2, t3, t4);
			Push(L, ret);
			return 1;
		}
		else if(n == 6 && CheckType<string, int, string, int, int, bool>(L, 1))
		{
			#if LUNA_SCRIPT
			const int startStack = 2;
			#else
			const int startStack = 1;
			#endif
			Get(L, 0 + startStack, out string t0);
			Get(L, 1 + startStack, out int t1);
			Get(L, 2 + startStack, out string t2);
			Get(L, 3 + startStack, out int t3);
			Get(L, 4 + startStack, out int t4);
			Get(L, 5 + startStack, out bool t5);
			int ret = System.String.Compare(t0, t1, t2, t3, t4, t5);
			Push(L, ret);
			return 1;
		}
		else if(n == 6 && CheckType<string, int, string, int, int, System.StringComparison>(L, 1))
		{
			#if LUNA_SCRIPT
			const int startStack = 2;
			#else
			const int startStack = 1;
			#endif
			Get(L, 0 + startStack, out string t0);
			Get(L, 1 + startStack, out int t1);
			Get(L, 2 + startStack, out string t2);
			Get(L, 3 + startStack, out int t3);
			Get(L, 4 + startStack, out int t4);
			Get(L, 5 + startStack, out System.StringComparison t5);
			int ret = System.String.Compare(t0, t1, t2, t3, t4, t5);
			Push(L, ret);
			return 1;
		}
		else if(n == 7 && CheckType<string, int, string, int, int, bool, System.Globalization.CultureInfo>(L, 1))
		{
			#if LUNA_SCRIPT
			const int startStack = 2;
			#else
			const int startStack = 1;
			#endif
			Get(L, 0 + startStack, out string t0);
			Get(L, 1 + startStack, out int t1);
			Get(L, 2 + startStack, out string t2);
			Get(L, 3 + startStack, out int t3);
			Get(L, 4 + startStack, out int t4);
			Get(L, 5 + startStack, out bool t5);
			Get(L, 6 + startStack, out System.Globalization.CultureInfo t6);
			int ret = System.String.Compare(t0, t1, t2, t3, t4, t5, t6);
			Push(L, ret);
			return 1;
		}
		else if(n == 7 && CheckType<string, int, string, int, int, System.Globalization.CultureInfo, System.Globalization.CompareOptions>(L, 1))
		{
			#if LUNA_SCRIPT
			const int startStack = 2;
			#else
			const int startStack = 1;
			#endif
			Get(L, 0 + startStack, out string t0);
			Get(L, 1 + startStack, out int t1);
			Get(L, 2 + startStack, out string t2);
			Get(L, 3 + startStack, out int t3);
			Get(L, 4 + startStack, out int t4);
			Get(L, 5 + startStack, out System.Globalization.CultureInfo t5);
			Get(L, 6 + startStack, out System.Globalization.CompareOptions t6);
			int ret = System.String.Compare(t0, t1, t2, t3, t4, t5, t6);
			Push(L, ret);
			return 1;
		}
		return 0;
	}

	[AOT.MonoPInvokeCallback(typeof(LuaNativeFunction))]
	static int CompareTo(IntPtr L)
	{
		int n = lua_gettop(L) - 1;
		if(n == 1 && CheckType<object>(L, 1))
		{
			const int startStack = 2;
			var obj = SharpObject.Get<System.String>(L, 1);
			Get(L, 0 + startStack, out object t0);
			int ret = obj.CompareTo(t0);
			Push(L, ret);
			return 1;
		}
		else if(n == 1 && CheckType<string>(L, 1))
		{
			const int startStack = 2;
			var obj = SharpObject.Get<System.String>(L, 1);
			Get(L, 0 + startStack, out string t0);
			int ret = obj.CompareTo(t0);
			Push(L, ret);
			return 1;
		}
		return 0;
	}

	[AOT.MonoPInvokeCallback(typeof(LuaNativeFunction))]
	static int CompareOrdinal(IntPtr L)
	{
		int n = lua_gettop(L) - 1;
		if(n == 2)
		{
			#if LUNA_SCRIPT
			const int startStack = 2;
			#else
			const int startStack = 1;
			#endif
			Get(L, 0 + startStack, out string t0);
			Get(L, 1 + startStack, out string t1);
			int ret = System.String.CompareOrdinal(t0, t1);
			Push(L, ret);
			return 1;
		}
		else if(n == 5)
		{
			#if LUNA_SCRIPT
			const int startStack = 2;
			#else
			const int startStack = 1;
			#endif
			Get(L, 0 + startStack, out string t0);
			Get(L, 1 + startStack, out int t1);
			Get(L, 2 + startStack, out string t2);
			Get(L, 3 + startStack, out int t3);
			Get(L, 4 + startStack, out int t4);
			int ret = System.String.CompareOrdinal(t0, t1, t2, t3, t4);
			Push(L, ret);
			return 1;
		}
		return 0;
	}

	[AOT.MonoPInvokeCallback(typeof(LuaNativeFunction))]
	static int Contains(IntPtr L)
	{
		const int startStack = 2;
		var obj = SharpObject.Get<System.String>(L, 1);
		Get(L, 0 + startStack, out string t0);
		bool ret = obj.Contains(t0);
		Push(L, ret);
		return 1;
	}

	[AOT.MonoPInvokeCallback(typeof(LuaNativeFunction))]
	static int EndsWith(IntPtr L)
	{
		int n = lua_gettop(L) - 1;
		if(n == 1)
		{
			const int startStack = 2;
			var obj = SharpObject.Get<System.String>(L, 1);
			Get(L, 0 + startStack, out string t0);
			bool ret = obj.EndsWith(t0);
			Push(L, ret);
			return 1;
		}
		else if(n == 2)
		{
			const int startStack = 2;
			var obj = SharpObject.Get<System.String>(L, 1);
			Get(L, 0 + startStack, out string t0);
			Get(L, 1 + startStack, out System.StringComparison t1);
			bool ret = obj.EndsWith(t0, t1);
			Push(L, ret);
			return 1;
		}
		else if(n == 3)
		{
			const int startStack = 2;
			var obj = SharpObject.Get<System.String>(L, 1);
			Get(L, 0 + startStack, out string t0);
			Get(L, 1 + startStack, out bool t1);
			Get(L, 2 + startStack, out System.Globalization.CultureInfo t2);
			bool ret = obj.EndsWith(t0, t1, t2);
			Push(L, ret);
			return 1;
		}
		return 0;
	}

	[AOT.MonoPInvokeCallback(typeof(LuaNativeFunction))]
	static int IndexOf(IntPtr L)
	{
		int n = lua_gettop(L) - 1;
		if(n == 1 && CheckType<char>(L, 1))
		{
			const int startStack = 2;
			var obj = SharpObject.Get<System.String>(L, 1);
			Get(L, 0 + startStack, out char t0);
			int ret = obj.IndexOf(t0);
			Push(L, ret);
			return 1;
		}
		else if(n == 1 && CheckType<string>(L, 1))
		{
			const int startStack = 2;
			var obj = SharpObject.Get<System.String>(L, 1);
			Get(L, 0 + startStack, out string t0);
			int ret = obj.IndexOf(t0);
			Push(L, ret);
			return 1;
		}
		else if(n == 2 && CheckType<char, int>(L, 1))
		{
			const int startStack = 2;
			var obj = SharpObject.Get<System.String>(L, 1);
			Get(L, 0 + startStack, out char t0);
			Get(L, 1 + startStack, out int t1);
			int ret = obj.IndexOf(t0, t1);
			Push(L, ret);
			return 1;
		}
		else if(n == 2 && CheckType<string, int>(L, 1))
		{
			const int startStack = 2;
			var obj = SharpObject.Get<System.String>(L, 1);
			Get(L, 0 + startStack, out string t0);
			Get(L, 1 + startStack, out int t1);
			int ret = obj.IndexOf(t0, t1);
			Push(L, ret);
			return 1;
		}
		else if(n == 2 && CheckType<string, System.StringComparison>(L, 1))
		{
			const int startStack = 2;
			var obj = SharpObject.Get<System.String>(L, 1);
			Get(L, 0 + startStack, out string t0);
			Get(L, 1 + startStack, out System.StringComparison t1);
			int ret = obj.IndexOf(t0, t1);
			Push(L, ret);
			return 1;
		}
		else if(n == 3 && CheckType<string, int, int>(L, 1))
		{
			const int startStack = 2;
			var obj = SharpObject.Get<System.String>(L, 1);
			Get(L, 0 + startStack, out string t0);
			Get(L, 1 + startStack, out int t1);
			Get(L, 2 + startStack, out int t2);
			int ret = obj.IndexOf(t0, t1, t2);
			Push(L, ret);
			return 1;
		}
		else if(n == 3 && CheckType<string, int, System.StringComparison>(L, 1))
		{
			const int startStack = 2;
			var obj = SharpObject.Get<System.String>(L, 1);
			Get(L, 0 + startStack, out string t0);
			Get(L, 1 + startStack, out int t1);
			Get(L, 2 + startStack, out System.StringComparison t2);
			int ret = obj.IndexOf(t0, t1, t2);
			Push(L, ret);
			return 1;
		}
		else if(n == 3 && CheckType<char, int, int>(L, 1))
		{
			const int startStack = 2;
			var obj = SharpObject.Get<System.String>(L, 1);
			Get(L, 0 + startStack, out char t0);
			Get(L, 1 + startStack, out int t1);
			Get(L, 2 + startStack, out int t2);
			int ret = obj.IndexOf(t0, t1, t2);
			Push(L, ret);
			return 1;
		}
		else if(n == 4)
		{
			const int startStack = 2;
			var obj = SharpObject.Get<System.String>(L, 1);
			Get(L, 0 + startStack, out string t0);
			Get(L, 1 + startStack, out int t1);
			Get(L, 2 + startStack, out int t2);
			Get(L, 3 + startStack, out System.StringComparison t3);
			int ret = obj.IndexOf(t0, t1, t2, t3);
			Push(L, ret);
			return 1;
		}
		return 0;
	}

	[AOT.MonoPInvokeCallback(typeof(LuaNativeFunction))]
	static int IndexOfAny(IntPtr L)
	{
		int n = lua_gettop(L) - 1;
		if(n == 1)
		{
			const int startStack = 2;
			var obj = SharpObject.Get<System.String>(L, 1);
			Get(L, 0 + startStack, out char[] t0);
			int ret = obj.IndexOfAny(t0);
			Push(L, ret);
			return 1;
		}
		else if(n == 2)
		{
			const int startStack = 2;
			var obj = SharpObject.Get<System.String>(L, 1);
			Get(L, 0 + startStack, out char[] t0);
			Get(L, 1 + startStack, out int t1);
			int ret = obj.IndexOfAny(t0, t1);
			Push(L, ret);
			return 1;
		}
		else if(n == 3)
		{
			const int startStack = 2;
			var obj = SharpObject.Get<System.String>(L, 1);
			Get(L, 0 + startStack, out char[] t0);
			Get(L, 1 + startStack, out int t1);
			Get(L, 2 + startStack, out int t2);
			int ret = obj.IndexOfAny(t0, t1, t2);
			Push(L, ret);
			return 1;
		}
		return 0;
	}

	[AOT.MonoPInvokeCallback(typeof(LuaNativeFunction))]
	static int LastIndexOf(IntPtr L)
	{
		int n = lua_gettop(L) - 1;
		if(n == 1 && CheckType<char>(L, 1))
		{
			const int startStack = 2;
			var obj = SharpObject.Get<System.String>(L, 1);
			Get(L, 0 + startStack, out char t0);
			int ret = obj.LastIndexOf(t0);
			Push(L, ret);
			return 1;
		}
		else if(n == 1 && CheckType<string>(L, 1))
		{
			const int startStack = 2;
			var obj = SharpObject.Get<System.String>(L, 1);
			Get(L, 0 + startStack, out string t0);
			int ret = obj.LastIndexOf(t0);
			Push(L, ret);
			return 1;
		}
		else if(n == 2 && CheckType<char, int>(L, 1))
		{
			const int startStack = 2;
			var obj = SharpObject.Get<System.String>(L, 1);
			Get(L, 0 + startStack, out char t0);
			Get(L, 1 + startStack, out int t1);
			int ret = obj.LastIndexOf(t0, t1);
			Push(L, ret);
			return 1;
		}
		else if(n == 2 && CheckType<string, int>(L, 1))
		{
			const int startStack = 2;
			var obj = SharpObject.Get<System.String>(L, 1);
			Get(L, 0 + startStack, out string t0);
			Get(L, 1 + startStack, out int t1);
			int ret = obj.LastIndexOf(t0, t1);
			Push(L, ret);
			return 1;
		}
		else if(n == 2 && CheckType<string, System.StringComparison>(L, 1))
		{
			const int startStack = 2;
			var obj = SharpObject.Get<System.String>(L, 1);
			Get(L, 0 + startStack, out string t0);
			Get(L, 1 + startStack, out System.StringComparison t1);
			int ret = obj.LastIndexOf(t0, t1);
			Push(L, ret);
			return 1;
		}
		else if(n == 3 && CheckType<string, int, int>(L, 1))
		{
			const int startStack = 2;
			var obj = SharpObject.Get<System.String>(L, 1);
			Get(L, 0 + startStack, out string t0);
			Get(L, 1 + startStack, out int t1);
			Get(L, 2 + startStack, out int t2);
			int ret = obj.LastIndexOf(t0, t1, t2);
			Push(L, ret);
			return 1;
		}
		else if(n == 3 && CheckType<string, int, System.StringComparison>(L, 1))
		{
			const int startStack = 2;
			var obj = SharpObject.Get<System.String>(L, 1);
			Get(L, 0 + startStack, out string t0);
			Get(L, 1 + startStack, out int t1);
			Get(L, 2 + startStack, out System.StringComparison t2);
			int ret = obj.LastIndexOf(t0, t1, t2);
			Push(L, ret);
			return 1;
		}
		else if(n == 3 && CheckType<char, int, int>(L, 1))
		{
			const int startStack = 2;
			var obj = SharpObject.Get<System.String>(L, 1);
			Get(L, 0 + startStack, out char t0);
			Get(L, 1 + startStack, out int t1);
			Get(L, 2 + startStack, out int t2);
			int ret = obj.LastIndexOf(t0, t1, t2);
			Push(L, ret);
			return 1;
		}
		else if(n == 4)
		{
			const int startStack = 2;
			var obj = SharpObject.Get<System.String>(L, 1);
			Get(L, 0 + startStack, out string t0);
			Get(L, 1 + startStack, out int t1);
			Get(L, 2 + startStack, out int t2);
			Get(L, 3 + startStack, out System.StringComparison t3);
			int ret = obj.LastIndexOf(t0, t1, t2, t3);
			Push(L, ret);
			return 1;
		}
		return 0;
	}

	[AOT.MonoPInvokeCallback(typeof(LuaNativeFunction))]
	static int LastIndexOfAny(IntPtr L)
	{
		int n = lua_gettop(L) - 1;
		if(n == 1)
		{
			const int startStack = 2;
			var obj = SharpObject.Get<System.String>(L, 1);
			Get(L, 0 + startStack, out char[] t0);
			int ret = obj.LastIndexOfAny(t0);
			Push(L, ret);
			return 1;
		}
		else if(n == 2)
		{
			const int startStack = 2;
			var obj = SharpObject.Get<System.String>(L, 1);
			Get(L, 0 + startStack, out char[] t0);
			Get(L, 1 + startStack, out int t1);
			int ret = obj.LastIndexOfAny(t0, t1);
			Push(L, ret);
			return 1;
		}
		else if(n == 3)
		{
			const int startStack = 2;
			var obj = SharpObject.Get<System.String>(L, 1);
			Get(L, 0 + startStack, out char[] t0);
			Get(L, 1 + startStack, out int t1);
			Get(L, 2 + startStack, out int t2);
			int ret = obj.LastIndexOfAny(t0, t1, t2);
			Push(L, ret);
			return 1;
		}
		return 0;
	}

	[AOT.MonoPInvokeCallback(typeof(LuaNativeFunction))]
	static int PadLeft(IntPtr L)
	{
		int n = lua_gettop(L) - 1;
		if(n == 1)
		{
			const int startStack = 2;
			var obj = SharpObject.Get<System.String>(L, 1);
			Get(L, 0 + startStack, out int t0);
			string ret = obj.PadLeft(t0);
			Push(L, ret);
			return 1;
		}
		else if(n == 2)
		{
			const int startStack = 2;
			var obj = SharpObject.Get<System.String>(L, 1);
			Get(L, 0 + startStack, out int t0);
			Get(L, 1 + startStack, out char t1);
			string ret = obj.PadLeft(t0, t1);
			Push(L, ret);
			return 1;
		}
		return 0;
	}

	[AOT.MonoPInvokeCallback(typeof(LuaNativeFunction))]
	static int PadRight(IntPtr L)
	{
		int n = lua_gettop(L) - 1;
		if(n == 1)
		{
			const int startStack = 2;
			var obj = SharpObject.Get<System.String>(L, 1);
			Get(L, 0 + startStack, out int t0);
			string ret = obj.PadRight(t0);
			Push(L, ret);
			return 1;
		}
		else if(n == 2)
		{
			const int startStack = 2;
			var obj = SharpObject.Get<System.String>(L, 1);
			Get(L, 0 + startStack, out int t0);
			Get(L, 1 + startStack, out char t1);
			string ret = obj.PadRight(t0, t1);
			Push(L, ret);
			return 1;
		}
		return 0;
	}

	[AOT.MonoPInvokeCallback(typeof(LuaNativeFunction))]
	static int StartsWith(IntPtr L)
	{
		int n = lua_gettop(L) - 1;
		if(n == 1)
		{
			const int startStack = 2;
			var obj = SharpObject.Get<System.String>(L, 1);
			Get(L, 0 + startStack, out string t0);
			bool ret = obj.StartsWith(t0);
			Push(L, ret);
			return 1;
		}
		else if(n == 2)
		{
			const int startStack = 2;
			var obj = SharpObject.Get<System.String>(L, 1);
			Get(L, 0 + startStack, out string t0);
			Get(L, 1 + startStack, out System.StringComparison t1);
			bool ret = obj.StartsWith(t0, t1);
			Push(L, ret);
			return 1;
		}
		else if(n == 3)
		{
			const int startStack = 2;
			var obj = SharpObject.Get<System.String>(L, 1);
			Get(L, 0 + startStack, out string t0);
			Get(L, 1 + startStack, out bool t1);
			Get(L, 2 + startStack, out System.Globalization.CultureInfo t2);
			bool ret = obj.StartsWith(t0, t1, t2);
			Push(L, ret);
			return 1;
		}
		return 0;
	}

	[AOT.MonoPInvokeCallback(typeof(LuaNativeFunction))]
	static int ToLower(IntPtr L)
	{
		int n = lua_gettop(L) - 1;
		if(n == 0)
		{
			var obj = SharpObject.Get<System.String>(L, 1);
			string ret = obj.ToLower();
			Push(L, ret);
			return 1;
		}
		else if(n == 1)
		{
			const int startStack = 2;
			var obj = SharpObject.Get<System.String>(L, 1);
			Get(L, 0 + startStack, out System.Globalization.CultureInfo t0);
			string ret = obj.ToLower(t0);
			Push(L, ret);
			return 1;
		}
		return 0;
	}

	[AOT.MonoPInvokeCallback(typeof(LuaNativeFunction))]
	static int ToLowerInvariant(IntPtr L)
	{
		var obj = SharpObject.Get<System.String>(L, 1);
		string ret = obj.ToLowerInvariant();
		Push(L, ret);
		return 1;
	}

	[AOT.MonoPInvokeCallback(typeof(LuaNativeFunction))]
	static int ToUpper(IntPtr L)
	{
		int n = lua_gettop(L) - 1;
		if(n == 0)
		{
			var obj = SharpObject.Get<System.String>(L, 1);
			string ret = obj.ToUpper();
			Push(L, ret);
			return 1;
		}
		else if(n == 1)
		{
			const int startStack = 2;
			var obj = SharpObject.Get<System.String>(L, 1);
			Get(L, 0 + startStack, out System.Globalization.CultureInfo t0);
			string ret = obj.ToUpper(t0);
			Push(L, ret);
			return 1;
		}
		return 0;
	}

	[AOT.MonoPInvokeCallback(typeof(LuaNativeFunction))]
	static int ToUpperInvariant(IntPtr L)
	{
		var obj = SharpObject.Get<System.String>(L, 1);
		string ret = obj.ToUpperInvariant();
		Push(L, ret);
		return 1;
	}

	[AOT.MonoPInvokeCallback(typeof(LuaNativeFunction))]
	static int ToString(IntPtr L)
	{
		int n = lua_gettop(L) - 1;
		if(n == 0)
		{
			var obj = SharpObject.Get<System.String>(L, 1);
			string ret = obj.ToString();
			Push(L, ret);
			return 1;
		}
		else if(n == 1)
		{
			const int startStack = 2;
			var obj = SharpObject.Get<System.String>(L, 1);
			Get(L, 0 + startStack, out System.IFormatProvider t0);
			string ret = obj.ToString(t0);
			Push(L, ret);
			return 1;
		}
		return 0;
	}

	[AOT.MonoPInvokeCallback(typeof(LuaNativeFunction))]
	static int Clone(IntPtr L)
	{
		var obj = SharpObject.Get<System.String>(L, 1);
		object ret = obj.Clone();
		Push(L, ret);
		return 1;
	}

	[AOT.MonoPInvokeCallback(typeof(LuaNativeFunction))]
	static int Insert(IntPtr L)
	{
		const int startStack = 2;
		var obj = SharpObject.Get<System.String>(L, 1);
		Get(L, 0 + startStack, out int t0);
		Get(L, 1 + startStack, out string t1);
		string ret = obj.Insert(t0, t1);
		Push(L, ret);
		return 1;
	}

	[AOT.MonoPInvokeCallback(typeof(LuaNativeFunction))]
	static int Replace(IntPtr L)
	{
		int n = lua_gettop(L) - 1;
		if(n == 2 && CheckType<char, char>(L, 1))
		{
			const int startStack = 2;
			var obj = SharpObject.Get<System.String>(L, 1);
			Get(L, 0 + startStack, out char t0);
			Get(L, 1 + startStack, out char t1);
			string ret = obj.Replace(t0, t1);
			Push(L, ret);
			return 1;
		}
		else if(n == 2 && CheckType<string, string>(L, 1))
		{
			const int startStack = 2;
			var obj = SharpObject.Get<System.String>(L, 1);
			Get(L, 0 + startStack, out string t0);
			Get(L, 1 + startStack, out string t1);
			string ret = obj.Replace(t0, t1);
			Push(L, ret);
			return 1;
		}
		return 0;
	}

	[AOT.MonoPInvokeCallback(typeof(LuaNativeFunction))]
	static int Remove(IntPtr L)
	{
		int n = lua_gettop(L) - 1;
		if(n == 1)
		{
			const int startStack = 2;
			var obj = SharpObject.Get<System.String>(L, 1);
			Get(L, 0 + startStack, out int t0);
			string ret = obj.Remove(t0);
			Push(L, ret);
			return 1;
		}
		else if(n == 2)
		{
			const int startStack = 2;
			var obj = SharpObject.Get<System.String>(L, 1);
			Get(L, 0 + startStack, out int t0);
			Get(L, 1 + startStack, out int t1);
			string ret = obj.Remove(t0, t1);
			Push(L, ret);
			return 1;
		}
		return 0;
	}

	[AOT.MonoPInvokeCallback(typeof(LuaNativeFunction))]
	static int Format(IntPtr L)
	{
		int n = lua_gettop(L) - 1;
		if(n == 2 && CheckType<string, object>(L, 1))
		{
			#if LUNA_SCRIPT
			const int startStack = 2;
			#else
			const int startStack = 1;
			#endif
			Get(L, 0 + startStack, out string t0);
			Get(L, 1 + startStack, out object t1);
			string ret = System.String.Format(t0, t1);
			Push(L, ret);
			return 1;
		}
		else if(n == 2 && CheckType<string, object[]>(L, 1))
		{
			#if LUNA_SCRIPT
			const int startStack = 2;
			#else
			const int startStack = 1;
			#endif
			Get(L, 0 + startStack, out string t0);
			Get(L, 1 + startStack, out object[] t1);
			string ret = System.String.Format(t0, t1);
			Push(L, ret);
			return 1;
		}
		else if(n == 3 && CheckType<string, object, object>(L, 1))
		{
			#if LUNA_SCRIPT
			const int startStack = 2;
			#else
			const int startStack = 1;
			#endif
			Get(L, 0 + startStack, out string t0);
			Get(L, 1 + startStack, out object t1);
			Get(L, 2 + startStack, out object t2);
			string ret = System.String.Format(t0, t1, t2);
			Push(L, ret);
			return 1;
		}
		else if(n == 3 && CheckType<System.IFormatProvider, string, object>(L, 1))
		{
			#if LUNA_SCRIPT
			const int startStack = 2;
			#else
			const int startStack = 1;
			#endif
			Get(L, 0 + startStack, out System.IFormatProvider t0);
			Get(L, 1 + startStack, out string t1);
			Get(L, 2 + startStack, out object t2);
			string ret = System.String.Format(t0, t1, t2);
			Push(L, ret);
			return 1;
		}
		else if(n == 3 && CheckType<System.IFormatProvider, string, object[]>(L, 1))
		{
			#if LUNA_SCRIPT
			const int startStack = 2;
			#else
			const int startStack = 1;
			#endif
			Get(L, 0 + startStack, out System.IFormatProvider t0);
			Get(L, 1 + startStack, out string t1);
			Get(L, 2 + startStack, out object[] t2);
			string ret = System.String.Format(t0, t1, t2);
			Push(L, ret);
			return 1;
		}
		else if(n == 4 && CheckType<string, object, object, object>(L, 1))
		{
			#if LUNA_SCRIPT
			const int startStack = 2;
			#else
			const int startStack = 1;
			#endif
			Get(L, 0 + startStack, out string t0);
			Get(L, 1 + startStack, out object t1);
			Get(L, 2 + startStack, out object t2);
			Get(L, 3 + startStack, out object t3);
			string ret = System.String.Format(t0, t1, t2, t3);
			Push(L, ret);
			return 1;
		}
		else if(n == 4 && CheckType<System.IFormatProvider, string, object, object>(L, 1))
		{
			#if LUNA_SCRIPT
			const int startStack = 2;
			#else
			const int startStack = 1;
			#endif
			Get(L, 0 + startStack, out System.IFormatProvider t0);
			Get(L, 1 + startStack, out string t1);
			Get(L, 2 + startStack, out object t2);
			Get(L, 3 + startStack, out object t3);
			string ret = System.String.Format(t0, t1, t2, t3);
			Push(L, ret);
			return 1;
		}
		else if(n == 5)
		{
			#if LUNA_SCRIPT
			const int startStack = 2;
			#else
			const int startStack = 1;
			#endif
			Get(L, 0 + startStack, out System.IFormatProvider t0);
			Get(L, 1 + startStack, out string t1);
			Get(L, 2 + startStack, out object t2);
			Get(L, 3 + startStack, out object t3);
			Get(L, 4 + startStack, out object t4);
			string ret = System.String.Format(t0, t1, t2, t3, t4);
			Push(L, ret);
			return 1;
		}
		return 0;
	}

	[AOT.MonoPInvokeCallback(typeof(LuaNativeFunction))]
	static int Copy(IntPtr L)
	{
		#if LUNA_SCRIPT
		const int startStack = 2;
		#else
		const int startStack = 1;
		#endif
		Get(L, 0 + startStack, out string t0);
		string ret = System.String.Copy(t0);
		Push(L, ret);
		return 1;
	}

	[AOT.MonoPInvokeCallback(typeof(LuaNativeFunction))]
	static int Concat(IntPtr L)
	{
		int n = lua_gettop(L) - 1;
		if(n == 1 && CheckType<object>(L, 1))
		{
			#if LUNA_SCRIPT
			const int startStack = 2;
			#else
			const int startStack = 1;
			#endif
			Get(L, 0 + startStack, out object t0);
			string ret = System.String.Concat(t0);
			Push(L, ret);
			return 1;
		}
		else if(n == 1 && CheckType<object[]>(L, 1))
		{
			#if LUNA_SCRIPT
			const int startStack = 2;
			#else
			const int startStack = 1;
			#endif
			Get(L, 0 + startStack, out object[] t0);
			string ret = System.String.Concat(t0);
			Push(L, ret);
			return 1;
		}
		else if(n == 1 && CheckType<string[]>(L, 1))
		{
			#if LUNA_SCRIPT
			const int startStack = 2;
			#else
			const int startStack = 1;
			#endif
			Get(L, 0 + startStack, out string[] t0);
			string ret = System.String.Concat(t0);
			Push(L, ret);
			return 1;
		}
		else if(n == 2 && CheckType<object, object>(L, 1))
		{
			#if LUNA_SCRIPT
			const int startStack = 2;
			#else
			const int startStack = 1;
			#endif
			Get(L, 0 + startStack, out object t0);
			Get(L, 1 + startStack, out object t1);
			string ret = System.String.Concat(t0, t1);
			Push(L, ret);
			return 1;
		}
		else if(n == 2 && CheckType<string, string>(L, 1))
		{
			#if LUNA_SCRIPT
			const int startStack = 2;
			#else
			const int startStack = 1;
			#endif
			Get(L, 0 + startStack, out string t0);
			Get(L, 1 + startStack, out string t1);
			string ret = System.String.Concat(t0, t1);
			Push(L, ret);
			return 1;
		}
		else if(n == 3 && CheckType<object, object, object>(L, 1))
		{
			#if LUNA_SCRIPT
			const int startStack = 2;
			#else
			const int startStack = 1;
			#endif
			Get(L, 0 + startStack, out object t0);
			Get(L, 1 + startStack, out object t1);
			Get(L, 2 + startStack, out object t2);
			string ret = System.String.Concat(t0, t1, t2);
			Push(L, ret);
			return 1;
		}
		else if(n == 3 && CheckType<string, string, string>(L, 1))
		{
			#if LUNA_SCRIPT
			const int startStack = 2;
			#else
			const int startStack = 1;
			#endif
			Get(L, 0 + startStack, out string t0);
			Get(L, 1 + startStack, out string t1);
			Get(L, 2 + startStack, out string t2);
			string ret = System.String.Concat(t0, t1, t2);
			Push(L, ret);
			return 1;
		}
		else if(n == 4 && CheckType<object, object, object, object>(L, 1))
		{
			#if LUNA_SCRIPT
			const int startStack = 2;
			#else
			const int startStack = 1;
			#endif
			Get(L, 0 + startStack, out object t0);
			Get(L, 1 + startStack, out object t1);
			Get(L, 2 + startStack, out object t2);
			Get(L, 3 + startStack, out object t3);
			string ret = System.String.Concat(t0, t1, t2, t3);
			Push(L, ret);
			return 1;
		}
		else if(n == 4 && CheckType<string, string, string, string>(L, 1))
		{
			#if LUNA_SCRIPT
			const int startStack = 2;
			#else
			const int startStack = 1;
			#endif
			Get(L, 0 + startStack, out string t0);
			Get(L, 1 + startStack, out string t1);
			Get(L, 2 + startStack, out string t2);
			Get(L, 3 + startStack, out string t3);
			string ret = System.String.Concat(t0, t1, t2, t3);
			Push(L, ret);
			return 1;
		}
		return 0;
	}

	[AOT.MonoPInvokeCallback(typeof(LuaNativeFunction))]
	static int Intern(IntPtr L)
	{
		#if LUNA_SCRIPT
		const int startStack = 2;
		#else
		const int startStack = 1;
		#endif
		Get(L, 0 + startStack, out string t0);
		string ret = System.String.Intern(t0);
		Push(L, ret);
		return 1;
	}

	[AOT.MonoPInvokeCallback(typeof(LuaNativeFunction))]
	static int IsInterned(IntPtr L)
	{
		#if LUNA_SCRIPT
		const int startStack = 2;
		#else
		const int startStack = 1;
		#endif
		Get(L, 0 + startStack, out string t0);
		string ret = System.String.IsInterned(t0);
		Push(L, ret);
		return 1;
	}

	[AOT.MonoPInvokeCallback(typeof(LuaNativeFunction))]
	static int GetTypeCode(IntPtr L)
	{
		var obj = SharpObject.Get<System.String>(L, 1);
		System.TypeCode ret = obj.GetTypeCode();
		Push(L, ret);
		return 1;
	}

	[AOT.MonoPInvokeCallback(typeof(LuaNativeFunction))]
	static int GetType(IntPtr L)
	{
		var obj = SharpObject.Get<System.String>(L, 1);
		System.Type ret = obj.GetType();
		Push(L, ret);
		return 1;
	}

	public static void Register(ClassWraper classWraper)
	{
		classWraper.RegConstructor(Constructor);
		classWraper.RegField("Empty", Get_Empty);
		classWraper.RegProperty("Length", Get_Length);
		classWraper.RegFunction("Join", Join);
		classWraper.RegFunction("Equals", Equals);
		classWraper.RegFunction("CopyTo", CopyTo);
		classWraper.RegFunction("ToCharArray", ToCharArray);
		classWraper.RegFunction("IsNullOrEmpty", IsNullOrEmpty);
		classWraper.RegFunction("IsNullOrWhiteSpace", IsNullOrWhiteSpace);
		classWraper.RegFunction("GetHashCode", GetHashCode);
		classWraper.RegFunction("Split", Split);
		classWraper.RegFunction("Substring", Substring);
		classWraper.RegFunction("Trim", Trim);
		classWraper.RegFunction("TrimStart", TrimStart);
		classWraper.RegFunction("TrimEnd", TrimEnd);
		classWraper.RegFunction("IsNormalized", IsNormalized);
		classWraper.RegFunction("Normalize", Normalize);
		classWraper.RegFunction("Compare", Compare);
		classWraper.RegFunction("CompareTo", CompareTo);
		classWraper.RegFunction("CompareOrdinal", CompareOrdinal);
		classWraper.RegFunction("Contains", Contains);
		classWraper.RegFunction("EndsWith", EndsWith);
		classWraper.RegFunction("IndexOf", IndexOf);
		classWraper.RegFunction("IndexOfAny", IndexOfAny);
		classWraper.RegFunction("LastIndexOf", LastIndexOf);
		classWraper.RegFunction("LastIndexOfAny", LastIndexOfAny);
		classWraper.RegFunction("PadLeft", PadLeft);
		classWraper.RegFunction("PadRight", PadRight);
		classWraper.RegFunction("StartsWith", StartsWith);
		classWraper.RegFunction("ToLower", ToLower);
		classWraper.RegFunction("ToLowerInvariant", ToLowerInvariant);
		classWraper.RegFunction("ToUpper", ToUpper);
		classWraper.RegFunction("ToUpperInvariant", ToUpperInvariant);
		classWraper.RegFunction("ToString", ToString);
		classWraper.RegFunction("Clone", Clone);
		classWraper.RegFunction("Insert", Insert);
		classWraper.RegFunction("Replace", Replace);
		classWraper.RegFunction("Remove", Remove);
		classWraper.RegFunction("Format", Format);
		classWraper.RegFunction("Copy", Copy);
		classWraper.RegFunction("Concat", Concat);
		classWraper.RegFunction("Intern", Intern);
		classWraper.RegFunction("IsInterned", IsInterned);
		classWraper.RegFunction("GetTypeCode", GetTypeCode);
		classWraper.RegFunction("GetType", GetType);
	}
}