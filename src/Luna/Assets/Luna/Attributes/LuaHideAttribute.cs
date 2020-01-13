using System;

namespace SharpLuna
{
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Field | AttributeTargets.Property)]
    public sealed class LuaHideAttribute : Attribute
    {
    }

}