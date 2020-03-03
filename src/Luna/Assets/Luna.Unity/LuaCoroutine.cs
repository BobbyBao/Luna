using UnityEngine;
using System;
using SharpLuna;
using System.Collections.Generic;
using static SharpLuna.Lua;
using System.Collections;

public static class LuaCoroutine
{
    static MonoBehaviour mb = null;

    static string strCo =
        @"
        let _WaitForSeconds, _WaitForFixedUpdate, _WaitForEndOfFrame, _Yield, _StopCoroutine = WaitForSeconds, WaitForFixedUpdate, WaitForEndOfFrame, Yield, StopCoroutine        
        let error = error
        let debug = debug
        let coroutine = coroutine
        let comap = {}
        setmetatable(comap, {__mode = 'k'})

        func _resume(co) {
            if comap[co] {
                comap[co] = nil
                var flag, msg = coroutine.resume(co)
                    
                if not flag {
                    msg = debug.traceback(co, msg)
                    error(msg)
                }
            }        
        }

        func WaitForSeconds(t) {
            let co = coroutine.running()
            let resume = func() {                  
                _resume(co)                     
            }
            
            comap[co] = _WaitForSeconds(t, resume)
            return coroutine.yield()
        }

        func WaitForFixedUpdate() {
            let co = coroutine.running()
            let resume = func() {         
                _resume(co)     
            }
        
            comap[co] = _WaitForFixedUpdate(resume)
            return coroutine.yield()
        }

        func WaitForEndOfFrame() {
            let co = coroutine.running()
            let resume = func() {        
                _resume(co)     
            }
        
            comap[co] = _WaitForEndOfFrame(resume)
            return coroutine.yield()
        }

        func Yield(o) {
            let co = coroutine.running()
            let resume = func() {       
                _resume(co)     
            }
        
            comap[co] = _Yield(o, resume)
            return coroutine.yield()
        }

        func StartCoroutine(fn) {
            let co = coroutine.create(fn)                       
            var flag, msg = coroutine.resume(co)

            if not flag {
                msg = debug.traceback(co, msg)
                error(msg)
            }

            return co
        }

        func StopCoroutine(co) {
            let _co = comap[co]

            if _co == nil {
                return
            }

            comap[co] = nil
            _StopCoroutine(_co)
        }
        ";

    public static void Register(Luna luna, MonoBehaviour behaviour)
    {
        luna.Register("WaitForSeconds", _WaitForSeconds);
        luna.Register("WaitForFixedUpdate", WaitForFixedUpdate);
        luna.Register("WaitForEndOfFrame", WaitForEndOfFrame);
        luna.Register("Yield", Yield);
        luna.Register("StopCoroutine", StopCoroutine);

        luna.DoString(strCo, "@LuaCoroutine.cs");
        mb = behaviour;
    }
    
    [AOT.MonoPInvokeCallbackAttribute(typeof(LuaNativeFunction))]
    static int _WaitForSeconds(IntPtr L)
    {
        try
        {
            float sec = (float)luaL_checknumber(L, 1);
            Get(L, 2, out LuaRef func);
            Coroutine co = mb.StartCoroutine(CoWaitForSeconds(sec, func));
            Push(L, co);
            return 1;
        }
        catch (Exception e)
        {
            return luaL_error(L, e.Message);
        }
    }

    static IEnumerator CoWaitForSeconds(float sec, LuaRef func)
    {
        yield return new WaitForSeconds(sec);
        func.Call();
    }

    [AOT.MonoPInvokeCallbackAttribute(typeof(LuaNativeFunction))]
    static int WaitForFixedUpdate(IntPtr L)
    {
        try
        {
            Get(L, 1, out LuaRef func);
            Coroutine co = mb.StartCoroutine(CoWaitForFixedUpdate(func));
            Push(L, co);
            return 1;
        }
        catch (Exception e)
        {
            return luaL_error(L, e.Message);
        }
    }

    static IEnumerator CoWaitForFixedUpdate(LuaRef func)
    {
        yield return new WaitForFixedUpdate();
        if(func)
            func.Call();
    }

    [AOT.MonoPInvokeCallbackAttribute(typeof(LuaNativeFunction))]
    static int WaitForEndOfFrame(IntPtr L)
    {
        try
        {
            Get(L, 1, out LuaRef func);
            Coroutine co = mb.StartCoroutine(CoWaitForEndOfFrame(func));
            Push(L, co);
            return 1;
        }
        catch (Exception e)
        {
            return luaL_error(L, e.Message);
        }
    }

    static IEnumerator CoWaitForEndOfFrame(LuaRef func)
    {
        yield return new WaitForEndOfFrame();
        func.Call();
    }

    [AOT.MonoPInvokeCallbackAttribute(typeof(LuaNativeFunction))]
    static int Yield(IntPtr L)
    {
        try
        {
            object o = SharpObject.Get(L, 1);
            Get(L, 2, out LuaRef func);
            Coroutine co = mb.StartCoroutine(CoYield(o, func));
            Push(L, co);
            return 1;
        }
        catch (Exception e)
        {
            return luaL_error(L, e.Message);
        }
    }

    static IEnumerator CoYield(object o, LuaRef func)
    {
        if (o is IEnumerator)
        {
            yield return mb.StartCoroutine((IEnumerator)o);
        }
        else
        {
            yield return o;
        }

        func.Call();
    }

    [AOT.MonoPInvokeCallbackAttribute(typeof(LuaNativeFunction))]
    static int StopCoroutine(IntPtr L)
    {
        try
        {
            Get(L, 1, out Coroutine co);
            mb.StopCoroutine(co);
            return 0;
        }
        catch (Exception e)
        {
            return luaL_error(L, e.Message);
        }
    }
}

