using CSharpTreeBuilder.Ast;
using CSharpTreeBuilder.CSharpSemanticGraph;
using CSharpTreeBuilder.ProjectContent;

namespace CSharpTreeBuilder.CSharpSemanticGraphBuilder
{
  // ================================================================================================
  /// <summary>
  /// This class represents a reference to a semantic entity, based on a simple-name AST node.
  /// </summary>
  // ================================================================================================
  public sealed class SimpleNameNodeBasedSemanticEntityReference
    : SyntaxNodeBasedSemanticEntityReference<ISemanticEntity, SimpleNameNode>
  {
    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Initializes a new instance of the <see cref="SimpleNameNodeBasedSemanticEntityReference"/> class.
    /// </summary>
    /// <param name="simpleNameNode">A syntax node that will be resolved to a semantic entity.</param>
    // ----------------------------------------------------------------------------------------------
    public SimpleNameNodeBasedSemanticEntityReference(SimpleNameNode simpleNameNode)
      : base(simpleNameNode)
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
    protected override ISemanticEntity GetResolvedEntity(
      SemanticEntity context, SemanticGraph semanticGraph, ICompilationErrorHandler errorHandler)
    {
      var simpleNameResolver = new SimpleNameResolver(errorHandler, semanticGraph);
      return simpleNameResolver.Resolve(SyntaxNode, context);
    }
  }
}
