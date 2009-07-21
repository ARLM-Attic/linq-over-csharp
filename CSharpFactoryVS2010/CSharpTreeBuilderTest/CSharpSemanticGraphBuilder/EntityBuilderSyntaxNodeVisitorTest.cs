using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SoftwareApproach.TestingExtensions;
using CSharpTreeBuilder.ProjectContent;
using CSharpTreeBuilder.CSharpSemanticGraph;
using CSharpTreeBuilder.CSharpSemanticGraphBuilder;

namespace CSharpTreeBuilderTest.CSharpSemanticGraphBuilder
{
  // ================================================================================================
  /// <summary>
  /// Tests the entity builder AST visitor
  /// </summary>
  // ================================================================================================
  [TestClass]
  public class EntityBuilderSyntaxNodeVisitorTest : ParserTestBed
  {
    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Tests the building of namespace entities
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    [TestMethod]
    public void BuildNamespaceEntities()
    {
      var project = new CSharpProject(WorkingFolder);
      project.AddFile(@"EntityBuilderSyntaxNodeVisitor\BuildNamespaceEntities.cs");
      InvokeParser(project).ShouldBeTrue();
      var semanticGraph = new SemanticGraph();
      var visitor = new EntityBuilderSyntaxNodeVisitor(semanticGraph, project);
      project.SyntaxTree.AcceptVisitor(visitor);

      semanticGraph.SemanticEntities.ToArray().Length.ShouldEqual(7);

      // global root namespace
      {
        var namespaceEntity = semanticGraph.SemanticEntities.ToArray()[0] as NamespaceEntity;
        (namespaceEntity is RootNamespaceEntity).ShouldBeTrue();
        namespaceEntity.Name.ShouldEqual("global");
        namespaceEntity.FullyQualifiedName.ShouldEqual("global");
        namespaceEntity.Parent.ShouldBeNull();
        namespaceEntity.IsExplicit.ShouldBeFalse();
        namespaceEntity.SyntaxNodes.Count.ShouldEqual(0);

        namespaceEntity.ChildNamespaces.Count.ShouldEqual(2);
        namespaceEntity.ChildNamespaces[0].FullyQualifiedName.ShouldEqual("global::A");
        namespaceEntity.ChildNamespaces[1].FullyQualifiedName.ShouldEqual("global::C");

        namespaceEntity.DeclarationSpace.NameCount.ShouldEqual(2);
        namespaceEntity.DeclarationSpace["A"].Entity.ShouldEqual(namespaceEntity.ChildNamespaces[0]);
        namespaceEntity.DeclarationSpace["C"].Entity.ShouldEqual(namespaceEntity.ChildNamespaces[1]);
      }
      // namespace global::A
      {
        var namespaceEntity = semanticGraph.SemanticEntities.ToArray()[1] as NamespaceEntity;
        namespaceEntity.Name.ShouldEqual("A");
        namespaceEntity.FullyQualifiedName.ShouldEqual("global::A");
        ((RootNamespaceEntity)namespaceEntity.Parent).FullyQualifiedName.ShouldEqual("global");
        namespaceEntity.IsExplicit.ShouldBeTrue();
        namespaceEntity.SyntaxNodes.Count.ShouldEqual(2);
        namespaceEntity.SyntaxNodes[0].ShouldEqual(project.SyntaxTree.CompilationUnitNodes[0].NamespaceDeclarations[0]);
        namespaceEntity.SyntaxNodes[1].ShouldEqual(project.SyntaxTree.CompilationUnitNodes[0].NamespaceDeclarations[2]);

        namespaceEntity.ChildNamespaces.Count.ShouldEqual(1);
        namespaceEntity.ChildNamespaces[0].FullyQualifiedName.ShouldEqual("global::A.B");

        namespaceEntity.DeclarationSpace.NameCount.ShouldEqual(1);
        namespaceEntity.DeclarationSpace["B"].Entity.ShouldEqual(namespaceEntity.ChildNamespaces[0]);
      }
      // namespace global::A.B
      {
        var namespaceEntity = semanticGraph.SemanticEntities.ToArray()[2] as NamespaceEntity;
        namespaceEntity.Name.ShouldEqual("B");
        namespaceEntity.FullyQualifiedName.ShouldEqual("global::A.B");
        ((NamespaceEntity)namespaceEntity.Parent).FullyQualifiedName.ShouldEqual("global::A");
        namespaceEntity.IsExplicit.ShouldBeTrue();
        namespaceEntity.SyntaxNodes.Count.ShouldEqual(2);
        namespaceEntity.SyntaxNodes[0].ShouldEqual(project.SyntaxTree.CompilationUnitNodes[0].NamespaceDeclarations[0].NamespaceDeclarations[0]);
        namespaceEntity.SyntaxNodes[1].ShouldEqual(project.SyntaxTree.CompilationUnitNodes[0].NamespaceDeclarations[2]);

        namespaceEntity.ChildNamespaces.Count.ShouldEqual(0);
        namespaceEntity.DeclarationSpace.NameCount.ShouldEqual(0);
      }
      // namespace global::C
      {
        var namespaceEntity = semanticGraph.SemanticEntities.ToArray()[3] as NamespaceEntity;
        namespaceEntity.Name.ShouldEqual("C");
        namespaceEntity.FullyQualifiedName.ShouldEqual("global::C");
        ((NamespaceEntity)namespaceEntity.Parent).FullyQualifiedName.ShouldEqual("global");
        namespaceEntity.IsExplicit.ShouldBeTrue();
        namespaceEntity.SyntaxNodes.Count.ShouldEqual(1);
        namespaceEntity.SyntaxNodes[0].ShouldEqual(project.SyntaxTree.CompilationUnitNodes[0].NamespaceDeclarations[1]);

        namespaceEntity.ChildNamespaces.Count.ShouldEqual(1);
        namespaceEntity.ChildNamespaces[0].FullyQualifiedName.ShouldEqual("global::C.D");

        namespaceEntity.DeclarationSpace.NameCount.ShouldEqual(1);
        namespaceEntity.DeclarationSpace["D"].Entity.ShouldEqual(namespaceEntity.ChildNamespaces[0]);
      }
      // namespace global::C.D
      {
        var namespaceEntity = semanticGraph.SemanticEntities.ToArray()[4] as NamespaceEntity;
        namespaceEntity.Name.ShouldEqual("D");
        namespaceEntity.FullyQualifiedName.ShouldEqual("global::C.D");
        ((NamespaceEntity)namespaceEntity.Parent).FullyQualifiedName.ShouldEqual("global::C");
        namespaceEntity.IsExplicit.ShouldBeTrue();
        namespaceEntity.SyntaxNodes.Count.ShouldEqual(1);
        namespaceEntity.SyntaxNodes[0].ShouldEqual(project.SyntaxTree.CompilationUnitNodes[0].NamespaceDeclarations[1]);

        namespaceEntity.ChildNamespaces.Count.ShouldEqual(1);
        namespaceEntity.ChildNamespaces[0].FullyQualifiedName.ShouldEqual("global::C.D.E");

        namespaceEntity.DeclarationSpace.NameCount.ShouldEqual(1);
        namespaceEntity.DeclarationSpace["E"].Entity.ShouldEqual(namespaceEntity.ChildNamespaces[0]);
      }
      // namespace global::C.D.E
      {
        var namespaceEntity = semanticGraph.SemanticEntities.ToArray()[5] as NamespaceEntity;
        namespaceEntity.Name.ShouldEqual("E");
        namespaceEntity.FullyQualifiedName.ShouldEqual("global::C.D.E");
        ((NamespaceEntity)namespaceEntity.Parent).FullyQualifiedName.ShouldEqual("global::C.D");
        namespaceEntity.IsExplicit.ShouldBeTrue();
        namespaceEntity.SyntaxNodes.Count.ShouldEqual(1);
        namespaceEntity.SyntaxNodes[0].ShouldEqual(project.SyntaxTree.CompilationUnitNodes[0].NamespaceDeclarations[1].NamespaceDeclarations[0]);

        namespaceEntity.ChildNamespaces.Count.ShouldEqual(1);
        namespaceEntity.ChildNamespaces[0].FullyQualifiedName.ShouldEqual("global::C.D.E.F");

        namespaceEntity.DeclarationSpace.NameCount.ShouldEqual(1);
        namespaceEntity.DeclarationSpace["F"].Entity.ShouldEqual(namespaceEntity.ChildNamespaces[0]);
      }
      // namespace global::C.D.E.F
      {
        var namespaceEntity = semanticGraph.SemanticEntities.ToArray()[6] as NamespaceEntity;
        namespaceEntity.Name.ShouldEqual("F");
        namespaceEntity.FullyQualifiedName.ShouldEqual("global::C.D.E.F");
        ((NamespaceEntity)namespaceEntity.Parent).FullyQualifiedName.ShouldEqual("global::C.D.E");
        namespaceEntity.IsExplicit.ShouldBeTrue();
        namespaceEntity.SyntaxNodes.Count.ShouldEqual(1);
        namespaceEntity.SyntaxNodes[0].ShouldEqual(project.SyntaxTree.CompilationUnitNodes[0].NamespaceDeclarations[1].NamespaceDeclarations[0]);

        namespaceEntity.ChildNamespaces.Count.ShouldEqual(0);
        namespaceEntity.DeclarationSpace.NameCount.ShouldEqual(0);
      }
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Tests the building of class entities
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    [TestMethod]
    public void BuildClassEntities()
    {
      var project = new CSharpProject(WorkingFolder);
      project.AddFile(@"EntityBuilderSyntaxNodeVisitor\BuildClassEntities.cs");
      InvokeParser(project).ShouldBeTrue();
      var semanticGraph = new SemanticGraph();
      var visitor = new EntityBuilderSyntaxNodeVisitor(semanticGraph, project);
      project.SyntaxTree.AcceptVisitor(visitor);

      semanticGraph.SemanticEntities.ToArray().Length.ShouldEqual(6);

      // global root namespace
      {
        var namespaceEntity = semanticGraph.SemanticEntities.ToArray()[0] as NamespaceEntity;
        namespaceEntity.FullyQualifiedName.ShouldEqual("global");

        namespaceEntity.ChildTypes.Count.ShouldEqual(1);
        namespaceEntity.ChildTypes[0].FullyQualifiedName.ShouldEqual("global::A");

        namespaceEntity.DeclarationSpace.NameCount.ShouldEqual(2);
        ((ClassEntity)namespaceEntity.DeclarationSpace["A"].Entity).FullyQualifiedName.ShouldEqual("global::A");
        ((NamespaceEntity)namespaceEntity.DeclarationSpace["C"].Entity).FullyQualifiedName.ShouldEqual("global::C");
      }
      // class A
      {
        var classEntity = semanticGraph.SemanticEntities.ToArray()[1] as ClassEntity;
        classEntity.FullyQualifiedName.ShouldEqual("global::A");
        classEntity.Name.ShouldEqual("A");
        ((RootNamespaceEntity)classEntity.Parent).FullyQualifiedName.ShouldEqual("global");
        classEntity.SyntaxNodes.Count.ShouldEqual(1);
        classEntity.SyntaxNodes[0].ShouldEqual(project.SyntaxTree.CompilationUnitNodes[0].TypeDeclarations[0]);

        classEntity.ChildTypes.Count.ShouldEqual(1);
        classEntity.ChildTypes[0].FullyQualifiedName.ShouldEqual("global::A.B");

        classEntity.DeclarationSpace.NameCount.ShouldEqual(1);
        ((ClassEntity)classEntity.DeclarationSpace["B"].Entity).FullyQualifiedName.ShouldEqual("global::A.B");
      }
      // class B
      {
        var classEntity = semanticGraph.SemanticEntities.ToArray()[2] as ClassEntity;
        classEntity.FullyQualifiedName.ShouldEqual("global::A.B");
        classEntity.Name.ShouldEqual("B");
        ((ClassEntity)classEntity.Parent).FullyQualifiedName.ShouldEqual("global::A");
        classEntity.SyntaxNodes.Count.ShouldEqual(1);
        classEntity.SyntaxNodes[0].ShouldEqual(project.SyntaxTree.CompilationUnitNodes[0].TypeDeclarations[0].NestedDeclarations[0]);

        classEntity.ChildTypes.Count.ShouldEqual(0);
        classEntity.DeclarationSpace.NameCount.ShouldEqual(0);
      }
      // namespace C
      {
        var namespaceEntity = semanticGraph.SemanticEntities.ToArray()[3] as NamespaceEntity;
        namespaceEntity.FullyQualifiedName.ShouldEqual("global::C");

        namespaceEntity.ChildTypes.Count.ShouldEqual(1);
        namespaceEntity.DeclarationSpace.NameCount.ShouldEqual(2);
      }
      // namespace C2
      {
        var namespaceEntity = semanticGraph.SemanticEntities.ToArray()[4] as NamespaceEntity;
        namespaceEntity.FullyQualifiedName.ShouldEqual("global::C.C2");

        namespaceEntity.ChildTypes.Count.ShouldEqual(0);
        namespaceEntity.DeclarationSpace.NameCount.ShouldEqual(0);
      }
      // class D
      {
        var classEntity = semanticGraph.SemanticEntities.ToArray()[5] as ClassEntity;
        classEntity.FullyQualifiedName.ShouldEqual("global::C.D");
        classEntity.Name.ShouldEqual("D");
        ((NamespaceEntity)classEntity.Parent).FullyQualifiedName.ShouldEqual("global::C");
        classEntity.SyntaxNodes.Count.ShouldEqual(1);
        classEntity.SyntaxNodes[0].ShouldEqual(project.SyntaxTree.CompilationUnitNodes[0].NamespaceDeclarations[0].TypeDeclarations[0]);

        classEntity.ChildTypes.Count.ShouldEqual(0);
        classEntity.DeclarationSpace.NameCount.ShouldEqual(0);

        classEntity.BaseTypes.Count.ShouldEqual(1);
        classEntity.BaseTypes[0].SyntaxNode.TypeTags[0].Identifier.ShouldEqual("A");
      }
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Tests the building of enum entities
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    [TestMethod]
    public void BuildEnumEntities()
    {
      var project = new CSharpProject(WorkingFolder);
      project.AddFile(@"EntityBuilderSyntaxNodeVisitor\BuildEnumEntities.cs");
      InvokeParser(project).ShouldBeTrue();
      var semanticGraph = new SemanticGraph();
      var visitor = new EntityBuilderSyntaxNodeVisitor(semanticGraph, project);
      project.SyntaxTree.AcceptVisitor(visitor);

      semanticGraph.SemanticEntities.ToArray().Length.ShouldEqual(4);

      // global root namespace
      {
        var namespaceEntity = semanticGraph.SemanticEntities.ToArray()[0] as NamespaceEntity;
        namespaceEntity.FullyQualifiedName.ShouldEqual("global");

        namespaceEntity.ChildTypes.Count.ShouldEqual(1);
        ((EnumEntity)namespaceEntity.ChildTypes[0]).FullyQualifiedName.ShouldEqual("global::A");

        namespaceEntity.DeclarationSpace.NameCount.ShouldEqual(2);
        ((EnumEntity)namespaceEntity.DeclarationSpace["A"].Entity).FullyQualifiedName.ShouldEqual("global::A");
        ((NamespaceEntity)namespaceEntity.DeclarationSpace["B"].Entity).FullyQualifiedName.ShouldEqual("global::B");
      }
      // enum A
      {
        var enumEntity = semanticGraph.SemanticEntities.ToArray()[1] as EnumEntity;
        enumEntity.FullyQualifiedName.ShouldEqual("global::A");
        enumEntity.Name.ShouldEqual("A");
        ((RootNamespaceEntity)enumEntity.Parent).FullyQualifiedName.ShouldEqual("global");
        enumEntity.SyntaxNodes.Count.ShouldEqual(1);
        enumEntity.SyntaxNodes[0].ShouldEqual(project.SyntaxTree.CompilationUnitNodes[0].TypeDeclarations[0]);

        enumEntity.DeclarationSpace.NameCount.ShouldEqual(0);
      }
      // namespace B
      {
        var namespaceEntity = semanticGraph.SemanticEntities.ToArray()[2] as NamespaceEntity;
        namespaceEntity.FullyQualifiedName.ShouldEqual("global::B");

        namespaceEntity.ChildTypes.Count.ShouldEqual(1);
        namespaceEntity.DeclarationSpace.NameCount.ShouldEqual(1);
      }
      // enum C
      {
        var enumEntity = semanticGraph.SemanticEntities.ToArray()[3] as EnumEntity;
        enumEntity.Name.ShouldEqual("C");
        enumEntity.FullyQualifiedName.ShouldEqual("global::B.C");
        ((NamespaceEntity)enumEntity.Parent).FullyQualifiedName.ShouldEqual("global::B");
        enumEntity.SyntaxNodes.Count.ShouldEqual(1);
        enumEntity.SyntaxNodes[0].ShouldEqual(project.SyntaxTree.CompilationUnitNodes[0].NamespaceDeclarations[0].TypeDeclarations[0]);

        enumEntity.DeclarationSpace.NameCount.ShouldEqual(0);
      }
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Tests the building of generic class entities
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    [TestMethod]
    public void BuildGenericClassEntities()
    {
      var project = new CSharpProject(WorkingFolder);
      project.AddFile(@"EntityBuilderSyntaxNodeVisitor\BuildGenericClassEntities.cs");
      InvokeParser(project).ShouldBeTrue();
      var semanticGraph = new SemanticGraph();
      var visitor = new EntityBuilderSyntaxNodeVisitor(semanticGraph, project);
      project.SyntaxTree.AcceptVisitor(visitor);

      var semanticEntities = semanticGraph.SemanticEntities.ToArray();
      semanticEntities.Length.ShouldEqual(4);

      // global root namespace
      {
        var namespaceEntity = semanticEntities[0] as NamespaceEntity;
        namespaceEntity.FullyQualifiedName.ShouldEqual("global");

        namespaceEntity.ChildTypes.Count.ShouldEqual(1);
        namespaceEntity.ChildTypes[0].FullyQualifiedName.ShouldEqual("global::A`2");

        namespaceEntity.DeclarationSpace.NameCount.ShouldEqual(2);
        ((NamespaceEntity)namespaceEntity.DeclarationSpace["A"].Entity).FullyQualifiedName.ShouldEqual("global::A");
        ((ClassEntity)namespaceEntity.DeclarationSpace["A`2"].Entity).FullyQualifiedName.ShouldEqual("global::A`2");
      }
      // class A<T1, T2>
      {
        var classEntity = semanticEntities[2] as ClassEntity;
        classEntity.FullyQualifiedName.ShouldEqual("global::A`2");

        classEntity.ChildTypes.Count.ShouldEqual(1);
        classEntity.ChildTypes[0].FullyQualifiedName.ShouldEqual("global::A`2.B`1");

        classEntity.DeclarationSpace.NameCount.ShouldEqual(3);
        ((TypeParameterEntity)classEntity.DeclarationSpace["T1"].Entity).FullyQualifiedName.ShouldEqual("T1");
        ((TypeParameterEntity)classEntity.DeclarationSpace["T2"].Entity).FullyQualifiedName.ShouldEqual("T2");
        ((ClassEntity)classEntity.DeclarationSpace["B`1"].Entity).FullyQualifiedName.ShouldEqual("global::A`2.B`1");
      }
      // class B<T3>
      {
        var classEntity = semanticEntities[3] as ClassEntity;
        classEntity.FullyQualifiedName.ShouldEqual("global::A`2.B`1");

        classEntity.ChildTypes.Count.ShouldEqual(0);

        classEntity.DeclarationSpace.NameCount.ShouldEqual(1);
        ((TypeParameterEntity)classEntity.DeclarationSpace["T3"].Entity).FullyQualifiedName.ShouldEqual("T3");
      }
    }
  }
}