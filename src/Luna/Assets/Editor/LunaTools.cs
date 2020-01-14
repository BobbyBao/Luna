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

        static readonly (Type classType, MemberTypes memberTypes)[] baseTypes = 
        {
            (typeof(Vector3), MemberTypes.Constructor |  MemberTypes.Field)
        };

        static readonly (Type classType, MemberTypes memberTypes)[] customTypes =
        {
            (typeof(LunaBehaviour), MemberTypes.Constructor |  MemberTypes.Field)
        };


        [MenuItem("Luna/生成WrapFile")]
        public static void GenerateWraps()
        {
            var path = Application.dataPath + "/Luna.Unity/BaseType/";
            
            if (Directory.Exists(path))
            {
                Directory.Delete(path, true);
            }
            
            WrapGenerator.ExportPath = path;

            foreach (var(type, member) in baseTypes)
            {
                WrapGenerator.GenerateClassWrap(type);
            }

            path = Application.dataPath + "/Luna.Unity/CustomType/";

            if (Directory.Exists(path))
            {
                Directory.Delete(path, true);
            }

            WrapGenerator.ExportPath = path;

            foreach (var (type, member) in customTypes)
            {
                WrapGenerator.GenerateClassWrap(type);
            }

            AssetDatabase.Refresh();
        }
    }
}
