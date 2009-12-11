using CSharpTreeBuilder.CSharpSemanticGraph;
using CSharpTreeBuilder.ProjectContent;

namespace CSharpTreeBuilder.CSharpSemanticGraphBuilder
{
  // ================================================================================================
  /// <summary>
  /// This class implements a semantic graph visitor that resolves expressions.
  /// </summary>
  // ================================================================================================
  public sealed class ExpressionEvaluatorSemanticGraphVisitor : SemanticGraphVisitor
  {
    /// <summary>Error handler object for error and warning reporting.</summary>
    private readonly ICompilationErrorHandler _ErrorHandler;

    /// <summary>The semantic graph that this visitor is working on.</summary>
    private readonly SemanticGraph _SemanticGraph;

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Initializes a new instance of the <see cref="ExpressionEvaluatorSemanticGraphVisitor"/> class.
    /// </summary>
    /// <param name="errorHandler">Error handler object for error and warning reporting.</param>
    /// <param name="semanticGraph">The semantic graph that this visitor is working on.</param>
    // ----------------------------------------------------------------------------------------------
    public ExpressionEvaluatorSemanticGraphVisitor(ICompilationErrorHandler errorHandler, SemanticGraph semanticGraph)
    {
      _ErrorHandler = errorHandler;
      _SemanticGraph = semanticGraph;
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Resolves expressions in a semantic entity.
    /// </summary>
    /// <param name="entity">A semantic entity.</param>
    // ----------------------------------------------------------------------------------------------
    public override void Visit(ExpressionEntity entity)
    {
      entity.Evaluate(_ErrorHandler);

      foreach(var clone in entity.GenericClones)
      {
        (clone as ExpressionEntity).Evaluate(_ErrorHandler);
      }
    }
  }
}
