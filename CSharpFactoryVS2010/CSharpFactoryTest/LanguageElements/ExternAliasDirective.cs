using System;
using CSharpFactory.ProjectModel;
using CSharpFactory.Syntax;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CSharpParserTest.LanguageElements
{
  [TestClass]
  public class ExternAliasDirective: ParserTestBed
  {
    [TestMethod]
    public void ExternAliasDirectiveCheckOK()
    {
      CompilationUnit parser = new CompilationUnit(WorkingFolder);
      parser.AddFile(@"ExternAliasDirective\ExternAliasDirectiveOK.cs");
      Assert.IsFalse(InvokeParser(parser));
      Assert.AreEqual(parser.Errors.Count, 3);
      Assert.AreEqual(parser.Errors[0].Code, "CS0430");
      Assert.AreEqual(parser.Errors[1].Code, "CS0430");
      Assert.AreEqual(parser.Errors[2].Code, "CS0430");
    }

    [TestMethod]
    public void ExternAliasSyntaxError()
    {
      CompilationUnit parser = new CompilationUnit(WorkingFolder);
      parser.AddFile(@"ExternAliasDirective\ExternAliasDirectiveFailed.cs");
      Assert.IsFalse(InvokeParser(parser));
      Assert.AreEqual(parser.Errors.Count, 8);
      Assert.AreEqual(parser.Errors[0].Code, "CS1003");
      Assert.AreEqual(parser.Errors[1].Code, "CS1003");
      Assert.AreEqual(parser.Errors[2].Code, "SYNERR");
    }

    [TestMethod]
    public void ExternAliasesCheckedOK()
    {
      CompilationUnit parser = new CompilationUnit(WorkingFolder);
      parser.AddFile(@"ExternAliasDirective\ExternAliasesChecked.cs");
      Assert.IsFalse(InvokeParser(parser));
      Assert.AreEqual(parser.Errors.Count, 5);
      Assert.AreEqual(parser.Errors[0].Code, "CS0430");
      Assert.AreEqual(parser.Errors[1].Code, "CS0430");
      Assert.AreEqual(parser.Errors[2].Code, "CS0430");
      Assert.AreEqual(parser.Errors[3].Code, "CS0430");
      Assert.AreEqual(parser.Errors[4].Code, "CS0430");
      SourceFile file = parser.Files[0];
      Assert.AreEqual(file.ExternAliases.Count, 3);
      Assert.AreEqual(file.ExternAliases[0].Name, "Alias1");
      Assert.AreEqual(file.ExternAliases[1].Name, "Alias2");
      Assert.AreEqual(file.ExternAliases[2].Name, "Alias3");
      NamespaceFragment ns = file.NestedNamespaces[0];
      Assert.AreEqual(ns.ExternAliases.Count, 2);
      Assert.AreEqual(ns.ExternAliases[0].Name, "Alias1");
      Assert.AreEqual(ns.ExternAliases[1].Name, "Alias2");
    }

    [TestMethod]
    public void ExternAliasSyntaxTreeOK()
    {
      CompilationUnit parser = new CompilationUnit(WorkingFolder);
      parser.AddFile(@"ExternAliasDirective\ExternAliasDirectiveOK.cs");
      Assert.IsFalse(InvokeParser(parser));
      Assert.AreEqual(parser.Errors.Count, 3);
      Assert.AreEqual(parser.Errors[0].Code, "CS0430");
      Assert.AreEqual(parser.Errors[1].Code, "CS0430");
      Assert.AreEqual(parser.Errors[2].Code, "CS0430");
      var externs = parser.SyntaxTree.SourceFileNodes[0].ExternAliaseNodes;
      Assert.AreEqual(externs.Count, 3);
      Assert.AreEqual(externs[0].AliasToken.val, "alias");
      Assert.AreEqual(externs[0].Identifier, "Alias1");
      Assert.AreEqual(externs[1].AliasToken.val, "alias");
      Assert.AreEqual(externs[1].Identifier, "Alias2");
      Assert.AreEqual(externs[2].AliasToken.val, "alias");
      Assert.AreEqual(externs[2].Identifier, "Alias3");
    }

    [TestMethod]
    public void ExternAliasSyntaxTreeWriterOk()
    {
      CompilationUnit parser = new CompilationUnit(WorkingFolder);
      parser.AddFile(@"ExternAliasDirective\ExternAliasDirectiveOK.cs");
      Assert.IsFalse(InvokeParser(parser));
      Assert.AreEqual(parser.Errors.Count, 3);
      Assert.AreEqual(parser.Errors[0].Code, "CS0430");
      Assert.AreEqual(parser.Errors[1].Code, "CS0430");
      Assert.AreEqual(parser.Errors[2].Code, "CS0430");
      var treeWriter = new SyntaxTreeTextWriter(parser.SyntaxTree, parser.ProjectProvider) 
      { WorkingFolder = TempOutputFolder };
      treeWriter.WriteTree();
      parser = new CompilationUnit(TempOutputFolder);
      parser.AddFile(@"ExternAliasDirective\ExternAliasDirectiveOK.cs");
      Assert.IsFalse(InvokeParser(parser));
      Assert.AreEqual(parser.Errors.Count, 3);
      Assert.AreEqual(parser.Errors[0].Code, "CS0430");
      Assert.AreEqual(parser.Errors[1].Code, "CS0430");
      Assert.AreEqual(parser.Errors[2].Code, "CS0430");
    }
  }
}
