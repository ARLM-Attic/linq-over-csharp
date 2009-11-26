using System;
using CSharpTreeBuilder.Ast;
using CSharpTreeBuilder.CSharpSemanticGraphBuilder;
using CSharpTreeBuilder.ProjectContent;

namespace CSharpTreeBuilder.CSharpSemanticGraph
{
  // ================================================================================================
  /// <summary>
  /// This class represents a simple name expression entity.
  /// </summary>
  // ================================================================================================
  public class SimpleNameExpressionEntity : ExpressionEntity
  {
    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Initializes a new instance of the <see cref="SimpleNameExpressionEntity"/> class.
    /// </summary>
    /// <param name="simpleNameResolver">A simple name resolver object.</param>
    // ----------------------------------------------------------------------------------------------
    public SimpleNameExpressionEntity(SimpleNameNodeToExpressionResultResolver simpleNameResolver)
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
    public SimpleNameNodeToExpressionResultResolver SimpleNameResolver { get; private set; }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Evaluates this expression.
    /// </summary>
    /// <param name="errorHandler">An error handler object.</param>
    // ----------------------------------------------------------------------------------------------
    public override void Evaluate(ICompilationErrorHandler errorHandler)
    {
      // First resolve the simple name

      if (SimpleNameResolver != null)
      {
        ExpressionResult = SimpleNameResolver.Resolve(this, errorHandler);
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
    }

    #endregion
  }
}
