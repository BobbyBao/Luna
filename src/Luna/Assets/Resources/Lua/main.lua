

print "hello"

test = TestClass()

test:MethodOverload()
test:MethodOverload(TestClass())
test:MethodOverload(1, 2, 3)

local a = test:MethodOverload(2, 3)

print(a)

--[[
    
itest={}
function itest:test5(x,y) 
    return x.testval+y.testval;
end

GameObject = luanet.import_type('UnityEngine.GameObject')
local go = GameObject()

a=test:callInterface5(itest)

print(a)
]]
