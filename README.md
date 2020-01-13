# Luna
基于Lua虚拟机的C#/Unity热更新系统，提供简洁简洁，高效的C# lua互访接口，可将C#类快速导出到Lua，支持Unity3D,.netcore和.NetFramework等各个平台

## 主要特点

1.高性能，没用使用LuaInterface那一套。使用Delegate进行反射，不用生成大量的Wrap代码，不使用Emit和Expression，也不用当心IOS平台的问题

2.简洁易用，使用接口非常简单，注册C#类到lua只需要一句话

```
luna.RegisterClass<TestClass>();

```

3.未对lua接口做修改，可以使用原汁原味的lua编程，可将C风格的lua操作代码直接拷贝的C#中，不用做修改就能编译通过并运行

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

4.对lua做了扩充，加入class和super关键字，可以优雅地进行面向对象编程

```

class Obj
	name = "test name"
	ID = 1

	function print()
		print(self.name)
	end

end

class GameObj : Obj

	function _init()
	end
	
	function setName(n)
		self.name = n
	end

	function print()
		print "test super"
		super:print()
    end
end

local o = GameObj
{
	name = "GameObj"
}

o:print()

```

