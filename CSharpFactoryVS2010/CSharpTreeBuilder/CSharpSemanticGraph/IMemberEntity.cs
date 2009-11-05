namespace CSharpTreeBuilder.CSharpSemanticGraph
{
  // ================================================================================================
  /// <summary>
  /// This interface defines the responsibilities of a member of a type
  /// (either a nested type or a "normal" member like a field or a method).
  /// </summary>
  // ================================================================================================
  public interface IMemberEntity : IGenericSupportingSemanticEntity, INamedEntity, IHasAccessibility
  {
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

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets a value indicating whether this is an instance member.
    /// </summary>
    /// <remarks>
    /// The members of a class are either static members or instance members. 
    /// Static members belong to classes, and instance members belong to objects (instances of classes).
    /// </remarks>
    // ----------------------------------------------------------------------------------------------
    bool IsInstanceMember { get; }
  }
}
