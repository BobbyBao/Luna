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
        
        public event Action onPostInit;

        public static ModuleInfo testModule = new ModuleInfo("Tests")
        {
            typeof(TestEnum),typeof(TestStruct),typeof(TestClass),
        };

        public TestFramework(params ModuleInfo[] modules)
        {          
            Luna.ReadBytes = ReadBytes;
            luna = new Luna(testModule);
            foreach(var m in modules)
            {
                luna.AddModuleInfo(m);
            }
            luna.PostInit += Luna_PostInit;
            Lua.Encoding = Encoding.UTF8;
        }

        public void Start()
        {   
            GenerateWraps();

            luna.Start();
            luna.AddSearcher(Loader);
            var L = luna.State;
            //luaopen_pb(luna.State);
            lua_requiref(L, "pb", luaopen_pb);
            /*
            lua_requiref(L, "pb.slice", luaopen_pb_slice);
            lua_requiref(L, "pb.buffer", luaopen_pb_buffer);
            lua_requiref(L, "pb.conv", luaopen_pb_conv);*/
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

        public void GenerateWraps(bool force = false)
        {
            string path = "../../../Test/Shared/Generate/";

            if(!force)
            {
                if (Directory.Exists(path))
                {
                    luna.RegisterWraps(this.GetType());
                    return;
                }
            }

            WrapGenerator.GenerateModule(testModule, path);
            
            luna.RegisterWraps(this.GetType());
        }

        private void Luna_PostInit()
        {
            //AutoBind();
            onPostInit?.Invoke();
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


    }
}
