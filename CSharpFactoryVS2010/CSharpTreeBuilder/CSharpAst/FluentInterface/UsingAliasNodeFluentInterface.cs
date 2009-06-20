namespace CSharpTreeBuilder.Ast
{
  // ================================================================================================
  /// <summary>
  /// A fluent interface for creating UsingAliasNode objects, implemented with extension methods.
  /// </summary>
  // ================================================================================================
  public static class UsingAliasNodeFluentInterface
  {
    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Adds a TypeTag to a UsingAliasNode's namespace-or-typename name.
    /// </summary>
    /// <param name="node">A UsingAliasNode node</param>
    /// <param name="names">Any number of name parts.</param>
    /// <returns>The UsingAliasNode is returned to enable method chaining.</returns>
    // ----------------------------------------------------------------------------------------------
    public static UsingAliasNode TypeTag(this UsingAliasNode node, params string[] names)
    {
      node.TypeName.TypeTag(names);
      return node;
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Adds a TypeTag to a UsingAliasNode's namespace-or-typename name.
    /// </summary>
    /// <param name="node">A UsingAliasNode node</param>
    /// <param name="typeTagNodes">Any number of TypeTageNode objects.</param>
    /// <returns>The UsingAliasNode is returned to enable method chaining.</returns>
    // ----------------------------------------------------------------------------------------------
    public static UsingAliasNode TypeTag(this UsingAliasNode node, params TypeTagNode[] typeTagNodes)
    {
      node.TypeName.TypeTag(typeTagNodes);
      return node;
    }
    
    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Sets the qualifier tag of the UsingAliasNode.
    /// </summary>
    /// <param name="node">A UsingAliasNode node</param>
    /// <param name="qualifier">The name of the qualifier</param>
    /// <returns>The UsingAliasNode parameter is returned to enable method chaining.</returns>
    // ----------------------------------------------------------------------------------------------
    public static UsingAliasNode Qualifier(this UsingAliasNode node, string qualifier)
    {
      node.TypeName.Qualifier(qualifier);
      return node;
    }
  }
}
