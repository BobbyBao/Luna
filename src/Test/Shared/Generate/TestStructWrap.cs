using System;
using SharpLuna;

[WrapClass(typeof(Tests.TestStruct))]
class TestStructWrap
{
	[AOT.MonoPInvokeCallback(typeof(LuaNativeFunction))]
	[WrapMethod("ctor", MethodType.Normal)]
	static int ctor_1(LuaState L)
	{
		var obj = new Tests.TestStruct(
			Lua.Get<System.Single>(L, 1)
		);
		Lua.Push(L, obj);
		return 1;
	}

	[AOT.MonoPInvokeCallback(typeof(LuaNativeFunction))]
	[WrapMethod("ctor", MethodType.Normal)]
	static int ctor_2(LuaState L)
	{
		var obj = new Tests.TestStruct(
			Lua.Get<System.Single>(L, 1),
			Lua.Get<System.Single>(L, 2)
		);
		Lua.Push(L, obj);
		return 1;
	}

	[AOT.MonoPInvokeCallback(typeof(LuaNativeFunction))]
	[WrapMethod("ctor", MethodType.Normal)]
	static int ctor_3(LuaState L)
	{
		var obj = new Tests.TestStruct(
			Lua.Get<System.Single>(L, 1),
			Lua.Get<System.Single>(L, 2),
			Lua.Get<System.Single>(L, 3)
		);
		Lua.Push(L, obj);
		return 1;
	}

	[AOT.MonoPInvokeCallback(typeof(LuaNativeFunction))]
	[WrapMethod("x", MethodType.Getter)]
	static int get_x(LuaState L)
	{
		ref var obj = ref SharpObject.GetValue<Tests.TestStruct>(L, 1);
		Lua.Push(L, obj.x);
		return 1;
	}

	[AOT.MonoPInvokeCallback(typeof(LuaNativeFunction))]
	[WrapMethod("x", MethodType.Setter)]
	static int set_x(LuaState L)
	{
		ref var obj = ref SharpObject.GetValue<Tests.TestStruct>(L, 1);
		var p1 = Lua.Get<System.Single>(L, 2);
		obj.x = p1;
		return 0;
	}

	[AOT.MonoPInvokeCallback(typeof(LuaNativeFunction))]
	[WrapMethod("y", MethodType.Getter)]
	static int get_y(LuaState L)
	{
		ref var obj = ref SharpObject.GetValue<Tests.TestStruct>(L, 1);
		Lua.Push(L, obj.y);
		return 1;
	}

	[AOT.MonoPInvokeCallback(typeof(LuaNativeFunction))]
	[WrapMethod("y", MethodType.Setter)]
	static int set_y(LuaState L)
	{
		ref var obj = ref SharpObject.GetValue<Tests.TestStruct>(L, 1);
		var p1 = Lua.Get<System.Single>(L, 2);
		obj.y = p1;
		return 0;
	}

	[AOT.MonoPInvokeCallback(typeof(LuaNativeFunction))]
	[WrapMethod("z", MethodType.Getter)]
	static int get_z(LuaState L)
	{
		ref var obj = ref SharpObject.GetValue<Tests.TestStruct>(L, 1);
		Lua.Push(L, obj.z);
		return 1;
	}

	[AOT.MonoPInvokeCallback(typeof(LuaNativeFunction))]
	[WrapMethod("z", MethodType.Setter)]
	static int set_z(LuaState L)
	{
		ref var obj = ref SharpObject.GetValue<Tests.TestStruct>(L, 1);
		var p1 = Lua.Get<System.Single>(L, 2);
		obj.z = p1;
		return 0;
	}

}