using System;
using CSharpFactory.ProjectModel;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CSharpParserTest.LanguageElements
{
  [TestClass]
  public class ParserRegressionTests : ParserTestBed
  {
    const string CSharpParserFolder = @"C:\Work\LINQOverCSharp\CSharpFactoryVS2010\CSharpFactory";

    [TestMethod]
    public void TypeResolutionCounterIsOk4()
    {
      CompilationUnit parser = new CompilationUnit(CSharpParserFolder);
      parser.AddFile(@"Utility\StringHelper.cs");
      Assert.IsTrue(InvokeParser(parser));
      Console.WriteLine("Type references: {0}", parser.Locations.Count);
      Console.WriteLine("Type resolutions: {0}", parser.ResolutionCounter);
      Assert.AreEqual(parser.ResolutionCounter, parser.Locations.Count);
    }

    [TestMethod]
    public void TypeResolutionCounterIsOk5()
    {
      CompilationUnit parser = new CompilationUnit(CSharpParserFolder);
      parser.AddFile(@"Properties\Resources.Designer.cs");
      Assert.IsTrue(InvokeParser(parser));
      Console.WriteLine("Type references: {0}", parser.Locations.Count);
      Console.WriteLine("Type resolutions: {0}", parser.ResolutionCounter);
      Assert.AreEqual(parser.ResolutionCounter, parser.Locations.Count);
    }
  }
}