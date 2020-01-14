using System;
using SharpLuna;
using static SharpLuna.Lua;

[WrapClass(typeof(Tests.TestStruct))]
public class TestStructWrap
{
	[AOT.MonoPInvokeCallback(typeof(LuaNativeFunction))]
	static int Constructor(LuaState L)
	{
		int n = lua_gettop(L);
		Tests.TestStruct obj = default;
		if(n == 1)
			obj = new Tests.TestStruct(
				Lua.Get<float>(L, 1)
			);
		else if(n == 2)
			obj = new Tests.TestStruct(
				Lua.Get<float>(L, 1),
				Lua.Get<float>(L, 2)
			);
		else if(n == 3)
			obj = new Tests.TestStruct(
				Lua.Get<float>(L, 1),
				Lua.Get<float>(L, 2),
				Lua.Get<float>(L, 3)
			);
		Lua.Push(L, obj);
		return 1;
	}

	[AOT.MonoPInvokeCallback(typeof(LuaNativeFunction))]
	static int Get_x(LuaState L)
	{
		ref var obj = ref SharpObject.GetValue<Tests.TestStruct>(L, 1);
		Lua.Push(L, obj.x);
		return 1;
	}

	[AOT.MonoPInvokeCallback(typeof(LuaNativeFunction))]
	static int Set_x(LuaState L)
	{
		ref var obj = ref SharpObject.GetValue<Tests.TestStruct>(L, 1);
		var p1 = Lua.Get<float>(L, 2);
		obj.x = p1;
		return 0;
	}

	[AOT.MonoPInvokeCallback(typeof(LuaNativeFunction))]
	static int Get_y(LuaState L)
	{
		ref var obj = ref SharpObject.GetValue<Tests.TestStruct>(L, 1);
		Lua.Push(L, obj.y);
		return 1;
	}

	[AOT.MonoPInvokeCallback(typeof(LuaNativeFunction))]
	static int Set_y(LuaState L)
	{
		ref var obj = ref SharpObject.GetValue<Tests.TestStruct>(L, 1);
		var p1 = Lua.Get<float>(L, 2);
		obj.y = p1;
		return 0;
	}

	[AOT.MonoPInvokeCallback(typeof(LuaNativeFunction))]
	static int Get_z(LuaState L)
	{
		ref var obj = ref SharpObject.GetValue<Tests.TestStruct>(L, 1);
		Lua.Push(L, obj.z);
		return 1;
	}

	[AOT.MonoPInvokeCallback(typeof(LuaNativeFunction))]
	static int Set_z(LuaState L)
	{
		ref var obj = ref SharpObject.GetValue<Tests.TestStruct>(L, 1);
		var p1 = Lua.Get<float>(L, 2);
		obj.z = p1;
		return 0;
	}

	public static void Register(ClassWraper classWraper)
	{
		classWraper.RegFunction("ctor", Constructor);
		classWraper.RegField("x", Get_x, Set_x);
		classWraper.RegField("y", Get_y, Set_y);
		classWraper.RegField("z", Get_z, Set_z);
	}
}