using System;
using CSharpTreeBuilder.CSharpSemanticGraphBuilder;
using CSharpTreeBuilder.ProjectContent;

namespace CSharpTreeBuilder.CSharpSemanticGraph
{
  // ================================================================================================
  /// <summary>
  /// This class represents a literal expression entity that has a type (ie. all but null literal).
  /// </summary>
  // ================================================================================================
  public class TypedLiteralExpressionEntity : LiteralExpressionEntity
  {
    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Initializes a new instance of the <see cref="TypedLiteralExpressionEntity"/> class.
    /// </summary>
    /// <param name="typeEntityReference">A reference to the type of the literal.</param>
    /// <param name="value">The value of the literal.</param>
    // ----------------------------------------------------------------------------------------------
    public TypedLiteralExpressionEntity(SemanticEntityReference<TypeEntity> typeEntityReference, object value)
    {
      if (typeEntityReference == null)
      {
        throw new ArgumentNullException("typeEntityReference");
      }

      TypeReference = typeEntityReference;
      Value = value;
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets the reference to the type of the literal.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public SemanticEntityReference<TypeEntity> TypeReference { get; private set; }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets the type of the literal.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public TypeEntity Type
    {
      get
      {
        return TypeReference == null ? null : TypeReference.TargetEntity;
      }
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets the value of the literal.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public object Value { get; private set; }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Evaluates this expression.
    /// </summary>
    /// <param name="semanticGraph">The semantic graph.</param>
    /// <param name="errorHandler">An error handler object.</param>
    // ----------------------------------------------------------------------------------------------
    public override void Evaluate(SemanticGraph semanticGraph, ICompilationErrorHandler errorHandler)
    {
      // First resolve the type of the literal.

      if (TypeReference != null)
      {
        TypeReference.Resolve(this, semanticGraph, errorHandler);
      }

      // Then obtain the result of the expression.

      if (Type != null)
      {
        Result = new ValueExpressionResult(Type);
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
      if (!visitor.Visit(this)) { return; }
    }

    #endregion
  }
}
