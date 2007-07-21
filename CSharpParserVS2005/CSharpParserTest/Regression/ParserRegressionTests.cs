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
    }
  }
}