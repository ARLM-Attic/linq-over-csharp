using System;
using CSharpTreeBuilder.CSharpSemanticGraphBuilder;
using CSharpTreeBuilder.ProjectContent;

namespace CSharpTreeBuilder.CSharpSemanticGraph
{
  // ================================================================================================
  /// <summary>
  /// This class represents a predefined type member access expression entity.
  /// </summary>
  // ================================================================================================
  public sealed class PredefinedTypeMemberAccessExpressionEntity : MemberAccessExpressionEntity
  {
    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Initializes a new instance of the <see cref="PredefinedTypeMemberAccessExpressionEntity"/> class.
    /// </summary>
    /// <param name="predefinedTypeName">An identifier representing a predefined type.</param>
    /// <param name="memberAccessNodeResolver">A member access node resolver object.</param>
    // ----------------------------------------------------------------------------------------------
    public PredefinedTypeMemberAccessExpressionEntity(
      string predefinedTypeName, 
      MemberAccessNodeResolver memberAccessNodeResolver)
      : base(memberAccessNodeResolver)
    {
      if (predefinedTypeName == null)
      {
        throw new ArgumentNullException("predefinedTypeName");
      }
      PredefinedTypeName = predefinedTypeName;
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets the identifier representing a predefined type.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public string PredefinedTypeName { get; private set; }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets the predefined type.
    /// </summary>
    /// <remarks>
    /// Propagated by the Evaluate method.
    /// </remarks>
    // ----------------------------------------------------------------------------------------------
    public TypeEntity PredefinedTypeEntity { get; private set; }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Evaluates this expression.
    /// </summary>
    /// <param name="errorHandler">An error handler object.</param>
    // ----------------------------------------------------------------------------------------------
    public override void Evaluate(ICompilationErrorHandler errorHandler)
    {
      // First resolve the qualified alias member.
      PredefinedTypeEntity = NamespaceOrTypeNameResolutionAlgorithm.ResolveBuiltInTypeName(PredefinedTypeName, SemanticGraph);

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
