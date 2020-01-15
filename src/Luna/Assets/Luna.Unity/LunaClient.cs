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

            loader = new ResLoader();
            loader.AddSearchPath("core");

            luna.Run();

        }

        void OnPreInit()
        {
            luna.RegisterWraps(this.GetType());
        }

        void OnPostInit()
        {
            luna.RegisterClass<UnityEngine.Object>();

            luna.RegisterClass<GameObject>();
            luna.RegisterClass<Component>();
            luna.RegisterClass<MonoBehaviour>();
            luna.RegisterClass<Transform>();

            luna.RegisterClass<Vector2>();
            luna.RegisterClass<Vector3>();
            luna.RegisterClass<Vector4>();

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
            luna?.Dispose();
            luna = null;
            Instance = null;
        }

    }

}
