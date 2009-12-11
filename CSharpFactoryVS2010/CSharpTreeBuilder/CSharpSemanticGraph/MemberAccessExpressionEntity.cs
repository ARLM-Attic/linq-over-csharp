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
    #region State

    /// <summary>Gets or sets the MemberAccessNodeResolver object.</summary>
    public MemberAccessNodeResolver MemberAccessNodeResolver { get; private set; }

    #endregion

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
    /// Initializes a new instance of the <see cref="MemberAccessExpressionEntity"/> class 
    /// by constructing it from a template instance.
    /// </summary>
    /// <param name="template">The template for the new instance.</param>
    /// <param name="typeParameterMap">The type parameter map of the new instance.</param>
    // ----------------------------------------------------------------------------------------------
    protected MemberAccessExpressionEntity(MemberAccessExpressionEntity template, TypeParameterMap typeParameterMap)
      : base(template, typeParameterMap)
    {
      if (template.MemberAccessNodeResolver != null)
      {
        MemberAccessNodeResolver = (MemberAccessNodeResolver)template.MemberAccessNodeResolver.GetGenericClone(typeParameterMap);
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
