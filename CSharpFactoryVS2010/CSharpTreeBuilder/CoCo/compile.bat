coco CSharp3.ATG -namespace CSharpTreeBuilder.CSharpAstBuilder -trace S > _Output.txt
copy trace.txt _Trace.txt
copy trace.txt ..\CSharpAstBuilder\SymbolTable.Generated.txt
copy Parser.cs ..\CSharpAstBuilder\CSharpParser.Generated.cs
copy Scanner.cs ..\CSharpAstBuilder\Scanner.Generated.cs
del Parser.cs
del Scanner.cs
pause
