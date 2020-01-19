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
            get => exportPath;
            set
            {
                exportPath = value;
                Directory.CreateDirectory(WrapGenerator.ExportPath);
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

        public static string GenerateClass(Type type)
        {      
            StringBuilder sb = new StringBuilder();
            sb.Append("using System;\n");
            sb.Append("using SharpLuna;\n");
            sb.Append("using static SharpLuna.Lua;\n");
            sb.AppendLine();

            sb.Append("[WrapClass(typeof(" + type.FullName + "))]\n");
            sb.Append("public class ").Append(type.Name).Append("Wrap\n{\n");

            List<(MemberTypes memberType, string name, bool hasGetter, bool hasSetter)> members =
                new List<(MemberTypes memberType, string name, bool hasGetter, bool hasSetter)>();

            var ctors = type.GetConstructors();
            List<ConstructorInfo> ctorList = new List<ConstructorInfo>();
            foreach (var ctor in ctors)
            {
                if (!ctor.ShouldExport())
                {
                    continue;
                }

                if(!ctor.IsPublic)
                {
                    continue;
                }

                bool gen = true;
                foreach (var p in ctor.GetParameters())
                {
                    if (p.ParameterType.IsByRef)
                    {
                        gen = false;
                    }

                    if (p.ParameterType.IsPointer)
                    {
                        gen = false;
                    }

                    if (p.ParameterType.IsGenericType)
                    {
                        gen = false;
                    }
                }

                if (gen)
                {
                    ctorList.Add(ctor);
                }
               
            }

            if (ctorList.Count > 0)
            {
                GenerateConstructor(type, ctorList, sb);
                members.Add((MemberTypes.Constructor, "ctor", false, false));
            }

            if(!type.IsEnum)
            {
                var fields = type.GetFields(BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static);
                foreach (var field in fields)
                {
                    //常量不生成
                    if (field.IsLiteral)
                    {
                        continue;
                    }

                    if (!field.ShouldExport())
                    {
                        continue;
                    }

                    GenerateField(type, field, sb);
                    members.Add((MemberTypes.Field, field.Name, true, !field.IsLiteral && !field.IsInitOnly));
                }
            }

            var properties = type.GetProperties(BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static);
            foreach (var prop in properties)
            {
                if (!prop.ShouldExport())
                {
                    continue;
                }

                //indexer 暂时不支持
                if (prop.Name == "Item")
                {
                    continue;
                }

                GenerateProperty(type, prop, sb);
                members.Add((MemberTypes.Property, prop.Name, prop.CanRead, prop.CanWrite));
            }

            var methods = type.GetMethods(BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static);
            foreach (var method in methods)
            {
                if (method.IsSpecialName)
                {
                    continue;
                }

                if (!method.ShouldExport())
                {
                    continue;
                }

                if (members.FindIndex((t) => t.Item2 == method.Name) != -1)
                {
                    continue;
                }

                var memberInfo = type.GetMember(method.Name);
                List<MethodInfo> methodInfos = new List<MethodInfo>();
                foreach (var m in memberInfo)
                {
                    if (!(m is MethodInfo mi))
                    {
                        continue;
                    }

                    bool gen = true;
                    foreach (var p in mi.GetParameters())
                    {
                        if (p.ParameterType.IsByRef)
                        {
                            gen = false;
                        }

                        if (p.ParameterType.IsPointer)
                        {
                            gen = false;
                        }

                        if (p.ParameterType.IsGenericType)
                        {
                            gen = false;
                        }
                    }

                    if (gen && m.ShouldExport())
                    {
                        methodInfos.Add((MethodInfo)m);
                    }
                }

                if (methodInfos.Count > 0)
                {
                    GenerateMethod(type, methodInfos.ToArray(), sb);
                    members.Add((MemberTypes.Method, method.Name, false, false));

                }
            }


            sb.Append($"\tpublic static void Register(ClassWraper classWraper)\n\t{{\n");

            foreach (var (memberType, name, hasGetter, hasSetter) in members)
            {
                if (memberType == MemberTypes.Constructor)
                {
                    sb.Append($"\t\tclassWraper.RegConstructor(Constructor);\n");
                }
                else if (memberType == MemberTypes.Field)
                {
                    if (hasGetter || hasSetter)
                    {
                        sb.Append($"\t\tclassWraper.RegField(\"{name}\"");

                        if (hasGetter)
                        {
                            sb.Append($", Get_{name}");
                        }

                        if (hasSetter)
                        {
                            sb.Append($", Set_{name}");
                        }

                        sb.Append($");\n");
                    }
                }
                else if (memberType == MemberTypes.Property)
                {
                    if (hasGetter || hasSetter)
                    {
                        sb.Append($"\t\tclassWraper.RegProperty(\"{name}\"");

                        if (hasGetter)
                        {
                            sb.Append($", Get_{name}");
                        }

                        if (hasSetter)
                        {
                            sb.Append($", Set_{name}");
                        }

                        sb.Append($");\n");
                    }
                }
                else if (memberType == MemberTypes.Method)
                {
                    sb.Append($"\t\tclassWraper.RegFunction(\"{name}\", {name});\n");
                }
            }

            sb.Append("\t}\n");

            sb.Append("}");
            return sb.ToString();

        }

        static void GenerateConstructor(Type type, List<ConstructorInfo> ctorList, StringBuilder sb)
        {
            sb.Append("\t[AOT.MonoPInvokeCallback(typeof(LuaNativeFunction))]\n");
            sb.Append($"\tstatic int Constructor(IntPtr L)\n\t{{\n");

            sb.Append($"\t\tint n = lua_gettop(L) - 1;\n");

            sb.Append($"\t\t{type.FullName} obj = default;\n");

            bool first = true;

            if (type.IsValueType)
            {
                first = false;
                sb.Append($"\t\tif(n == 0)\n");               
                sb.Append($"\t\t\tobj = new {type.FullName}();\n");
            }
            foreach (var ctor in ctorList)
            {
                var parameters = ctor.GetParameters();
                if (parameters.Length == 0)
                {
                    if (first)
                    {
                        first = false;
                        sb.Append($"\t\tif(n == 0)\n");
                    }
                    else
                    {
                        sb.Append($"\t\telse if(n == 0)\n");
                    }
                    sb.Append($"\t\t\tobj = new {type.FullName}();\n");
                }
                else
                {
                    if (first)
                    {
                        first = false;
                        sb.Append($"\t\tif(n == {parameters.Length})\n");
                    }
                    else
                    {
                        sb.Append($"\t\telse if(n == {parameters.Length})\n");
                    }

                    sb.Append($"\t\t\tobj = new {type.FullName}(\n");

                    for (int i = 1; i <= parameters.Length; i++)
                    {
                        var paramInfo = parameters[i - 1];
                        sb.Append($"\t\t\t\tLua.Get<{GetTypeName(paramInfo.ParameterType)}>(L, {(i+1)})");
                        if (i != parameters.Length)
                        {
                            sb.Append(",");
                        }
                        sb.AppendLine();
                    }

                    sb.Append("\t\t\t);\n");
                }

            }         

            sb.Append("\t\tLua.Push(L, obj);\n");
            sb.Append("\t\treturn 1;\n");
            sb.Append("\t}\n");
            sb.AppendLine();
        }

        static void GenerateField(Type type, FieldInfo field, StringBuilder sb)
        {
            GenerateVariable(type, field.Name, field.IsStatic, field.FieldType, sb, true, true);
        }

        static void GenerateProperty(Type type, PropertyInfo propertyInfo, StringBuilder sb)
        {
            GenerateVariable(type, propertyInfo.Name, propertyInfo.GetMethod.IsStatic,
                propertyInfo.PropertyType, sb, propertyInfo.CanRead, propertyInfo.CanWrite);
        }

        static void GenerateVariable(Type type, string name, bool isStatic, Type valType, StringBuilder sb, bool read, bool write)
        {
            if (read)
            {
                sb.Append("\t[AOT.MonoPInvokeCallback(typeof(LuaNativeFunction))]\n");
                sb.Append($"\tstatic int Get_{name}(IntPtr L)\n\t{{\n");

                if (isStatic)
                {
                    sb.Append($"\t\tLua.Push(L, {type.FullName}.{name});\n");
                }
                else
                {
                    if (type.IsUnManaged())
                    {
                        sb.Append($"\t\tref var obj = ref SharpObject.GetValue<{type.FullName}>(L, 1);\n");
                    }
                    else
                    {
                        sb.Append($"\t\tvar obj = SharpObject.Get<{type.FullName}>(L, 1);\n");
                    }

                    sb.Append($"\t\tLua.Push(L, obj.{name});\n");
                }

                sb.Append("\t\treturn 1;\n");
                sb.Append("\t}\n");
                sb.AppendLine();
            }
           
            if (write)
            {
                sb.Append("\t[AOT.MonoPInvokeCallback(typeof(LuaNativeFunction))]\n");
                sb.Append($"\tstatic int Set_{name}(IntPtr L)\n\t{{\n");
                
                if (isStatic)
                {
                    sb.Append($"\t\tvar p1 = Lua.Get<{GetTypeName(valType)}>(L, 1);\n");
                    sb.Append($"\t\t{type.FullName}.{name} = p1;\n");
                }
                else
                {                
                    if (type.IsUnManaged())
                    {
                        sb.Append($"\t\tref var obj = ref SharpObject.GetValue<{type.FullName}>(L, 1);\n");
                    }
                    else
                    {
                        sb.Append($"\t\tvar obj = SharpObject.Get<{type.FullName}>(L, 1);\n");
                    }

                    sb.Append($"\t\tvar p1 = Lua.Get<{GetTypeName(valType)}>(L, 2);\n");
                    sb.Append($"\t\tobj.{name} = p1;\n");
                }

                sb.Append("\t\treturn 0;\n");
                sb.Append("\t}\n");
                sb.AppendLine();
            }
            
        }

        static void GenerateMethod(Type type, MethodInfo[] methodInfo, StringBuilder sb)
        {
            sb.Append("\t[AOT.MonoPInvokeCallback(typeof(LuaNativeFunction))]\n");
            sb.Append($"\tstatic int {methodInfo[0].Name}(IntPtr L)\n\t{{\n");

            sb.Append($"\t\tint n = lua_gettop(L) - 1;\n");
            

            bool first = true;
            int idx = 0;
            foreach (var method in methodInfo)
            {
                idx++;
                var parameters = method.GetParameters();

                if (methodInfo.Length > 1)
                {
                    if (first)
                    {
                        first = false;
                        sb.Append($"\tif(n == {parameters.Length})\n\t{{\n");
                    }                    
                    else
                    {
                        sb.Append($"\t}} else if(n == {parameters.Length})\n\t{{\n");
                    }
                }

                if (!method.IsStatic)
                {
                    if (type.IsUnManaged())
                    {
                        sb.Append($"\t\tref var obj = ref SharpObject.GetValue<{type.FullName}>(L, 1);\n");
                    }
                    else
                    {
                        sb.Append($"\t\tvar obj = SharpObject.Get<{type.FullName}>(L, 1);\n");
                    }
                }

                if (method.ReturnType != typeof(void))
                {
                    sb.Append("\t\tvar ret = ");
                    if (method.IsStatic)
                        sb.Append($"{type.FullName}.{method.Name}(");
                    else
                        sb.Append($"obj.{method.Name}(");
                }
                else
                {
                    if (method.IsStatic)
                        sb.Append($"\t\t{type.FullName}.{method.Name}(");
                    else
                        sb.Append($"\t\tobj.{method.Name}(");                    
                }

                if (parameters.Length > 0)
                {
                    sb.Append("\n");
                }
                   
                for (int i = 0; i < parameters.Length; i++)
                {
                    var paramInfo = parameters[i];
                    sb.Append($"\t\t\tLua.Get<{GetTypeName(paramInfo.ParameterType)}>(L, {(i + 1)})");
                    if (i != parameters.Length - 1)
                    {
                        sb.Append(",");
                    }
                    sb.AppendLine();
                }

                if (parameters.Length > 0)
                {
                    sb.Append("\t\t);\n");
                }
                else
                {
                    sb.Append(");\n");
                }

                if (method.ReturnType != typeof(void))
                {
                    sb.Append("\t\tLua.Push(L, ret);\n");
                    sb.Append("\t\treturn 1;\n");
                }
                else
                {
                    sb.Append("\t\treturn 0;\n");
                }

                if (methodInfo.Length > 1)
                {
                    if (idx == methodInfo.Length)
                    {
                        sb.Append("\t\t}\n");
                    }

                }
            }

            if (methodInfo.Length > 1)
            {
                sb.Append("\t\treturn 0;\n");
            }
              
            sb.Append("\t}\n");
            sb.AppendLine();
        }

    }

}
