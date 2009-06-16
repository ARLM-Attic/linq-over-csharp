// ================================================================================================
// Pragmas.cs
//
// Created: 2009.06.14, by Istvan Novak (DeepDiver)
// ================================================================================================
using CSharpTreeBuilder.Ast;
using CSharpTreeBuilder.ProjectContent;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SoftwareApproach.TestingExtensions;

namespace CSharpTreeBuilderTest.Ast
{
  [TestClass]
  public class Pragmas: ParserTestBed
  {
    [TestMethod]
    public void DefineUndefOk1()
    {
      var project = new CSharpProject(WorkingFolder);
      project.AddFile(@"Pragmas\DefineUndefineOK.cs");
      InvokeParser(project).ShouldBeTrue();
      project.ConditionalSymbols.Contains("SymbolA").ShouldBeTrue();
      project.ConditionalSymbols.Contains("SymbolB").ShouldBeFalse();
    }

    [TestMethod]
    public void DefineUndefOk2()
    {
      var project = new CSharpProject(WorkingFolder);
      project.AddFile(@"Pragmas\DefineUndefineOK.cs");
      project.AddConditionalCompilationSymbol("SymbolC");
      InvokeParser(project).ShouldBeTrue();
      project.ConditionalSymbols.Contains("SymbolA").ShouldBeTrue();
      project.ConditionalSymbols.Contains("SymbolB").ShouldBeFalse();
      project.ConditionalSymbols.Contains("SymbolC").ShouldBeTrue();
    }

    [TestMethod]
    public void DefineUndefOk3()
    {
      var project = new CSharpProject(WorkingFolder);
      project.AddFile(@"Pragmas\DefineUndefineOK.cs");
      project.AddConditionalCompilationSymbols(new[] { "SymbolA", "SymbolB", "SymbolC" });
      InvokeParser(project).ShouldBeTrue();
      project.ConditionalSymbols.Contains("SymbolA").ShouldBeTrue();
      project.ConditionalSymbols.Contains("SymbolB").ShouldBeFalse();
      project.ConditionalSymbols.Contains("SymbolC").ShouldBeTrue();
    }

    [TestMethod]
    public void DefineUndeWithCS1032()
    {
      var project = new CSharpProject(WorkingFolder);
      project.AddFile(@"Pragmas\DefineUndefineWithCS1032.cs");
      project.AddConditionalCompilationSymbols(new[] { "SymbolA", "SymbolB", "SymbolC" });
      InvokeParser(project).ShouldBeFalse();
      project.ConditionalSymbols.Contains("SymbolA").ShouldBeTrue();
      project.ConditionalSymbols.Contains("SymbolB").ShouldBeFalse();
      project.ConditionalSymbols.Contains("SymbolC").ShouldBeTrue();
      project.Errors.Count.ShouldEqual(8);
    }

    [TestMethod]
    public void NestedIfEndifOk()
    {
      var project = new CSharpProject(WorkingFolder);
      project.AddFile(@"Pragmas\IfEndif1.cs");
      InvokeParser(project).ShouldBeTrue();
      var cd = project.SyntaxTree.SourceFileNodes[0].TypeDeclarations[0] as ClassDeclarationNode;
      cd.ShouldNotBeNull();
      var md = cd.MemberDeclarations[0] as MethodDeclarationNode;
      md.ShouldNotBeNull();
      md.Body.Statements.Count.ShouldEqual(2);
      md.Body.Statements[0].LeftmostName().ShouldEqual("CheckConsistency");
      md.Body.Statements[1].LeftmostName().ShouldEqual("CommitHelper");
    }

    [TestMethod]
    public void DoubleTrueElifOk()
    {
      var parser = new CSharpProject(WorkingFolder);
      parser.AddFile(@"Pragmas\IfEndif5.cs");
      InvokeParser(parser).ShouldBeTrue();
      var cd = parser.SyntaxTree.SourceFileNodes[0].TypeDeclarations[0] as ClassDeclarationNode;
      cd.ShouldNotBeNull();
      var md = cd.MemberDeclarations[0] as MethodDeclarationNode;
      md.ShouldNotBeNull();
      md.Body.Statements.Count.ShouldEqual(1);
      md.Body.Statements[0].LeftmostName().ShouldEqual("Console");
    }
  }
}