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
      semanticEntities.Length.ShouldEqual(7);

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
      // type param T1
      {
        var typeParamEntity = semanticEntities[2] as TypeParameterEntity;
        typeParamEntity.FullyQualifiedName.ShouldEqual("global::A`2.T1");
        ((ClassEntity) typeParamEntity.Parent).FullyQualifiedName.ShouldEqual("global::A`2");
      }
      // type param T2
      {
        var typeParamEntity = semanticEntities[3] as TypeParameterEntity;
        typeParamEntity.FullyQualifiedName.ShouldEqual("global::A`2.T2");
        ((ClassEntity)typeParamEntity.Parent).FullyQualifiedName.ShouldEqual("global::A`2");
      }
      // class A<T1, T2>
      {
        var classEntity = semanticEntities[4] as ClassEntity;
        classEntity.FullyQualifiedName.ShouldEqual("global::A`2");

        classEntity.ChildTypes.Count.ShouldEqual(1);
        classEntity.ChildTypes[0].FullyQualifiedName.ShouldEqual("global::A`2.B`1");

        classEntity.DeclarationSpace.NameCount.ShouldEqual(3);
        ((TypeParameterEntity)classEntity.DeclarationSpace["T1"].Entity).FullyQualifiedName.ShouldEqual("global::A`2.T1");
        ((TypeParameterEntity)classEntity.DeclarationSpace["T2"].Entity).FullyQualifiedName.ShouldEqual("global::A`2.T2");
        ((ClassEntity)classEntity.DeclarationSpace["B`1"].Entity).FullyQualifiedName.ShouldEqual("global::A`2.B`1");
      }
      // type param T3
      {
        var typeParamEntity = semanticEntities[5] as TypeParameterEntity;
        typeParamEntity.FullyQualifiedName.ShouldEqual("global::A`2.B`1.T3");
        ((ClassEntity)typeParamEntity.Parent).FullyQualifiedName.ShouldEqual("global::A`2.B`1");
      }
      // class B<T3>
      {
        var classEntity = semanticEntities[6] as ClassEntity;
        classEntity.FullyQualifiedName.ShouldEqual("global::A`2.B`1");

        classEntity.ChildTypes.Count.ShouldEqual(0);

        classEntity.DeclarationSpace.NameCount.ShouldEqual(1);
        ((TypeParameterEntity)classEntity.DeclarationSpace["T3"].Entity).FullyQualifiedName.ShouldEqual("global::A`2.B`1.T3");
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
      InvokeParser(project).ShouldBeTrue();
      var semanticGraph = new SemanticGraph();
      var visitor = new EntityBuilderSyntaxNodeVisitor(semanticGraph, project);
      project.SyntaxTree.AcceptVisitor(visitor);

      var semanticEntities = semanticGraph.SemanticEntities.ToArray();
      semanticEntities.Length.ShouldEqual(7);

      // class A
      {
        var classEntity = semanticEntities[1] as ClassEntity;
        classEntity.FullyQualifiedName.ShouldEqual("global::A");

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
        var fieldEntity = semanticEntities[2] as FieldEntity;
        fieldEntity.Name.ShouldEqual("a1");
        fieldEntity.DistinctiveName.ShouldEqual("a1");
        fieldEntity.IsExplicitlyDefined.ShouldBeTrue();
        fieldEntity.IsStatic.ShouldBeFalse();
        ((ClassEntity) fieldEntity.Parent).FullyQualifiedName.ShouldEqual("global::A");
        fieldEntity.SyntaxNodes.Count.ShouldEqual(1);
        ((FieldTagNode) fieldEntity.SyntaxNodes[0]).Identifier.ShouldEqual("a1");
        fieldEntity.Type.ResolutionState.ShouldEqual(ResolutionState.NotYetResolved);
      }
      // A a1, a2;
      {
        var fieldEntity = semanticEntities[3] as FieldEntity;
        fieldEntity.Name.ShouldEqual("a2");
        fieldEntity.DistinctiveName.ShouldEqual("a2");
        fieldEntity.IsExplicitlyDefined.ShouldBeTrue();
        fieldEntity.IsStatic.ShouldBeFalse();
        ((ClassEntity)fieldEntity.Parent).FullyQualifiedName.ShouldEqual("global::A");
        fieldEntity.SyntaxNodes.Count.ShouldEqual(1);
        ((FieldTagNode)fieldEntity.SyntaxNodes[0]).Identifier.ShouldEqual("a2");
        fieldEntity.Type.ResolutionState.ShouldEqual(ResolutionState.NotYetResolved);
      }
      // static A a3;
      {
        var fieldEntity = semanticEntities[4] as FieldEntity;
        fieldEntity.Name.ShouldEqual("a3");
        fieldEntity.DistinctiveName.ShouldEqual("a3");
        fieldEntity.IsExplicitlyDefined.ShouldBeTrue();
        fieldEntity.IsStatic.ShouldBeTrue();
        ((ClassEntity)fieldEntity.Parent).FullyQualifiedName.ShouldEqual("global::A");
        fieldEntity.SyntaxNodes.Count.ShouldEqual(1);
        ((FieldTagNode)fieldEntity.SyntaxNodes[0]).Identifier.ShouldEqual("a3");
        fieldEntity.Type.ResolutionState.ShouldEqual(ResolutionState.NotYetResolved);
      }
      // struct S
      {
        var structEntity = semanticEntities[5] as StructEntity;
        structEntity.FullyQualifiedName.ShouldEqual("global::S");

        var memberArray = structEntity.Members.ToArray();
        memberArray.Length.ShouldEqual(1);
        memberArray[0].Name.ShouldEqual("a4");

        structEntity.DeclarationSpace.NameCount.ShouldEqual(1);
        ((FieldEntity)structEntity.DeclarationSpace["a4"].Entity).Name.ShouldEqual("a4");
      }
      // A a4;
      {
        var fieldEntity = semanticEntities[6] as FieldEntity;
        fieldEntity.Name.ShouldEqual("a4");
        fieldEntity.DistinctiveName.ShouldEqual("a4");
        fieldEntity.IsExplicitlyDefined.ShouldBeTrue();
        fieldEntity.IsStatic.ShouldBeFalse();
        ((StructEntity)fieldEntity.Parent).FullyQualifiedName.ShouldEqual("global::S");
        fieldEntity.SyntaxNodes.Count.ShouldEqual(1);
        ((FieldTagNode)fieldEntity.SyntaxNodes[0]).Identifier.ShouldEqual("a4");
        fieldEntity.Type.ResolutionState.ShouldEqual(ResolutionState.NotYetResolved);
      }
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
      InvokeParser(project).ShouldBeTrue();
      var semanticGraph = new SemanticGraph();
      var visitor = new EntityBuilderSyntaxNodeVisitor(semanticGraph, project);
      project.SyntaxTree.AcceptVisitor(visitor);

      project.Errors.Count.ShouldEqual(1);
      project.Errors[0].Code.ShouldEqual("CS0102");
    }
  }
}