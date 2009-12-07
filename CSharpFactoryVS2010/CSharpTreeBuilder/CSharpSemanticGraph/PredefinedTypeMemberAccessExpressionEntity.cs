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
    #region State

    /// <summary>Gets the identifier representing a predefined type.</summary>
    public string PredefinedTypeName { get; private set; }

    /// <summary>Gets the predefined type entity.</summary>
    /// <remarks>Propagated by the Evaluate method.</remarks>
    public TypeEntity PredefinedTypeEntity { get; private set; }

    #endregion

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
    /// Initializes a new instance of the <see cref="PredefinedTypeMemberAccessExpressionEntity"/> class 
    /// by constructing it from a template instance.
    /// </summary>
    /// <param name="template">The template for the new instance.</param>
    /// <param name="typeParameterMap">The type parameter map of the new instance.</param>
    // ----------------------------------------------------------------------------------------------
    private PredefinedTypeMemberAccessExpressionEntity(PredefinedTypeMemberAccessExpressionEntity template, TypeParameterMap typeParameterMap)
      : base(template, typeParameterMap)
    {
      PredefinedTypeName = template.PredefinedTypeName;
      PredefinedTypeEntity = template.PredefinedTypeEntity;
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
      return new PredefinedTypeMemberAccessExpressionEntity(this, typeParameterMap);
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
      if (PredefinedTypeEntity == null)
      {
        PredefinedTypeEntity = NamespaceOrTypeNameResolutionAlgorithm.ResolveBuiltInTypeName(PredefinedTypeName, SemanticGraph);
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
