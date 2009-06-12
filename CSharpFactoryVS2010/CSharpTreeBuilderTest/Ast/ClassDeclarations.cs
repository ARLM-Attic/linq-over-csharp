// ================================================================================================
// ClassDeclarations.cs
//
// Created: 2009.06.10, by Istvan Novak (DeepDiver)
// ================================================================================================
using CSharpTreeBuilder.Ast;
using CSharpTreeBuilder.ProjectContent;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SoftwareApproach.TestingExtensions;

namespace CSharpTreeBuilderTest
{
  [TestClass]
  public class ClassDeclarations: ParserTestBed
  {
    [TestMethod]
    public void ClassDeclarationIsOk()
    {
      var project = new CSharpProject(WorkingFolder);
      project.AddFile(@"ClassDeclaration\ClassDeclaration1.cs");
      InvokeParser(project).ShouldBeTrue();
      var source = project.SyntaxTree.SourceFileNodes[0];
      source.TypeDeclarations.Count.ShouldEqual(3);
      var classDecl = source.TypeDeclarations[0] as ClassDeclarationNode;
      classDecl.ShouldNotBeNull();
      classDecl.Modifiers.Count.ShouldEqual(1);
      classDecl.Modifiers[0].Value.ShouldEqual(ModifierType.Public);
      classDecl.Name.ShouldEqual("A");
      classDecl.NestedTypes.Count.ShouldEqual(1);
      classDecl.DeclaringType.ShouldBeNull();

      var parentClass = classDecl;
      classDecl = classDecl.NestedTypes[0] as ClassDeclarationNode;
      classDecl.ShouldNotBeNull();
      classDecl.Modifiers.Count.ShouldEqual(2);
      classDecl.Modifiers[0].Value.ShouldEqual(ModifierType.Public);
      classDecl.Modifiers[1].Value.ShouldEqual(ModifierType.New);
      classDecl.Name.ShouldEqual("C");
      classDecl.DeclaringType.ShouldBeSameAs(parentClass);

      classDecl = source.TypeDeclarations[1] as ClassDeclarationNode;
      classDecl.ShouldNotBeNull();
      classDecl.Modifiers.Count.ShouldEqual(1);
      classDecl.Modifiers[0].Value.ShouldEqual(ModifierType.Public);
      classDecl.Name.ShouldEqual("B");
      classDecl.NestedTypes.Count.ShouldEqual(1);
      classDecl.DeclaringType.ShouldBeNull();
      classDecl.BaseTypes.Count.ShouldEqual(1);
      classDecl.BaseTypes[0].TypeTags.Count.ShouldEqual(1);
      classDecl.BaseTypes[0].TypeTags[0].Identifier.ShouldEqual("A");

      parentClass = classDecl;
      classDecl = classDecl.NestedTypes[0] as ClassDeclarationNode;
      classDecl.ShouldNotBeNull();
      classDecl.Modifiers.Count.ShouldEqual(2);
      classDecl.Modifiers[0].Value.ShouldEqual(ModifierType.Public);
      classDecl.Modifiers[1].Value.ShouldEqual(ModifierType.New);
      classDecl.Name.ShouldEqual("C");
      classDecl.DeclaringType.ShouldBeSameAs(parentClass);

      classDecl = source.TypeDeclarations[2] as ClassDeclarationNode;
      classDecl.ShouldNotBeNull();
      classDecl.Modifiers.Count.ShouldEqual(2);
      classDecl.Modifiers[0].Value.ShouldEqual(ModifierType.Public);
      classDecl.Modifiers[1].Value.ShouldEqual(ModifierType.New);
      classDecl.Name.ShouldEqual("C");
      classDecl.DeclaringType.ShouldBeNull();
    }

    [TestMethod]
    public void ClassBoundariesAreOk()
    {
      var project = new CSharpProject(WorkingFolder);
      project.AddFile(@"ClassDeclaration\ClassDeclaration1.cs");
      InvokeParser(project).ShouldBeTrue();
      var source = project.SyntaxTree.SourceFileNodes[0];
      source.TypeDeclarations.Count.ShouldEqual(3);
      var classDecl = source.TypeDeclarations[0] as ClassDeclarationNode;
      classDecl.StartToken.Value.ShouldEqual("class");
      classDecl.StartToken.Line.ShouldEqual(1);
      classDecl.StartToken.Column.ShouldEqual(8);
      classDecl.TerminatingToken.Line.ShouldEqual(4);
      classDecl.TerminatingToken.Column.ShouldEqual(1);
      classDecl.Modifiers.Count.ShouldEqual(1);
      var mod = classDecl.Modifiers[0];
      mod.StartToken.ShouldBeSameAs(mod.TerminatingToken);
      mod.StartToken.Line.ShouldEqual(1);
      mod.StartToken.Column.ShouldEqual(1);
      classDecl.IdentifierToken.Line.ShouldEqual(1);
      classDecl.IdentifierToken.Column.ShouldEqual(14);
      classDecl.OpenBrace.Line.ShouldEqual(2);
      classDecl.OpenBrace.Column.ShouldEqual(1);
      classDecl.CloseBrace.Line.ShouldEqual(4);
      classDecl.CloseBrace.Column.ShouldEqual(1);

      var parentClass = source.TypeDeclarations[1] as ClassDeclarationNode;
      parentClass.ShouldNotBeNull();
      classDecl = parentClass.NestedTypes[0] as ClassDeclarationNode;
      classDecl.ShouldNotBeNull();
      classDecl.StartToken.Value.ShouldEqual("class");
      classDecl.StartToken.Line.ShouldEqual(8);
      classDecl.StartToken.Column.ShouldEqual(14);
      classDecl.TerminatingToken.Line.ShouldEqual(8);
      classDecl.TerminatingToken.Column.ShouldEqual(23);
      classDecl.Modifiers.Count.ShouldEqual(2);
      mod = classDecl.Modifiers[0];
      mod.StartToken.ShouldBeSameAs(mod.TerminatingToken);
      mod.StartToken.Line.ShouldEqual(8);
      mod.StartToken.Column.ShouldEqual(3);
      mod = classDecl.Modifiers[1];
      mod.StartToken.ShouldBeSameAs(mod.TerminatingToken);
      mod.StartToken.Line.ShouldEqual(8);
      mod.StartToken.Column.ShouldEqual(10);
      classDecl.IdentifierToken.Line.ShouldEqual(8);
      classDecl.IdentifierToken.Column.ShouldEqual(20);
      classDecl.OpenBrace.Line.ShouldEqual(8);
      classDecl.OpenBrace.Column.ShouldEqual(22);
      classDecl.CloseBrace.Line.ShouldEqual(8);
      classDecl.CloseBrace.Column.ShouldEqual(23);
    }

    [TestMethod]
    public void GenericClassDeclarationIsOk()
    {
      var project = new CSharpProject(WorkingFolder);
      project.AddFile(@"ClassDeclaration\ClassDeclaration8.cs");
      InvokeParser(project).ShouldBeTrue();
      var source = project.SyntaxTree.SourceFileNodes[0];
      source.ShouldNotBeNull();
      source.UsingNodes.Count.ShouldEqual(2);
      source.NamespaceDeclarations.Count.ShouldEqual(1);
      source.InScopeDeclarations.Count.ShouldEqual(2);
      var nsDecl = source.NamespaceDeclarations[0];
      nsDecl.ShouldNotBeNull();
      nsDecl.TypeDeclarations.Count.ShouldEqual(2);

      var classDecl = nsDecl.TypeDeclarations[0] as ClassDeclarationNode;
      classDecl.ShouldNotBeNull();
      classDecl.Modifiers.Count.ShouldEqual(1);
      classDecl.Modifiers[0].Value.ShouldEqual(ModifierType.Public);
      classDecl.Name.ShouldEqual("A");
      classDecl.BaseTypes.Count.ShouldEqual(2);
      var baseDecl = classDecl.BaseTypes[0];
      baseDecl.ShouldNotBeNull();
      baseDecl.TypeTags.Count.ShouldEqual(1);
      var tag = baseDecl.TypeTags[0];
      tag.ShouldNotBeNull();
      tag.Identifier.ShouldEqual("List");
      tag.Arguments.Count.ShouldEqual(1);
      tag.Arguments[0].TypeTags.Count.ShouldEqual(1);
      tag.Arguments[0].TypeTags[0].Identifier.ShouldEqual("string");
      tag.Arguments[0].TypeTags[0].HasTypeArguments.ShouldBeFalse();
      baseDecl = classDecl.BaseTypes[1];
      baseDecl.ShouldNotBeNull();
      baseDecl.TypeTags.Count.ShouldEqual(1);
      tag = baseDecl.TypeTags[0];
      tag.ShouldNotBeNull();
      tag.Identifier.ShouldEqual("IEnumerable");
      tag.Arguments.Count.ShouldEqual(1);
      tag.Arguments[0].TypeTags.Count.ShouldEqual(1);
      tag.Arguments[0].TypeTags[0].Identifier.ShouldEqual("string");
      tag.Arguments[0].TypeTags[0].HasTypeArguments.ShouldBeFalse();

      classDecl = nsDecl.TypeDeclarations[1] as ClassDeclarationNode;
      classDecl.ShouldNotBeNull();
      classDecl.Modifiers.Count.ShouldEqual(1);
      classDecl.Modifiers[0].Value.ShouldEqual(ModifierType.Public);
      classDecl.Name.ShouldEqual("B");
      classDecl.TypeParameters.Count.ShouldEqual(2);
      classDecl.TypeParameters[0].Identifier.ShouldEqual("X");
      classDecl.TypeParameters[1].Identifier.ShouldEqual("Y");

      classDecl.BaseTypes.Count.ShouldEqual(1);
      baseDecl = classDecl.BaseTypes[0];
      baseDecl.ShouldNotBeNull();
      baseDecl.TypeTags.Count.ShouldEqual(1);
      tag = baseDecl.TypeTags[0];
      tag.ShouldNotBeNull();
      tag.Identifier.ShouldEqual("Dictionary");
      tag.Arguments.Count.ShouldEqual(2);
      tag.Arguments[0].TypeTags.Count.ShouldEqual(1);
      tag.Arguments[0].TypeTags[0].Identifier.ShouldEqual("X");
      tag.Arguments[0].TypeTags[0].HasTypeArguments.ShouldBeFalse();
      tag.Arguments[1].TypeTags.Count.ShouldEqual(1);
      tag.Arguments[1].TypeTags[0].Identifier.ShouldEqual("Y");
      tag.Arguments[1].TypeTags[0].HasTypeArguments.ShouldBeFalse();

      classDecl.TypeParameterConstraints.Count.ShouldEqual(2);
      var constraint = classDecl.TypeParameterConstraints[0];
      constraint.Identifier.ShouldEqual("X");
      constraint.ConstraintTags.Count.ShouldEqual(3);
      var cTag = constraint.ConstraintTags[0];
      cTag.ConstraintToken.Value.ShouldEqual("class");
      cTag.IsClass.ShouldBeTrue();
      cTag = constraint.ConstraintTags[1];
      cTag.IsTypeName.ShouldBeTrue();
      cTag.TypeName.TypeTags.Count.ShouldEqual(1);
      cTag.TypeName.TypeTags[0].Identifier.ShouldEqual("IEnumerable");
      cTag.TypeName.TypeTags[0].Arguments.Count.ShouldEqual(1);
      cTag.TypeName.TypeTags[0].Arguments[0].TypeTags.Count.ShouldEqual(1);
      cTag.TypeName.TypeTags[0].Arguments[0].TypeTags[0].Identifier.ShouldEqual("Y");
      cTag = constraint.ConstraintTags[2];
      cTag.IsNew.ShouldBeTrue();
      constraint = classDecl.TypeParameterConstraints[1];
      constraint.Identifier.ShouldEqual("Y");
      constraint.ConstraintTags.Count.ShouldEqual(1);
      cTag = constraint.ConstraintTags[0];
      cTag.ConstraintToken.Value.ShouldEqual("struct");
      cTag.IsStruct.ShouldBeTrue();

      classDecl = source.TypeDeclarations[0] as ClassDeclarationNode;
      classDecl.ShouldNotBeNull();
      classDecl.Modifiers.Count.ShouldEqual(1);
      classDecl.Modifiers[0].Value.ShouldEqual(ModifierType.Public);
      classDecl.Name.ShouldEqual("C");
      classDecl.TypeParameters.Count.ShouldEqual(1);
      classDecl.TypeParameters[0].Identifier.ShouldEqual("Z");
      classDecl.BaseTypes.Count.ShouldEqual(1);
      baseDecl = classDecl.BaseTypes[0];
      baseDecl.ShouldNotBeNull();
      baseDecl.TypeTags.Count.ShouldEqual(4);
      tag = baseDecl.TypeTags[0];
      tag.ShouldNotBeNull();
      tag.Identifier.ShouldEqual("System");
      tag.HasTypeArguments.ShouldBeFalse();
      tag = baseDecl.TypeTags[1];
      tag.ShouldNotBeNull();
      tag.Identifier.ShouldEqual("Collections");
      tag.HasTypeArguments.ShouldBeFalse();
      tag = baseDecl.TypeTags[2];
      tag.ShouldNotBeNull();
      tag.Identifier.ShouldEqual("Generic");
      tag.HasTypeArguments.ShouldBeFalse();
      tag = baseDecl.TypeTags[3];
      tag.ShouldNotBeNull();
      tag.Identifier.ShouldEqual("List");
      tag.Arguments.Count.ShouldEqual(1);
      tag.Arguments[0].TypeTags.Count.ShouldEqual(1);
      tag.Arguments[0].TypeTags[0].Identifier.ShouldEqual("Z");
      tag.Arguments[0].TypeTags[0].HasTypeArguments.ShouldBeFalse();
      classDecl.TypeParameterConstraints.Count.ShouldEqual(1);
      
      constraint = classDecl.TypeParameterConstraints[0];
      constraint.Identifier.ShouldEqual("Z");
      constraint.ConstraintTags.Count.ShouldEqual(2);
      cTag = constraint.ConstraintTags[0];
      cTag.IsTypeName.ShouldBeTrue();
      cTag.TypeName.TypeTags.Count.ShouldEqual(1);
      cTag.TypeName.TypeTags[0].Identifier.ShouldEqual("Hashtable");
      cTag.TypeName.TypeTags[0].HasTypeArguments.ShouldBeFalse();
      cTag = constraint.ConstraintTags[1];
      cTag.IsTypeName.ShouldBeTrue();
      cTag.TypeName.TypeTags.Count.ShouldEqual(1);
      cTag.TypeName.TypeTags[0].Identifier.ShouldEqual("Dictionary");
      cTag.TypeName.TypeTags[0].Arguments.Count.ShouldEqual(2);
      tag = cTag.TypeName.TypeTags[0].Arguments[0].TypeTags[0];
      tag.Identifier.ShouldEqual("string");
      tag.HasTypeArguments.ShouldBeFalse();
      tag = cTag.TypeName.TypeTags[0].Arguments[1].TypeTags[0];
      tag.Identifier.ShouldEqual("IEnumerable");
      tag.Arguments.Count.ShouldEqual(1);
      tag.Arguments[0].TypeTags.Count.ShouldEqual(1);
      tag.Arguments[0].TypeTags[0].Identifier.ShouldEqual("string");
      tag.Arguments[0].TypeTags[0].HasTypeArguments.ShouldBeFalse();
    }

    [TestMethod]
    public void ClassSyntaxTreeWriterIsOk()
    {
      var project = new CSharpProject(WorkingFolder);
      project.AddFile(@"ClassDeclaration\ClassDeclaration8.cs");
      Assert.IsTrue(InvokeParser(project));
      var treeWriter = new SyntaxTreeTextWriter(project.SyntaxTree, project.ProjectProvider) { WorkingFolder = TempOutputFolder };
      treeWriter.WriteTree();
      project = new CSharpProject(TempOutputFolder);
      project.AddFile(@"ClassDeclaration\ClassDeclaration8.cs");
      Assert.IsTrue(InvokeParser(project));
    }


  }
}