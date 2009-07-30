namespace CSharpTreeBuilder.CSharpSemanticGraph
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
    /// Sets the reference to resolved state, and sets the target entity.
    /// </summary>
    /// <param name="resolvedEntity">The result of the resolution.</param>
    // ----------------------------------------------------------------------------------------------
    public void SetResolved(TTargetEntity resolvedEntity)
    {
      TargetEntity = resolvedEntity;
      ResolutionState = ResolutionState.Resolved;
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Sets the reference to unresolvable state.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public void SetUnresolvable()
    {
      TargetEntity = null;
      ResolutionState = ResolutionState.Unresolvable;
    }

  }
}
