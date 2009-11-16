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
    public ExpressionResult ExpressionResult { get; protected set; }
    
    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Evaluates this expression.
    /// </summary>
    /// <param name="errorHandler">An error handler object.</param>
    // ----------------------------------------------------------------------------------------------
    public abstract void Evaluate(ICompilationErrorHandler errorHandler);

    #region Visitor methods

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Accepts a visitor object, according to the Visitor pattern.
    /// </summary>
    /// <param name="visitor">A visitor object</param>
    // ----------------------------------------------------------------------------------------------
    public override void AcceptVisitor(SemanticGraphVisitor visitor)
    {
      visitor.Visit(this);
      base.AcceptVisitor(visitor);
    }

    #endregion
  }
}
