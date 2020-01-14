using System;
using SharpLuna;
using static SharpLuna.Lua;

[WrapClass(typeof(Tests.TestStruct))]
public class TestStructWrap
{
	[AOT.MonoPInvokeCallback(typeof(LuaNativeFunction))]
	[WrapMethod("ctor", MethodType.Normal)]
	static int Constructor(LuaState L)
	{
		var obj = new Tests.TestStruct(
			Lua.Get<float>(L, 1)
		);
		Lua.Push(L, obj);
		return 1;
	}

	[AOT.MonoPInvokeCallback(typeof(LuaNativeFunction))]
	[WrapMethod("x", MethodType.Getter)]
	static int Get_x(LuaState L)
	{
		ref var obj = ref SharpObject.GetValue<Tests.TestStruct>(L, 1);
		Lua.Push(L, obj.x);
		return 1;
	}

	[AOT.MonoPInvokeCallback(typeof(LuaNativeFunction))]
	[WrapMethod("x", MethodType.Setter)]
	static int Set_x(LuaState L)
	{
		ref var obj = ref SharpObject.GetValue<Tests.TestStruct>(L, 1);
		var p1 = Lua.Get<float>(L, 2);
		obj.x = p1;
		return 0;
	}

	[AOT.MonoPInvokeCallback(typeof(LuaNativeFunction))]
	[WrapMethod("y", MethodType.Getter)]
	static int Get_y(LuaState L)
	{
		ref var obj = ref SharpObject.GetValue<Tests.TestStruct>(L, 1);
		Lua.Push(L, obj.y);
		return 1;
	}

	[AOT.MonoPInvokeCallback(typeof(LuaNativeFunction))]
	[WrapMethod("y", MethodType.Setter)]
	static int Set_y(LuaState L)
	{
		ref var obj = ref SharpObject.GetValue<Tests.TestStruct>(L, 1);
		var p1 = Lua.Get<float>(L, 2);
		obj.y = p1;
		return 0;
	}

	[AOT.MonoPInvokeCallback(typeof(LuaNativeFunction))]
	[WrapMethod("z", MethodType.Getter)]
	static int Get_z(LuaState L)
	{
		ref var obj = ref SharpObject.GetValue<Tests.TestStruct>(L, 1);
		Lua.Push(L, obj.z);
		return 1;
	}

	[AOT.MonoPInvokeCallback(typeof(LuaNativeFunction))]
	[WrapMethod("z", MethodType.Setter)]
	static int Set_z(LuaState L)
	{
		ref var obj = ref SharpObject.GetValue<Tests.TestStruct>(L, 1);
		var p1 = Lua.Get<float>(L, 2);
		obj.z = p1;
		return 0;
	}

}