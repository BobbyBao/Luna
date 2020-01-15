
local Vector3 = Vector3
local Quaternion = Quaternion
local Normalize = Vector3.Normalize
--local verbo = require("jit.v")
--verbo.start()

function Test1(transform)	
	local one = Vector3.one
	local t = os.clock()
	
	for i = 1,200000 do
		transform.position = transform.position
	end
	
	t = os.clock() - t
	print("Transform.position lua cost time: ", t)	
end

function Test2(transform)		
	local up = Vector3.up
	local t = os.clock()

	for i = 1,200000 do
		transform:Rotate(up, 1)	
	end
	
	t = os.clock() - t
	print("Transform.Rotate lua cost time: ", t)	
end

function Test3()		
	local t = os.clock()
	local New = Vector3.__call
	
	for i = 1, 200000 do
		local v = New(i, i, i)		
	end
		
	t = os.clock() - t
	print("Vector3.New lua cost time: ", t)	
end

--会存在大量gc操作
function Test4()	
	local GameObject = UnityEngine.GameObject
	local t = os.clock()	
	local go = GameObject()
	local node = go.transform

	for i = 1,100000 do				
		--GameObject.New()
		go = node.gameObject
	end
	
	t = os.clock() - t
	print("GameObject.New lua cost time: ", t)	
end

--[[
function Test5()		
	local t = os.clock()
	local GameObject = UnityEngine.GameObject
	local SkinnedMeshRenderer = UnityEngine.SkinnedMeshRenderer
	local tp = typeof(SkinnedMeshRenderer)

	for i = 1,20000 do				
		local go = GameObject.New()
		go:AddComponent(tp)
    	local c = go:GetComponent(tp)
    	c.castShadows=false
    	c.receiveShadows=false
	end
		
	print("Test5 lua cost time: ", os.clock() - t)	
end

function Test6()	
	local t = os.clock()
	
	for i = 1,200000 do		
		local t = Input.GetTouch(0)		
		local p = Input.mousePosition
		--Physics.RayCast
	end
		
	print("lua cost time: ", os.clock() - t)	
end
]]

function Test7()		
	local Vector3 = Vector3	
	local t = os.clock()
		
	for i = 1, 200000 do
		local v = Vector3(i,i,i)
		Vector3.Normalize(v)
	end
		
	print("lua Vector3 New Normalize cost time: ", os.clock() - t)	
end

function Test8()		
	local Quaternion = Quaternion
	local t = os.clock()
	
	for i=1,200000 do
		local q1 = Quaternion.Euler(i, i, i)		
		local q2 = Quaternion.Euler(i * 2, i * 2, i * 2)
		Quaternion.Slerp(Quaternion.identity, q1, 0.5)		
	end
		
	print("Quaternion Euler Slerp const: ", os.clock() - t)		
end


function fib(n)
  if n < 2 then return n end
  return fib(n - 2) + fib(n - 1)
end

local start = os.clock()
for i = 1, 5 do
  print(fib(28))
end
print(string.format("elapsed: %.8f", os.clock() - start))

function Test9()	
	local total = 0
	local t = os.clock()

	for i = 0, 1000000 do
		total = total + i - (i/2) * (i + 3) / (i + 5)
	end

	print("math cal cost: ", os.clock() - t)		
end

function TestTable()	
	local array = {}

	for i = 1, 1024 do
		array[i] = i
	end

	local total = 0
	local t = os.clock()
		
	for j = 1, 100000 do
		for i = 1, 1024 do
			total = total + array[i]
		end			
	end
		
	print("Array cost time: ", os.clock() - t)	
end

Test3()
Test4()
Test7()
--Test8()

Test9()

TestTable()
