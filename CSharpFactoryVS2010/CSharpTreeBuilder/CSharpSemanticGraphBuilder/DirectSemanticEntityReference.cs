using CSharpTreeBuilder.CSharpSemanticGraph;
using CSharpTreeBuilder.ProjectContent;

namespace CSharpTreeBuilder.CSharpSemanticGraphBuilder
{
  // ================================================================================================
  /// <summary>
  /// This class represents a reference to an explicitly specified semantic entity.
  /// </summary>
  /// <typeparam name="TTargetEntity">The type of the target entity, must be a subclass of SemanticEntity.</typeparam>
  // ================================================================================================
  public sealed class DirectSemanticEntityReference<TTargetEntity> : SemanticEntityReference<TTargetEntity>
    where TTargetEntity : class, ISemanticEntity
  {
    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Initializes a new instance of the <see cref="DirectSemanticEntityReference{TTargetEntity}"/> class.
    /// </summary>
    /// <param name="entity">The referenced semantic entity.</param>
    // ----------------------------------------------------------------------------------------------
    public DirectSemanticEntityReference(TTargetEntity entity)
    {
      SetResolved(entity);
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Dummy implementation, because this kind of reference don't need resolution.
    /// </summary>
    /// <param name="context">A semantic entity that is the context of the resolution.</param>
    /// <param name="semanticGraph">The semantic graph.</param>
    /// <param name="errorHandler">An object for error and warning reporting.</param>
    /// <returns>Always null.</returns>
    // ----------------------------------------------------------------------------------------------
    protected override TTargetEntity GetResolvedEntity(ISemanticEntity context, SemanticGraph semanticGraph, ICompilationErrorHandler errorHandler)
    {
      return null;
    }
  }
}
