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
  /// Tests the SemanticEntityExtensions class.
  /// </summary>
  // ================================================================================================
  [TestClass]
  public sealed class SemanticEntityExtensionsTest
  {
    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Enclosing type of a namespace should be null.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    [TestMethod]
    public void GetEnclosingType_ForNamespace()
    {
      var semanticGraph = new SemanticGraph();
      var ns = new NamespaceEntity("A");
      semanticGraph.GlobalNamespace.AddChildNamespace(ns);

      ns.GetEnclosing<TypeEntity>().ShouldBeNull();
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Enclosing type of a type should be itself.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    [TestMethod]
    public void GetEnclosingType_ForType()
    {
      var semanticGraph = new SemanticGraph();
      var type = new ClassEntity(null, "Class");
      semanticGraph.GlobalNamespace.AddChildType(type);

      type.GetEnclosing<TypeEntity>().ShouldEqual(type);
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Enclosing type of a field should be the parent type.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    [TestMethod]
    public void GetEnclosingType_ForField()
    {
      var semanticGraph = new SemanticGraph();
      var type = new ClassEntity(null, "Class");
      semanticGraph.GlobalNamespace.AddChildType(type);
      var field = new FieldEntity(true, null, false, null, "Field", null);
      type.AddMember(field);

      field.GetEnclosing<TypeEntity>().ShouldEqual(type);
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// IsInTypeDeclarationBody for a namespace should be false.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    [TestMethod]
    public void IsInTypeDeclarationBody_ForNamespace()
    {
      var semanticGraph = new SemanticGraph();
      var ns = new NamespaceEntity("A");
      semanticGraph.GlobalNamespace.AddChildNamespace(ns);

      ns.IsInTypeDeclarationBody().ShouldBeFalse();
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// IsInTypeDeclarationBody for a using directive should be false.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    [TestMethod]
    public void IsInTypeDeclarationBody_ForUsingNamespace()
    {
      var semanticGraph = new SemanticGraph();
      var ns = new NamespaceEntity("A");
      semanticGraph.GlobalNamespace.AddChildNamespace(ns);
      var usingNamespace = new UsingNamespaceEntity(SourceRegion.GetDummy(), NamespaceOrTypeNameNode.CreateFromDottedName("a"));
      ns.AddUsingNamespace(usingNamespace);

      usingNamespace.IsInTypeDeclarationBody().ShouldBeFalse();
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// IsInTypeDeclarationBody for a type should be false.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    [TestMethod]
    public void IsInTypeDeclarationBody_ForType()
    {
      var semanticGraph = new SemanticGraph();
      var type = new ClassEntity(null, "Class");
      semanticGraph.GlobalNamespace.AddChildType(type);

      type.IsInTypeDeclarationBody().ShouldBeFalse();
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// IsInTypeDeclarationBody for a field should be true.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    [TestMethod]
    public void IsInTypeDeclarationBody_ForField()
    {
      var semanticGraph = new SemanticGraph();
      var type = new ClassEntity(null, "Class");
      semanticGraph.GlobalNamespace.AddChildType(type);
      var field = new FieldEntity(true, null, false, null, "Field", null);
      type.AddMember(field);

      field.IsInTypeDeclarationBody().ShouldBeTrue();
    }
  }
}
