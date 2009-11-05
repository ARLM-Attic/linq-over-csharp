using CSharpTreeBuilder.Ast;
using CSharpTreeBuilder.CSharpSemanticGraph;
using CSharpTreeBuilder.ProjectContent;

namespace CSharpTreeBuilder.CSharpSemanticGraphBuilder
{
  // ================================================================================================
  /// <summary>
  /// This class represents a reference to a namespace entity, based on an extern alias.
  /// </summary>
  // ================================================================================================
  public sealed class ExternAliasNodeBasedRootNamespaceEntityReference 
    : SyntaxNodeBasedSemanticEntityReference<RootNamespaceEntity, ExternAliasNode>
  {
    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Initializes a new instance of the <see cref="ExternAliasNodeBasedRootNamespaceEntityReference"/> class.
    /// </summary>
    /// <param name="externAliasNode">An extern alias AST node.</param>
    // ----------------------------------------------------------------------------------------------
    public ExternAliasNodeBasedRootNamespaceEntityReference(ExternAliasNode externAliasNode)
      : base(externAliasNode)
    {
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Implements the resolution logic.
    /// </summary>
    /// <param name="context">A semantic entity that is the context of the resolution.</param>
    /// <param name="semanticGraph">The semantic graph.</param>
    /// <param name="errorHandler">An object for error and warning reporting.</param>
    /// <returns>The resolved entity, or null if could not resolve.</returns>
    // ----------------------------------------------------------------------------------------------
    protected override RootNamespaceEntity GetResolvedEntity(
      ISemanticEntity context, SemanticGraph semanticGraph, ICompilationErrorHandler errorHandler)
    {
      if (SyntaxNode.Identifier == semanticGraph.GlobalNamespace.Name)
      {
        errorHandler.Error("CS1681", SyntaxNode.IdentifierToken, "You cannot redefine the global extern alias");
        return null;
      }

      var rootNamespace = semanticGraph.GetRootNamespaceByName(SyntaxNode.Identifier);
      if (rootNamespace != null)
      {
        return rootNamespace;
      }

      errorHandler.Error("CS0430", SyntaxNode.IdentifierToken,
                         "The extern alias '{0}' was not specified in a /reference option", SyntaxNode.Identifier);
      return null;
    }

    #region Private methods

    #endregion
  }
}
