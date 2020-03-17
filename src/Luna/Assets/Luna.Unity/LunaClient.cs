using SharpLuna;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.Networking;
using UnityEngine.UI;

namespace SharpLuna.Unity
{
    using static Lua;
    using lua_State = IntPtr;

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
            typeof(WaitForSeconds),
            typeof(WaitForFixedUpdate),
            typeof(WaitForEndOfFrame),

            typeof(UnityWebRequest),
            typeof(DownloadHandler),
            typeof(DownloadHandlerBuffer),

            typeof(Coroutine),
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

        public static readonly ModuleInfo uiTypes = new ModuleInfo("UnityEngine.UI")
        {
            typeof(Image),
            typeof(Button),
            typeof(RawImage),
            typeof(EventTriggerType),
            typeof(BaseEventData),
            typeof(UnityEvent),
            typeof(Button.ButtonClickedEvent),
        };

        private Luna luna;
        public ScriptLoader loader { get; set; }

        public string startFile;

        public bool cjson = true;
        public bool protobuf = true;

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
            Luna.LoadAssembly("mscorlib");
            Luna.LoadAssembly("UnityEngine");
            Luna.LoadAssembly("UnityEngine.UI");

            Converter.RegisterAction<UnityEngine.Object>();
            Converter.RegisterAction<GameObject>();
            Converter.RegisterAction<AsyncOperation>();            

            Converter.RegisterFunc<UnityEngine.Object>();
            Converter.RegisterFunc<GameObject>();

            Converter.RegDelegateFactory(IEnumeratorBridge.Create);

            Converter.RegDelegateFactory(CreateUnityAction);
        }

        protected virtual void OnPostInit()
        {
            luna.Register("luna.startCoroutine", _StartCoroutine);
            luna.Register("luna.stopCoroutine", _StopCoroutine);
            
            var L = luna.State;

            if(cjson)
                luaopen_cjson(L);

            if(protobuf)
                lua_requiref(L, "pb", luaopen_pb);
        }

        protected virtual IEnumerator OnStart()
        {
            yield return null;
        }

        public void Run(string file)
        {
            luna.DoFile(file);

            update = luna.GetGlobal("update");
            fixedUpdate = luna.GetGlobal("fixedUpdate");
            lateUpdate = luna.GetGlobal("lateUpdate");

        }

        [AOT.MonoPInvokeCallback(typeof(LuaNativeFunction))]
        static int _StartCoroutine(lua_State L)
        {
            Get(L, STATIC_STARTSTACK, out IEnumerator cor);
            var coro = Instance.StartCoroutine(cor);
            Push(L, coro);
            return 1;
        }

        [AOT.MonoPInvokeCallback(typeof(LuaNativeFunction))]
        static int _StopCoroutine(lua_State L)
        {
            Get(L, STATIC_STARTSTACK, out Coroutine cor);
            Instance.StopCoroutine(cor);
            return 0;
        }

        public static UnityAction CreateUnityAction(IntPtr L, int index)
        {
            lua_pushvalue(L, index);
            int luaref = luaL_ref(L, LUA_REGISTRYINDEX);
            return () =>
            {
                L = mainState;
                int errFunc = pcall_prepare(L, errorFuncRef, luaref);
                if (lua_pcall(L, 0, 0, errFunc) != (int)LuaStatus.OK)
                {
                    ThrowExceptionFromError(L, errFunc - 1);
                }
                lua_settop(L, errFunc - 1);
            };
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
