using CSharpTreeBuilder.CSharpAstBuilder;

namespace CSharpTreeBuilder.Ast
{
  // ================================================================================================
  /// <summary>
  /// A fluent interface for creating TypeOrNamespaceNode objects, implemented with extension methods.
  /// </summary>
  // ================================================================================================
  public static class TypeOrNamespaceNodeExtension
  {
    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Creates a new TypeTag and adds it to a TypeOrNamespaceNode.
    /// </summary>
    /// <param name="node">A TypeOrNamespaceNode node</param>
    /// <param name="name">The name of the TypeTag</param>
    /// <returns>The TypeOrNamespaceNode parameter is returned to enable method chaining.</returns>
    // ----------------------------------------------------------------------------------------------
    public static TypeOrNamespaceNode TypeTag(this TypeOrNamespaceNode node, string name)
    {
      var typeTagNode = new TypeTagNode(new Token(name), null);
      node.TypeTags.Add(Token.Dot, typeTagNode);
      return node;
    }

    public static TypeOrNamespaceNode Qualifier(this TypeOrNamespaceNode node, string qualifier)
    {
      node.TypeTags.Insert(0,new TypeTagNode(new Token(qualifier),null ));
      return node;
    }
  }
}