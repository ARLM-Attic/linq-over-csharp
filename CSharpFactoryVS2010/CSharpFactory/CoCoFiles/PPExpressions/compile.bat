..\coco PPExpr.ATG -namespace CSharpParser.ParserFiles.PPExpressions > _Output.txt
copy Parser.cs ..\..\ParserFiles\PPExpressions\Parser.cs
copy Scanner.cs ..\..\ParserFiles\PPExpressions\Scanner.cs
del Parser.cs
del Scanner.cs
pause
