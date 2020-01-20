using System;
using System.Linq;
using System.Reflection;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace SharpLuna
{
    using static Lua;
    using lua_State = IntPtr;

    public class LunaConfig
    {
        public Dictionary<string, Type[]> modules;
        public Dictionary<Type, string[]> excludes;
    }

    public sealed class Luna : IDisposable
    {
#if LUNA_SCRIPT
        public const string Ext = ".luna";
#else
        public const string Ext = ".lua";
#endif
        public lua_State State => L;
        private lua_State L;
        private LuaRef _global;

        public bool IsExecuting => _executing;
        private bool _executing;

        public bool UseTraceback { get; set; } = false;

        public static Action<string> Print { get; set; }
        public static Action<string> Warning { get; set; }
        public static Action<string> Error { get; set; }
        public static Func<string, byte[]> ReadBytes { get; set; }

        public event Action PreInit;
        public event Action PostInit;
        public event EventHandler<HookExceptionEventArgs> HookException;
        public event EventHandler<DebugHookEventArgs> DebugHook;
        private KyHookFunction _hookCallback;

        private SharpModule _binder;
        private readonly Dictionary<Type, ClassWraper> _classWrapers = new Dictionary<Type, ClassWraper>();


        public Luna()
        {
        }

        ~Luna()
        {
            Dispose();
        }

        public void Run()
        {
            if (Print == null)
            {
                Print = Console.WriteLine;
            }

            if (Warning == null)
            {
                Warning = Console.WriteLine;
            }

            if (Error == null)
            {
                Error = Console.WriteLine;
            }

            if (ReadBytes == null)
            {
                ReadBytes = System.IO.File.ReadAllBytes;
            }

            L = Lua.NewState();
            
            luaL_openlibs(L);
            lua_atpanic(L, PanicCallback);

            Register("print", DoPrint);
            Register("dofile", DoFile);
            Register("loadfile", LoadFile);

            _global = LuaRef.Globals(L);
            _global.AddRef();
            _binder = new SharpModule(this);

            AddSearcher(LuaLoader);

            SharpClass.Init(L);
            SharpObject.Init(L);

            RegisterWraps(this.GetType());

            PreInit?.Invoke();

            Init(); 

            PostInit?.Invoke();

            var it = _classWrapers.GetEnumerator();
            while(it.MoveNext())
            {
                if (!SharpClass.IsRegistered(it.Current.Key))
                {
                    RegisterClass(it.Current.Key);
                }
            }

            _classWrapers.Clear();

            RefCountHelper.Collect();
            
        }

        private void Init()
        {
            SharpClass.SetAlias(typeof(object), "object");

            RegisterClass<object>();

            RegisterClass<Enum>();
            RegisterClass<string>();
            RegisterClass<Delegate>();

        }

        public void Dispose()
        {
            Close();

            GC.SuppressFinalize(this);
        }

        public void Close()
        {
            if (L == IntPtr.Zero)
                return;

            _binder.Dispose();
            
            _global.Release();

            RefCountHelper.Clear();

            Lua.CloseState(L);
            L = IntPtr.Zero;
        }

        public static void Log(params object[] args) => Print?.Invoke(string.Join("\t", args));
        public static void LogWarning(params object[] args) => Warning?.Invoke(string.Join("\t", args));
        public static void LogError(params object[] args) => Error?.Invoke(string.Join("\t", args));

        public void Register(string name, LuaNativeFunction function)
        {
            savedFn.TryAdd(function);
            lua_pushcfunction(L, function);
            lua_setglobal(L, name);
        }

        public void AddSearcher(LuaNativeFunction searcher)
        {
            var state = State;
            lua_getglobal(L, "package");
            lua_getfield(L, -1, "searchers");
            lua_pushcfunction(L, searcher);

            for (long i = luaL_len(L, -2) + 1; i > 2; i--)
            {
                lua_rawgeti(L, -2, i - 1);
                lua_rawseti(L, -3, i);
            }

            lua_rawseti(L, -2, 2);
            lua_pop(L, 2);
        }

        [AOT.MonoPInvokeCallback(typeof(LuaNativeFunction))]
        static int DoPrint(lua_State L)
        {
            int n = lua_gettop(L);

            StringBuilder sb = new StringBuilder();            
            for (int i = 1; i <= n; i++)
            {
                string s = lua_tostring(L, i);
                lua_pop(L, 1);
                if (i > 1) sb.Append("\t");
                sb.Append(s);
            }

            if(Print != null)
            {                
                Print(sb.ToString());
            }
            else
            {
                Console.WriteLine(sb.ToString());
            }
            return 0;
        }

        [AOT.MonoPInvokeCallback(typeof(LuaNativeFunction))]
        static int LuaLoader(lua_State L)
        {
            string fileName = lua_tostring(L, 1);

            byte[] buffer = ReadBytes(fileName);

            if (buffer == null)
            {
                string error = "Load file failed : " + fileName;
                lua_pushstring(L, error);
                return 1;
            }

            if (luaL_loadbuffer(L, buffer, "@" + fileName) != 0)
            {
                string err = lua_tostring(L, -1);
                throw new LuaException(err);
            }

            return 1;

        }

        [AOT.MonoPInvokeCallback(typeof(LuaNativeFunction))]
        private static int DoFile(lua_State L)
        {
            string fileName = lua_tostring(L, 1);
            int n = lua_gettop(L);
            byte[] buffer = ReadBytes(fileName);

            if (buffer == null)
            {
                string error = string.Format("cannot open {0}: No such file or directory", fileName);
                throw new LuaException(error);
            }

            if (luaL_loadbuffer(L, buffer, fileName) != LuaStatus.OK)
            {
                return lua_error(L);
            }

            if (lua_pcall(L, 0, -1, 0) != 0)
            {
                string error = lua_tostring(L, -1);
                throw new LuaException(error);
            }

            return lua_gettop(L) - n;

        }

        [AOT.MonoPInvokeCallback(typeof(LuaNativeFunction))]
        private static int LoadFile(lua_State L)
        {
            int oldTop = lua_gettop(L);
            string fileName = lua_tostring(L, 1);
            byte[] buffer = ReadBytes(fileName);

            if (buffer == null)
            {
                string error = string.Format("cannot open {0}: No such file or directory", fileName);
                throw new LuaException(error);
            }

            if (luaL_loadbuffer(L, buffer, fileName) == LuaStatus.OK)
            {
                return 1;
            }

            lua_pushnil(L);
            lua_insert(L, -2);  /* put before error message */
            return 2;
        }


        [AOT.MonoPInvokeCallback(typeof(LuaNativeFunction))]
        static int PanicCallback(lua_State L)
        {
            string reason = string.Format("Unprotected error in call to Lua API ({0})", lua_tostring(L, -1));
            throw new LuaException(reason);
        }

        private void ThrowExceptionFromError(int oldTop)
        {
            object err = GetObject(L, -1);

            lua_settop(L, oldTop);

            // A pre-wrapped exception - just rethrow it (stack trace of InnerException will be preserved)
            var luaEx = err as LuaScriptException;

            if (luaEx != null)
                throw luaEx;

            // A non-wrapped Lua error (best interpreted as a string) - wrap it and throw it
            if (err == null)
                err = "Unknown Lua Error";

            throw new LuaScriptException(err.ToString(), string.Empty);
        }

        private static int PushDebugTraceback(lua_State L, int argCount)
        {
            lua_getglobal(L, "debug");
            lua_getfield(L, -1, "traceback");
            lua_remove(L, -2);
            int errIndex = -argCount - 2;
            lua_insert(L, errIndex);
            return errIndex;
        }

        public string GetDebugTraceback()
        {
            int oldTop = lua_gettop(L);
            lua_getglobal(L, "debug"); // stack: debug
            lua_getfield(L, -1, "traceback"); // stack: debug,traceback
            lua_remove(L, -2); // stack: traceback
            lua_pcall(L, 0, -1, 0);
            return PopValues(L, oldTop)[0] as string;            
        }

        public object[] DoString(byte[] chunk, string chunkName = "chunk")
        {
            int oldTop = lua_gettop(L);
            _executing = true;

            if (luaL_loadbuffer(L, chunk, chunkName) != LuaStatus.OK)
                ThrowExceptionFromError(oldTop);

            int errorFunctionIndex = 0;

            if (UseTraceback)
            {
                errorFunctionIndex = PushDebugTraceback(L, 0);
                oldTop++;
            }

            try
            {
                if (lua_pcall(L, 0, -1, errorFunctionIndex) != LuaStatus.OK)
                    ThrowExceptionFromError(oldTop);
                
                return PopValues(L, oldTop);
            }
            finally
            {
                _executing = false;
            }
        }

        public object[] DoString(string chunk, string chunkName = "chunk")
        {
            int oldTop = lua_gettop(L);
            _executing = true;

            if (lua_loadstring(L, chunk, chunkName) != LuaStatus.OK)
                ThrowExceptionFromError(oldTop);

            int errorFunctionIndex = 0;

            if (UseTraceback)
            {
                errorFunctionIndex = PushDebugTraceback(L, 0);
                oldTop++;
            }

            try
            {
                if (lua_pcall(L, 0, -1, errorFunctionIndex) != LuaStatus.OK)
                    ThrowExceptionFromError(oldTop);

                return PopValues(L, oldTop);
            }
            finally
            {
                _executing = false;
            }
        }

        public object[] DoFile(string fileName)
        {
            int oldTop = lua_gettop(L);

            byte[] buffer = ReadBytes(fileName);
            if (luaL_loadbuffer(L, buffer, "@" + fileName) != LuaStatus.OK)
                ThrowExceptionFromError(oldTop);

            _executing = true;

            int errorFunctionIndex = 0;
            if (UseTraceback)
            {
                errorFunctionIndex = PushDebugTraceback(L, 0);
                oldTop++;
            }

            try
            {
                if (lua_pcall(L, 0, -1, errorFunctionIndex) != LuaStatus.OK)
                    ThrowExceptionFromError(oldTop);

                return PopValues(L, oldTop);
            }
            finally
            {
                _executing = false;
            }
        }

        public LuaRef Global => _global;

        public LuaRef GetGlobal(string fullPath)
        {
            return _global.RawGet(fullPath);
        }

        public string ToString(int index, bool callMetamethod = true)
        {
            var str = lua_tostring(L, index);

            if (callMetamethod)
            {
                lua_pop(L, 1);
            }

            return str;
        }

        public ClassWraper GetClassWrapper(Type type)
        {
            if (_classWrapers.TryGetValue(type, out var classWraper))
            {
                return classWraper;
            }

            classWraper = new ClassWraper();
            _classWrapers.Add(type, classWraper);
            return classWraper;
        }

        public bool IsRegistered(Type type)
        {
            return _classWrapers.ContainsKey(type);
        }

        public void RegisterWraps(Type type)
        {
            var types = type.Assembly.GetTypes();
            foreach(var t in types)
            {
                var attr = t.GetCustomAttribute<WrapClassAttribute>();
                if (attr == null)
                {
                    continue;
                }
                 
                AddWrapClass(attr.Type, t);
                
            }

        }
        
        void AddWrapClass(Type type, Type wrapType)
        {
            if(IsRegistered(type))
            {
                return;
            }

            var classWrapper = GetClassWrapper(type);
            var method = wrapType.GetMethod("Register", BindingFlags.Static | BindingFlags.Public);
            method?.Invoke(null, new object[] { classWrapper });           
        }

        public SharpClass RegisterModel(string name, Type[] types)
        {
            var model = _binder.GetModule(name);
            foreach (Type t in types)
            {
                model.RegClass(t);
            }
            return model;
        }

        public SharpClass RegisterClass<T>()
        {
            return _binder.RegClass<T>();
        }

        public SharpClass RegisterClass(Type classType)
        {
            return _binder.RegClass(classType);
        }

        public SharpClass RegisterClass(Type classType, Type superType)
        {
            return _binder.RegClass(classType, superType);
        }   

        #region lua debug functions

        public int SetDebugHook(LuaHookMask mask, int count)
        {
            if (_hookCallback == null)
            {
                _hookCallback = DebugHookCallback;
                lua_sethook(L, _hookCallback, mask, count);
            }

            return -1;
        }

        public void RemoveDebugHook()
        {
            _hookCallback = null;
            lua_sethook(L, null, LuaHookMask.Disabled, 0);
        }

        public LuaHookMask GetHookMask()
        {
            return (LuaHookMask)lua_gethookmask(L);
        }

        public int GetHookCount()
        {
            return lua_gethookcount(L);
        }

        public string GetLocal(LuaDebug luaDebug, int n)
        {
            return Lua.GetLocal(L, luaDebug, n);
        }

        public string SetLocal(LuaDebug luaDebug, int n)
        {            
            return Lua.SetLocal(L, luaDebug, n);
        }

        public int GetStack(int level, ref LuaDebug ar)
        {            
            return Lua.GetStack(L, level, ref ar);
        }

        public bool GetInfo(string what, ref LuaDebug ar)
        {
            return Lua.GetInfo(L, what, ref ar);
        }

        [AOT.MonoPInvokeCallback(typeof(LuaNativeFunction))]
        void DebugHookCallback(lua_State L, IntPtr luaDebug)
        {
            lua_getstack(L, 0, luaDebug);

            if (!Lua.GetInfo(L, "Snlu", luaDebug))
                return;

            var debug = LuaDebug.FromIntPtr(luaDebug);

            DebugHookCallbackInternal(debug);
        }

        private void DebugHookCallbackInternal(LuaDebug luaDebug)
        {
            try
            {
                var temp = DebugHook;

                if (temp != null)
                    temp(this, new DebugHookEventArgs(luaDebug));
            }
            catch (Exception ex)
            {
                OnHookException(new HookExceptionEventArgs(ex));
            }
        }

        private void OnHookException(HookExceptionEventArgs e)
        {
            var temp = HookException;
            if (temp != null)
                temp(this, e);
        }

#endregion


    }


    public class MethodWraper
    {
        public ref LuaNativeFunction func => ref funcs[0];
        public ref LuaNativeFunction setter => ref funcs[1];
        public ref LuaNativeFunction getter => ref funcs[2];
        //同一个名字的函数，最多支持8种不同参数类型
        public LuaNativeFunction[] funcs = new LuaNativeFunction[8];
    }

    public class ClassWraper : Dictionary<string, MethodWraper>
    {
        public void RegConstructor(LuaNativeFunction func) => RegFunction("ctor", func);

        public void RegField(string name, LuaNativeFunction getter, LuaNativeFunction setter = null) => RegProperty(name, getter, setter);

        public void RegProperty(string name, LuaNativeFunction getter, LuaNativeFunction setter = null)
        {
            if (!TryGetValue(name, out var methodWraper))
            {
                methodWraper = new MethodWraper();
                Add(name, methodWraper);
            }

            methodWraper.getter = getter;
            methodWraper.setter = setter;
        }

        public void RegFunction(string name, LuaNativeFunction func)
        {
            if (!TryGetValue(name, out var methodWraper))
            {
                methodWraper = new MethodWraper();
                Add(name, methodWraper);
            }

            methodWraper.func = func;
        }
    }
}
