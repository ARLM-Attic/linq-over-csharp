using CSharpTreeBuilder.Ast;
using CSharpTreeBuilder.CSharpAstBuilder;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SoftwareApproach.TestingExtensions;
using CSharpTreeBuilder.CSharpSemanticGraph;
using CSharpTreeBuilder.CSharpSemanticGraphBuilder;

namespace CSharpTreeBuilderTest.CSharpSemanticGraphBuilder
{
  // ================================================================================================
  /// <summary>
  /// Tests the SemanticEntityReferenceEqualityComparerTest class.
  /// </summary>
  // ================================================================================================
  [TestClass]
  public sealed class SemanticEntityReferenceEqualityComparerTest
  {
    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Same reference should yield true (x=x)
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    [TestMethod]
    public void Reflective()
    {
      var reference = new DummyResolver<NamespaceEntity>(new NamespaceEntity("A"));
      var comparer = new SemanticEntityReferenceEqualityComparer<NamespaceEntity>();
      comparer.Equals(reference, reference).ShouldBeTrue();
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Different target entities should no be equal.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    [TestMethod]
    public void DifferentTargetEntities()
    {
      var reference1 = new DummyResolver<NamespaceEntity>(new NamespaceEntity("A"));
      var reference2 = new DummyResolver<NamespaceEntity>(new NamespaceEntity("B"));
      var comparer = new SemanticEntityReferenceEqualityComparer<NamespaceEntity>();
      comparer.Equals(reference1, reference2).ShouldBeFalse();
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Same target entities should be equal.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    [TestMethod]
    public void SameTargetEntities()
    {
      var namespaceEntity = new NamespaceEntity("A");
      var reference1 = new DummyResolver<NamespaceEntity>(namespaceEntity);
      var reference2 = new DummyResolver<NamespaceEntity>(namespaceEntity);
      var comparer = new SemanticEntityReferenceEqualityComparer<NamespaceEntity>();
      comparer.Equals(reference1, reference2).ShouldBeTrue();
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// References to null target entity should yield false.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    [TestMethod]
    public void NullTargetEntities()
    {
      var reference = new DummyResolver<NamespaceEntity>(null);
      var comparer = new SemanticEntityReferenceEqualityComparer<NamespaceEntity>();
      comparer.Equals(reference, reference).ShouldBeFalse();
    }
  }
}