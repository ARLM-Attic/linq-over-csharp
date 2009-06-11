namespace CSharpTreeBuilder.Ast
{
  // ================================================================================================
  /// <summary>
  /// A fluent interface for creating UsingNamespaceNode objects, implemented with extension methods.
  /// </summary>
  // ================================================================================================
  public static class UsingNamespaceNodeExtension
  {
    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Adds a TypeTag to a UsingNamespaceNode's namespace-or-typename name.
    /// </summary>
    /// <param name="node">A UsingNamespaceNode node</param>
    /// <returns>The UsingNamespaceNode is returned to enable method chaining.</returns>
    // ----------------------------------------------------------------------------------------------
    public static UsingNamespaceNode TypeTag(this UsingNamespaceNode node, string name)
    {
      node.TypeName.TypeTag(name);
      return node;
    }

    public static UsingNamespaceNode Qualifier(this UsingNamespaceNode node, string qualifier)
    {
      node.TypeName.Qualifier(qualifier);
      return node;
    }
  }
}
