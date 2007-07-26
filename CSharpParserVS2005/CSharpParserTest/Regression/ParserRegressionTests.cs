using System;
using CSharpParser.ProjectModel;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CSharpParserTest.LanguageElements
{
  [TestClass]
  public class ParserRegressionTests : ParserTestBed
  {
    const string CSharpParserFolder = @"C:\Work\LINQOverCSharp\CSharpParserVS2005\CSharpParser";

    [TestMethod]
    public void Scanner_cs_ErrorFixed()
    {
      CompilationUnit parser = new CompilationUnit(CSharpParserFolder);
      parser.AddFile(@"ParserFiles\Scanner.cs");
      Assert.IsTrue(InvokeParser(parser));
    }

    [TestMethod]
    public void RestricterCollection_cs_ErrorFixed()
    {
      CompilationUnit parser = new CompilationUnit(CSharpParserFolder);
      parser.AddFile(@"Utility\RestrictedCollection.cs");
      bool result = InvokeParser(parser);
      Assert.IsTrue(result);
      Console.WriteLine('\u00a0');
      Console.WriteLine('\u00aa');
      Console.WriteLine('\u00b5');
      Console.WriteLine('\u00ba');
      Console.WriteLine('\u00c0');
      Console.WriteLine('\u00d6');
      Console.WriteLine('\u00d8');
      Console.WriteLine('\u00f6');
      Console.WriteLine('\u00f8');
      Console.WriteLine('\u00ff');
    }
  }
}