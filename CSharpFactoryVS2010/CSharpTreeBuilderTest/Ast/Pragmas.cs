// ================================================================================================
// Pragmas.cs
//
// Created: 2009.06.14, by Istvan Novak (DeepDiver)
// ================================================================================================
using System.Linq;
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
      InvokeParser(project, true, false).ShouldBeTrue();
      project.ConditionalSymbols.Contains("SymbolA").ShouldBeTrue();
      project.ConditionalSymbols.Contains("SymbolB").ShouldBeFalse();
    }

    [TestMethod]
    public void DefineUndefOk2()
    {
      var project = new CSharpProject(WorkingFolder);
      project.AddFile(@"Pragmas\DefineUndefineOK.cs");
      project.AddConditionalCompilationSymbol("SymbolC");
      InvokeParser(project, true, false).ShouldBeTrue();
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
      InvokeParser(project, true, false).ShouldBeTrue();
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
      InvokeParser(project, true, false).ShouldBeFalse();
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
      InvokeParser(project, true, false).ShouldBeTrue();
      var cd = project.SyntaxTree.CompilationUnitNodes[0].TypeDeclarations[0] as ClassDeclarationNode;
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
      InvokeParser(parser, true, false).ShouldBeTrue();
      var cd = parser.SyntaxTree.CompilationUnitNodes[0].TypeDeclarations[0] as ClassDeclarationNode;
      cd.ShouldNotBeNull();
      var md = cd.MemberDeclarations[0] as MethodDeclarationNode;
      md.ShouldNotBeNull();
      md.Body.Statements.Count.ShouldEqual(1);
      md.Body.Statements[0].LeftmostName().ShouldEqual("Console");
    }

    [TestMethod]
    public void SimpleTrueElifOk()
    {
      var project = new CSharpProject(WorkingFolder);
      project.AddFile(@"Pragmas\IfEndif6.cs");
      InvokeParser(project, true, false).ShouldBeTrue();
      var cd = project.SyntaxTree.CompilationUnitNodes[0].TypeDeclarations[0] as ClassDeclarationNode;
      cd.ShouldNotBeNull();
      var md = cd.MemberDeclarations[0] as MethodDeclarationNode;
      md.ShouldNotBeNull();
      md.Body.Statements.Count.ShouldEqual(2);
      md.Body.Statements[0].LeftmostName().ShouldEqual("Console");
      md.Body.Statements[1].LeftmostName().ShouldEqual("Console");
    }

    [TestMethod]
    public void ComplexNestedIfOk()
    {
      var project = new CSharpProject(WorkingFolder);
      project.AddFile(@"Pragmas\IfEndif7.cs");
      InvokeParser(project, true, false).ShouldBeTrue();
      var cd = project.SyntaxTree.CompilationUnitNodes[0].TypeDeclarations[0] as ClassDeclarationNode;
      cd.ShouldNotBeNull();
      var md = cd.MemberDeclarations[0] as MethodDeclarationNode;
      md.ShouldNotBeNull();
      md.Body.Statements.Count.ShouldEqual(3);
      md.Body.Statements[0].LeftmostName().ShouldEqual("Console");
      md.Body.Statements[1].LeftmostName().ShouldEqual("Console");
      md.Body.Statements[2].LeftmostName().ShouldEqual("Console");
    }

    [TestMethod]
    public void IfPragmaBoundariesAreOk()
    {
      var project = new CSharpProject(WorkingFolder);
      project.AddFile(@"Pragmas\IfEndif7.cs");
      InvokeParser(project, true, false).ShouldBeTrue();
      var source = project.SyntaxTree.CompilationUnitNodes[0];
      source.Pragmas.Count.ShouldEqual(13);
      var ifPragma = source.Pragmas[0] as IfPragmaNode;
      ifPragma.ShouldNotBeNull();
      ifPragma.StartLine.ShouldEqual(6);
      ifPragma.StartColumn.ShouldEqual(1);
      ifPragma.EvaluatesToTrue.ShouldBeFalse();
      ifPragma.SkippedTokens.Count().ShouldEqual(5);
      // TODO: Fix this check
      //ifPragma.SkippedTokens.Take(1).First().Value.ShouldEqual("/*");
      ifPragma.SkippedTokens.Skip(1).Take(1).First().Value.ShouldEqual("This");
      ifPragma.SkippedTokens.Skip(2).Take(1).First().Value.ShouldEqual("is");
      ifPragma.SkippedTokens.Skip(3).Take(1).First().Value.ShouldEqual("syntactically");
      ifPragma.SkippedTokens.Skip(4).Take(1).First().Value.ShouldEqual("legal");
      ifPragma.ElseIfPragmas.Count.ShouldEqual(2);
      ifPragma.ElseIfPragmas[0].ShouldBeSameAs(source.Pragmas[1]);
      ifPragma.ElseIfPragmas[1].ShouldBeSameAs(source.Pragmas[2]);
      ifPragma.ElsePragma.ShouldNotBeNull();
      ifPragma.ElsePragma.ShouldBeSameAs(source.Pragmas[11]);
      ifPragma.EndIfPragma.ShouldNotBeNull();
      ifPragma.EndIfPragma.ShouldBeSameAs(source.Pragmas[12]);

      var elifPragma = source.Pragmas[1] as ElseIfPragmaNode;
      elifPragma.ShouldNotBeNull();
      elifPragma.StartLine.ShouldEqual(8);
      elifPragma.StartColumn.ShouldEqual(1);
      elifPragma.EvaluatesToTrue.ShouldBeFalse();
      elifPragma.SkippedTokens.Count().ShouldEqual(115);
      elifPragma.SkippedTokens.Skip(0).Take(1).First().Value.ShouldEqual("Console");
      elifPragma.SkippedTokens.Skip(1).Take(1).First().Value.ShouldEqual(".");
      elifPragma.SkippedTokens.Skip(2).Take(1).First().Value.ShouldEqual("WriteLine");
      elifPragma.SkippedTokens.Skip(3).Take(1).First().Value.ShouldEqual("(");
      elifPragma.SkippedTokens.Skip(4).Take(1).First().Value.ShouldEqual(")");
      // --- Other skipped token check omitted.
      elifPragma.SkippedTokens.Skip(110).Take(1).First().Value.ShouldEqual("-");
      elifPragma.SkippedTokens.Skip(111).Take(1).First().Value.ShouldEqual("End");
      elifPragma.SkippedTokens.Skip(112).Take(1).First().Value.ShouldEqual("of");
      elifPragma.SkippedTokens.Skip(113).Take(1).First().Value.ShouldEqual("\"false\"");
      elifPragma.SkippedTokens.Skip(114).Take(1).First().Value.ShouldEqual("branch");

      elifPragma = source.Pragmas[2] as ElseIfPragmaNode;
      elifPragma.ShouldNotBeNull();
      elifPragma.StartLine.ShouldEqual(30);
      elifPragma.StartColumn.ShouldEqual(1);
      elifPragma.EvaluatesToTrue.ShouldBeTrue();

      ifPragma = source.Pragmas[3] as IfPragmaNode;
      ifPragma.ShouldNotBeNull();
      ifPragma.StartLine.ShouldEqual(32);
      ifPragma.StartColumn.ShouldEqual(3);
      ifPragma.EvaluatesToTrue.ShouldBeFalse();
      ifPragma.SkippedTokens.Count().ShouldEqual(12);
      ifPragma.SkippedTokens.Skip(0).Take(1).First().Value.ShouldEqual("Console");
      ifPragma.SkippedTokens.Skip(1).Take(1).First().Value.ShouldEqual(".");
      ifPragma.SkippedTokens.Skip(2).Take(1).First().Value.ShouldEqual("WriteLine");
      ifPragma.SkippedTokens.Skip(3).Take(1).First().Value.ShouldEqual("(");
      ifPragma.SkippedTokens.Skip(4).Take(1).First().Value.ShouldEqual(")");
      ifPragma.SkippedTokens.Skip(5).Take(1).First().Value.ShouldEqual(";");
      ifPragma.SkippedTokens.Skip(6).Take(1).First().Value.ShouldEqual("Console");
      ifPragma.SkippedTokens.Skip(7).Take(1).First().Value.ShouldEqual(".");
      ifPragma.SkippedTokens.Skip(8).Take(1).First().Value.ShouldEqual("WriteLine");
      ifPragma.SkippedTokens.Skip(9).Take(1).First().Value.ShouldEqual("(");
      ifPragma.SkippedTokens.Skip(10).Take(1).First().Value.ShouldEqual(")");
      ifPragma.SkippedTokens.Skip(11).Take(1).First().Value.ShouldEqual(";");
      ifPragma.ElseIfPragmas.Count.ShouldEqual(2);
      ifPragma.ElseIfPragmas[0].ShouldBeSameAs(source.Pragmas[4]);
      ifPragma.ElseIfPragmas[1].ShouldBeSameAs(source.Pragmas[9]);
      ifPragma.ElsePragma.ShouldBeNull();
      ifPragma.EndIfPragma.ShouldNotBeNull();
      ifPragma.EndIfPragma.ShouldBeSameAs(source.Pragmas[10]);

      elifPragma = source.Pragmas[4] as ElseIfPragmaNode;
      elifPragma.ShouldNotBeNull();
      elifPragma.StartLine.ShouldEqual(35);
      elifPragma.StartColumn.ShouldEqual(3);
      elifPragma.EvaluatesToTrue.ShouldBeTrue();

      ifPragma = source.Pragmas[5] as IfPragmaNode;
      ifPragma.ShouldNotBeNull();
      ifPragma.StartLine.ShouldEqual(36);
      ifPragma.StartColumn.ShouldEqual(5);
      ifPragma.EvaluatesToTrue.ShouldBeFalse();
      ifPragma.SkippedTokens.Count().ShouldEqual(6);
      ifPragma.SkippedTokens.Skip(0).Take(1).First().Value.ShouldEqual("Console");
      ifPragma.SkippedTokens.Skip(1).Take(1).First().Value.ShouldEqual(".");
      ifPragma.SkippedTokens.Skip(2).Take(1).First().Value.ShouldEqual("WriteLine");
      ifPragma.SkippedTokens.Skip(3).Take(1).First().Value.ShouldEqual("(");
      ifPragma.SkippedTokens.Skip(4).Take(1).First().Value.ShouldEqual(")");
      ifPragma.SkippedTokens.Skip(5).Take(1).First().Value.ShouldEqual(";");
      ifPragma.ElseIfPragmas.Count.ShouldEqual(1);
      ifPragma.ElseIfPragmas[0].ShouldBeSameAs(source.Pragmas[6]);
      ifPragma.ElsePragma.ShouldNotBeNull();
      ifPragma.ElsePragma.ShouldBeSameAs(source.Pragmas[7]);
      ifPragma.EndIfPragma.ShouldNotBeNull();
      ifPragma.EndIfPragma.ShouldBeSameAs(source.Pragmas[8]);

      elifPragma = source.Pragmas[6] as ElseIfPragmaNode;
      elifPragma.ShouldNotBeNull();
      elifPragma.StartLine.ShouldEqual(38);
      elifPragma.StartColumn.ShouldEqual(5);
      elifPragma.EvaluatesToTrue.ShouldBeFalse();
      elifPragma.SkippedTokens.Count().ShouldEqual(6);
      elifPragma.SkippedTokens.Skip(0).Take(1).First().Value.ShouldEqual("Console");
      elifPragma.SkippedTokens.Skip(1).Take(1).First().Value.ShouldEqual(".");
      elifPragma.SkippedTokens.Skip(2).Take(1).First().Value.ShouldEqual("WriteLine");
      elifPragma.SkippedTokens.Skip(3).Take(1).First().Value.ShouldEqual("(");
      elifPragma.SkippedTokens.Skip(4).Take(1).First().Value.ShouldEqual(")");
      elifPragma.SkippedTokens.Skip(5).Take(1).First().Value.ShouldEqual(";");

      var elsePragma = source.Pragmas[7] as ElsePragmaNode;
      elsePragma.ShouldNotBeNull();
      elsePragma.StartLine.ShouldEqual(40);
      elsePragma.StartColumn.ShouldEqual(5);
      elsePragma.EvaluatesToTrue.ShouldBeTrue();
      elsePragma.SkippedTokens.Count().ShouldEqual(0);

      var endifPragma = source.Pragmas[8] as EndIfPragmaNode;
      endifPragma.ShouldNotBeNull();
      endifPragma.StartLine.ShouldEqual(44);
      endifPragma.StartColumn.ShouldEqual(5);

      elifPragma = source.Pragmas[9] as ElseIfPragmaNode;
      elifPragma.ShouldNotBeNull();
      elifPragma.StartLine.ShouldEqual(47);
      elifPragma.StartColumn.ShouldEqual(3);
      elifPragma.EvaluatesToTrue.ShouldBeFalse();
      elifPragma.SkippedTokens.Count().ShouldEqual(12);
      elifPragma.SkippedTokens.Skip(0).Take(1).First().Value.ShouldEqual("Console");
      elifPragma.SkippedTokens.Skip(1).Take(1).First().Value.ShouldEqual(".");
      elifPragma.SkippedTokens.Skip(2).Take(1).First().Value.ShouldEqual("WriteLine");
      elifPragma.SkippedTokens.Skip(3).Take(1).First().Value.ShouldEqual("(");
      elifPragma.SkippedTokens.Skip(4).Take(1).First().Value.ShouldEqual(")");
      elifPragma.SkippedTokens.Skip(5).Take(1).First().Value.ShouldEqual(";");
      elifPragma.SkippedTokens.Skip(6).Take(1).First().Value.ShouldEqual("Console");
      elifPragma.SkippedTokens.Skip(7).Take(1).First().Value.ShouldEqual(".");
      elifPragma.SkippedTokens.Skip(8).Take(1).First().Value.ShouldEqual("WriteLine");
      elifPragma.SkippedTokens.Skip(9).Take(1).First().Value.ShouldEqual("(");
      elifPragma.SkippedTokens.Skip(10).Take(1).First().Value.ShouldEqual(")");
      elifPragma.SkippedTokens.Skip(11).Take(1).First().Value.ShouldEqual(";");

      endifPragma = source.Pragmas[10] as EndIfPragmaNode;
      endifPragma.ShouldNotBeNull();
      endifPragma.StartLine.ShouldEqual(50);
      endifPragma.StartColumn.ShouldEqual(3);

      elsePragma = source.Pragmas[11] as ElsePragmaNode;
      elsePragma.ShouldNotBeNull();
      elsePragma.StartLine.ShouldEqual(51);
      elsePragma.StartColumn.ShouldEqual(1);
      elsePragma.EvaluatesToTrue.ShouldBeFalse();
      elsePragma.SkippedTokens.Count().ShouldEqual(6);
      elsePragma.SkippedTokens.Skip(0).Take(1).First().Value.ShouldEqual("Console");
      elsePragma.SkippedTokens.Skip(1).Take(1).First().Value.ShouldEqual(".");
      elsePragma.SkippedTokens.Skip(2).Take(1).First().Value.ShouldEqual("WriteLine");
      elsePragma.SkippedTokens.Skip(3).Take(1).First().Value.ShouldEqual("(");
      elsePragma.SkippedTokens.Skip(4).Take(1).First().Value.ShouldEqual(")");
      elsePragma.SkippedTokens.Skip(5).Take(1).First().Value.ShouldEqual(";");

      endifPragma = source.Pragmas[12] as EndIfPragmaNode;
      endifPragma.ShouldNotBeNull();
      endifPragma.StartLine.ShouldEqual(53);
      endifPragma.StartColumn.ShouldEqual(1);
    }

    [TestMethod]
    public void IfEndifInVerbatimStringOk()
    {
      var project = new CSharpProject(WorkingFolder);
      project.AddFile(@"Pragmas\IfEndifInVerbatimString.cs");
      InvokeParser(project, true, false).ShouldBeTrue();
      var cd = project.SyntaxTree.CompilationUnitNodes[0].TypeDeclarations[0] as ClassDeclarationNode;
      cd.ShouldNotBeNull();
      var md = cd.MemberDeclarations[0] as MethodDeclarationNode;
      md.ShouldNotBeNull();
      md.Body.Statements.Count.ShouldEqual(1);
      md.Body.Statements[0].LeftmostName().ShouldEqual("System");
    }

    [TestMethod]
    public void PragmaAfterNonWhiteSpaceFails()
    {
      var project = new CSharpProject(WorkingFolder);
      project.AddFile(@"Pragmas\PragmaAfterNonWhiteSpace.cs");
      InvokeParser(project, true, false).ShouldBeFalse();
      Assert.AreEqual(project.Errors.Count, 12);
      foreach (var error in project.Errors)
      {
        Assert.AreEqual(error.Code, "CS1040");
      }
    }

    [TestMethod]
    public void ErrorAndWarningsByPragma()
    {
      var project = new CSharpProject(WorkingFolder);
      project.AddFile(@"Pragmas\ErrorAndWarningByPragma.cs");
      InvokeParser(project, true, false).ShouldBeFalse();
      project.Errors.Count.ShouldEqual(2);
      project.Warnings.Count.ShouldEqual(2);
      project.Errors[0].Description.ShouldEqual("#error: 1. Error");
      project.Errors[1].Description.ShouldEqual("#error: 2. Error");
      project.Warnings[0].Description.ShouldEqual("#warning: 1. Warning");
      project.Warnings[1].Description.ShouldEqual("#warning: 2. Warning");
    }

    [TestMethod]
    public void LinePragmaIsOk()
    {
      var project = new CSharpProject(WorkingFolder);
      project.AddFile(@"Pragmas\LinePragma.cs");
      InvokeParser(project, true, false).ShouldBeFalse();
      project.Errors.Count.ShouldEqual(4);
      project.Warnings.Count.ShouldEqual(3);
      project.Warnings[0].Line.ShouldBeGreaterThan(1000);
      project.Warnings[0].SourceFileName.ShouldNotEqual("correct.cs");
      project.Warnings[1].Line.ShouldBeLessThan(1000);
      project.Warnings[1].SourceFileName.ShouldNotEqual("correct.cs");
      project.Warnings[2].Line.ShouldBeGreaterThan(2000);
      project.Warnings[2].SourceFileName.ShouldEqual("correct.cs");
      foreach (var error in project.Errors)
      {
        Assert.AreEqual(error.Code, "CS1576");
      }
    }

    [TestMethod]
    public void RegionHandlingIsOk()
    {
      var project = new CSharpProject(WorkingFolder);
      project.AddFile(@"Pragmas\RegionOk.cs");
      InvokeParser(project, true, false).ShouldBeTrue();
      var source = project.SyntaxTree.CompilationUnitNodes[0];
      source.Regions.Count().ShouldEqual(3);

      var reg = source.Regions.Skip(0).Take(1).First();
      reg.ParentRegion.ShouldBeNull();
      reg.StartLine.ShouldEqual(1);
      reg.StartColumn.ShouldEqual(1);
      reg.PreprocessorText.ShouldEqual("1");
      reg.EndLine.ShouldEqual(15);
      reg.EndColumn.ShouldEqual(17);
      reg.EndRegion.PreprocessorText.ShouldEqual("End 1");
      reg.EndRegion.RegionPragma.ShouldBeSameAs(reg);
      reg.NestedRegions.Count.ShouldEqual(2);

      reg = source.Regions.Skip(0).Take(1).First().NestedRegions[0];
      reg.ParentRegion.ShouldBeSameAs(source.Regions.Skip(0).Take(1).First());
      reg.StartLine.ShouldEqual(3);
      reg.StartColumn.ShouldEqual(3);
      reg.PreprocessorText.ShouldEqual("1a");
      reg.EndLine.ShouldEqual(9);
      reg.EndColumn.ShouldEqual(20);
      reg.EndRegion.PreprocessorText.ShouldEqual("End 1a");
      reg.EndRegion.RegionPragma.ShouldBeSameAs(reg);
      reg.NestedRegions.Count.ShouldEqual(1);

      reg = source.Regions.Skip(0).Take(1).First().NestedRegions[0].NestedRegions[0];
      reg.ParentRegion.ShouldBeSameAs(source.Regions.Skip(0).Take(1).First().NestedRegions[0]);
      reg.StartLine.ShouldEqual(5);
      reg.StartColumn.ShouldEqual(5);
      reg.PreprocessorText.ShouldEqual("1a1");
      reg.EndLine.ShouldEqual(7);
      reg.EndColumn.ShouldEqual(23);
      reg.EndRegion.PreprocessorText.ShouldEqual("End 1a1");
      reg.EndRegion.RegionPragma.ShouldBeSameAs(reg);
      reg.NestedRegions.Count.ShouldEqual(0);

      reg = source.Regions.Skip(0).Take(1).First().NestedRegions[1];
      reg.ParentRegion.ShouldBeSameAs(source.Regions.Skip(0).Take(1).First());
      reg.StartLine.ShouldEqual(11);
      reg.StartColumn.ShouldEqual(3);
      reg.PreprocessorText.ShouldEqual("1b");
      reg.EndLine.ShouldEqual(13);
      reg.EndColumn.ShouldEqual(20);
      reg.EndRegion.PreprocessorText.ShouldEqual("End 1b");
      reg.EndRegion.RegionPragma.ShouldBeSameAs(reg);
      reg.NestedRegions.Count.ShouldEqual(0);

      reg = source.Regions.Skip(1).Take(1).First();
      reg.ParentRegion.ShouldBeNull();
      reg.StartLine.ShouldEqual(17);
      reg.StartColumn.ShouldEqual(1);
      reg.PreprocessorText.ShouldEqual("2");
      reg.EndLine.ShouldEqual(18);
      reg.EndColumn.ShouldEqual(11);
      reg.EndRegion.PreprocessorText.ShouldEqual("");
      reg.EndRegion.RegionPragma.ShouldBeSameAs(reg);
      reg.NestedRegions.Count.ShouldEqual(0);

      reg = source.Regions.Skip(2).Take(1).First();
      reg.ParentRegion.ShouldBeNull();
      reg.StartLine.ShouldEqual(20);
      reg.StartColumn.ShouldEqual(1);
      reg.PreprocessorText.ShouldEqual("3");
      reg.EndLine.ShouldEqual(23);
      reg.EndColumn.ShouldEqual(17);
      reg.EndRegion.PreprocessorText.ShouldEqual("End 3");
      reg.EndRegion.RegionPragma.ShouldBeSameAs(reg);
      reg.NestedRegions.Count.ShouldEqual(1);

      reg = source.Regions.Skip(2).Take(1).First().NestedRegions[0];
      reg.ParentRegion.ShouldBeSameAs(source.Regions.Skip(2).Take(1).First());
      reg.StartLine.ShouldEqual(21);
      reg.StartColumn.ShouldEqual(3);
      reg.PreprocessorText.ShouldEqual("3a");
      reg.EndLine.ShouldEqual(22);
      reg.EndColumn.ShouldEqual(20);
      reg.EndRegion.PreprocessorText.ShouldEqual("End 3a");
      reg.EndRegion.RegionPragma.ShouldBeSameAs(reg);
      reg.NestedRegions.Count.ShouldEqual(0);
    }

    [TestMethod]
    public void OpenRegionFound()
    {
      var project = new CSharpProject(WorkingFolder);
      project.AddFile(@"Pragmas\OpenRegion.cs");
      InvokeParser(project, true, false).ShouldBeFalse();
      project.Errors.Count.ShouldEqual(1);
      project.Errors[0].Code.ShouldEqual("CS1038");
    }

    [TestMethod]
    public void UnexpectedEndregionFound()
    {
      var project = new CSharpProject(WorkingFolder);
      project.AddFile(@"Pragmas\UnexpectedEndRegion.cs");
      InvokeParser(project, true, false).ShouldBeFalse();
      project.Errors.Count.ShouldEqual(1);
      project.Errors[0].Code.ShouldEqual("CS1028");
    }

  }
}