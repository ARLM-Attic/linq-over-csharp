// ================================================================================================
// UsingsAndNamespaces.cs
//
// Created: 2009.05.29, by Istvan Novak (DeepDiver)
// ================================================================================================
using System.Collections.Generic;
using System.Linq;
using CSharpTreeBuilder.Ast;
using CSharpTreeBuilder.ProjectContent;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CSharpTreeBuilderTest
{
  [TestClass]
  public class UsingsAndNamespaces: ParserTestBed
  {
    [TestMethod]
    public void UsingsSyntaxTreeIsOk()
    {
      var project = new CSharpProject(WorkingFolder);
      project.AddFile(@"UsingsAndNamespaces\UsingsAndNamespacesOK.cs");
      project.AddAssemblyReference("System.Data");
      project.AddAssemblyReference("System.Xml");
      Assert.IsTrue(InvokeParser(project));

      var sn = project.SyntaxTree.SourceFileNodes[0];
      Assert.AreEqual(sn.UsingNodes.Count, 6);
      Assert.AreEqual(sn.UsingWithAliasNodes.Count(), 2);

      var typeName = sn.UsingNodes[0].TypeName;
      Assert.IsFalse(typeName.HasQualifier);
      Assert.AreEqual(typeName.TypeTags.Count, 1);
      var tag = typeName.TypeTags[0];
      Assert.AreEqual(tag.Identifier, "System");

      typeName = sn.UsingNodes[1].TypeName;
      Assert.IsFalse(typeName.HasQualifier);
      Assert.AreEqual(typeName.TypeTags.Count, 3);
      tag = typeName.TypeTags[0];
      Assert.AreEqual(tag.Identifier, "System");
      tag = typeName.TypeTags[1];
      Assert.AreEqual(tag.Identifier, "Collections");
      tag = typeName.TypeTags[2];
      Assert.AreEqual(tag.Identifier, "Generic");

      typeName = sn.UsingNodes[2].TypeName;
      Assert.IsFalse(typeName.HasQualifier);
      Assert.AreEqual(typeName.TypeTags.Count, 2);
      tag = typeName.TypeTags[0];
      Assert.AreEqual(tag.Identifier, "System");
      tag = typeName.TypeTags[1];
      Assert.AreEqual(tag.Identifier, "Text");

      typeName = sn.UsingNodes[3].TypeName;
      Assert.IsTrue(typeName.HasQualifier);
      Assert.AreEqual("global", typeName.Qualifier);
      Assert.AreEqual(2, typeName.TypeTags.Count);
      tag = typeName.TypeTags[0];
      Assert.AreEqual("System", tag.Identifier);
      tag = typeName.TypeTags[1];
      Assert.AreEqual("IO", tag.Identifier);
      
      var aliasNodes = new List<UsingAliasNode>(sn.UsingWithAliasNodes);
      Assert.AreEqual(aliasNodes[0].Alias, "AliasName");
      typeName = aliasNodes[0].TypeName;
      Assert.IsFalse(typeName.HasQualifier);
      Assert.AreEqual(typeName.TypeTags.Count, 3);
      tag = typeName.TypeTags[0];
      Assert.AreEqual(tag.Identifier, "System");
      tag = typeName.TypeTags[1];
      Assert.AreEqual(tag.Identifier, "Text");
      tag = typeName.TypeTags[2];
      Assert.AreEqual(tag.Identifier, "Encoding");

      Assert.AreEqual(aliasNodes[1].Alias, "SecondAlias");
      typeName = aliasNodes[1].TypeName;
      Assert.IsFalse(typeName.HasQualifier);
      Assert.AreEqual(typeName.TypeTags.Count, 2);
      tag = typeName.TypeTags[0];
      Assert.AreEqual(tag.Identifier, "Microsoft");
      tag = typeName.TypeTags[1];
      Assert.AreEqual(tag.Identifier, "Win32");
    }

    [TestMethod]
    public void UsingsNodeBoundariesAreOk()
    {
      var project = new CSharpProject(WorkingFolder);
      project.AddFile(@"UsingsAndNamespaces\UsingsAndNamespacesOK.cs");
      project.AddAssemblyReference("System.Data");
      project.AddAssemblyReference("System.Xml");
      Assert.IsTrue(InvokeParser(project));

      var sn = project.SyntaxTree.SourceFileNodes[0];
      Assert.AreEqual(sn.UsingNodes.Count, 6);
      Assert.AreEqual(sn.UsingWithAliasNodes.Count(), 2);

      // --- Check for "using System;" token positions
      var usingNode = sn.UsingNodes[0];
      Assert.AreEqual(usingNode.StartToken.Value, "using");
      Assert.AreEqual(usingNode.StartLine, 1);
      Assert.AreEqual(usingNode.StartColumn, 1);
      Assert.AreEqual(usingNode.EndLine, 1);
      Assert.AreEqual(usingNode.EndColumn, 13);
      Assert.AreEqual(usingNode.TypeName.TypeTags.Count, 1);
      var typeTag = usingNode.TypeName.TypeTags[0];
      Assert.AreEqual(typeTag.StartToken.Value, "System");
      Assert.AreEqual(typeTag.StartLine, 1);
      Assert.AreEqual(typeTag.StartColumn, 7);
      Assert.AreEqual(typeTag.EndLine, 1);
      Assert.AreEqual(typeTag.EndColumn, 12);
      Assert.AreEqual(usingNode.TerminatingToken.Value, ";");
      Assert.AreEqual(usingNode.TerminatingToken.Line, 1);
      Assert.AreEqual(usingNode.TerminatingToken.Column, 13);

      // --- Check for "using System.Collections.Generic;"
      usingNode = sn.UsingNodes[1];
      Assert.AreEqual(usingNode.StartToken.Value, "using");
      Assert.AreEqual(usingNode.StartLine, 2);
      Assert.AreEqual(usingNode.StartColumn, 1);
      Assert.AreEqual(usingNode.EndLine, 2);
      Assert.AreEqual(usingNode.EndColumn, 33);
      Assert.AreEqual(usingNode.TypeName.TypeTags.Count, 3);
      typeTag = usingNode.TypeName.TypeTags[0];
      Assert.AreEqual(typeTag.StartToken.Value, "System");
      Assert.AreEqual(typeTag.StartLine, 2);
      Assert.AreEqual(typeTag.StartColumn, 7);
      Assert.AreEqual(typeTag.EndLine, 2);
      Assert.AreEqual(typeTag.EndColumn, 12);
      typeTag = usingNode.TypeName.TypeTags[1];
      Assert.AreEqual(typeTag.StartToken.Value, "Collections");
      Assert.AreEqual(typeTag.SeparatorToken.Value, ".");
      Assert.AreEqual(typeTag.StartLine, 2);
      Assert.AreEqual(typeTag.StartColumn, 14);
      Assert.AreEqual(typeTag.EndLine, 2);
      Assert.AreEqual(typeTag.EndColumn, 24);
      typeTag = usingNode.TypeName.TypeTags[2];
      Assert.AreEqual(typeTag.StartToken.Value, "Generic");
      Assert.AreEqual(typeTag.SeparatorToken.Value, ".");
      Assert.AreEqual(typeTag.StartLine, 2);
      Assert.AreEqual(typeTag.StartColumn, 26);
      Assert.AreEqual(typeTag.EndLine, 2);
      Assert.AreEqual(typeTag.EndColumn, 32);
      Assert.AreEqual(usingNode.TerminatingToken.Value, ";");
      Assert.AreEqual(usingNode.TerminatingToken.Line, 2);
      Assert.AreEqual(usingNode.TerminatingToken.Column, 33);

      // --- Check for "using AliasName = System.Text.Encoding;"
      usingNode = sn.UsingNodes[4];
      Assert.AreEqual(usingNode.StartToken.Value, "using");
      Assert.AreEqual(usingNode.StartLine, 5);
      Assert.AreEqual(usingNode.StartColumn, 1);
      Assert.AreEqual(usingNode.EndLine, 5);
      Assert.AreEqual(usingNode.EndColumn, 39);
      Assert.AreEqual(usingNode.TypeName.TypeTags.Count, 3);
      typeTag = usingNode.TypeName.TypeTags[0];
      Assert.AreEqual(typeTag.StartToken.Value, "System");
      Assert.AreEqual(typeTag.StartLine, 5);
      Assert.AreEqual(typeTag.StartColumn, 19);
      Assert.AreEqual(typeTag.EndLine, 5);
      Assert.AreEqual(typeTag.EndColumn, 24);
      typeTag = usingNode.TypeName.TypeTags[1];
      Assert.AreEqual(typeTag.StartToken.Value, "Text");
      Assert.AreEqual(typeTag.SeparatorToken.Value, ".");
      Assert.AreEqual(typeTag.StartLine, 5);
      Assert.AreEqual(typeTag.StartColumn, 26);
      Assert.AreEqual(typeTag.EndLine, 5);
      Assert.AreEqual(typeTag.EndColumn, 29);
      typeTag = usingNode.TypeName.TypeTags[2];
      Assert.AreEqual(typeTag.StartToken.Value, "Encoding");
      Assert.AreEqual(typeTag.SeparatorToken.Value, ".");
      Assert.AreEqual(typeTag.StartLine, 5);
      Assert.AreEqual(typeTag.StartColumn, 31);
      Assert.AreEqual(typeTag.EndLine, 5);
      Assert.AreEqual(typeTag.EndColumn, 38);
      var aliasNode = usingNode as UsingAliasNode;
      Assert.IsNotNull(aliasNode);
      Assert.AreEqual(aliasNode.EqualToken.Line, 5);
      Assert.AreEqual(aliasNode.EqualToken.Column, 17);
      Assert.AreEqual(aliasNode.Alias, "AliasName");
      Assert.AreEqual(aliasNode.AliasToken.Line, 5);
      Assert.AreEqual(aliasNode.AliasToken.Column, 7);
      Assert.AreEqual(usingNode.TerminatingToken.Value, ";");
      Assert.AreEqual(usingNode.TerminatingToken.Line, 5);
      Assert.AreEqual(usingNode.TerminatingToken.Column, 39);
    }

    [TestMethod]
    public void NameSpacesSyntaxTreeIsOk()
    {
      var project = new CSharpProject(WorkingFolder);
      project.AddFile(@"UsingsAndNamespaces\UsingsAndNamespacesOK.cs");
      project.AddAssemblyReference("System.Data");
      project.AddAssemblyReference("System.Xml");
      Assert.IsTrue(InvokeParser(project));

      // --- Check syntax tree
      var sn = project.SyntaxTree.SourceFileNodes[0];
      Assert.AreEqual(sn.NamespaceDeclarations.Count, 3);
      var nsDecl = sn.NamespaceDeclarations[0];
      Assert.AreEqual(nsDecl.NameTags.FullName, "CSharpParserTest.TestFiles");
      Assert.AreEqual(nsDecl.UsingNodes.Count, 3);
      Assert.AreEqual(nsDecl.NamespaceDeclarations.Count, 3);

      nsDecl = sn.NamespaceDeclarations[1];
      Assert.AreEqual(nsDecl.NameTags.FullName, "OtherNameSpace");
      Assert.AreEqual(nsDecl.UsingNodes.Count, 0);
      Assert.AreEqual(nsDecl.NamespaceDeclarations.Count, 1);

      nsDecl = sn.NamespaceDeclarations[2];
      Assert.AreEqual(nsDecl.NameTags.FullName, "CSharpParserTest.TestFiles");
      Assert.AreEqual(nsDecl.UsingNodes.Count, 2);
      Assert.AreEqual(nsDecl.NamespaceDeclarations.Count, 1);
    }

    [TestMethod]
    public void NamespaceNodeBoundariesAreOk()
    {
      RunBoudaryTestOn(WorkingFolder, @"UsingsAndNamespaces\UsingsAndNamespacesOK.cs");
    }

    [TestMethod]
    public void UsingAndNamespaceSyntaxTreeWriterOk()
    {
      var project = new CSharpProject(WorkingFolder);
      project.AddFile(@"UsingsAndNamespaces\UsingsAndNamespacesOK.cs");
      project.AddAssemblyReference("System.Data");
      project.AddAssemblyReference("System.Xml");
      Assert.IsTrue(InvokeParser(project));
      var treeWriter = new SyntaxTreeTextWriter(project.SyntaxTree, project.ProjectProvider) { WorkingFolder = TempOutputFolder };
      treeWriter.WriteTree();
      RunBoudaryTestOn(TempOutputFolder, @"UsingsAndNamespaces\UsingsAndNamespacesOK.cs");
    }

    private void RunBoudaryTestOn(string workingDir, string fileName)
    {
      var project = new CSharpProject(workingDir);
      project.AddFile(fileName);
      project.AddAssemblyReference("System.Data");
      project.AddAssemblyReference("System.Xml");
      Assert.IsTrue(InvokeParser(project));

      var sn = project.SyntaxTree.SourceFileNodes[0];
      Assert.AreEqual(sn.NamespaceDeclarations.Count, 3);
      var nsDecl = sn.NamespaceDeclarations[0];
      Assert.AreEqual(nsDecl.StartToken.Value, "namespace");
      Assert.AreEqual(nsDecl.StartLine, 8);
      Assert.AreEqual(nsDecl.StartColumn, 1);
      Assert.AreEqual(nsDecl.EndLine, 48);
      Assert.AreEqual(nsDecl.EndColumn, 1);
      Assert.AreEqual(nsDecl.NameTags.Count, 2);
      var nameTag = nsDecl.NameTags[0];
      Assert.AreEqual(nameTag.StartToken.Value, "CSharpParserTest");
      Assert.AreEqual(nameTag.StartLine, 8);
      Assert.AreEqual(nameTag.StartColumn, 11);
      Assert.AreEqual(nameTag.EndLine, 8);
      Assert.AreEqual(nameTag.EndColumn, 26);
      nameTag = nsDecl.NameTags[1];
      Assert.AreEqual(nameTag.StartToken.Value, "TestFiles");
      Assert.AreEqual(nameTag.SeparatorToken.Value, ".");
      Assert.AreEqual(nameTag.StartLine, 8);
      Assert.AreEqual(nameTag.StartColumn, 28);
      Assert.AreEqual(nameTag.EndLine, 8);
      Assert.AreEqual(nameTag.EndColumn, 36);
      Assert.AreEqual(nsDecl.OpenBracket.Value, "{");
      Assert.AreEqual(nsDecl.OpenBracket.Line, 9);
      Assert.AreEqual(nsDecl.OpenBracket.Column, 1);
      Assert.AreEqual(nsDecl.CloseBracket.Line, 48);
      Assert.AreEqual(nsDecl.OpenBracket.Column, 1);

      nsDecl = sn.NamespaceDeclarations[0].NamespaceDeclarations[0].NamespaceDeclarations[0];
      Assert.AreEqual(nsDecl.StartToken.Value, "namespace");
      Assert.AreEqual(nsDecl.StartLine, 18);
      Assert.AreEqual(nsDecl.StartColumn, 5);
      Assert.AreEqual(nsDecl.EndLine, 22);
      Assert.AreEqual(nsDecl.EndColumn, 5);
      Assert.AreEqual(nsDecl.NameTags.Count, 1);
      nameTag = nsDecl.NameTags[0];
      Assert.AreEqual(nameTag.StartToken.Value, "Level2");
      Assert.AreEqual(nameTag.StartLine, 18);
      Assert.AreEqual(nameTag.StartColumn, 15);
      Assert.AreEqual(nameTag.EndLine, 18);
      Assert.AreEqual(nameTag.EndColumn, 20);
      Assert.AreEqual(nsDecl.OpenBracket.Value, "{");
      Assert.AreEqual(nsDecl.OpenBracket.Line, 19);
      Assert.AreEqual(nsDecl.OpenBracket.Column, 5);
      Assert.AreEqual(nsDecl.CloseBracket.Line, 22);
      Assert.AreEqual(nsDecl.OpenBracket.Column, 5);
    }

    [TestMethod]
    public void UsingsParentIsOk()
    {
      var project = new CSharpProject(WorkingFolder);
      project.AddFile(@"UsingsAndNamespaces\UsingsAndNamespacesOK.cs");
      project.AddAssemblyReference("System.Data");
      project.AddAssemblyReference("System.Xml");
      Assert.IsTrue(InvokeParser(project));

      var sn = project.SyntaxTree.SourceFileNodes[0];
      Assert.AreEqual(sn.UsingNodes.Count, 6);
      foreach (var usingNode in sn.UsingNodes)
      {
        Assert.AreEqual(usingNode.ParentNode, sn);
        Assert.AreEqual(usingNode.TypeName.ParentNode, sn);
      }

      var nsDecl = sn.NamespaceDeclarations[0];
      Assert.AreEqual(nsDecl.UsingNodes.Count, 3);
      foreach (var usingNode in nsDecl.UsingNodes)
      {
        Assert.AreEqual(usingNode.ParentNode, nsDecl);
        Assert.AreEqual(usingNode.TypeName.ParentNode, nsDecl);
      }
      
      nsDecl = sn.NamespaceDeclarations[0].NamespaceDeclarations[0];
      Assert.AreEqual(nsDecl.UsingNodes.Count, 1);
      foreach (var usingNode in nsDecl.UsingNodes)
      {
        Assert.AreEqual(usingNode.ParentNode, nsDecl);
        Assert.AreEqual(usingNode.TypeName.ParentNode, nsDecl);
      }

      nsDecl = sn.NamespaceDeclarations[0].NamespaceDeclarations[0].NamespaceDeclarations[0];
      Assert.AreEqual(nsDecl.UsingNodes.Count, 2);
      foreach (var usingNode in nsDecl.UsingNodes)
      {
        Assert.AreEqual(usingNode.ParentNode, nsDecl);
        Assert.AreEqual(usingNode.TypeName.ParentNode, nsDecl);
      }

      nsDecl = sn.NamespaceDeclarations[0].NamespaceDeclarations[1];
      Assert.AreEqual(nsDecl.UsingNodes.Count, 1);
      foreach (var usingNode in nsDecl.UsingNodes)
      {
        Assert.AreEqual(usingNode.ParentNode, nsDecl);
        Assert.AreEqual(usingNode.TypeName.ParentNode, nsDecl);
      }

      nsDecl = sn.NamespaceDeclarations[0].NamespaceDeclarations[2].NamespaceDeclarations[0];
      Assert.AreEqual(nsDecl.UsingNodes.Count, 2);
      foreach (var usingNode in nsDecl.UsingNodes)
      {
        Assert.AreEqual(usingNode.ParentNode, nsDecl);
        Assert.AreEqual(usingNode.TypeName.ParentNode, nsDecl);
      }
    }

    [TestMethod]
    public void NamespaceParentsAreOk()
    {
      var project = new CSharpProject(WorkingFolder);
      project.AddFile(@"UsingsAndNamespaces\UsingsAndNamespacesOK.cs");
      project.AddAssemblyReference("System.Data");
      project.AddAssemblyReference("System.Xml");
      Assert.IsTrue(InvokeParser(project));

      var sn = project.SyntaxTree.SourceFileNodes[0];
      foreach (var nsNode in sn.NamespaceDeclarations)
      {
        Assert.AreEqual(nsNode.ParentNode, sn);
      }

      var nsDecl = sn.NamespaceDeclarations[0];
      foreach (var nsNode in nsDecl.NamespaceDeclarations)
      {
        Assert.AreEqual(nsNode.ParentNode, nsDecl);
      }

      nsDecl = sn.NamespaceDeclarations[0].NamespaceDeclarations[0];
      foreach (var nsNode in nsDecl.NamespaceDeclarations)
      {
        Assert.AreEqual(nsNode.ParentNode, nsDecl);
      }

      nsDecl = sn.NamespaceDeclarations[0].NamespaceDeclarations[0].NamespaceDeclarations[0];
      foreach (var nsNode in nsDecl.NamespaceDeclarations)
      {
        Assert.AreEqual(nsNode.ParentNode, nsDecl);
      }

      nsDecl = sn.NamespaceDeclarations[0].NamespaceDeclarations[1];
      foreach (var nsNode in nsDecl.NamespaceDeclarations)
      {
        Assert.AreEqual(nsNode.ParentNode, nsDecl);
      }

      nsDecl = sn.NamespaceDeclarations[0].NamespaceDeclarations[2].NamespaceDeclarations[0];
      foreach (var nsNode in nsDecl.NamespaceDeclarations)
      {
        Assert.AreEqual(nsNode.ParentNode, nsDecl);
      }
    }
  }
}