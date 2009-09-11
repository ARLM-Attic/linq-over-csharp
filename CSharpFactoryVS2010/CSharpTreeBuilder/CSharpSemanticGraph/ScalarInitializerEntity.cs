using System;
using System.Collections.Generic;

namespace CSharpTreeBuilder.CSharpSemanticGraph
{
  // ================================================================================================
  /// <summary>
  /// This class represents an initialier that is an expression.
  /// </summary>
  // ================================================================================================
  public sealed class ScalarInitializerEntity : VariableInitializer, IHasExpressions
  {
    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets the expression of the initializer.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public ExpressionEntity Expression { get; set; }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Adds an expression to this entity.
    /// </summary>
    /// <param name="expressionEntity">An expression entity.</param>
    // ----------------------------------------------------------------------------------------------
    public void AddExpression(ExpressionEntity expressionEntity)
    {
      Expression = expressionEntity;

      if (expressionEntity != null)
      {
        expressionEntity.Parent = this;
      }
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets a collection of expression of this entity.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public IEnumerable<ExpressionEntity> Expressions
    {
      get
      {
        return new List<ExpressionEntity> { Expression };
      }
    }

    #region Visitor methods

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Accepts a visitor object, according to the Visitor pattern.
    /// </summary>
    /// <param name="visitor">A visitor object</param>
    // ----------------------------------------------------------------------------------------------
    public override void AcceptVisitor(SemanticGraphVisitor visitor)
    {
      if (!visitor.Visit(this)) { return; }

      if (Expression != null)
      {
        Expression.AcceptVisitor(visitor);
      }
    }

    #endregion
  }
}
