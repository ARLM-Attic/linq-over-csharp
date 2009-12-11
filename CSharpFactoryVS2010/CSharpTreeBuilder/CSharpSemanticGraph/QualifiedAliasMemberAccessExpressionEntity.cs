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
    #region State

    /// <summary>Gets the qualified alias member node resolver object.</summary>
    public QualifiedAliasMemberNodeResolver QualifiedAliasMemberNodeResolver { get; private set; }

    #endregion

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
    /// Initializes a new instance of the <see cref="QualifiedAliasMemberAccessExpressionEntity"/> class 
    /// by constructing it from a template instance.
    /// </summary>
    /// <param name="template">The template for the new instance.</param>
    /// <param name="typeParameterMap">The type parameter map of the new instance.</param>
    // ----------------------------------------------------------------------------------------------
    private QualifiedAliasMemberAccessExpressionEntity(QualifiedAliasMemberAccessExpressionEntity template, TypeParameterMap typeParameterMap)
      : base(template, typeParameterMap)
    {
      if (template.QualifiedAliasMemberNodeResolver != null)
      {
        QualifiedAliasMemberNodeResolver =
          (QualifiedAliasMemberNodeResolver)template.QualifiedAliasMemberNodeResolver.GetGenericClone(typeParameterMap);
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
      return new QualifiedAliasMemberAccessExpressionEntity(this, typeParameterMap);
    }
    
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
