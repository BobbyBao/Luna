

require "list"

local a = [1, 2, 3, 4, 5]

print "iter():"

for v in a:iter() do
	print(v)
end

print "range:"

for i = 0, #a-1 do
	print(i, a[i])
end

local a = List()

print "append:"
a:append(2)
:append(3)
:append(4)
:append(5)
:append(6)


	
print("#len:", #a)

for k,v in pairs(a) do
	print(k, v)
end

print "remove:"
a:remove(2)

for v in a:iter() do
	print(v)
end

print "insert:"
a:insert(2, 4)

for v in a:iter() do
	print(v)
end