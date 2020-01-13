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
            //sb.Append("using static SharpLuna.Lua;\n");
            sb.AppendLine();

            sb.Append("[WrapClass(typeof(" + type.FullName + "))]\n");
            sb.Append("class ").Append(type.Name).Append("Wrap\n{\n");

            var ctors = type.GetConstructors();
            foreach (var ctor in ctors)
            {
                if (ctor.IsDefined(typeof(LuaHideAttribute)))
                {
                    continue;
                }

                if (ctor.IsPublic)
                {
                    GenerateConstructor(type, ctor, sb);
                }

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

        static void GenerateConstructor(Type type, ConstructorInfo ctor, StringBuilder sb)
        {
            var parameters = ctor.GetParameters();

            sb.Append("\t[AOT.MonoPInvokeCallback(typeof(LuaNativeFunction))]\n");
            sb.Append($"\t[WrapMethod(\"ctor\", MethodType.Normal)]\n");
            sb.Append($"\tstatic int ctor_{parameters.Length}(LuaState L)\n\t{{\n");

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
                    sb.Append($"\t\t\tLua.Get<{paramInfo.ParameterType.FullName}>(L, {i})");
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
            sb.Append($"\tstatic int get_{field.Name}(LuaState L)\n\t{{\n");

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
            sb.Append($"\tstatic int set_{field.Name}(LuaState L)\n\t{{\n");

            if (type.IsUnManaged())
            {
                sb.Append($"\t\tref var obj = ref SharpObject.GetValue<{type.FullName}>(L, 1);\n");
            }
            else
            {
                sb.Append($"\t\tvar obj = SharpObject.Get<{type.FullName}>(L, 1);\n");
            }

            sb.Append($"\t\tvar p1 = Lua.Get<{field.FieldType.FullName}>(L, 2);\n");
            sb.Append($"\t\tobj.{field.Name} = p1;\n");
            sb.Append("\t\treturn 0;\n");
            sb.Append("\t}\n");
            sb.AppendLine();
        }
    }
}
