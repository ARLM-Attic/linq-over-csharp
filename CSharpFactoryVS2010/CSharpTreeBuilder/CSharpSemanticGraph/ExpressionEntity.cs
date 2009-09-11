using CSharpTreeBuilder.CSharpSemanticGraphBuilder;
using CSharpTreeBuilder.ProjectContent;

namespace CSharpTreeBuilder.CSharpSemanticGraph
{
  // ================================================================================================
  /// <summary>
  /// This abstract class represents an expression entity.
  /// </summary>
  // ================================================================================================
  public abstract class ExpressionEntity : SemanticEntity
  {
    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets or sets the result obtained by evaluating this expression. Null if not yet evaluated.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public ExpressionResult Result { get; protected set; }
    
    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Evaluates this expression.
    /// </summary>
    /// <param name="semanticGraph">The semantic graph.</param>
    /// <param name="errorHandler">An error handler object.</param>
    // ----------------------------------------------------------------------------------------------
    public abstract void Evaluate(SemanticGraph semanticGraph, ICompilationErrorHandler errorHandler);
  }
}
