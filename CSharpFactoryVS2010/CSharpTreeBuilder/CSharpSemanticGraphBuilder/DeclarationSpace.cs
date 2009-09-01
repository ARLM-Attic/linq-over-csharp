using System;
using System.Linq;
using System.Collections.Generic;
using CSharpTreeBuilder.CSharpSemanticGraph;

namespace CSharpTreeBuilder.CSharpSemanticGraphBuilder
{
  // ================================================================================================
  /// <summary>
  /// This class represents a declaration space. 
  /// Declarations found in the source code are registered and then retrieved from it.
  /// </summary>
  /// <remarks>
  /// The declaration space contains <see cref="DeclarationSlot"/> objects. 
  /// A declaration slot can hold one or more named entities. 
  /// The type of the declaration slot defines what kind of entities can be registered into the slot,
  /// and how to differentiate between the entities in the slot (eg. different method signatures).
  /// Entities in one slot always have the same name.
  /// </remarks>
  // ================================================================================================
  public class DeclarationSpace
  {
    /// <summary>The dictionary the holds the declaration slots. 
    /// The key is the name of the slot which is the name of entities registered in the slot.</summary>
    private readonly Dictionary<string, DeclarationSlot> _Slots;

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Initializes a new instance of the <see cref="DeclarationSpace"/> type.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public DeclarationSpace()
    {
      _Slots = new Dictionary<string, DeclarationSlot>();
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Registers an entity into the declaration space.
    /// </summary>
    /// <param name="entity">A named entity.</param>
    /// <remarks>
    /// Either a new slot will be created for the entity, or it will be stored in an existing one.
    /// A <see cref="DeclarationConflictException"/> exception is thrown 
    /// if the entity cannot be registered because it conflict with an already registered entity.
    /// </remarks>
    // ----------------------------------------------------------------------------------------------
    public void Register(INamedEntity entity)
    {
      DeclarationSlot slot = null;

      // If there's no slot with this name then create the slot, declare the entity and bail out.
      if (!_Slots.ContainsKey(entity.Name))
      {
        slot = CreateDeclarationSlot(entity);
        _Slots.Add(entity.Name, slot);
        slot.Register(entity);
        return;
      }

      // The slot exists, so get it.
      slot = _Slots[entity.Name];

      // Check whether the entity exists in the slot.
      var alreadyDeclaredEntity = slot.GetBySimilarEntity(entity);

      // If the entity does not exist in the slot, then declare it inside the slot and bail out.
      if (alreadyDeclaredEntity == null)
      {
        slot.Register(entity);
        return;
      }

      // An entity is already declared, error.
      throw new DeclarationConflictException(alreadyDeclaredEntity, entity);
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Removes an entity from the declaration space.
    /// </summary>
    /// <param name="entity">A named entity.</param>
    /// <remarks>If the entity is not registered then just returns without error.</remarks>
    // ----------------------------------------------------------------------------------------------
    public void Unregister(INamedEntity entity)
    {
      if (!_Slots.ContainsKey(entity.Name))
      {
        return;
      }

      var slot = _Slots[entity.Name];

      var slotMustBeDeleted = slot.Unregister(entity);

      if (slotMustBeDeleted)
      {
        _Slots.Remove(entity.Name);
      }
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Finds an entity that can be identified be name only.
    /// </summary>
    /// <typeparam name="TEntityType">
    /// The expected type of the resulting entity. Must be an INamedEntity.
    /// </typeparam>
    /// <param name="name">The name of the entity to be found.</param>
    /// <returns>The found entity, or null if not found.</returns>
    /// <remarks>
    /// If used for finding generic types then assumes zero type parameters.
    /// If used for finding overloadable entites then assumes zero type parameters and null parameter list.
    /// </remarks>
    // ----------------------------------------------------------------------------------------------
    public TEntityType FindEntityByName<TEntityType>(string name)
      where TEntityType : class, INamedEntity
    {
      var slot = GetDeclarationSlot(name);

      if (slot == null)
      {
        return null;
      }

      return slot.GetByName(name) as TEntityType;
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Finds an entity by name and number of type parameters.
    /// </summary>
    /// <typeparam name="TEntityType">
    /// The expected type of the resulting entity. Must be a subclass of NamespaceOrTypeEntity.
    /// </typeparam>
    /// <param name="name">The name of the entity to be found.</param>
    /// <param name="typeParameterCount">The number of type parameters of the entity to be found.</param>
    /// <returns>The found entity, or null if not found.</returns>
    // ----------------------------------------------------------------------------------------------
    public TEntityType FindEntityByNameAndTypeParameterCount<TEntityType>(string name, int typeParameterCount)
      where TEntityType : NamespaceOrTypeEntity
    {
      if (typeParameterCount == 0)
      {
        return FindEntityByName<TEntityType>(name);
      }

      var slot = GetDeclarationSlot(name) as NamespaceOrTypeDeclarationSlot;

      if (slot == null)
      {
        return null;
      }

      return slot.GetByTypeParameterCount(typeParameterCount) as TEntityType;
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Finds an entity by signature.
    /// </summary>
    /// <typeparam name="TEntityType">
    /// The expected type of the resulting entity. Must be an IOverloadableEntity.
    /// </typeparam>
    /// <param name="signature">A signature.</param>
    /// <returns>The found entity, or null if not found.</returns>
    // ----------------------------------------------------------------------------------------------
    public TEntityType FindEntityBySignature<TEntityType>(Signature signature) 
      where TEntityType : class, IOverloadableEntity
    {
      var slot = GetDeclarationSlot(signature.Name) as OverloadableEntityDeclarationSlot;

      if (slot == null)
      {
        return null;
      }

      var foundEntity = slot.GetBySignature(signature) as TEntityType;
      
      // If no match then return
      if (foundEntity == null)
      {
        return null;
      }

      // The declaration space uses a comparer that evaluates to equal for ref/out parameter kind mismatch.
      // But when finding an entity, we have to use a complete matching comparer.
      var comparer = new SignatureEqualityComparerForCompleteMatching();

      return comparer.Equals(signature, foundEntity.Signature) ? foundEntity : null;
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets the number of slots in the declaration space.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public int SlotCount
    {
      get
      {
        return _Slots.Count;
      }
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets the number of entities registered in the declaration space.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public int EntityCount
    {
      get
      {
        return _Slots.Sum( slot => slot.Value.EntityCount);
      }
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets a value indicating whether this entity allows a declaration of an entity 
    /// with the given type and entity name.
    /// </summary>
    /// <typeparam name="TEntityType">The type of the entity to be declared.</typeparam>
    /// <param name="name">The name of the entity to be declared.</param>
    /// <returns>True if the entity allows the declaration, false otherwise.</returns>
    // ----------------------------------------------------------------------------------------------
    public bool AllowsDeclaration<TEntityType>(string name)
      where TEntityType : SemanticEntity
    {
      // If there's no slot with the name than any kind of declaration is allowed.
      if (!_Slots.ContainsKey(name))
      {
        return true;
      }

      // If there's a slot with the name, then we must check each case.
      var slot = _Slots[name];

      // For NamespaceOrTypeDeclarationSlot: only non-type-parameter namespace-or-type-entity can be registered,
      // if the type-parameter-count is not used yet.
      if (slot is NamespaceOrTypeDeclarationSlot
        && typeof(TEntityType).IsSubclassOf(typeof(NamespaceOrTypeEntity)) 
        && typeof(TEntityType) != typeof(TypeParameterEntity))
      {
        return (slot as NamespaceOrTypeDeclarationSlot).GetByTypeParameterCount(0) == null;
      }

      // For OverloadableEntityDeclarationSlot: only IOverloadableEntity can be registered,
      // if the signature is not used yet.
      if (slot is OverloadableEntityDeclarationSlot
        && typeof(IOverloadableEntity).IsAssignableFrom(typeof(TEntityType)))
      {
        return (slot as OverloadableEntityDeclarationSlot).GetBySignature(new Signature(name, 0, null)) == null;
      }

      // All other cases: not allowed.
      // (The slot is a SimpleDeclarationSlot, or the entity type does not match the slot's type.)
      return false;
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets a value indicating whether this entity allows a declaration of an entity 
    /// with the given type, entity name, and number of type parameters.
    /// </summary>
    /// <typeparam name="TEntityType">The type of the entity to be declared.</typeparam>
    /// <param name="name">The name of the entity to be declared.</param>
    /// <param name="typeParameterCount">The number of type parameters of the entity to be declared.</param>
    /// <returns>True if the entity allows the declaration, false otherwise.</returns>
    // ----------------------------------------------------------------------------------------------
    public bool AllowsDeclaration<TEntityType>(string name, int typeParameterCount)
      where TEntityType : GenericCapableTypeEntity
    {
      // The declaration is allowed if there's no slot with the name, or there's a slot, but the signature is free.
      if (!_Slots.ContainsKey(name)
        || (_Slots[name] is NamespaceOrTypeDeclarationSlot
            && (_Slots[name] as NamespaceOrTypeDeclarationSlot).GetByTypeParameterCount(typeParameterCount) == null))
      {
        return true;
      }

      // All other case is false.
      return false;
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets a value indicating whether this entity allows a declaration of an entity 
    /// with the given type and signature.
    /// </summary>
    /// <typeparam name="TEntityType">The type of the entity to be declared.</typeparam>
    /// <param name="signature">The signature of the entity to be declared.</param>
    /// <returns>True if the entity allows the declaration, false otherwise.</returns>
    // ----------------------------------------------------------------------------------------------
    public bool AllowsDeclaration<TEntityType>(Signature signature)
      where TEntityType : SemanticEntity, IOverloadableEntity
    {
      // The declaration is allowed if there's no slot with the name, or there's a slot, but the signature is free.
      if (!_Slots.ContainsKey(signature.Name)
        || (_Slots[signature.Name] is OverloadableEntityDeclarationSlot 
            && (_Slots[signature.Name] as OverloadableEntityDeclarationSlot).GetBySignature(signature) == null))
      {
        return true;
      }

      // All other case is false.
      return false;
    }

    #region Private methods

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets a slot by name, or null if no such slot can be found.
    /// </summary>
    /// <param name="name">A name.</param>
    /// <returns>A slot or null if no slot registered with this name.</returns>
    // ----------------------------------------------------------------------------------------------
    private DeclarationSlot GetDeclarationSlot(string name)
    {
      return _Slots.ContainsKey(name) ? _Slots[name] : null;
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Creates a declaration slot for an entity.
    /// </summary>
    /// <param name="entity">A named entity.</param>
    /// <returns>The created declaration slot.</returns>
    // ----------------------------------------------------------------------------------------------
    private static DeclarationSlot CreateDeclarationSlot(INamedEntity entity)
    {
      DeclarationSlot slot = null;

      if (entity is NamespaceOrTypeEntity)
      {
        var namespaceOrTypeEntity = entity as NamespaceOrTypeEntity;
        slot = new NamespaceOrTypeDeclarationSlot(namespaceOrTypeEntity.Name);
      }
      else if (entity is IOverloadableEntity)
      {
        var overloadableEntity = entity as IOverloadableEntity;
        slot = new OverloadableEntityDeclarationSlot(overloadableEntity.Name);
      }
      else
      {
        slot = new SimpleDeclarationSlot(entity);
      }

      return slot;
    }

    #endregion

  }
}
