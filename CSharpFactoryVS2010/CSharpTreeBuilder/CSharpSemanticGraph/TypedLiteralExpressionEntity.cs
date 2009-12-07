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
  public sealed class TypedLiteralExpressionEntity : LiteralExpressionEntity
  {
    #region State

    /// <summary>Gets the reference to the type of the literal.</summary>
    public Resolver<TypeEntity> TypeReference { get; private set; }

    /// <summary>Gets the value of the literal.</summary>
    public object Value { get; private set; }

    #endregion

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Initializes a new instance of the <see cref="TypedLiteralExpressionEntity"/> class.
    /// </summary>
    /// <param name="typeEntityReference">A reference to the type of the literal.</param>
    /// <param name="value">The value of the literal.</param>
    // ----------------------------------------------------------------------------------------------
    public TypedLiteralExpressionEntity(Resolver<TypeEntity> typeEntityReference, object value)
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
    /// Initializes a new instance of the <see cref="TypedLiteralExpressionEntity"/> class 
    /// by constructing it from a template instance.
    /// </summary>
    /// <param name="template">The template for the new instance.</param>
    /// <param name="typeParameterMap">The type parameter map of the new instance.</param>
    // ----------------------------------------------------------------------------------------------
    private TypedLiteralExpressionEntity(TypedLiteralExpressionEntity template, TypeParameterMap typeParameterMap)
      : base(template, typeParameterMap)
    {
      TypeReference = template.TypeReference;
      Value = template.Value;

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
      return new TypedLiteralExpressionEntity(this, typeParameterMap);
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets the type of the literal.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public TypeEntity Type
    {
      get
      {
        return TypeReference != null && TypeReference.Target != null
          ? TypeReference.Target
          : null;
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
      // First resolve the type of the literal.

      if (TypeReference != null)
      {
        TypeReference.Resolve(this, errorHandler);
      }

      // Then obtain the result of the expression.

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
