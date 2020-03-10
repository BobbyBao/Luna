# Luna
基于Lua虚拟机的面向对象脚本语言，支持C#/Unity热更新，提供简洁，高效的C# lua互访接口，可将C#类快速导出给Lua VM调用，支持Unity3D,.NetCore和.NetFramework等各个平台

## 主要特点

1.简洁高效的访问接口，开发时使用高性能的Delegate进行反射，发布的时候生成Wrap代码，进一步提高运行效率  
2.提供基于lua vm的luna脚本语言，同时也支持原生的lua，可根据个人喜好选择  
3.未对lua接口做修改，可将C风格的lua操作代码直接拷贝的C#中  

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

具有Modern语法风格的面向对象脚本语言，类似Swift，支持class，继承以及 zero based数组，既能享受lua脚本的效率，又可以优雅的进行面向对象编程

```

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
支持var, let关键字，成员函数调用和静态方法调用统一用".",不再有":"和"."选择的问题

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
去掉do..end,用C风格的{}代替, if else 语句更加自由

```
func test() {
    var i = 3

    while not i {
        if true {
            break
        }
        
    }

    for a = 1, 8 {

        if a == 3 {
            print "111"
        }
    
        elseif a == 4 {
            print "1111"
            if a == 4 {
            }
        }
        else if a == 5 {

            print "11111"
        } 
        else
        if a == 6 {

            print "111111"
        } else {
            print "11111111"
        }
    }
}

```

操作符的风格也做了相应的改变，增加了复合赋值操作符，注释风格也向C系靠拢

```
var x = 1

//+=
x += 2
print(x)

//-=
x -= 2
print(x)

//*=
x*= 2
print(x)

// /=
x /= 2

print(x)

/*
逻辑操作符
*/

let i = 0
let j = 1

let t = nil

if !i {
    print "not"
} else {
    print(i)
}

if !t || !t.aaa {
    print("not", t)
}

if i != 1 {
    print "not eq"
}

let i = false

if !i {
    print("not", i)
}

if i && j {
    print("and", i, j)
}

if i || j {
    print("or", i, j)
}

```

更加直观的操作符重载

```
class vec3 {
    var x = 0, y = 0, z = 0

    init(x, y, z) {
        self.x = x
        self.y = y
        self.z = z
    }

    func + (v1, v2) {
        return vec3(v1.x + v2.x, v1.y + v2.y, v1.z + v2.z)
    }

    func - (v1, v2) {
        return vec3(v1.x - v2.x, v1.y - v2.y, v1.z - v2.z)
    }

    func * (v1, v2) {
        return vec3(v1.x * v2, v1.y * v2, v1.z * v2)
    }

    func / (v1, v2) {
        return vec3(v1.x / v2, v1.y / v2, v1.z / v2)
    }

    func == (v1, v2) {
        return v1.x == v2.x && v1.y == v2.y && v1.z == v2.z
    }

    func < (v1, v2) {
        return v1.x < v2.x && v1.y < v2.y && v1.z < v2.z
    }

    func <= (v1, v2) {
        return v1.x <= v2.x && v1.y <= v2.y && v1.z <= v2.z
    }
}

```
支持类似C#的async/await 语法，利用了lua的coroutine机制，可以像写同步代码一样写异步代码，以充值的流程为例

```

class Recharge {

    async func recharge(num, cb) {
        print('requst server...')
        cb(true, num)
    }

    func buy() {

        print("Recharge buy : ", self)

        await UIManager.ShowAlertBox("您余额不足，请充值！", "余额提醒")
        if await UIManager.ShowConfirmBox("确认充值10元吗？", "确认框") {
            print('recharging...')
            let r1, r2 = await self.recharge(10)
            print('recharge result:', r1, r2)
            await UIManager.ShowAlertBox("充值成功！", "提示")
        } else {
            print('cancel')
            await UIManager.ShowAlertBox("取消充值！", "提示")
        }

        print('recharge finished')
    }
}


```

