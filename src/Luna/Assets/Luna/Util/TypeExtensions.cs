using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace SharpLuna
{
    public static class TypeExtensions
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
    }
}
