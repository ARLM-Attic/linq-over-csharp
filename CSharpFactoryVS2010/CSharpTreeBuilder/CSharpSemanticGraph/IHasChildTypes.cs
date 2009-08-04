using System.Collections.Generic;

namespace CSharpTreeBuilder.CSharpSemanticGraph
{
  // ================================================================================================
  /// <summary>
  /// This interface is implemented by semantic entities that can have child types.
  /// </summary>
  // ================================================================================================
  public interface IHasChildTypes
  {
    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets an iterate-only collection of child types.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    List<TypeEntity> ChildTypes { get; }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets a child type by name. 
    /// </summary>
    /// <param name="distinctiveName">A distinctive name.</param>
    /// <returns>The type with the given name, or null if not found.</returns>
    // ----------------------------------------------------------------------------------------------
    TypeEntity GetChildTypeByDistinctiveName(string distinctiveName);

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Adds a child type. 
    /// </summary>
    /// <param name="typeEntity">The child type entity.</param>
    // ----------------------------------------------------------------------------------------------
    void AddChildType(TypeEntity typeEntity);
  }
}
