namespace CSharpTreeBuilder.Ast
{
  // ================================================================================================
  /// <summary>
  /// A fluent interface for creating UsingAliasNode objects, implemented with extension methods.
  /// </summary>
  // ================================================================================================
  public static class UsingAliasNodeExtension
  {
    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Adds a TypeTag to a UsingAliasNode's namespace-or-typename name.
    /// </summary>
    /// <param name="node">A UsingAliasNode node</param>
    /// <returns>The UsingAliasNode is returned to enable method chaining.</returns>
    // ----------------------------------------------------------------------------------------------
    public static UsingAliasNode TypeTag(this UsingAliasNode node, string name)
    {
      node.TypeName.TypeTag(name);
      return node;
    }

    public static UsingAliasNode Qualifier(this UsingAliasNode node, string qualifier)
    {
      node.TypeName.Qualifier(qualifier);
      return node;
    }
  }
}
