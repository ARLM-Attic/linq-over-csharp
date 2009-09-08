using System;
using System.Collections.Generic;

namespace CSharpTreeBuilder.CSharpSemanticGraph
{
  // ================================================================================================
  /// <summary>
  /// This class represents an initialier that is an expression.
  /// </summary>
  // ================================================================================================
  public sealed class ScalarInitializerEntity : SemanticEntity, IVariableInitializer, IHasExpressions
  {
    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets a value indicating whether this initializer is an expression.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public bool IsExpression
    {
      get { return true; }
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets a value indicating whether this is an array initializer.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public bool IsArrayInitializer
    {
      get { return false; }
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets the expression, if this initializer is an expression. Null if it's not an expression.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public ExpressionEntity Expression { get; private set; }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets the collection of variable initializers. Null if it's not an array initializer.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public IEnumerable<IVariableInitializer> VariableInitializers
    {
      get { return null; }
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Adds an expression to this entity.
    /// </summary>
    /// <param name="expressionEntity">An expression entity.</param>
    // ----------------------------------------------------------------------------------------------
    public void AddExpression(ExpressionEntity expressionEntity)
    {
      Expression = expressionEntity;
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
    }

    #endregion
  }
}
