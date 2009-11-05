using System;
using CSharpTreeBuilder.CSharpSemanticGraphBuilder;
using CSharpTreeBuilder.ProjectContent;

namespace CSharpTreeBuilder.CSharpSemanticGraph
{
  // ================================================================================================
  /// <summary>
  /// This class represents a default value expression entity.
  /// </summary>
  // ================================================================================================
  public sealed class DefaultValueExpressionEntity : ExpressionEntity
  {
    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Initializes a new instance of the <see cref="DefaultValueExpressionEntity"/> class.
    /// </summary>
    /// <param name="typeReference">A reference to a type entity.</param>
    // ----------------------------------------------------------------------------------------------
    public DefaultValueExpressionEntity(SemanticEntityReference<TypeEntity> typeReference)
    {
      if (typeReference == null)
      {
        throw new ArgumentNullException("typeReference");
      }

      TypeReference = typeReference;
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets the reference to a type entity.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public SemanticEntityReference<TypeEntity> TypeReference { get; private set; }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets the type.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public TypeEntity Type
    {
      get
      {
        return TypeReference != null && TypeReference.TargetEntity != null
          ? TypeReference.TargetEntity.GetMappedType(TypeParameterMap)
          : null;
      }
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Evaluates this expression.
    /// </summary>
    /// <param name="semanticGraph">The semantic graph.</param>
    /// <param name="errorHandler">An error handler object.</param>
    // ----------------------------------------------------------------------------------------------
    public override void Evaluate(SemanticGraph semanticGraph, ICompilationErrorHandler errorHandler)
    {
      // First resolve the type reference

      if (TypeReference != null)
      {
        TypeReference.Resolve(this, semanticGraph, errorHandler);
      }
      
      // Then determine the expression result
      
      if (Type != null)
      {
        ExpressionResult = new ValueExpressionResult(Type);
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
