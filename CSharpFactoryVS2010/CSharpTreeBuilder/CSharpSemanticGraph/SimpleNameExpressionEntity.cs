using System;
using CSharpTreeBuilder.CSharpSemanticGraphBuilder;
using CSharpTreeBuilder.ProjectContent;

namespace CSharpTreeBuilder.CSharpSemanticGraph
{
  // ================================================================================================
  /// <summary>
  /// This class represents a simple name expression entity.
  /// </summary>
  // ================================================================================================
  public sealed class SimpleNameExpressionEntity : ExpressionEntity
  {
    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Initializes a new instance of the <see cref="SimpleNameExpressionEntity"/> class.
    /// </summary>
    /// <param name="simpleNameResolver">A simple name resolver object.</param>
    // ----------------------------------------------------------------------------------------------
    public SimpleNameExpressionEntity(SimpleNameNodeResolver simpleNameResolver)
    {
      if (simpleNameResolver == null)
      {
        throw new ArgumentNullException("simpleNameResolver");
      }

      SimpleNameResolver = simpleNameResolver;
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets or sets the simple name resolver object.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public SimpleNameNodeResolver SimpleNameResolver { get; private set; }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Evaluates this expression.
    /// </summary>
    /// <param name="errorHandler">An error handler object.</param>
    // ----------------------------------------------------------------------------------------------
    public override void Evaluate(ICompilationErrorHandler errorHandler)
    {
      if (SimpleNameResolver != null)
      {
        ExpressionResult = SimpleNameResolver.Resolve(this, errorHandler);
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
    }

    #endregion
  }
}
