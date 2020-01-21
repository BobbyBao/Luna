# Luna
基于Lua虚拟机的C#/Unity热更新系统，提供简洁简洁，高效的C# lua互访接口，可将C#类快速导出到Lua，支持Unity3D,.NetCore和.NetFramework等各个平台

## 主要特点

1.简洁高效的访问接口，没有使用LuaInterface/NLua那一套。开发时使用高性能的Delegate进行反射，发布的时候生产Wrap代码，进一步提高运行效率  
2.提供基于lua vm的luna脚本语言，同时也支持原生的lua，可根据个人喜好选择  
3.未对lua接口做修改，也可以使用原汁原味的lua编程，可将C风格的lua操作代码直接拷贝的C#中  

```

            lua_getmetatable(L, 1);
            lua_pushvalue(L, 2);
            lua_rawget(L, -2);

            if (lua_isnil(L, -1))
            {
                lua_pop(L, 1); 
                lua_pushliteral(L, "___getters");
                lua_rawget(L, -2);
                assert(lua_istable(L, -1));

                lua_pushvalue(L, 2);
                lua_rawget(L, -2);

                if (lua_iscfunction(L, -1))
                {
                    lua_call(L, 0, 1);
                }
            }

```
## Luna脚本语言




