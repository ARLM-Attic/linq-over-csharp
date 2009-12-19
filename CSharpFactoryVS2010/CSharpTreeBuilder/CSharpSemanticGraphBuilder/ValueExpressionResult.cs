using CSharpTreeBuilder.CSharpSemanticGraph;

namespace CSharpTreeBuilder.CSharpSemanticGraphBuilder
{
  // ================================================================================================
  /// <summary>
  /// This class represents an expression result classified as a value.
  /// </summary>
  // ================================================================================================
  public sealed class ValueExpressionResult : TypedExpressionResult
  {
    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Initializes a new instance of the <see cref="ValueExpressionResult"/> class.
    /// </summary>
    /// <param name="typeEntity">The type associated with the expression.</param>
    // ----------------------------------------------------------------------------------------------
    public ValueExpressionResult(TypeEntity typeEntity)
      : base(typeEntity)
    {
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Initializes a new instance of the <see cref="ValueExpressionResult"/> class.
    /// </summary>
    /// <param name="typeEntity">The type associated with the expression.</param>
    /// <param name="entity">The entity associated with this expression.</param>
    // ----------------------------------------------------------------------------------------------
    public ValueExpressionResult(TypeEntity typeEntity, ISemanticEntity entity)
      : base(typeEntity)
    {
      Entity = entity;
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets the entity associated with this expression result. Can be null (eg. for literals).
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public ISemanticEntity Entity { get; private set; }
  }
}
