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
      var visitor = new EntityBuilderSyntaxNodeVisitor(semanticGraph);
      project.SyntaxTree.AcceptVisitor(visitor);

      semanticGraph.SemanticEntitiesForUnitTest.Length.ShouldEqual(7);

      // global root namespace
      {
        var namespaceEntity = semanticGraph.SemanticEntitiesForUnitTest[0] as NamespaceEntity;
        (namespaceEntity is RootNamespaceEntity).ShouldBeTrue();
        namespaceEntity.Name.ShouldEqual("global");
        namespaceEntity.FullyQualifiedName.ShouldEqual("global");
        namespaceEntity.FqnWithRoot.ShouldEqual("global");
        namespaceEntity.Parent.ShouldBeNull();
        namespaceEntity.IsExplicit.ShouldBeFalse();
        namespaceEntity.SyntaxNodes.Count.ShouldEqual(0);

        namespaceEntity.ChildNamespacesForUnitTests.Count.ShouldEqual(2);
        namespaceEntity.ChildNamespacesForUnitTests[0].FqnWithRoot.ShouldEqual("global::A");
        namespaceEntity.ChildNamespacesForUnitTests[1].FqnWithRoot.ShouldEqual("global::C");

        namespaceEntity.DeclarationSpace.NameCount.ShouldEqual(2);
        namespaceEntity.DeclarationSpace["A"].Entity.ShouldEqual(namespaceEntity.ChildNamespacesForUnitTests[0]);
        namespaceEntity.DeclarationSpace["C"].Entity.ShouldEqual(namespaceEntity.ChildNamespacesForUnitTests[1]);
      }
      // namespace global::A
      {
        var namespaceEntity = semanticGraph.SemanticEntitiesForUnitTest[1] as NamespaceEntity;
        namespaceEntity.Name.ShouldEqual("A");
        namespaceEntity.FullyQualifiedName.ShouldEqual("A");
        namespaceEntity.FqnWithRoot.ShouldEqual("global::A");
        ((RootNamespaceEntity)namespaceEntity.Parent).FqnWithRoot.ShouldEqual("global");
        namespaceEntity.IsExplicit.ShouldBeTrue();
        namespaceEntity.SyntaxNodes.Count.ShouldEqual(2);
        namespaceEntity.SyntaxNodes[0].ShouldEqual(project.SyntaxTree.CompilationUnitNodes[0].NamespaceDeclarations[0]);
        namespaceEntity.SyntaxNodes[1].ShouldEqual(project.SyntaxTree.CompilationUnitNodes[0].NamespaceDeclarations[2]);

        namespaceEntity.ChildNamespacesForUnitTests.Count.ShouldEqual(1);
        namespaceEntity.ChildNamespacesForUnitTests[0].FqnWithRoot.ShouldEqual("global::A.B");

        namespaceEntity.DeclarationSpace.NameCount.ShouldEqual(1);
        namespaceEntity.DeclarationSpace["B"].Entity.ShouldEqual(namespaceEntity.ChildNamespacesForUnitTests[0]);
      }
      // namespace global::A.B
      {
        var namespaceEntity = semanticGraph.SemanticEntitiesForUnitTest[2] as NamespaceEntity;
        namespaceEntity.Name.ShouldEqual("B");
        namespaceEntity.FullyQualifiedName.ShouldEqual("A.B");
        namespaceEntity.FqnWithRoot.ShouldEqual("global::A.B");
        ((NamespaceEntity)namespaceEntity.Parent).FqnWithRoot.ShouldEqual("global::A");
        namespaceEntity.IsExplicit.ShouldBeTrue();
        namespaceEntity.SyntaxNodes.Count.ShouldEqual(2);
        namespaceEntity.SyntaxNodes[0].ShouldEqual(project.SyntaxTree.CompilationUnitNodes[0].NamespaceDeclarations[0].NamespaceDeclarations[0]);
        namespaceEntity.SyntaxNodes[1].ShouldEqual(project.SyntaxTree.CompilationUnitNodes[0].NamespaceDeclarations[2]);

        namespaceEntity.ChildNamespacesForUnitTests.Count.ShouldEqual(0);
        namespaceEntity.DeclarationSpace.NameCount.ShouldEqual(0);
      }
      // namespace global::C
      {
        var namespaceEntity = semanticGraph.SemanticEntitiesForUnitTest[3] as NamespaceEntity;
        namespaceEntity.Name.ShouldEqual("C");
        namespaceEntity.FullyQualifiedName.ShouldEqual("C");
        namespaceEntity.FqnWithRoot.ShouldEqual("global::C");
        ((NamespaceEntity)namespaceEntity.Parent).FqnWithRoot.ShouldEqual("global");
        namespaceEntity.IsExplicit.ShouldBeTrue();
        namespaceEntity.SyntaxNodes.Count.ShouldEqual(1);
        namespaceEntity.SyntaxNodes[0].ShouldEqual(project.SyntaxTree.CompilationUnitNodes[0].NamespaceDeclarations[1]);

        namespaceEntity.ChildNamespacesForUnitTests.Count.ShouldEqual(1);
        namespaceEntity.ChildNamespacesForUnitTests[0].FqnWithRoot.ShouldEqual("global::C.D");

        namespaceEntity.DeclarationSpace.NameCount.ShouldEqual(1);
        namespaceEntity.DeclarationSpace["D"].Entity.ShouldEqual(namespaceEntity.ChildNamespacesForUnitTests[0]);
      }
      // namespace global::C.D
      {
        var namespaceEntity = semanticGraph.SemanticEntitiesForUnitTest[4] as NamespaceEntity;
        namespaceEntity.Name.ShouldEqual("D");
        namespaceEntity.FullyQualifiedName.ShouldEqual("C.D");
        namespaceEntity.FqnWithRoot.ShouldEqual("global::C.D");
        ((NamespaceEntity)namespaceEntity.Parent).FqnWithRoot.ShouldEqual("global::C");
        namespaceEntity.IsExplicit.ShouldBeTrue();
        namespaceEntity.SyntaxNodes.Count.ShouldEqual(1);
        namespaceEntity.SyntaxNodes[0].ShouldEqual(project.SyntaxTree.CompilationUnitNodes[0].NamespaceDeclarations[1]);

        namespaceEntity.ChildNamespacesForUnitTests.Count.ShouldEqual(1);
        namespaceEntity.ChildNamespacesForUnitTests[0].FqnWithRoot.ShouldEqual("global::C.D.E");

        namespaceEntity.DeclarationSpace.NameCount.ShouldEqual(1);
        namespaceEntity.DeclarationSpace["E"].Entity.ShouldEqual(namespaceEntity.ChildNamespacesForUnitTests[0]);
      }
      // namespace global::C.D.E
      {
        var namespaceEntity = semanticGraph.SemanticEntitiesForUnitTest[5] as NamespaceEntity;
        namespaceEntity.Name.ShouldEqual("E");
        namespaceEntity.FullyQualifiedName.ShouldEqual("C.D.E");
        namespaceEntity.FqnWithRoot.ShouldEqual("global::C.D.E");
        ((NamespaceEntity)namespaceEntity.Parent).FqnWithRoot.ShouldEqual("global::C.D");
        namespaceEntity.IsExplicit.ShouldBeTrue();
        namespaceEntity.SyntaxNodes.Count.ShouldEqual(1);
        namespaceEntity.SyntaxNodes[0].ShouldEqual(project.SyntaxTree.CompilationUnitNodes[0].NamespaceDeclarations[1].NamespaceDeclarations[0]);

        namespaceEntity.ChildNamespacesForUnitTests.Count.ShouldEqual(1);
        namespaceEntity.ChildNamespacesForUnitTests[0].FqnWithRoot.ShouldEqual("global::C.D.E.F");

        namespaceEntity.DeclarationSpace.NameCount.ShouldEqual(1);
        namespaceEntity.DeclarationSpace["F"].Entity.ShouldEqual(namespaceEntity.ChildNamespacesForUnitTests[0]);
      }
      // namespace global::C.D.E.F
      {
        var namespaceEntity = semanticGraph.SemanticEntitiesForUnitTest[6] as NamespaceEntity;
        namespaceEntity.Name.ShouldEqual("F");
        namespaceEntity.FullyQualifiedName.ShouldEqual("C.D.E.F");
        namespaceEntity.FqnWithRoot.ShouldEqual("global::C.D.E.F");
        ((NamespaceEntity)namespaceEntity.Parent).FqnWithRoot.ShouldEqual("global::C.D.E");
        namespaceEntity.IsExplicit.ShouldBeTrue();
        namespaceEntity.SyntaxNodes.Count.ShouldEqual(1);
        namespaceEntity.SyntaxNodes[0].ShouldEqual(project.SyntaxTree.CompilationUnitNodes[0].NamespaceDeclarations[1].NamespaceDeclarations[0]);

        namespaceEntity.ChildNamespacesForUnitTests.Count.ShouldEqual(0);
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
      var visitor = new EntityBuilderSyntaxNodeVisitor(semanticGraph);
      project.SyntaxTree.AcceptVisitor(visitor);

      semanticGraph.SemanticEntitiesForUnitTest.Length.ShouldEqual(6);

      // global root namespace
      {
        var namespaceEntity = semanticGraph.SemanticEntitiesForUnitTest[0] as NamespaceEntity;
        namespaceEntity.FqnWithRoot.ShouldEqual("global");

        namespaceEntity.ChildTypesForUnitTests.Count.ShouldEqual(1);
        namespaceEntity.ChildTypesForUnitTests[0].FqnWithRoot.ShouldEqual("global::A");

        namespaceEntity.DeclarationSpace.NameCount.ShouldEqual(2);
        ((ClassEntity) namespaceEntity.DeclarationSpace["A"].Entity).FqnWithRoot.ShouldEqual("global::A");
        ((NamespaceEntity)namespaceEntity.DeclarationSpace["C"].Entity).FqnWithRoot.ShouldEqual("global::C");
      }
      // class A
      {
        var classEntity = semanticGraph.SemanticEntitiesForUnitTest[1] as ClassEntity;
        classEntity.FullyQualifiedName.ShouldEqual("A");
        classEntity.FqnWithRoot.ShouldEqual("global::A");
        classEntity.Name.ShouldEqual("A");
        ((RootNamespaceEntity) classEntity.Parent).FqnWithRoot.ShouldEqual("global");
        classEntity.SyntaxNodes.Count.ShouldEqual(1);
        classEntity.SyntaxNodes[0].ShouldEqual(project.SyntaxTree.CompilationUnitNodes[0].TypeDeclarations[0]);

        classEntity.ChildTypesForUnitTests.Count.ShouldEqual(1);
        classEntity.ChildTypesForUnitTests[0].FqnWithRoot.ShouldEqual("global::A.B");

        classEntity.DeclarationSpace.NameCount.ShouldEqual(1);
        ((ClassEntity)classEntity.DeclarationSpace["B"].Entity).FqnWithRoot.ShouldEqual("global::A.B");
      }
      // class B
      {
        var classEntity = semanticGraph.SemanticEntitiesForUnitTest[2] as ClassEntity;
        classEntity.FullyQualifiedName.ShouldEqual("A.B");
        classEntity.FqnWithRoot.ShouldEqual("global::A.B");
        classEntity.Name.ShouldEqual("B");
        ((ClassEntity)classEntity.Parent).FqnWithRoot.ShouldEqual("global::A");
        classEntity.SyntaxNodes.Count.ShouldEqual(1);
        classEntity.SyntaxNodes[0].ShouldEqual(project.SyntaxTree.CompilationUnitNodes[0].TypeDeclarations[0].NestedDeclarations[0]);

        classEntity.ChildTypesForUnitTests.Count.ShouldEqual(0);
        classEntity.DeclarationSpace.NameCount.ShouldEqual(0);
      }
      // namespace C
      {
        var namespaceEntity = semanticGraph.SemanticEntitiesForUnitTest[3] as NamespaceEntity;
        namespaceEntity.FqnWithRoot.ShouldEqual("global::C");

        namespaceEntity.ChildTypesForUnitTests.Count.ShouldEqual(1);
        namespaceEntity.DeclarationSpace.NameCount.ShouldEqual(2);
      }
      // namespace C2
      {
        var namespaceEntity = semanticGraph.SemanticEntitiesForUnitTest[4] as NamespaceEntity;
        namespaceEntity.FqnWithRoot.ShouldEqual("global::C.C2");

        namespaceEntity.ChildTypesForUnitTests.Count.ShouldEqual(0);
        namespaceEntity.DeclarationSpace.NameCount.ShouldEqual(0);
      }
      // class D
      {
        var classEntity = semanticGraph.SemanticEntitiesForUnitTest[5] as ClassEntity;
        classEntity.FullyQualifiedName.ShouldEqual("C.D");
        classEntity.FqnWithRoot.ShouldEqual("global::C.D");
        classEntity.Name.ShouldEqual("D");
        ((NamespaceEntity)classEntity.Parent).FqnWithRoot.ShouldEqual("global::C");
        classEntity.SyntaxNodes.Count.ShouldEqual(1);
        classEntity.SyntaxNodes[0].ShouldEqual(project.SyntaxTree.CompilationUnitNodes[0].NamespaceDeclarations[0].TypeDeclarations[0]);

        classEntity.ChildTypesForUnitTests.Count.ShouldEqual(0);
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
      var visitor = new EntityBuilderSyntaxNodeVisitor(semanticGraph);
      project.SyntaxTree.AcceptVisitor(visitor);

      semanticGraph.SemanticEntitiesForUnitTest.Length.ShouldEqual(4);

      // global root namespace
      {
        var namespaceEntity = semanticGraph.SemanticEntitiesForUnitTest[0] as NamespaceEntity;
        namespaceEntity.FqnWithRoot.ShouldEqual("global");

        namespaceEntity.ChildTypesForUnitTests.Count.ShouldEqual(1);
        ((EnumEntity)namespaceEntity.ChildTypesForUnitTests[0]).FqnWithRoot.ShouldEqual("global::A");

        namespaceEntity.DeclarationSpace.NameCount.ShouldEqual(2);
        ((EnumEntity) namespaceEntity.DeclarationSpace["A"].Entity).FqnWithRoot.ShouldEqual("global::A");
        ((NamespaceEntity) namespaceEntity.DeclarationSpace["B"].Entity).FqnWithRoot.ShouldEqual("global::B");
      }
      // enum A
      {
        var enumEntity = semanticGraph.SemanticEntitiesForUnitTest[1] as EnumEntity;
        enumEntity.FullyQualifiedName.ShouldEqual("A");
        enumEntity.FqnWithRoot.ShouldEqual("global::A");
        enumEntity.Name.ShouldEqual("A");
        ((RootNamespaceEntity) enumEntity.Parent).FqnWithRoot.ShouldEqual("global");
        enumEntity.SyntaxNodes.Count.ShouldEqual(1);
        enumEntity.SyntaxNodes[0].ShouldEqual(project.SyntaxTree.CompilationUnitNodes[0].TypeDeclarations[0]);

        enumEntity.ChildTypesForUnitTests.Count.ShouldEqual(0);
        enumEntity.DeclarationSpace.NameCount.ShouldEqual(0);
      }
      // namespace B
      {
        var namespaceEntity = semanticGraph.SemanticEntitiesForUnitTest[2] as NamespaceEntity;
        namespaceEntity.FqnWithRoot.ShouldEqual("global::B");

        namespaceEntity.ChildTypesForUnitTests.Count.ShouldEqual(1);
        namespaceEntity.DeclarationSpace.NameCount.ShouldEqual(1);
      }
      // enum C
      {
        var enumEntity = semanticGraph.SemanticEntitiesForUnitTest[3] as EnumEntity;
        enumEntity.Name.ShouldEqual("C");
        enumEntity.FullyQualifiedName.ShouldEqual("B.C");
        enumEntity.FqnWithRoot.ShouldEqual("global::B.C");
        ((NamespaceEntity)enumEntity.Parent).FqnWithRoot.ShouldEqual("global::B");
        enumEntity.SyntaxNodes.Count.ShouldEqual(1);
        enumEntity.SyntaxNodes[0].ShouldEqual(project.SyntaxTree.CompilationUnitNodes[0].NamespaceDeclarations[0].TypeDeclarations[0]);

        enumEntity.ChildTypesForUnitTests.Count.ShouldEqual(0);
        enumEntity.DeclarationSpace.NameCount.ShouldEqual(0);
      }
    }
  }
}