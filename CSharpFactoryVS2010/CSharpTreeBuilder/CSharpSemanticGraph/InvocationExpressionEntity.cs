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
    /// Initializes a new instance of the <see cref="InvocationExpressionEntity"/> class.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public InvocationExpressionEntity()
    {
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Initializes a new instance of the <see cref="InvocationExpressionEntity"/> class 
    /// by constructing it from a template instance.
    /// </summary>
    /// <param name="template">The template for the new instance.</param>
    /// <param name="typeParameterMap">The type parameter map of the new instance.</param>
    // ----------------------------------------------------------------------------------------------
    private InvocationExpressionEntity(InvocationExpressionEntity template, TypeParameterMap typeParameterMap)
      : base(template, typeParameterMap)
    {
      foreach(var argument in template.Arguments)
      {
        _Arguments.Add((ArgumentEntity)argument.GetGenericClone(typeParameterMap));
      }

      if (template.ChildExpression != null)
      {
        ChildExpression = (ExpressionEntity)template.ChildExpression.GetGenericClone(typeParameterMap);
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
      return new InvocationExpressionEntity(this, typeParameterMap);
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
