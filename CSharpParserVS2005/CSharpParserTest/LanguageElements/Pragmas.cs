using CSharpParser;
using CSharpParser.ProjectModel;
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
      parser.AddConditionalCompilationSymbols(new string[] { "SymbolC" });
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
      parser.AddConditionalCompilationSymbols(new string[] { "SymbolA", "SymbolB", "SymbolC" });
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
      parser.AddConditionalCompilationSymbols(new string[] { "SymbolA", "SymbolB", "SymbolC" });
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
  }
}
