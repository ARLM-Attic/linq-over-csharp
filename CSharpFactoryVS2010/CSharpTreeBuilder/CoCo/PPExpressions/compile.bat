..\coco PPExpr.ATG -namespace CSharpTreeBuilder.CSharpAstBuilder.PPExpressions > _Output.txt
copy Parser.cs ..\..\CSharpAstBuilder\PPExpressions\Parser.cs
copy Scanner.cs ..\..\CSharpAstBuilder\PPExpressions\Scanner.cs
del Parser.cs
del Scanner.cs
pause
