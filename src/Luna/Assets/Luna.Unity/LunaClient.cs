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

        private Luna luna;
        public Loader loader { get; set; }

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

            loader = new ResLoader
            {
#if LUNA_SCRIPT
                ScriptPath = "luna/"
#else
                ScriptPath = "lua/"
#endif
            };

            loader.AddSearchPath("core");

            luna.Run();

            LunaCreate?.Invoke(luna);
        }

        private void Start()
        {
            if (!string.IsNullOrWhiteSpace(startFile))
            {
                Run(startFile);
            }
        }

        public void Run(string file)
        {
            luna.DoFile(file);
        }

        private byte[] ReadBytes(string fileName) => loader?.ReadBytes(fileName);

        private void OnDestroy()
        {
            LunaDestroy?.Invoke(luna);
            luna?.Dispose();
            luna = null;
            Instance = null;
        }

        void OnPreInit()
        {
            //luna.RegisterWraps(this.GetType());
        }

        static Type[] unityModule =
        {
            typeof(UnityEngine.Object),
            typeof(GameObject),
            typeof(Component),
            typeof(MonoBehaviour),
            typeof(Transform),
        };

        void OnPostInit()
        {
            luna.RegisterModel("UnityEngine", unityModule);

            luna.RegisterClass<Vector2>();
            luna.RegisterClass<Vector3>();
            luna.RegisterClass<Vector4>();
            luna.RegisterClass<Quaternion>();


            luna.RegisterClass<LunaBehaviour>();

        }

    }

}
