// ================================================================================================
// StructDeclarations.cs
//
// Created: 2009.06.13, by Istvan Novak (DeepDiver)
// ================================================================================================
using CSharpTreeBuilder.Ast;
using CSharpTreeBuilder.ProjectContent;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SoftwareApproach.TestingExtensions;

namespace CSharpTreeBuilderTest.Ast
{
  [TestClass]
  public class StructDeclarations: ParserTestBed
  {
    [TestMethod]
    public void StructDeclarationIsOk()
    {
      var project = new CSharpProject(WorkingFolder);
      project.AddFile(@"StructDeclaration\StructDeclaration3.cs");
      InvokeParser(project).ShouldBeTrue();
      var source = project.SyntaxTree.SourceFileNodes[0];
      source.TypeDeclarations.Count.ShouldEqual(1);
      source.NamespaceDeclarations.Count.ShouldEqual(1);
      source.NamespaceDeclarations[0].TypeDeclarations.Count.ShouldEqual(3);
      
      var structDecl = source.NamespaceDeclarations[0].TypeDeclarations[0] as StructDeclarationNode;
      structDecl.ShouldNotBeNull();
      structDecl.Modifiers.Count.ShouldEqual(1);
      structDecl.Modifiers[0].Value.ShouldEqual(ModifierType.Public);
      structDecl.Name.ShouldEqual("A");
      structDecl.NestedTypes.Count.ShouldEqual(0);
      structDecl.DeclaringType.ShouldBeNull();
      structDecl.TypeParameters.Count.ShouldEqual(0);
      structDecl.TypeParameterConstraints.Count.ShouldEqual(0);
      structDecl.BaseTypes.Count.ShouldEqual(0);

      structDecl = source.NamespaceDeclarations[0].TypeDeclarations[1] as StructDeclarationNode;
      structDecl.ShouldNotBeNull();
      structDecl.Modifiers.Count.ShouldEqual(1);
      structDecl.Modifiers[0].Value.ShouldEqual(ModifierType.Public);
      structDecl.Name.ShouldEqual("B");
      structDecl.NestedTypes.Count.ShouldEqual(0);
      structDecl.DeclaringType.ShouldBeNull();
      structDecl.TypeParameters.Count.ShouldEqual(1);
      structDecl.TypeParameters[0].Identifier.ShouldEqual("X");
      structDecl.BaseTypes.Count.ShouldEqual(1);
      var typeDecl = structDecl.BaseTypes[0];
      typeDecl.TypeTags.Count.ShouldEqual(1);
      typeDecl.TypeTags[0].Identifier.ShouldEqual("IDictionary");
      typeDecl.TypeTags[0].Arguments.Count.ShouldEqual(2);
      typeDecl.TypeTags[0].Arguments[0].TypeTags.Count.ShouldEqual(1);
      typeDecl.TypeTags[0].Arguments[0].TypeTags[0].Identifier.ShouldEqual("string");
      typeDecl.TypeTags[0].Arguments[1].TypeTags.Count.ShouldEqual(1);
      typeDecl.TypeTags[0].Arguments[1].TypeTags[0].Identifier.ShouldEqual("X");
      structDecl.TypeParameterConstraints.Count.ShouldEqual(1);
      structDecl.TypeParameterConstraints[0].Identifier.ShouldEqual("X");
      structDecl.TypeParameterConstraints[0].ConstraintTags.Count.ShouldEqual(2);
      structDecl.TypeParameterConstraints[0].ConstraintTags[0].IsStruct.ShouldBeTrue();
      structDecl.TypeParameterConstraints[0].ConstraintTags[1].IsTypeName.ShouldBeTrue();
      structDecl.TypeParameterConstraints[0].ConstraintTags[1].TypeName.TypeTags.Count.ShouldEqual(1);
      structDecl.TypeParameterConstraints[0].ConstraintTags[1].TypeName.TypeTags[0].
        Identifier.ShouldEqual("IEnumerable");

      structDecl = source.NamespaceDeclarations[0].TypeDeclarations[2] as StructDeclarationNode;
      structDecl.ShouldNotBeNull();
      structDecl.Modifiers.Count.ShouldEqual(1);
      structDecl.Modifiers[0].Value.ShouldEqual(ModifierType.Public);
      structDecl.Name.ShouldEqual("C");
      structDecl.NestedTypes.Count.ShouldEqual(2);
      structDecl.DeclaringType.ShouldBeNull();
      structDecl.TypeParameters.Count.ShouldEqual(0);
      structDecl.TypeParameterConstraints.Count.ShouldEqual(0);
      structDecl.BaseTypes.Count.ShouldEqual(0);

      structDecl = source.NamespaceDeclarations[0].TypeDeclarations[2].NestedTypes[0] as StructDeclarationNode;
      structDecl.ShouldNotBeNull();
      structDecl.Modifiers.Count.ShouldEqual(1);
      structDecl.Modifiers[0].Value.ShouldEqual(ModifierType.Internal);
      structDecl.Name.ShouldEqual("D");
      structDecl.NestedTypes.Count.ShouldEqual(0);
      structDecl.DeclaringType.ShouldBeSameAs(source.NamespaceDeclarations[0].TypeDeclarations[2]);
      structDecl.TypeParameters.Count.ShouldEqual(0);
      structDecl.TypeParameterConstraints.Count.ShouldEqual(0);
      structDecl.BaseTypes.Count.ShouldEqual(0);

      structDecl = source.NamespaceDeclarations[0].TypeDeclarations[2].NestedTypes[1] as StructDeclarationNode;
      structDecl.ShouldNotBeNull();
      structDecl.Modifiers.Count.ShouldEqual(1);
      structDecl.Modifiers[0].Value.ShouldEqual(ModifierType.Private);
      structDecl.Name.ShouldEqual("E");
      structDecl.NestedTypes.Count.ShouldEqual(0);
      structDecl.DeclaringType.ShouldBeSameAs(source.NamespaceDeclarations[0].TypeDeclarations[2]);
      structDecl.TypeParameters.Count.ShouldEqual(0);
      structDecl.TypeParameterConstraints.Count.ShouldEqual(0);
      structDecl.BaseTypes.Count.ShouldEqual(0);

      structDecl = source.TypeDeclarations[0] as StructDeclarationNode;
      structDecl.ShouldNotBeNull();
      structDecl.Modifiers.Count.ShouldEqual(1);
      structDecl.Modifiers[0].Value.ShouldEqual(ModifierType.Public);
      structDecl.Name.ShouldEqual("F");
      structDecl.NestedTypes.Count.ShouldEqual(0);
      structDecl.DeclaringType.ShouldBeNull();
      structDecl.TypeParameters.Count.ShouldEqual(0);
      structDecl.TypeParameterConstraints.Count.ShouldEqual(0);
      structDecl.BaseTypes.Count.ShouldEqual(0);
    }

    [TestMethod]
    public void StructBoundariesAreOk()
    {
      var project = new CSharpProject(WorkingFolder);
      project.AddFile(@"StructDeclaration\StructDeclaration3.cs");
      InvokeParser(project).ShouldBeTrue();
      var source = project.SyntaxTree.SourceFileNodes[0];
      source.NamespaceDeclarations.Count.ShouldEqual(1);
      var nsDecl = source.NamespaceDeclarations[0];
      nsDecl.StartToken.Line.ShouldEqual(3);
      nsDecl.StartToken.Column.ShouldEqual(1);
      nsDecl.TerminatingToken.Line.ShouldEqual(18);
      nsDecl.TerminatingToken.Column.ShouldEqual(1);
      nsDecl.TypeDeclarations.Count.ShouldEqual(3);

      var structDecl = nsDecl.TypeDeclarations[0] as StructDeclarationNode;
      structDecl.StartToken.Value.ShouldEqual("struct");
      structDecl.StartToken.Line.ShouldEqual(5);
      structDecl.StartToken.Column.ShouldEqual(10);
      structDecl.TerminatingToken.Line.ShouldEqual(6);
      structDecl.TerminatingToken.Column.ShouldEqual(5);
      structDecl.OpenBrace.Line.ShouldEqual(6);
      structDecl.OpenBrace.Column.ShouldEqual(3);
      structDecl.CloseBrace.Line.ShouldEqual(6);
      structDecl.CloseBrace.Column.ShouldEqual(5);

      structDecl = nsDecl.TypeDeclarations[1] as StructDeclarationNode;
      structDecl.StartToken.Value.ShouldEqual("struct");
      structDecl.StartToken.Line.ShouldEqual(8);
      structDecl.StartToken.Column.ShouldEqual(10);
      structDecl.TerminatingToken.Line.ShouldEqual(11);
      structDecl.TerminatingToken.Column.ShouldEqual(3);
      structDecl.OpenBrace.Line.ShouldEqual(10);
      structDecl.OpenBrace.Column.ShouldEqual(3);
      structDecl.CloseBrace.Line.ShouldEqual(11);
      structDecl.CloseBrace.Column.ShouldEqual(3);
      structDecl.TypeParameters.StartLine.ShouldEqual(8);
      structDecl.TypeParameters.StartColumn.ShouldEqual(18);
      structDecl.TypeParameters.EndLine.ShouldEqual(8);
      structDecl.TypeParameters.EndColumn.ShouldEqual(20);
      structDecl.BaseTypes.StartLine.ShouldEqual(8);
      structDecl.BaseTypes.StartColumn.ShouldEqual(23);
      structDecl.BaseTypes.EndLine.ShouldEqual(8);
      structDecl.BaseTypes.EndColumn.ShouldEqual(44);
      structDecl.TypeParameterConstraints.StartLine.ShouldEqual(9);
      structDecl.TypeParameterConstraints.StartColumn.ShouldEqual(5);
      structDecl.TypeParameterConstraints.EndLine.ShouldEqual(9);
      structDecl.TypeParameterConstraints.EndColumn.ShouldEqual(32);

      structDecl = nsDecl.TypeDeclarations[2] as StructDeclarationNode;
      structDecl.StartToken.Value.ShouldEqual("struct");
      structDecl.StartToken.Line.ShouldEqual(13);
      structDecl.StartToken.Column.ShouldEqual(10);
      structDecl.TerminatingToken.Line.ShouldEqual(17);
      structDecl.TerminatingToken.Column.ShouldEqual(3);
      structDecl.OpenBrace.Line.ShouldEqual(14);
      structDecl.OpenBrace.Column.ShouldEqual(3);
      structDecl.CloseBrace.Line.ShouldEqual(17);
      structDecl.CloseBrace.Column.ShouldEqual(3);

      structDecl = nsDecl.TypeDeclarations[2].NestedTypes[0] as StructDeclarationNode;
      structDecl.StartToken.Value.ShouldEqual("struct");
      structDecl.StartToken.Line.ShouldEqual(15);
      structDecl.StartToken.Column.ShouldEqual(14);
      structDecl.TerminatingToken.Line.ShouldEqual(15);
      structDecl.TerminatingToken.Column.ShouldEqual(25);
      structDecl.OpenBrace.Line.ShouldEqual(15);
      structDecl.OpenBrace.Column.ShouldEqual(23);
      structDecl.CloseBrace.Line.ShouldEqual(15);
      structDecl.CloseBrace.Column.ShouldEqual(25);

      structDecl = nsDecl.TypeDeclarations[2].NestedTypes[1] as StructDeclarationNode;
      structDecl.StartToken.Value.ShouldEqual("struct");
      structDecl.StartToken.Line.ShouldEqual(16);
      structDecl.StartToken.Column.ShouldEqual(13);
      structDecl.TerminatingToken.Line.ShouldEqual(16);
      structDecl.TerminatingToken.Column.ShouldEqual(24);
      structDecl.OpenBrace.Line.ShouldEqual(16);
      structDecl.OpenBrace.Column.ShouldEqual(22);
      structDecl.CloseBrace.Line.ShouldEqual(16);
      structDecl.CloseBrace.Column.ShouldEqual(24);

      structDecl = source.TypeDeclarations[0] as StructDeclarationNode;
      structDecl.StartToken.Value.ShouldEqual("struct");
      structDecl.StartToken.Line.ShouldEqual(20);
      structDecl.StartToken.Column.ShouldEqual(8);
      structDecl.TerminatingToken.Line.ShouldEqual(22);
      structDecl.TerminatingToken.Column.ShouldEqual(1);
      structDecl.OpenBrace.Line.ShouldEqual(21);
      structDecl.OpenBrace.Column.ShouldEqual(1);
      structDecl.CloseBrace.Line.ShouldEqual(22);
      structDecl.CloseBrace.Column.ShouldEqual(1);
    }

    [TestMethod]
    public void StructSyntaxTreeWriterIsOk()
    {
      var project = new CSharpProject(WorkingFolder);
      project.AddFile(@"StructDeclaration\StructDeclaration3.cs");
      Assert.IsTrue(InvokeParser(project));
      var treeWriter = new SyntaxTreeTextWriter(project.SyntaxTree, project.ProjectProvider) { WorkingFolder = TempOutputFolder };
      treeWriter.WriteTree();
      project = new CSharpProject(TempOutputFolder);
      project.AddFile(@"StructDeclaration\StructDeclaration3.cs");
      Assert.IsTrue(InvokeParser(project));
    }

  }
}