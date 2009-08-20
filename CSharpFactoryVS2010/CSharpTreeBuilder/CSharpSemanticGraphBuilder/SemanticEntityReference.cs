using CSharpTreeBuilder.CSharpSemanticGraph;
using CSharpTreeBuilder.ProjectContent;

namespace CSharpTreeBuilder.CSharpSemanticGraphBuilder
{
  // ================================================================================================
  /// <summary>
  /// This class represents a reference to a semantic entity.
  /// </summary>
  /// <typeparam name="TTargetEntity">The type of the target entity, must be a subclass of SemanticEntity.</typeparam>
  // ================================================================================================
  public abstract class SemanticEntityReference<TTargetEntity> where TTargetEntity : SemanticEntity
  {
    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Initializes a new instance of the <see cref="SemanticEntityReference{TTargetEntity}"/> class.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    protected SemanticEntityReference()
    {
      ResolutionState = ResolutionState.NotYetResolved;
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets the target entity. Null if not resolved.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public TTargetEntity TargetEntity { get; private set; }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets the state of the reference resolution.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public ResolutionState ResolutionState { get; private set; }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Tries to resolve this reference, and sets ResolutionState and TargetEntity accordingly.
    /// </summary>
    /// <param name="context">A semantic entity that is the context of the resolution.</param>
    /// <param name="semanticGraph">The semantic graph.</param>
    /// <param name="errorHandler">An object for error and warning reporting.</param>
    // ----------------------------------------------------------------------------------------------
    public void Resolve(SemanticEntity context, SemanticGraph semanticGraph, ICompilationErrorHandler errorHandler)
    {
      // If already resolved then bail out.
      if (ResolutionState == ResolutionState.Resolved)
      {
        return;
      }

      var resolvedEntity = GetResolvedEntity(context, semanticGraph, errorHandler);

      // Set the reference to the correct state
      if (resolvedEntity != null)
      {
        SetResolved(resolvedEntity);
      }
      else
      {
        SetUnresolvable();
      }
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Implements the resolution logic.
    /// </summary>
    /// <param name="context">A semantic entity that is the context of the resolution.</param>
    /// <param name="semanticGraph">The semantic graph.</param>
    /// <param name="errorHandler">An object for error and warning reporting.</param>
    /// <returns>The resolved entity, or null if could not resolve.</returns>
    // ----------------------------------------------------------------------------------------------
    protected abstract TTargetEntity GetResolvedEntity(
      SemanticEntity context, SemanticGraph semanticGraph, ICompilationErrorHandler errorHandler);

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Sets the reference to resolved state, and sets the target entity.
    /// </summary>
    /// <param name="resolvedEntity">The result of the resolution.</param>
    // ----------------------------------------------------------------------------------------------
    protected void SetResolved(TTargetEntity resolvedEntity)
    {
      TargetEntity = resolvedEntity;
      ResolutionState = ResolutionState.Resolved;
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Sets the reference to unresolvable state.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    protected void SetUnresolvable()
    {
      TargetEntity = null;
      ResolutionState = ResolutionState.Unresolvable;
    }

  }
}
