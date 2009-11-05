using System;
using CSharpTreeBuilder.Ast;
using CSharpTreeBuilder.CSharpSemanticGraph;
using CSharpTreeBuilder.ProjectContent;

namespace CSharpTreeBuilder.CSharpSemanticGraphBuilder
{
  // ================================================================================================
  /// <summary>
  /// This class represents a reference to a interface entity, 
  /// based on a namespace-or-type-name AST node.
  /// </summary>
  // ================================================================================================
  public sealed class NamespaceOrTypeNameNodeBasedTypeEntityReference 
    : SyntaxNodeBasedSemanticEntityReference<TypeEntity, NamespaceOrTypeNameNode>
  {
    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Initializes a new instance of the <see cref="NamespaceOrTypeNameNodeBasedTypeEntityReference"/> class.
    /// </summary>
    /// <param name="syntaxNode">A syntax node that will be resolved to a semantic entity.</param>
    // ----------------------------------------------------------------------------------------------
    public NamespaceOrTypeNameNodeBasedTypeEntityReference(NamespaceOrTypeNameNode syntaxNode)
      : base(syntaxNode)
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
    protected override TypeEntity GetResolvedEntity(
      ISemanticEntity context, SemanticGraph semanticGraph, ICompilationErrorHandler errorHandler)
    {
      var namespaceOrTypeNameResolver = new NamespaceOrTypeNameResolver(errorHandler, semanticGraph);
      return namespaceOrTypeNameResolver.ResolveToTypeEntity(SyntaxNode, context);
    }
  }
}
