// ================================================================================================
// ExternAliasDirective.cs
//
// Created: 2009.05.29, by Istvan Novak (DeepDiver)
// ================================================================================================
using CSharpTreeBuilder.Ast;
using CSharpTreeBuilder.ProjectContent;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CSharpTreeBuilderTest
{
  [TestClass]
  public class ExternAliasDirective: ParserTestBed
  {
    [TestMethod]
    public void ExternAliasNodeOk()
    {
      var project = new CSharpProject(WorkingFolder);
      project.AddFile(@"ExternAliasDirective\ExternAliasDirectiveOK.cs");
      Assert.IsTrue(InvokeParser(project));
      Assert.AreEqual(project.Errors.Count, 0);
      var externs = project.SyntaxTree.SourceFileNodes[0].ExternAliasNodes;
      Assert.AreEqual(externs.Count, 3);
      Assert.AreEqual(externs[0].AliasToken.Value, "alias");
      Assert.AreEqual(externs[0].Identifier, "Alias1");
      Assert.AreEqual(externs[1].AliasToken.Value, "alias");
      Assert.AreEqual(externs[1].Identifier, "Alias2");
      Assert.AreEqual(externs[2].AliasToken.Value, "alias");
      Assert.AreEqual(externs[2].Identifier, "Alias3");
    }

    [TestMethod]
    public void ExternAliasNodeBoundariesOk()
    {
      RunBoudaryTestOn(WorkingFolder, @"ExternAliasDirective\ExternAliasDirectiveOK.cs");
    }

    [TestMethod]
    public void ExternAliasSyntaxTreeWriterOk()
    {
      var project = new CSharpProject(WorkingFolder);
      project.AddFile(@"ExternAliasDirective\ExternAliasDirectiveOK.cs");
      Assert.IsTrue(InvokeParser(project));
      Assert.AreEqual(project.Errors.Count, 0);
      var treeWriter = new SyntaxTreeTextWriter(project.SyntaxTree, project.ProjectProvider) { WorkingFolder = TempOutputFolder };
      treeWriter.WriteTree();
      RunBoudaryTestOn(TempOutputFolder, @"ExternAliasDirective\ExternAliasDirectiveOK.cs");
    }

    private void RunBoudaryTestOn(string workingDir, string fileName)
    {
      var project = new CSharpProject(workingDir);
      project.AddFile(fileName);
      Assert.IsTrue(InvokeParser(project));
      Assert.AreEqual(project.Errors.Count, 0);
      var externs = project.SyntaxTree.SourceFileNodes[0].ExternAliasNodes;
      Assert.AreEqual(externs.Count, 3);
      var eaNode = externs[0];
      Assert.AreEqual(eaNode.StartToken.Value, "extern");
      Assert.AreEqual(eaNode.StartToken.Line, 1);
      Assert.AreEqual(eaNode.StartToken.Column, 1);
      Assert.AreEqual(eaNode.TerminatingToken.Line, 1);
      Assert.AreEqual(eaNode.TerminatingToken.Column, 20);
      Assert.AreEqual(eaNode.AliasToken.Value, "alias");
      Assert.AreEqual(eaNode.AliasToken.Line, 1);
      Assert.AreEqual(eaNode.AliasToken.Column, 8);
      Assert.AreEqual(eaNode.Identifier, "Alias1");
      Assert.AreEqual(eaNode.IdentifierToken.Line, 1);
      Assert.AreEqual(eaNode.IdentifierToken.Column, 14);

      eaNode = externs[1];
      Assert.AreEqual(eaNode.StartToken.Value, "extern");
      Assert.AreEqual(eaNode.StartToken.Line, 2);
      Assert.AreEqual(eaNode.StartToken.Column, 1);
      Assert.AreEqual(eaNode.TerminatingToken.Line, 2);
      Assert.AreEqual(eaNode.TerminatingToken.Column, 20);
      Assert.AreEqual(eaNode.AliasToken.Value, "alias");
      Assert.AreEqual(eaNode.AliasToken.Line, 2);
      Assert.AreEqual(eaNode.AliasToken.Column, 8);
      Assert.AreEqual(eaNode.Identifier, "Alias2");
      Assert.AreEqual(eaNode.IdentifierToken.Line, 2);
      Assert.AreEqual(eaNode.IdentifierToken.Column, 14);

      eaNode = externs[2];
      Assert.AreEqual(eaNode.StartToken.Value, "extern");
      Assert.AreEqual(eaNode.StartToken.Line, 3);
      Assert.AreEqual(eaNode.StartToken.Column, 1);
      Assert.AreEqual(eaNode.TerminatingToken.Line, 3);
      Assert.AreEqual(eaNode.TerminatingToken.Column, 20);
      Assert.AreEqual(eaNode.AliasToken.Value, "alias");
      Assert.AreEqual(eaNode.AliasToken.Line, 3);
      Assert.AreEqual(eaNode.AliasToken.Column, 8);
      Assert.AreEqual(eaNode.Identifier, "Alias3");
      Assert.AreEqual(eaNode.IdentifierToken.Line, 3);
      Assert.AreEqual(eaNode.IdentifierToken.Column, 14);
    }

    [TestMethod]
    public void ExternAliasParentsOk()
    {
      var project = new CSharpProject(WorkingFolder);
      project.AddFile(@"ExternAliasDirective\ExternAliasDirectiveOK.cs");
      Assert.IsTrue(InvokeParser(project));
      var sn = project.SyntaxTree.SourceFileNodes[0];
      foreach (var extNode in sn.ExternAliasNodes)
      {
        Assert.AreEqual(extNode.ParentNode, sn);
      }

    }
  }
}