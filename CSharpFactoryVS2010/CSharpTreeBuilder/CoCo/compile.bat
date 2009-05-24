coco CSharp3.ATG -namespace CSharpTreeBuilder.CSharpAstBuilder > _Output.txt
copy trace.txt _Trace.txt
copy Parser.cs ..\CSharpAstBuilder\CSharpParser.Generated.cs
copy Scanner.cs ..\CSharpAstBuilder\Scanner.Generated.cs
del Parser.cs
del Scanner.cs
pause
