namespace CSharpTreeBuilder.Ast
{
  // ================================================================================================
  /// <summary>
  /// A fluent interface for creating CompilationUnitNode objects, implemented with extension methods.
  /// </summary>
  // ================================================================================================
  public static class CompilationUnitNodeExtension
  {
    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Creates a new UsingNamespaceNode and adds it to a CompilationUnitNode.
    /// </summary>
    /// <param name="node">A CompilationUnitNode node</param>
    /// <returns>The created UsingNamespaceNode is returned to enable method chaining.</returns>
    // ----------------------------------------------------------------------------------------------
    public static UsingNamespaceNode Using(this SourceFileNode node)
    {
      var usingNamespaceNode = new UsingNamespaceNode();
      node.AddChild(usingNamespaceNode);
      return usingNamespaceNode;
    }
  }
}
