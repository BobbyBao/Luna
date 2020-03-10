using System;
using System.Linq;
using System.Reflection;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;
using System.Collections;
using UnityEngine;

namespace SharpLuna
{
    using static Lua;
    using lua_State = IntPtr;

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

        public static Action<string> Print { get; set; }
        public static Func<string, byte[]> ReadBytes { get; set; }

        public event Action PostInit;
        public event EventHandler<HookExceptionEventArgs> HookException;
        public event EventHandler<DebugHookEventArgs> DebugHook;
        private KyHookFunction _hookCallback;
        private List<ModuleInfo> _config = new List<ModuleInfo>();
        private SharpModule _binder;
        private readonly Dictionary<Type, ClassWraper> _classWrapers = new Dictionary<Type, ClassWraper>();

        private static List<Assembly> assemblies = new List<Assembly>();
        public static ModuleInfo systemModule = new ModuleInfo
        {
            typeof(object),
            typeof(Enum),
            new ClassInfo(typeof(string))
            {
                "Chars",
                "GetEnumerator"
            },

            typeof(Delegate),
            typeof(Array),

            typeof(byte[]),
            typeof(int[]),
            typeof(float[]),
            typeof(object[]),
        };

        public Luna(params ModuleInfo[] modules)
        {
            _config.Add(systemModule);

            foreach (var m in modules)
            {
                _config.Add(m);
            }
        }

        ~Luna()
        {
            Dispose();
        }

        public void AddModuleInfo(ModuleInfo moduleInfo)
        {
            _config.Add(moduleInfo);
        }

        public void Start()
        {
            if (ReadBytes == null)
            {
                ReadBytes = System.IO.File.ReadAllBytes;
            }

            L = Lua.newstate();
            mainState = L;

            Debug.Log(string.Format("newstate L{0:X000}", L));
           
            luaL_openlibs(L);
            lua_atpanic(L, PanicCallback);

            Register("print", DoPrint);
            Register("dofile", DoFile);
            Register("loadfile", LoadFile);

            errorFuncRef = get_error_func_ref(L);

            AddAssembly(Assembly.GetExecutingAssembly());

            _global = LuaRef.Globals(L);
            _global.Set("luna", LuaRef.CreateTable(L));

            Register("luna.typeof", GetClassType);
            Register("luna.findType", FindClassType);

#if LUNA_SCRIPT
            DoString(coroutineSource);
            DoString(classSource);
            DoString(listSource);
#endif

            _binder = new SharpModule(this);

            AddSearcher(LuaLoader);

            SharpClass.Init(L);
            SharpObject.Init(L);

            RegisterWraps(this.GetType());

            foreach (var moduleInfo in this._config)
            {
                this.RegisterModel(moduleInfo);
            }

            PostInit?.Invoke();

            var it = _classWrapers.GetEnumerator();
            while (it.MoveNext())
            {
                if (!SharpModule.IsRegistered(it.Current.Key))
                {
                    RegisterClass(it.Current.Key);
                }
            }

            _classWrapers.Clear();


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

            _global.Dispose();

            L.unrefall();
            Lua.closestate(L);
            L = IntPtr.Zero;
        }

        public static bool LoadAssembly(string name)
        {
            for (int i = 0; i < assemblies.Count; i++)
            {
                if (assemblies[i].GetName().Name == name)
                {
                    return true;
                }
            }

            Assembly assembly = Assembly.Load(name);
            if (assembly == null)
            {
                assembly = Assembly.Load(AssemblyName.GetAssemblyName(name));
            }

            if (assembly != null && !assemblies.Contains(assembly))
            {
                assemblies.Add(assembly);
            }

            return assembly != null;
        }

        public static void AddAssembly(Assembly ass)
        {
            if(!assemblies.Contains(ass))
                assemblies.Add(ass);
        }

        public void Register(string name, LuaNativeFunction function)
        {
            savedFn.TryAdd(function);
            lua_pushcfunction(L, function);
            SetGlobal(L, name);
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

            if (Print != null)
                Print(sb.ToString());
            else
                Debug.Log(sb.ToString());

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
        static int GetClassType(IntPtr L)
        {
            lua_pushvalue(L, -1);
            lua_pushliteral(L, "type");
            lua_rawget(L, -2);
            return 1;
        }

        [AOT.MonoPInvokeCallback(typeof(LuaNativeFunction))]
        static int FindClassType(IntPtr L)
        {
            Get(L, 1, out string typeName);
            Type t = null;
            foreach(var ass in assemblies)
            {
                t = ass.GetType(typeName);
                if(t != null)
                {
                    break;
                }
            }
            Push(L, t);
            return 1;
        }

        [AOT.MonoPInvokeCallback(typeof(LuaNativeFunction))]
        static int PanicCallback(lua_State L)
        {
            string reason = string.Format("Unprotected error in call to Lua API ({0})", lua_tostring(L, -1));
            throw new LuaException(reason);
        }

        public object[] DoString(byte[] chunk, string chunkName = "chunk")
        {
            int oldTop = lua_gettop(L);
            _executing = true;

            if (luaL_loadbuffer(L, chunk, chunkName) != LuaStatus.OK)
                ThrowExceptionFromError(L, oldTop);

            int errorFunctionIndex = 0;

            if (UseTraceback)
            {
                errorFunctionIndex = PushDebugTraceback(L, 0);
                oldTop++;
            }

            try
            {
                if (lua_pcall(L, 0, -1, errorFunctionIndex) != LuaStatus.OK)
                    ThrowExceptionFromError(L, oldTop);

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
                ThrowExceptionFromError(L, oldTop);

            int errorFunctionIndex = 0;

            if (UseTraceback)
            {
                errorFunctionIndex = PushDebugTraceback(L, 0);
                oldTop++;
            }

            try
            {
                if (lua_pcall(L, 0, -1, errorFunctionIndex) != LuaStatus.OK)
                    ThrowExceptionFromError(L, oldTop);

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
                ThrowExceptionFromError(L, oldTop);

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
                    ThrowExceptionFromError(L, oldTop);

                return PopValues(L, oldTop);
            }/*
            catch(Exception e)
            {
                Debug.LogError(e.Message);
                return null;
            }*/
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

        public bool IsWrapered(Type type)
        {
            return _classWrapers.ContainsKey(type);
        }

        public void RegisterWraps(Type type)
        {
            var types = type.Assembly.GetTypes();
            foreach (var t in types)
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
            if (IsWrapered(type))
            {
                return;
            }

            var classWrapper = GetClassWrapper(type);
            var method = wrapType.GetMethod("Register", BindingFlags.Static | BindingFlags.Public);
            method?.Invoke(null, new object[] { classWrapper });
        }

        public SharpClass RegisterModel(ModuleInfo moduleInfo)
        {
            var model = string.IsNullOrEmpty(moduleInfo.Name) ? _binder : SharpModule.Get(_binder, moduleInfo.Name);
            foreach (var t in moduleInfo)
            {
                model.RegClass(t.type);
            }
            return model;
        }

        public SharpClass RegisterModel(string name, IEnumerable<Type> types)
        {
            var model = string.IsNullOrEmpty(name) ? _binder : SharpModule.Get(_binder, name);
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

#if LUNA_SCRIPT

        const string coroutineSource = @"
let unpack = unpack or table.unpack

func coroutine.call(fn, self){
    return func(...){ 
        let co = coroutine.create(fn)
        if self {
            coroutine.resume(co, self, ...)
        } else {
            coroutine.resume(co, ...)
        }
    }
}

func coroutine.__async(async_func, callback_pos) {
    return func(...) {
        var _co = coroutine.running() or error ('this function must be run in coroutine')
        var rets
        var waiting = false
        local func cb_func(...) {
            if waiting {
                coroutine.resume(_co, ...)
            } else {
                rets = {...}
            }
        }
        var params = {...}
        table.insert(params, callback_pos or (#params + 1), cb_func)
        async_func(unpack(params))
        if rets == nil {
            waiting = true
            rets = {coroutine.yield()}
        }
        
        return unpack(rets)
    }
}

__async = coroutine.__async

func __def_async(t, fnName) {
    t = t or _G
    t[""_async_""..fnName] = __async(t[fnName])
}

";


        const string classSource = @"
local func is_a(self,klass) {
    if klass == nil {
        return getmetatable(self)
    }
    
    var m = getmetatable(self)
    if not m { return false }
    while m {
        if m == klass { return true }
        m = rawget(m,'_base')
    }
    return false
}

local func class_of(klass,obj) {
    if type(klass) != 'table' or not rawget(klass,'is_a') { return false }
    return klass.is_a(obj,klass)
}

local func cast (klass, obj) {
    return setmetatable(obj,klass)
}

local func copyProperties(td, ts) {
    for k,v in pairs(ts) {
        if td[k] == nil {
            td[k] = v
        }
    }
}

func __class(c, className, base) {
    var mt = {}
    c = c or {}

    if type(base) == 'table' {
		copyProperties(c, base)
        c._base = base
    } elseif base != nil {
        error(""must derive from a table type"", 3)
    }

    c.__index = c
    setmetatable(c, mt)

    c._class = c
    c.name = className
    c.is_a = is_a
    c.class_of = class_of

    if c.__class_init {
        c.__class_init(mt)
        return c
    }

    mt.__call = func(cls_table, ...)
    {
        var obj = {}
        if c.init {
            setmetatable(obj, c)
            obj.init(...)
        }
        else
        {
            var args = { ...}
            if #args == 1 and type(args[1]) == 'table' {
                obj = args[1]
            }

            setmetatable(obj, c)
        }

        return obj
    }

    mt.__close = func()
    {
    }

    mt.__tostring = func(self)
    {
        let mt = self._class
        let name = mt.name
        setmetatable(self, nil)
        var str = tostring(self)
        setmetatable(self, mt)

        if name { str = name..str.gsub('table', '') }
        return str
    }

    return c
}

let type = type
let types = {}
let _typeof = luna.typeof
let _findtype = luna.findType

func typeof(obj) {
	let t = type(obj)
	var ret = nil
	
	if t == ""table"" {
		ret = types[obj]		
		if ret == nil {
            ret = _typeof(obj)
            types[obj] = ret
        }
        } else if t == ""string"" {
        ret = types[obj]
  		if ret == nil {
            ret = _findtype(obj)
            types[obj] = ret
        }
    } else {
            error(debug.traceback(""attemp to call typeof on type ""..t))
    }
	
    return ret
}

";


        const string listSource = @"
class List {
	var len_ = 0
	
	init(n) {
		self.len_ = n
	}

	func count() {
		return self.len_
	}

	func #() {
		return self.len_
	}

    func isEmpty() {
        return self.len_ == 0
    }

	func push(i) {
		self[self.len_] = i
		self.len_ = self.len_ + 1
	}

	func append(i) {
		self.push(i)
		return self
	}

	func insert(i, x) {
		var idx = self.len_
		while i <= idx {

			if idx == i {				
				self[i] = x
				self.len_ = self.len_ + 1
				break
			}

			self[idx] = self[idx - 1]
			idx = idx - 1
		} 
		
		return self
	}

	func removeAt (idx) {
		
		repeat {
			self[idx] = self[idx + 1]
			idx = idx + 1
		} until(idx == self.len_ - 1)

		table.remove(self, self.len_ - 1)
		self.len_ = self.len_ - 1
		return self
	}

	func remove (item) {
		let idx = self.indexOf(item)
        print(idx)
        if idx != -1 {
            self.removeAt(idx)
        }
	}

    func fastRemove(item) {
		let idx = self.indexOf(item)
        if idx != -1 {
		    self[idx] = self[self.len_-1]
            table.remove(self, self.len_ - 1)
		    self.len_ = self.len_ - 1
        }
    }
    
    func contains(item) {
        return self.indexOf(item) != -1
    }       
    
    func indexOf(item) {
        for i = 0, self.len_-1 {
            var it = self[idx]
            if it == item {
                return i
            }
        }
        return -1
    }

	func clear() {
		while self.len_ > 0 {
			self.len_ = self.len_ - 1
			table.remove(self, self.len_)
		} 
	}

	func iter() {
		var k = -1
		return func (t) {
			k = k + 1
			if k < t.len_ {
				return t[k]
			}
			
		}, self
	}
	
}

func __array(c, len) {
	setmetatable(c, List)
	c.len_ = len
}
";

#endif
    }

    public class ModuleInfo : IEnumerable<ClassInfo>
    {
        public string Name { get; }

        List<ClassInfo> classes = new List<ClassInfo>();

        public ModuleInfo(string name = "")
        {
            Name = name;
        }

        public IEnumerator<ClassInfo> GetEnumerator()
        {
            return classes.GetEnumerator();
        }

        public void Add(object obj)
        {
            if (obj is ClassInfo cls)
            {
                classes.Add(cls);
            }
            else if(obj is Type t)
            {
                classes.Add(new ClassInfo(t));
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable<ClassInfo>)classes).GetEnumerator();
        }
    }

    public class ClassInfo : System.Collections.IEnumerable
    {
        public Type type;
        public bool generateSuperMembers = false;
        public string Alias;
        public List<string> excludeMembers;

        public ClassInfo(Type type, bool genSuperMembers = false, string alias = "")
        {
            this.type = type;
            generateSuperMembers = genSuperMembers;
            this.Alias = alias;
        }

        public void Add(string exclude)
        {
            if(excludeMembers == null)
            {
                excludeMembers = new List<string>();
            }
             
            excludeMembers.Add(exclude);
        }

        public IEnumerator GetEnumerator()
        {
            return excludeMembers.GetEnumerator();
        }
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
