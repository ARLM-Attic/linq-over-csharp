coco CSharp2.ATG -namespace CSharpParser.ParserFiles > _Output.txt
copy trace.txt _Trace.txt
copy Parser.cs ..\ParserFiles\Parser.cs
copy Scanner.cs ..\ParserFiles\Scanner.cs
del Parser.cs
del Scanner.cs
pause
