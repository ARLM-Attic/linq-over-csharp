using CSharpTreeBuilder.CSharpSemanticGraphBuilder;

namespace CSharpTreeBuilder.CSharpSemanticGraph
{
  // ================================================================================================
  /// <summary>
  /// This interface is implemented by member entities that can be an explicitly implemented 
  /// interface member.
  /// </summary>
  // ================================================================================================
  public interface ICanBeExplicitlyImplementedMember
  {
    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets a value indicating whether this member is an explicitly implemented interface member.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    bool IsExplicitlyImplemented { get; }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets or sets the reference to the interface entity whose member is explicitly implemented.
    /// Null if this member is not an explicitly implemented interface member.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    SemanticEntityReference<TypeEntity> InterfaceReference { get; }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets the interface entity whose member is explicitly implemented.
    /// Null if this member is not an explicitly implemented interface member 
    /// or if the reference to the interface entity is not resolved.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    InterfaceEntity Interface { get; }
  }
}
