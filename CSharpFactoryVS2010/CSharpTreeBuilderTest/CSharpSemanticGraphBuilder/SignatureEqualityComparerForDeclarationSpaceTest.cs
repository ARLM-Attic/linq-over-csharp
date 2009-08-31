using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SoftwareApproach.TestingExtensions;
using CSharpTreeBuilder.CSharpSemanticGraph;
using CSharpTreeBuilder.CSharpSemanticGraphBuilder;

namespace CSharpTreeBuilderTest.CSharpSemanticGraphBuilder
{
  // ================================================================================================
  /// <summary>
  /// Tests the SignatureEqualityComparerForDeclarationSpace class.
  /// </summary>
  // ================================================================================================
  [TestClass]
  public sealed class SignatureEqualityComparerForDeclarationSpaceTest
  {
    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Same signature with resolved types should equal to itself
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    [TestMethod]
    public void SignatureWithResolvedType_EqualsToItself()
    {
      var signature = new Signature("M",1,new List<ParameterEntity> 
      {
        new ParameterEntity("a", new DirectSemanticEntityReference<TypeEntity>(new ClassEntity("A")), ParameterKind.Value),
        new ParameterEntity("b", new DirectSemanticEntityReference<TypeEntity>(new ClassEntity("B")), ParameterKind.Reference)
      });
      var comparer = new SignatureEqualityComparerForDeclarationSpace();
      comparer.Equals(signature, signature).ShouldBeTrue();
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Same signature with unresolved types should not equal to itself
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    [TestMethod]
    public void SignatureWithUnresolvedType_DoesNotEqualToItself()
    {
      var signature = new Signature("M", 1, new List<ParameterEntity> 
      {
        new ParameterEntity("a", new ReflectedTypeBasedTypeEntityReference(typeof(int)), ParameterKind.Value),
        new ParameterEntity("b", new ReflectedTypeBasedTypeEntityReference(typeof(long)), ParameterKind.Reference)
      });
      var comparer = new SignatureEqualityComparerForDeclarationSpace();
      comparer.Equals(signature, signature).ShouldBeFalse();
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Two signatures that satisfy every condition to be equal.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    [TestMethod]
    public void EqualSignatures()
    {
      var type1 = new ClassEntity("A");
      var type2 = new ClassEntity("B");
      var type3 = new ClassEntity("C");

      var signature = new Signature("M", 1, new List<ParameterEntity> 
      {
        new ParameterEntity("a", new DirectSemanticEntityReference<TypeEntity>(type1), ParameterKind.Value),
        new ParameterEntity("b", new DirectSemanticEntityReference<TypeEntity>(type2), ParameterKind.Reference),
        new ParameterEntity("c", new DirectSemanticEntityReference<TypeEntity>(type3), ParameterKind.Output)
      });

      var signature2 = new Signature("M", 1, new List<ParameterEntity> 
      {
        new ParameterEntity("a", new DirectSemanticEntityReference<TypeEntity>(type1), ParameterKind.Value),
        new ParameterEntity("b", new DirectSemanticEntityReference<TypeEntity>(type2), ParameterKind.Reference),
        new ParameterEntity("c", new DirectSemanticEntityReference<TypeEntity>(type3), ParameterKind.Output)
      });

      var comparer = new SignatureEqualityComparerForDeclarationSpace();
      comparer.Equals(signature, signature2).ShouldBeTrue();
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Two signatures that satisfy every condition to be equal, and have no parameters.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    [TestMethod]
    public void EqualSignatures_NoParameters()
    {
      var signature = new Signature("M", 1, new List<ParameterEntity>());
      var signature2 = new Signature("M", 1, new List<ParameterEntity>());

      var comparer = new SignatureEqualityComparerForDeclarationSpace();
      comparer.Equals(signature, signature2).ShouldBeTrue();
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Two signatures that differ in name.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    [TestMethod]
    public void NonEqualSignatures_DifferentNames()
    {
      var signature = new Signature("M", 1, new List<ParameterEntity>());
      var signature2 = new Signature("N", 1, new List<ParameterEntity>());

      var comparer = new SignatureEqualityComparerForDeclarationSpace();
      comparer.Equals(signature, signature2).ShouldBeFalse();
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Two signatures that differ in type parameter count.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    [TestMethod]
    public void NonEqualSignatures_DifferentTypeParameterCount()
    {
      var signature = new Signature("M", 1, new List<ParameterEntity>());
      var signature2 = new Signature("M", 0, new List<ParameterEntity>());

      var comparer = new SignatureEqualityComparerForDeclarationSpace();
      comparer.Equals(signature, signature2).ShouldBeFalse();
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Two signatures with different number of parameters are non-equal.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    [TestMethod]
    public void NonEqualSignatures_DifferentNumberOfParameters()
    {
      var type1 = new ClassEntity("A");

      var signature = new Signature("M", 1, new List<ParameterEntity> 
      {
        new ParameterEntity("a", new DirectSemanticEntityReference<TypeEntity>(type1), ParameterKind.Value),
      });

      var signature2 = new Signature("M", 1, new List<ParameterEntity>()); 

      var comparer = new SignatureEqualityComparerForDeclarationSpace();
      comparer.Equals(signature, signature2).ShouldBeFalse();
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Two signatures that only differ in ref/out are considered equal.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    [TestMethod]
    public void EqualSignatures_WithOutAndRef()
    {
      var type1 = new ClassEntity("A");
      var type2 = new ClassEntity("B");
      var type3 = new ClassEntity("C");

      var signature = new Signature("M", 1, new List<ParameterEntity> 
      {
        new ParameterEntity("a", new DirectSemanticEntityReference<TypeEntity>(type1), ParameterKind.Value),
        new ParameterEntity("b", new DirectSemanticEntityReference<TypeEntity>(type2), ParameterKind.Reference),
        new ParameterEntity("c", new DirectSemanticEntityReference<TypeEntity>(type3), ParameterKind.Output)
      });

      var signature2 = new Signature("M", 1, new List<ParameterEntity> 
      {
        new ParameterEntity("a", new DirectSemanticEntityReference<TypeEntity>(type1), ParameterKind.Value),
        new ParameterEntity("b", new DirectSemanticEntityReference<TypeEntity>(type2), ParameterKind.Output),
        new ParameterEntity("c", new DirectSemanticEntityReference<TypeEntity>(type3), ParameterKind.Reference)
      });

      var comparer = new SignatureEqualityComparerForDeclarationSpace();
      comparer.Equals(signature, signature2).ShouldBeTrue();
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Two signatures that only differ in out/value are considered non-equal.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    [TestMethod]
    public void NonEqualSignatures_WithOutAndValue()
    {
      var type1 = new ClassEntity("A");

      var signature = new Signature("M", 1, new List<ParameterEntity> 
      {
        new ParameterEntity("a", new DirectSemanticEntityReference<TypeEntity>(type1), ParameterKind.Value)
      });

      var signature2 = new Signature("M", 1, new List<ParameterEntity> 
      {
        new ParameterEntity("a", new DirectSemanticEntityReference<TypeEntity>(type1), ParameterKind.Output)
      });

      var comparer = new SignatureEqualityComparerForDeclarationSpace();
      comparer.Equals(signature, signature2).ShouldBeFalse();
    }

  }
}
