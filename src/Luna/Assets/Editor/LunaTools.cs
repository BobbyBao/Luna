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
            var path = Application.dataPath + "/Luna.Unity/SystemType/";
            if (Directory.Exists(path))
            {
                Directory.Delete(path, true);
            }

            GenerateModule(Luna.systemModule, path);

            path = Application.dataPath + "/Luna.Unity/BaseType/";
            if (Directory.Exists(path))
            {
                Directory.Delete(path, true);
            }

            GenerateModule(LunaClient.mathTypes, path);
            GenerateModule(LunaClient.baseTypes, path);

            path = Application.dataPath + "/Luna.Unity/CustomType/";
            if (Directory.Exists(path))
            {
                Directory.Delete(path, true);
            }

            GenerateModule(LunaClient.customTypes, path);

            AssetDatabase.Refresh();
        }

        static void GenerateModule(ModuleInfo moduleInfo, string path)
        {
            WrapGenerator.ExportPath = path;

            foreach (var t in moduleInfo)
            {
                WrapGenerator.GenerateClassWrap(moduleInfo.Name, t.type, t.excludeMembers);
            }

        }

        static void GenerateModule(Type[] types, string path)
        {
            WrapGenerator.ExportPath = path;

            foreach (var t in types)
            {
                WrapGenerator.GenerateClassWrap("", t, null);
            }

        }
    }
}
