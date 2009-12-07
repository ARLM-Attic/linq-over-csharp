using System.Collections.Generic;

namespace CSharpTreeBuilder.CSharpSemanticGraph
{
  // ================================================================================================
  /// <summary>
  /// This interface is implemented by semantic entities that can have type parameters.
  /// </summary>
  // ================================================================================================
  public interface ICanHaveTypeParameters : ISemanticEntity
  {
    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets a collection of all type parameters of this type (parent's + own).
    /// Empty list for non-generic types.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    IEnumerable<TypeParameterEntity> AllTypeParameters { get; }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets a collection of the own type parameters of this type (excluding parents' type params). 
    /// Empty list for non-generic types.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    IEnumerable<TypeParameterEntity> OwnTypeParameters { get; }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets the number of all type parameters.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    int AllTypeParameterCount { get; }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets the number of own type parameters.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    int OwnTypeParameterCount { get; }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets a value indicating whether this is a generic type. 
    /// </summary>
    /// <remarks>It's a generic type if it has any type parameters (parent's or own).</remarks>
    // ----------------------------------------------------------------------------------------------
    bool IsGeneric { get; }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Adds a type parameter to this type.
    /// </summary>
    /// <param name="typeParameterEntity">The type parameter entity to add.</param>
    // ----------------------------------------------------------------------------------------------
    void AddTypeParameter(TypeParameterEntity typeParameterEntity);

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Removes a type parameter entity from the type.
    /// </summary>
    /// <param name="typeParameterEntity">The type parameter entity to remove.</param>
    // ----------------------------------------------------------------------------------------------
    void RemoveTypeParameter(TypeParameterEntity typeParameterEntity);

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets an own type parameter by name.
    /// </summary>
    /// <param name="name">The name of the type parameter to be found.</param>
    /// <returns>A type parameter entity, or null if not found.</returns>
    // ----------------------------------------------------------------------------------------------
    TypeParameterEntity GetOwnTypeParameterByName(string name);

  }
}
