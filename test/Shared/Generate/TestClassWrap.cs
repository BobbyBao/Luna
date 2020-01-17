using System;
using SharpLuna;
using static SharpLuna.Lua;

[WrapClass(typeof(Tests.TestClass))]
public class TestClassWrap
{
	[AOT.MonoPInvokeCallback(typeof(LuaNativeFunction))]
	static int Constructor(IntPtr L)
	{
		int n = lua_gettop(L);
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
		var obj = SharpObject.Get<Tests.TestClass>(L, 1);
		Lua.Push(L, Tests.TestClass.staticVar);
		return 1;
	}

	[AOT.MonoPInvokeCallback(typeof(LuaNativeFunction))]
	static int Set_staticVar(IntPtr L)
	{
		var obj = SharpObject.Get<Tests.TestClass>(L, 1);
		var p1 = Lua.Get<string>(L, 2);
		Tests.TestClass.staticVar = p1;
		return 0;
	}

	[AOT.MonoPInvokeCallback(typeof(LuaNativeFunction))]
	static int Get_staticProp(IntPtr L)
	{
		var obj = SharpObject.Get<Tests.TestClass>(L, 1);
		Lua.Push(L, Tests.TestClass.staticProp);
		return 1;
	}

	[AOT.MonoPInvokeCallback(typeof(LuaNativeFunction))]
	static int Set_staticProp(IntPtr L)
	{
		var obj = SharpObject.Get<Tests.TestClass>(L, 1);
		var p1 = Lua.Get<string>(L, 2);
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

	public static void Register(ClassWraper classWraper)
	{
		classWraper.RegFunction("ctor", Constructor);
		classWraper.RegField("variable", Get_variable, Set_variable);
		classWraper.RegField("staticVar", Get_staticVar, Set_staticVar);
		classWraper.RegProp("staticProp", Get_staticProp, Set_staticProp);
		classWraper.RegProp("name", Get_name, Set_name);
		classWraper.RegProp("Child", Get_Child, Set_Child);
	}
}