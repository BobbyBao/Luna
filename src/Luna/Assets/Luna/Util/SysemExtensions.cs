﻿using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace SharpLuna
{
    public static class SystemExtensions
    {
        private static Dictionary<Type, bool> cachedTypes = new Dictionary<Type, bool>();
        public static bool IsUnManaged(this Type t)
        {
            bool result;
            if (cachedTypes.TryGetValue(t, out result))
                return result;

            else if (t.IsPrimitive || t.IsPointer || t.IsEnum)
                result = true;
            else if (t.IsGenericType || !t.IsValueType)
                result = false;
            else
            {
               var fieldInfos =  t.GetFields(BindingFlags.Public |
                   BindingFlags.NonPublic | BindingFlags.Instance);

                result = true;
                foreach (var fieldInfo in fieldInfos)
                {
                    if (!fieldInfo.FieldType.IsUnManaged())
                    {
                        result = false;
                        break;
                    }
                }
            }

            cachedTypes.Add(t, result);
            return result;
        }

        public static bool ShouldExport(this Type t)
        {
            if (t.IsGenericType)
            {
                return false;
            }

            if (t.IsByRef || t.IsPointer)
            {
                return false;
            }
            
            return true;
        }

        public static StringBuilder Indent(this StringBuilder sb, int count)
        {
            for (int i = 0; i < count; i++)
            {
                sb.Append('\t');
            }

            return sb;
        }

        public static bool ShouldExport(this MemberInfo memberInfo)
        {
            if (memberInfo.IsDefined(typeof(ObsoleteAttribute)))
            {
                return false;
            }

            if (memberInfo.IsDefined(typeof(LuaHideAttribute)))
            {
                return false;
            }
            return true;
        }

        public static bool ShouldExport(this MethodInfo methodInfo)
        {
            if (!methodInfo.IsPublic)
            {
                return false;
            }

            if (methodInfo.IsSpecialName)
            {
                return false;
            }

            if (methodInfo.IsGenericMethod)
            {
                return false;
            }

            if (methodInfo.IsDefined(typeof(ObsoleteAttribute)))
            {
                return false;
            }

            if (methodInfo.IsDefined(typeof(LuaHideAttribute)))
            {
                return false;
            }

            return true;
        }

        public static void TryAdd<T>(this HashSet<T> self, T obj)
        {
            if (!self.Contains(obj))
            {
                self.Add(obj);
            }
        }
    }
}
