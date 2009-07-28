using Microsoft.VisualStudio.TestTools.UnitTesting;
using SoftwareApproach.TestingExtensions;
using CSharpTreeBuilder.Ast;
using CSharpTreeBuilder.CSharpAstBuilder;
using CSharpTreeBuilder.CSharpSemanticGraph;

namespace CSharpTreeBuilderTest.CSharpSemanticGraph
{
  // ================================================================================================
  /// <summary>
  /// Tests the NamespaceOrTypeEntityReference class.
  /// </summary>
  // ================================================================================================
  [TestClass]
  public class NamespaceOrTypeEntityReferenceTest
  {
    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Tests the state of a reference that is not yet resolved.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    [TestMethod]
    public void NotYetResolved()
    {
      var typeOrNamespaceNode = new TypeOrNamespaceNode();
      typeOrNamespaceNode.AddTypeTag(new TypeTagNode("a"));
      var namespaceOrTypeEntityReference = new NamespaceOrTypeEntityReference(typeOrNamespaceNode);
      
      namespaceOrTypeEntityReference.ResolutionState.ShouldEqual(ResolutionState.NotYetResolved);
      namespaceOrTypeEntityReference.SyntaxNode.TypeTags[0].Identifier.ShouldEqual("a");
      namespaceOrTypeEntityReference.ResolvedEntity.ShouldBeNull();
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Tests the state of a reference that is resolved.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    [TestMethod]
    public void Resolved()
    {
      var typeOrNamespaceNode = new TypeOrNamespaceNode();
      typeOrNamespaceNode.AddTypeTag(new TypeTagNode("a"));
      var namespaceOrTypeEntityReference = new NamespaceOrTypeEntityReference(typeOrNamespaceNode);
      namespaceOrTypeEntityReference.SetResolved(new ClassEntity() { Name = "A" });

      namespaceOrTypeEntityReference.ResolutionState.ShouldEqual(ResolutionState.Resolved);
      namespaceOrTypeEntityReference.SyntaxNode.TypeTags[0].Identifier.ShouldEqual("a");
      namespaceOrTypeEntityReference.ResolvedEntity.Name.ShouldEqual("A");
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Tests the state of a reference that is unresolvable.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    [TestMethod]
    public void Unresolvable()
    {
      var typeOrNamespaceNode = new TypeOrNamespaceNode();
      typeOrNamespaceNode.AddTypeTag(new TypeTagNode("a"));
      var namespaceOrTypeEntityReference = new NamespaceOrTypeEntityReference(typeOrNamespaceNode);
      namespaceOrTypeEntityReference.SetUnresolvable();

      namespaceOrTypeEntityReference.ResolutionState.ShouldEqual(ResolutionState.Unresolvable);
      namespaceOrTypeEntityReference.SyntaxNode.TypeTags[0].Identifier.ShouldEqual("a");
      namespaceOrTypeEntityReference.ResolvedEntity.ShouldBeNull();
    }
  }
}
