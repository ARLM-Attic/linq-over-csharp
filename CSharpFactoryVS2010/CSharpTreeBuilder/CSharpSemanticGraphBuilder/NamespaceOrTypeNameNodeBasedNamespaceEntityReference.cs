using System;
using CSharpTreeBuilder.Ast;
using CSharpTreeBuilder.CSharpSemanticGraph;
using CSharpTreeBuilder.ProjectContent;

namespace CSharpTreeBuilder.CSharpSemanticGraphBuilder
{
  // ================================================================================================
  /// <summary>
  /// This class represents a reference to a namespace or type entity, based on a type-or-namespace AST node.
  /// </summary>
  // ================================================================================================
  public sealed class NamespaceOrTypeNameNodeBasedNamespaceEntityReference 
    : SyntaxNodeBasedSemanticEntityReference<NamespaceEntity, NamespaceOrTypeNameNode>
  {
    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Initializes a new instance of the <see cref="NamespaceOrTypeNameNodeBasedNamespaceEntityReference"/> class.
    /// </summary>
    /// <param name="syntaxNode">A syntax node that will be resolved to a semantic entity.</param>
    // ----------------------------------------------------------------------------------------------
    public NamespaceOrTypeNameNodeBasedNamespaceEntityReference(NamespaceOrTypeNameNode syntaxNode)
      : base(syntaxNode)
    {
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Implements the resolution logic.
    /// </summary>
    /// <param name="context">A semantic entity that is the context of the resolution.</param>
    /// <param name="errorHandler">An object for error and warning reporting.</param>
    /// <returns>The resolved entity, or null if could not resolve.</returns>
    // ----------------------------------------------------------------------------------------------
    protected override NamespaceEntity GetResolvedEntity(SemanticEntity context, ICompilationErrorHandler errorHandler)
    {
      var namespaceOrTypeNameResolver = new NamespaceOrTypeNameResolver(errorHandler, context.SemanticGraph);
      return namespaceOrTypeNameResolver.ResolveToNamespaceEntity(SyntaxNode, context);
    }
  }
}
