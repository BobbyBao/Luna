using System;
using SharpLuna;
using static SharpLuna.Lua;

[WrapClass(typeof(Tests.TestClass))]
public class TestClassWrap
{
	[AOT.MonoPInvokeCallback(typeof(LuaNativeFunction))]
	static int Constructor(IntPtr L)
	{
		int n = lua_gettop(L) - 1;
		Tests.TestClass obj = default;
		if(n == 0)
			obj = new Tests.TestClass();
		Lua.Push(L, obj);
		return 1;
	}

	[AOT.MonoPInvokeCallback(typeof(LuaNativeFunction))]
	static int Get_variable(IntPtr L)
	{
		var obj = SharpObject.Get<Tests.TestClass>(L, 1);
		Lua.Push(L, obj.variable);
		return 1;
	}

	[AOT.MonoPInvokeCallback(typeof(LuaNativeFunction))]
	static int Set_variable(IntPtr L)
	{
		var obj = SharpObject.Get<Tests.TestClass>(L, 1);
		var p1 = Lua.Get<string>(L, 2);
		obj.variable = p1;
		return 0;
	}

	[AOT.MonoPInvokeCallback(typeof(LuaNativeFunction))]
	static int Get_staticVar(IntPtr L)
	{
		Lua.Push(L, Tests.TestClass.staticVar);
		return 1;
	}

	[AOT.MonoPInvokeCallback(typeof(LuaNativeFunction))]
	static int Set_staticVar(IntPtr L)
	{
		var p1 = Lua.Get<string>(L, 1);
		Tests.TestClass.staticVar = p1;
		return 0;
	}

	[AOT.MonoPInvokeCallback(typeof(LuaNativeFunction))]
	static int Get_staticProp(IntPtr L)
	{
		Lua.Push(L, Tests.TestClass.staticProp);
		return 1;
	}

	[AOT.MonoPInvokeCallback(typeof(LuaNativeFunction))]
	static int Set_staticProp(IntPtr L)
	{
		var p1 = Lua.Get<string>(L, 1);
		Tests.TestClass.staticProp = p1;
		return 0;
	}

	[AOT.MonoPInvokeCallback(typeof(LuaNativeFunction))]
	static int Get_name(IntPtr L)
	{
		var obj = SharpObject.Get<Tests.TestClass>(L, 1);
		Lua.Push(L, obj.name);
		return 1;
	}

	[AOT.MonoPInvokeCallback(typeof(LuaNativeFunction))]
	static int Set_name(IntPtr L)
	{
		var obj = SharpObject.Get<Tests.TestClass>(L, 1);
		var p1 = Lua.Get<string>(L, 2);
		obj.name = p1;
		return 0;
	}

	[AOT.MonoPInvokeCallback(typeof(LuaNativeFunction))]
	static int Get_Child(IntPtr L)
	{
		var obj = SharpObject.Get<Tests.TestClass>(L, 1);
		Lua.Push(L, obj.Child);
		return 1;
	}

	[AOT.MonoPInvokeCallback(typeof(LuaNativeFunction))]
	static int Set_Child(IntPtr L)
	{
		var obj = SharpObject.Get<Tests.TestClass>(L, 1);
		var p1 = Lua.Get<Tests.TestClass>(L, 2);
		obj.Child = p1;
		return 0;
	}

	[AOT.MonoPInvokeCallback(typeof(LuaNativeFunction))]
	static int StaticFunc(IntPtr L)
	{
		int n = lua_gettop(L) - 1;
		Tests.TestClass.StaticFunc();
		return 0;
	}

	[AOT.MonoPInvokeCallback(typeof(LuaNativeFunction))]
	static int StaticFunc1(IntPtr L)
	{
		int n = lua_gettop(L) - 1;
		#if LUNA_SCRIPT
		const int startStack = 2;
		#else
		const int startStack = 1;
		#endif
		Tests.TestClass.StaticFunc1(
			Lua.Get<string>(L, 0 + startStack)
		);
		return 0;
	}

	[AOT.MonoPInvokeCallback(typeof(LuaNativeFunction))]
	static int StaticFunc2(IntPtr L)
	{
		int n = lua_gettop(L) - 1;
		#if LUNA_SCRIPT
		const int startStack = 2;
		#else
		const int startStack = 1;
		#endif
		Tests.TestClass.StaticFunc2(
			Lua.Get<string>(L, 0 + startStack),
			Lua.Get<int>(L, 1 + startStack)
		);
		return 0;
	}

	[AOT.MonoPInvokeCallback(typeof(LuaNativeFunction))]
	static int CreateChild(IntPtr L)
	{
		int n = lua_gettop(L) - 1;
		var obj = SharpObject.Get<Tests.TestClass>(L, 1);
		obj.CreateChild();
		return 0;
	}

	[AOT.MonoPInvokeCallback(typeof(LuaNativeFunction))]
	static int Method(IntPtr L)
	{
		int n = lua_gettop(L) - 1;
		var obj = SharpObject.Get<Tests.TestClass>(L, 1);
		obj.Method();
		return 0;
	}

	[AOT.MonoPInvokeCallback(typeof(LuaNativeFunction))]
	static int Method1(IntPtr L)
	{
		int n = lua_gettop(L) - 1;
		const int startStack = 2;
		var obj = SharpObject.Get<Tests.TestClass>(L, 1);
		obj.Method1(
			Lua.Get<string>(L, 0 + startStack)
		);
		return 0;
	}

	[AOT.MonoPInvokeCallback(typeof(LuaNativeFunction))]
	static int Method2(IntPtr L)
	{
		int n = lua_gettop(L) - 1;
		const int startStack = 2;
		var obj = SharpObject.Get<Tests.TestClass>(L, 1);
		obj.Method2(
			Lua.Get<string>(L, 0 + startStack),
			Lua.Get<float>(L, 1 + startStack)
		);
		return 0;
	}

	[AOT.MonoPInvokeCallback(typeof(LuaNativeFunction))]
	static int Fun0(IntPtr L)
	{
		int n = lua_gettop(L) - 1;
		var obj = SharpObject.Get<Tests.TestClass>(L, 1);
		var ret = obj.Fun0();
		Lua.Push(L, ret);
		return 1;
	}

	[AOT.MonoPInvokeCallback(typeof(LuaNativeFunction))]
	static int Fun1(IntPtr L)
	{
		int n = lua_gettop(L) - 1;
		const int startStack = 2;
		var obj = SharpObject.Get<Tests.TestClass>(L, 1);
		var ret = obj.Fun1(
			Lua.Get<int>(L, 0 + startStack)
		);
		Lua.Push(L, ret);
		return 1;
	}

	[AOT.MonoPInvokeCallback(typeof(LuaNativeFunction))]
	static int Fun2(IntPtr L)
	{
		int n = lua_gettop(L) - 1;
		const int startStack = 2;
		var obj = SharpObject.Get<Tests.TestClass>(L, 1);
		var ret = obj.Fun2(
			Lua.Get<int>(L, 0 + startStack)
		);
		Lua.Push(L, ret);
		return 1;
	}

	[AOT.MonoPInvokeCallback(typeof(LuaNativeFunction))]
	static int ToString(IntPtr L)
	{
		int n = lua_gettop(L) - 1;
		var obj = SharpObject.Get<Tests.TestClass>(L, 1);
		var ret = obj.ToString();
		Lua.Push(L, ret);
		return 1;
	}

	[AOT.MonoPInvokeCallback(typeof(LuaNativeFunction))]
	static int Equals(IntPtr L)
	{
		int n = lua_gettop(L) - 1;
		const int startStack = 2;
		var obj = SharpObject.Get<Tests.TestClass>(L, 1);
		var ret = obj.Equals(
			Lua.Get<object>(L, 0 + startStack)
		);
		Lua.Push(L, ret);
		return 1;
	}

	[AOT.MonoPInvokeCallback(typeof(LuaNativeFunction))]
	static int GetHashCode(IntPtr L)
	{
		int n = lua_gettop(L) - 1;
		var obj = SharpObject.Get<Tests.TestClass>(L, 1);
		var ret = obj.GetHashCode();
		Lua.Push(L, ret);
		return 1;
	}

	[AOT.MonoPInvokeCallback(typeof(LuaNativeFunction))]
	static int GetType(IntPtr L)
	{
		int n = lua_gettop(L) - 1;
		var obj = SharpObject.Get<Tests.TestClass>(L, 1);
		var ret = obj.GetType();
		Lua.Push(L, ret);
		return 1;
	}

	public static void Register(ClassWraper classWraper)
	{
		classWraper.RegConstructor(Constructor);
		classWraper.RegField("variable", Get_variable, Set_variable);
		classWraper.RegField("staticVar", Get_staticVar, Set_staticVar);
		classWraper.RegProperty("staticProp", Get_staticProp, Set_staticProp);
		classWraper.RegProperty("name", Get_name, Set_name);
		classWraper.RegProperty("Child", Get_Child, Set_Child);
		classWraper.RegFunction("StaticFunc", StaticFunc);
		classWraper.RegFunction("StaticFunc1", StaticFunc1);
		classWraper.RegFunction("StaticFunc2", StaticFunc2);
		classWraper.RegFunction("CreateChild", CreateChild);
		classWraper.RegFunction("Method", Method);
		classWraper.RegFunction("Method1", Method1);
		classWraper.RegFunction("Method2", Method2);
		classWraper.RegFunction("Fun0", Fun0);
		classWraper.RegFunction("Fun1", Fun1);
		classWraper.RegFunction("Fun2", Fun2);
		classWraper.RegFunction("ToString", ToString);
		classWraper.RegFunction("Equals", Equals);
		classWraper.RegFunction("GetHashCode", GetHashCode);
		classWraper.RegFunction("GetType", GetType);
	}
}