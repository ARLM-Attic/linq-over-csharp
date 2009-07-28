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
        baseTypeRef.ResolvedEntity.FullyQualifiedName.ShouldEqual("C0");
      }
      // class A2 : C0.C1
      {
        var baseTypeRef = semanticGraph.GlobalNamespace.ChildTypes[1].BaseTypes[0];
        baseTypeRef.ResolvedEntity.FullyQualifiedName.ShouldEqual("C0.C1");
      }
      // class A3 : N1.N1C0
      {
        var baseTypeRef = semanticGraph.GlobalNamespace.ChildTypes[2].BaseTypes[0];
        baseTypeRef.ResolvedEntity.FullyQualifiedName.ShouldEqual("N1.N1C0");
      }
      // class A4 : N1.N2.N2C0
      {
        var baseTypeRef = semanticGraph.GlobalNamespace.ChildTypes[3].BaseTypes[0];
        baseTypeRef.ResolvedEntity.FullyQualifiedName.ShouldEqual("N1.N2.N2C0");
      }
      // class A5 : N1.N2.N2C0.N2C1
      {
        var baseTypeRef = semanticGraph.GlobalNamespace.ChildTypes[4].BaseTypes[0];
        baseTypeRef.ResolvedEntity.FullyQualifiedName.ShouldEqual("N1.N2.N2C0.N2C1");
      }
      // class N3C1 : N3C2
      {
        var baseTypeRef = semanticGraph.GlobalNamespace.ChildNamespaces[0].ChildTypes[0].BaseTypes[0];
        baseTypeRef.ResolvedEntity.FullyQualifiedName.ShouldEqual("N3.N3C2");
      }
      // class N5C1 : N4C1
      {
        var baseTypeRef = semanticGraph.GlobalNamespace.ChildNamespaces[1].ChildNamespaces[0].ChildTypes[0].BaseTypes[0];
        baseTypeRef.ResolvedEntity.FullyQualifiedName.ShouldEqual("N4.N4C1");
      }
      // class N5C2 : N6.N6C1
      {
        var baseTypeRef = semanticGraph.GlobalNamespace.ChildNamespaces[1].ChildNamespaces[0].ChildTypes[1].BaseTypes[0];
        baseTypeRef.ResolvedEntity.FullyQualifiedName.ShouldEqual("N4.N6.N6C1");
      }
      // class N5C2C1 : N5C1
      {
        var baseTypeRef = ((ClassEntity)semanticGraph.GlobalNamespace.ChildNamespaces[1].ChildNamespaces[0].ChildTypes[1])
          .ChildTypes[0].BaseTypes[0];
        baseTypeRef.ResolvedEntity.FullyQualifiedName.ShouldEqual("N4.N5.N5C1");
      }
      // class N5C2C2 : N4C1
      {
        var baseTypeRef = ((ClassEntity)semanticGraph.GlobalNamespace.ChildNamespaces[1].ChildNamespaces[0].ChildTypes[1])
          .ChildTypes[1].BaseTypes[0];
        baseTypeRef.ResolvedEntity.FullyQualifiedName.ShouldEqual("N4.N4C1");
      }
      // class N5C2C3 : N5C2C1
      {
        var baseTypeRef = ((ClassEntity)semanticGraph.GlobalNamespace.ChildNamespaces[1].ChildNamespaces[0].ChildTypes[1])
          .ChildTypes[2].BaseTypes[0];
        baseTypeRef.ResolvedEntity.FullyQualifiedName.ShouldEqual("N4.N5.N5C2.N5C2C1");
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
      ((TypeParameterEntity) fieldEntity.Type.TypeEntity).FullyQualifiedName.ShouldEqual("A`1.T1");
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
      typeEntity.Name.ShouldEqual("A2");
      typeEntity.DistinctiveName.ShouldEqual("A2`2<A3,A4>");
      typeEntity.FullyQualifiedName.ShouldEqual("A2`2<A3,A4>");
      typeEntity.BaseTypes.ShouldEqual(typeEntity.EmbeddedType.BaseTypes);
      typeEntity.Members.ShouldEqual(typeEntity.EmbeddedType.Members);
      typeEntity.SyntaxNodes.Count.ShouldEqual(0);
      typeEntity.Parent.ShouldEqual(semanticGraph.GlobalNamespace);
      typeEntity.DeclarationSpace.ShouldEqual(typeEntity.EmbeddedType.DeclarationSpace);

      var typeArgs = typeEntity.TypeArguments.ToArray();
      typeArgs[0].ResolutionState.ShouldEqual(ResolutionState.Resolved);
      typeArgs[0].TypeEntity.ShouldEqual(semanticGraph.GlobalNamespace.ChildTypes[2]);
      typeArgs[1].ResolutionState.ShouldEqual(ResolutionState.Resolved);
      typeArgs[1].TypeEntity.ShouldEqual(semanticGraph.GlobalNamespace.ChildTypes[3]);

      ((ClassEntity)typeEntity.EmbeddedType).FullyQualifiedName.ShouldEqual("A2`2");
      ((ClassEntity)typeEntity.EmbeddedType).IsGeneric.ShouldBeTrue();
      var typeParams = ((ClassEntity)typeEntity.EmbeddedType).TypeParameters.ToArray();
      typeParams[0].FullyQualifiedName.ShouldEqual("A2`2.T1");
      typeParams[1].FullyQualifiedName.ShouldEqual("A2`2.T2");
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Resolving a nested generic type (a type argument is also a generic type).
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    [TestMethod]
    public void NestedGenericType()
    {
      // Set up SyntaxTree and SemanticGraph
      var project = new CSharpProject(WorkingFolder);
      project.AddFile(@"TypeResolution\NestedGenericType.cs");
      InvokeParser(project).ShouldBeTrue();
      var semanticGraph = new SemanticGraph();
      project.SyntaxTree.AcceptVisitor(new EntityBuilderSyntaxNodeVisitor(semanticGraph, project));
      semanticGraph.AcceptVisitor(new TypeResolverSemanticGraphVisitor(project));

      var fieldEntity = semanticGraph.GlobalNamespace.ChildTypes[0].Members.ToArray()[0] as FieldEntity;
      fieldEntity.Type.ResolutionState.ShouldEqual(ResolutionState.Resolved);
      var typeEntity = fieldEntity.Type.TypeEntity as ConstructedGenericTypeEntity;
      typeEntity.ShouldNotBeNull();
      typeEntity.Name.ShouldEqual("A3");
      typeEntity.DistinctiveName.ShouldEqual("A3`1<A2.A3`1<A4>>");
      typeEntity.FullyQualifiedName.ShouldEqual("A2.A3`1<A2.A3`1<A4>>");
      typeEntity.EmbeddedType.FullyQualifiedName.ShouldEqual("A2.A3`1");

      var typeArgs = typeEntity.TypeArguments.ToArray();
      typeArgs[0].ResolutionState.ShouldEqual(ResolutionState.Resolved);
      var typeArgTypeEntity = typeArgs[0].TypeEntity as ConstructedGenericTypeEntity;
      typeArgTypeEntity.FullyQualifiedName.ShouldEqual("A2.A3`1<A4>");
      typeArgTypeEntity.EmbeddedType.FullyQualifiedName.ShouldEqual("A2.A3`1");

      var typeArgs2 = typeArgTypeEntity.TypeArguments.ToArray();
      typeArgs2[0].ResolutionState.ShouldEqual(ResolutionState.Resolved);
      var typeArgTypeEntity2 = typeArgs2[0].TypeEntity as ClassEntity;
      typeArgTypeEntity2.FullyQualifiedName.ShouldEqual("A4");
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Resolving a chained generic type (the child of a generic is also a generic).
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    [TestMethod]
    public void ChainedGenericType()
    {
      // Set up SyntaxTree and SemanticGraph
      var project = new CSharpProject(WorkingFolder);
      project.AddFile(@"TypeResolution\ChainedGenericType.cs");
      InvokeParser(project).ShouldBeTrue();
      var semanticGraph = new SemanticGraph();
      project.SyntaxTree.AcceptVisitor(new EntityBuilderSyntaxNodeVisitor(semanticGraph, project));
      semanticGraph.AcceptVisitor(new TypeResolverSemanticGraphVisitor(project));

      var fieldEntity = semanticGraph.GlobalNamespace.ChildTypes[0].Members.ToArray()[0] as FieldEntity;
      fieldEntity.Type.ResolutionState.ShouldEqual(ResolutionState.Resolved);
      var typeEntity = fieldEntity.Type.TypeEntity as ConstructedGenericTypeEntity;
      typeEntity.ShouldNotBeNull();
      typeEntity.Name.ShouldEqual("A4");
      typeEntity.DistinctiveName.ShouldEqual("A4`1<A5>");
      typeEntity.FullyQualifiedName.ShouldEqual("A2`1<A3>.A4`1<A5>");
      typeEntity.EmbeddedType.FullyQualifiedName.ShouldEqual("A2`1.A4`1");

      var parentEntity = typeEntity.Parent as ConstructedGenericTypeEntity;
      parentEntity.ShouldNotBeNull();
      parentEntity.Name.ShouldEqual("A2");
      parentEntity.DistinctiveName.ShouldEqual("A2`1<A3>");
      parentEntity.FullyQualifiedName.ShouldEqual("A2`1<A3>");
      parentEntity.EmbeddedType.FullyQualifiedName.ShouldEqual("A2`1");
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Resolving array type references.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    [TestMethod]
    public void ArrayType()
    {
      // Set up SyntaxTree and SemanticGraph
      var project = new CSharpProject(WorkingFolder);
      project.AddFile(@"TypeResolution\ArrayType.cs");
      InvokeParser(project).ShouldBeTrue();
      var semanticGraph = new SemanticGraph();
      project.SyntaxTree.AcceptVisitor(new EntityBuilderSyntaxNodeVisitor(semanticGraph, project));
      semanticGraph.AcceptVisitor(new TypeResolverSemanticGraphVisitor(project));

      {
        var fieldEntity = semanticGraph.GlobalNamespace.ChildTypes[0].Members.ToArray()[0] as FieldEntity;
        fieldEntity.Type.ResolutionState.ShouldEqual(ResolutionState.Resolved);
        var array1 = fieldEntity.Type.TypeEntity as ArrayTypeEntity;
        // TODO: check that the base type is System.Array
        array1.Members.Count().ShouldEqual(0);
        array1.DeclarationSpace.ShouldEqual(semanticGraph.GlobalNamespace.ChildTypes[1].DeclarationSpace);
        array1.DistinctiveName.ShouldEqual("A2[][,]");
        array1.FullyQualifiedName.ShouldEqual("A2[][,]");
        array1.Name.ShouldEqual("A2");
        array1.Parent.ShouldEqual(semanticGraph.GlobalNamespace.ChildTypes[1].Parent);
        array1.Rank.ShouldEqual(2);
        array1.SyntaxNodes.Count.ShouldEqual(0);

        var array2 = array1.EmbeddedType as ArrayTypeEntity;
        // TODO: check that the base type is System.Array
        array1.Members.Count().ShouldEqual(0);
        array2.DeclarationSpace.ShouldEqual(semanticGraph.GlobalNamespace.ChildTypes[1].DeclarationSpace);
        array2.DistinctiveName.ShouldEqual("A2[]");
        array2.FullyQualifiedName.ShouldEqual("A2[]");
        array2.Name.ShouldEqual("A2");
        array2.Parent.ShouldEqual(semanticGraph.GlobalNamespace.ChildTypes[1].Parent);
        array2.Rank.ShouldEqual(1);
        array2.SyntaxNodes.Count.ShouldEqual(0);
      }

      {
        var fieldEntity = semanticGraph.GlobalNamespace.ChildTypes[0].Members.ToArray()[1] as FieldEntity;
        fieldEntity.Type.ResolutionState.ShouldEqual(ResolutionState.Resolved);
        var array1 = fieldEntity.Type.TypeEntity as ArrayTypeEntity;
        array1.FullyQualifiedName.ShouldEqual("A2**[][,]");
        var array2 = array1.EmbeddedType as ArrayTypeEntity;
        array2.FullyQualifiedName.ShouldEqual("A2**[]");
        var pointer1 = array2.EmbeddedType as PointerToTypeEntity;
        pointer1.FullyQualifiedName.ShouldEqual("A2**");
        var pointer2 = pointer1.EmbeddedType as PointerToTypeEntity;
        pointer2.FullyQualifiedName.ShouldEqual("A2*");
        var structEntity = pointer2.EmbeddedType as StructEntity;
        structEntity.FullyQualifiedName.ShouldEqual("A2");
      }

    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Resolving pointer-to-type references.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    [TestMethod]
    public void PointerToType()
    {
      // Set up SyntaxTree and SemanticGraph
      var project = new CSharpProject(WorkingFolder);
      project.AddFile(@"TypeResolution\PointerToType.cs");
      InvokeParser(project).ShouldBeTrue();
      var semanticGraph = new SemanticGraph();
      project.SyntaxTree.AcceptVisitor(new EntityBuilderSyntaxNodeVisitor(semanticGraph, project));
      semanticGraph.AcceptVisitor(new TypeResolverSemanticGraphVisitor(project));

      var fieldEntity = semanticGraph.GlobalNamespace.ChildTypes[0].Members.ToArray()[0] as FieldEntity;
      fieldEntity.Type.ResolutionState.ShouldEqual(ResolutionState.Resolved);
      var pointer1 = fieldEntity.Type.TypeEntity as PointerToTypeEntity;
      pointer1.BaseTypes.Count.ShouldEqual(0);
      pointer1.Members.Count().ShouldEqual(0);
      pointer1.DeclarationSpace.ShouldEqual(semanticGraph.GlobalNamespace.ChildTypes[1].DeclarationSpace);
      pointer1.DistinctiveName.ShouldEqual("A2**");
      pointer1.FullyQualifiedName.ShouldEqual("A2**");
      pointer1.Name.ShouldEqual("A2");
      pointer1.Parent.ShouldEqual(semanticGraph.GlobalNamespace.ChildTypes[1].Parent);
      pointer1.SyntaxNodes.Count.ShouldEqual(0);

      var pointer2 = pointer1.EmbeddedType as PointerToTypeEntity;
      pointer2.BaseTypes.Count.ShouldEqual(0);
      pointer2.Members.Count().ShouldEqual(0);
      pointer2.DeclarationSpace.ShouldEqual(semanticGraph.GlobalNamespace.ChildTypes[1].DeclarationSpace);
      pointer2.DistinctiveName.ShouldEqual("A2*");
      pointer2.FullyQualifiedName.ShouldEqual("A2*");
      pointer2.Name.ShouldEqual("A2");
      pointer2.Parent.ShouldEqual(semanticGraph.GlobalNamespace.ChildTypes[1].Parent);
      pointer2.SyntaxNodes.Count.ShouldEqual(0);
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Resolving pointer-to-unknown (void*) references.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    [TestMethod]
    public void PointerToUnknown()
    {
      // Set up SyntaxTree and SemanticGraph
      var project = new CSharpProject(WorkingFolder);
      project.AddFile(@"TypeResolution\PointerToUnknown.cs");
      InvokeParser(project).ShouldBeTrue();
      var semanticGraph = new SemanticGraph();
      project.SyntaxTree.AcceptVisitor(new EntityBuilderSyntaxNodeVisitor(semanticGraph, project));
      semanticGraph.AcceptVisitor(new TypeResolverSemanticGraphVisitor(project));

      {
        var fieldEntity = semanticGraph.GlobalNamespace.ChildTypes[0].Members.ToArray()[0] as FieldEntity;
        fieldEntity.Type.ResolutionState.ShouldEqual(ResolutionState.Resolved);
        var pointer1 = fieldEntity.Type.TypeEntity as PointerToUnknownTypeEntity;
        pointer1.BaseTypes.Count.ShouldEqual(0);
        pointer1.Members.Count().ShouldEqual(0);
        pointer1.DeclarationSpace.ShouldBeNull();
        pointer1.DistinctiveName.ShouldEqual("void*");
        pointer1.FullyQualifiedName.ShouldEqual("void*");
        pointer1.Name.ShouldEqual("void*");
        pointer1.Parent.ShouldBeNull();
        pointer1.SyntaxNodes.Count.ShouldEqual(0);
      }
      {
        var fieldEntity = semanticGraph.GlobalNamespace.ChildTypes[0].Members.ToArray()[1] as FieldEntity;
        fieldEntity.Type.ResolutionState.ShouldEqual(ResolutionState.Resolved);
        var pointer1 = fieldEntity.Type.TypeEntity as PointerToTypeEntity;
        pointer1.Name.ShouldEqual("void*");
        pointer1.DistinctiveName.ShouldEqual("void**");
        pointer1.FullyQualifiedName.ShouldEqual("void**");
        pointer1.Parent.ShouldBeNull();
        pointer1.DeclarationSpace.ShouldBeNull();
        pointer1.SyntaxNodes.Count.ShouldEqual(0);
        var pointer2 = pointer1.EmbeddedType as PointerToUnknownTypeEntity;
        pointer2.Name.ShouldEqual("void*");
        pointer2.DistinctiveName.ShouldEqual("void*");
        pointer2.FullyQualifiedName.ShouldEqual("void*");
        pointer2.Parent.ShouldBeNull();
        pointer2.DeclarationSpace.ShouldBeNull();
        pointer2.SyntaxNodes.Count.ShouldEqual(0);
      }
    }
  }
}