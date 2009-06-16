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
      node.AddTypeTag(new TypeTagNode(new Token(name), null));
      return node;
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Sets the qualifier tag of the TypeOrNamespaceNode.
    /// </summary>
    /// <param name="node">A TypeOrNamespaceNode node</param>
    /// <param name="qualifier">The name of the qualifier</param>
    /// <returns>The TypeOrNamespaceNode parameter is returned to enable method chaining.</returns>
    /// <remarks>If there's no qualifier, then adds one. 
    /// If there's already a qualifier then first deletes it, then adds the new one.</remarks>
    // ----------------------------------------------------------------------------------------------
    public static TypeOrNamespaceNode Qualifier(this TypeOrNamespaceNode node, string qualifier)
    {
      node.QualifierToken = new Token(qualifier);
      return node;
    }
  }
}