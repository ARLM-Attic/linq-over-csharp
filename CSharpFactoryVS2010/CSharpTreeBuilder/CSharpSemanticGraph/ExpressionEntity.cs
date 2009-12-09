using CSharpTreeBuilder.CSharpSemanticGraphBuilder;
using CSharpTreeBuilder.ProjectContent;

namespace CSharpTreeBuilder.CSharpSemanticGraph
{
  // ================================================================================================
  /// <summary>
  /// This abstract class represents an expression entity.
  /// </summary>
  // ================================================================================================
  public abstract class ExpressionEntity : SemanticEntity
  {
    #region State

    /// <summary>Backing field for ExpressionResult property.</summary>
    private ExpressionResult _ExpressionResult;

    #endregion

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Initializes a new instance of the <see cref="ExpressionEntity"/> class.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    protected ExpressionEntity()
    {
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Initializes a new instance of the <see cref="ExpressionEntity"/> class 
    /// by constructing it from a template instance.
    /// </summary>
    /// <param name="template">The template for the new instance.</param>
    /// <param name="typeParameterMap">The type parameter map of the new instance.</param>
    // ----------------------------------------------------------------------------------------------
    protected ExpressionEntity(ExpressionEntity template, TypeParameterMap typeParameterMap)
      : base(template, typeParameterMap)
    {
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets or sets the result of evaluating the expression. Null if not yet evaluated.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public ExpressionResult ExpressionResult
    {
      get
      {
        if (_ExpressionResult == null)
        {
          // TODO: errorHandler object is needed here! (instead of null)
          Evaluate(null);
        }

        return _ExpressionResult;
      }

      protected set
      {
        _ExpressionResult = value;
      }
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Evaluates this expression.
    /// </summary>
    /// <param name="errorHandler">An error handler object.</param>
    // ----------------------------------------------------------------------------------------------
    public abstract void Evaluate(ICompilationErrorHandler errorHandler);

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
