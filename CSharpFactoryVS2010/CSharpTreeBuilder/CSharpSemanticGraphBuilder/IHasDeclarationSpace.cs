using CSharpTreeBuilder.CSharpSemanticGraph;

namespace CSharpTreeBuilder.CSharpSemanticGraphBuilder
{
  // ================================================================================================
  /// <summary>
  /// This interface is implemented by semantic entities that define a declaration space.
  /// </summary>
  /// <remarks>
  /// The interface doesn't define methods for registering entities into a declaration space.
  /// This must be done be the entity when adding a child entity.
  /// </remarks>
  // ================================================================================================
  public interface IHasDeclarationSpace
  {
    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets a value indicating whether this entity allows a declaration of an entity 
    /// with the given type and entity name.
    /// </summary>
    /// <typeparam name="TEntityType">
    /// The type of the entity to be declared. Can be any semantic entity.
    /// </typeparam>
    /// <param name="name">The name of the entity to be declared.</param>
    /// <returns>True if the entity allows the declaration, false otherwise.</returns>
    // ----------------------------------------------------------------------------------------------
    bool AllowsDeclaration<TEntityType>(string name)
      where TEntityType : SemanticEntity;

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets a value indicating whether this entity allows a declaration of an entity 
    /// with the given type, entity name, and number of type parameters.
    /// </summary>
    /// <typeparam name="TEntityType">
    /// The type of the entity to be declared. Must be a generic capable type entity.
    /// </typeparam>
    /// <param name="name">The name of the entity to be declared.</param>
    /// <param name="typeParameterCount">The number of type parameters of the entity to be declared.</param>
    /// <returns>True if the entity allows the declaration, false otherwise.</returns>
    // ----------------------------------------------------------------------------------------------
    bool AllowsDeclaration<TEntityType>(string name, int typeParameterCount)
      where TEntityType : GenericCapableTypeEntity;

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets a value indicating whether this entity allows a declaration of an entity 
    /// with the given type and signature.
    /// </summary>
    /// <typeparam name="TEntityType">
    /// The type of the entity to be declared. Must be an overloadable semantic entity.
    /// </typeparam>
    /// <param name="signature">The signature of the entity to be declared.</param>
    /// <returns>True if the entity allows the declaration, false otherwise.</returns>
    // ----------------------------------------------------------------------------------------------
    bool AllowsDeclaration<TEntityType>(Signature signature)
      where TEntityType : SemanticEntity, IOverloadableEntity;
  }
}