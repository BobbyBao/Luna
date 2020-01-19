using System;
using SharpLuna;
using static SharpLuna.Lua;

[WrapClass(typeof(Tests.TestStruct))]
public class TestStructWrap
{
	[AOT.MonoPInvokeCallback(typeof(LuaNativeFunction))]
	static int Constructor(IntPtr L)
	{
		int n = lua_gettop(L) - 1;
		Tests.TestStruct obj = default;
		if(n == 0)
		{
			obj = new Tests.TestStruct();
		}
		else if(n == 1)
		{
			Get(L, 2, out float t1);
			obj = new Tests.TestStruct(t1);
		}
		else if(n == 2)
		{
			Get(L, 2, out float t1);
			Get(L, 3, out float t2);
			obj = new Tests.TestStruct(t1, t2);
		}
		else if(n == 3)
		{
			Get(L, 2, out float t1);
			Get(L, 3, out float t2);
			Get(L, 4, out float t3);
			obj = new Tests.TestStruct(t1, t2, t3);
		}
		Push(L, obj);
		return 1;
	}

	[AOT.MonoPInvokeCallback(typeof(LuaNativeFunction))]
	static int Get_x(IntPtr L)
	{
		ref var obj = ref SharpObject.GetValue<Tests.TestStruct>(L, 1);
		Push(L, obj.x);
		return 1;
	}

	[AOT.MonoPInvokeCallback(typeof(LuaNativeFunction))]
	static int Set_x(IntPtr L)
	{
		ref var obj = ref SharpObject.GetValue<Tests.TestStruct>(L, 1);
		Get(L, 2, out float p1);
		obj.x = p1;
		return 0;
	}

	[AOT.MonoPInvokeCallback(typeof(LuaNativeFunction))]
	static int Get_y(IntPtr L)
	{
		ref var obj = ref SharpObject.GetValue<Tests.TestStruct>(L, 1);
		Push(L, obj.y);
		return 1;
	}

	[AOT.MonoPInvokeCallback(typeof(LuaNativeFunction))]
	static int Set_y(IntPtr L)
	{
		ref var obj = ref SharpObject.GetValue<Tests.TestStruct>(L, 1);
		Get(L, 2, out float p1);
		obj.y = p1;
		return 0;
	}

	[AOT.MonoPInvokeCallback(typeof(LuaNativeFunction))]
	static int Get_z(IntPtr L)
	{
		ref var obj = ref SharpObject.GetValue<Tests.TestStruct>(L, 1);
		Push(L, obj.z);
		return 1;
	}

	[AOT.MonoPInvokeCallback(typeof(LuaNativeFunction))]
	static int Set_z(IntPtr L)
	{
		ref var obj = ref SharpObject.GetValue<Tests.TestStruct>(L, 1);
		Get(L, 2, out float p1);
		obj.z = p1;
		return 0;
	}

	[AOT.MonoPInvokeCallback(typeof(LuaNativeFunction))]
	static int Normalize(IntPtr L)
	{
		ref var obj = ref SharpObject.GetValue<Tests.TestStruct>(L, 1);
		obj.Normalize();
		return 0;
	}

	[AOT.MonoPInvokeCallback(typeof(LuaNativeFunction))]
	static int Equals(IntPtr L)
	{
		const int startStack = 2;
		ref var obj = ref SharpObject.GetValue<Tests.TestStruct>(L, 1);
		Get(L, 0 + startStack, out object t0);
		var ret = obj.Equals(t0);
		Push(L, ret);
		return 1;
	}

	[AOT.MonoPInvokeCallback(typeof(LuaNativeFunction))]
	static int GetHashCode(IntPtr L)
	{
		ref var obj = ref SharpObject.GetValue<Tests.TestStruct>(L, 1);
		var ret = obj.GetHashCode();
		Push(L, ret);
		return 1;
	}

	[AOT.MonoPInvokeCallback(typeof(LuaNativeFunction))]
	static int ToString(IntPtr L)
	{
		ref var obj = ref SharpObject.GetValue<Tests.TestStruct>(L, 1);
		var ret = obj.ToString();
		Push(L, ret);
		return 1;
	}

	[AOT.MonoPInvokeCallback(typeof(LuaNativeFunction))]
	static int GetType(IntPtr L)
	{
		ref var obj = ref SharpObject.GetValue<Tests.TestStruct>(L, 1);
		var ret = obj.GetType();
		Push(L, ret);
		return 1;
	}

	public static void Register(ClassWraper classWraper)
	{
		classWraper.RegConstructor(Constructor);
		classWraper.RegField("x", Get_x, Set_x);
		classWraper.RegField("y", Get_y, Set_y);
		classWraper.RegField("z", Get_z, Set_z);
		classWraper.RegFunction("Normalize", Normalize);
		classWraper.RegFunction("Equals", Equals);
		classWraper.RegFunction("GetHashCode", GetHashCode);
		classWraper.RegFunction("ToString", ToString);
		classWraper.RegFunction("GetType", GetType);
	}
}