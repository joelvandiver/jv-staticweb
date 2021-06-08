DECLARE @Data TABLE ([value] nvarchar(max))
    
INSERT INTO @Data ([value]) 
VALUES 
    ('Item #1')
    , ('Item #2')
    , ('Item #3')
    , ('Item #4')
    
SELECT CONCAT('[', STRING_AGG(CONCAT('"', [value], '"'), ','), ']')
FROM @Data
