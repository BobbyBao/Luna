using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;

namespace SharpLuna
{
    public class WrapGenerator
    {
        static HashSet<Type> generatedTypes = new HashSet<Type>();

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

        public static void Clear()
        {
            generatedTypes.Clear();
            enums.Clear();
            genericTypes.Clear();
            classDelegates.Clear();
        }

        static HashSet<string> namespaces = new HashSet<string>();
        static HashSet<Type> enums = new HashSet<Type>();
        static HashSet<Type> genericTypes = new HashSet<Type>();
        static HashSet<Type> classDelegates = new HashSet<Type>();

        static Dictionary<Type, string> allDelegates = new Dictionary<Type, string>();

        struct MemberGenInfo
        {
            public MemberTypes memberType;
            public string name;
            public string alias;
            public bool hasGetter;
            public bool hasSetter;

            public MemberGenInfo(MemberTypes t, string n, bool hasGetter = false, bool hasSetter = false, string alias = "")
            {
                this.memberType = t;
                this.name = n;
                this.alias = alias;
                this.hasGetter = hasGetter;
                this.hasSetter = hasSetter;
            }

            public string Name => string.IsNullOrEmpty(alias) ? name : alias;
        }

        public static void GenerateClassWrap(string module, Type type, bool genSuper = false, List<string> excludeMembers = null)
        {
            string code = GenerateClass(module, type, genSuper, excludeMembers);
            string fileName = type.Name.Replace("[]", "Array") + "Wrap.cs";
            if(!string.IsNullOrEmpty(module))
            {
                fileName = module + "_" + fileName;
            }

            File.WriteAllText(Path.Combine(exportPath, fileName), code);
        }

        static string GenerateClass(string module, Type type, bool genSuper, List<string> excludeMembers)
        {
            StringBuilder sbHead = new StringBuilder();
            namespaces.Add("System");
            namespaces.Add("SharpLuna");
            namespaces.Add("System.Collections.Generic");

            StringBuilder sb = new StringBuilder();
            sb.Append("using static SharpLuna.Lua;\n");

            sb.AppendLine();

            sb.Append("[WrapClass(typeof(" + type.FullName + "))]\n");
            sb.Append("public class ");

            if (!string.IsNullOrEmpty(module))
            {
                sb.Append(module).Append("_");
            }

            sb.Append(type.Name.Replace("[]", "Array")).Append("Wrap\n{\n");

            sb.Indent(1).Append($"#if LUNA_SCRIPT\n");
            sb.Indent(1).Append($"const int startStack = 2;\n");
            sb.Indent(1).Append($"#else\n");
            sb.Indent(1).Append($"const int startStack = 1;\n");
            sb.Indent(1).Append($"#endif\n");

            List<MemberGenInfo> members =
                new List<MemberGenInfo>();

            if (genSuper)
            {
                Type currentType = type;
                do
                {
                    currentType = currentType.BaseType;

                    if (generatedTypes.Contains(currentType))
                    {
                        break;
                    }

                    GenerateClassWrap(type, excludeMembers, sb, members);

                } while (currentType != null);

            }

            GenerateClassWrap(type, excludeMembers, sb, members);

            sb.Append($"\tpublic static void Register(ClassWraper classWraper)\n\t{{\n");

            foreach (var memberGenInfo in members)
            {
                if (memberGenInfo.memberType == MemberTypes.Constructor)
                {
                    sb.Append($"\t\tclassWraper.RegConstructor(Constructor);\n");
                }
                else if (memberGenInfo.memberType == MemberTypes.Field)
                {
                    if (memberGenInfo.hasGetter || memberGenInfo.hasSetter)
                    {
                        sb.Append($"\t\tclassWraper.RegField(\"{memberGenInfo.name}\"");

                        if (memberGenInfo.hasGetter)
                        {
                            sb.Append($", Get_{memberGenInfo.name}");
                        }

                        if (memberGenInfo.hasSetter)
                        {
                            sb.Append($", Set_{memberGenInfo.name}");
                        }

                        sb.Append($");\n");
                    }
                }
                else if (memberGenInfo.memberType == MemberTypes.Property)
                {
                    if (memberGenInfo.hasGetter || memberGenInfo.hasSetter)
                    {
                        sb.Append($"\t\tclassWraper.RegProperty(\"{memberGenInfo.name}\"");

                        if (memberGenInfo.hasGetter)
                        {
                            sb.Append($", Get_{memberGenInfo.name}");
                        }

                        if (memberGenInfo.hasSetter)
                        {
                            sb.Append($", Set_{memberGenInfo.name}");
                        }

                        sb.Append($");\n");
                    }
                }
                else if (memberGenInfo.memberType == MemberTypes.Method)
                {
                    sb.Append($"\t\tclassWraper.RegFunction(\"{memberGenInfo.name}\", {memberGenInfo.Name});\n");
                }

            }

            sb.AppendLine();
            foreach (var delType in classDelegates)
            {
                if(allDelegates.TryGetValue(delType, out var delFn))
                {
                    if (!string.IsNullOrEmpty(delFn))
                    {
                        var fullName = delType.GetFriendlyName();
                        sb.Append($"\t\tConverter.Register<{fullName}>({delFn});\n");
                        allDelegates[delType] = "";
                    }                 
                }
            }
            
            classDelegates.Clear();

            sb.Append("\t}\n");

            sb.Append("}");

            foreach (var @namespace in namespaces)
            {
                sbHead.Append("using ").Append(@namespace).Append(";").AppendLine();
            }

            sbHead.Append(sb);
            return sbHead.ToString();

        }

        private static void GenerateClassWrap(Type type, List<string> excludeMembers, StringBuilder sb, List<MemberGenInfo> members)
        {
            var ctors = type.GetConstructors(BindingFlags.Public | BindingFlags.Instance);
            List<ConstructorInfo> ctorList = new List<ConstructorInfo>();
            foreach (var ctor in ctors)
            {
                if (!ctor.ShouldExport())
                {
                    continue;
                }

                if (!ctor.IsPublic)
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

                    if(!classDelegates.Contains(p.ParameterType))
                    {
                        if(p.ParameterType.IsSubclassOf(typeof(Delegate)))
                        {
                            classDelegates.Add(p.ParameterType);
                        }

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
                members.Add(new MemberGenInfo(MemberTypes.Constructor, "ctor", false, false));
            }

            if (!type.IsEnum)
            {
                var fields = type.GetFields(BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static | BindingFlags.DeclaredOnly);
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

                    if (!classDelegates.Contains(field.FieldType))
                    {
                        if (field.FieldType.IsSubclassOf(typeof(Delegate)))
                        {
                            classDelegates.Add(field.FieldType);
                        }

                    }

                    GenerateField(type, field, sb);
                    members.Add(new MemberGenInfo(MemberTypes.Field, field.Name, true, !field.IsLiteral && !field.IsInitOnly));
                }
            }

            var properties = type.GetProperties(BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static | BindingFlags.DeclaredOnly);
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

                if (!classDelegates.Contains(prop.PropertyType))
                {
                    if (prop.PropertyType.IsSubclassOf(typeof(Delegate)))
                    {
                        classDelegates.Add(prop.PropertyType);
                    }

                }

                GenerateProperty(type, prop, sb);
                members.Add(new MemberGenInfo(MemberTypes.Property, prop.Name, prop.CanRead, prop.CanWrite));
            }

            var methods = type.GetMethods(BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static | BindingFlags.DeclaredOnly);
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

                if (members.FindIndex((t) => t.name == method.Name) != -1)
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

                var memberInfo = type.GetMember(method.Name, BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static | BindingFlags.DeclaredOnly);
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

                        if (!classDelegates.Contains(p.ParameterType))
                        {
                            if (p.ParameterType.IsSubclassOf(typeof(Delegate)))
                            {
                                classDelegates.Add(p.ParameterType);
                            }

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
                    string methodName = GenerateMethod(type, methodInfos.ToArray(), paraments, sb);
                    members.Add(new MemberGenInfo(MemberTypes.Method, method.Name, false, false, methodName));
                }
            }
            /*
            foreach(var delType in classDelegates)
            {
                if(allDelegates.ContainsKey(delType))
                {
                    continue;
                }
                var name = GenerateDelegate(delType, sb);
                allDelegates[delType] = name;
            }

            generatedTypes.Add(type);*/
        }

        static void GenerateConstructor(Type type, List<ConstructorInfo> ctorList, StringBuilder sb)
        {
            bool isArray = type.IsArray;
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

                    if(isArray)
                    {
                        sb.Append($"\t\t\tobj = new {type.GetElementType().FullName}[");
                    }
                    else
                    {
                        sb.Append($"\t\t\tobj = new {type.FullName}(");
                    }

                    for (int i = 1; i <= parameters.Length; i++)
                    {
                        var paramInfo = parameters[i - 1];
                        sb.Append($"t{i}");
                        if (i != parameters.Length)
                        {
                            sb.Append(", ");
                        }
                    }

                    if (isArray)
                    {
                        sb.Append("];\n");
                    }
                    else
                    {
                        sb.Append(");\n");
                    }
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

        static string GenerateMethod(Type type, MethodInfo[] methodInfo, int[] paraments, StringBuilder sb)
        {
            bool isArrayGet = false;
            bool isArraySet = false;
            string methodName = methodInfo[0].Name;
            
            if (methodName == "Get")
            {
                methodName += "_Item";
                isArrayGet = type.IsArray;
            }

            if (methodName == "Set")
            {
                methodName += "_Item";
                isArraySet = type.IsArray;
            }
            
            sb.Append("\t[AOT.MonoPInvokeCallback(typeof(LuaNativeFunction))]\n");
            sb.Append($"\tstatic int {methodName}(IntPtr L)\n\t{{\n");

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
                        if (paraments[parameters.Length] > 1 && parameters.Length > 0)
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
                        if (paraments[parameters.Length] > 1 && parameters.Length > 0)
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
                {/*
                    if (parameters.Length > 0)
                    {
                        sb.Indent(indent).Append($"#if LUNA_SCRIPT\n");
                        sb.Indent(indent).Append($"const int startStack = 2;\n");
                        sb.Indent(indent).Append($"#else\n");
                        sb.Indent(indent).Append($"const int startStack = 1;\n");
                        sb.Indent(indent).Append($"#endif\n");
                    }*/
                    for (int i = 0; i < parameters.Length; i++)
                    {
                        var paramInfo = parameters[i];
                        sb.Indent(indent).Append($"Get(L, {i} + startStack, out {paramInfo.ParameterType.GetFriendlyName()} t{i});");
                        sb.AppendLine();
                    }

                }
                else
                {
                    if (type.IsUnManaged())
                    {
                        sb.Indent(indent).Append($"ref var obj = ref SharpObject.GetValue<{type.FullName}>(L, 1);\n");
                    }
                    else
                    {
                        sb.Indent(indent).Append($"var obj = SharpObject.Get<{type.FullName}>(L, 1);\n");
                    }

                    for (int i = 0; i < parameters.Length; i++)
                    {
                        var paramInfo = parameters[i];
                        sb.Indent(indent).Append($"Get(L, {i + 2}, out {paramInfo.ParameterType.GetFriendlyName()} t{i});");
                        sb.AppendLine();
                    }

                }

                if (method.ReturnType != typeof(void))
                {
                    sb.Indent(indent).Append($"{method.ReturnType.GetFriendlyName()} ret = ");
                    if (method.IsStatic)
                        sb.Append($"{type.FullName}.{method.Name}(");
                    else
                    {
                        if (isArrayGet || isArraySet)
                        {
                            sb.Append($"obj[");
                        }
                        else
                            sb.Append($"obj.{method.Name}(");
                    }
                }
                else
                {
                    if (method.IsStatic)
                        sb.Indent(indent).Append($"{type.FullName}.{method.Name}(");
                    else
                    {
                        if(isArrayGet || isArraySet)
                        {
                            sb.Indent(indent).Append($"obj[");
                        }
                        else
                            sb.Indent(indent).Append($"obj.{method.Name}(");
                    }
                }
                   
                for (int i = 0; i < parameters.Length - 1; i++)
                {
                    var paramInfo = parameters[i];
                    sb.Append($"t{i}");

                    if (i == parameters.Length - 2 && isArraySet)
                    {
                    }
                    else
                    {
                        sb.Append(", ");
                    }
                }

                if(parameters.Length - 1 >= 0)
                {
                    if (isArraySet)
                    {

                        sb.Append($"] = t{parameters.Length - 1};\n");
                    }
                    else
                    {
                        sb.Append($"t{parameters.Length - 1}");
                    }
                }
                
                if (isArrayGet)
                {
                    sb.Append("];\n");
                }
                else if (isArraySet)
                {
                }
                else
                {
                    sb.Append(");\n");
                }

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

            return methodName;
        }

        static string GenerateDelegate(Type delType, StringBuilder sb)
        {
            string delName = delType.GetFriendlyName();
            string[] strs = delName.Split('.', '<', '>');
            string name = string.Join("", strs);

            string parameters = "";
            int paramCount = 0;
            if(delType.IsGenericType)
            {
                paramCount = delType.GetGenericArguments().Length;
                for (int i = 1; i <= paramCount; i++)
                {
                    if(i != 1)
                    {
                        parameters += ", ";
                    }

                    parameters += "p" + i;
                }
            }

            sb.Append($"\tstatic {delName} {name}(IntPtr L, int index)\n\t{{\n");

            sb.Append("\t\tlua_pushvalue(L, index);\n");
            sb.Append("\t\tint luaref = luaL_ref(L, LUA_REGISTRYINDEX);\n");

            sb.Append($"\t\treturn ({parameters})=> \n\t\t{{\n");
            sb.Append("\t\t\tlua_pushcfunction(L, LuaException.traceback);\n");
            sb.Append("\t\t\tlua_rawgeti(L, LUA_REGISTRYINDEX, luaref);\n");

            for(int i = 1; i <= paramCount; i++)
            {
                sb.Append($"\t\t\tPush(L, p{i});\n");
            }

            sb.Append($"\t\t\tif (lua_pcall(L, {paramCount}, 0, -{paramCount} + 2) != (int)LuaStatus.OK)\n");
            sb.Append("\t\t\t{\n");
            sb.Append("\t\t\t\tlua_remove(L, -2);\n");
            sb.Append("\t\t\t\tthrow new LuaException(L);\n");
            sb.Append("\t\t\t}\n");
            sb.Append("\t\t\tlua_pop(L, 1);\n");

            sb.Append("\t\t};\n");
            sb.Append("\t}\n");
            sb.AppendLine();
            return name;
        }
    }

}
