using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace SharpLuna
{
    public static class SystemExtensions
    {
        public static void FastRemove<T>(this List<T> list, T item)
        {
            int index = list.IndexOf(item);
            FastRemove(list, index);
        }

        public static void FastRemove<T>(this List<T> list, int index)
        {
            int size = list.Count;

            if (index < 0 || index >= size) 
                throw new ArgumentOutOfRangeException(nameof(index));

            list[index] = list[size - 1];
            list.RemoveAt(size - 1);
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
