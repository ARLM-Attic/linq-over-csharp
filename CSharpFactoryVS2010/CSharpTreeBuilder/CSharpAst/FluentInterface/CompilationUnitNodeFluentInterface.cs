using CSharpTreeBuilder.CSharpAstBuilder;

namespace CSharpTreeBuilder.Ast
{
  // ================================================================================================
  /// <summary>
  /// A fluent interface for creating CompilationUnitNode objects, implemented with extension methods.
  /// </summary>
  // ================================================================================================
  public static class CompilationUnitNodeFluentInterface
  {
    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Creates a new UsingNamespaceNode and adds it to a CompilationUnitNode.
    /// </summary>
    /// <param name="node">A CompilationUnitNode node</param>
    /// <returns>The created UsingNamespaceNode is returned to enable method chaining.</returns>
    // ----------------------------------------------------------------------------------------------
    public static UsingNamespaceNode UsingNamespace(this CompilationUnitNode node)
    {
      var usingNamespaceNode = new UsingNamespaceNode();
      node.AddChild(usingNamespaceNode);
      return usingNamespaceNode;
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Creates a new UsingAliasNode and adds it to a CompilationUnitNode.
    /// </summary>
    /// <param name="node">A CompilationUnitNode node</param>
    /// <param name="alias">Alias name.</param>
    /// <returns>The created UsingAliasNode is returned to enable method chaining.</returns>
    // ----------------------------------------------------------------------------------------------
    public static UsingNamespaceNode UsingAlias(this CompilationUnitNode node, string alias)
    {
      var usingAliasNode = new UsingAliasNode(new Token(alias));
      node.AddChild(usingAliasNode);
      return usingAliasNode;
    }
  }
}
