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
            luna.PreInit += Luna_PreInit;
            luna.PostInit += Luna_PostInit;
            luna.Run();
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

            Luna.Log("LiveCount:", RefCountHelper.LiveCount, "FreeCount:", RefCountHelper.FreeCount);
            RefCountHelper.Collect();

            Luna.Log("LiveCount:", RefCountHelper.LiveCount, "FreeCount:", RefCountHelper.FreeCount);
        }

        private void Luna_PreInit()
        {
            string path = "../../../../Test/Shared/Generate/";
      
            if (Directory.Exists(path))
            {
                luna.RegisterWraps(this.GetType());
                return;
            }
     
            WrapGenerator.ExportPath = path;
            WrapGenerator.GenerateClassWrap(typeof(TestStruct));
            WrapGenerator.GenerateClassWrap(typeof(TestClass));

            luna.RegisterWraps(this.GetType());
        }

        private void Luna_PostInit()
        {
            AutoBind();
        }

        void AutoBind()
        {
            luna.RegisterClass(typeof(TestEnum));
            luna.RegisterClass(typeof(TestStruct));
            luna.RegisterClass(typeof(TestClass));

        }

        void TestTable()
        {

            var c = luna.GetGlobal("TestClass");
            var it = c.GetEnumerator();

            Luna.Log("TestClass : ======================== ");

            while (it.MoveNext())
            {
                Luna.Log(it.Current.Key<string>());
            }

            it.Reset();
            Luna.Log("TestClass : ======================== ");

            while (it.MoveNext())
            {
                Luna.Log(it.Current.Key<string>());
            }

            Luna.Log("TestClass : ======================== ");

            foreach (var e in c)
            {
                Luna.Log(e.Key<string>());
            }

            Luna.Log("LiveCount:", RefCountHelper.LiveCount, "FreeCount:", RefCountHelper.FreeCount);
        }

    }
}
