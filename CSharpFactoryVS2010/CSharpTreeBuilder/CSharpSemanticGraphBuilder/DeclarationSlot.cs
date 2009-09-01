using CSharpTreeBuilder.CSharpSemanticGraph;

namespace CSharpTreeBuilder.CSharpSemanticGraphBuilder
{
  // ================================================================================================
  /// <summary>
  /// This abstract class represents a slot in a declaration space. A slot is identified by a name,
  /// and can hold one or many entities. Eg. overloaded methods get registered into the same slot.
  /// </summary>
  // ================================================================================================
  public abstract class DeclarationSlot
  {
    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Initializes a new instance of the <see cref="DeclarationSlot"/> type.
    /// </summary>
    /// <param name="name">The name of the slot.</param>
    // ----------------------------------------------------------------------------------------------
    protected DeclarationSlot(string name)
    {
      Name = name;
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets the name of the slot. 
    /// </summary>
    /// <remarks>Only entities with this name will be registered into this slot.</remarks>
    // ----------------------------------------------------------------------------------------------
    public string Name { get; private set; }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Registers an entity into this slot.
    /// </summary>
    /// <param name="entity">An entity that has a name.</param>
    // ----------------------------------------------------------------------------------------------
    public abstract void Register(INamedEntity entity);

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Removes an entity from this slot.
    /// </summary>
    /// <param name="entity">A named entity.</param>
    /// <returns>True if the slot also has to be deleted.</returns>
    // ----------------------------------------------------------------------------------------------
    public abstract bool Unregister(INamedEntity entity);

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets the number of entities in the slot.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public abstract int EntityCount { get; }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets an entity from the slot that matches the given entity.
    /// </summary>
    /// <param name="entity">An entity that has a name.</param>
    /// <returns>An entity registered in the slot, or null if no matching entity was found.</returns>
    // ----------------------------------------------------------------------------------------------
    public abstract INamedEntity GetBySimilarEntity(INamedEntity entity);

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets an entity from the slot by name only.
    /// </summary>
    /// <param name="name">A name.</param>
    /// <returns>An entity with the given name, or null if not found.</returns>
    // ----------------------------------------------------------------------------------------------
    public abstract INamedEntity GetByName(string name);
  }
}
