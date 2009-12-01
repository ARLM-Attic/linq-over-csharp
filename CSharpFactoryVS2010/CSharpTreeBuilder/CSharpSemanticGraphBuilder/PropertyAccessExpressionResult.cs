using CSharpTreeBuilder.CSharpSemanticGraph;

namespace CSharpTreeBuilder.CSharpSemanticGraphBuilder
{
  // ================================================================================================
  /// <summary>
  /// This class represents an expression result classified as a property access.
  /// </summary>
  // ================================================================================================
  public sealed class PropertyAccessExpressionResult : ExpressionResult, IHasInstanceExpression
  {
    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Initializes a new instance of the <see cref="PropertyAccessExpressionResult"/> class.
    /// </summary>
    /// <param name="propertyEntity">The property entity associated with this expression.</param>
    /// <param name="instanceExpression">The instance expression associated with this entity.</param>
    // ----------------------------------------------------------------------------------------------
    public PropertyAccessExpressionResult(PropertyEntity propertyEntity, ExpressionResult instanceExpression)
    {
      PropertyEntity = propertyEntity;
      InstanceExpression = instanceExpression;
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets the property entity associated with the expression.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public PropertyEntity PropertyEntity { get; private set; }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets the instance expression associated with this expression result.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public ExpressionResult InstanceExpression { get; private set; }

  }
}
