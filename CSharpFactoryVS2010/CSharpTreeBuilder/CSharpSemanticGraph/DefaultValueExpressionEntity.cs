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
    #region State

    /// <summary>Gets the reference to a type entity.</summary>
    public Resolver<TypeEntity> TypeReference { get; private set; }

    #endregion

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Initializes a new instance of the <see cref="DefaultValueExpressionEntity"/> class.
    /// </summary>
    /// <param name="typeReference">A reference to a type entity.</param>
    // ----------------------------------------------------------------------------------------------
    public DefaultValueExpressionEntity(Resolver<TypeEntity> typeReference)
    {
      if (typeReference == null)
      {
        throw new ArgumentNullException("typeReference");
      }

      TypeReference = typeReference;
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Initializes a new instance of the <see cref="DefaultValueExpressionEntity"/> class 
    /// by constructing it from a template instance.
    /// </summary>
    /// <param name="template">The template for the new instance.</param>
    /// <param name="typeParameterMap">The type parameter map of the new instance.</param>
    // ----------------------------------------------------------------------------------------------
    private DefaultValueExpressionEntity(DefaultValueExpressionEntity template, TypeParameterMap typeParameterMap)
      : base(template, typeParameterMap)
    {
      TypeReference = template.TypeReference;
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
      return new DefaultValueExpressionEntity(this, typeParameterMap);
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets the type.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public TypeEntity Type
    {
      get
      {
        return TypeReference != null && TypeReference.Target != null
          ? TypeReference.Target.GetMappedType(TypeParameterMap)
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
      // First resolve the type reference

      if (TypeReference != null)
      {
        TypeReference.Resolve(this, errorHandler);
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
