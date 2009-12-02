using System.Collections.Generic;
using CSharpTreeBuilder.ProjectContent;

namespace CSharpTreeBuilder.CSharpSemanticGraph
{
  // ================================================================================================
  /// <summary>
  /// This class represents an invocation expression entity.
  /// </summary>
  // ================================================================================================
  public sealed class InvocationExpressionEntity : ExpressionEntity, IHasExpressions, IHasArguments
  {
    #region State

    /// <summary>The list of arguments.</summary>
    private readonly List<ArgumentEntity> _Arguments = new List<ArgumentEntity>();

    /// <summary>Gets or sets the child expression object.</summary>
    public ExpressionEntity ChildExpression { get; private set; }

    #endregion

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
          ? new ExpressionEntity[] { }
          : new[] { ChildExpression };
      }
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Adds an argument. 
    /// </summary>
    /// <param name="argumentEntity">An argument entity.</param>
    // ----------------------------------------------------------------------------------------------
    public void AddArgument(ArgumentEntity argumentEntity)
    {
      if (argumentEntity != null)
      {
        _Arguments.Add(argumentEntity);
        argumentEntity.Parent = this;
      }
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets an iterate-only collection of arguments.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public IEnumerable<ArgumentEntity> Arguments
    {
      get
      {
        return _Arguments;
      }
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Evaluates this expression.
    /// </summary>
    /// <param name="errorHandler">An error handler object.</param>
    // ----------------------------------------------------------------------------------------------
    public override void Evaluate(ICompilationErrorHandler errorHandler)
    {
      // TODO: evaluate arguments

      // Evaluate the child expression.
      if (ChildExpression.ExpressionResult == null)
      {
        ChildExpression.Evaluate(errorHandler);
      }
      
      // TODO
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

      if (ChildExpression != null)
      {
        ChildExpression.AcceptVisitor(visitor);
      }

      foreach(var argument in _Arguments)
      {
        argument.AcceptVisitor(visitor);
      }
    }

    #endregion
  }
}
