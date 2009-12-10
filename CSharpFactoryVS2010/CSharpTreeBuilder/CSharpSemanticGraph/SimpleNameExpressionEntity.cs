using System;
using CSharpTreeBuilder.CSharpSemanticGraphBuilder;
using CSharpTreeBuilder.ProjectContent;

namespace CSharpTreeBuilder.CSharpSemanticGraph
{
  // ================================================================================================
  /// <summary>
  /// This class represents a simple name expression entity.
  /// </summary>
  // ================================================================================================
  public sealed class SimpleNameExpressionEntity : ExpressionEntity
  {
    #region State

    /// <summary>Gets or sets the simple name resolver object.</summary>
    public SimpleNameNodeResolver SimpleNameResolver { get; private set; }

    #endregion

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Initializes a new instance of the <see cref="SimpleNameExpressionEntity"/> class.
    /// </summary>
    /// <param name="simpleNameResolver">A simple name resolver object.</param>
    // ----------------------------------------------------------------------------------------------
    public SimpleNameExpressionEntity(SimpleNameNodeResolver simpleNameResolver)
    {
      if (simpleNameResolver == null)
      {
        throw new ArgumentNullException("simpleNameResolver");
      }

      SimpleNameResolver = simpleNameResolver;
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Initializes a new instance of the <see cref="SimpleNameExpressionEntity"/> class 
    /// by constructing it from a template instance.
    /// </summary>
    /// <param name="template">The template for the new instance.</param>
    /// <param name="typeParameterMap">The type parameter map of the new instance.</param>
    // ----------------------------------------------------------------------------------------------
    private SimpleNameExpressionEntity(SimpleNameExpressionEntity template, TypeParameterMap typeParameterMap)
      : base(template, typeParameterMap)
    {
      // TODO: do we have to clone resolvers?
      SimpleNameResolver = template.SimpleNameResolver;
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
      return new SimpleNameExpressionEntity(this, typeParameterMap);
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Evaluates this expression.
    /// </summary>
    /// <param name="errorHandler">An error handler object.</param>
    // ----------------------------------------------------------------------------------------------
    public override void Evaluate(ICompilationErrorHandler errorHandler)
    {
      // If the expression was cloned from a generic template, then the result is 
      // the result of the generic template expression. 
      // Type results must also be mapped using the TypeParameterMap.
      if (HasGenericTemplate)
      {
        var templateExpressionResult = (OriginalGenericTemplate as ExpressionEntity).ExpressionResult;
        if (templateExpressionResult is TypeExpressionResult)
        {
          var mappedType = (templateExpressionResult as TypeExpressionResult).Type.GetMappedType(TypeParameterMap);
          ExpressionResult = new TypeExpressionResult(mappedType);
        }
      }
      else
      {
        if (SimpleNameResolver != null)
        {
          ExpressionResult = SimpleNameResolver.Resolve(this, errorHandler);
        }
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
