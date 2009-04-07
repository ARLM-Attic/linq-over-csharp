using CSharpFactory.ProjectModel;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CSharpParserTest.LanguageElements
{
  [TestClass]
  public class Pragmas : ParserTestBed
  {
    [TestMethod]
    public void DefineUndefOk1()
    {
      CompilationUnit parser = new CompilationUnit(WorkingFolder);
      parser.AddFile(@"Pragmas\DefineUndefineOK.cs");
      Assert.IsTrue(InvokeParser(parser));
      Assert.IsTrue(parser.IsConditionalSymbolDefined("SymbolA"));
      Assert.IsFalse(parser.IsConditionalSymbolDefined("SymbolB"));
    }      

    [TestMethod]
    public void DefineUndefOk2()
    {
      CompilationUnit parser = new CompilationUnit(WorkingFolder);
      parser.AddFile(@"Pragmas\DefineUndefineOK.cs");
      parser.AddConditionalCompilationSymbol("SymbolC");
      Assert.IsTrue(InvokeParser(parser));
      Assert.IsTrue(parser.IsConditionalSymbolDefined("SymbolA"));
      Assert.IsFalse(parser.IsConditionalSymbolDefined("SymbolB"));
      Assert.IsTrue(parser.IsConditionalSymbolDefined("SymbolC"));
    }

    [TestMethod]
    public void DefineUndefOk3()
    {
      CompilationUnit parser = new CompilationUnit(WorkingFolder);
      parser.AddFile(@"Pragmas\DefineUndefineOK.cs");
      parser.AddConditionalCompilationSymbols(new[] { "SymbolA", "SymbolB", "SymbolC" });
      Assert.IsTrue(InvokeParser(parser));
      Assert.IsTrue(parser.IsConditionalSymbolDefined("SymbolA"));
      Assert.IsFalse(parser.IsConditionalSymbolDefined("SymbolB"));
      Assert.IsTrue(parser.IsConditionalSymbolDefined("SymbolC"));
    }

    [TestMethod]
    public void DefineUndeWithCS1032()
    {
      CompilationUnit parser = new CompilationUnit(WorkingFolder);
      parser.AddFile(@"Pragmas\DefineUndefineWithCS1032.cs");
      parser.AddConditionalCompilationSymbols(new[] { "SymbolA", "SymbolB", "SymbolC" });
      Assert.IsFalse(InvokeParser(parser));
      Assert.IsTrue(parser.IsConditionalSymbolDefined("SymbolA"));
      Assert.IsFalse(parser.IsConditionalSymbolDefined("SymbolB"));
      Assert.IsTrue(parser.IsConditionalSymbolDefined("SymbolC"));
      Assert.AreEqual(parser.Errors.Count, 8);
    }

    [TestMethod]
    public void NestedIfEndifOk()
    {
      CompilationUnit parser = new CompilationUnit(WorkingFolder);
      parser.AddFile(@"Pragmas\IfEndif1.cs");
      Assert.IsTrue(InvokeParser(parser));
      ClassDeclaration cd = parser.DeclaredTypes[0] as ClassDeclaration;
      Assert.IsNotNull(cd);
      MethodDeclaration md = cd.Members[0] as MethodDeclaration;
      Assert.IsNotNull(md);
      Assert.AreEqual(md.Statements.Count, 2);
      Assert.AreEqual(md.Statements[0].LeftmostName, "CheckConsistency");
      Assert.AreEqual(md.Statements[1].LeftmostName, "CommitHelper");
    }

    [TestMethod]
    public void BeginningCommentInIfOk()
    {
      CompilationUnit parser = new CompilationUnit(WorkingFolder);
      parser.AddFile(@"Pragmas\IfEndif3.cs");
      Assert.IsTrue(InvokeParser(parser));
      ClassDeclaration cd = parser.DeclaredTypes[0] as ClassDeclaration;
      Assert.IsNotNull(cd);
      MethodDeclaration md = cd.Members[0] as MethodDeclaration;
      Assert.IsNotNull(md);
      Assert.AreEqual(md.Statements.Count, 1);
      Assert.AreEqual(md.Statements[0].LeftmostName, "Console");
    }

    [TestMethod]
    public void BeginningCommentInElseOk()
    {
      CompilationUnit parser = new CompilationUnit(WorkingFolder);
      parser.AddFile(@"Pragmas\IfEndif4.cs");
      Assert.IsTrue(InvokeParser(parser));
      ClassDeclaration cd = parser.DeclaredTypes[0] as ClassDeclaration;
      Assert.IsNotNull(cd);
      MethodDeclaration md = cd.Members[0] as MethodDeclaration;
      Assert.IsNotNull(md);
      Assert.AreEqual(md.Statements.Count, 1);
      Assert.AreEqual(md.Statements[0].LeftmostName, "Console");
    }

    [TestMethod]
    public void DoubleTrueElifOk()
    {
      CompilationUnit parser = new CompilationUnit(WorkingFolder);
      parser.AddFile(@"Pragmas\IfEndif5.cs");
      Assert.IsTrue(InvokeParser(parser));
      ClassDeclaration cd = parser.DeclaredTypes[0] as ClassDeclaration;
      Assert.IsNotNull(cd);
      MethodDeclaration md = cd.Members[0] as MethodDeclaration;
      Assert.IsNotNull(md);
      Assert.AreEqual(md.Statements.Count, 1);
      Assert.AreEqual(md.Statements[0].LeftmostName, "Console");
    }

    [TestMethod]
    public void SimpleTrueElifOk()
    {
      CompilationUnit parser = new CompilationUnit(WorkingFolder);
      parser.AddFile(@"Pragmas\IfEndif6.cs");
      Assert.IsTrue(InvokeParser(parser));
      ClassDeclaration cd = parser.DeclaredTypes[0] as ClassDeclaration;
      Assert.IsNotNull(cd);
      MethodDeclaration md = cd.Members[0] as MethodDeclaration;
      Assert.IsNotNull(md);
      Assert.AreEqual(md.Statements.Count, 2);
      Assert.AreEqual(md.Statements[0].LeftmostName, "Console");
      Assert.AreEqual(md.Statements[1].LeftmostName, "Console");
    }

    [TestMethod]
    public void ComplexNestedIffOk()
    {
      CompilationUnit parser = new CompilationUnit(WorkingFolder);
      parser.AddFile(@"Pragmas\IfEndif7.cs");
      Assert.IsTrue(InvokeParser(parser));
      ClassDeclaration cd = parser.DeclaredTypes[0] as ClassDeclaration;
      Assert.IsNotNull(cd);
      MethodDeclaration md = cd.Members[0] as MethodDeclaration;
      Assert.IsNotNull(md);
      Assert.AreEqual(md.Statements.Count, 3);
      Assert.AreEqual(md.Statements[0].LeftmostName, "Console");
      Assert.AreEqual(md.Statements[1].LeftmostName, "Console");
      Assert.AreEqual(md.Statements[2].LeftmostName, "Console");
    }

    [TestMethod]
    public void IfEndifInVerbatimStringOk()
    {
      CompilationUnit parser = new CompilationUnit(WorkingFolder);
      parser.AddFile(@"Pragmas\IfEndifInVerbatimString.cs");
      Assert.IsTrue(InvokeParser(parser));
      ClassDeclaration cd = parser.DeclaredTypes[0] as ClassDeclaration;
      Assert.IsNotNull(cd);
      MethodDeclaration md = cd.Members[0] as MethodDeclaration;
      Assert.IsNotNull(md);
      Assert.AreEqual(md.Statements.Count, 1);
      Assert.AreEqual(md.Statements[0].LeftmostName, "System");
    }

    [TestMethod]
    public void PragmaAfterNonWhiteSpaceFails()
    {
      CompilationUnit parser = new CompilationUnit(WorkingFolder);
      parser.AddFile(@"Pragmas\PragmaAfterNonWhiteSpace.cs");
      Assert.IsFalse(InvokeParser(parser));
      Assert.AreEqual(parser.Errors.Count, 12);
      foreach (var error in parser.Errors)
      {
        Assert.AreEqual(error.Code, "CS1040");
      }
    }

    [TestMethod]
    public void ErrorAndWarningsByPragma()
    {
      CompilationUnit parser = new CompilationUnit(WorkingFolder);
      parser.AddFile(@"Pragmas\ErrorAndWarningByPragma.cs");
      Assert.IsFalse(InvokeParser(parser));
      Assert.AreEqual(parser.Errors.Count, 2);
      Assert.AreEqual(parser.Warnings.Count, 2);
      Assert.AreEqual(parser.Errors[0].Description, "#error: 1. Error");
      Assert.AreEqual(parser.Errors[1].Description, "#error: 2. Error");
      Assert.AreEqual(parser.Warnings[0].Description, "#warning: 1. Warning");
      Assert.AreEqual(parser.Warnings[1].Description, "#warning: 2. Warning");
    }

    [TestMethod]
    public void LinePragmaIsOk()
    {
      CompilationUnit parser = new CompilationUnit(WorkingFolder);
      parser.AddFile(@"Pragmas\LinePragma.cs");
      Assert.IsFalse(InvokeParser(parser));
      Assert.AreEqual(parser.Errors.Count, 4);
      Assert.AreEqual(parser.Warnings.Count, 3);
      Assert.IsTrue(parser.Warnings[0].Line > 1000);
      Assert.AreNotEqual(parser.Warnings[0].SourceFileName, "correct.cs");
      Assert.IsTrue(parser.Warnings[1].Line < 1000);
      Assert.AreNotEqual(parser.Warnings[1].SourceFileName, "correct.cs");
      Assert.IsTrue(parser.Warnings[2].Line > 2000);
      Assert.AreEqual(parser.Warnings[2].SourceFileName, "correct.cs");
      foreach (var error in parser.Errors)
      {
        Assert.AreEqual(error.Code, "CS1576");
      }
    }

    [TestMethod]
    public void RegionHandlingIsOk()
    {
      CompilationUnit parser = new CompilationUnit(WorkingFolder);
      parser.AddFile(@"Pragmas\RegionOk.cs");
      Assert.IsTrue(InvokeParser(parser));
      Assert.AreEqual(parser.Files[0].Regions.Count, 6);

      RegionInfo reg = parser.Files[0].Regions[0];
      Assert.AreEqual(reg.StartLine, 1);
      Assert.AreEqual(reg.StartColumn, 1);
      Assert.AreEqual(reg.StartText, "1");
      Assert.AreEqual(reg.EndLine, 11);
      Assert.AreEqual(reg.EndColumn, 1);
      Assert.AreEqual(reg.EndText, "End 1");

      reg = parser.Files[0].Regions[1];
      Assert.AreEqual(reg.StartLine, 3);
      Assert.AreEqual(reg.StartColumn, 1);
      Assert.AreEqual(reg.StartText, "1a");
      Assert.AreEqual(reg.EndLine, 9);
      Assert.AreEqual(reg.EndColumn, 1);
      Assert.AreEqual(reg.EndText, "End 1a");

      reg = parser.Files[0].Regions[2];
      Assert.AreEqual(reg.StartLine, 5);
      Assert.AreEqual(reg.StartColumn, 1);
      Assert.AreEqual(reg.StartText, "1a1");
      Assert.AreEqual(reg.EndLine, 7);
      Assert.AreEqual(reg.EndColumn, 1);
      Assert.AreEqual(reg.EndText, "End 1a1");

      reg = parser.Files[0].Regions[5];
      Assert.AreEqual(reg.StartLine, 17);
      Assert.AreEqual(reg.StartColumn, 3);
      Assert.AreEqual(reg.StartText, "3a");
      Assert.AreEqual(reg.EndLine, 18);
      Assert.AreEqual(reg.EndColumn, 3);
      Assert.AreEqual(reg.EndText, "End 3a");
    }

    [TestMethod]
    public void OpenRegionFound()
    {
      CompilationUnit parser = new CompilationUnit(WorkingFolder);
      parser.AddFile(@"Pragmas\OpenRegion.cs");
      Assert.IsFalse(InvokeParser(parser));
      Assert.AreEqual(parser.Errors.Count, 1);
      Assert.AreEqual(parser.Errors[0].Code, "CS1038");
    }

    [TestMethod]
    public void UnexpectedEndregionFound()
    {
      CompilationUnit parser = new CompilationUnit(WorkingFolder);
      parser.AddFile(@"Pragmas\UnexpectedEndRegion.cs");
      Assert.IsFalse(InvokeParser(parser));
      Assert.AreEqual(parser.Errors.Count, 1);
      Assert.AreEqual(parser.Errors[0].Code, "CS1028");
    }
  }
}
