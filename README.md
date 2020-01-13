# Luna
基于Lua虚拟机的C#/Unity热更新系统，提供简洁简洁，高效的C# lua互访接口，可将C#类方便的导出到Lua，支持Unity3D,.netcore和.NetFramework等各个平台

## 主要特点

1.使用Delegate进行反射，不使用Emit和Expression
不用生成大量的Wrap代码，而又能保证高性能，还能在各个平台上使用

2.使用接口非常简单，注册C#类到lua只需要一句话

```
luna.RegisterClass<TestClass>();
```
3. 未对lua接口做修改，可以使用原汁原味的lua编程
```

            // push metatable of table -> <mt>
            lua_getmetatable(L, 1);

            // push metatable[key] -> <mt> <mt[key]>
            lua_pushvalue(L, 2);
            lua_rawget(L, -2);

            if (lua_isnil(L, -1))
            {
                // get metatable.getters -> <mt> <getters>
                lua_pop(L, 1);          // pop nil
                lua_pushliteral(L, "___getters");
                lua_rawget(L, -2);      // get getters table
                assert(lua_istable(L, -1));

                // get metatable.getters[key] -> <mt> <getters> <getters[key]>
                lua_pushvalue(L, 2);    // push key
                lua_rawget(L, -2);      // lookup key in getters

                if (lua_iscfunction(L, -1))
                {
                    // getter function found
                    lua_call(L, 0, 1);
                }
            }

```
