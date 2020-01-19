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
		Push(L, obj.fingerId);
		return 1;
	}

	[AOT.MonoPInvokeCallback(typeof(LuaNativeFunction))]
	static int Set_fingerId(IntPtr L)
	{
		ref var obj = ref SharpObject.GetValue<UnityEngine.Touch>(L, 1);
		Get(L, 2, out int p1);
		obj.fingerId = p1;
		return 0;
	}

	[AOT.MonoPInvokeCallback(typeof(LuaNativeFunction))]
	static int Get_position(IntPtr L)
	{
		ref var obj = ref SharpObject.GetValue<UnityEngine.Touch>(L, 1);
		Push(L, obj.position);
		return 1;
	}

	[AOT.MonoPInvokeCallback(typeof(LuaNativeFunction))]
	static int Set_position(IntPtr L)
	{
		ref var obj = ref SharpObject.GetValue<UnityEngine.Touch>(L, 1);
		Get(L, 2, out UnityEngine.Vector2 p1);
		obj.position = p1;
		return 0;
	}

	[AOT.MonoPInvokeCallback(typeof(LuaNativeFunction))]
	static int Get_rawPosition(IntPtr L)
	{
		ref var obj = ref SharpObject.GetValue<UnityEngine.Touch>(L, 1);
		Push(L, obj.rawPosition);
		return 1;
	}

	[AOT.MonoPInvokeCallback(typeof(LuaNativeFunction))]
	static int Set_rawPosition(IntPtr L)
	{
		ref var obj = ref SharpObject.GetValue<UnityEngine.Touch>(L, 1);
		Get(L, 2, out UnityEngine.Vector2 p1);
		obj.rawPosition = p1;
		return 0;
	}

	[AOT.MonoPInvokeCallback(typeof(LuaNativeFunction))]
	static int Get_deltaPosition(IntPtr L)
	{
		ref var obj = ref SharpObject.GetValue<UnityEngine.Touch>(L, 1);
		Push(L, obj.deltaPosition);
		return 1;
	}

	[AOT.MonoPInvokeCallback(typeof(LuaNativeFunction))]
	static int Set_deltaPosition(IntPtr L)
	{
		ref var obj = ref SharpObject.GetValue<UnityEngine.Touch>(L, 1);
		Get(L, 2, out UnityEngine.Vector2 p1);
		obj.deltaPosition = p1;
		return 0;
	}

	[AOT.MonoPInvokeCallback(typeof(LuaNativeFunction))]
	static int Get_deltaTime(IntPtr L)
	{
		ref var obj = ref SharpObject.GetValue<UnityEngine.Touch>(L, 1);
		Push(L, obj.deltaTime);
		return 1;
	}

	[AOT.MonoPInvokeCallback(typeof(LuaNativeFunction))]
	static int Set_deltaTime(IntPtr L)
	{
		ref var obj = ref SharpObject.GetValue<UnityEngine.Touch>(L, 1);
		Get(L, 2, out float p1);
		obj.deltaTime = p1;
		return 0;
	}

	[AOT.MonoPInvokeCallback(typeof(LuaNativeFunction))]
	static int Get_tapCount(IntPtr L)
	{
		ref var obj = ref SharpObject.GetValue<UnityEngine.Touch>(L, 1);
		Push(L, obj.tapCount);
		return 1;
	}

	[AOT.MonoPInvokeCallback(typeof(LuaNativeFunction))]
	static int Set_tapCount(IntPtr L)
	{
		ref var obj = ref SharpObject.GetValue<UnityEngine.Touch>(L, 1);
		Get(L, 2, out int p1);
		obj.tapCount = p1;
		return 0;
	}

	[AOT.MonoPInvokeCallback(typeof(LuaNativeFunction))]
	static int Get_phase(IntPtr L)
	{
		ref var obj = ref SharpObject.GetValue<UnityEngine.Touch>(L, 1);
		Push(L, obj.phase);
		return 1;
	}

	[AOT.MonoPInvokeCallback(typeof(LuaNativeFunction))]
	static int Set_phase(IntPtr L)
	{
		ref var obj = ref SharpObject.GetValue<UnityEngine.Touch>(L, 1);
		Get(L, 2, out UnityEngine.TouchPhase p1);
		obj.phase = p1;
		return 0;
	}

	[AOT.MonoPInvokeCallback(typeof(LuaNativeFunction))]
	static int Get_pressure(IntPtr L)
	{
		ref var obj = ref SharpObject.GetValue<UnityEngine.Touch>(L, 1);
		Push(L, obj.pressure);
		return 1;
	}

	[AOT.MonoPInvokeCallback(typeof(LuaNativeFunction))]
	static int Set_pressure(IntPtr L)
	{
		ref var obj = ref SharpObject.GetValue<UnityEngine.Touch>(L, 1);
		Get(L, 2, out float p1);
		obj.pressure = p1;
		return 0;
	}

	[AOT.MonoPInvokeCallback(typeof(LuaNativeFunction))]
	static int Get_maximumPossiblePressure(IntPtr L)
	{
		ref var obj = ref SharpObject.GetValue<UnityEngine.Touch>(L, 1);
		Push(L, obj.maximumPossiblePressure);
		return 1;
	}

	[AOT.MonoPInvokeCallback(typeof(LuaNativeFunction))]
	static int Set_maximumPossiblePressure(IntPtr L)
	{
		ref var obj = ref SharpObject.GetValue<UnityEngine.Touch>(L, 1);
		Get(L, 2, out float p1);
		obj.maximumPossiblePressure = p1;
		return 0;
	}

	[AOT.MonoPInvokeCallback(typeof(LuaNativeFunction))]
	static int Get_type(IntPtr L)
	{
		ref var obj = ref SharpObject.GetValue<UnityEngine.Touch>(L, 1);
		Push(L, obj.type);
		return 1;
	}

	[AOT.MonoPInvokeCallback(typeof(LuaNativeFunction))]
	static int Set_type(IntPtr L)
	{
		ref var obj = ref SharpObject.GetValue<UnityEngine.Touch>(L, 1);
		Get(L, 2, out UnityEngine.TouchType p1);
		obj.type = p1;
		return 0;
	}

	[AOT.MonoPInvokeCallback(typeof(LuaNativeFunction))]
	static int Get_altitudeAngle(IntPtr L)
	{
		ref var obj = ref SharpObject.GetValue<UnityEngine.Touch>(L, 1);
		Push(L, obj.altitudeAngle);
		return 1;
	}

	[AOT.MonoPInvokeCallback(typeof(LuaNativeFunction))]
	static int Set_altitudeAngle(IntPtr L)
	{
		ref var obj = ref SharpObject.GetValue<UnityEngine.Touch>(L, 1);
		Get(L, 2, out float p1);
		obj.altitudeAngle = p1;
		return 0;
	}

	[AOT.MonoPInvokeCallback(typeof(LuaNativeFunction))]
	static int Get_azimuthAngle(IntPtr L)
	{
		ref var obj = ref SharpObject.GetValue<UnityEngine.Touch>(L, 1);
		Push(L, obj.azimuthAngle);
		return 1;
	}

	[AOT.MonoPInvokeCallback(typeof(LuaNativeFunction))]
	static int Set_azimuthAngle(IntPtr L)
	{
		ref var obj = ref SharpObject.GetValue<UnityEngine.Touch>(L, 1);
		Get(L, 2, out float p1);
		obj.azimuthAngle = p1;
		return 0;
	}

	[AOT.MonoPInvokeCallback(typeof(LuaNativeFunction))]
	static int Get_radius(IntPtr L)
	{
		ref var obj = ref SharpObject.GetValue<UnityEngine.Touch>(L, 1);
		Push(L, obj.radius);
		return 1;
	}

	[AOT.MonoPInvokeCallback(typeof(LuaNativeFunction))]
	static int Set_radius(IntPtr L)
	{
		ref var obj = ref SharpObject.GetValue<UnityEngine.Touch>(L, 1);
		Get(L, 2, out float p1);
		obj.radius = p1;
		return 0;
	}

	[AOT.MonoPInvokeCallback(typeof(LuaNativeFunction))]
	static int Get_radiusVariance(IntPtr L)
	{
		ref var obj = ref SharpObject.GetValue<UnityEngine.Touch>(L, 1);
		Push(L, obj.radiusVariance);
		return 1;
	}

	[AOT.MonoPInvokeCallback(typeof(LuaNativeFunction))]
	static int Set_radiusVariance(IntPtr L)
	{
		ref var obj = ref SharpObject.GetValue<UnityEngine.Touch>(L, 1);
		Get(L, 2, out float p1);
		obj.radiusVariance = p1;
		return 0;
	}

	[AOT.MonoPInvokeCallback(typeof(LuaNativeFunction))]
	static int Equals(IntPtr L)
	{
		const int startStack = 2;
		ref var obj = ref SharpObject.GetValue<UnityEngine.Touch>(L, 1);
		Get(L, 0 + startStack, out object t0);
		var ret = obj.Equals(t0);
		Push(L, ret);
		return 1;
	}

	[AOT.MonoPInvokeCallback(typeof(LuaNativeFunction))]
	static int GetHashCode(IntPtr L)
	{
		ref var obj = ref SharpObject.GetValue<UnityEngine.Touch>(L, 1);
		var ret = obj.GetHashCode();
		Push(L, ret);
		return 1;
	}

	[AOT.MonoPInvokeCallback(typeof(LuaNativeFunction))]
	static int ToString(IntPtr L)
	{
		ref var obj = ref SharpObject.GetValue<UnityEngine.Touch>(L, 1);
		var ret = obj.ToString();
		Push(L, ret);
		return 1;
	}

	[AOT.MonoPInvokeCallback(typeof(LuaNativeFunction))]
	static int GetType(IntPtr L)
	{
		ref var obj = ref SharpObject.GetValue<UnityEngine.Touch>(L, 1);
		var ret = obj.GetType();
		Push(L, ret);
		return 1;
	}

	public static void Register(ClassWraper classWraper)
	{
		classWraper.RegProperty("fingerId", Get_fingerId, Set_fingerId);
		classWraper.RegProperty("position", Get_position, Set_position);
		classWraper.RegProperty("rawPosition", Get_rawPosition, Set_rawPosition);
		classWraper.RegProperty("deltaPosition", Get_deltaPosition, Set_deltaPosition);
		classWraper.RegProperty("deltaTime", Get_deltaTime, Set_deltaTime);
		classWraper.RegProperty("tapCount", Get_tapCount, Set_tapCount);
		classWraper.RegProperty("phase", Get_phase, Set_phase);
		classWraper.RegProperty("pressure", Get_pressure, Set_pressure);
		classWraper.RegProperty("maximumPossiblePressure", Get_maximumPossiblePressure, Set_maximumPossiblePressure);
		classWraper.RegProperty("type", Get_type, Set_type);
		classWraper.RegProperty("altitudeAngle", Get_altitudeAngle, Set_altitudeAngle);
		classWraper.RegProperty("azimuthAngle", Get_azimuthAngle, Set_azimuthAngle);
		classWraper.RegProperty("radius", Get_radius, Set_radius);
		classWraper.RegProperty("radiusVariance", Get_radiusVariance, Set_radiusVariance);
		classWraper.RegFunction("Equals", Equals);
		classWraper.RegFunction("GetHashCode", GetHashCode);
		classWraper.RegFunction("ToString", ToString);
		classWraper.RegFunction("GetType", GetType);
	}
}