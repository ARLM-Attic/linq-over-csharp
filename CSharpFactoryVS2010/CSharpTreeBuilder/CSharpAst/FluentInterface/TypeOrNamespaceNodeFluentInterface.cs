using CSharpTreeBuilder.CSharpAstBuilder;

namespace CSharpTreeBuilder.Ast
{
  // ================================================================================================
  /// <summary>
  /// A fluent interface for creating TypeOrNamespaceNode objects, implemented with extension methods.
  /// </summary>
  // ================================================================================================
  public static class TypeOrNamespaceNodeFluentInterface
  {
    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Creates a new TypeTag and adds it to a TypeOrNamespaceNode.
    /// </summary>
    /// <param name="node">A TypeOrNamespaceNode node</param>
    /// <param name="names">Any number of name parts.</param>
    /// <returns>The TypeOrNamespaceNode parameter is returned to enable method chaining.</returns>
    // ----------------------------------------------------------------------------------------------
    public static TypeOrNamespaceNode TypeTag(this TypeOrNamespaceNode node, params string[] names)
    {
      foreach (var name in names)
      {
        node.AddTypeTag(new TypeTagNode(new Token(name), null));
      }
      return node;
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Creates a new TypeTag and adds it to a TypeOrNamespaceNode.
    /// </summary>
    /// <param name="node">A TypeOrNamespaceNode node</param>
    /// <param name="typeTagNodes">Any number of TypeTagNode objects.</param>
    /// <returns>The TypeOrNamespaceNode parameter is returned to enable method chaining.</returns>
    // ----------------------------------------------------------------------------------------------
    public static TypeOrNamespaceNode TypeTag(this TypeOrNamespaceNode node, params TypeTagNode[] typeTagNodes)
    {
      foreach (var typeTagNode in typeTagNodes)
      {
        node.AddTypeTag(typeTagNode);
      }
      return node;
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Sets the qualifier tag of the TypeOrNamespaceNode.
    /// </summary>
    /// <param name="node">A TypeOrNamespaceNode node</param>
    /// <param name="qualifier">The name of the qualifier</param>
    /// <returns>The TypeOrNamespaceNode parameter is returned to enable method chaining.</returns>
    // ----------------------------------------------------------------------------------------------
    public static TypeOrNamespaceNode Qualifier(this TypeOrNamespaceNode node, string qualifier)
    {
      node.QualifierToken = new Token(qualifier);
      return node;
    }
  }
}