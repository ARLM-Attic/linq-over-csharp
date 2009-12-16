using System.Linq;
using System.Collections.Generic;

namespace CSharpTreeBuilder.CSharpSemanticGraph
{
  // ================================================================================================
  /// <summary>
  /// This class represents an expression statement entity.
  /// </summary>
  // ================================================================================================
  public sealed class ExpressionStatementEntity : StatementEntity, IHasExpressions
  {
    #region State

    /// <summary>Gets the expression of the statement.</summary>
    public ExpressionEntity Expression { get; private set; }

    #endregion

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Initializes a new instance of the <see cref="ExpressionStatementEntity"/> class.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public ExpressionStatementEntity()
    {
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Initializes a new instance of the <see cref="ExpressionStatementEntity"/> class 
    /// by constructing it from a template instance.
    /// </summary>
    /// <param name="template">The template for the new instance.</param>
    /// <param name="typeParameterMap">The type parameter map of the new instance.</param>
    // ----------------------------------------------------------------------------------------------
    private ExpressionStatementEntity(ExpressionStatementEntity template, TypeParameterMap typeParameterMap)
      : base(template, typeParameterMap)
    {
      if (template.Expression != null)
      {
        Expression = (ExpressionEntity) template.Expression.GetGenericClone(typeParameterMap);
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
      return new ExpressionStatementEntity(this, typeParameterMap);
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Adds a child entity.
    /// </summary>
    /// <param name="entity">A child entity.</param>
    // ----------------------------------------------------------------------------------------------
    public override void AddChild(ISemanticEntity entity)
    {
      if (entity is ExpressionEntity)
      {
        AddExpression(entity as ExpressionEntity);
      }
      else
      {
        base.AddChild(entity);
      }
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
        return Expression == null 
          ? new List<ExpressionEntity>()
          : new List<ExpressionEntity> { Expression };
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
      visitor.Visit(this);
      base.AcceptVisitor(visitor);

      if (Expression != null)
      {
        Expression.AcceptVisitor(visitor);
      }
    }

    #endregion
  }
}
