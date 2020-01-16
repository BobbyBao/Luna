using System;
using SharpLuna;
using static SharpLuna.Lua;

[WrapClass(typeof(UnityEngine.Touch))]
public class TouchWrap
{
	[AOT.MonoPInvokeCallback(typeof(LuaNativeFunction))]
	static int Get_fingerId(IntPtr L)
	{
		ref var obj = ref SharpObject.GetValue<UnityEngine.Touch>(L, 1);
		Lua.Push(L, obj.fingerId);
		return 1;
	}

	[AOT.MonoPInvokeCallback(typeof(LuaNativeFunction))]
	static int Set_fingerId(IntPtr L)
	{
		ref var obj = ref SharpObject.GetValue<UnityEngine.Touch>(L, 1);
		var p1 = Lua.Get<int>(L, 2);
		obj.fingerId = p1;
		return 0;
	}

	[AOT.MonoPInvokeCallback(typeof(LuaNativeFunction))]
	static int Get_position(IntPtr L)
	{
		ref var obj = ref SharpObject.GetValue<UnityEngine.Touch>(L, 1);
		Lua.Push(L, obj.position);
		return 1;
	}

	[AOT.MonoPInvokeCallback(typeof(LuaNativeFunction))]
	static int Set_position(IntPtr L)
	{
		ref var obj = ref SharpObject.GetValue<UnityEngine.Touch>(L, 1);
		var p1 = Lua.Get<UnityEngine.Vector2>(L, 2);
		obj.position = p1;
		return 0;
	}

	[AOT.MonoPInvokeCallback(typeof(LuaNativeFunction))]
	static int Get_rawPosition(IntPtr L)
	{
		ref var obj = ref SharpObject.GetValue<UnityEngine.Touch>(L, 1);
		Lua.Push(L, obj.rawPosition);
		return 1;
	}

	[AOT.MonoPInvokeCallback(typeof(LuaNativeFunction))]
	static int Set_rawPosition(IntPtr L)
	{
		ref var obj = ref SharpObject.GetValue<UnityEngine.Touch>(L, 1);
		var p1 = Lua.Get<UnityEngine.Vector2>(L, 2);
		obj.rawPosition = p1;
		return 0;
	}

	[AOT.MonoPInvokeCallback(typeof(LuaNativeFunction))]
	static int Get_deltaPosition(IntPtr L)
	{
		ref var obj = ref SharpObject.GetValue<UnityEngine.Touch>(L, 1);
		Lua.Push(L, obj.deltaPosition);
		return 1;
	}

	[AOT.MonoPInvokeCallback(typeof(LuaNativeFunction))]
	static int Set_deltaPosition(IntPtr L)
	{
		ref var obj = ref SharpObject.GetValue<UnityEngine.Touch>(L, 1);
		var p1 = Lua.Get<UnityEngine.Vector2>(L, 2);
		obj.deltaPosition = p1;
		return 0;
	}

	[AOT.MonoPInvokeCallback(typeof(LuaNativeFunction))]
	static int Get_deltaTime(IntPtr L)
	{
		ref var obj = ref SharpObject.GetValue<UnityEngine.Touch>(L, 1);
		Lua.Push(L, obj.deltaTime);
		return 1;
	}

	[AOT.MonoPInvokeCallback(typeof(LuaNativeFunction))]
	static int Set_deltaTime(IntPtr L)
	{
		ref var obj = ref SharpObject.GetValue<UnityEngine.Touch>(L, 1);
		var p1 = Lua.Get<float>(L, 2);
		obj.deltaTime = p1;
		return 0;
	}

	[AOT.MonoPInvokeCallback(typeof(LuaNativeFunction))]
	static int Get_tapCount(IntPtr L)
	{
		ref var obj = ref SharpObject.GetValue<UnityEngine.Touch>(L, 1);
		Lua.Push(L, obj.tapCount);
		return 1;
	}

	[AOT.MonoPInvokeCallback(typeof(LuaNativeFunction))]
	static int Set_tapCount(IntPtr L)
	{
		ref var obj = ref SharpObject.GetValue<UnityEngine.Touch>(L, 1);
		var p1 = Lua.Get<int>(L, 2);
		obj.tapCount = p1;
		return 0;
	}

	[AOT.MonoPInvokeCallback(typeof(LuaNativeFunction))]
	static int Get_phase(IntPtr L)
	{
		ref var obj = ref SharpObject.GetValue<UnityEngine.Touch>(L, 1);
		Lua.Push(L, obj.phase);
		return 1;
	}

	[AOT.MonoPInvokeCallback(typeof(LuaNativeFunction))]
	static int Set_phase(IntPtr L)
	{
		ref var obj = ref SharpObject.GetValue<UnityEngine.Touch>(L, 1);
		var p1 = Lua.Get<UnityEngine.TouchPhase>(L, 2);
		obj.phase = p1;
		return 0;
	}

	[AOT.MonoPInvokeCallback(typeof(LuaNativeFunction))]
	static int Get_pressure(IntPtr L)
	{
		ref var obj = ref SharpObject.GetValue<UnityEngine.Touch>(L, 1);
		Lua.Push(L, obj.pressure);
		return 1;
	}

	[AOT.MonoPInvokeCallback(typeof(LuaNativeFunction))]
	static int Set_pressure(IntPtr L)
	{
		ref var obj = ref SharpObject.GetValue<UnityEngine.Touch>(L, 1);
		var p1 = Lua.Get<float>(L, 2);
		obj.pressure = p1;
		return 0;
	}

	[AOT.MonoPInvokeCallback(typeof(LuaNativeFunction))]
	static int Get_maximumPossiblePressure(IntPtr L)
	{
		ref var obj = ref SharpObject.GetValue<UnityEngine.Touch>(L, 1);
		Lua.Push(L, obj.maximumPossiblePressure);
		return 1;
	}

	[AOT.MonoPInvokeCallback(typeof(LuaNativeFunction))]
	static int Set_maximumPossiblePressure(IntPtr L)
	{
		ref var obj = ref SharpObject.GetValue<UnityEngine.Touch>(L, 1);
		var p1 = Lua.Get<float>(L, 2);
		obj.maximumPossiblePressure = p1;
		return 0;
	}

	[AOT.MonoPInvokeCallback(typeof(LuaNativeFunction))]
	static int Get_type(IntPtr L)
	{
		ref var obj = ref SharpObject.GetValue<UnityEngine.Touch>(L, 1);
		Lua.Push(L, obj.type);
		return 1;
	}

	[AOT.MonoPInvokeCallback(typeof(LuaNativeFunction))]
	static int Set_type(IntPtr L)
	{
		ref var obj = ref SharpObject.GetValue<UnityEngine.Touch>(L, 1);
		var p1 = Lua.Get<UnityEngine.TouchType>(L, 2);
		obj.type = p1;
		return 0;
	}

	[AOT.MonoPInvokeCallback(typeof(LuaNativeFunction))]
	static int Get_altitudeAngle(IntPtr L)
	{
		ref var obj = ref SharpObject.GetValue<UnityEngine.Touch>(L, 1);
		Lua.Push(L, obj.altitudeAngle);
		return 1;
	}

	[AOT.MonoPInvokeCallback(typeof(LuaNativeFunction))]
	static int Set_altitudeAngle(IntPtr L)
	{
		ref var obj = ref SharpObject.GetValue<UnityEngine.Touch>(L, 1);
		var p1 = Lua.Get<float>(L, 2);
		obj.altitudeAngle = p1;
		return 0;
	}

	[AOT.MonoPInvokeCallback(typeof(LuaNativeFunction))]
	static int Get_azimuthAngle(IntPtr L)
	{
		ref var obj = ref SharpObject.GetValue<UnityEngine.Touch>(L, 1);
		Lua.Push(L, obj.azimuthAngle);
		return 1;
	}

	[AOT.MonoPInvokeCallback(typeof(LuaNativeFunction))]
	static int Set_azimuthAngle(IntPtr L)
	{
		ref var obj = ref SharpObject.GetValue<UnityEngine.Touch>(L, 1);
		var p1 = Lua.Get<float>(L, 2);
		obj.azimuthAngle = p1;
		return 0;
	}

	[AOT.MonoPInvokeCallback(typeof(LuaNativeFunction))]
	static int Get_radius(IntPtr L)
	{
		ref var obj = ref SharpObject.GetValue<UnityEngine.Touch>(L, 1);
		Lua.Push(L, obj.radius);
		return 1;
	}

	[AOT.MonoPInvokeCallback(typeof(LuaNativeFunction))]
	static int Set_radius(IntPtr L)
	{
		ref var obj = ref SharpObject.GetValue<UnityEngine.Touch>(L, 1);
		var p1 = Lua.Get<float>(L, 2);
		obj.radius = p1;
		return 0;
	}

	[AOT.MonoPInvokeCallback(typeof(LuaNativeFunction))]
	static int Get_radiusVariance(IntPtr L)
	{
		ref var obj = ref SharpObject.GetValue<UnityEngine.Touch>(L, 1);
		Lua.Push(L, obj.radiusVariance);
		return 1;
	}

	[AOT.MonoPInvokeCallback(typeof(LuaNativeFunction))]
	static int Set_radiusVariance(IntPtr L)
	{
		ref var obj = ref SharpObject.GetValue<UnityEngine.Touch>(L, 1);
		var p1 = Lua.Get<float>(L, 2);
		obj.radiusVariance = p1;
		return 0;
	}

	public static void Register(ClassWraper classWraper)
	{
		classWraper.RegProp("fingerId", Get_fingerId, Set_fingerId);
		classWraper.RegProp("position", Get_position, Set_position);
		classWraper.RegProp("rawPosition", Get_rawPosition, Set_rawPosition);
		classWraper.RegProp("deltaPosition", Get_deltaPosition, Set_deltaPosition);
		classWraper.RegProp("deltaTime", Get_deltaTime, Set_deltaTime);
		classWraper.RegProp("tapCount", Get_tapCount, Set_tapCount);
		classWraper.RegProp("phase", Get_phase, Set_phase);
		classWraper.RegProp("pressure", Get_pressure, Set_pressure);
		classWraper.RegProp("maximumPossiblePressure", Get_maximumPossiblePressure, Set_maximumPossiblePressure);
		classWraper.RegProp("type", Get_type, Set_type);
		classWraper.RegProp("altitudeAngle", Get_altitudeAngle, Set_altitudeAngle);
		classWraper.RegProp("azimuthAngle", Get_azimuthAngle, Set_azimuthAngle);
		classWraper.RegProp("radius", Get_radius, Set_radius);
		classWraper.RegProp("radiusVariance", Get_radiusVariance, Set_radiusVariance);
	}
}