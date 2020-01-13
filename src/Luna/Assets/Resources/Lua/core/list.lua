
require "class"

class List
	_len = 0
	
	function _init(n)
		self._len = n
	end

	function len()
		return self._len
	end

	function __len()
		return self._len
	end

	function push(i)
		self[self._len] = i
		self._len = self._len + 1
	end

	function append(i)
		self:push(i)
		return self
	end

	function insert(i, x)
		local idx = self._len
		while i <= idx do

			if idx == i then				
				self[i] = x
				self._len = self._len + 1
				break
			end

			self[idx] = self[idx - 1]
			idx = idx - 1
		end
		
		return self
	end

	function remove (idx)
		
		repeat
			self[idx] = self[idx + 1]
			idx = idx + 1
		until(idx == self._len - 1)

		table.remove(self, self._len - 1)
		self._len = self._len - 1
		return self
	end

	function clear()

		while self._len > 0 do
			self._len = self._len - 1
			table.remove(self, self._len)
		end 

	end

	function iter()
		local k = -1
		return function (t)
			k = k + 1
			if k < t._len then
				return t[k]
			end
			
		end, self
	end
	
	
end

function __array(c, len)
	setmetatable(c, List)
	c._len = len
end