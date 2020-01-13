
require "class"



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
