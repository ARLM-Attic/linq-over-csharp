using CSharpTreeBuilder.CSharpSemanticGraphBuilder;

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
    /// Evaluates this expressions.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public abstract void Evaluate();
  }
}
