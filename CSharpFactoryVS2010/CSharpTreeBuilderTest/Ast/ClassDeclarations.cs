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
  }
}