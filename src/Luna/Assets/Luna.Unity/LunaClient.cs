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

        public static readonly ModuleInfo baseTypes = new ModuleInfo("UnityEngine")
        {
            new ClassInfo(typeof(UnityEngine.Object)),
            new ClassInfo(typeof(GameObject)),
            new ClassInfo(typeof(Component)),
            new ClassInfo(typeof(MonoBehaviour)),
            new ClassInfo(typeof(Transform)),
            new ClassInfo(typeof(Resources)),
        };

        public static ModuleInfo customTypes = new ModuleInfo
        {
            new ClassInfo(typeof(LunaBehaviour)),

            //new ClassInfo(typeof(ResourceMananger))
        };

        private Luna luna;
        public ScriptLoader loader { get; set; }

        public string startFile;

        void Awake()
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);

            Luna.Print = Debug.Log;
            Luna.Warning = Debug.LogWarning;
            Luna.Error = Debug.LogError;
            Luna.ReadBytes = ReadBytes;

            luna = new Luna();
            luna.PreInit += OnPreInit;
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

            luna.Run();

            LunaCreate?.Invoke(luna);
        }

        private byte[] ReadBytes(string fileName) => loader?.ReadBytes(fileName);

        private void Start()
        {
            loader?.PreLoad();

            if (!string.IsNullOrWhiteSpace(startFile))
            {
                Run(startFile);
            }
        }

        void OnPreInit()
        {
        }

        void OnPostInit()
        {
            luna.RegisterModel(mathTypes);
            luna.RegisterModel(baseTypes);
            luna.RegisterModel(customTypes);

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
