using System;
using System.Collections.Generic;
using UnityEngine;

namespace SharpLuna.Unity
{
    public abstract class ScriptLoader
    {
        protected List<string> searchers = new List<string>();

        public string ScriptPath { get; set; } = "lua/";

        public bool Loaded { get; protected set; } = false;

        public ScriptLoader AddSearchPath(string path)
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

        public virtual void PreLoad() { }
        public abstract byte[] ReadBytes(string fileName);
    }

    public class ResScriptLoader : ScriptLoader
    {
        public override void PreLoad()
        {
            Loaded = true;
        }

        public override byte[] ReadBytes(string fileName)
        {
            if (fileName.EndsWith(Luna.Ext))
            {
                fileName = fileName.Remove(fileName.Length - 4, 4);
            }

            fileName = fileName.Replace(".", "/");

            byte[] buffer = null;
            string filePath = CombinePath(ScriptPath, fileName);
            var bytes = Resources.Load<BytesAsset>(filePath);
            if (bytes != null)
            {
                buffer = bytes.bytes;
                Resources.UnloadAsset(bytes);
                return buffer;
            }

            foreach (var path in searchers)
            {
                filePath = CombinePath(path, fileName);
                bytes = Resources.Load<BytesAsset>(CombinePath(ScriptPath, filePath));

                if (bytes != null)
                {
                    buffer = bytes.bytes;
                    Resources.UnloadAsset(bytes);
                    break;
                }
            }
            
            return buffer;
        }
    }

    public class ABScriptLoader : ScriptLoader
    {
        public const string BundleResPath = "/Res";
        public readonly static string BundleStreamingPath = Application.dataPath + "/StreamingAssets" + BundleResPath;
        public static string BundlePath(string subPath) => $"Assets{BundleResPath}/{subPath}";

        Dictionary<string, AssetBundle> boundles = new Dictionary<string, AssetBundle>();

        public override void PreLoad()
        {
            foreach (var searcher in searchers)
            {
                var path = CombinePath(ScriptPath, searcher);
                var bundle = AssetBundle.LoadFromFile(path);
                boundles.Add(searcher, bundle);
            }

            Loaded = true;
        }

        public override byte[] ReadBytes(string fileName)
        {
            if (!Loaded)
            {
                return null;
            }

            foreach (var it in boundles)
            {
                var abName = it.Key;
                var ab = it.Value;
                string path = fileName;
                if (!path.StartsWith(abName, StringComparison.OrdinalIgnoreCase))
                {
                    path = abName + "/" + fileName;
                }

                var assetName = BundlePath(path);
                if (!ab.Contains(assetName))
                {
                    continue;
                }

                var ba = ab.LoadAsset<BytesAsset>(path);
                if (ba)
                {
                    var bytes = ba.bytes;
                    Resources.UnloadAsset(ba);
                    return bytes;
                }
            }

            return null;
        }
    }
}
