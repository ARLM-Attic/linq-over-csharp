using System;
using System.Linq;
using CSharpTreeBuilder.CSharpSemanticGraphBuilder;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SoftwareApproach.TestingExtensions;
using CSharpTreeBuilder.CSharpSemanticGraph;

namespace CSharpTreeBuilderTest.CSharpSemanticGraph
{
  // ================================================================================================
  /// <summary>
  /// Tests SemanticGraph class
  /// </summary>
  // ================================================================================================
  [TestClass]
  public class SemanticGraphTest
  {
    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Tests the GlobalNamespace property.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    [TestMethod]
    public void GlobalNamespace()
    {
      var semanticGraph = new SemanticGraph(null);
      semanticGraph.GlobalNamespace.FullyQualifiedName.ShouldEqual("global");
      semanticGraph.GlobalNamespace.ChildNamespaces.Count.ShouldEqual(0);
      semanticGraph.GlobalNamespace.ChildTypes.Count().ShouldEqual(0);
      semanticGraph.GlobalNamespace.SyntaxNodes.Count.ShouldEqual(0);
      semanticGraph.GlobalNamespace.IsDeclaredInSource.ShouldBeFalse();
      semanticGraph.GlobalNamespace.ReflectedMetadata.ShouldBeNull();
      semanticGraph.GlobalNamespace.Parent.ShouldBeNull();
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Tests the RootNamespaces property.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    [TestMethod]
    public void RootNamespaces()
    {
      var semanticGraph = new SemanticGraph(null);
      semanticGraph.RootNamespaces.Count().ShouldEqual(1);
      semanticGraph.RootNamespaces.ToArray()[0].ShouldEqual(semanticGraph.GlobalNamespace);
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Tests the NullableGenericTypeDefinition property.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    [TestMethod]
    public void NullableGenericTypeDefinition()
    {
      var semanticGraph = new SemanticGraph(null);
      // mscorlib is not yet imported, so this is null 
      semanticGraph.NullableGenericTypeDefinition.ShouldBeNull();
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Tests the SystemArray property.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    [TestMethod]
    public void SystemArray()
    {
      var semanticGraph = new SemanticGraph(null);
      // mscorlib is not yet imported, so this is null 
      semanticGraph.SystemArray.ShouldBeNull();
    }
  }
}