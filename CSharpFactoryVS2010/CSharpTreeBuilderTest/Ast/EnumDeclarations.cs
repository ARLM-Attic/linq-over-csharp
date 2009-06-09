// ================================================================================================
// EnumDeclarations.cs
//
// Created: 2009.05.29, by Istvan Novak (DeepDiver)
// ================================================================================================
using CSharpTreeBuilder.Ast;
using CSharpTreeBuilder.ProjectContent;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CSharpTreeBuilderTest
{
  [TestClass]
  public class EnumDeclarations: ParserTestBed
  {
    [TestMethod]
    public void EnumDeclarationIsOk()
    {
      var parser = new CSharpProject(WorkingFolder);
      parser.AddFile(@"EnumDeclaration\EnumDeclaration3.cs");
      Assert.IsTrue(InvokeParser(parser));
      var source = parser.SyntaxTree.SourceFileNodes[0];
      Assert.AreEqual(source.TypeDeclarations.Count, 1);
      var typeDecl = source.TypeDeclarations[0] as EnumDeclarationNode;
      Assert.IsNotNull(typeDecl);
      Assert.AreEqual(typeDecl.Name, "C");
      Assert.AreEqual(typeDecl.Values.Count, 2);

      var nsDecl = source.NamespaceDeclarations[0];
      Assert.AreEqual(nsDecl.TypeDeclarations.Count, 2);
      
      typeDecl = nsDecl.TypeDeclarations[0] as EnumDeclarationNode;
      Assert.IsNotNull(typeDecl);
      Assert.AreEqual(typeDecl.Name, "A");
      Assert.AreEqual(typeDecl.Values.Count, 2);
      
      typeDecl = nsDecl.TypeDeclarations[1] as EnumDeclarationNode;
      Assert.IsNotNull(typeDecl);
      Assert.AreEqual(typeDecl.Name, "B");
      Assert.AreEqual(typeDecl.Values.Count, 3);
    }

    [TestMethod]
    public void EnumNodeBounderiesAreOk()
    {
      var parser = new CSharpProject(WorkingFolder);
      parser.AddFile(@"EnumDeclaration\EnumDeclaration3.cs");
      Assert.IsTrue(InvokeParser(parser));
      var source = parser.SyntaxTree.SourceFileNodes[0];
      var enumDecl = source.TypeDeclarations[0] as EnumDeclarationNode;
      Assert.IsNotNull(enumDecl);
      Assert.AreEqual(enumDecl.StartToken.Value, "enum");
      Assert.AreEqual(enumDecl.StartLine, 17);
      Assert.AreEqual(enumDecl.StartColumn, 8);
      Assert.AreEqual(enumDecl.EndLine, 21);
      Assert.AreEqual(enumDecl.EndColumn, 1);
      Assert.AreEqual(enumDecl.Identifier, "C");
      Assert.AreEqual(enumDecl.IdentifierToken.Line, 17);
      Assert.AreEqual(enumDecl.IdentifierToken.Column, 13);
      Assert.AreEqual(enumDecl.OpenBrace.Line, 18);
      Assert.AreEqual(enumDecl.OpenBrace.Column, 1);
      Assert.AreEqual(enumDecl.CloseBrace.Line, 21);
      Assert.AreEqual(enumDecl.CloseBrace.Column, 1);

      var enumVal = enumDecl.Values[0];
      Assert.AreEqual(enumVal.StartLine, 19);
      Assert.AreEqual(enumVal.StartColumn, 3);
    }

    [TestMethod]
    public void EnumSyntaxTreeWriterIsOk()
    {
      var project = new CSharpProject(WorkingFolder);
      project.AddFile(@"EnumDeclaration\EnumDeclaration3.cs");
      Assert.IsTrue(InvokeParser(project));
      var treeWriter = new SyntaxTreeTextWriter(project.SyntaxTree, project.ProjectProvider) 
      { WorkingFolder = TempOutputFolder };
      treeWriter.WriteTree();
      project = new CSharpProject(TempOutputFolder);
      project.AddFile(@"EnumDeclaration\EnumDeclaration3.cs");
      Assert.IsTrue(InvokeParser(project));
    }

    [TestMethod]
    public void NestedEnumDeclarationIsOk()
    {
      var parser = new CSharpProject(WorkingFolder);
      parser.AddFile(@"EnumDeclaration\EnumDeclaration2.cs");
      Assert.IsTrue(InvokeParser(parser));
      var source = parser.SyntaxTree.SourceFileNodes[0];
      Assert.AreEqual(source.TypeDeclarations.Count, 2);
      var typeDecl = source.TypeDeclarations[0] as StructDeclarationNode;
      Assert.IsNotNull(typeDecl);
      Assert.AreEqual(typeDecl.Name, "A");
      
      var enumDecl = typeDecl.NestedTypes[0] as EnumDeclarationNode;
      Assert.IsNotNull(enumDecl);
      Assert.AreEqual(enumDecl.Name, "C");
      Assert.AreEqual(enumDecl.Modifiers.Count, 2);
      Assert.AreEqual(enumDecl.Modifiers[0].Value, ModifierType.Public);
      Assert.AreEqual(enumDecl.Modifiers[1].Value, ModifierType.New);
    }


  }
}