// ================================================================================================
// EnumDeclarations.cs
//
// Created: 2009.05.29, by Istvan Novak (DeepDiver)
// ================================================================================================
using CSharpTreeBuilder.Ast;
using CSharpTreeBuilder.ProjectContent;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SoftwareApproach.TestingExtensions;

namespace CSharpTreeBuilderTest
{
  [TestClass]
  public class EnumDeclarations: ParserTestBed
  {
    [TestMethod]
    public void EnumDeclarationIsOk()
    {
      var project = new CSharpProject(WorkingFolder);
      project.AddFile(@"EnumDeclaration\EnumDeclaration3.cs");
      Assert.IsTrue(InvokeParser(project));
      var source = project.SyntaxTree.CompilationUnitNodes[0];
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
      var project = new CSharpProject(WorkingFolder);
      project.AddFile(@"EnumDeclaration\EnumDeclaration3.cs");
      Assert.IsTrue(InvokeParser(project));
      var source = project.SyntaxTree.CompilationUnitNodes[0];
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
      var project = new CSharpProject(WorkingFolder);
      project.AddFile(@"EnumDeclaration\EnumDeclaration2.cs");
      Assert.IsTrue(InvokeParser(project));
      var source = project.SyntaxTree.CompilationUnitNodes[0];
      Assert.AreEqual(source.TypeDeclarations.Count, 2);
      var typeDecl = source.TypeDeclarations[0] as StructDeclarationNode;
      Assert.IsNotNull(typeDecl);
      Assert.AreEqual(typeDecl.Name, "A");
      
      var enumDecl = typeDecl.NestedTypes[0] as EnumDeclarationNode;
      Assert.IsNotNull(enumDecl);
      Assert.AreEqual(enumDecl.Name, "C");
      Assert.AreEqual(enumDecl.DeclaringType, typeDecl);
      Assert.AreEqual(enumDecl.Modifiers.Count, 2);
      Assert.AreEqual(enumDecl.Modifiers[0].Value, ModifierType.Public);
      Assert.AreEqual(enumDecl.Modifiers[1].Value, ModifierType.New);
      Assert.AreEqual(enumDecl.Values.Count, 2);
      Assert.AreEqual(enumDecl.Values[0].Identifier, "Value");
      Assert.AreEqual(enumDecl.Values[1].Identifier, "Value2");

      enumDecl = source.TypeDeclarations[1] as EnumDeclarationNode;
      Assert.AreEqual(enumDecl.Name, "C");
      Assert.IsNull(enumDecl.DeclaringType);
      Assert.AreEqual(enumDecl.Modifiers.Count, 1);
      Assert.AreEqual(enumDecl.Modifiers[0].Value, ModifierType.Public);
      Assert.AreEqual(enumDecl.Values.Count, 2);
      Assert.AreEqual(enumDecl.Values[0].Identifier, "EnumVal1");
      Assert.AreEqual(enumDecl.Values[1].Identifier, "Enumval2");
    }

    [TestMethod]
    public void EnumDeclarationWithBaseType()
    {
      var project = new CSharpProject(WorkingFolder);
      project.AddFile(@"EnumDeclaration\EnumDeclarationWithBaseType.cs");
      Assert.IsTrue(InvokeParser(project));

      var enumDecl = project.SyntaxTree.CompilationUnitNodes[0].TypeDeclarations[0] as EnumDeclarationNode;
      enumDecl.EnumBase.TypeName.TypeTags[0].Identifier.ShouldEqual("int");
    }
  }
}