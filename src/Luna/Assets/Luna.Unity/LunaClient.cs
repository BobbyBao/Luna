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

            Luna.Print = Print;
            Luna.Error = Error;
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


            //luna.RegisterClass<Vector3>();

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
