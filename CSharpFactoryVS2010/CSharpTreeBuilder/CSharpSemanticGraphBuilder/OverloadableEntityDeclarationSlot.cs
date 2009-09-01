using System;
using System.Collections.Generic;
using CSharpTreeBuilder.CSharpSemanticGraph;

namespace CSharpTreeBuilder.CSharpSemanticGraphBuilder
{
  // ================================================================================================
  /// <summary>
  /// This class represents a declaration slot that holds one or more overloadable entity.
  /// (methods, indexers, operators, instance constructors)
  /// </summary>
  /// <remarks>
  /// Overloadable entities with the same name but different signature get registered into the same
  /// declaration slot. The signature in the slot must be unique. The equality of signatures is 
  /// deteminde with the <see cref="SignatureEqualityComparerForDeclarationSpace"/> class,
  /// that yields true for signatures that differ only in out/ref parameter kinds.
  /// </remarks>
  // ================================================================================================
  public sealed class OverloadableEntityDeclarationSlot : DeclarationSlot
  {
    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// A dictionary that holds the registered entities. The key is the signature.
    /// </summary>
    /// <remarks>
    /// The equality of signatures is determined with 
    /// the <see cref="SignatureEqualityComparerForDeclarationSpace"/> class
    /// </remarks>
    // ----------------------------------------------------------------------------------------------
    private readonly Dictionary<Signature, IOverloadableEntity> _Entities;

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Initializes a new instance of the <see cref="OverloadableEntityDeclarationSlot"/> type.
    /// </summary>
    /// <param name="name">The name of the slot.</param>
    // ----------------------------------------------------------------------------------------------
    public OverloadableEntityDeclarationSlot(string name)
      : base(name)
    {
      _Entities = new Dictionary<Signature, IOverloadableEntity>(new SignatureEqualityComparerForDeclarationSpace());
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Registers an entity into this slot.
    /// </summary>
    /// <param name="entity">An overloadable entity.</param>
    // ----------------------------------------------------------------------------------------------
    public override void Register(INamedEntity entity)
    {
      IOverloadableEntity overloadableEntity = CastToOverloadableEntity(entity);

      _Entities.Add(overloadableEntity.Signature, overloadableEntity);
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Removes an entity from this slot.
    /// </summary>
    /// <param name="entity">A named entity.</param>
    /// <returns>True if the slot also has to be deleted.</returns>
    // ----------------------------------------------------------------------------------------------
    public override bool Unregister(INamedEntity entity)
    {
      IOverloadableEntity overloadableEntity = CastToOverloadableEntity(entity);

      _Entities.Remove(overloadableEntity.Signature);

      return _Entities.Count == 0;
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets the number of entities in the slot.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public override int EntityCount
    {
      get { return _Entities.Count; }
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets an entity from the slot that matches the given entity.
    /// </summary>
    /// <param name="entity">An overloadable entity.</param>
    /// <returns>An entity registered in the slot, or null if no matching entity was found.</returns>
    /// <remarks>Two entities match if their signature differ only in out/ref parameter kinds.</remarks>
    // ----------------------------------------------------------------------------------------------
    public override INamedEntity GetBySimilarEntity(INamedEntity entity)
    {
      IOverloadableEntity overloadableEntity = CastToOverloadableEntity(entity);

      return GetBySignature(overloadableEntity.Signature);
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets an entity from the slot by name. Assumes zero type parameters and null parameter list.
    /// </summary>
    /// <param name="name">A name.</param>
    /// <returns>An entity with the given name, zero type parameters, and null parameter list, 
    /// or null if not found.</returns>
    // ----------------------------------------------------------------------------------------------
    public override INamedEntity GetByName(string name)
    {
      return GetBySignature(new Signature(name, 0, null));
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets an entity from the slot that has a certain signature.
    /// </summary>
    /// <param name="signature">A signature.</param>
    /// <returns>An entity registered in the slot, or null if no matching entity was found.</returns>
    // ----------------------------------------------------------------------------------------------
    public IOverloadableEntity GetBySignature(Signature signature)
    {
      return _Entities.ContainsKey(signature) ? _Entities[signature] : null;
    }

    #region Private methods

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Casts a named entity to IOverloadableEntity and throws an exception if could not do it.
    /// </summary>
    /// <param name="entity">A named entity.</param>
    /// <returns>The entity casted to IOverloadableEntity.</returns>
    // ----------------------------------------------------------------------------------------------
    private static IOverloadableEntity CastToOverloadableEntity(INamedEntity entity)
    {
      var overloadableEntity = entity as IOverloadableEntity;

      if (overloadableEntity == null)
      {
        throw new ApplicationException("Entity is null or not an IOverloadableEntity.");
      }
      return overloadableEntity;
    }

    #endregion
  }
}
