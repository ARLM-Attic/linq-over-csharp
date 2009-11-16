using CSharpTreeBuilder.CSharpSemanticGraph;

namespace CSharpTreeBuilder.CSharpSemanticGraphBuilder
{
  // ================================================================================================
  /// <summary>
  /// This class represents an expression result classified as a type.
  /// </summary>
  // ================================================================================================
  public sealed class TypeExpressionResult : TypedExpressionResult
  {
    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Initializes a new instance of the <see cref="TypeExpressionResult"/> class.
    /// </summary>
    /// <param name="typeEntity">The type associated with this expression.</param>
    // ----------------------------------------------------------------------------------------------
    public TypeExpressionResult(TypeEntity typeEntity)
      : base(typeEntity)
    {
    }
  }
}
