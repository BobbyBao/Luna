using Tests;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;
using SharpLuna;

namespace Tests
{
    using static Lua;

    public class TestFramework : IDisposable
    {
        Luna luna;
        public Luna Luna => luna;
        public static string dataPath = "../../../../Test/Scripts/";

        public TestFramework()
        {          
            Luna.ReadBytes = ReadBytes;

            luna = new Luna();
            luna.PostInit += Luna_PostInit;
            
            GenerateWraps();

            luna.Start();
            luna.AddSearcher(Loader);           
        }

        public void Dispose()
        {
            luna.Dispose();
        }

        string[] searchers =
        {
            "core/",
            "math/",
        };

        byte[] ReadBytes(string fileName)
        {
            byte[] buffer = null;
            string filePath = fileName;
            int i = 0;
            while (!File.Exists(dataPath + filePath))
            {
                if (i >= searchers.Length)
                {
                    break;
                }

                filePath = searchers[i++] + fileName;
            }

            buffer = File.ReadAllBytes(dataPath + filePath);
            return buffer;
        }

        int Loader(IntPtr L)
        {
            string fileName = Luna.ToString(1);
            fileName = fileName.Replace(".", "/");
            if (!fileName.EndsWith(Luna.Ext))
            {
                fileName += Luna.Ext;
            }

            var buffer = ReadBytes(fileName);

            if (buffer == null)
            {
                string error = "Load file failed : " + fileName;
                lua_pushstring(L, error);
                return 1;
            }

            if (luaL_loadbuffer(L, buffer, "@" + fileName) != 0)
            {
                luaL_error(L, "error loading module {0} from file {1}:\n\t{2}", Luna.ToString(1), fileName, Luna.ToString(-1));
            }

            lua_pushstring(L, fileName);
            return 2;

        }


        public void Run()
        {
            luna.DoFile("test.luna");
        }

        private void GenerateWraps()
        {
            string path = "../../../../Test/Shared/Generate/";

            if (Directory.Exists(path))
            {
                luna.RegisterWraps(this.GetType());
                return;
            }
     
            WrapGenerator.ExportPath = path;
            //WrapGenerator.GenerateClassWrap(typeof(string));
            WrapGenerator.GenerateClassWrap("", typeof(TestStruct));
            WrapGenerator.GenerateClassWrap("", typeof(TestClass));

            luna.RegisterWraps(this.GetType());
        }

        private void Luna_PostInit()
        {
            AutoBind();
        }

        void AutoBind()
        {
            Type[] testTypes =
            {
                typeof(TestEnum),
                typeof(TestStruct),
                typeof(TestClass) 
            };

            luna.RegisterModel("Tests", testTypes);
        }

        void TestTable()
        {

            var c = luna.GetGlobal("TestClass");
            var it = c.GetEnumerator();

            Debug.Log("TestClass : ======================== ");

            while (it.MoveNext())
            {
                Debug.Log(it.Current.Key<string>());
            }

            it.Reset();
            Debug.Log("TestClass : ======================== ");

            while (it.MoveNext())
            {
                Debug.Log(it.Current.Key<string>());
            }

            Debug.Log("TestClass : ======================== ");

            foreach (var e in c)
            {
                Debug.Log(e.Key<string>());
            }

            it.Dispose();
        }

    }
}
