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
    public void RestrictedCollection_cs_ErrorFixed()
    {
      CompilationUnit parser = new CompilationUnit(CSharpParserFolder);
      parser.AddFile(@"Utility\RestrictedCollection.cs");
      parser.AddFile(@"Utility\IReadOnlySupport.cs");
      parser.AddFile(@"Utility\EventArguments.cs");
      bool result = InvokeParser(parser);
      Assert.IsTrue(result);
    }

    [TestMethod]
    public void TypeResolutionCounterIsOk1()
    {
      CompilationUnit parser = new CompilationUnit(CSharpParserFolder);
      TypeReference.Locations.Clear();
      TypeReference.ResolutionCounter = 0;
      parser.AddFile(@"ParserFiles\CommentHandler.cs");
      parser.AddFile(@"ParserFiles\CSharpErrorHandling.cs");
      parser.AddAssemblyReference("CSharpParser", AsmFolder);
      Assert.IsTrue(InvokeParser(parser));
      Console.WriteLine("Type references: {0}", TypeReference.Locations.Count);
      Console.WriteLine("Type resolutions: {0}", TypeReference.ResolutionCounter);
      Assert.AreEqual(TypeReference.ResolutionCounter, TypeReference.Locations.Count);
    }

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