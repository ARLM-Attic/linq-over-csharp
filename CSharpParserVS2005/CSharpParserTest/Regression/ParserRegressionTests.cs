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
    public void TypeResolutionCounterIsOk4()
    {
      CompilationUnit parser = new CompilationUnit(CSharpParserFolder);
      TypeReference.Locations.Clear();
      TypeReference.ResolutionCounter = 0;
      parser.AddFile(@"Utility\StringHelper.cs");
      Assert.IsTrue(InvokeParser(parser));
      Console.WriteLine("Type references: {0}", TypeReference.Locations.Count);
      Console.WriteLine("Type resolutions: {0}", TypeReference.ResolutionCounter);
      Assert.AreEqual(TypeReference.ResolutionCounter, TypeReference.Locations.Count);
    }

    [TestMethod]
    public void TypeResolutionCounterIsOk5()
    {
      CompilationUnit parser = new CompilationUnit(CSharpParserFolder);
      TypeReference.Locations.Clear();
      TypeReference.ResolutionCounter = 0;
      parser.AddFile(@"Properties\Resources.Designer.cs");
      Assert.IsTrue(InvokeParser(parser));
      Console.WriteLine("Type references: {0}", TypeReference.Locations.Count);
      Console.WriteLine("Type resolutions: {0}", TypeReference.ResolutionCounter);
      Assert.AreEqual(TypeReference.ResolutionCounter, TypeReference.Locations.Count);
    }
  }
}