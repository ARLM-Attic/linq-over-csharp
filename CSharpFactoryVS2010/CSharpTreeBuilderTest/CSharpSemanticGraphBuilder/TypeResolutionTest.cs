using System;
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
  /// Tests the type resolution logic of the TypeResolverSemanticGraphVisitor class
  /// </summary>
  // ================================================================================================
  [TestClass]
  public class TypeResolutionTest : ParserTestBed
  {
    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Tests the resolution of base types, declared in the same file
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    [TestMethod]
    public void LocallyDeclaredBaseTypes()
    {
      // Set up SyntaxTree and SemanticGraph
      var project = new CSharpProject(WorkingFolder);
      project.AddFile(@"TypeResolution\LocallyDeclaredBaseTypes.cs");
      InvokeParser(project).ShouldBeTrue();
      var semanticGraph = new SemanticGraph();
      project.SyntaxTree.AcceptVisitor(new EntityBuilderSyntaxNodeVisitor(semanticGraph,project));
      semanticGraph.AcceptVisitor(new TypeResolverSemanticGraphVisitor(project));

      // class A1 : C0
      {
        var baseTypeRef = semanticGraph.GlobalNamespace.ChildTypes[0].BaseTypes[0];
        baseTypeRef.ResolvedEntity.FullyQualifiedName.ShouldEqual("global::C0");
      }
      // class A2 : C0.C1
      {
        var baseTypeRef = semanticGraph.GlobalNamespace.ChildTypes[1].BaseTypes[0];
        baseTypeRef.ResolvedEntity.FullyQualifiedName.ShouldEqual("global::C0.C1");
      }
      // class A3 : N1.N1C0
      {
        var baseTypeRef = semanticGraph.GlobalNamespace.ChildTypes[2].BaseTypes[0];
        baseTypeRef.ResolvedEntity.FullyQualifiedName.ShouldEqual("global::N1.N1C0");
      }
      // class A4 : N1.N2.N2C0
      {
        var baseTypeRef = semanticGraph.GlobalNamespace.ChildTypes[3].BaseTypes[0];
        baseTypeRef.ResolvedEntity.FullyQualifiedName.ShouldEqual("global::N1.N2.N2C0");
      }
      // class A5 : N1.N2.N2C0.N2C1
      {
        var baseTypeRef = semanticGraph.GlobalNamespace.ChildTypes[4].BaseTypes[0];
        baseTypeRef.ResolvedEntity.FullyQualifiedName.ShouldEqual("global::N1.N2.N2C0.N2C1");
      }
      // class N3C1 : N3C2
      {
        var baseTypeRef = semanticGraph.GlobalNamespace.ChildNamespaces[0].ChildTypes[0].BaseTypes[0];
        baseTypeRef.ResolvedEntity.FullyQualifiedName.ShouldEqual("global::N3.N3C2");
      }
      // class N5C1 : N4C1
      {
        var baseTypeRef = semanticGraph.GlobalNamespace.ChildNamespaces[1].ChildNamespaces[0].ChildTypes[0].BaseTypes[0];
        baseTypeRef.ResolvedEntity.FullyQualifiedName.ShouldEqual("global::N4.N4C1");
      }
      // class N5C2 : N6.N6C1
      {
        var baseTypeRef = semanticGraph.GlobalNamespace.ChildNamespaces[1].ChildNamespaces[0].ChildTypes[1].BaseTypes[0];
        baseTypeRef.ResolvedEntity.FullyQualifiedName.ShouldEqual("global::N4.N6.N6C1");
      }
      // class N5C2C1 : N5C1
      {
        var baseTypeRef = ((ClassEntity)semanticGraph.GlobalNamespace.ChildNamespaces[1].ChildNamespaces[0].ChildTypes[1])
          .ChildTypes[0].BaseTypes[0];
        baseTypeRef.ResolvedEntity.FullyQualifiedName.ShouldEqual("global::N4.N5.N5C1");
      }
      // class N5C2C2 : N4C1
      {
        var baseTypeRef = ((ClassEntity)semanticGraph.GlobalNamespace.ChildNamespaces[1].ChildNamespaces[0].ChildTypes[1])
          .ChildTypes[1].BaseTypes[0];
        baseTypeRef.ResolvedEntity.FullyQualifiedName.ShouldEqual("global::N4.N4C1");
      }
      // class N5C2C3 : N5C2C1
      {
        var baseTypeRef = ((ClassEntity)semanticGraph.GlobalNamespace.ChildNamespaces[1].ChildNamespaces[0].ChildTypes[1])
          .ChildTypes[2].BaseTypes[0];
        baseTypeRef.ResolvedEntity.FullyQualifiedName.ShouldEqual("global::N4.N5.N5C2.N5C2C1");
      }
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Error CS0118: 'A' is a 'namespace' but is used like a 'type'
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    [TestMethod]
    public void CS0118_NamespaceIsUsedLikeAType()
    {
      // Set up SyntaxTree and SemanticGraph
      var project = new CSharpProject(WorkingFolder);
      project.AddFile(@"TypeResolution\CS0118_NamespaceIsUsedLikeAType.cs");
      InvokeParser(project).ShouldBeTrue();
      var semanticGraph = new SemanticGraph();
      project.SyntaxTree.AcceptVisitor(new EntityBuilderSyntaxNodeVisitor(semanticGraph,project));
      semanticGraph.AcceptVisitor(new TypeResolverSemanticGraphVisitor(project));

      var baseTypeRef = semanticGraph.GlobalNamespace.ChildTypes[0].BaseTypes[0];
      baseTypeRef.ResolutionState.ShouldEqual(ResolutionState.Unresolvable);

      project.Errors.Count.ShouldEqual(1);
      project.Errors[0].Code.ShouldEqual("CS0118");
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Error CS0246: The type or namespace name 'A' could not be found (are you missing a using directive or an assembly reference?)
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    [TestMethod]
    public void CS0246_TypeOrNamespaceNameCouldNotBeFound()
    {
      // Set up SyntaxTree and SemanticGraph
      var project = new CSharpProject(WorkingFolder);
      project.AddFile(@"TypeResolution\CS0246_TypeOrNamespaceNameCouldNotBeFound.cs");
      InvokeParser(project).ShouldBeTrue();
      var semanticGraph = new SemanticGraph();
      project.SyntaxTree.AcceptVisitor(new EntityBuilderSyntaxNodeVisitor(semanticGraph,project));
      semanticGraph.AcceptVisitor(new TypeResolverSemanticGraphVisitor(project));

      var baseTypeRef = semanticGraph.GlobalNamespace.ChildTypes[0].BaseTypes[0];
      baseTypeRef.ResolutionState.ShouldEqual(ResolutionState.Unresolvable);

      project.Errors.Count.ShouldEqual(1);
      project.Errors[0].Code.ShouldEqual("CS0246");
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Resolving a type to type parameter.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    [TestMethod]
    public void TypeParameter()
    {
      // Set up SyntaxTree and SemanticGraph
      var project = new CSharpProject(WorkingFolder);
      project.AddFile(@"TypeResolution\TypeParameter.cs");
      InvokeParser(project).ShouldBeTrue();
      var semanticGraph = new SemanticGraph();
      project.SyntaxTree.AcceptVisitor(new EntityBuilderSyntaxNodeVisitor(semanticGraph, project));
      semanticGraph.AcceptVisitor(new TypeResolverSemanticGraphVisitor(project));

      var fieldEntity = semanticGraph.GlobalNamespace.ChildTypes[0].Members.ToArray()[0] as FieldEntity;
      fieldEntity.Type.ResolutionState.ShouldEqual(ResolutionState.Resolved);
      ((TypeParameterEntity) fieldEntity.Type.TypeEntity).FullyQualifiedName.ShouldEqual("global::A`1.T1");
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Resolving a constructed generic type.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    [TestMethod]
    public void ConstructedGenericType()
    {
      // Set up SyntaxTree and SemanticGraph
      var project = new CSharpProject(WorkingFolder);
      project.AddFile(@"TypeResolution\ConstructedGenericType.cs");
      InvokeParser(project).ShouldBeTrue();
      var semanticGraph = new SemanticGraph();
      project.SyntaxTree.AcceptVisitor(new EntityBuilderSyntaxNodeVisitor(semanticGraph, project));
      semanticGraph.AcceptVisitor(new TypeResolverSemanticGraphVisitor(project));

      var fieldEntity = semanticGraph.GlobalNamespace.ChildTypes[0].Members.ToArray()[0] as FieldEntity;
      fieldEntity.Type.ResolutionState.ShouldEqual(ResolutionState.Resolved);
      var typeEntity = fieldEntity.Type.TypeEntity as ConstructedGenericTypeEntity;
      typeEntity.ShouldNotBeNull();
      ((ClassEntity)typeEntity.EmbeddedType).FullyQualifiedName.ShouldEqual("global::A2`2");
      ((ClassEntity)typeEntity.EmbeddedType).IsGeneric.ShouldBeTrue();
      var typeParams = ((ClassEntity)typeEntity.EmbeddedType).TypeParameters.ToArray();
      typeParams[0].FullyQualifiedName.ShouldEqual("global::A2`2.T1");
      typeParams[1].FullyQualifiedName.ShouldEqual("global::A2`2.T2");
      var typeArgs = typeEntity.TypeArguments.ToArray();
      typeArgs[0].ResolutionState.ShouldEqual(ResolutionState.Resolved);
      typeArgs[0].TypeEntity.ShouldEqual(semanticGraph.GlobalNamespace.ChildTypes[2]);
      typeArgs[1].ResolutionState.ShouldEqual(ResolutionState.Resolved);
      typeArgs[1].TypeEntity.ShouldEqual(semanticGraph.GlobalNamespace.ChildTypes[3]);
    }
  }
}