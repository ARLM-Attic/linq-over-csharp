﻿using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SoftwareApproach.TestingExtensions;
using CSharpTreeBuilder.CSharpSemanticGraph;
using CSharpTreeBuilder.CSharpSemanticGraphBuilder;

namespace CSharpTreeBuilderTest.CSharpSemanticGraphBuilder
{
  // ================================================================================================
  /// <summary>
  /// Tests the SignatureEqualityComparerForCompleteMatching class.
  /// </summary>
  // ================================================================================================
  [TestClass]
  public sealed class SignatureEqualityComparerForCompleteMatchingTest
  {
    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Two signatures that satisfy every condition to be equal.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    [TestMethod]
    public void EqualSignatures()
    {
      var type1 = new ClassEntity(null, "A");
      var type2 = new ClassEntity(null, "B");
      var type3 = new ClassEntity(null, "C");

      var signature = new Signature("M", 1, new List<ParameterEntity> 
      {
        new ParameterEntity("a", new DummyResolver<TypeEntity>(type1), ParameterKind.Value),
        new ParameterEntity("b", new DummyResolver<TypeEntity>(type2), ParameterKind.Reference),
        new ParameterEntity("c", new DummyResolver<TypeEntity>(type3), ParameterKind.Output)
      });

      var signature2 = new Signature("M", 1, new List<ParameterEntity> 
      {
        new ParameterEntity("a", new DummyResolver<TypeEntity>(type1), ParameterKind.Value),
        new ParameterEntity("b", new DummyResolver<TypeEntity>(type2), ParameterKind.Reference),
        new ParameterEntity("c", new DummyResolver<TypeEntity>(type3), ParameterKind.Output)
      });

      var comparer = new SignatureEqualityComparerForCompleteMatching();
      comparer.Equals(signature, signature2).ShouldBeTrue();
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Two signatures that only differ in ref/out are considered non-equal.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    [TestMethod]
    public void EqualSignatures_WithOutAndRef()
    {
      var type1 = new ClassEntity(null, "A");
      var type2 = new ClassEntity(null, "B");
      var type3 = new ClassEntity(null, "C");

      var signature = new Signature("M", 1, new List<ParameterEntity> 
      {
        new ParameterEntity("a", new DummyResolver<TypeEntity>(type1), ParameterKind.Value),
        new ParameterEntity("b", new DummyResolver<TypeEntity>(type2), ParameterKind.Reference),
        new ParameterEntity("c", new DummyResolver<TypeEntity>(type3), ParameterKind.Output)
      });

      var signature2 = new Signature("M", 1, new List<ParameterEntity> 
      {
        new ParameterEntity("a", new DummyResolver<TypeEntity>(type1), ParameterKind.Value),
        new ParameterEntity("b", new DummyResolver<TypeEntity>(type2), ParameterKind.Output),
        new ParameterEntity("c", new DummyResolver<TypeEntity>(type3), ParameterKind.Reference)
      });

      var comparer = new SignatureEqualityComparerForCompleteMatching();
      comparer.Equals(signature, signature2).ShouldBeFalse();
    }
  }
}
