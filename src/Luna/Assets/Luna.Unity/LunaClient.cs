using SharpLuna;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SharpLuna.Unity
{

    public class LunaClient : MonoBehaviour
    {
        public static LunaClient Instance { get; private set; }
        public static event Action<Luna> LunaCreate;
        public static event Action<Luna> LunaDestroy;

        public static Luna Luna => Instance?.luna;

        public static readonly ModuleInfo mathTypes = new ModuleInfo
        {
            typeof(Vector2),
            typeof(Vector3),
            typeof(Vector4),
            typeof(Quaternion),
            typeof(Plane),
            typeof(LayerMask),
            typeof(Ray),
            typeof(Bounds),
            typeof(Color),
            typeof(Touch),
            typeof(RaycastHit),
            typeof(TouchPhase),
        };

        public static readonly ModuleInfo baseTypes = new ModuleInfo("UnityEngine")
        {
            typeof(UnityEngine.Object),
            typeof(GameObject),
            typeof(Component),
            typeof(MonoBehaviour),
            typeof(Transform),
            typeof(RectTransform),
            typeof(Resources),
            typeof(AsyncOperation),
        };

        private Luna luna;
        public ScriptLoader loader { get; set; }

        public string startFile;

        private List<ModuleInfo> modules = new List<ModuleInfo>
        {
            mathTypes, baseTypes
        };

        System.Diagnostics.Stopwatch sw = new System.Diagnostics.Stopwatch();

        private void Awake()
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);

            OnInit();

            Luna.Print = Debug.Log;
            Luna.Warning = Debug.LogWarning;
            Luna.Error = Debug.LogError;
            Luna.ReadBytes = ReadBytes;

            luna = new Luna(modules.ToArray());
            luna.PostInit += OnPostInit;

            loader = new ResScriptLoader
            {
#if LUNA_SCRIPT
                ScriptPath = "luna/"
#else
                ScriptPath = "lua/"
#endif
            };

            loader.AddSearchPath("core");
            loader.AddSearchPath("math");

            sw.Start();

            luna.Run();

            sw.Stop();

            Luna.Log("Luna init time :", sw.ElapsedMilliseconds, "ms");

            LunaCreate?.Invoke(luna);
        }

        public void AddModule(ModuleInfo moduleInfo) => this.modules.Add(moduleInfo);

        private byte[] ReadBytes(string fileName) => loader?.ReadBytes(fileName);

        private IEnumerator Start()
        {
            loader?.PreLoad();

            if (!string.IsNullOrWhiteSpace(startFile))
            {
                Run(startFile);
            }

            yield return OnStart();
        }

        protected virtual void OnInit()
        {
        }

        protected virtual void OnPostInit()
        {
        }

        protected virtual IEnumerator OnStart()
        {
            yield return null;
        }

        public void Run(string file)
        {
            luna.DoFile(file);
        }

        private void OnDestroy()
        {
            LunaDestroy?.Invoke(luna);
            luna?.Dispose();
            luna = null;
            Instance = null;
        }


    }

}
