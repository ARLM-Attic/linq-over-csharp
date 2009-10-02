namespace CSharpTreeBuilder.CSharpSemanticGraph
{
  // ================================================================================================
  /// <summary>
  /// This interface defines the responsibilities of a member of a type
  /// (either a nested type or a "normal" member like a field or a method).
  /// </summary>
  // ================================================================================================
  public interface IMemberEntity : INamedEntity, IHasAccessibility
  {
    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets or sets the parent of this entity.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    SemanticEntity Parent { get; set; }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets a value indicating whether this type intentionally hides an inherited member.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    bool IsNew { get; set; }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets a value indicating whether this member is invocable.
    /// </summary>
    /// <remarks>A member is invocable if it's a method or event, 
    /// or if it is a constant, field or property of a delegate type.</remarks>
    // ----------------------------------------------------------------------------------------------
    bool IsInvocable { get; }

  }
}
