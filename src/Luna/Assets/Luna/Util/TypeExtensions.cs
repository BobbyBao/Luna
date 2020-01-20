using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace SharpLuna
{
    public class TypeData
    {
        public int signature;
        public bool registered;
        public bool isUnmanaged;
        public LuaNativeFunction pushFn;
        public LuaNativeFunction getFn;
        public LuaNativeFunction optFn;
    }

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
                var fieldInfos = t.GetFields(BindingFlags.Public |
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

        private static Dictionary<Type, string> _defaultDictionary = new Dictionary<System.Type, string>
        {
            {typeof(int), "int"},
            {typeof(uint), "uint"},
            {typeof(long), "long"},
            {typeof(ulong), "ulong"},
            {typeof(short), "short"},
            {typeof(ushort), "ushort"},
            {typeof(byte), "byte"},
            {typeof(sbyte), "sbyte"},
            {typeof(bool), "bool"},
            {typeof(float), "float"},
            {typeof(double), "double"},
            {typeof(decimal), "decimal"},
            {typeof(char), "char"},
            {typeof(string), "string"},
            {typeof(object), "object"},
            {typeof(void), "void"}
        };

        public static string GetFriendlyName(this Type type, Dictionary<Type, string> translations)
        {
            if (translations.ContainsKey(type))
                return translations[type];
            else if (type.IsArray)
                return GetFriendlyName(type.GetElementType(), translations) + "[]";
            else if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>))
                return type.GetGenericArguments()[0].GetFriendlyName() + "?";
            else if (type.IsGenericType)
                return type.Name.Split('`')[0] + "<" + string.Join(", ", type.GetGenericArguments().Select(x => GetFriendlyName(x)).ToArray()) + ">";
            else
                return type.FullName;
        }

        public static string GetFriendlyName(this Type type)
        {
            return type.GetFriendlyName(_defaultDictionary);
        }


    }
}
