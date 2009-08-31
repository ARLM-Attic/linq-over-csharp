using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SoftwareApproach.TestingExtensions;
using CSharpTreeBuilder.CSharpSemanticGraph;
using CSharpTreeBuilder.CSharpSemanticGraphBuilder;

namespace CSharpTreeBuilderTest.CSharpSemanticGraphBuilder
{
  // ================================================================================================
  /// <summary>
  /// Tests declaration space.
  /// </summary>
  // ================================================================================================
  [TestClass]
  public class DeclarationSpaceTest
  {
    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Tests the properties of an empty declaration space.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    [TestMethod]
    public void Empty()
    {
      var declarationSpace = new DeclarationSpace();

      declarationSpace.SlotCount.ShouldEqual(0);
      declarationSpace.EntityCount.ShouldEqual(0);

      declarationSpace.FindEntityByName<FieldEntity>("A").ShouldBeNull();
      declarationSpace.FindEntityByNameAndTypeParameterCount<TypeEntity>("A", 1).ShouldBeNull();
      declarationSpace.FindEntityBySignature<MethodEntity>(new Signature("A", 1, null)).ShouldBeNull();
      
      declarationSpace.AllowsDeclaration<FieldEntity>("A").ShouldBeTrue();
      declarationSpace.AllowsDeclaration<ClassEntity>("A", 1).ShouldBeTrue();
      declarationSpace.AllowsDeclaration<MethodEntity>(new Signature("A", 1, null)).ShouldBeTrue();
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Tests the properties of a declaration space with a registered field entity.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    [TestMethod]
    public void RegisteredFieldEntity()
    {
      var declarationSpace = new DeclarationSpace();
      var fieldEntity = new FieldEntity("A", true, new DirectSemanticEntityReference<TypeEntity>(new ClassEntity("A")),
                                        false);
      declarationSpace.Register(fieldEntity);

      declarationSpace.SlotCount.ShouldEqual(1);
      declarationSpace.EntityCount.ShouldEqual(1);

      declarationSpace.FindEntityByName<FieldEntity>("A").ShouldEqual(fieldEntity);
      declarationSpace.FindEntityByName<FieldEntity>("X").ShouldBeNull();
      
      declarationSpace.AllowsDeclaration<FieldEntity>("A").ShouldBeFalse();
      declarationSpace.AllowsDeclaration<NamespaceEntity>("A").ShouldBeFalse();
      declarationSpace.AllowsDeclaration<ClassEntity>("A").ShouldBeFalse();
      declarationSpace.AllowsDeclaration<MethodEntity>(new Signature("A", 0, null)).ShouldBeFalse();

      declarationSpace.AllowsDeclaration<FieldEntity>("X").ShouldBeTrue();
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Tests the properties of a declaration space with a registered type parameter entity.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    [TestMethod]
    public void RegisteredTypeParameterEntity()
    {
      var declarationSpace = new DeclarationSpace();
      var typeParameterEntity = new TypeParameterEntity("A");
      declarationSpace.Register(typeParameterEntity);

      declarationSpace.SlotCount.ShouldEqual(1);
      declarationSpace.EntityCount.ShouldEqual(1);

      declarationSpace.FindEntityByName<TypeParameterEntity>("A").ShouldEqual(typeParameterEntity);
      declarationSpace.FindEntityByName<TypeParameterEntity>("X").ShouldBeNull();

      declarationSpace.AllowsDeclaration<TypeParameterEntity>("A").ShouldBeFalse();
      declarationSpace.AllowsDeclaration<FieldEntity>("A").ShouldBeFalse();
      declarationSpace.AllowsDeclaration<NamespaceEntity>("A").ShouldBeFalse();
      declarationSpace.AllowsDeclaration<ClassEntity>("A").ShouldBeFalse();
      declarationSpace.AllowsDeclaration<MethodEntity>(new Signature("A", 0, null)).ShouldBeFalse();

      declarationSpace.AllowsDeclaration<TypeParameterEntity>("X").ShouldBeTrue();
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Tests the properties of a declaration space with a registered namespace entity.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    [TestMethod]
    public void RegisteredNamespaceEntity()
    {
      var declarationSpace = new DeclarationSpace();
      var namespaceEntity = new NamespaceEntity("A");
      declarationSpace.Register(namespaceEntity);

      declarationSpace.SlotCount.ShouldEqual(1);
      declarationSpace.EntityCount.ShouldEqual(1);

      declarationSpace.FindEntityByName<NamespaceEntity>("A").ShouldEqual(namespaceEntity);
      declarationSpace.FindEntityByNameAndTypeParameterCount<NamespaceEntity>("A", 0).ShouldEqual(namespaceEntity);

      declarationSpace.FindEntityByName<NamespaceEntity>("X").ShouldBeNull();
      declarationSpace.FindEntityByNameAndTypeParameterCount<NamespaceEntity>("X", 0).ShouldBeNull();
      declarationSpace.FindEntityByNameAndTypeParameterCount<NamespaceEntity>("A", 1).ShouldBeNull();
      
      declarationSpace.AllowsDeclaration<NamespaceEntity>("A").ShouldBeFalse();
      declarationSpace.AllowsDeclaration<FieldEntity>("A").ShouldBeFalse();
      declarationSpace.AllowsDeclaration<ClassEntity>("A").ShouldBeFalse();
      declarationSpace.AllowsDeclaration<ClassEntity>("A", 0).ShouldBeFalse();
      declarationSpace.AllowsDeclaration<MethodEntity>(new Signature("A", 0, null)).ShouldBeFalse();

      declarationSpace.AllowsDeclaration<NamespaceEntity>("X").ShouldBeTrue();
      declarationSpace.AllowsDeclaration<ClassEntity>("A", 1).ShouldBeTrue();
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Tests the properties of a declaration space with a registered generic class entity.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    [TestMethod]
    public void RegisteredClassEntityWithTypeParameter()
    {
      var declarationSpace = new DeclarationSpace();
      var classEntity = new ClassEntity("A");
      classEntity.AddTypeParameter(new TypeParameterEntity("T"));
      declarationSpace.Register(classEntity);

      declarationSpace.SlotCount.ShouldEqual(1);
      declarationSpace.EntityCount.ShouldEqual(1);

      declarationSpace.FindEntityByNameAndTypeParameterCount<ClassEntity>("A", 1).ShouldEqual(classEntity);

      declarationSpace.FindEntityByName<ClassEntity>("A").ShouldBeNull();
      declarationSpace.FindEntityByNameAndTypeParameterCount<ClassEntity>("X", 1).ShouldBeNull();

      declarationSpace.AllowsDeclaration<FieldEntity>("A").ShouldBeFalse();
      declarationSpace.AllowsDeclaration<MethodEntity>(new Signature("A", 0, null)).ShouldBeFalse();
      declarationSpace.AllowsDeclaration<TypeParameterEntity>("A").ShouldBeFalse();
      declarationSpace.AllowsDeclaration<ClassEntity>("A", 1).ShouldBeFalse();

      declarationSpace.AllowsDeclaration<NamespaceEntity>("A").ShouldBeTrue();
      declarationSpace.AllowsDeclaration<ClassEntity>("A").ShouldBeTrue();
      declarationSpace.AllowsDeclaration<ClassEntity>("A", 2).ShouldBeTrue();

    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Tests the properties of a declaration space with a registered method entity.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    [TestMethod]
    public void RegisteredMethod()
    {
      var classA = new ClassEntity("A");
      var classB = new ClassEntity("B");
      var classC = new ClassEntity("C");

      var declarationSpace = new DeclarationSpace();
      var methodEntity = new MethodEntity("A", true, false, false, false, null);
      methodEntity.AddTypeParameter(new TypeParameterEntity("T"));
      methodEntity.AddParameter(new ParameterEntity("a", new DirectSemanticEntityReference<TypeEntity>(classA),
                                                    ParameterKind.Value));
      methodEntity.AddParameter(new ParameterEntity("b", new DirectSemanticEntityReference<TypeEntity>(classB),
                                                    ParameterKind.Reference));
      methodEntity.AddParameter(new ParameterEntity("c", new DirectSemanticEntityReference<TypeEntity>(classC),
                                                    ParameterKind.Output));
      declarationSpace.Register(methodEntity);

      declarationSpace.SlotCount.ShouldEqual(1);
      declarationSpace.EntityCount.ShouldEqual(1);

      declarationSpace.FindEntityByName<MethodEntity>("A").ShouldBeNull();

      // Matching signature
      declarationSpace.FindEntityBySignature<MethodEntity>(new Signature("A", 1, new List<ParameterEntity>
      {
        new ParameterEntity("a2", new DirectSemanticEntityReference<TypeEntity>(classA), ParameterKind.Value),
        new ParameterEntity("b2", new DirectSemanticEntityReference<TypeEntity>(classB), ParameterKind.Reference),
        new ParameterEntity("c2", new DirectSemanticEntityReference<TypeEntity>(classC), ParameterKind.Output)
      })).ShouldEqual(methodEntity);

      // Method name mismatch
      declarationSpace.FindEntityBySignature<MethodEntity>(new Signature("B", 1, new List<ParameterEntity>
      {
        new ParameterEntity("a2", new DirectSemanticEntityReference<TypeEntity>(classA), ParameterKind.Value),
        new ParameterEntity("b2", new DirectSemanticEntityReference<TypeEntity>(classB), ParameterKind.Reference),
        new ParameterEntity("c2", new DirectSemanticEntityReference<TypeEntity>(classC), ParameterKind.Output)
      })).ShouldBeNull();

      // Type parameter count mismatch
      declarationSpace.FindEntityBySignature<MethodEntity>(new Signature("A", 0, new List<ParameterEntity>
      {
        new ParameterEntity("a2", new DirectSemanticEntityReference<TypeEntity>(classA), ParameterKind.Value),
        new ParameterEntity("b2", new DirectSemanticEntityReference<TypeEntity>(classB), ParameterKind.Reference),
        new ParameterEntity("c2", new DirectSemanticEntityReference<TypeEntity>(classC), ParameterKind.Output)
      })).ShouldBeNull();

      // Parameter type mismatch
      declarationSpace.FindEntityBySignature<MethodEntity>(new Signature("A", 1, new List<ParameterEntity>
      {
        new ParameterEntity("a2", new DirectSemanticEntityReference<TypeEntity>(classA), ParameterKind.Value),
        new ParameterEntity("b2", new DirectSemanticEntityReference<TypeEntity>(classA), ParameterKind.Reference),
        new ParameterEntity("c2", new DirectSemanticEntityReference<TypeEntity>(classC), ParameterKind.Output)
      })).ShouldBeNull();

      // Parameter kind mismatch value/ref
      declarationSpace.FindEntityBySignature<MethodEntity>(new Signature("A", 1, new List<ParameterEntity>
      {
        new ParameterEntity("a2", new DirectSemanticEntityReference<TypeEntity>(classA), ParameterKind.Reference),
        new ParameterEntity("b2", new DirectSemanticEntityReference<TypeEntity>(classB), ParameterKind.Reference),
        new ParameterEntity("c2", new DirectSemanticEntityReference<TypeEntity>(classC), ParameterKind.Output)
      })).ShouldBeNull();

      // Parameter kind mismatch ref/out
      declarationSpace.FindEntityBySignature<MethodEntity>(new Signature("A", 1, new List<ParameterEntity>
      {
        new ParameterEntity("a2", new DirectSemanticEntityReference<TypeEntity>(classA), ParameterKind.Value),
        new ParameterEntity("b2", new DirectSemanticEntityReference<TypeEntity>(classB), ParameterKind.Output),
        new ParameterEntity("c2", new DirectSemanticEntityReference<TypeEntity>(classC), ParameterKind.Reference)
      })).ShouldBeNull();

      declarationSpace.AllowsDeclaration<FieldEntity>("A").ShouldBeFalse();
      declarationSpace.AllowsDeclaration<TypeParameterEntity>("A").ShouldBeFalse();
      declarationSpace.AllowsDeclaration<ClassEntity>("A", 1).ShouldBeFalse();

      // Same signature cannot be declared
      declarationSpace.AllowsDeclaration<MethodEntity>(new Signature("A", 1, new List<ParameterEntity>
      {
        new ParameterEntity("a2", new DirectSemanticEntityReference<TypeEntity>(classA), ParameterKind.Value),
        new ParameterEntity("b2", new DirectSemanticEntityReference<TypeEntity>(classB), ParameterKind.Reference),
        new ParameterEntity("c2", new DirectSemanticEntityReference<TypeEntity>(classC), ParameterKind.Output)
      })).ShouldBeFalse();

      // Signature that differs only in ref/out cannot be declared
      declarationSpace.AllowsDeclaration<MethodEntity>(new Signature("A", 1, new List<ParameterEntity>
      {
        new ParameterEntity("a2", new DirectSemanticEntityReference<TypeEntity>(classA), ParameterKind.Value),
        new ParameterEntity("b2", new DirectSemanticEntityReference<TypeEntity>(classB), ParameterKind.Output),
        new ParameterEntity("c2", new DirectSemanticEntityReference<TypeEntity>(classC), ParameterKind.Reference)
      })).ShouldBeFalse();

      // Allowed: signature with different name
      declarationSpace.AllowsDeclaration<MethodEntity>(new Signature("X", 1, new List<ParameterEntity>
      {
        new ParameterEntity("a2", new DirectSemanticEntityReference<TypeEntity>(classA), ParameterKind.Value),
        new ParameterEntity("b2", new DirectSemanticEntityReference<TypeEntity>(classB), ParameterKind.Reference),
        new ParameterEntity("c2", new DirectSemanticEntityReference<TypeEntity>(classC), ParameterKind.Output)
      })).ShouldBeTrue();

      // Allowed: signature with different number of type parameters
      declarationSpace.AllowsDeclaration<MethodEntity>(new Signature("A", 0, new List<ParameterEntity>
      {
        new ParameterEntity("a2", new DirectSemanticEntityReference<TypeEntity>(classA), ParameterKind.Value),
        new ParameterEntity("b2", new DirectSemanticEntityReference<TypeEntity>(classB), ParameterKind.Reference),
        new ParameterEntity("c2", new DirectSemanticEntityReference<TypeEntity>(classC), ParameterKind.Output)
      })).ShouldBeTrue();

      // Allowed: signature with different parameter kind (not ref/out)
      declarationSpace.AllowsDeclaration<MethodEntity>(new Signature("A", 1, new List<ParameterEntity>
      {
        new ParameterEntity("a2", new DirectSemanticEntityReference<TypeEntity>(classA), ParameterKind.Reference),
        new ParameterEntity("b2", new DirectSemanticEntityReference<TypeEntity>(classB), ParameterKind.Reference),
        new ParameterEntity("c2", new DirectSemanticEntityReference<TypeEntity>(classC), ParameterKind.Output)
      })).ShouldBeTrue();
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Tests the properties of a declaration space with a registered method entity 
    /// that has zero type parameters and has zero parameters.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    [TestMethod]
    public void RegisteredMethodWithZeroTypeParametersAndNoParameters()
    {
      var declarationSpace = new DeclarationSpace();
      var methodEntity = new MethodEntity("A", true, false, false, false, null);
      declarationSpace.Register(methodEntity);

      declarationSpace.SlotCount.ShouldEqual(1);
      declarationSpace.EntityCount.ShouldEqual(1);

      declarationSpace.FindEntityByName<MethodEntity>("A").ShouldEqual(methodEntity);
    }
  }
}
