using SharpLuna.Unity;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;

namespace SharpLuna
{
    public class LunaTools
    {

        [MenuItem("Luna/生成WrapFile")]
        public static void GenerateWraps()
        {
            WrapGenerator.Clear();

            var path = Application.dataPath + "/LunaFramework/SystemType/";
            if (Directory.Exists(path))
            {
                Directory.Delete(path, true);
            }

            GenerateModule(Luna.systemModule, path);

            path = Application.dataPath + "/LunaFramework/BaseType/";
            if (Directory.Exists(path))
            {
                Directory.Delete(path, true);
            }

            GenerateModule(LunaClient.mathTypes, path);
            GenerateModule(LunaClient.baseTypes, path);

            path = Application.dataPath + "/LunaFramework/CustomType/";
            if (Directory.Exists(path))
            {
                Directory.Delete(path, true);
            }

            //GenerateModule(LunaClient.customTypes, path);

            var types = typeof(LunaClient).Assembly.GetTypes();
            foreach (var t in types)
            {
                if (!t.IsSubclassOf(typeof(LunaClient)))
                {
                    continue;
                }

                var fields = t.GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static);
                foreach (var fieldInfo in fields)
                {
                    var attr = fieldInfo.GetCustomAttribute(typeof(SharpLuna.LunaExportAttribute));
                    if (attr != null)
                    {
                        var moduleInfo = fieldInfo.GetValue(null) as ModuleInfo;
                        if(moduleInfo != null)
                        {
                            GenerateModule(moduleInfo, path);
                        }
                    }
                }
            }

            AssetDatabase.Refresh();
        }

        static void GenerateModule(ModuleInfo moduleInfo, string path)
        {
            WrapGenerator.ExportPath = path;

            foreach (var t in moduleInfo)
            {
                WrapGenerator.GenerateClassWrap(moduleInfo.Name, t.type, t.generateSuperMembers, t.excludeMembers);
            }

        }

    }
}
