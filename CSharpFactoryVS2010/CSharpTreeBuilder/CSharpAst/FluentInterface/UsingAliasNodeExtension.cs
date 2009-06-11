using CSharpTreeBuilder.CSharpAstBuilder;

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
    /// Sets the alias of a UsingAliasNode.
    /// </summary>
    /// <param name="node">A UsingAliasNode node.</param>
    /// <param name="alias">Alias name.</param>
    /// <returns>The UsingAliasNode is returned as a UsingNamespaceNode to enable method chaining.</returns>
    // ----------------------------------------------------------------------------------------------
    public static UsingNamespaceNode Alias(this UsingAliasNode node, string alias)
    {
      node.AliasToken = new Token(alias);
      return node;
    }
  }
}
