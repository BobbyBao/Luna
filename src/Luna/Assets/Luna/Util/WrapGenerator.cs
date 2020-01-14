using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;

namespace SharpLuna
{
    public class WrapGenerator
    {
        static string exportPath = "";
        public static string ExportPath
        {
            set
            {
                exportPath = value;
                Directory.CreateDirectory(exportPath);
            }
        }

        static string GetTypeName(Type type)
        {
            if (type == typeof(bool))
                return "bool";
            else if (type == typeof(sbyte))
                return "sbyte";
            else if (type == typeof(byte))
                return "byte";
            else if (type == typeof(short))
                return "short";
            else if (type == typeof(ushort))
                return "ushort";
            else if (type == typeof(int))
                return "int";
            else if (type == typeof(uint))
                return "uint";
            else if (type == typeof(long))
                return "long";
            else if (type == typeof(ulong))
                return "ulong";
            else if (type == typeof(float))
                return "float";
            else if (type == typeof(double))
                return "double";
            else if (type == typeof(decimal))
                return "decimal";
            else if (type == typeof(char))
                return "char";
            else if (type == typeof(string))
                return "string";
            else if (type == typeof(object))
                return "object";
            else
                return type.FullName;
        }

        public static void GenerateClassWrap(Type type, string path = "")
        {
            string code = GenerateClass(type);

            if(!string.IsNullOrEmpty(path))
            {
                File.WriteAllText(path, code);
            }
            else
            {
                File.WriteAllText(Path.Combine(exportPath, type.Name + "Wrap.cs"), code);
            }
        }

        public static String GenerateClass(Type type)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("using System;\n");
            sb.Append("using SharpLuna;\n");
            sb.Append("using static SharpLuna.Lua;\n");
            sb.AppendLine();

            sb.Append("[WrapClass(typeof(" + type.FullName + "))]\n");
            sb.Append("public class ").Append(type.Name).Append("Wrap\n{\n");

            var ctors = type.GetConstructors();
            List<ConstructorInfo> ctorList = new List<ConstructorInfo>();
            foreach (var ctor in ctors)
            {
                if (ctor.IsDefined(typeof(LuaHideAttribute)))
                {
                    continue;
                }

                if (ctor.IsPublic)
                {
                    ctorList.Add(ctor);
                }

            }

            if (ctorList.Count > 0)
            {
                GenerateConstructor(type, ctorList, sb);
            }

            var fields = type.GetFields(BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static);
            foreach(var field in fields)
            {
                if(field.IsDefined(typeof(LuaHideAttribute)))
                {
                    continue;
                }

                GenerateField(type, field, sb);
            }

            sb.Append("}");
            return sb.ToString();

        }

        static void GenerateConstructor(Type type, List<ConstructorInfo> ctor, StringBuilder sb)
        {
            var parameters = ctor[0].GetParameters();

            sb.Append("\t[AOT.MonoPInvokeCallback(typeof(LuaNativeFunction))]\n");
            sb.Append($"\t[WrapMethod(\"ctor\", MethodType.Normal)]\n");
            sb.Append($"\tstatic int Constructor(LuaState L)\n\t{{\n");

            if(parameters.Length == 0)
            {
                sb.Append($"\t\tvar obj = new {type.FullName}();\n");
            }
            else
            {
                sb.Append($"\t\tvar obj = new {type.FullName}(\n");

                for(int i = 1; i <= parameters.Length; i++)
                {
                    var paramInfo = parameters[i - 1];                    
                    sb.Append($"\t\t\tLua.Get<{GetTypeName(paramInfo.ParameterType)}>(L, {i})");
                    if(i != parameters.Length)
                    {
                        sb.Append(",");
                    }
                    sb.AppendLine();
                }

                sb.Append("\t\t);\n"); 
            }
         

            sb.Append("\t\tLua.Push(L, obj);\n");
            sb.Append("\t\treturn 1;\n");
            sb.Append("\t}\n");
            sb.AppendLine();
        }

        static void GenerateField(Type type, FieldInfo field, StringBuilder sb)
        {
            sb.Append("\t[AOT.MonoPInvokeCallback(typeof(LuaNativeFunction))]\n");
            sb.Append($"\t[WrapMethod(\"{field.Name}\", MethodType.Getter)]\n");
            sb.Append($"\tstatic int Get_{field.Name}(LuaState L)\n\t{{\n");

            if(type.IsUnManaged())
            {
                sb.Append($"\t\tref var obj = ref SharpObject.GetValue<{type.FullName}>(L, 1);\n");
            }
            else
            {
                sb.Append($"\t\tvar obj = SharpObject.Get<{type.FullName}>(L, 1);\n");
            }

            sb.Append($"\t\tLua.Push(L, obj.{field.Name});\n");
            sb.Append("\t\treturn 1;\n");
            sb.Append("\t}\n");
            sb.AppendLine();

            sb.Append("\t[AOT.MonoPInvokeCallback(typeof(LuaNativeFunction))]\n");
            sb.Append($"\t[WrapMethod(\"{field.Name}\", MethodType.Setter)]\n");
            sb.Append($"\tstatic int Set_{field.Name}(LuaState L)\n\t{{\n");

            if (type.IsUnManaged())
            {
                sb.Append($"\t\tref var obj = ref SharpObject.GetValue<{type.FullName}>(L, 1);\n");
            }
            else
            {
                sb.Append($"\t\tvar obj = SharpObject.Get<{type.FullName}>(L, 1);\n");
            }

            sb.Append($"\t\tvar p1 = Lua.Get<{GetTypeName(field.FieldType)}>(L, 2);\n");
            sb.Append($"\t\tobj.{field.Name} = p1;\n");
            sb.Append("\t\treturn 0;\n");
            sb.Append("\t}\n");
            sb.AppendLine();
        }
    }
}
