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
    /// Adds a child type. 
    /// </summary>
    /// <param name="typeEntity">The type entity.</param>
    // ----------------------------------------------------------------------------------------------
    void AddChildType(TypeEntity typeEntity);

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Removes a child type. 
    /// </summary>
    /// <param name="typeEntity">The type entity.</param>
    // ----------------------------------------------------------------------------------------------
    void RemoveChildType(TypeEntity typeEntity);
    
    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets an iterate-only collection of child types.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    IEnumerable<TypeEntity> ChildTypes { get; }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets a collection of child type entities by type and name.
    /// </summary>
    /// <param name="name">The name of the type.</param>
    /// <returns>A collection of child type entities, possibly empty.</returns>
    // ----------------------------------------------------------------------------------------------
    IEnumerable<TEntityType> GetChildTypes<TEntityType>(string name) where TEntityType : TypeEntity;

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets a collection of child type entities by type, name and number of type parameters.
    /// </summary>
    /// <param name="name">The name of the type.</param>
    /// <param name="typeParameterCount">The number of type parameters.</param>
    /// <returns>A collection of child type entities, possibly empty.</returns>
    // ----------------------------------------------------------------------------------------------
    IEnumerable<TEntityType> GetChildTypes<TEntityType>(string name, int typeParameterCount) where TEntityType : TypeEntity;

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets a child type entity by type and name.
    /// </summary>
    /// <param name="name">The name of the type.</param>
    /// <returns>The found type, or null if not found.</returns>
    /// <remarks>Throws AmbiguousDeclarationsException if more than one type was found.</remarks>
    // ----------------------------------------------------------------------------------------------
    TEntityType GetSingleChildType<TEntityType>(string name) where TEntityType : TypeEntity;

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets a child type entity by type, name and number of type parameters.
    /// </summary>
    /// <param name="name">The name of the type.</param>
    /// <param name="typeParameterCount">The number of type parameters.</param>
    /// <returns>The found type, or null if not found.</returns>
    /// <remarks>Throws AmbiguousDeclarationsException if more than one type was found.</remarks>
    // ----------------------------------------------------------------------------------------------
    TEntityType GetSingleChildType<TEntityType>(string name, int typeParameterCount) where TEntityType : TypeEntity;
  }
}
