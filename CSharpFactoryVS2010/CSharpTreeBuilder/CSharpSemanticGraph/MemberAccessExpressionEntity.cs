using System;
using CSharpTreeBuilder.CSharpSemanticGraphBuilder;
using CSharpTreeBuilder.ProjectContent;

namespace CSharpTreeBuilder.CSharpSemanticGraph
{
  // ================================================================================================
  /// <summary>
  /// This class represents a member access expression entity.
  /// </summary>
  // ================================================================================================
  public abstract class MemberAccessExpressionEntity : ExpressionEntity
  {
    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Initializes a new instance of the <see cref="MemberAccessExpressionEntity"/> class.
    /// </summary>
    /// <param name="memberAccessNodeResolver">A member access node resolver object.</param>
    // ----------------------------------------------------------------------------------------------
    protected MemberAccessExpressionEntity(MemberAccessNodeResolver memberAccessNodeResolver)
    {
      if (memberAccessNodeResolver == null)
      {
        throw new ArgumentNullException("memberAccessNodeResolver");
      }

      MemberAccessNodeResolver = memberAccessNodeResolver;
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets or sets the MemberAccessNodeResolver object.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public MemberAccessNodeResolver MemberAccessNodeResolver { get; private set; }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Evaluates this expression.
    /// </summary>
    /// <param name="errorHandler">An error handler object.</param>
    // ----------------------------------------------------------------------------------------------
    public override void Evaluate(ICompilationErrorHandler errorHandler)
    {
      if (MemberAccessNodeResolver != null)
      {
        ExpressionResult = MemberAccessNodeResolver.Resolve(this, errorHandler);
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
