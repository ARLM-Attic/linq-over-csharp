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

    [TestMethod]
    public void TypeResolutionCounterIsOk1()
    {
      CompilationUnit parser = new CompilationUnit(CSharpParserFolder);
      TypeReference.Locations.Clear();
      TypeReference.ResolutionCounter = 0;
      parser.AddFile(@"ParserFiles\CommentHandler.cs");
      Assert.IsTrue(InvokeParser(parser));
      Console.WriteLine("Type references: {0}", TypeReference.Locations.Count);
      Console.WriteLine("Type resolutions: {0}", TypeReference.ResolutionCounter);
      Assert.AreEqual(TypeReference.ResolutionCounter, TypeReference.Locations.Count);

      int counter = 0;
      foreach (TypeReferenceLocation loc in TypeReference.Locations)
      {
        if (!loc.Reference.IsResolved)
        {
          Console.WriteLine("{0}, ({1}, {2})", loc.File.Name, loc.Reference.StartLine,
            loc.Reference.StartColumn);
          counter++;
        }
        if (counter > 10) break;
      }
    }

    [TestMethod]
    public void _TypeResolutionCounterIsOk2()
    {
      CompilationUnit parser = new CompilationUnit(CSharpParserFolder);
      TypeReference.Locations.Clear();
      TypeReference.ResolutionCounter = 0;
      parser.AddFile(@"ParserFiles\Modifier.cs");
      Assert.IsTrue(InvokeParser(parser));
      Console.WriteLine("Type references: {0}", TypeReference.Locations.Count);
      Console.WriteLine("Type resolutions: {0}", TypeReference.ResolutionCounter);
      Assert.AreEqual(TypeReference.ResolutionCounter, TypeReference.Locations.Count);

      int counter = 0;
      foreach (TypeReferenceLocation loc in TypeReference.Locations)
      {
        if (!loc.Reference.IsResolved)
        {
          Console.WriteLine("{0}, ({1}, {2})", loc.File.Name, loc.Reference.StartLine,
            loc.Reference.StartColumn);
          counter++;
        }
        if (counter > 10) break;
      }
    }
  }
}