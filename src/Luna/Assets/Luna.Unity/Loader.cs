using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace SharpLuna.Unity
{

    public abstract class Loader
    {
        protected List<string> searchers = new List<string>();

        public string LuaPath { get; set; } = "lua/";

        public Loader AddSearchPath(string path)
        {
            searchers.Add(path);
            return this;
        }

        public static string CombinePath(string dir, string path)
        {
            if (path.StartsWith("/"))
            {
                if (dir.EndsWith("/"))
                {
                    return dir + path.Substring(1);
                }
                else
                    return dir + path;
            }
            else
            {
                if (dir.EndsWith("/"))
                {
                    return dir + path;
                }
                else
                    return dir + "/" + path;
            }
        }


        public abstract byte[] ReadBytes(string fileName);
    }

    public class ResLoader : Loader
    {
        public override byte[] ReadBytes(string fileName)
        {
            if (fileName.EndsWith(".lua"))
            {
                fileName = fileName.Remove(fileName.Length - 4, 4);
            }

            fileName = fileName.Replace(".", "/");

            byte[] buffer = null;
            string filePath = CombinePath(LuaPath, fileName);
            var bytes = Resources.Load<BytesAsset>(filePath);
            if (bytes != null)
            {
                buffer = bytes.bytes;
                Resources.UnloadAsset(bytes);
                return buffer;
            }
            else
            {
                Debug.LogError(filePath);
            }

            foreach (var path in searchers)
            {
                filePath = CombinePath(path, fileName);
                bytes = Resources.Load<BytesAsset>(CombinePath(LuaPath, filePath));

                if (bytes != null)
                {
                    buffer = bytes.bytes;
                    Resources.UnloadAsset(bytes);
                    break;
                }
                else
                {
                    Debug.LogError(filePath);
                }
            }
            
            return buffer;
        }
    }

    public class ABLoader : Loader
    {
        public override byte[] ReadBytes(string fileName)
        {
            throw new NotImplementedException();
        }
    }
}
