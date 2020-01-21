# Luna
基于Lua虚拟机的C#/Unity热更新系统，提供简洁，高效的C# lua互访接口，可将C#类快速导出到Lua，支持Unity3D,.NetCore和.NetFramework等各个平台

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

面向对象的脚本语言，类似Swift的语法，支持class和继承 zero based数组

```
import "class"


class GameObj {
	var name = ""
	var id = 1

	init(n) {
		self.name = n
		print("GameObj name:", self.name)
	}

	func testFunc(a) {
		print("GameObj testFunc", a)
	}

}

class Character : GameObj {
		
	init(n) {
		super.init(n)
	}

	func testFunc(a) {
		super.testFunc(a)
		print("Character testFunc", a)
	}

}

```
支持let关键字，成员函数调用，和静态方法调用统一用".",不再有":"和"."选择的问题

```
let c = Character("test name")
c.testFunc("test arg")


```

0 based 数组

```
var a = [1, 2, 3, 4, 5]

print "iter():"

for v in a.iter() {
	print(v)
}

print "range:"

for i = 0, 4 {
	print(i, a[i])
}
```

由于采用了lua的虚拟机，执行效率上和lua一样，没有变化
