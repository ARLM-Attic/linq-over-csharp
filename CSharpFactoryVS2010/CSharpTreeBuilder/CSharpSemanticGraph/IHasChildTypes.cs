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
    /// Adds a child type. 
    /// </summary>
    /// <param name="typeEntity">The child type entity.</param>
    // ----------------------------------------------------------------------------------------------
    void AddChildType(TypeEntity typeEntity);
  }
}
