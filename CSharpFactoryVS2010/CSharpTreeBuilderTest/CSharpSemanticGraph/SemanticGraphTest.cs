using System;
using System.Linq;
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
      var semanticGraph = new SemanticGraph();
      semanticGraph.GlobalNamespace.FullyQualifiedName.ShouldEqual("global");
      semanticGraph.GlobalNamespace.ChildNamespaces.Count.ShouldEqual(0);
      semanticGraph.GlobalNamespace.ChildTypes.Count.ShouldEqual(0);
      semanticGraph.GlobalNamespace.SyntaxNodes.Count.ShouldEqual(0);
      semanticGraph.GlobalNamespace.IsExplicit.ShouldBeFalse();
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
      var semanticGraph = new SemanticGraph();
      semanticGraph.RootNamespaces.Count().ShouldEqual(1);
      semanticGraph.RootNamespaces.ToArray()[0].ShouldEqual(semanticGraph.GlobalNamespace);
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Tests the BuiltInTypes property.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    [TestMethod]
    public void BuiltInTypes()
    {
      var semanticGraph = new SemanticGraph();
      var builtInTypes = semanticGraph.BuiltInTypes.ToArray();
      builtInTypes.Length.ShouldEqual(15);
      // mscorlib is not yet imported, so alias is null
      builtInTypes[0].AliasedType.ShouldBeNull();
      builtInTypes[0].BaseTypeReferences.Count().ShouldEqual(0);
      builtInTypes[0].Members.Count().ShouldEqual(0);
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Tests the NullableGenericTypeDefinition property.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    [TestMethod]
    public void NullableGenericTypeDefinition()
    {
      var semanticGraph = new SemanticGraph();
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
      var semanticGraph = new SemanticGraph();
      // mscorlib is not yet imported, so this is null 
      semanticGraph.SystemArray.ShouldBeNull();
    }


    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Tests the PointerToUnknownType property.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    [TestMethod]
    public void PointerToUnknownType()
    {
      var semanticGraph = new SemanticGraph();
      semanticGraph.PointerToUnknownType.FullyQualifiedName.ShouldEqual("void*");
      semanticGraph.PointerToUnknownType.IsPointerType.ShouldBeTrue();
    }
  }
}