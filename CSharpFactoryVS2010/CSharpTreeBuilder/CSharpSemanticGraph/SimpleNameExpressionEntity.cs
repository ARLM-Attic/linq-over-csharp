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
  public class SimpleNameExpressionEntity : ExpressionEntity
  {
    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Initializes a new instance of the <see cref="SimpleNameExpressionEntity"/> class.
    /// </summary>
    /// <param name="entityReference">A reference to a semantic entity.</param>
    // ----------------------------------------------------------------------------------------------
    public SimpleNameExpressionEntity(SemanticEntityReference<SemanticEntity> entityReference)
    {
      if (entityReference == null)
      {
        throw new ArgumentNullException("entityReference");
      }

      EntityReference = entityReference;
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets the reference to an entity.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public SemanticEntityReference<SemanticEntity> EntityReference { get; private set; }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets the entity denoted by this simple name.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public SemanticEntity Entity
    {
      get
      {
        // TODO: if TypeEntity then return TargetEntity.GetMappedType(TypeParameterMap) ???

        return EntityReference == null ? null : EntityReference.TargetEntity;
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
      // First resolve the entity reference

      if (EntityReference != null)
      {
        EntityReference.Resolve(this, semanticGraph, errorHandler);
      }
      
      // Then determine the expression result
      
      if (Entity != null)
      {
        if (Entity is IVariableEntity)
        {
          Result = new VariableExpressionResult(Entity as IVariableEntity);
        }

        // TODO
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
