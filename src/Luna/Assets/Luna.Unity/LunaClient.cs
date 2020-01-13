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

            luna = new Luna
            {
                Print = Print,
                Error = Error,
                ReadBytes = ReadBytes,
            };

            luna.PreInit += OnPreInit;
            luna.PostInit += OnPostInit;

            luna.Run();

            loader = new ResLoader();
            loader.AddSearchPath("core");
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

        static void Print(string msg) => Debug.Log(msg);
        static void Error(string msg) => Debug.LogError(msg);

        byte[] ReadBytes(string fileName) => loader.ReadBytes(fileName);
        
        void OnDestroy()
        {
            luna?.Dispose();
            luna = null;
            Instance = null;
        }

    }

}
