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
      var visitor = new EntityBuilderSyntaxNodeVisitor(project, project.SemanticGraph);
      project.SyntaxTree.AcceptVisitor(visitor);

      // global root namespace
      {
        var namespaceEntity = project.SemanticGraph.GlobalNamespace;
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
        var namespaceEntity = project.SemanticGraph.GlobalNamespace.ChildNamespaces[0];
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
        var namespaceEntity = project.SemanticGraph.GlobalNamespace.ChildNamespaces[0].ChildNamespaces[0];
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
        var namespaceEntity = project.SemanticGraph.GlobalNamespace.ChildNamespaces[1];
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
        var namespaceEntity = project.SemanticGraph.GlobalNamespace.ChildNamespaces[1].ChildNamespaces[0];
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
        var namespaceEntity = project.SemanticGraph.GlobalNamespace.ChildNamespaces[1].ChildNamespaces[0].ChildNamespaces[0];
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
          project.SemanticGraph.GlobalNamespace.ChildNamespaces[1].ChildNamespaces[0].ChildNamespaces[0].ChildNamespaces[0];
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
      var visitor = new EntityBuilderSyntaxNodeVisitor(project, project.SemanticGraph);
      project.SyntaxTree.AcceptVisitor(visitor);

      // global root namespace
      {
        var namespaceEntity = project.SemanticGraph.GlobalNamespace;
        namespaceEntity.FullyQualifiedName.ShouldEqual("global");

        namespaceEntity.ChildTypes.Count.ShouldEqual(1);
        namespaceEntity.ChildTypes[0].FullyQualifiedName.ShouldEqual("A");

        namespaceEntity.DeclarationSpace.NameCount.ShouldEqual(2);
        ((ClassEntity)namespaceEntity.DeclarationSpace["A"].Entity).FullyQualifiedName.ShouldEqual("A");
        ((NamespaceEntity)namespaceEntity.DeclarationSpace["C"].Entity).FullyQualifiedName.ShouldEqual("C");
      }
      // class A
      {
        var classEntity = project.SemanticGraph.GlobalNamespace.ChildTypes[0] as ClassEntity;
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

        // base class is not yet resolved, so it's null
        classEntity.BaseType.ShouldBeNull();

        // TODO: basetypes, members
      }
      // class B
      {
        var classEntity = ((ClassEntity)project.SemanticGraph.GlobalNamespace.ChildTypes[0]).ChildTypes[0] as ClassEntity;
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
        var namespaceEntity = project.SemanticGraph.GlobalNamespace.ChildNamespaces[0];
        namespaceEntity.FullyQualifiedName.ShouldEqual("C");

        namespaceEntity.ChildTypes.Count.ShouldEqual(1);
        namespaceEntity.DeclarationSpace.NameCount.ShouldEqual(2);
      }
      // namespace C2
      {
        var namespaceEntity = project.SemanticGraph.GlobalNamespace.ChildNamespaces[0].ChildNamespaces[0];
        namespaceEntity.FullyQualifiedName.ShouldEqual("C.C2");

        namespaceEntity.ChildTypes.Count.ShouldEqual(0);
        namespaceEntity.DeclarationSpace.NameCount.ShouldEqual(0);
      }
      // class D
      {
        var classEntity = project.SemanticGraph.GlobalNamespace.ChildNamespaces[0].ChildTypes[0] as ClassEntity;
        classEntity.FullyQualifiedName.ShouldEqual("C.D");
        classEntity.Name.ShouldEqual("D");
        ((NamespaceEntity)classEntity.Parent).FullyQualifiedName.ShouldEqual("C");
        classEntity.SyntaxNodes.Count.ShouldEqual(1);
        classEntity.SyntaxNodes[0].ShouldEqual(project.SyntaxTree.CompilationUnitNodes[0].NamespaceDeclarations[0].TypeDeclarations[0]);

        classEntity.ChildTypes.Count.ShouldEqual(0);
        classEntity.DeclarationSpace.NameCount.ShouldEqual(0);

        classEntity.BaseTypeReferences.Count().ShouldEqual(1);
        ((TypeNodeBasedTypeEntityReference)classEntity.BaseTypeReferences.ToArray()[0]).SyntaxNode.TypeName.TypeTags[0].Identifier.ShouldEqual("A");
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
      var visitor = new EntityBuilderSyntaxNodeVisitor(project, project.SemanticGraph);
      project.SyntaxTree.AcceptVisitor(visitor);

      // global root namespace
      {
        var namespaceEntity = project.SemanticGraph.GlobalNamespace;
        namespaceEntity.FullyQualifiedName.ShouldEqual("global");
        namespaceEntity.ChildNamespaces.Count.ShouldEqual(1);
        namespaceEntity.ChildNamespaces[0].FullyQualifiedName.ShouldEqual("A");
      }
      // enum B
      {
        var enumEntity = project.SemanticGraph.GlobalNamespace.ChildNamespaces[0].ChildTypes[0] as EnumEntity;
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

        enumEntity.BaseTypeReferences.ToList().Count.ShouldEqual(0);
        enumEntity.UnderlyingType.ResolutionState.ShouldEqual(ResolutionState.NotYetResolved);
        ((TypeNodeBasedTypeEntityReference)enumEntity.UnderlyingType).SyntaxNode.ShouldEqual(
          ((EnumDeclarationNode)project.SyntaxTree.CompilationUnitNodes[0].NamespaceDeclarations[0].TypeDeclarations[0]).EnumBase);
      }

      // enum C
      {
        var enumEntity = project.SemanticGraph.GlobalNamespace.ChildNamespaces[0].ChildTypes[1] as EnumEntity;
        enumEntity.Name.ShouldEqual("C");

        enumEntity.UnderlyingType.ResolutionState.ShouldEqual(ResolutionState.NotYetResolved);
        ((ReflectedTypeBasedTypeEntityReference)enumEntity.UnderlyingType).Metadata.ShouldEqual(typeof(int));
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
      var visitor = new EntityBuilderSyntaxNodeVisitor(project, project.SemanticGraph);
      project.SyntaxTree.AcceptVisitor(visitor);

      // global root namespace
      {
        var namespaceEntity = project.SemanticGraph.GlobalNamespace;
        namespaceEntity.FullyQualifiedName.ShouldEqual("global");
      }
      // namespace A
      {
        var namespaceEntity = project.SemanticGraph.GlobalNamespace.ChildNamespaces[0];
        namespaceEntity.FullyQualifiedName.ShouldEqual("A");
        namespaceEntity.ChildTypes.Count.ShouldEqual(1);
        namespaceEntity.ChildTypes[0].FullyQualifiedName.ShouldEqual("A.B");
        namespaceEntity.DeclarationSpace.NameCount.ShouldEqual(1);
        ((StructEntity)namespaceEntity.DeclarationSpace["B"].Entity).FullyQualifiedName.ShouldEqual("A.B");
      }
      // struct B
      {
        var structEntity = project.SemanticGraph.GlobalNamespace.ChildNamespaces[0].ChildTypes[0] as StructEntity;
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

        var baseTypes = structEntity.BaseTypeReferences.ToArray();
        baseTypes.Length.ShouldEqual(1);
        baseTypes[0].ResolutionState.ShouldEqual(ResolutionState.NotYetResolved);
        ((TypeNodeBasedTypeEntityReference)baseTypes[0]).SyntaxNode.ShouldEqual(
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
      var visitor = new EntityBuilderSyntaxNodeVisitor(project, project.SemanticGraph);
      project.SyntaxTree.AcceptVisitor(visitor);

      // global root namespace
      {
        var namespaceEntity = project.SemanticGraph.GlobalNamespace;
        namespaceEntity.FullyQualifiedName.ShouldEqual("global");
      }
      // namespace A
      {
        var namespaceEntity = project.SemanticGraph.GlobalNamespace.ChildNamespaces[0];
        namespaceEntity.FullyQualifiedName.ShouldEqual("A");
        namespaceEntity.ChildTypes.Count.ShouldEqual(1);
        namespaceEntity.ChildTypes[0].FullyQualifiedName.ShouldEqual("A.B");
        namespaceEntity.DeclarationSpace.NameCount.ShouldEqual(1);
        ((InterfaceEntity)namespaceEntity.DeclarationSpace["B"].Entity).FullyQualifiedName.ShouldEqual("A.B");
      }
      // interface B
      {
        var interfaceEntity = project.SemanticGraph.GlobalNamespace.ChildNamespaces[0].ChildTypes[0] as InterfaceEntity;
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

        var baseTypes = interfaceEntity.BaseTypeReferences.ToArray();
        baseTypes.Length.ShouldEqual(1);
        baseTypes[0].ResolutionState.ShouldEqual(ResolutionState.NotYetResolved);
        ((TypeNodeBasedTypeEntityReference)baseTypes[0]).SyntaxNode.ShouldEqual(
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
      var visitor = new EntityBuilderSyntaxNodeVisitor(project, project.SemanticGraph);
      project.SyntaxTree.AcceptVisitor(visitor);

      // global root namespace
      {
        var namespaceEntity = project.SemanticGraph.GlobalNamespace;
        namespaceEntity.FullyQualifiedName.ShouldEqual("global");
        namespaceEntity.ChildNamespaces.Count.ShouldEqual(1);
        namespaceEntity.ChildNamespaces[0].FullyQualifiedName.ShouldEqual("A");
        namespaceEntity.DeclarationSpace.NameCount.ShouldEqual(1);
        ((NamespaceEntity)namespaceEntity.DeclarationSpace["A"].Entity).FullyQualifiedName.ShouldEqual("A");
      }
      // namespace A
      {
        var namespaceEntity = project.SemanticGraph.GlobalNamespace.ChildNamespaces[0];
        namespaceEntity.FullyQualifiedName.ShouldEqual("A");
        namespaceEntity.ChildTypes.Count.ShouldEqual(1);
        namespaceEntity.ChildTypes[0].FullyQualifiedName.ShouldEqual("A.B");
        namespaceEntity.DeclarationSpace.NameCount.ShouldEqual(1);
        ((DelegateEntity)namespaceEntity.DeclarationSpace["B"].Entity).FullyQualifiedName.ShouldEqual("A.B");
      }
      // delegate B
      {
        var delegateEntity = project.SemanticGraph.GlobalNamespace.ChildNamespaces[0].ChildTypes[0] as DelegateEntity;
        delegateEntity.Name.ShouldEqual("B");
        delegateEntity.DistinctiveName.ShouldEqual("B");
        delegateEntity.FullyQualifiedName.ShouldEqual("A.B");
        ((NamespaceEntity)delegateEntity.Parent).FullyQualifiedName.ShouldEqual("A");
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
      var visitor = new EntityBuilderSyntaxNodeVisitor(project, project.SemanticGraph);
      project.SyntaxTree.AcceptVisitor(visitor);

      // global root namespace
      {
        var namespaceEntity = project.SemanticGraph.GlobalNamespace as NamespaceEntity;
        namespaceEntity.ChildNamespaces.Count.ShouldEqual(1);
        namespaceEntity.DeclarationSpace.NameCount.ShouldEqual(1);
        ((NamespaceEntity)namespaceEntity.DeclarationSpace["N"].Entity).FullyQualifiedName.ShouldEqual("N");
      }
      // namespace N
      {
        var namespaceEntity = project.SemanticGraph.GlobalNamespace.ChildNamespaces[0];
        namespaceEntity.DeclarationSpace.NameCount.ShouldEqual(2);
        ((NamespaceEntity)namespaceEntity.DeclarationSpace["A"].Entity).FullyQualifiedName.ShouldEqual("N.A");
        ((ClassEntity)namespaceEntity.DeclarationSpace["A`2"].Entity).FullyQualifiedName.ShouldEqual("N.A`2");
      }
      // class A<T1, T2>
      {
        var classEntity = project.SemanticGraph.GlobalNamespace.ChildNamespaces[0].ChildTypes[0] as ClassEntity;
        classEntity.Name.ShouldEqual("A");
        classEntity.DistinctiveName.ShouldEqual("A`2");
        classEntity.FullyQualifiedName.ShouldEqual("N.A`2");

        classEntity.ChildTypes.Count.ShouldEqual(3);
        classEntity.DeclarationSpace.NameCount.ShouldEqual(5);
        ((TypeParameterEntity)classEntity.DeclarationSpace["T1"].Entity).FullyQualifiedName.ShouldEqual("N.A`2.T1");
        ((TypeParameterEntity)classEntity.DeclarationSpace["T2"].Entity).FullyQualifiedName.ShouldEqual("N.A`2.T2");
        ((ClassEntity)classEntity.DeclarationSpace["B1"].Entity).FullyQualifiedName.ShouldEqual("N.A`2.B1");
        ((ClassEntity)classEntity.DeclarationSpace["B2`1"].Entity).FullyQualifiedName.ShouldEqual("N.A`2.B2`1");
        ((ClassEntity)classEntity.DeclarationSpace["B3`1"].Entity).FullyQualifiedName.ShouldEqual("N.A`2.B3`1");

        classEntity.IsGeneric.ShouldBeTrue();
        classEntity.OwnTypeParameters.ToArray().Count().ShouldEqual(2);
        var typeParams = classEntity.AllTypeParameters.ToArray();
        typeParams.Length.ShouldEqual(2);

        // type param T1
        {
          var typeParam = typeParams[0];
          typeParam.Name.ShouldEqual("T1");
          typeParam.DistinctiveName.ShouldEqual("T1");
          typeParam.FullyQualifiedName.ShouldEqual("N.A`2.T1");
          typeParam.IsPointerType.ShouldBeFalse();
          typeParam.IsReferenceType.ShouldBeFalse();
          typeParam.IsValueType.ShouldBeFalse();
          typeParam.BaseTypeReferences.Count().ShouldEqual(0);
          typeParam.DeclarationSpace.NameCount.ShouldEqual(0);
          typeParam.Members.Count().ShouldEqual(0);
          typeParam.Parent.ShouldEqual(classEntity);
          typeParam.SyntaxNodes.Count.ShouldEqual(1);
          typeParam.SyntaxNodes[0].ShouldEqual(
            project.SyntaxTree.CompilationUnitNodes[0].NamespaceDeclarations[0].TypeDeclarations[0].TypeParameters[0]);
        }
        // type param T2
        {
          var typeParam = typeParams[1];
          typeParam.FullyQualifiedName.ShouldEqual("N.A`2.T2");
          typeParam.Parent.ShouldEqual(classEntity);
        }
      }

      var parentClass = project.SemanticGraph.GlobalNamespace.ChildNamespaces[0].ChildTypes[0] as ClassEntity;
      int child = 0;
      // class B1
      {
        var classEntity = parentClass.ChildTypes[child++] as ClassEntity;
        classEntity.FullyQualifiedName.ShouldEqual("N.A`2.B1");
        var ownTypeParams = classEntity.OwnTypeParameters.ToArray();
        ownTypeParams.Length.ShouldEqual(0);
        var typeParams = classEntity.AllTypeParameters.ToArray();
        typeParams.Length.ShouldEqual(2);
        typeParams[0].FullyQualifiedName.ShouldEqual("N.A`2.T1");
        typeParams[1].FullyQualifiedName.ShouldEqual("N.A`2.T2");
      }
      // class B2<T1>
      {
        var classEntity = parentClass.ChildTypes[child++] as ClassEntity;
        classEntity.FullyQualifiedName.ShouldEqual("N.A`2.B2`1");
        var ownTypeParams = classEntity.OwnTypeParameters.ToArray();
        ownTypeParams.Length.ShouldEqual(1);
        ownTypeParams[0].FullyQualifiedName.ShouldEqual("N.A`2.B2`1.T1");
        var typeParams = classEntity.AllTypeParameters.ToArray();
        typeParams.Length.ShouldEqual(3);
        typeParams[0].FullyQualifiedName.ShouldEqual("N.A`2.T1");
        typeParams[1].FullyQualifiedName.ShouldEqual("N.A`2.T2");
        typeParams[2].FullyQualifiedName.ShouldEqual("N.A`2.B2`1.T1");
      }
      // class B3<T3>
      {
        var classEntity = parentClass.ChildTypes[child++] as ClassEntity;
        classEntity.FullyQualifiedName.ShouldEqual("N.A`2.B3`1");
        var ownTypeParams = classEntity.OwnTypeParameters.ToArray();
        ownTypeParams.Length.ShouldEqual(1);
        ownTypeParams[0].FullyQualifiedName.ShouldEqual("N.A`2.B3`1.T3");
        var typeParams = classEntity.AllTypeParameters.ToArray();
        typeParams.Length.ShouldEqual(3);
        typeParams[0].FullyQualifiedName.ShouldEqual("N.A`2.T1");
        typeParams[1].FullyQualifiedName.ShouldEqual("N.A`2.T2");
        typeParams[2].FullyQualifiedName.ShouldEqual("N.A`2.B3`1.T3");
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
      var visitor = new EntityBuilderSyntaxNodeVisitor(project, project.SemanticGraph);
      project.SyntaxTree.AcceptVisitor(visitor);

      // class A
      {
        var classEntity = project.SemanticGraph.GlobalNamespace.ChildTypes[0];
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
        var fieldEntity = project.SemanticGraph.GlobalNamespace.ChildTypes[0].Members.ToArray()[0] as FieldEntity;
        fieldEntity.Name.ShouldEqual("a1");
        fieldEntity.DistinctiveName.ShouldEqual("a1");
        fieldEntity.IsExplicitlyDefined.ShouldBeTrue();
        fieldEntity.IsStatic.ShouldBeFalse();
        ((ClassEntity)fieldEntity.Parent).FullyQualifiedName.ShouldEqual("A");
        fieldEntity.SyntaxNodes.Count.ShouldEqual(1);
        ((FieldTagNode)fieldEntity.SyntaxNodes[0]).ShouldEqual(
          ((FieldDeclarationNode)project.SyntaxTree.CompilationUnitNodes[0].TypeDeclarations[0].MemberDeclarations[0]).FieldTags[0]);
        fieldEntity.Type.ResolutionState.ShouldEqual(ResolutionState.NotYetResolved);
      }
      // A a1, a2;
      {
        var fieldEntity = project.SemanticGraph.GlobalNamespace.ChildTypes[0].Members.ToArray()[1] as FieldEntity;
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
        var fieldEntity = project.SemanticGraph.GlobalNamespace.ChildTypes[0].Members.ToArray()[2] as FieldEntity;
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
        var structEntity = project.SemanticGraph.GlobalNamespace.ChildTypes[1] as StructEntity;
        structEntity.FullyQualifiedName.ShouldEqual("S");

        var memberArray = structEntity.Members.ToArray();
        memberArray.Length.ShouldEqual(1);
        memberArray[0].Name.ShouldEqual("a4");

        structEntity.DeclarationSpace.NameCount.ShouldEqual(1);
        ((FieldEntity)structEntity.DeclarationSpace["a4"].Entity).Name.ShouldEqual("a4");
      }
      // A a4;
      {
        var fieldEntity = project.SemanticGraph.GlobalNamespace.ChildTypes[1].Members.ToArray()[0] as FieldEntity;
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
      var visitor = new EntityBuilderSyntaxNodeVisitor(project, project.SemanticGraph);
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
      var visitor = new EntityBuilderSyntaxNodeVisitor(project, project.SemanticGraph);
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
      var visitor = new EntityBuilderSyntaxNodeVisitor(project, project.SemanticGraph);
      project.SyntaxTree.AcceptVisitor(visitor);

      project.Errors.Count.ShouldEqual(1);
      project.Errors[0].Code.ShouldEqual("CS0102");
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Tests the building of using namespace entities
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    [TestMethod]
    public void BuildUsingNamespaceEntities()
    {
      var project = new CSharpProject(WorkingFolder);
      project.AddFile(@"EntityBuilderSyntaxNodeVisitor\BuildUsingNamespaceEntities.cs");
      InvokeParser(project, true, false).ShouldBeTrue();
      var visitor = new EntityBuilderSyntaxNodeVisitor(project, project.SemanticGraph);
      project.SyntaxTree.AcceptVisitor(visitor);

      var compilationUnitNode = project.SyntaxTree.CompilationUnitNodes[0];
      {
        // global root namespace
        var namespaceEntity = project.SemanticGraph.GlobalNamespace;
        namespaceEntity.FullyQualifiedName.ShouldEqual("global");

        // using A.B;
        var usingNamespaces = namespaceEntity.UsingNamespaces.ToList();
        usingNamespaces.Count.ShouldEqual(1);
        var usingNamespace = usingNamespaces[0];
        usingNamespace.ImportedNamespace.ShouldBeNull();
        usingNamespace.LexicalScope.CompilationUnit.ShouldEqual(compilationUnitNode);
        usingNamespace.LexicalScope.FromSourcePoint.Position.ShouldEqual(compilationUnitNode.StartPosition);
        usingNamespace.LexicalScope.ToSourcePoint.Position.ShouldEqual(compilationUnitNode.EndPosition);
        usingNamespace.NamespaceReference.ResolutionState.ShouldEqual(ResolutionState.NotYetResolved);
        usingNamespace.NamespaceReference.SyntaxNode.ShouldEqual(compilationUnitNode.UsingNodes[0].NamespaceOrTypeName);
        usingNamespace.Parent.ShouldEqual(namespaceEntity);
        usingNamespace.ReflectedMetadata.ShouldBeNull();
        usingNamespace.SyntaxNodes.Count.ShouldEqual(1);
        usingNamespace.SyntaxNodes[0].ShouldEqual(compilationUnitNode.UsingNodes[0]);

        compilationUnitNode.UsingNodes[0].SemanticEntities.Count.ShouldEqual(1);
        compilationUnitNode.UsingNodes[0].SemanticEntities[0].ShouldEqual(usingNamespace);

        namespaceEntity.GetUsingNamespacesBySourcePoint(new SourcePoint(null, 0)).Count().ShouldEqual(0);
        namespaceEntity.GetUsingNamespacesBySourcePoint(new SourcePoint(compilationUnitNode, 4)).Count().ShouldEqual(1);
      }
      {
        // namespace A
        var namespaceA = project.SemanticGraph.GlobalNamespace.ChildNamespaces[0];
        namespaceA.FullyQualifiedName.ShouldEqual("A");
        namespaceA.UsingNamespaces.Count().ShouldEqual(0);

        // namespace B
        var namespaceB = namespaceA.ChildNamespaces[0];
        namespaceB.FullyQualifiedName.ShouldEqual("A.B");
        namespaceB.UsingNamespaces.Count().ShouldEqual(1);

        // using B;
        var usingNamespace = namespaceB.UsingNamespaces.ToList()[0];
        usingNamespace.ImportedNamespace.ShouldBeNull();
        usingNamespace.LexicalScope.CompilationUnit.ShouldEqual(compilationUnitNode);
        usingNamespace.LexicalScope.FromSourcePoint.Position.ShouldEqual(compilationUnitNode.NamespaceDeclarations[0].StartPosition);
        usingNamespace.LexicalScope.ToSourcePoint.Position.ShouldEqual(compilationUnitNode.NamespaceDeclarations[0].EndPosition);
        usingNamespace.NamespaceReference.ResolutionState.ShouldEqual(ResolutionState.NotYetResolved);
        usingNamespace.NamespaceReference.SyntaxNode.ShouldEqual(compilationUnitNode.NamespaceDeclarations[0].UsingNodes[0].NamespaceOrTypeName);
        usingNamespace.Parent.ShouldEqual(namespaceB);
        usingNamespace.ReflectedMetadata.ShouldBeNull();
        usingNamespace.SyntaxNodes.Count.ShouldEqual(1);
        usingNamespace.SyntaxNodes[0].ShouldEqual(compilationUnitNode.NamespaceDeclarations[0].UsingNodes[0]);

        compilationUnitNode.NamespaceDeclarations[0].UsingNodes[0].SemanticEntities.Count.ShouldEqual(1);
        compilationUnitNode.NamespaceDeclarations[0].UsingNodes[0].SemanticEntities[0].ShouldEqual(usingNamespace);

        namespaceB.GetUsingNamespacesBySourcePoint(new SourcePoint(null, 0)).Count().ShouldEqual(0);
        namespaceB.GetUsingNamespacesBySourcePoint(new SourcePoint(compilationUnitNode,
            compilationUnitNode.NamespaceDeclarations[0].UsingNodes[0].StartPosition)).Count().ShouldEqual(1);
      }
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Error CS0105: The using directive for '{0}' appeared previously in this namespace
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    [TestMethod]
    public void CS0105_UsingNamespaceDuplicate()
    {
      var project = new CSharpProject(WorkingFolder);
      project.AddFile(@"EntityBuilderSyntaxNodeVisitor\CS0105_UsingNamespaceDuplicate.cs");
      InvokeParser(project, true, false).ShouldBeTrue();
      var visitor = new EntityBuilderSyntaxNodeVisitor(project, project.SemanticGraph);
      project.SyntaxTree.AcceptVisitor(visitor);

      project.Errors.Count.ShouldEqual(0);
      project.Warnings.Count.ShouldEqual(1);
      project.Warnings[0].Code.ShouldEqual("CS0105");
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Tests the building of using alias entities
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    [TestMethod]
    public void BuildUsingAliasEntities()
    {
      var project = new CSharpProject(WorkingFolder);
      project.AddFile(@"EntityBuilderSyntaxNodeVisitor\BuildUsingAliasEntities.cs");
      InvokeParser(project, true, false).ShouldBeTrue();
      var visitor = new EntityBuilderSyntaxNodeVisitor(project, project.SemanticGraph);
      project.SyntaxTree.AcceptVisitor(visitor);

      var compilationUnitNode = project.SyntaxTree.CompilationUnitNodes[0];
      {
        // global root namespace
        var namespaceEntity = project.SemanticGraph.GlobalNamespace;
        namespaceEntity.FullyQualifiedName.ShouldEqual("global");

        // using S = System;
        var usingAliases = namespaceEntity.UsingAliases.ToList();
        usingAliases.Count.ShouldEqual(1);
        var usingAlias = usingAliases[0];
        usingAlias.Alias.ShouldEqual("S");
        usingAlias.AliasedNamespace.ShouldBeNull();
        usingAlias.AliasedType.ShouldBeNull();
        usingAlias.LexicalScope.CompilationUnit.ShouldEqual(compilationUnitNode);
        usingAlias.LexicalScope.FromSourcePoint.Position.ShouldEqual(compilationUnitNode.StartPosition);
        usingAlias.LexicalScope.ToSourcePoint.Position.ShouldEqual(compilationUnitNode.EndPosition);
        usingAlias.NamespaceOrTypeReference.ResolutionState.ShouldEqual(ResolutionState.NotYetResolved);
        usingAlias.NamespaceOrTypeReference.SyntaxNode.ShouldEqual(compilationUnitNode.UsingNodes[0].NamespaceOrTypeName);
        usingAlias.Parent.ShouldEqual(namespaceEntity);
        usingAlias.ReflectedMetadata.ShouldBeNull();
        usingAlias.SyntaxNodes.Count.ShouldEqual(1);
        usingAlias.SyntaxNodes[0].ShouldEqual(compilationUnitNode.UsingNodes[0]);

        compilationUnitNode.UsingNodes[0].SemanticEntities.Count.ShouldEqual(1);
        compilationUnitNode.UsingNodes[0].SemanticEntities[0].ShouldEqual(usingAlias);

        namespaceEntity.GetUsingAliasByNameAndSourcePoint("S", new SourcePoint(null, 0)).ShouldBeNull();
        namespaceEntity.GetUsingAliasByNameAndSourcePoint("X", new SourcePoint(compilationUnitNode, 4)).ShouldBeNull();
        namespaceEntity.GetUsingAliasByNameAndSourcePoint("S", new SourcePoint(compilationUnitNode, 4)).ShouldEqual(usingAlias);
      }
      {
        // namespace A
        var namespaceA = project.SemanticGraph.GlobalNamespace.ChildNamespaces[0];
        namespaceA.FullyQualifiedName.ShouldEqual("A");
        namespaceA.UsingAliases.Count().ShouldEqual(0);

        // namespace B
        var namespaceB = namespaceA.ChildNamespaces[0];
        namespaceB.FullyQualifiedName.ShouldEqual("A.B");
        namespaceB.UsingAliases.Count().ShouldEqual(1);

        // using B;
        var usingAlias = namespaceB.UsingAliases.ToList()[0];
        usingAlias.Alias.ShouldEqual("E");
        usingAlias.AliasedNamespace.ShouldBeNull();
        usingAlias.AliasedType.ShouldBeNull();
        usingAlias.LexicalScope.CompilationUnit.ShouldEqual(compilationUnitNode);
        usingAlias.LexicalScope.FromSourcePoint.Position.ShouldEqual(compilationUnitNode.NamespaceDeclarations[0].StartPosition);
        usingAlias.LexicalScope.ToSourcePoint.Position.ShouldEqual(compilationUnitNode.NamespaceDeclarations[0].EndPosition);
        usingAlias.NamespaceOrTypeReference.ResolutionState.ShouldEqual(ResolutionState.NotYetResolved);
        usingAlias.NamespaceOrTypeReference.SyntaxNode.ShouldEqual(compilationUnitNode.NamespaceDeclarations[0].UsingNodes[0].NamespaceOrTypeName);
        usingAlias.Parent.ShouldEqual(namespaceB);
        usingAlias.ReflectedMetadata.ShouldBeNull();
        usingAlias.SyntaxNodes.Count.ShouldEqual(1);
        usingAlias.SyntaxNodes[0].ShouldEqual(compilationUnitNode.NamespaceDeclarations[0].UsingNodes[0]);

        compilationUnitNode.NamespaceDeclarations[0].UsingNodes[0].SemanticEntities.Count.ShouldEqual(1);
        compilationUnitNode.NamespaceDeclarations[0].UsingNodes[0].SemanticEntities[0].ShouldEqual(usingAlias);

        namespaceB.GetUsingAliasByNameAndSourcePoint("E", new SourcePoint(null, 0)).ShouldBeNull();
        namespaceB.GetUsingAliasByNameAndSourcePoint("X", new SourcePoint(compilationUnitNode,
          compilationUnitNode.NamespaceDeclarations[0].UsingNodes[0].StartPosition)).ShouldBeNull();
        namespaceB.GetUsingAliasByNameAndSourcePoint("E", new SourcePoint(compilationUnitNode,
          compilationUnitNode.NamespaceDeclarations[0].UsingNodes[0].StartPosition)).ShouldEqual(usingAlias);
      }
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Error CS1537: The using alias 'alias' appeared previously in this namespace
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    [TestMethod]
    public void CS1537_UsingAliasDuplicate()
    {
      var project = new CSharpProject(WorkingFolder);
      project.AddFile(@"EntityBuilderSyntaxNodeVisitor\CS1537_UsingAliasDuplicate.cs");
      InvokeParser(project, true, false).ShouldBeTrue();
      var visitor = new EntityBuilderSyntaxNodeVisitor(project, project.SemanticGraph);
      project.SyntaxTree.AcceptVisitor(visitor);

      project.Errors.Count.ShouldEqual(1);
      project.Warnings.Count.ShouldEqual(0);
      project.Errors[0].Code.ShouldEqual("CS1537");
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Tests the building of extern alias entities
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    [TestMethod]
    public void BuildExternAliasEntities()
    {
      var project = new CSharpProject(WorkingFolder);
      project.AddFile(@"EntityBuilderSyntaxNodeVisitor\BuildExternAliasEntities.cs");
      InvokeParser(project, true, false).ShouldBeTrue();
      var visitor = new EntityBuilderSyntaxNodeVisitor(project, project.SemanticGraph);
      project.SyntaxTree.AcceptVisitor(visitor);

      var compilationUnitNode = project.SyntaxTree.CompilationUnitNodes[0];
      {
        // global root namespace
        var namespaceEntity = project.SemanticGraph.GlobalNamespace;
        namespaceEntity.FullyQualifiedName.ShouldEqual("global");

        // extern alias A;
        var externAliases = namespaceEntity.ExternAliases.ToList();
        externAliases.Count.ShouldEqual(1);
        var externAlias = externAliases[0];
        externAlias.Alias.ShouldEqual("A");
        externAlias.AliasedRootNamespace.ShouldBeNull();
        externAlias.LexicalScope.CompilationUnit.ShouldEqual(compilationUnitNode);
        externAlias.LexicalScope.FromSourcePoint.Position.ShouldEqual(compilationUnitNode.StartPosition);
        externAlias.LexicalScope.ToSourcePoint.Position.ShouldEqual(compilationUnitNode.EndPosition);
        externAlias.RootNamespaceReference.ResolutionState.ShouldEqual(ResolutionState.NotYetResolved);
        externAlias.RootNamespaceReference.SyntaxNode.ShouldEqual(compilationUnitNode.ExternAliasNodes[0]);
        externAlias.Parent.ShouldEqual(namespaceEntity);
        externAlias.ReflectedMetadata.ShouldBeNull();
        externAlias.SyntaxNodes.Count.ShouldEqual(1);
        externAlias.SyntaxNodes[0].ShouldEqual(compilationUnitNode.ExternAliasNodes[0]);

        compilationUnitNode.ExternAliasNodes[0].SemanticEntities.Count.ShouldEqual(1);
        compilationUnitNode.ExternAliasNodes[0].SemanticEntities[0].ShouldEqual(externAlias);

        namespaceEntity.GetExternAliasByNameAndSourcePoint("A", new SourcePoint(null, 0)).ShouldBeNull();
        namespaceEntity.GetExternAliasByNameAndSourcePoint("X", new SourcePoint(compilationUnitNode, 4)).ShouldBeNull();
        namespaceEntity.GetExternAliasByNameAndSourcePoint("A", new SourcePoint(compilationUnitNode, 4)).ShouldEqual(
          externAlias);
      }
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Error CS1537: The using alias 'alias' appeared previously in this namespace (for extern alias)
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    [TestMethod]
    public void CS1537_ExternAliasDuplicateName()
    {
      var project = new CSharpProject(WorkingFolder);
      project.AddFile(@"EntityBuilderSyntaxNodeVisitor\CS1537_ExternAliasDuplicateName.cs");
      InvokeParser(project, true, false).ShouldBeTrue();
      var visitor = new EntityBuilderSyntaxNodeVisitor(project, project.SemanticGraph);
      project.SyntaxTree.AcceptVisitor(visitor);

      project.Errors.Count.ShouldEqual(1);
      project.Warnings.Count.ShouldEqual(0);
      project.Errors[0].Code.ShouldEqual("CS1537");
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Error CS1537: conflicting extern and using alias names
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    [TestMethod]
    public void CS1537_ExternAndUsingAliasDuplicateName()
    {
      var project = new CSharpProject(WorkingFolder);
      project.AddFile(@"EntityBuilderSyntaxNodeVisitor\CS1537_ExternAndUsingAliasDuplicateName.cs");
      InvokeParser(project, true, false).ShouldBeTrue();
      var visitor = new EntityBuilderSyntaxNodeVisitor(project, project.SemanticGraph);
      project.SyntaxTree.AcceptVisitor(visitor);

      project.Errors.Count.ShouldEqual(1);
      project.Warnings.Count.ShouldEqual(0);
      project.Errors[0].Code.ShouldEqual("CS1537");
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Tests the building of enum members
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    [TestMethod]
    public void BuildEnumMemberEntities()
    {
      var project = new CSharpProject(WorkingFolder);
      project.AddFile(@"EntityBuilderSyntaxNodeVisitor\BuildEnumMemberEntities.cs");
      InvokeParser(project, true, false).ShouldBeTrue();
      var visitor = new EntityBuilderSyntaxNodeVisitor(project, project.SemanticGraph);
      project.SyntaxTree.AcceptVisitor(visitor);

      int i = 0;

      var enumDeclarationNode = project.SyntaxTree.CompilationUnitNodes[0].TypeDeclarations[0] as EnumDeclarationNode;

      {
        var enumMember = project.SemanticGraph.GlobalNamespace.ChildTypes[0].Members.ToList()[i] as EnumMemberEntity;
        enumMember.Name.ShouldEqual(enumDeclarationNode.Values[i].Identifier);
        enumMember.DistinctiveName.ShouldEqual(enumDeclarationNode.Values[i].Identifier);
        enumMember.IsExplicitlyDefined.ShouldBeTrue();
        enumMember.IsStatic.ShouldBeTrue();
        enumMember.Parent.ShouldEqual(project.SemanticGraph.GlobalNamespace.ChildTypes[0]);
        enumMember.ReflectedMetadata.ShouldBeNull();
        enumMember.SyntaxNodes[0].ShouldEqual(enumDeclarationNode.Values[i]);
        enumMember.Type.ShouldEqual(((EnumEntity)project.SemanticGraph.GlobalNamespace.ChildTypes[0]).UnderlyingType);
      }

      i++;
      {
        var enumMember = project.SemanticGraph.GlobalNamespace.ChildTypes[0].Members.ToList()[i] as EnumMemberEntity;
        enumMember.Name.ShouldEqual(enumDeclarationNode.Values[i].Identifier);
        enumMember.DistinctiveName.ShouldEqual(enumDeclarationNode.Values[i].Identifier);
        enumMember.IsExplicitlyDefined.ShouldBeTrue();
        enumMember.IsStatic.ShouldBeTrue();
        enumMember.Parent.ShouldEqual(project.SemanticGraph.GlobalNamespace.ChildTypes[0]);
        enumMember.ReflectedMetadata.ShouldBeNull();
        enumMember.SyntaxNodes[0].ShouldEqual(enumDeclarationNode.Values[i]);
        enumMember.Type.ShouldEqual(((EnumEntity)project.SemanticGraph.GlobalNamespace.ChildTypes[0]).UnderlyingType);
      }
    }
  }
}