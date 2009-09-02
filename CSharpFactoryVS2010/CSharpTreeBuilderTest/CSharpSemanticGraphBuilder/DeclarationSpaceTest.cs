using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SoftwareApproach.TestingExtensions;
using CSharpTreeBuilder.CSharpSemanticGraph;
using CSharpTreeBuilder.CSharpSemanticGraphBuilder;
using System;

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

      declarationSpace.DeclarationCount.ShouldEqual(0);

      declarationSpace.GetDeclarationSpaceEntry(new NamespaceEntity("A")).ShouldBeNull();

      declarationSpace.GetEntities<FieldEntity>("A").ToList().Count.ShouldEqual(0);
      declarationSpace.GetSingleEntity<FieldEntity>("A").ShouldBeNull();

      declarationSpace.GetEntities<ClassEntity>("A", 0).ToList().Count.ShouldEqual(0);
      declarationSpace.GetSingleEntity<ClassEntity>("A", 0).ShouldBeNull();

      declarationSpace.GetEntities<MethodEntity>(new Signature("A", 0, null)).ToList().Count.ShouldEqual(0);
      declarationSpace.GetSingleEntity<MethodEntity>(new Signature("A", 0, null)).ShouldBeNull();
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Entities cannot be registered multiple times.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    [TestMethod]
    [ExpectedException(typeof(InvalidOperationException))]
    public void RegisteredMultipleTimes()
    {
      var declarationSpace = new DeclarationSpace();

      var namespaceEntity = new NamespaceEntity("A");
      declarationSpace.Register(namespaceEntity);
      declarationSpace.Register(namespaceEntity);
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Entities cannot be registered multiple times.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    [TestMethod]
    public void Retrieval()
    {
      // Setting up entities
      var namespaceEntity = new NamespaceEntity("A");
      var typeParameterEntity = new TypeParameterEntity("A");
      var classEntity = new ClassEntity("A");
      classEntity.AddTypeParameter(typeParameterEntity);
      var typeRef = new DirectSemanticEntityReference<TypeEntity>(classEntity);
      var methodEntity = new MethodEntity("A", true, true, false, false, typeRef);
      var parameterEntity = new ParameterEntity("a", typeRef, ParameterKind.Value);
      methodEntity.AddTypeParameter(typeParameterEntity);
      methodEntity.AddParameter(parameterEntity);
      var propertyEntity = new PropertyEntity("A", true, typeRef, false, true);

      var methodSignature = new Signature("A", 1, new List<ParameterEntity> {parameterEntity});

      // Setting up declaration space
      var declarationSpace = new DeclarationSpace();
      declarationSpace.Register(namespaceEntity);
      declarationSpace.Register(typeParameterEntity);
      declarationSpace.Register(classEntity);
      declarationSpace.Register(methodEntity);
      declarationSpace.Register(propertyEntity);

      declarationSpace.DeclarationCount.ShouldEqual(5);

      declarationSpace.GetDeclarationSpaceEntry(namespaceEntity).Entity.ShouldEqual(namespaceEntity);

      // Check GetSingleEntity with the right parameters
      declarationSpace.GetSingleEntity<NamespaceEntity>("A").ShouldEqual(namespaceEntity);
      declarationSpace.GetSingleEntity<TypeParameterEntity>("A").ShouldEqual(typeParameterEntity);
      declarationSpace.GetSingleEntity<ClassEntity>("A", 1).ShouldEqual(classEntity);
      declarationSpace.GetSingleEntity<MethodEntity>(methodSignature).ShouldEqual(methodEntity);
      declarationSpace.GetSingleEntity<PropertyEntity>("A").ShouldEqual(propertyEntity);

      // Get all entities named 'A'
      var entitiesNamedA = declarationSpace.GetEntities<INamedEntity>("A").ToList();
      entitiesNamedA.Count.ShouldEqual(5);
      entitiesNamedA.Contains(namespaceEntity).ShouldBeTrue();
      entitiesNamedA.Contains(typeParameterEntity).ShouldBeTrue();
      entitiesNamedA.Contains(classEntity).ShouldBeTrue();
      entitiesNamedA.Contains(methodEntity).ShouldBeTrue();
      entitiesNamedA.Contains(propertyEntity).ShouldBeTrue();

      // Get all entities named 'A', with 1 type parameter
      var entitiesNamedAWithTypeParam = declarationSpace.GetEntities<INamedEntity>("A",1).ToList();
      entitiesNamedAWithTypeParam.Count.ShouldEqual(2);
      entitiesNamedAWithTypeParam.Contains(classEntity).ShouldBeTrue();
      entitiesNamedAWithTypeParam.Contains(methodEntity).ShouldBeTrue();

      // Get all entities named 'A', with 1 type parameter and a parameter
      var entitiesWithSignature = declarationSpace.GetEntities<IOverloadableEntity>(methodSignature).ToList();
      entitiesWithSignature.Count.ShouldEqual(1);
      entitiesWithSignature.Contains(methodEntity).ShouldBeTrue();
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Tests that method retrievel with different ParamterKind values.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    [TestMethod]
    public void MethodParameterKinds()
    {
      var declarationSpace = new DeclarationSpace();

      var typeRef = new DirectSemanticEntityReference<TypeEntity>(new ClassEntity("A"));
      var methodEntity = new MethodEntity("A", true, true, false, false, typeRef);
      methodEntity.AddTypeParameter(new TypeParameterEntity("T"));
      methodEntity.AddParameter(new ParameterEntity("a", typeRef, ParameterKind.Reference));

      declarationSpace.Register(methodEntity);

      var goodSignature = new Signature("A", 1,
        new List<ParameterEntity> { new ParameterEntity("a", typeRef, ParameterKind.Reference) });

      declarationSpace.GetSingleEntity<MethodEntity>(goodSignature).ShouldEqual(methodEntity);

      var wrongSignature = new Signature("A", 1,
        new List<ParameterEntity> {new ParameterEntity("a", typeRef, ParameterKind.Output)});
      
      declarationSpace.GetSingleEntity<MethodEntity>(wrongSignature).ShouldBeNull();
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Tests unregistration.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    [TestMethod]
    public void Unregister()
    {
      var declarationSpace = new DeclarationSpace();
      var namespaceEntity = new NamespaceEntity("A");
      var namespaceEntity2 = new NamespaceEntity("A");

      declarationSpace.Register(namespaceEntity);
      declarationSpace.Register(namespaceEntity2);

      declarationSpace.DeclarationCount.ShouldEqual(2);

      declarationSpace.Unregister(namespaceEntity);

      declarationSpace.DeclarationCount.ShouldEqual(1);
      declarationSpace.GetDeclarationSpaceEntry(namespaceEntity2).Entity.ShouldEqual(namespaceEntity2);
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Ambigous declaration.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    [TestMethod]
    [ExpectedException(typeof(AmbiguousDeclarationsException))]
    public void AmbiguousDeclaration()
    {
      var declarationSpace = new DeclarationSpace();
      var namespaceEntity = new NamespaceEntity("A");
      var classEntity = new ClassEntity("A");

      declarationSpace.Register(namespaceEntity);
      declarationSpace.Register(classEntity);

      declarationSpace.GetSingleEntity<NamespaceOrTypeEntity>("A");
    }
  }
}
