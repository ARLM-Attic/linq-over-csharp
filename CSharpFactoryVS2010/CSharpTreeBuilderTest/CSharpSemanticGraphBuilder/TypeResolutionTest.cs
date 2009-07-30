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
      InvokeParser(project, true, false).ShouldBeTrue();
      var semanticGraph = new SemanticGraph();
      project.SyntaxTree.AcceptVisitor(new EntityBuilderSyntaxNodeVisitor(project, semanticGraph));
      semanticGraph.AcceptVisitor(new TypeResolverSemanticGraphVisitor(project, semanticGraph));

      // class A1 : C0
      {
        var baseTypeRef = semanticGraph.GlobalNamespace.ChildTypes[0].BaseTypes.ToArray()[0];
        baseTypeRef.TargetEntity.FullyQualifiedName.ShouldEqual("C0");
      }
      // class A2 : C0.C1
      {
        var baseTypeRef = semanticGraph.GlobalNamespace.ChildTypes[1].BaseTypes.ToArray()[0];
        baseTypeRef.TargetEntity.FullyQualifiedName.ShouldEqual("C0.C1");
      }
      // class A3 : N1.N1C0
      {
        var baseTypeRef = semanticGraph.GlobalNamespace.ChildTypes[2].BaseTypes.ToArray()[0];
        baseTypeRef.TargetEntity.FullyQualifiedName.ShouldEqual("N1.N1C0");
      }
      // class A4 : N1.N2.N2C0
      {
        var baseTypeRef = semanticGraph.GlobalNamespace.ChildTypes[3].BaseTypes.ToArray()[0];
        baseTypeRef.TargetEntity.FullyQualifiedName.ShouldEqual("N1.N2.N2C0");
      }
      // class A5 : N1.N2.N2C0.N2C1
      {
        var baseTypeRef = semanticGraph.GlobalNamespace.ChildTypes[4].BaseTypes.ToArray()[0];
        baseTypeRef.TargetEntity.FullyQualifiedName.ShouldEqual("N1.N2.N2C0.N2C1");
      }
      // class N3C1 : N3C2
      {
        var baseTypeRef = semanticGraph.GlobalNamespace.ChildNamespaces[0].ChildTypes[0].BaseTypes.ToArray()[0];
        baseTypeRef.TargetEntity.FullyQualifiedName.ShouldEqual("N3.N3C2");
      }
      // class N5C1 : N4C1
      {
        var baseTypeRef = semanticGraph.GlobalNamespace.ChildNamespaces[1].ChildNamespaces[0].ChildTypes[0].BaseTypes.ToArray()[0];
        baseTypeRef.TargetEntity.FullyQualifiedName.ShouldEqual("N4.N4C1");
      }
      // class N5C2 : N6.N6C1
      {
        var baseTypeRef = semanticGraph.GlobalNamespace.ChildNamespaces[1].ChildNamespaces[0].ChildTypes[1].BaseTypes.ToArray()[0];
        baseTypeRef.TargetEntity.FullyQualifiedName.ShouldEqual("N4.N6.N6C1");
      }
      // class N5C2C1 : N5C1
      {
        var baseTypeRef = ((ClassEntity)semanticGraph.GlobalNamespace.ChildNamespaces[1].ChildNamespaces[0].ChildTypes[1])
          .ChildTypes[0].BaseTypes.ToArray()[0];
        baseTypeRef.TargetEntity.FullyQualifiedName.ShouldEqual("N4.N5.N5C1");
      }
      // class N5C2C2 : N4C1
      {
        var baseTypeRef = ((ClassEntity)semanticGraph.GlobalNamespace.ChildNamespaces[1].ChildNamespaces[0].ChildTypes[1])
          .ChildTypes[1].BaseTypes.ToArray()[0];
        baseTypeRef.TargetEntity.FullyQualifiedName.ShouldEqual("N4.N4C1");
      }
      // class N5C2C3 : N5C2C1
      {
        var baseTypeRef = ((ClassEntity)semanticGraph.GlobalNamespace.ChildNamespaces[1].ChildNamespaces[0].ChildTypes[1])
          .ChildTypes[2].BaseTypes.ToArray()[0];
        baseTypeRef.TargetEntity.FullyQualifiedName.ShouldEqual("N4.N5.N5C2.N5C2C1");
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
      InvokeParser(project, true, false).ShouldBeTrue();
      var semanticGraph = new SemanticGraph();
      project.SyntaxTree.AcceptVisitor(new EntityBuilderSyntaxNodeVisitor(project, semanticGraph));
      semanticGraph.AcceptVisitor(new TypeResolverSemanticGraphVisitor(project, semanticGraph));

      var baseTypeRef = semanticGraph.GlobalNamespace.ChildTypes[0].BaseTypes.ToArray()[0];
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
      InvokeParser(project, true, false).ShouldBeTrue();
      var semanticGraph = new SemanticGraph();
      project.SyntaxTree.AcceptVisitor(new EntityBuilderSyntaxNodeVisitor(project, semanticGraph));
      semanticGraph.AcceptVisitor(new TypeResolverSemanticGraphVisitor(project, semanticGraph));

      var baseTypeRef = semanticGraph.GlobalNamespace.ChildTypes[0].BaseTypes.ToArray()[0];
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
      InvokeParser(project, true, false).ShouldBeTrue();
      var semanticGraph = new SemanticGraph();
      project.SyntaxTree.AcceptVisitor(new EntityBuilderSyntaxNodeVisitor(project, semanticGraph));
      semanticGraph.AcceptVisitor(new TypeResolverSemanticGraphVisitor(project, semanticGraph));

      var fieldEntity = semanticGraph.GlobalNamespace.ChildTypes[0].Members.ToArray()[0] as FieldEntity;
      fieldEntity.Type.ResolutionState.ShouldEqual(ResolutionState.Resolved);
      (fieldEntity.Type.TargetEntity as TypeParameterEntity).FullyQualifiedName.ShouldEqual("A`1.T1");
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
      InvokeParser(project, true, false).ShouldBeTrue();
      var semanticGraph = new SemanticGraph();
      project.SyntaxTree.AcceptVisitor(new EntityBuilderSyntaxNodeVisitor(project, semanticGraph));
      semanticGraph.AcceptVisitor(new TypeResolverSemanticGraphVisitor(project, semanticGraph));

      var fieldEntity = semanticGraph.GlobalNamespace.ChildTypes[0].Members.ToArray()[0] as FieldEntity;
      fieldEntity.Type.ResolutionState.ShouldEqual(ResolutionState.Resolved);
      var typeEntity = fieldEntity.Type.TargetEntity as ConstructedGenericTypeEntity;
      typeEntity.ShouldNotBeNull();
      typeEntity.Name.ShouldEqual("A2");
      typeEntity.DistinctiveName.ShouldEqual("A2`2<A3,A4>");
      typeEntity.FullyQualifiedName.ShouldEqual("A2`2<A3,A4>");
      typeEntity.BaseTypes.ShouldEqual(typeEntity.EmbeddedType.BaseTypes);
      typeEntity.Members.ShouldEqual(typeEntity.EmbeddedType.Members);
      typeEntity.SyntaxNodes.Count.ShouldEqual(0);
      typeEntity.Parent.ShouldEqual(semanticGraph.GlobalNamespace);
      typeEntity.DeclarationSpace.ShouldEqual(typeEntity.EmbeddedType.DeclarationSpace);
      typeEntity.IsPointerType.ShouldBeFalse();
      typeEntity.IsReferenceType.ShouldBeTrue();
      typeEntity.IsValueType.ShouldBeFalse();

      var typeArgs = typeEntity.TypeArguments.ToArray();
      typeArgs[0].ResolutionState.ShouldEqual(ResolutionState.Resolved);
      typeArgs[0].TargetEntity.ShouldEqual(semanticGraph.GlobalNamespace.ChildTypes[2]);
      typeArgs[1].ResolutionState.ShouldEqual(ResolutionState.Resolved);
      typeArgs[1].TargetEntity.ShouldEqual(semanticGraph.GlobalNamespace.ChildTypes[3]);

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
      InvokeParser(project, true, false).ShouldBeTrue();
      var semanticGraph = new SemanticGraph();
      project.SyntaxTree.AcceptVisitor(new EntityBuilderSyntaxNodeVisitor(project, semanticGraph));
      semanticGraph.AcceptVisitor(new TypeResolverSemanticGraphVisitor(project, semanticGraph));

      var fieldEntity = semanticGraph.GlobalNamespace.ChildTypes[0].Members.ToArray()[0] as FieldEntity;
      fieldEntity.Type.ResolutionState.ShouldEqual(ResolutionState.Resolved);
      var typeEntity = fieldEntity.Type.TargetEntity as ConstructedGenericTypeEntity;
      typeEntity.ShouldNotBeNull();
      typeEntity.Name.ShouldEqual("A3");
      typeEntity.DistinctiveName.ShouldEqual("A3`1<A2.A3`1<A4>>");
      typeEntity.FullyQualifiedName.ShouldEqual("A2.A3`1<A2.A3`1<A4>>");
      typeEntity.EmbeddedType.FullyQualifiedName.ShouldEqual("A2.A3`1");

      var typeArgs = typeEntity.TypeArguments.ToArray();
      typeArgs[0].ResolutionState.ShouldEqual(ResolutionState.Resolved);
      var typeArgTypeEntity = typeArgs[0].TargetEntity as ConstructedGenericTypeEntity;
      typeArgTypeEntity.FullyQualifiedName.ShouldEqual("A2.A3`1<A4>");
      typeArgTypeEntity.EmbeddedType.FullyQualifiedName.ShouldEqual("A2.A3`1");

      var typeArgs2 = typeArgTypeEntity.TypeArguments.ToArray();
      typeArgs2[0].ResolutionState.ShouldEqual(ResolutionState.Resolved);
      var typeArgTypeEntity2 = typeArgs2[0].TargetEntity as ClassEntity;
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
      InvokeParser(project, true, false).ShouldBeTrue();
      var semanticGraph = new SemanticGraph();
      project.SyntaxTree.AcceptVisitor(new EntityBuilderSyntaxNodeVisitor(project, semanticGraph));
      semanticGraph.AcceptVisitor(new TypeResolverSemanticGraphVisitor(project, semanticGraph));

      var fieldEntity = semanticGraph.GlobalNamespace.ChildTypes[0].Members.ToArray()[0] as FieldEntity;
      fieldEntity.Type.ResolutionState.ShouldEqual(ResolutionState.Resolved);
      var typeEntity = fieldEntity.Type.TargetEntity as ConstructedGenericTypeEntity;
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
      InvokeParser(project, true, false).ShouldBeTrue();
      var semanticGraph = new SemanticGraph();
      project.SyntaxTree.AcceptVisitor(new EntityBuilderSyntaxNodeVisitor(project, semanticGraph));
      semanticGraph.AcceptVisitor(new TypeResolverSemanticGraphVisitor(project, semanticGraph));

      {
        var fieldEntity = semanticGraph.GlobalNamespace.ChildTypes[0].Members.ToArray()[0] as FieldEntity;
        fieldEntity.Type.ResolutionState.ShouldEqual(ResolutionState.Resolved);
        var array1 = fieldEntity.Type.TargetEntity as ArrayTypeEntity;
        // TODO: check that the base type is System.Array
        array1.Members.Count().ShouldEqual(0);
        array1.DeclarationSpace.ShouldEqual(semanticGraph.GlobalNamespace.ChildTypes[1].DeclarationSpace);
        array1.DistinctiveName.ShouldEqual("A2[][,]");
        array1.FullyQualifiedName.ShouldEqual("A2[][,]");
        array1.Name.ShouldEqual("A2");
        array1.Parent.ShouldEqual(semanticGraph.GlobalNamespace.ChildTypes[1].Parent);
        array1.Rank.ShouldEqual(2);
        array1.SyntaxNodes.Count.ShouldEqual(0);
        array1.IsPointerType.ShouldBeFalse();
        array1.IsReferenceType.ShouldBeTrue();
        array1.IsValueType.ShouldBeFalse();

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
        var array1 = fieldEntity.Type.TargetEntity as ArrayTypeEntity;
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
      InvokeParser(project, true, false).ShouldBeTrue();
      var semanticGraph = new SemanticGraph();
      project.SyntaxTree.AcceptVisitor(new EntityBuilderSyntaxNodeVisitor(project, semanticGraph));
      semanticGraph.AcceptVisitor(new TypeResolverSemanticGraphVisitor(project, semanticGraph));

      var fieldEntity = semanticGraph.GlobalNamespace.ChildTypes[0].Members.ToArray()[0] as FieldEntity;
      fieldEntity.Type.ResolutionState.ShouldEqual(ResolutionState.Resolved);
      var pointer1 = fieldEntity.Type.TargetEntity as PointerToTypeEntity;
      pointer1.BaseTypes.Count().ShouldEqual(0);
      pointer1.Members.Count().ShouldEqual(0);
      pointer1.DeclarationSpace.ShouldEqual(semanticGraph.GlobalNamespace.ChildTypes[1].DeclarationSpace);
      pointer1.DistinctiveName.ShouldEqual("A2**");
      pointer1.FullyQualifiedName.ShouldEqual("A2**");
      pointer1.Name.ShouldEqual("A2");
      pointer1.Parent.ShouldEqual(semanticGraph.GlobalNamespace.ChildTypes[1].Parent);
      pointer1.SyntaxNodes.Count.ShouldEqual(0);
      pointer1.IsPointerType.ShouldBeTrue();
      pointer1.IsReferenceType.ShouldBeFalse();
      pointer1.IsValueType.ShouldBeFalse();

      var pointer2 = pointer1.EmbeddedType as PointerToTypeEntity;
      pointer2.BaseTypes.Count().ShouldEqual(0);
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
      InvokeParser(project, true, false).ShouldBeTrue();
      var semanticGraph = new SemanticGraph();
      project.SyntaxTree.AcceptVisitor(new EntityBuilderSyntaxNodeVisitor(project, semanticGraph));
      semanticGraph.AcceptVisitor(new TypeResolverSemanticGraphVisitor(project, semanticGraph));

      {
        var fieldEntity = semanticGraph.GlobalNamespace.ChildTypes[0].Members.ToArray()[0] as FieldEntity;
        fieldEntity.Type.ResolutionState.ShouldEqual(ResolutionState.Resolved);
        var pointer1 = fieldEntity.Type.TargetEntity as PointerToUnknownTypeEntity;
        pointer1.BaseTypes.Count().ShouldEqual(0);
        pointer1.Members.Count().ShouldEqual(0);
        pointer1.DeclarationSpace.ShouldBeNull();
        pointer1.DistinctiveName.ShouldEqual("void*");
        pointer1.FullyQualifiedName.ShouldEqual("void*");
        pointer1.Name.ShouldEqual("void*");
        pointer1.Parent.ShouldBeNull();
        pointer1.SyntaxNodes.Count.ShouldEqual(0);
        pointer1.IsPointerType.ShouldBeTrue();
        pointer1.IsReferenceType.ShouldBeFalse();
        pointer1.IsValueType.ShouldBeFalse();
      }
      {
        var fieldEntity = semanticGraph.GlobalNamespace.ChildTypes[0].Members.ToArray()[1] as FieldEntity;
        fieldEntity.Type.ResolutionState.ShouldEqual(ResolutionState.Resolved);
        var pointer1 = fieldEntity.Type.TargetEntity as PointerToTypeEntity;
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

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Resolving nullable type references.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    [TestMethod]
    public void NullableType()
    {
      // Set up SyntaxTree and SemanticGraph
      var project = new CSharpProject(WorkingFolder);
      project.AddFile(@"TypeResolution\NullableType.cs");
      InvokeParser(project).ShouldBeTrue();

      // A2? a1;
      {
        var fieldEntity = project.SemanticGraph.GlobalNamespace.ChildTypes[0].Members.ToArray()[0] as FieldEntity;
        fieldEntity.Name.ShouldEqual("a1");
        fieldEntity.Type.ResolutionState.ShouldEqual(ResolutionState.Resolved);
        var nullable = fieldEntity.Type.TargetEntity as NullableTypeEntity;
        nullable.Name.ShouldEqual("A2");
        nullable.DistinctiveName.ShouldEqual("A2?");
        nullable.FullyQualifiedName.ShouldEqual("A2?");
        nullable.Parent.ShouldEqual(project.SemanticGraph.GlobalNamespace);
        nullable.SyntaxNodes.Count.ShouldEqual(0);
        nullable.IsPointerType.ShouldBeFalse();
        nullable.IsReferenceType.ShouldBeFalse();
        nullable.IsValueType.ShouldBeTrue();
        var embedded = nullable.EmbeddedType as StructEntity;
        embedded.FullyQualifiedName.ShouldEqual("A2");

        nullable.AliasToType.ResolutionState.ShouldEqual(ResolutionState.Resolved);
        var alias = nullable.AliasToType.TargetEntity as StructEntity;
        alias.FullyQualifiedName.ShouldEqual("System.Nullable`1");

        nullable.BaseTypes.ShouldEqual(nullable.AliasToType.TargetEntity.BaseTypes);
        nullable.Members.ShouldEqual(nullable.AliasToType.TargetEntity.Members);
        nullable.DeclarationSpace.ShouldEqual(nullable.AliasToType.TargetEntity.DeclarationSpace);
      }
      // A2?[] a2;
      {
        var fieldEntity = project.SemanticGraph.GlobalNamespace.ChildTypes[0].Members.ToArray()[1] as FieldEntity;
        fieldEntity.Name.ShouldEqual("a2");
        fieldEntity.Type.ResolutionState.ShouldEqual(ResolutionState.Resolved);
        var array = fieldEntity.Type.TargetEntity as ArrayTypeEntity;
        var nullable = array.EmbeddedType as NullableTypeEntity;
        nullable.Name.ShouldEqual("A2");
        nullable.DistinctiveName.ShouldEqual("A2?");
        nullable.FullyQualifiedName.ShouldEqual("A2?");
        nullable.Parent.ShouldEqual(project.SemanticGraph.GlobalNamespace);
        nullable.SyntaxNodes.Count.ShouldEqual(0);
        var embedded = nullable.EmbeddedType as StructEntity;
        embedded.FullyQualifiedName.ShouldEqual("A2");

        nullable.AliasToType.ResolutionState.ShouldEqual(ResolutionState.Resolved);
        var alias = nullable.AliasToType.TargetEntity as StructEntity;
        alias.FullyQualifiedName.ShouldEqual("System.Nullable`1");
      }
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Resolving builtin types.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    [TestMethod]
    public void BuiltInTypes()
    {
      // Set up SyntaxTree and SemanticGraph
      var project = new CSharpProject(WorkingFolder);
      project.AddFile(@"TypeResolution\BuiltInTypes.cs");
      InvokeParser(project).ShouldBeTrue();

      int i = 0;

      // sbyte a1;
      {
        var fieldEntity = project.SemanticGraph.GlobalNamespace.ChildTypes[0].Members.ToArray()[i++] as FieldEntity;
        fieldEntity.Name.ShouldEqual("a1");
        fieldEntity.Type.ResolutionState.ShouldEqual(ResolutionState.Resolved);
        var builtin = fieldEntity.Type.TargetEntity as BuiltInTypeEntity;
        builtin.BuiltInType.ShouldEqual(BuiltInType.Sbyte);
        builtin.Name.ShouldEqual("sbyte");
        builtin.DistinctiveName.ShouldEqual("sbyte");
        builtin.FullyQualifiedName.ShouldEqual("sbyte");
        builtin.Parent.ShouldBeNull();
        builtin.SyntaxNodes.Count.ShouldEqual(0);

        builtin.IsFloatingPointType.ShouldBeFalse();
        builtin.IsIntegralType.ShouldBeTrue();
        builtin.IsNumericType.ShouldBeTrue();
        builtin.IsPointerType.ShouldBeFalse();
        builtin.IsReferenceType.ShouldBeFalse();
        builtin.IsSimpleType.ShouldBeTrue();
        builtin.IsValueType.ShouldBeTrue();

        builtin.AliasToType.ResolutionState.ShouldEqual(ResolutionState.Resolved);
        var alias = builtin.AliasToType.TargetEntity as StructEntity;
        alias.FullyQualifiedName.ShouldEqual("System.SByte");

        builtin.BaseTypes.ShouldEqual(builtin.AliasToType.TargetEntity.BaseTypes);
        builtin.Members.ShouldEqual(builtin.AliasToType.TargetEntity.Members);
        builtin.DeclarationSpace.ShouldEqual(builtin.AliasToType.TargetEntity.DeclarationSpace);
      }
      // byte a2;
      {
        var fieldEntity = project.SemanticGraph.GlobalNamespace.ChildTypes[0].Members.ToArray()[i++] as FieldEntity;
        fieldEntity.Name.ShouldEqual("a2");
        fieldEntity.Type.ResolutionState.ShouldEqual(ResolutionState.Resolved);
        var builtin = fieldEntity.Type.TargetEntity as BuiltInTypeEntity;
        builtin.BuiltInType.ShouldEqual(BuiltInType.Byte);
        builtin.Name.ShouldEqual("byte");
        builtin.DistinctiveName.ShouldEqual("byte");
        builtin.FullyQualifiedName.ShouldEqual("byte");
        builtin.Parent.ShouldBeNull();
        builtin.SyntaxNodes.Count.ShouldEqual(0);

        builtin.IsFloatingPointType.ShouldBeFalse();
        builtin.IsIntegralType.ShouldBeTrue();
        builtin.IsNumericType.ShouldBeTrue();
        builtin.IsPointerType.ShouldBeFalse();
        builtin.IsReferenceType.ShouldBeFalse();
        builtin.IsSimpleType.ShouldBeTrue();
        builtin.IsValueType.ShouldBeTrue();

        builtin.AliasToType.ResolutionState.ShouldEqual(ResolutionState.Resolved);
        var alias = builtin.AliasToType.TargetEntity as StructEntity;
        alias.FullyQualifiedName.ShouldEqual("System.Byte");

        builtin.BaseTypes.ShouldEqual(builtin.AliasToType.TargetEntity.BaseTypes);
        builtin.Members.ShouldEqual(builtin.AliasToType.TargetEntity.Members);
        builtin.DeclarationSpace.ShouldEqual(builtin.AliasToType.TargetEntity.DeclarationSpace);
      }
      // short a3;
      {
        var fieldEntity = project.SemanticGraph.GlobalNamespace.ChildTypes[0].Members.ToArray()[i++] as FieldEntity;
        fieldEntity.Name.ShouldEqual("a3");
        fieldEntity.Type.ResolutionState.ShouldEqual(ResolutionState.Resolved);
        var builtin = fieldEntity.Type.TargetEntity as BuiltInTypeEntity;
        builtin.BuiltInType.ShouldEqual(BuiltInType.Short);
        builtin.Name.ShouldEqual("short");
        builtin.DistinctiveName.ShouldEqual("short");
        builtin.FullyQualifiedName.ShouldEqual("short");
        builtin.Parent.ShouldBeNull();
        builtin.SyntaxNodes.Count.ShouldEqual(0);

        builtin.IsFloatingPointType.ShouldBeFalse();
        builtin.IsIntegralType.ShouldBeTrue();
        builtin.IsNumericType.ShouldBeTrue();
        builtin.IsPointerType.ShouldBeFalse();
        builtin.IsReferenceType.ShouldBeFalse();
        builtin.IsSimpleType.ShouldBeTrue();
        builtin.IsValueType.ShouldBeTrue();

        builtin.AliasToType.ResolutionState.ShouldEqual(ResolutionState.Resolved);
        var alias = builtin.AliasToType.TargetEntity as StructEntity;
        alias.FullyQualifiedName.ShouldEqual("System.Int16");

        builtin.BaseTypes.ShouldEqual(builtin.AliasToType.TargetEntity.BaseTypes);
        builtin.Members.ShouldEqual(builtin.AliasToType.TargetEntity.Members);
        builtin.DeclarationSpace.ShouldEqual(builtin.AliasToType.TargetEntity.DeclarationSpace);
      }
      // ushort a4;
      {
        var fieldEntity = project.SemanticGraph.GlobalNamespace.ChildTypes[0].Members.ToArray()[i++] as FieldEntity;
        fieldEntity.Name.ShouldEqual("a4");
        fieldEntity.Type.ResolutionState.ShouldEqual(ResolutionState.Resolved);
        var builtin = fieldEntity.Type.TargetEntity as BuiltInTypeEntity;
        builtin.BuiltInType.ShouldEqual(BuiltInType.Ushort);
        builtin.Name.ShouldEqual("ushort");
        builtin.DistinctiveName.ShouldEqual("ushort");
        builtin.FullyQualifiedName.ShouldEqual("ushort");
        builtin.Parent.ShouldBeNull();
        builtin.SyntaxNodes.Count.ShouldEqual(0);

        builtin.IsFloatingPointType.ShouldBeFalse();
        builtin.IsIntegralType.ShouldBeTrue();
        builtin.IsNumericType.ShouldBeTrue();
        builtin.IsPointerType.ShouldBeFalse();
        builtin.IsReferenceType.ShouldBeFalse();
        builtin.IsSimpleType.ShouldBeTrue();
        builtin.IsValueType.ShouldBeTrue();

        builtin.AliasToType.ResolutionState.ShouldEqual(ResolutionState.Resolved);
        var alias = builtin.AliasToType.TargetEntity as StructEntity;
        alias.FullyQualifiedName.ShouldEqual("System.UInt16");

        builtin.BaseTypes.ShouldEqual(builtin.AliasToType.TargetEntity.BaseTypes);
        builtin.Members.ShouldEqual(builtin.AliasToType.TargetEntity.Members);
        builtin.DeclarationSpace.ShouldEqual(builtin.AliasToType.TargetEntity.DeclarationSpace);
      }
      // int a5;
      {
        var fieldEntity = project.SemanticGraph.GlobalNamespace.ChildTypes[0].Members.ToArray()[i++] as FieldEntity;
        fieldEntity.Name.ShouldEqual("a5");
        fieldEntity.Type.ResolutionState.ShouldEqual(ResolutionState.Resolved);
        var builtin = fieldEntity.Type.TargetEntity as BuiltInTypeEntity;
        builtin.BuiltInType.ShouldEqual(BuiltInType.Int);
        builtin.Name.ShouldEqual("int");
        builtin.DistinctiveName.ShouldEqual("int");
        builtin.FullyQualifiedName.ShouldEqual("int");
        builtin.Parent.ShouldBeNull();
        builtin.SyntaxNodes.Count.ShouldEqual(0);

        builtin.IsFloatingPointType.ShouldBeFalse();
        builtin.IsIntegralType.ShouldBeTrue();
        builtin.IsNumericType.ShouldBeTrue();
        builtin.IsPointerType.ShouldBeFalse();
        builtin.IsReferenceType.ShouldBeFalse();
        builtin.IsSimpleType.ShouldBeTrue();
        builtin.IsValueType.ShouldBeTrue();

        builtin.AliasToType.ResolutionState.ShouldEqual(ResolutionState.Resolved);
        var alias = builtin.AliasToType.TargetEntity as StructEntity;
        alias.FullyQualifiedName.ShouldEqual("System.Int32");

        builtin.BaseTypes.ShouldEqual(builtin.AliasToType.TargetEntity.BaseTypes);
        builtin.Members.ShouldEqual(builtin.AliasToType.TargetEntity.Members);
        builtin.DeclarationSpace.ShouldEqual(builtin.AliasToType.TargetEntity.DeclarationSpace);
      }
      // uint a6;
      {
        var fieldEntity = project.SemanticGraph.GlobalNamespace.ChildTypes[0].Members.ToArray()[i++] as FieldEntity;
        fieldEntity.Name.ShouldEqual("a6");
        fieldEntity.Type.ResolutionState.ShouldEqual(ResolutionState.Resolved);
        var builtin = fieldEntity.Type.TargetEntity as BuiltInTypeEntity;
        builtin.BuiltInType.ShouldEqual(BuiltInType.Uint);
        builtin.Name.ShouldEqual("uint");
        builtin.DistinctiveName.ShouldEqual("uint");
        builtin.FullyQualifiedName.ShouldEqual("uint");
        builtin.Parent.ShouldBeNull();
        builtin.SyntaxNodes.Count.ShouldEqual(0);

        builtin.IsFloatingPointType.ShouldBeFalse();
        builtin.IsIntegralType.ShouldBeTrue();
        builtin.IsNumericType.ShouldBeTrue();
        builtin.IsPointerType.ShouldBeFalse();
        builtin.IsReferenceType.ShouldBeFalse();
        builtin.IsSimpleType.ShouldBeTrue();
        builtin.IsValueType.ShouldBeTrue();

        builtin.AliasToType.ResolutionState.ShouldEqual(ResolutionState.Resolved);
        var alias = builtin.AliasToType.TargetEntity as StructEntity;
        alias.FullyQualifiedName.ShouldEqual("System.UInt32");

        builtin.BaseTypes.ShouldEqual(builtin.AliasToType.TargetEntity.BaseTypes);
        builtin.Members.ShouldEqual(builtin.AliasToType.TargetEntity.Members);
        builtin.DeclarationSpace.ShouldEqual(builtin.AliasToType.TargetEntity.DeclarationSpace);
      }
      // long a7;
      {
        var fieldEntity = project.SemanticGraph.GlobalNamespace.ChildTypes[0].Members.ToArray()[i++] as FieldEntity;
        fieldEntity.Name.ShouldEqual("a7");
        fieldEntity.Type.ResolutionState.ShouldEqual(ResolutionState.Resolved);
        var builtin = fieldEntity.Type.TargetEntity as BuiltInTypeEntity;
        builtin.BuiltInType.ShouldEqual(BuiltInType.Long);
        builtin.Name.ShouldEqual("long");
        builtin.DistinctiveName.ShouldEqual("long");
        builtin.FullyQualifiedName.ShouldEqual("long");
        builtin.Parent.ShouldBeNull();
        builtin.SyntaxNodes.Count.ShouldEqual(0);

        builtin.IsFloatingPointType.ShouldBeFalse();
        builtin.IsIntegralType.ShouldBeTrue();
        builtin.IsNumericType.ShouldBeTrue();
        builtin.IsPointerType.ShouldBeFalse();
        builtin.IsReferenceType.ShouldBeFalse();
        builtin.IsSimpleType.ShouldBeTrue();
        builtin.IsValueType.ShouldBeTrue();

        builtin.AliasToType.ResolutionState.ShouldEqual(ResolutionState.Resolved);
        var alias = builtin.AliasToType.TargetEntity as StructEntity;
        alias.FullyQualifiedName.ShouldEqual("System.Int64");

        builtin.BaseTypes.ShouldEqual(builtin.AliasToType.TargetEntity.BaseTypes);
        builtin.Members.ShouldEqual(builtin.AliasToType.TargetEntity.Members);
        builtin.DeclarationSpace.ShouldEqual(builtin.AliasToType.TargetEntity.DeclarationSpace);
      }
      // ulong a8;
      {
        var fieldEntity = project.SemanticGraph.GlobalNamespace.ChildTypes[0].Members.ToArray()[i++] as FieldEntity;
        fieldEntity.Name.ShouldEqual("a8");
        fieldEntity.Type.ResolutionState.ShouldEqual(ResolutionState.Resolved);
        var builtin = fieldEntity.Type.TargetEntity as BuiltInTypeEntity;
        builtin.BuiltInType.ShouldEqual(BuiltInType.Ulong);
        builtin.Name.ShouldEqual("ulong");
        builtin.DistinctiveName.ShouldEqual("ulong");
        builtin.FullyQualifiedName.ShouldEqual("ulong");
        builtin.Parent.ShouldBeNull();
        builtin.SyntaxNodes.Count.ShouldEqual(0);

        builtin.IsFloatingPointType.ShouldBeFalse();
        builtin.IsIntegralType.ShouldBeTrue();
        builtin.IsNumericType.ShouldBeTrue();
        builtin.IsPointerType.ShouldBeFalse();
        builtin.IsReferenceType.ShouldBeFalse();
        builtin.IsSimpleType.ShouldBeTrue();
        builtin.IsValueType.ShouldBeTrue();

        builtin.AliasToType.ResolutionState.ShouldEqual(ResolutionState.Resolved);
        var alias = builtin.AliasToType.TargetEntity as StructEntity;
        alias.FullyQualifiedName.ShouldEqual("System.UInt64");

        builtin.BaseTypes.ShouldEqual(builtin.AliasToType.TargetEntity.BaseTypes);
        builtin.Members.ShouldEqual(builtin.AliasToType.TargetEntity.Members);
        builtin.DeclarationSpace.ShouldEqual(builtin.AliasToType.TargetEntity.DeclarationSpace);
      }
      // char a9;
      {
        var fieldEntity = project.SemanticGraph.GlobalNamespace.ChildTypes[0].Members.ToArray()[i++] as FieldEntity;
        fieldEntity.Name.ShouldEqual("a9");
        fieldEntity.Type.ResolutionState.ShouldEqual(ResolutionState.Resolved);
        var builtin = fieldEntity.Type.TargetEntity as BuiltInTypeEntity;
        builtin.BuiltInType.ShouldEqual(BuiltInType.Char);
        builtin.Name.ShouldEqual("char");
        builtin.DistinctiveName.ShouldEqual("char");
        builtin.FullyQualifiedName.ShouldEqual("char");
        builtin.Parent.ShouldBeNull();
        builtin.SyntaxNodes.Count.ShouldEqual(0);

        builtin.IsFloatingPointType.ShouldBeFalse();
        builtin.IsIntegralType.ShouldBeTrue();
        builtin.IsNumericType.ShouldBeTrue();
        builtin.IsPointerType.ShouldBeFalse();
        builtin.IsReferenceType.ShouldBeFalse();
        builtin.IsSimpleType.ShouldBeTrue();
        builtin.IsValueType.ShouldBeTrue();

        builtin.AliasToType.ResolutionState.ShouldEqual(ResolutionState.Resolved);
        var alias = builtin.AliasToType.TargetEntity as StructEntity;
        alias.FullyQualifiedName.ShouldEqual("System.Char");

        builtin.BaseTypes.ShouldEqual(builtin.AliasToType.TargetEntity.BaseTypes);
        builtin.Members.ShouldEqual(builtin.AliasToType.TargetEntity.Members);
        builtin.DeclarationSpace.ShouldEqual(builtin.AliasToType.TargetEntity.DeclarationSpace);
      }
      // float a10;
      {
        var fieldEntity = project.SemanticGraph.GlobalNamespace.ChildTypes[0].Members.ToArray()[i++] as FieldEntity;
        fieldEntity.Name.ShouldEqual("a10");
        fieldEntity.Type.ResolutionState.ShouldEqual(ResolutionState.Resolved);
        var builtin = fieldEntity.Type.TargetEntity as BuiltInTypeEntity;
        builtin.BuiltInType.ShouldEqual(BuiltInType.Float);
        builtin.Name.ShouldEqual("float");
        builtin.DistinctiveName.ShouldEqual("float");
        builtin.FullyQualifiedName.ShouldEqual("float");
        builtin.Parent.ShouldBeNull();
        builtin.SyntaxNodes.Count.ShouldEqual(0);

        builtin.IsFloatingPointType.ShouldBeTrue();
        builtin.IsIntegralType.ShouldBeFalse();
        builtin.IsNumericType.ShouldBeTrue();
        builtin.IsPointerType.ShouldBeFalse();
        builtin.IsReferenceType.ShouldBeFalse();
        builtin.IsSimpleType.ShouldBeTrue();
        builtin.IsValueType.ShouldBeTrue();

        builtin.AliasToType.ResolutionState.ShouldEqual(ResolutionState.Resolved);
        var alias = builtin.AliasToType.TargetEntity as StructEntity;
        alias.FullyQualifiedName.ShouldEqual("System.Single");

        builtin.BaseTypes.ShouldEqual(builtin.AliasToType.TargetEntity.BaseTypes);
        builtin.Members.ShouldEqual(builtin.AliasToType.TargetEntity.Members);
        builtin.DeclarationSpace.ShouldEqual(builtin.AliasToType.TargetEntity.DeclarationSpace);
      }
      // double a11;
      {
        var fieldEntity = project.SemanticGraph.GlobalNamespace.ChildTypes[0].Members.ToArray()[i++] as FieldEntity;
        fieldEntity.Name.ShouldEqual("a11");
        fieldEntity.Type.ResolutionState.ShouldEqual(ResolutionState.Resolved);
        var builtin = fieldEntity.Type.TargetEntity as BuiltInTypeEntity;
        builtin.BuiltInType.ShouldEqual(BuiltInType.Double);
        builtin.Name.ShouldEqual("double");
        builtin.DistinctiveName.ShouldEqual("double");
        builtin.FullyQualifiedName.ShouldEqual("double");
        builtin.Parent.ShouldBeNull();
        builtin.SyntaxNodes.Count.ShouldEqual(0);

        builtin.IsFloatingPointType.ShouldBeTrue();
        builtin.IsIntegralType.ShouldBeFalse();
        builtin.IsNumericType.ShouldBeTrue();
        builtin.IsPointerType.ShouldBeFalse();
        builtin.IsReferenceType.ShouldBeFalse();
        builtin.IsSimpleType.ShouldBeTrue();
        builtin.IsValueType.ShouldBeTrue();

        builtin.AliasToType.ResolutionState.ShouldEqual(ResolutionState.Resolved);
        var alias = builtin.AliasToType.TargetEntity as StructEntity;
        alias.FullyQualifiedName.ShouldEqual("System.Double");

        builtin.BaseTypes.ShouldEqual(builtin.AliasToType.TargetEntity.BaseTypes);
        builtin.Members.ShouldEqual(builtin.AliasToType.TargetEntity.Members);
        builtin.DeclarationSpace.ShouldEqual(builtin.AliasToType.TargetEntity.DeclarationSpace);
      }
      // bool a12;
      {
        var fieldEntity = project.SemanticGraph.GlobalNamespace.ChildTypes[0].Members.ToArray()[i++] as FieldEntity;
        fieldEntity.Name.ShouldEqual("a12");
        fieldEntity.Type.ResolutionState.ShouldEqual(ResolutionState.Resolved);
        var builtin = fieldEntity.Type.TargetEntity as BuiltInTypeEntity;
        builtin.BuiltInType.ShouldEqual(BuiltInType.Bool);
        builtin.Name.ShouldEqual("bool");
        builtin.DistinctiveName.ShouldEqual("bool");
        builtin.FullyQualifiedName.ShouldEqual("bool");
        builtin.Parent.ShouldBeNull();
        builtin.SyntaxNodes.Count.ShouldEqual(0);

        builtin.IsFloatingPointType.ShouldBeFalse();
        builtin.IsIntegralType.ShouldBeFalse();
        builtin.IsNumericType.ShouldBeFalse();
        builtin.IsPointerType.ShouldBeFalse();
        builtin.IsReferenceType.ShouldBeFalse();
        builtin.IsSimpleType.ShouldBeTrue();
        builtin.IsValueType.ShouldBeTrue();

        builtin.AliasToType.ResolutionState.ShouldEqual(ResolutionState.Resolved);
        var alias = builtin.AliasToType.TargetEntity as StructEntity;
        alias.FullyQualifiedName.ShouldEqual("System.Boolean");

        builtin.BaseTypes.ShouldEqual(builtin.AliasToType.TargetEntity.BaseTypes);
        builtin.Members.ShouldEqual(builtin.AliasToType.TargetEntity.Members);
        builtin.DeclarationSpace.ShouldEqual(builtin.AliasToType.TargetEntity.DeclarationSpace);
      }
      // decimal a13;
      {
        var fieldEntity = project.SemanticGraph.GlobalNamespace.ChildTypes[0].Members.ToArray()[i++] as FieldEntity;
        fieldEntity.Name.ShouldEqual("a13");
        fieldEntity.Type.ResolutionState.ShouldEqual(ResolutionState.Resolved);
        var builtin = fieldEntity.Type.TargetEntity as BuiltInTypeEntity;
        builtin.BuiltInType.ShouldEqual(BuiltInType.Decimal);
        builtin.Name.ShouldEqual("decimal");
        builtin.DistinctiveName.ShouldEqual("decimal");
        builtin.FullyQualifiedName.ShouldEqual("decimal");
        builtin.Parent.ShouldBeNull();
        builtin.SyntaxNodes.Count.ShouldEqual(0);

        builtin.IsFloatingPointType.ShouldBeFalse();
        builtin.IsIntegralType.ShouldBeFalse();
        builtin.IsNumericType.ShouldBeTrue();
        builtin.IsPointerType.ShouldBeFalse();
        builtin.IsReferenceType.ShouldBeFalse();
        builtin.IsSimpleType.ShouldBeTrue();
        builtin.IsValueType.ShouldBeTrue();

        builtin.AliasToType.ResolutionState.ShouldEqual(ResolutionState.Resolved);
        var alias = builtin.AliasToType.TargetEntity as StructEntity;
        alias.FullyQualifiedName.ShouldEqual("System.Decimal");

        builtin.BaseTypes.ShouldEqual(builtin.AliasToType.TargetEntity.BaseTypes);
        builtin.Members.ShouldEqual(builtin.AliasToType.TargetEntity.Members);
        builtin.DeclarationSpace.ShouldEqual(builtin.AliasToType.TargetEntity.DeclarationSpace);
      }
      // object a14;
      {
        var fieldEntity = project.SemanticGraph.GlobalNamespace.ChildTypes[0].Members.ToArray()[i++] as FieldEntity;
        fieldEntity.Name.ShouldEqual("a14");
        fieldEntity.Type.ResolutionState.ShouldEqual(ResolutionState.Resolved);
        var builtin = fieldEntity.Type.TargetEntity as BuiltInTypeEntity;
        builtin.BuiltInType.ShouldEqual(BuiltInType.Object);
        builtin.Name.ShouldEqual("object");
        builtin.DistinctiveName.ShouldEqual("object");
        builtin.FullyQualifiedName.ShouldEqual("object");
        builtin.Parent.ShouldBeNull();
        builtin.SyntaxNodes.Count.ShouldEqual(0);

        builtin.IsFloatingPointType.ShouldBeFalse();
        builtin.IsIntegralType.ShouldBeFalse();
        builtin.IsNumericType.ShouldBeFalse();
        builtin.IsPointerType.ShouldBeFalse();
        builtin.IsReferenceType.ShouldBeTrue();
        builtin.IsSimpleType.ShouldBeFalse();
        builtin.IsValueType.ShouldBeFalse();

        builtin.AliasToType.ResolutionState.ShouldEqual(ResolutionState.Resolved);
        var alias = builtin.AliasToType.TargetEntity as ClassEntity;
        alias.FullyQualifiedName.ShouldEqual("System.Object");

        builtin.BaseTypes.ShouldEqual(builtin.AliasToType.TargetEntity.BaseTypes);
        builtin.Members.ShouldEqual(builtin.AliasToType.TargetEntity.Members);
        builtin.DeclarationSpace.ShouldEqual(builtin.AliasToType.TargetEntity.DeclarationSpace);
      }
      // string a15;
      {
        var fieldEntity = project.SemanticGraph.GlobalNamespace.ChildTypes[0].Members.ToArray()[i++] as FieldEntity;
        fieldEntity.Name.ShouldEqual("a15");
        fieldEntity.Type.ResolutionState.ShouldEqual(ResolutionState.Resolved);
        var builtin = fieldEntity.Type.TargetEntity as BuiltInTypeEntity;
        builtin.BuiltInType.ShouldEqual(BuiltInType.String);
        builtin.Name.ShouldEqual("string");
        builtin.DistinctiveName.ShouldEqual("string");
        builtin.FullyQualifiedName.ShouldEqual("string");
        builtin.Parent.ShouldBeNull();
        builtin.SyntaxNodes.Count.ShouldEqual(0);

        builtin.IsFloatingPointType.ShouldBeFalse();
        builtin.IsIntegralType.ShouldBeFalse();
        builtin.IsNumericType.ShouldBeFalse();
        builtin.IsPointerType.ShouldBeFalse();
        builtin.IsReferenceType.ShouldBeTrue();
        builtin.IsSimpleType.ShouldBeFalse();
        builtin.IsValueType.ShouldBeFalse();

        builtin.AliasToType.ResolutionState.ShouldEqual(ResolutionState.Resolved);
        var alias = builtin.AliasToType.TargetEntity as ClassEntity;
        alias.FullyQualifiedName.ShouldEqual("System.String");

        builtin.BaseTypes.ShouldEqual(builtin.AliasToType.TargetEntity.BaseTypes);
        builtin.Members.ShouldEqual(builtin.AliasToType.TargetEntity.Members);
        builtin.DeclarationSpace.ShouldEqual(builtin.AliasToType.TargetEntity.DeclarationSpace);
      }

      // int? a16;
      {
        var fieldEntity = project.SemanticGraph.GlobalNamespace.ChildTypes[0].Members.ToArray()[i++] as FieldEntity;
        fieldEntity.Name.ShouldEqual("a16");
        fieldEntity.Type.ResolutionState.ShouldEqual(ResolutionState.Resolved);
        var nullable = fieldEntity.Type.TargetEntity as NullableTypeEntity;
        var builtin = nullable.EmbeddedType as BuiltInTypeEntity;
        builtin.BuiltInType.ShouldEqual(BuiltInType.Int);

        builtin.AliasToType.ResolutionState.ShouldEqual(ResolutionState.Resolved);
        var alias = builtin.AliasToType.TargetEntity as StructEntity;
        alias.FullyQualifiedName.ShouldEqual("System.Int32");
      }

      // int*[] a17;
      {
        var fieldEntity = project.SemanticGraph.GlobalNamespace.ChildTypes[0].Members.ToArray()[i++] as FieldEntity;
        fieldEntity.Name.ShouldEqual("a17");
        fieldEntity.Type.ResolutionState.ShouldEqual(ResolutionState.Resolved);
        var array = fieldEntity.Type.TargetEntity as ArrayTypeEntity;
        var pointer = array.EmbeddedType as PointerToTypeEntity;
        var builtin = pointer.EmbeddedType as BuiltInTypeEntity;
        builtin.BuiltInType.ShouldEqual(BuiltInType.Int);

        builtin.AliasToType.ResolutionState.ShouldEqual(ResolutionState.Resolved);
        var alias = builtin.AliasToType.TargetEntity as StructEntity;
        alias.FullyQualifiedName.ShouldEqual("System.Int32");
      }

    }
  }
}