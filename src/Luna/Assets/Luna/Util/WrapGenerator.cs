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

        public static void GenerateClassWrap(string module, Type type, List<string> excludeMembers = null)
        {
            string code = GenerateClass(module, type, excludeMembers);
            string fileName = type.Name + "Wrap.cs";
            if(!string.IsNullOrEmpty(module))
            {
                fileName = module + "_" + fileName;
            }

            File.WriteAllText(Path.Combine(exportPath, fileName), code);
        }

        public static string GenerateClass(string module, Type type, List<string> excludeMembers)
        {      
            StringBuilder sb = new StringBuilder();
            sb.Append("using System;\n");
            sb.Append("using SharpLuna;\n");
            sb.Append("using System.Collections.Generic;\n");
#if UNITY_EDITOR
            //sb.Append("using UnityEngine;\n");
#endif
            sb.Append("using static SharpLuna.Lua;\n");

            sb.AppendLine();

            sb.Append("[WrapClass(typeof(" + type.FullName + "))]\n");
            sb.Append("public class ");

            if (!string.IsNullOrEmpty(module))
            {
                sb.Append(module).Append("_");
            }

            sb.Append(type.Name).Append("Wrap\n{\n");

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
                    if (!p.ParameterType.ShouldExport())
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
                ctorList.Sort((a, b) => a.GetParameters().Length - b.GetParameters().Length);
                GenerateConstructor(type, ctorList, sb);
                members.Add((MemberTypes.Constructor, "ctor", false, false));
            }

            if(!type.IsEnum)
            {
                var fields = type.GetFields(BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static);
                foreach (var field in fields)
                {
                    if (!field.ShouldExport())
                    {
                        continue;
                    }

                    if (excludeMembers != null)
                    {
                        if (excludeMembers.Contains(field.Name))
                        {
                            continue;
                        }
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

                if (excludeMembers != null)
                {
                    if (excludeMembers.Contains(prop.Name))
                    {
                        continue;
                    }
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

                if (excludeMembers != null)
                {
                    if (excludeMembers.Contains(method.Name))
                    {
                        continue;
                    }
                }

                var memberInfo = type.GetMember(method.Name);
                int[] paraments = new int[16];
                List<MethodInfo> methodInfos = new List<MethodInfo>();
                foreach (var m in memberInfo)
                {
                    if (!(m is MethodInfo mi))
                    {
                        continue;
                    }

                    if (!mi.ShouldExport())
                    {
                        continue;
                    }

                    if (!mi.ReturnType.ShouldExport())
                    {
                        continue;
                    }

                    bool gen = true;
                    var paramments = mi.GetParameters();
                    foreach (var p in paramments)
                    {
                        if (!p.ParameterType.ShouldExport())
                        {
                            gen = false;
                        }
                    }

                    if (gen)
                    {
                        paraments[paramments.Length] = paraments[paramments.Length] + 1;
                        methodInfos.Add((MethodInfo)m);
                    }
                }

                if (methodInfos.Count > 0)
                {
                    methodInfos.Sort((a, b) => a.GetParameters().Length - b.GetParameters().Length);
                    GenerateMethod(type, methodInfos.ToArray(), paraments, sb);
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
                sb.Append($"\t\tif(n == 0)\n\t\t{{\n");               
                sb.Append($"\t\t\tobj = new {type.FullName}();\n");
            }
            int idx = 0;
            foreach (var ctor in ctorList)
            {
                idx++;
                var parameters = ctor.GetParameters();
                if (parameters.Length == 0)
                {
                    if (first)
                    {
                        first = false;
                        sb.Append($"\t\tif(n == 0)\n\t\t{{\n");
                    }
                    else
                    {
                        sb.Append($"\t\t}}\n\t\telse if(n == 0)\n\t\t{{\n");
                    }
                    sb.Append($"\t\t\tobj = new {type.FullName}();\n");
                }
                else
                {
                    if (first)
                    {
                        first = false;
                        sb.Append($"\t\tif(n == {parameters.Length})\n\t\t{{\n");
                    }
                    else
                    {
                        sb.Append($"\t\t}}\n\t\telse if(n == {parameters.Length})\n\t\t{{\n");
                    }

                    for (int i = 1; i <= parameters.Length; i++)
                    {
                        var paramInfo = parameters[i - 1];
                        sb.Append($"\t\t\tGet(L, {(i + 1)}, out {paramInfo.ParameterType.GetFriendlyName()} t{i});");                        
                        sb.AppendLine();
                    }

                    sb.Append($"\t\t\tobj = new {type.FullName}(");

                    for (int i = 1; i <= parameters.Length; i++)
                    {
                        var paramInfo = parameters[i - 1];
                        sb.Append($"t{i}");
                        if (i != parameters.Length)
                        {
                            sb.Append(", ");
                        }
                    }

                    sb.Append(");\n");
                }

                if(idx == ctorList.Count)
                    sb.Append("\t\t}\n");
            }         

            sb.Append("\t\tPush(L, obj);\n");
            sb.Append("\t\treturn 1;\n");
            sb.Append("\t}\n");
            sb.AppendLine();
        }

        static void GenerateField(Type type, FieldInfo field, StringBuilder sb)
        {
            GenerateVariable(type, field.Name, field.IsStatic, field.FieldType, sb, true, !field.IsInitOnly && !field.IsLiteral);
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
                    sb.Append($"\t\tPush(L, {type.FullName}.{name});\n");
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

                    sb.Append($"\t\tPush(L, obj.{name});\n");
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
                    sb.Append($"\t\tGet(L, 1, out {valType.GetFriendlyName()} p1);\n");
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

                    sb.Append($"\t\tGet(L, 2, out {valType.GetFriendlyName()} p1);\n");
                    sb.Append($"\t\tobj.{name} = p1;\n");
                }

                sb.Append("\t\treturn 0;\n");
                sb.Append("\t}\n");
                sb.AppendLine();
            }
            
        }

        static void GenerateMethod(Type type, MethodInfo[] methodInfo, int[] paraments, StringBuilder sb)
        {
            sb.Append("\t[AOT.MonoPInvokeCallback(typeof(LuaNativeFunction))]\n");
            sb.Append($"\tstatic int {methodInfo[0].Name}(IntPtr L)\n\t{{\n");

            int indent = 2;
            if (methodInfo.Length > 1)
            {
                sb.Append($"\t\tint n = lua_gettop(L) - 1;\n");

                indent = 3;
            }           

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
                        sb.Append($"\t\tif(n == {parameters.Length}");
                        if (paraments[parameters.Length] > 1)
                        {
                            sb.Append(" && CheckType<");
                            for (int i = 0; i < parameters.Length; i++)
                            {
                                var paramInfo = parameters[i];
                                sb.Append($"{paramInfo.ParameterType.GetFriendlyName()}");
                                if (i != parameters.Length - 1)
                                {
                                    sb.Append(", ");
                                }
                            }
                            sb.Append(">(L, 1)");
                        }
                        sb.Append(")\n\t\t{\n");
                    }                    
                    else
                    {
                        sb.Append($"\t\t}}\n\t\telse if(n == {parameters.Length}");
                        if (paraments[parameters.Length] > 1)
                        {
                            sb.Append(" && CheckType<");
                            for (int i = 0; i < parameters.Length; i++)
                            {
                                var paramInfo = parameters[i];
                                sb.Append($"{paramInfo.ParameterType.GetFriendlyName()}");
                                if (i != parameters.Length - 1)
                                {
                                    sb.Append(", ");
                                }
                            }
                            sb.Append(">(L, 1)");
                        }
                        sb.Append(")\n\t\t{\n");;
                    }

                }

                if(method.IsStatic)
                {
                    if (parameters.Length > 0)
                    {
                        sb.Indent(indent).Append($"#if LUNA_SCRIPT\n");
                        sb.Indent(indent).Append($"const int startStack = 2;\n");
                        sb.Indent(indent).Append($"#else\n");
                        sb.Indent(indent).Append($"const int startStack = 1;\n");
                        sb.Indent(indent).Append($"#endif\n");
                    }
                }
                else
                {
                    if (parameters.Length > 0)
                    {
                        sb.Indent(indent).Append($"const int startStack = 2;\n");
                    }

                    if (type.IsUnManaged())
                    {
                        sb.Indent(indent).Append($"ref var obj = ref SharpObject.GetValue<{type.FullName}>(L, 1);\n");
                    }
                    else
                    {
                        sb.Indent(indent).Append($"var obj = SharpObject.Get<{type.FullName}>(L, 1);\n");
                    }
                }

                for (int i = 0; i < parameters.Length; i++)
                {
                    var paramInfo = parameters[i];
                    sb.Indent(indent).Append($"Get(L, {i} + startStack, out {paramInfo.ParameterType.GetFriendlyName()} t{i});");
                    sb.AppendLine();
                }

                if (method.ReturnType != typeof(void))
                {
                    sb.Indent(indent).Append($"{method.ReturnType.GetFriendlyName()} ret = ");
                    if (method.IsStatic)
                        sb.Append($"{type.FullName}.{method.Name}(");
                    else
                        sb.Append($"obj.{method.Name}(");
                }
                else
                {
                    if (method.IsStatic)
                        sb.Indent(indent).Append($"{type.FullName}.{method.Name}(");
                    else
                        sb.Indent(indent).Append($"obj.{method.Name}(");                    
                }
                   
                for (int i = 0; i < parameters.Length; i++)
                {
                    var paramInfo = parameters[i];
                    sb.Append($"t{i}");
                    if (i != parameters.Length - 1)
                    {
                        sb.Append(", ");
                    }
                }
               
                sb.Append(");\n");

                if (method.ReturnType != typeof(void))
                {
                    sb.Indent(indent).Append("Push(L, ret);\n");
                    sb.Indent(indent).Append("return 1;\n");
                }
                else
                {
                    sb.Indent(indent).Append("return 0;\n");
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
