namespace CSharpTreeBuilder.CSharpSemanticGraph
{
  // ================================================================================================
  /// <summary>
  /// This interface defines the behaviour of an entity that has accessibility modifiers.
  /// </summary>
  // ================================================================================================
  public interface IHasAccessibility
  {
    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets or sets the declared accessibility of the entity.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    AccessibilityKind? DeclaredAccessibility { get; set; }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets the effective accessibility of the entity.
    /// </summary>
    /// <remarks>
    /// If there's no declared accessibility then returns the default accessibility.
    /// If the default cannot be determined (eg. no parent entity) then returns null.
    /// </remarks>
    // ----------------------------------------------------------------------------------------------
    AccessibilityKind? EffectiveAccessibility { get; }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets a value indicating whether this entity is accessible by another entity.
    /// </summary>
    /// <param name="entity">The accessing entity.</param>
    /// <returns>True if the accessing entity can access this entity, false otherwise.</returns>
    // ----------------------------------------------------------------------------------------------
    bool IsAccessibleBy(SemanticEntity entity);
  }
}
