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
    /// Gets a child type by name and number of type parameters.
    /// </summary>
    /// <param name="name">The name of the type.</param>
    /// <param name="typeParameterCount">The number of type parameters.</param>
    /// <returns>The type with the given name and number of type parameters, or null if not found.</returns>
    // ----------------------------------------------------------------------------------------------
    TypeEntity GetChildType(string name, int typeParameterCount);

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Adds a child type. 
    /// </summary>
    /// <param name="typeEntity">The child type entity.</param>
    // ----------------------------------------------------------------------------------------------
    void AddChildType(TypeEntity typeEntity);
  }
}
