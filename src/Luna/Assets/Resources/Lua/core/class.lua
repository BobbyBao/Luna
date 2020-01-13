
local function copyProperties(td, ts)
    for k,v in pairs(ts) do
        if td[k] == nil then
            td[k] = v
        end
    end
end

function __class(c, base)
    local mt = {}
    c = c or {}

    if type(base) == 'table' then
		copyProperties(c, base)
        c._base = base
    elseif base ~= nil then
        error("must derive from a table type",3)
    end

    c.__index = c
    setmetatable(c,mt)

    c._class = c

    mt.__call = function(class_tbl,...)
        local obj = {}

        if c._init then
            setmetatable(obj, c)
            obj:_init(...)
        else
            local args = {...}

            if #args == 1 and type(args[1]) == 'table' then
                obj = args[1]
            end

            setmetatable(obj, c)
        end

        return obj
    end


    return c
end
