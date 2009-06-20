namespace CSharpTreeBuilder.Ast
{
  // ================================================================================================
  /// <summary>
  /// A fluent interface for creating UsingNamespaceNode objects, implemented with extension methods.
  /// </summary>
  // ================================================================================================
  public static class UsingNamespaceNodeFluentInterface
  {
    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Adds a TypeTag to a UsingNamespaceNode's namespace-or-typename name.
    /// </summary>
    /// <param name="node">A UsingNamespaceNode node</param>
    /// <param name="names">Any number of name parts.</param>
    /// <returns>The UsingNamespaceNode is returned to enable method chaining.</returns>
    // ----------------------------------------------------------------------------------------------
    public static UsingNamespaceNode TypeTag(this UsingNamespaceNode node, params string[] names)
    {
      node.TypeName.TypeTag(names);
      return node;
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Sets the qualifier tag of the UsingNamespaceNode.
    /// </summary>
    /// <param name="node">A UsingNamespaceNode node</param>
    /// <param name="qualifier">The name of the qualifier</param>
    /// <returns>The UsingNamespaceNode parameter is returned to enable method chaining.</returns>
    // ----------------------------------------------------------------------------------------------
    public static UsingNamespaceNode Qualifier(this UsingNamespaceNode node, string qualifier)
    {
      node.TypeName.Qualifier(qualifier);
      return node;
    }
  }
}
