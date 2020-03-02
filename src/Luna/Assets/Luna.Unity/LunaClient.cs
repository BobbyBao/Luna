using SharpLuna;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace SharpLuna.Unity
{

    public class LunaClient : MonoBehaviour
    {
        public static LunaClient Instance { get; private set; }
        public static event Action<Luna> LunaCreate;
        public static event Action<Luna> LunaDestroy;

        public static Luna Luna => Instance?.luna;

        public static readonly ModuleInfo baseTypes = new ModuleInfo("UnityEngine")
        {
            typeof(UnityEngine.Object),
            typeof(GameObject),
            typeof(Component),
            typeof(MonoBehaviour),
            typeof(Transform),
            typeof(RectTransform),
            typeof(Renderer),
            typeof(MeshRenderer),
            typeof(SkinnedMeshRenderer),
            typeof(Resources),
            typeof(AsyncOperation),
            typeof(Input),

            typeof(Time),
            typeof(WWW),
            typeof(Space),
            typeof(UnityEngine.Rendering.ShadowCastingMode),
        };

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

        public static readonly ModuleInfo uiTypes = new ModuleInfo
        {
            typeof(Image),
            typeof(Button),
            typeof(RawImage),
            typeof(EventTriggerType),
            typeof(BaseEventData),
        };

        private Luna luna;
        public ScriptLoader loader { get; set; }

        public string startFile;

        private List<ModuleInfo> modules = new List<ModuleInfo>
        {
            mathTypes, baseTypes, uiTypes
        };

        System.Diagnostics.Stopwatch sw = new System.Diagnostics.Stopwatch();

        LuaRef update;
        LuaRef fixedUpdate;
        LuaRef lateUpdate;

        private void Awake()
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);

            sw.Start();

            Luna.ReadBytes = ReadBytes;

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
            
            OnPreInit();

            sw.Stop();

            Debug.Log($"Luna preinit time : {sw.ElapsedMilliseconds} ms");

            sw.Start();

            luna = new Luna(modules.ToArray());
            luna.PostInit += OnPostInit;

            OnInit();

            sw.Stop();

            Debug.Log($"Luna init time : { sw.ElapsedMilliseconds} ms");

            sw.Start();

            luna.Start();

            sw.Stop();

            Debug.Log($"Luna start time : {sw.ElapsedMilliseconds} ms");

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

        protected virtual void OnPreInit()
        {
        }

        protected virtual void OnInit()
        {
            Converter.RegisterAction<UnityEngine.Object>();
            Converter.RegisterAction<GameObject>();

            Converter.RegisterFunc<UnityEngine.Object>();
            Converter.RegisterFunc<GameObject>();

        }

        protected virtual void OnPostInit()
        {
            LuaCoroutine.Register(Luna, this);
        }

        protected virtual IEnumerator OnStart()
        {
            yield return null;
        }

        public void Run(string file)
        {
            luna.DoFile(file);

            update = luna.GetGlobal("Update");
            fixedUpdate = luna.GetGlobal("FixedUpdate");
            lateUpdate = luna.GetGlobal("LateUpdate");

        }

        protected virtual void Update()
        {
            if(update)
            {
                update.Call(Time.deltaTime, Time.unscaledDeltaTime);
            }
            
        }

        protected virtual void FixedUpdate()
        {

            if (fixedUpdate)
            {
                fixedUpdate.Call(Time.fixedDeltaTime);
            }

        }

        protected virtual void LateUpdate()
        {
            if (lateUpdate)
            {
                lateUpdate.Call();
            }

        }

        private void OnDestroy()
        {
            update?.Dispose();
            fixedUpdate?.Dispose();
            lateUpdate?.Dispose();

            LunaDestroy?.Invoke(luna);
            luna?.Dispose();
            luna = null;
            Instance = null;
        }


    }

}
