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
    using static BindHelper;

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

        int Loader(LuaState L)
        {
            string fileName = L.ToString(1);
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
                luaL_error(L, "error loading module {0} from file {1}:\n\t{2}", L.ToString(1), fileName, L.ToString(-1));
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
            WrapGenerator.ExportPath = "../src/Test/Shared/Generate/";
            WrapGenerator.GenerateClassWrap(typeof(TestStruct));

        }

        private void Luna_PostInit()
        {
            AutoBind();
            //CustomBind();
        }

        void AutoBind()
        {
            luna.RegisterClass(typeof(TestEnum));
            luna.RegisterClass(typeof(TestStruct));
            luna.RegisterClass(typeof(TestClass));

            var c = luna.GetGlobal("TestClass");
            var it = c.GetEnumerator();

            Luna.Log("TestClass : ======================== ");

            while(it.MoveNext())
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

        void CustomBind()
        {
            var t = luna.Bind
            [
                Class<TestClass>()
                [
                    Constant("constVar", TestClass.constVar),
                    Enum<TestEnum>(),
                    Property<TestClass, string>("staticProp"),
                    Variable("staticVar", () => TestClass.staticVar, (val) => TestClass.staticVar = val),

                    Method<TestClass>("StaticFunc"),
                    Method<TestClass, string>("StaticFunc1"),
                    Method<TestClass, string, int>("StaticFunc2"),

                    Constructor<TestClass>(),
                    Variable<TestClass, string>("variable", (_) => _.variable, (_, val) => _.variable = val),
                    Property<TestClass, string>("name"),

                    Method<TestClass>("Method"),
                    Method<TestClass, string>("Method1"),
                    Method<TestClass, string, float>("Method2"),

                    MethodR<TestClass, string>("Fun0"),
                    MethodR<TestClass, int, string>("Fun1"),
                    MethodR<TestClass, int, float>("Fun2")
                ]
            ];

        }

    }
}
