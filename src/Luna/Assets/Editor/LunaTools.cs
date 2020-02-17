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

        static readonly ModuleInfo baseTypes = new ModuleInfo
        {
            new ClassInfo(typeof(GameObject)),
            new ClassInfo(typeof(Transform)),
            new ClassInfo(typeof(Vector2)),
            new ClassInfo(typeof(Vector3)),
            new ClassInfo(typeof(Vector4)),
            new ClassInfo(typeof(Quaternion)),
            new ClassInfo(typeof(Plane)),
            new ClassInfo(typeof(LayerMask)),
            new ClassInfo(typeof(Ray)),
            new ClassInfo(typeof(Bounds)),
            new ClassInfo(typeof(Color)),
            new ClassInfo(typeof(Touch)),
            new ClassInfo(typeof(RaycastHit)),
            new ClassInfo(typeof(TouchPhase)),
        };

        public static ModuleInfo customTypes = new ModuleInfo
        {
        };

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

            GenerateModule(baseTypes, path);

            path = Application.dataPath + "/Luna.Unity/CustomType/";
            if (Directory.Exists(path))
            {
                Directory.Delete(path, true);
            }

            GenerateModule(customTypes, path);

            AssetDatabase.Refresh();
        }

        static void GenerateModule(ModuleInfo moduleInfo, string path)
        {
            WrapGenerator.ExportPath = path;

            foreach (var t in customTypes)
            {
                WrapGenerator.GenerateClassWrap(t.type, t.excludeMembers);
            }

        }

        static void GenerateModule(Type[] types, string path)
        {
            WrapGenerator.ExportPath = path;

            foreach (var t in types)
            {
                WrapGenerator.GenerateClassWrap(t, null);
            }

        }
    }
}
