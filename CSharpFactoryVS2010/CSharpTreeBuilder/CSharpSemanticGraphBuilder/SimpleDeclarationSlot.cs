using CSharpTreeBuilder.CSharpSemanticGraph;

namespace CSharpTreeBuilder.CSharpSemanticGraphBuilder
{
  // ================================================================================================
  /// <summary>
  /// This class represents a declaration slot that holds only one entity.
  /// </summary>
  // ================================================================================================
  public sealed class SimpleDeclarationSlot : DeclarationSlot
  {
    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Initializes a new instance of the <see cref="SimpleDeclarationSlot"/> type.
    /// </summary>
    /// <param name="entity">A named entity.</param>
    // ----------------------------------------------------------------------------------------------
    public SimpleDeclarationSlot(INamedEntity entity)
      : base(entity.Name)
    {
      Entity = entity;
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets the entity registered in the slot.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public INamedEntity Entity { get; private set; }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Registers an entity into this slot.
    /// </summary>
    /// <param name="entity">A named entity.</param>
    // ----------------------------------------------------------------------------------------------
    public override void Register(INamedEntity entity)
    {
      // This is intentionally blank. The entity is already registered in the constructor.
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets the number of entities in the slot. Always 1 for simple declaration slots.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public override int EntityCount
    {
      get { return 1; }
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets an entity from the slot that matches the given entity.
    /// </summary>
    /// <param name="entity">A named entity.</param>
    /// <returns>The entity registered in the slot, or null if no match.</returns>
    /// <remarks>The criteria for matching is name equality.</remarks>
    // ----------------------------------------------------------------------------------------------
    public override INamedEntity GetBySimilarEntity(INamedEntity entity)
    {
      return entity.Name == Entity.Name ? Entity : null;
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets an entity from the slot by name.
    /// </summary>
    /// <param name="name">A name.</param>
    /// <returns>An entity with the given name, or null if not found.</returns>
    /// <remarks>Throws an exception if the name does not definitely identify an entity.</remarks>
    // ----------------------------------------------------------------------------------------------
    public override INamedEntity GetByName(string name)
    {
      return name == Entity.Name ? Entity : null;
    }
  }
}
