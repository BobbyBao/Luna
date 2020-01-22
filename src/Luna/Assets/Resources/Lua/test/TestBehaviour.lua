

class = require "pl.class"

class.TestBehaviour()

function TestBehaviour:Awake()
	print "Awake"
end

function TestBehaviour:Start()
	print "Start"
end

function TestBehaviour:Update()
	print "Update"
end

function TestBehaviour:OnDestroy()
	print "OnDestroy"
end

