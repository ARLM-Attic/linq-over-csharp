using CSharpTreeBuilder.CSharpSemanticGraph;

namespace CSharpTreeBuilder.CSharpSemanticGraphBuilder
{
  // ================================================================================================
  /// <summary>
  /// This is the abstract base class of expression results that has an associated type.
  /// </summary>
  // ================================================================================================
  public abstract class TypedExpressionResult : ExpressionResult
  {
    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Initializes a new instance of the <see cref="TypedExpressionResult"/> class.
    /// </summary>
    /// <param name="typeEntity">The type associated with the expression.</param>
    // ----------------------------------------------------------------------------------------------
    protected TypedExpressionResult(TypeEntity typeEntity)
    {
      Type = typeEntity;
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets the type associated with the expression.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public TypeEntity Type { get; private set; }
  }
}
