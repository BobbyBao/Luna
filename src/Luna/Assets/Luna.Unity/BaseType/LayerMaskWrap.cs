using System;
using SharpLuna;
using static SharpLuna.Lua;

[WrapClass(typeof(UnityEngine.LayerMask))]
public class LayerMaskWrap
{
	[AOT.MonoPInvokeCallback(typeof(LuaNativeFunction))]
	static int Get_value(IntPtr L)
	{
		ref var obj = ref SharpObject.GetValue<UnityEngine.LayerMask>(L, 1);
		Lua.Push(L, obj.value);
		return 1;
	}

	[AOT.MonoPInvokeCallback(typeof(LuaNativeFunction))]
	static int Set_value(IntPtr L)
	{
		ref var obj = ref SharpObject.GetValue<UnityEngine.LayerMask>(L, 1);
		var p1 = Lua.Get<int>(L, 2);
		obj.value = p1;
		return 0;
	}

	public static void Register(ClassWraper classWraper)
	{
		classWraper.RegProp("value", Get_value, Set_value);
	}
}