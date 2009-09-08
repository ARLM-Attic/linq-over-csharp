using System.Collections.Generic;

namespace CSharpTreeBuilder.CSharpSemanticGraph
{
  // ================================================================================================
  /// <summary>
  /// This interface is implemented by semantic entities that can have expressions as child entities.
  /// </summary>
  // ================================================================================================
  public interface IHasExpressions
  {
    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Adds a child expression. 
    /// </summary>
    /// <param name="expressionEntity">An expression entity.</param>
    // ----------------------------------------------------------------------------------------------
    void AddExpression(ExpressionEntity expressionEntity);

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets an iterate-only collection of child expressions.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    IEnumerable<ExpressionEntity> Expressions { get; }
  }
}
