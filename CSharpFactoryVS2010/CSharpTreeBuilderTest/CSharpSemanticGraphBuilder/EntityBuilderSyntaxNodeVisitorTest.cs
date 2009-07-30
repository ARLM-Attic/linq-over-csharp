using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SoftwareApproach.TestingExtensions;
using CSharpTreeBuilder.ProjectContent;
using CSharpTreeBuilder.CSharpSemanticGraph;
using CSharpTreeBuilder.CSharpSemanticGraphBuilder;
using CSharpTreeBuilder.Ast;

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
      InvokeParser(project, true, false).ShouldBeTrue();
      var semanticGraph = new SemanticGraph();
      var visitor = new EntityBuilderSyntaxNodeVisitor(project, semanticGraph);
      project.SyntaxTree.AcceptVisitor(visitor);

      // global root namespace
      {
        var namespaceEntity = semanticGraph.GlobalNamespace;
        (namespaceEntity is RootNamespaceEntity).ShouldBeTrue();
        namespaceEntity.Name.ShouldEqual("global");
        namespaceEntity.FullyQualifiedName.ShouldEqual("global");
        namespaceEntity.Parent.ShouldBeNull();
        namespaceEntity.IsExplicit.ShouldBeFalse();
        namespaceEntity.SyntaxNodes.Count.ShouldEqual(0);

        namespaceEntity.ChildNamespaces.Count.ShouldEqual(2);
        namespaceEntity.ChildNamespaces[0].FullyQualifiedName.ShouldEqual("A");
        namespaceEntity.ChildNamespaces[1].FullyQualifiedName.ShouldEqual("C");

        namespaceEntity.DeclarationSpace.NameCount.ShouldEqual(2);
        namespaceEntity.DeclarationSpace["A"].Entity.ShouldEqual(namespaceEntity.ChildNamespaces[0]);
        namespaceEntity.DeclarationSpace["C"].Entity.ShouldEqual(namespaceEntity.ChildNamespaces[1]);
      }
      // namespace global::A
      {
        var namespaceEntity = semanticGraph.GlobalNamespace.ChildNamespaces[0];
        namespaceEntity.Name.ShouldEqual("A");
        namespaceEntity.FullyQualifiedName.ShouldEqual("A");
        ((RootNamespaceEntity)namespaceEntity.Parent).FullyQualifiedName.ShouldEqual("global");
        namespaceEntity.IsExplicit.ShouldBeTrue();
        namespaceEntity.SyntaxNodes.Count.ShouldEqual(2);
        namespaceEntity.SyntaxNodes[0].ShouldEqual(project.SyntaxTree.CompilationUnitNodes[0].NamespaceDeclarations[0]);
        namespaceEntity.SyntaxNodes[1].ShouldEqual(project.SyntaxTree.CompilationUnitNodes[0].NamespaceDeclarations[2]);

        namespaceEntity.ChildNamespaces.Count.ShouldEqual(1);
        namespaceEntity.ChildNamespaces[0].FullyQualifiedName.ShouldEqual("A.B");

        namespaceEntity.DeclarationSpace.NameCount.ShouldEqual(1);
        namespaceEntity.DeclarationSpace["B"].Entity.ShouldEqual(namespaceEntity.ChildNamespaces[0]);
      }
      // namespace global::A.B
      {
        var namespaceEntity = semanticGraph.GlobalNamespace.ChildNamespaces[0].ChildNamespaces[0];
        namespaceEntity.Name.ShouldEqual("B");
        namespaceEntity.FullyQualifiedName.ShouldEqual("A.B");
        ((NamespaceEntity)namespaceEntity.Parent).FullyQualifiedName.ShouldEqual("A");
        namespaceEntity.IsExplicit.ShouldBeTrue();
        namespaceEntity.SyntaxNodes.Count.ShouldEqual(2);
        namespaceEntity.SyntaxNodes[0].ShouldEqual(project.SyntaxTree.CompilationUnitNodes[0].NamespaceDeclarations[0].NamespaceDeclarations[0]);
        namespaceEntity.SyntaxNodes[1].ShouldEqual(project.SyntaxTree.CompilationUnitNodes[0].NamespaceDeclarations[2]);

        namespaceEntity.ChildNamespaces.Count.ShouldEqual(0);
        namespaceEntity.DeclarationSpace.NameCount.ShouldEqual(0);
      }
      // namespace global::C
      {
        var namespaceEntity = semanticGraph.GlobalNamespace.ChildNamespaces[1];
        namespaceEntity.Name.ShouldEqual("C");
        namespaceEntity.FullyQualifiedName.ShouldEqual("C");
        ((NamespaceEntity)namespaceEntity.Parent).FullyQualifiedName.ShouldEqual("global");
        namespaceEntity.IsExplicit.ShouldBeTrue();
        namespaceEntity.SyntaxNodes.Count.ShouldEqual(1);
        namespaceEntity.SyntaxNodes[0].ShouldEqual(project.SyntaxTree.CompilationUnitNodes[0].NamespaceDeclarations[1]);

        namespaceEntity.ChildNamespaces.Count.ShouldEqual(1);
        namespaceEntity.ChildNamespaces[0].FullyQualifiedName.ShouldEqual("C.D");

        namespaceEntity.DeclarationSpace.NameCount.ShouldEqual(1);
        namespaceEntity.DeclarationSpace["D"].Entity.ShouldEqual(namespaceEntity.ChildNamespaces[0]);
      }
      // namespace global::C.D
      {
        var namespaceEntity = semanticGraph.GlobalNamespace.ChildNamespaces[1].ChildNamespaces[0];
        namespaceEntity.Name.ShouldEqual("D");
        namespaceEntity.FullyQualifiedName.ShouldEqual("C.D");
        ((NamespaceEntity)namespaceEntity.Parent).FullyQualifiedName.ShouldEqual("C");
        namespaceEntity.IsExplicit.ShouldBeTrue();
        namespaceEntity.SyntaxNodes.Count.ShouldEqual(1);
        namespaceEntity.SyntaxNodes[0].ShouldEqual(project.SyntaxTree.CompilationUnitNodes[0].NamespaceDeclarations[1]);

        namespaceEntity.ChildNamespaces.Count.ShouldEqual(1);
        namespaceEntity.ChildNamespaces[0].FullyQualifiedName.ShouldEqual("C.D.E");

        namespaceEntity.DeclarationSpace.NameCount.ShouldEqual(1);
        namespaceEntity.DeclarationSpace["E"].Entity.ShouldEqual(namespaceEntity.ChildNamespaces[0]);
      }
      // namespace global::C.D.E
      {
        var namespaceEntity = semanticGraph.GlobalNamespace.ChildNamespaces[1].ChildNamespaces[0].ChildNamespaces[0];
        namespaceEntity.Name.ShouldEqual("E");
        namespaceEntity.FullyQualifiedName.ShouldEqual("C.D.E");
        ((NamespaceEntity)namespaceEntity.Parent).FullyQualifiedName.ShouldEqual("C.D");
        namespaceEntity.IsExplicit.ShouldBeTrue();
        namespaceEntity.SyntaxNodes.Count.ShouldEqual(1);
        namespaceEntity.SyntaxNodes[0].ShouldEqual(project.SyntaxTree.CompilationUnitNodes[0].NamespaceDeclarations[1].NamespaceDeclarations[0]);

        namespaceEntity.ChildNamespaces.Count.ShouldEqual(1);
        namespaceEntity.ChildNamespaces[0].FullyQualifiedName.ShouldEqual("C.D.E.F");

        namespaceEntity.DeclarationSpace.NameCount.ShouldEqual(1);
        namespaceEntity.DeclarationSpace["F"].Entity.ShouldEqual(namespaceEntity.ChildNamespaces[0]);
      }
      // namespace global::C.D.E.F
      {
        var namespaceEntity =
          semanticGraph.GlobalNamespace.ChildNamespaces[1].ChildNamespaces[0].ChildNamespaces[0].ChildNamespaces[0];
        namespaceEntity.Name.ShouldEqual("F");
        namespaceEntity.FullyQualifiedName.ShouldEqual("C.D.E.F");
        ((NamespaceEntity)namespaceEntity.Parent).FullyQualifiedName.ShouldEqual("C.D.E");
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
      InvokeParser(project, true, false).ShouldBeTrue();
      var semanticGraph = new SemanticGraph();
      var visitor = new EntityBuilderSyntaxNodeVisitor(project, semanticGraph);
      project.SyntaxTree.AcceptVisitor(visitor);

      // global root namespace
      {
        var namespaceEntity = semanticGraph.GlobalNamespace;
        namespaceEntity.FullyQualifiedName.ShouldEqual("global");

        namespaceEntity.ChildTypes.Count.ShouldEqual(1);
        namespaceEntity.ChildTypes[0].FullyQualifiedName.ShouldEqual("A");

        namespaceEntity.DeclarationSpace.NameCount.ShouldEqual(2);
        ((ClassEntity)namespaceEntity.DeclarationSpace["A"].Entity).FullyQualifiedName.ShouldEqual("A");
        ((NamespaceEntity)namespaceEntity.DeclarationSpace["C"].Entity).FullyQualifiedName.ShouldEqual("C");
      }
      // class A
      {
        var classEntity = semanticGraph.GlobalNamespace.ChildTypes[0] as ClassEntity;
        classEntity.FullyQualifiedName.ShouldEqual("A");
        classEntity.Name.ShouldEqual("A");
        ((RootNamespaceEntity)classEntity.Parent).FullyQualifiedName.ShouldEqual("global");
        classEntity.SyntaxNodes.Count.ShouldEqual(1);
        classEntity.SyntaxNodes[0].ShouldEqual(project.SyntaxTree.CompilationUnitNodes[0].TypeDeclarations[0]);

        classEntity.ChildTypes.Count.ShouldEqual(1);
        classEntity.ChildTypes[0].FullyQualifiedName.ShouldEqual("A.B");

        classEntity.DeclarationSpace.NameCount.ShouldEqual(1);
        ((ClassEntity)classEntity.DeclarationSpace["B"].Entity).FullyQualifiedName.ShouldEqual("A.B");

        classEntity.IsGeneric.ShouldBeFalse();
        classEntity.IsPointerType.ShouldBeFalse();
        classEntity.IsReferenceType.ShouldBeTrue();
        classEntity.IsValueType.ShouldBeFalse();
      }
      // class B
      {
        var classEntity = ((ClassEntity)semanticGraph.GlobalNamespace.ChildTypes[0]).ChildTypes[0] as ClassEntity;
        classEntity.FullyQualifiedName.ShouldEqual("A.B");
        classEntity.Name.ShouldEqual("B");
        ((ClassEntity)classEntity.Parent).FullyQualifiedName.ShouldEqual("A");
        classEntity.SyntaxNodes.Count.ShouldEqual(1);
        classEntity.SyntaxNodes[0].ShouldEqual(project.SyntaxTree.CompilationUnitNodes[0].TypeDeclarations[0].NestedDeclarations[0]);

        classEntity.ChildTypes.Count.ShouldEqual(0);
        classEntity.DeclarationSpace.NameCount.ShouldEqual(0);
      }
      // namespace C
      {
        var namespaceEntity = semanticGraph.GlobalNamespace.ChildNamespaces[0];
        namespaceEntity.FullyQualifiedName.ShouldEqual("C");

        namespaceEntity.ChildTypes.Count.ShouldEqual(1);
        namespaceEntity.DeclarationSpace.NameCount.ShouldEqual(2);
      }
      // namespace C2
      {
        var namespaceEntity = semanticGraph.GlobalNamespace.ChildNamespaces[0].ChildNamespaces[0];
        namespaceEntity.FullyQualifiedName.ShouldEqual("C.C2");

        namespaceEntity.ChildTypes.Count.ShouldEqual(0);
        namespaceEntity.DeclarationSpace.NameCount.ShouldEqual(0);
      }
      // class D
      {
        var classEntity = semanticGraph.GlobalNamespace.ChildNamespaces[0].ChildTypes[0] as ClassEntity;
        classEntity.FullyQualifiedName.ShouldEqual("C.D");
        classEntity.Name.ShouldEqual("D");
        ((NamespaceEntity)classEntity.Parent).FullyQualifiedName.ShouldEqual("C");
        classEntity.SyntaxNodes.Count.ShouldEqual(1);
        classEntity.SyntaxNodes[0].ShouldEqual(project.SyntaxTree.CompilationUnitNodes[0].NamespaceDeclarations[0].TypeDeclarations[0]);

        classEntity.ChildTypes.Count.ShouldEqual(0);
        classEntity.DeclarationSpace.NameCount.ShouldEqual(0);

        classEntity.BaseTypes.Count().ShouldEqual(1);
        ((TypeOrNamespaceNodeBasedTypeEntityReference)classEntity.BaseTypes.ToArray()[0]).SyntaxNode.TypeTags[0].Identifier.ShouldEqual("A");
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
      InvokeParser(project, true, false).ShouldBeTrue();
      var semanticGraph = new SemanticGraph();
      var visitor = new EntityBuilderSyntaxNodeVisitor(project, semanticGraph);
      project.SyntaxTree.AcceptVisitor(visitor);

      // global root namespace
      {
        var namespaceEntity = semanticGraph.GlobalNamespace;
        namespaceEntity.FullyQualifiedName.ShouldEqual("global");
        namespaceEntity.ChildNamespaces.Count.ShouldEqual(1);
        namespaceEntity.ChildNamespaces[0].FullyQualifiedName.ShouldEqual("A");
      }
      // enum A
      {
        var enumEntity = semanticGraph.GlobalNamespace.ChildNamespaces[0].ChildTypes[0] as EnumEntity;
        enumEntity.Name.ShouldEqual("B");
        enumEntity.DistinctiveName.ShouldEqual("B");
        enumEntity.FullyQualifiedName.ShouldEqual("A.B");
        ((NamespaceEntity)enumEntity.Parent).FullyQualifiedName.ShouldEqual("A");
        enumEntity.SyntaxNodes.Count.ShouldEqual(1);
        enumEntity.SyntaxNodes[0].ShouldEqual(project.SyntaxTree.CompilationUnitNodes[0].NamespaceDeclarations[0].TypeDeclarations[0]);

        enumEntity.IsPointerType.ShouldBeFalse();
        enumEntity.IsReferenceType.ShouldBeFalse();
        enumEntity.IsValueType.ShouldBeTrue();

        enumEntity.DeclarationSpace.NameCount.ShouldEqual(0);

        var baseTypes = enumEntity.BaseTypes.ToArray();
        baseTypes.Length.ShouldEqual(1);
        baseTypes[0].ResolutionState.ShouldEqual(ResolutionState.NotYetResolved);
        ((TypeOrNamespaceNodeBasedTypeEntityReference)baseTypes[0]).SyntaxNode.ShouldEqual(
          project.SyntaxTree.CompilationUnitNodes[0].NamespaceDeclarations[0].TypeDeclarations[0].BaseTypes[0]);
      }
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Tests the building of struct entities
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    [TestMethod]
    public void BuildStructEntities()
    {
      var project = new CSharpProject(WorkingFolder);
      project.AddFile(@"EntityBuilderSyntaxNodeVisitor\BuildStructEntities.cs");
      InvokeParser(project, true, false).ShouldBeTrue();
      var semanticGraph = new SemanticGraph();
      var visitor = new EntityBuilderSyntaxNodeVisitor(project, semanticGraph);
      project.SyntaxTree.AcceptVisitor(visitor);

      // global root namespace
      {
        var namespaceEntity = semanticGraph.GlobalNamespace;
        namespaceEntity.FullyQualifiedName.ShouldEqual("global");
      }
      // namespace A
      {
        var namespaceEntity = semanticGraph.GlobalNamespace.ChildNamespaces[0];
        namespaceEntity.FullyQualifiedName.ShouldEqual("A");
        namespaceEntity.ChildTypes.Count.ShouldEqual(1);
        namespaceEntity.ChildTypes[0].FullyQualifiedName.ShouldEqual("A.B");
        namespaceEntity.DeclarationSpace.NameCount.ShouldEqual(1);
        ((StructEntity)namespaceEntity.DeclarationSpace["B"].Entity).FullyQualifiedName.ShouldEqual("A.B");
      }
      // struct B
      {
        var structEntity = semanticGraph.GlobalNamespace.ChildNamespaces[0].ChildTypes[0] as StructEntity;
        structEntity.Name.ShouldEqual("B");
        structEntity.DistinctiveName.ShouldEqual("B");
        structEntity.FullyQualifiedName.ShouldEqual("A.B");
        ((NamespaceEntity)structEntity.Parent).FullyQualifiedName.ShouldEqual("A");
        structEntity.SyntaxNodes.Count.ShouldEqual(1);
        structEntity.SyntaxNodes[0].ShouldEqual(project.SyntaxTree.CompilationUnitNodes[0].NamespaceDeclarations[0].TypeDeclarations[0]);

        structEntity.IsGeneric.ShouldBeFalse();
        structEntity.IsPointerType.ShouldBeFalse();
        structEntity.IsReferenceType.ShouldBeFalse();
        structEntity.IsValueType.ShouldBeTrue();

        structEntity.DeclarationSpace.NameCount.ShouldEqual(0);

        var baseTypes = structEntity.BaseTypes.ToArray();
        baseTypes.Length.ShouldEqual(1);
        baseTypes[0].ResolutionState.ShouldEqual(ResolutionState.NotYetResolved);
        ((TypeOrNamespaceNodeBasedTypeEntityReference)baseTypes[0]).SyntaxNode.ShouldEqual(
          project.SyntaxTree.CompilationUnitNodes[0].NamespaceDeclarations[0].TypeDeclarations[0].BaseTypes[0]);
      }
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Tests the building of interface entities
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    [TestMethod]
    public void BuildInterfaceEntities()
    {
      var project = new CSharpProject(WorkingFolder);
      project.AddFile(@"EntityBuilderSyntaxNodeVisitor\BuildInterfaceEntities.cs");
      InvokeParser(project, true, false).ShouldBeTrue();
      var semanticGraph = new SemanticGraph();
      var visitor = new EntityBuilderSyntaxNodeVisitor(project, semanticGraph);
      project.SyntaxTree.AcceptVisitor(visitor);

      // global root namespace
      {
        var namespaceEntity = semanticGraph.GlobalNamespace;
        namespaceEntity.FullyQualifiedName.ShouldEqual("global");
      }
      // namespace A
      {
        var namespaceEntity = semanticGraph.GlobalNamespace.ChildNamespaces[0];
        namespaceEntity.FullyQualifiedName.ShouldEqual("A");
        namespaceEntity.ChildTypes.Count.ShouldEqual(1);
        namespaceEntity.ChildTypes[0].FullyQualifiedName.ShouldEqual("A.B");
        namespaceEntity.DeclarationSpace.NameCount.ShouldEqual(1);
        ((InterfaceEntity)namespaceEntity.DeclarationSpace["B"].Entity).FullyQualifiedName.ShouldEqual("A.B");
      }
      // interface B
      {
        var interfaceEntity = semanticGraph.GlobalNamespace.ChildNamespaces[0].ChildTypes[0] as InterfaceEntity;
        interfaceEntity.Name.ShouldEqual("B");
        interfaceEntity.DistinctiveName.ShouldEqual("B");
        interfaceEntity.FullyQualifiedName.ShouldEqual("A.B");
        ((NamespaceEntity)interfaceEntity.Parent).FullyQualifiedName.ShouldEqual("A");
        interfaceEntity.SyntaxNodes.Count.ShouldEqual(1);
        interfaceEntity.SyntaxNodes[0].ShouldEqual(project.SyntaxTree.CompilationUnitNodes[0].NamespaceDeclarations[0].TypeDeclarations[0]);

        interfaceEntity.IsGeneric.ShouldBeFalse();
        interfaceEntity.IsPointerType.ShouldBeFalse();
        interfaceEntity.IsReferenceType.ShouldBeTrue();
        interfaceEntity.IsValueType.ShouldBeFalse();

        interfaceEntity.DeclarationSpace.NameCount.ShouldEqual(0);

        var baseTypes = interfaceEntity.BaseTypes.ToArray();
        baseTypes.Length.ShouldEqual(1);
        baseTypes[0].ResolutionState.ShouldEqual(ResolutionState.NotYetResolved);
        ((TypeOrNamespaceNodeBasedTypeEntityReference) baseTypes[0]).SyntaxNode.ShouldEqual(
          project.SyntaxTree.CompilationUnitNodes[0].NamespaceDeclarations[0].TypeDeclarations[0].BaseTypes[0]);
      }
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Tests the building of delegate entities
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    [TestMethod]
    public void BuildDelegateEntities()
    {
      var project = new CSharpProject(WorkingFolder);
      project.AddFile(@"EntityBuilderSyntaxNodeVisitor\BuildDelegateEntities.cs");
      InvokeParser(project, true, false).ShouldBeTrue();
      var semanticGraph = new SemanticGraph();
      var visitor = new EntityBuilderSyntaxNodeVisitor(project, semanticGraph);
      project.SyntaxTree.AcceptVisitor(visitor);

      // global root namespace
      {
        var namespaceEntity = semanticGraph.GlobalNamespace;
        namespaceEntity.FullyQualifiedName.ShouldEqual("global");
        namespaceEntity.ChildNamespaces.Count.ShouldEqual(1);
        namespaceEntity.ChildNamespaces[0].FullyQualifiedName.ShouldEqual("A");
        namespaceEntity.DeclarationSpace.NameCount.ShouldEqual(1);
        ((NamespaceEntity)namespaceEntity.DeclarationSpace["A"].Entity).FullyQualifiedName.ShouldEqual("A");
      }
      // namespace A
      {
        var namespaceEntity = semanticGraph.GlobalNamespace.ChildNamespaces[0];
        namespaceEntity.FullyQualifiedName.ShouldEqual("A");
        namespaceEntity.ChildTypes.Count.ShouldEqual(1);
        namespaceEntity.ChildTypes[0].FullyQualifiedName.ShouldEqual("A.B");
        namespaceEntity.DeclarationSpace.NameCount.ShouldEqual(1);
        ((DelegateEntity)namespaceEntity.DeclarationSpace["B"].Entity).FullyQualifiedName.ShouldEqual("A.B");
      }
      // delegate B
      {
        var delegateEntity = semanticGraph.GlobalNamespace.ChildNamespaces[0].ChildTypes[0] as DelegateEntity;
        delegateEntity.Name.ShouldEqual("B");
        delegateEntity.DistinctiveName.ShouldEqual("B");
        delegateEntity.FullyQualifiedName.ShouldEqual("A.B");
        ((NamespaceEntity) delegateEntity.Parent).FullyQualifiedName.ShouldEqual("A");
        delegateEntity.SyntaxNodes.Count.ShouldEqual(1);
        delegateEntity.SyntaxNodes[0].ShouldEqual(project.SyntaxTree.CompilationUnitNodes[0].NamespaceDeclarations[0].TypeDeclarations[0]);

        delegateEntity.DeclarationSpace.NameCount.ShouldEqual(0);

        delegateEntity.IsPointerType.ShouldBeFalse();
        delegateEntity.IsReferenceType.ShouldBeTrue();
        delegateEntity.IsValueType.ShouldBeFalse();
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
      InvokeParser(project, true, false).ShouldBeTrue();
      var semanticGraph = new SemanticGraph();
      var visitor = new EntityBuilderSyntaxNodeVisitor(project, semanticGraph);
      project.SyntaxTree.AcceptVisitor(visitor);

      // global root namespace
      {
        var namespaceEntity = semanticGraph.GlobalNamespace as NamespaceEntity;
        namespaceEntity.ChildTypes.Count.ShouldEqual(1);
        namespaceEntity.ChildTypes[0].FullyQualifiedName.ShouldEqual("A`2");

        namespaceEntity.DeclarationSpace.NameCount.ShouldEqual(2);
        ((NamespaceEntity)namespaceEntity.DeclarationSpace["A"].Entity).FullyQualifiedName.ShouldEqual("A");
        ((ClassEntity)namespaceEntity.DeclarationSpace["A`2"].Entity).FullyQualifiedName.ShouldEqual("A`2");
      }
      // type param T1
      {
        var typeParam = (semanticGraph.GlobalNamespace.ChildTypes[0] as ClassEntity).TypeParameters.ToArray()[0];
        typeParam.Name.ShouldEqual("T1");
        typeParam.DistinctiveName.ShouldEqual("T1");
        typeParam.FullyQualifiedName.ShouldEqual("A`2.T1");
        typeParam.IsPointerType.ShouldBeFalse();
        typeParam.IsReferenceType.ShouldBeFalse();
        typeParam.IsValueType.ShouldBeFalse();
        typeParam.BaseTypes.Count().ShouldEqual(0);
        typeParam.DeclarationSpace.NameCount.ShouldEqual(0);
        typeParam.Members.Count().ShouldEqual(0);
        typeParam.Parent.ShouldEqual(semanticGraph.GlobalNamespace.ChildTypes[0]);
        typeParam.SyntaxNodes.Count.ShouldEqual(1);
        typeParam.SyntaxNodes[0].ShouldEqual(
          project.SyntaxTree.CompilationUnitNodes[0].TypeDeclarations[0].TypeParameters[0]);
      }
      // type param T2
      {
        var typeParamEntity = (semanticGraph.GlobalNamespace.ChildTypes[0] as ClassEntity).TypeParameters.ToArray()[1];
        typeParamEntity.FullyQualifiedName.ShouldEqual("A`2.T2");
        ((ClassEntity)typeParamEntity.Parent).FullyQualifiedName.ShouldEqual("A`2");
      }
      // class A<T1, T2>
      {
        var classEntity = semanticGraph.GlobalNamespace.ChildTypes[0] as ClassEntity;
        classEntity.FullyQualifiedName.ShouldEqual("A`2");

        classEntity.ChildTypes.Count.ShouldEqual(1);
        classEntity.ChildTypes[0].FullyQualifiedName.ShouldEqual("A`2.B`1");

        classEntity.DeclarationSpace.NameCount.ShouldEqual(3);
        ((TypeParameterEntity)classEntity.DeclarationSpace["T1"].Entity).FullyQualifiedName.ShouldEqual("A`2.T1");
        ((TypeParameterEntity)classEntity.DeclarationSpace["T2"].Entity).FullyQualifiedName.ShouldEqual("A`2.T2");
        ((ClassEntity)classEntity.DeclarationSpace["B`1"].Entity).FullyQualifiedName.ShouldEqual("A`2.B`1");
      }
      // type param T3
      {
        var typeParamEntity =
          ((semanticGraph.GlobalNamespace.ChildTypes[0] as ClassEntity).ChildTypes[0] as ClassEntity).TypeParameters.ToArray()[0];
        typeParamEntity.FullyQualifiedName.ShouldEqual("A`2.B`1.T3");
        ((ClassEntity)typeParamEntity.Parent).FullyQualifiedName.ShouldEqual("A`2.B`1");
      }
      // class B<T3>
      {
        var classEntity = (semanticGraph.GlobalNamespace.ChildTypes[0] as ClassEntity).ChildTypes[0] as ClassEntity;
        classEntity.FullyQualifiedName.ShouldEqual("A`2.B`1");

        classEntity.ChildTypes.Count.ShouldEqual(0);

        classEntity.DeclarationSpace.NameCount.ShouldEqual(1);
        ((TypeParameterEntity)classEntity.DeclarationSpace["T3"].Entity).FullyQualifiedName.ShouldEqual("A`2.B`1.T3");
      }
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Tests the building of field member entities
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    [TestMethod]
    public void BuildFieldEntities()
    {
      var project = new CSharpProject(WorkingFolder);
      project.AddFile(@"EntityBuilderSyntaxNodeVisitor\BuildFieldEntities.cs");
      InvokeParser(project, true, false).ShouldBeTrue();
      var semanticGraph = new SemanticGraph();
      var visitor = new EntityBuilderSyntaxNodeVisitor(project, semanticGraph);
      project.SyntaxTree.AcceptVisitor(visitor);

      // class A
      {
        var classEntity = semanticGraph.GlobalNamespace.ChildTypes[0];
        classEntity.FullyQualifiedName.ShouldEqual("A");

        var memberArray = classEntity.Members.ToArray();
        memberArray.Length.ShouldEqual(3);
        memberArray[0].Name.ShouldEqual("a1");
        memberArray[1].Name.ShouldEqual("a2");
        memberArray[2].Name.ShouldEqual("a3");

        classEntity.DeclarationSpace.NameCount.ShouldEqual(3);
        ((FieldEntity)classEntity.DeclarationSpace["a1"].Entity).Name.ShouldEqual("a1");
        ((FieldEntity)classEntity.DeclarationSpace["a2"].Entity).Name.ShouldEqual("a2");
        ((FieldEntity)classEntity.DeclarationSpace["a3"].Entity).Name.ShouldEqual("a3");
      }

      // A a1, a2;
      {
        var fieldEntity = semanticGraph.GlobalNamespace.ChildTypes[0].Members.ToArray()[0] as FieldEntity;
        fieldEntity.Name.ShouldEqual("a1");
        fieldEntity.DistinctiveName.ShouldEqual("a1");
        fieldEntity.IsExplicitlyDefined.ShouldBeTrue();
        fieldEntity.IsStatic.ShouldBeFalse();
        ((ClassEntity) fieldEntity.Parent).FullyQualifiedName.ShouldEqual("A");
        fieldEntity.SyntaxNodes.Count.ShouldEqual(1);
        ((FieldTagNode) fieldEntity.SyntaxNodes[0]).ShouldEqual(
          ((FieldDeclarationNode) project.SyntaxTree.CompilationUnitNodes[0].TypeDeclarations[0].MemberDeclarations[0]).FieldTags[0]);
        fieldEntity.Type.ResolutionState.ShouldEqual(ResolutionState.NotYetResolved);
      }
      // A a1, a2;
      {
        var fieldEntity = semanticGraph.GlobalNamespace.ChildTypes[0].Members.ToArray()[1] as FieldEntity;
        fieldEntity.Name.ShouldEqual("a2");
        fieldEntity.DistinctiveName.ShouldEqual("a2");
        fieldEntity.IsExplicitlyDefined.ShouldBeTrue();
        fieldEntity.IsStatic.ShouldBeFalse();
        ((ClassEntity)fieldEntity.Parent).FullyQualifiedName.ShouldEqual("A");
        fieldEntity.SyntaxNodes.Count.ShouldEqual(1);
        ((FieldTagNode)fieldEntity.SyntaxNodes[0]).Identifier.ShouldEqual("a2");
        fieldEntity.Type.ResolutionState.ShouldEqual(ResolutionState.NotYetResolved);
      }
      // static A a3;
      {
        var fieldEntity = semanticGraph.GlobalNamespace.ChildTypes[0].Members.ToArray()[2] as FieldEntity;
        fieldEntity.Name.ShouldEqual("a3");
        fieldEntity.DistinctiveName.ShouldEqual("a3");
        fieldEntity.IsExplicitlyDefined.ShouldBeTrue();
        fieldEntity.IsStatic.ShouldBeTrue();
        ((ClassEntity)fieldEntity.Parent).FullyQualifiedName.ShouldEqual("A");
        fieldEntity.SyntaxNodes.Count.ShouldEqual(1);
        ((FieldTagNode)fieldEntity.SyntaxNodes[0]).Identifier.ShouldEqual("a3");
        fieldEntity.Type.ResolutionState.ShouldEqual(ResolutionState.NotYetResolved);
      }
      // struct S
      {
        var structEntity = semanticGraph.GlobalNamespace.ChildTypes[1] as StructEntity;
        structEntity.FullyQualifiedName.ShouldEqual("S");

        var memberArray = structEntity.Members.ToArray();
        memberArray.Length.ShouldEqual(1);
        memberArray[0].Name.ShouldEqual("a4");

        structEntity.DeclarationSpace.NameCount.ShouldEqual(1);
        ((FieldEntity)structEntity.DeclarationSpace["a4"].Entity).Name.ShouldEqual("a4");
      }
      // A a4;
      {
        var fieldEntity = semanticGraph.GlobalNamespace.ChildTypes[1].Members.ToArray()[0] as FieldEntity;
        fieldEntity.Name.ShouldEqual("a4");
        fieldEntity.DistinctiveName.ShouldEqual("a4");
        fieldEntity.IsExplicitlyDefined.ShouldBeTrue();
        fieldEntity.IsStatic.ShouldBeFalse();
        ((StructEntity)fieldEntity.Parent).FullyQualifiedName.ShouldEqual("S");
        fieldEntity.SyntaxNodes.Count.ShouldEqual(1);
        ((FieldTagNode)fieldEntity.SyntaxNodes[0]).Identifier.ShouldEqual("a4");
        fieldEntity.Type.ResolutionState.ShouldEqual(ResolutionState.NotYetResolved);
      }
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Error CS0101: The namespace 'A' already contains a definition for 'B' (namespace and class)
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    [TestMethod]
    public void CS0101_ClassAndNamespaceSameName()
    {
      var project = new CSharpProject(WorkingFolder);
      project.AddFile(@"EntityBuilderSyntaxNodeVisitor\CS0101_ClassAndNamespaceSameName.cs");
      InvokeParser(project, true, false).ShouldBeTrue();
      var semanticGraph = new SemanticGraph();
      var visitor = new EntityBuilderSyntaxNodeVisitor(project, semanticGraph);
      project.SyntaxTree.AcceptVisitor(visitor);

      project.Errors.Count.ShouldEqual(1);
      project.Errors[0].Code.ShouldEqual("CS0101");
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Error CS0101: The namespace 'A' already contains a definition for 'B' (class and struct)
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    [TestMethod]
    public void CS0101_ClassAndStructSameName()
    {
      var project = new CSharpProject(WorkingFolder);
      project.AddFile(@"EntityBuilderSyntaxNodeVisitor\CS0101_ClassAndStructSameName.cs");
      InvokeParser(project, true, false).ShouldBeTrue();
      var semanticGraph = new SemanticGraph();
      var visitor = new EntityBuilderSyntaxNodeVisitor(project, semanticGraph);
      project.SyntaxTree.AcceptVisitor(visitor);

      project.Errors.Count.ShouldEqual(1);
      project.Errors[0].Code.ShouldEqual("CS0101");
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Error CS0102: The type 'A' already contains a definition for 'a1'
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    [TestMethod]
    public void CS0102_TypeAlreadyContainsADefinition()
    {
      var project = new CSharpProject(WorkingFolder);
      project.AddFile(@"EntityBuilderSyntaxNodeVisitor\CS0102_TypeAlreadyContainsADefinition.cs");
      InvokeParser(project, true, false).ShouldBeTrue();
      var semanticGraph = new SemanticGraph();
      var visitor = new EntityBuilderSyntaxNodeVisitor(project, semanticGraph);
      project.SyntaxTree.AcceptVisitor(visitor);

      project.Errors.Count.ShouldEqual(1);
      project.Errors[0].Code.ShouldEqual("CS0102");
    }
  }
}