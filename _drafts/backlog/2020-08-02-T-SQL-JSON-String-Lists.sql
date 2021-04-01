DECLARE @Data TABLE ([value] nvarchar(max))
    
INSERT INTO @Data ([value]) VALUES ('Item #1')
INSERT INTO @Data ([value]) VALUES ('Item #2')
INSERT INTO @Data ([value]) VALUES ('Item #3')
INSERT INTO @Data ([value]) VALUES ('Item #4')
    
SELECT CONCAT('[', STRING_AGG(CONCAT('"', [value], '"'), ','), ']')
FROM @Data