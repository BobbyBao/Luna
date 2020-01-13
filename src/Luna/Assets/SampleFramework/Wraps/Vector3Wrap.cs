using System;
using SharpLuna;
using UnityEngine;

[WrapClass(typeof(Vector3))]
public class Vector3Wrap
{



	[AOT.MonoPInvokeCallbackAttribute(typeof(LuaNativeFunction))]
	[WrapMethod("x", MethodType.Getter)]
	static int get_x(IntPtr L)
	{
		return 1;
	}

	[AOT.MonoPInvokeCallbackAttribute(typeof(LuaNativeFunction))]
	[WrapMethod("x", MethodType.Setter)]
	static int set_x(IntPtr L)
	{
		return 0;
	}

}