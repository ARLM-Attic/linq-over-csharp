using CSharpTreeBuilder.CSharpSemanticGraphBuilder;
using CSharpTreeBuilder.ProjectContent;

namespace CSharpTreeBuilder.CSharpSemanticGraph
{
  // ================================================================================================
  /// <summary>
  /// This class represents a qualified alias member access expression entity.
  /// </summary>
  // ================================================================================================
  public sealed class QualifiedAliasMemberAccessExpressionEntity : MemberAccessExpressionEntity
  {
    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Initializes a new instance of the <see cref="QualifiedAliasMemberAccessExpressionEntity"/> class.
    /// </summary>
    /// <param name="qualifiedAliasMemberNodeResolver">A qualified alias member resolver object.</param>
    /// <param name="memberAccessNodeResolver">A member access node resolver object.</param>
    // ----------------------------------------------------------------------------------------------
    public QualifiedAliasMemberAccessExpressionEntity(
      QualifiedAliasMemberNodeResolver qualifiedAliasMemberNodeResolver,
      MemberAccessNodeResolver memberAccessNodeResolver)
      : base(memberAccessNodeResolver)
    {
      QualifiedAliasMemberNodeResolver = qualifiedAliasMemberNodeResolver;
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets the qualified alias member node resolver object.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public QualifiedAliasMemberNodeResolver QualifiedAliasMemberNodeResolver { get; private set; }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Evaluates this expression.
    /// </summary>
    /// <param name="errorHandler">An error handler object.</param>
    // ----------------------------------------------------------------------------------------------
    public override void Evaluate(ICompilationErrorHandler errorHandler)
    {
      // First resolve the qualified alias member.
      if (QualifiedAliasMemberNodeResolver != null)
      {
        QualifiedAliasMemberNodeResolver.Resolve(this, errorHandler);
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
