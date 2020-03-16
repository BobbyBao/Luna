using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;

namespace SharpLuna
{
    public struct StructElement
    {
        public IntPtr name;
        public TypeCode type;
        public short offset;
        public short size;

        public string Name => Marshal.PtrToStringAnsi(name);
    }

    public unsafe struct NativeBuffer : IEnumerable<StructElement>
    {
        public int size;
        public StructElement[] layout;
        public List<string> keys;
        public byte* data;

        public NativeBuffer(StructElement[] layout)
        {
            this.layout = new StructElement[layout.Length];
            Array.Copy(layout, 0, this.layout, 0, layout.Length);
            keys = new List<string>(layout.Length);
            this.size = 0;
            foreach (var e in layout)
            {
                keys.Add(e.Name);
                this.size += e.size;
            }

            data = null;
        }

        public StructElement* Addr => (StructElement*)Unsafe.AsPointer(ref layout[0]);
        public int Count => keys.Count;

        public T Get<T>(string key, T v) where T : unmanaged
        {
            for (int i = 0; i < keys.Count; i++)
            {
                if (key == keys[i])
                {
                    Debug.Assert(sizeof(T) == layout[i].size);
                    return Unsafe.Read<T>(data + layout[i].offset);
                    
                }
            }

            throw new Exception("Ket not found!");
        }

        public void Set<T>(string key, T v) where T : unmanaged
        {
            for (int i = 0; i < keys.Count; i++)
            {
                if (key == keys[i])
                {
                    Debug.Assert(sizeof(T) == layout[i].size);
                    Unsafe.Write(data + layout[i].offset, v);
                    break;
                }
            }
        }

        public void Add(object obj)
        {
            if (obj is StructElement e)
            {
                Add(e.Name, e);
            }
            else if (obj is ValueTuple<string, Type> vt)
            {
                Add(vt.Item1, vt.Item2);
            }
            else if (obj is Tuple<string, Type> t)
            {
                Add(t.Item1, t.Item2);
            }
        }

        void Add(string key, StructElement e)
        {
            if (keys == null)                  
                keys = new List<string>();    

            if (this.layout == null)
                this.layout = new StructElement[4];
            
            if(keys.Count == this.layout.Length)
            {
                Array.Resize(ref layout, this.layout.Length + 4);
            }

            this.layout[keys.Count] = e;
            keys.Add(key);
        }

        public void Add(string key, Type type)
        {
            StructElement e = new StructElement
            {
                name = Marshal.StringToHGlobalAnsi(key),
                type = TypeExtensions.GetTypeCode(type),
                offset = (short)size,
                size = (short)TypeExtensions.GetSize(type)
            };

            this.size += e.size;
            Add(key, e);
        }


        public IEnumerator<StructElement> GetEnumerator()
        {
            return ((IEnumerable<StructElement>)layout).GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable<StructElement>)layout).GetEnumerator();
        }
    }

    public unsafe static class TypeExtensions
    {
        private static Dictionary<Type, bool> cachedTypes = new Dictionary<Type, bool>();

        static int[] size = new int[]
        {
            0, 0, 0,
            sizeof(bool), sizeof(char), sizeof(sbyte), sizeof(byte),
            sizeof(short), sizeof(ushort), sizeof(int), sizeof(uint),
            sizeof(long), sizeof(ulong), sizeof(float), sizeof(double),
            sizeof(decimal), sizeof(DateTime), 0, sizeof(IntPtr)
        };

        public static StructElement[] GetLayout(this Type type, out int size)
        {
            if(!type.IsUnManaged())
            {
                size = 0;
                return null;
            }

            var fieldInfos = type.GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
            int len = fieldInfos.Length;
            StructElement[] layout = new StructElement[len];
            size = 0;
            for (int i = 0; i < len; i++)
            {
                var fieldInfo = fieldInfos[i];
                ref var e = ref layout[i];
                e.name = Marshal.StringToHGlobalAnsi(fieldInfo.Name);
                e.type = GetTypeCode(fieldInfo.FieldType);
                e.offset = (short)Marshal.OffsetOf(type, fieldInfo.Name);
                e.size = (short)GetSize(fieldInfo.FieldType);
                Debug.Assert(e.size != 0);
                size += e.size;
            }

            return layout;
        }

        public static TypeCode GetTypeCode(Type type)
        {
            if(type == typeof(IntPtr))
            {
                return TypeCode.Int64;
            }
            else if (type == typeof(UIntPtr))
            {
                return TypeCode.UInt64;
            }

            return Type.GetTypeCode(type);
        }

        public static int GetSize(Type type)
        {
            return size[(int)GetTypeCode(type)];
        }

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
                var fieldInfos = t.GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
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
                return type.FullName.Replace("+", ".");
        }

        public static string GetFriendlyName(this Type type)
        {
            return type.GetFriendlyName(_defaultDictionary);
        }


    }
}
