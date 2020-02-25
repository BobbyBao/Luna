using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace SharpLuna
{
    using static Lua;
    using lua_State = IntPtr;

    public static partial class Lua
    {

        public static void Push(lua_State L, in Space v) => Push(L, (int)v);

        public static void Push(lua_State L, in Vector2 v) => SharpObject.PushUnmanagedObject(L, v);
        public static void Push(lua_State L, in Vector3 v) => SharpObject.PushUnmanagedObject(L, v);
        public static void Push(lua_State L, in Vector4 v) => SharpObject.PushUnmanagedObject(L, v);
        public static void Push(lua_State L, in Quaternion v) => SharpObject.PushUnmanagedObject(L, v);
        public static void Push(lua_State L, in Color v) => SharpObject.PushUnmanagedObject(L, v);
        public static void Push(lua_State L, in Plane v) => SharpObject.PushUnmanagedObject(L, v);
        public static void Push(lua_State L, in Ray v) => SharpObject.PushUnmanagedObject(L, v);
        public static void Push(lua_State L, in Bounds v) => SharpObject.PushUnmanagedObject(L, v);

        public static void Get(lua_State L, int index, out Space v) { Get(L, index, out int v1); v = (Space)v1;}

        public static void Get(lua_State L, int index, out Vector2 v) => v = SharpObject.GetUnmanaged<Vector2>(L, index);
        public static void Get(lua_State L, int index, out Vector3 v) => v = SharpObject.GetUnmanaged<Vector3>(L, index);
        public static void Get(lua_State L, int index, out Vector4 v) => v = SharpObject.GetUnmanaged<Vector4>(L, index);
        public static void Get(lua_State L, int index, out Quaternion v) => v = SharpObject.GetUnmanaged<Quaternion>(L, index);
        public static void Get(lua_State L, int index, out Color v) => v = SharpObject.GetUnmanaged<Color>(L, index);
        public static void Get(lua_State L, int index, out Plane v) => v = SharpObject.GetUnmanaged<Plane>(L, index);
        public static void Get(lua_State L, int index, out Ray v) => v = SharpObject.GetUnmanaged<Ray>(L, index);
        public static void Get(lua_State L, int index, out Bounds v) => v = SharpObject.GetUnmanaged<Bounds>(L, index);
    

    }
}
