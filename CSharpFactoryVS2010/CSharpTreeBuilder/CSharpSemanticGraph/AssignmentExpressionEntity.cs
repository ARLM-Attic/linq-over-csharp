using System.Collections.Generic;
using CSharpTreeBuilder.ProjectContent;

namespace CSharpTreeBuilder.CSharpSemanticGraph
{
  // ================================================================================================
  /// <summary>
  /// This class represents an assignment expression entity.
  /// </summary>
  // ================================================================================================
  public sealed class AssignmentExpressionEntity : ExpressionEntity, IHasExpressions
  {
    #region State

    /// <summary>Gets or sets the left-hand expression object.</summary>
    public ExpressionEntity LeftExpression { get; private set; }

    /// <summary>Gets or sets the right-hand expression object.</summary>
    public ExpressionEntity RightExpression { get; private set; }

    #endregion

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Initializes a new instance of the <see cref="AssignmentExpressionEntity"/> class.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public AssignmentExpressionEntity()
    {
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Initializes a new instance of the <see cref="AssignmentExpressionEntity"/> class 
    /// by constructing it from a template instance.
    /// </summary>
    /// <param name="template">The template for the new instance.</param>
    /// <param name="typeParameterMap">The type parameter map of the new instance.</param>
    // ----------------------------------------------------------------------------------------------
    private AssignmentExpressionEntity(AssignmentExpressionEntity template, TypeParameterMap typeParameterMap)
      : base(template, typeParameterMap)
    {
      if (template.LeftExpression != null)
      {
        LeftExpression = (ExpressionEntity) template.LeftExpression.GetGenericClone(typeParameterMap);
      }

      if (template.RightExpression != null)
      {
        RightExpression = (ExpressionEntity) template.RightExpression.GetGenericClone(typeParameterMap);
      }
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Creates a new constructed entity.
    /// </summary>
    /// <param name="typeParameterMap">A collection of type parameters and associated type arguments.</param>
    /// <returns>
    /// A new semantic entity constructed from this entity using the specified type parameter map.
    /// </returns>
    // ----------------------------------------------------------------------------------------------
    protected override SemanticEntity ConstructNew(TypeParameterMap typeParameterMap)
    {
      return new AssignmentExpressionEntity(this, typeParameterMap);
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Adds a child expression. 
    /// </summary>
    /// <param name="expressionEntity">An expression entity.</param>
    // ----------------------------------------------------------------------------------------------
    public void AddExpression(ExpressionEntity expressionEntity)
    {
      if (LeftExpression == null)
      {
        LeftExpression = expressionEntity;
      }
      else
      {
        RightExpression = expressionEntity;
      }

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
        var result = new List<ExpressionEntity>();
        
        if (LeftExpression != null )
        {
          result.Add(LeftExpression);
        }
        if (RightExpression != null)
        {
          result.Add(RightExpression);
        }
        
        return result;
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
      // First evaluate the child expressions.
      if (LeftExpression.ExpressionResult == null)
      {
        LeftExpression.Evaluate(errorHandler);
      }

      if (RightExpression.ExpressionResult == null)
      {
        RightExpression.Evaluate(errorHandler);
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

      if (LeftExpression != null)
      {
        LeftExpression.AcceptVisitor(visitor);
      }

      if (RightExpression != null)
      {
        RightExpression.AcceptVisitor(visitor);
      }
    }

    #endregion
  }
}
