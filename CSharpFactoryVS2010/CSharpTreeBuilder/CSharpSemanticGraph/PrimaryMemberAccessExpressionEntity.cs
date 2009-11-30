using System.Collections.Generic;
using CSharpTreeBuilder.CSharpSemanticGraphBuilder;
using CSharpTreeBuilder.ProjectContent;

namespace CSharpTreeBuilder.CSharpSemanticGraph
{
  // ================================================================================================
  /// <summary>
  /// This class represents a member access expression entity.
  /// </summary>
  // ================================================================================================
  public sealed class PrimaryMemberAccessExpressionEntity : MemberAccessExpressionEntity, IHasExpressions
  {
    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Initializes a new instance of the <see cref="PrimaryMemberAccessExpressionEntity"/> class.
    /// </summary>
    /// <param name="resolver">A member access node resolver object.</param>
    // ----------------------------------------------------------------------------------------------
    public PrimaryMemberAccessExpressionEntity(MemberAccessNodeResolver resolver)
      : base (resolver)
    {
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Adds a child expression. 
    /// </summary>
    /// <param name="expressionEntity">An expression entity.</param>
    // ----------------------------------------------------------------------------------------------
    public void AddExpression(ExpressionEntity expressionEntity)
    {
      ChildExpression = expressionEntity;

      if (expressionEntity != null)
      {
        expressionEntity.Parent = this;
      }
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets an iterate-only collection of child expressions.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public IEnumerable<ExpressionEntity> Expressions 
    { 
      get
      {
        return ChildExpression == null 
          ? new ExpressionEntity[] {}
          : new[] {ChildExpression};
      }
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets or sets the child expression object.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public ExpressionEntity ChildExpression { get; private set; }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Evaluates this expression.
    /// </summary>
    /// <param name="errorHandler">An error handler object.</param>
    // ----------------------------------------------------------------------------------------------
    public override void Evaluate(ICompilationErrorHandler errorHandler)
    {
      // First evaluate the child expression.
      if (ChildExpression.ExpressionResult == null)
      {
        ChildExpression.Evaluate(errorHandler);
      }

      // Then evaluate this expression.
      base.Evaluate(errorHandler);
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
      visitor.Visit(this);
      base.AcceptVisitor(visitor);
    }

    #endregion
  }
}
