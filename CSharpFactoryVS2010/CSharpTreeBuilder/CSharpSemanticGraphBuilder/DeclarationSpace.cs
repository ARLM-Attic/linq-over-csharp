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
  /// Does not check declaration conflicts.
  /// An entity cannot be registered multiple times.
  /// </remarks>
  // ================================================================================================
  public class DeclarationSpace
  {
    /// <summary>
    /// The dictionary of registered names. The values are lists of declarations with the same name.
    /// </summary>
    /// <remarks>
    /// This data structure is just a quick and simple optimisation of the first naive implementation
    /// of having just a flat list of DeclarationSpaceEntry objects and using Linq to find objects.
    /// It's still quite a bottleneck though.
    /// TODO: further optimisation (maybe eliminate Linq usage?)
    /// </remarks>
    private readonly Dictionary<string, List<DeclarationSpaceEntry>> _NameTable;

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Initializes a new instance of the <see cref="DeclarationSpace"/> type.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public DeclarationSpace()
    {
      _NameTable = new Dictionary<string, List<DeclarationSpaceEntry>>();
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Registers a semantic entity into the declaration space.
    /// </summary>
    /// <param name="entity">A named semantic entity.</param>
    /// <remarks>Doesn't allow the registration of an already registered entity.</remarks>
    // ----------------------------------------------------------------------------------------------
    public void Register(INamedEntity entity)
    {
      if (GetDeclarationSpaceEntry(entity) != null)
      {
        throw new InvalidOperationException("Entities cannot be registered multiple times.");
      }

      DeclarationSpaceEntry declarationSpaceEntry = null;

      if (entity is IOverloadableEntity)
      {
        var overloadableEntity = entity as IOverloadableEntity;
        declarationSpaceEntry = new DeclarationSpaceEntry(entity, overloadableEntity.Signature);
      }
      else if (entity is ICanHaveTypeParameters)
      {
        var typeParameterHolder = entity as ICanHaveTypeParameters;
        declarationSpaceEntry = new DeclarationSpaceEntry(entity, entity.Name, typeParameterHolder.OwnTypeParameterCount);
      }
      else
      {
        declarationSpaceEntry = new DeclarationSpaceEntry(entity, entity.Name);
      }

      if (!_NameTable.ContainsKey(declarationSpaceEntry.Name))
      {
        _NameTable.Add(declarationSpaceEntry.Name, new List<DeclarationSpaceEntry>());
      }

      _NameTable[declarationSpaceEntry.Name].Add(declarationSpaceEntry);
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
      if (_NameTable.ContainsKey(entity.Name))
      {
        var declarationSpaceEntry = GetDeclarationSpaceEntry(entity);
        if (declarationSpaceEntry != null)
        {
          _NameTable[entity.Name].Remove(declarationSpaceEntry);
        }
      }
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Get a declaration space entry be the registered entity.
    /// </summary>
    /// <param name="entity">A named entity.</param>
    /// <returns>
    /// A declaration space entry where the name entity is registered, or null if not found.
    /// </returns>
    /// <remarks>
    /// There can be no more than one matching entry, because Register doesn't allow
    /// the registration of the same entity multiple times.
    /// </remarks>
    // ----------------------------------------------------------------------------------------------
    public DeclarationSpaceEntry GetDeclarationSpaceEntry(INamedEntity entity)
    {
      if (_NameTable.ContainsKey(entity.Name))
      {
        return (from declarationSpaceEntry in _NameTable[entity.Name]
                where declarationSpaceEntry.Entity == entity
                select declarationSpaceEntry).FirstOrDefault();
      }
      return null;
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Returns matching entities by entity type and name.
    /// </summary>
    /// <typeparam name="TEntityType">
    /// The expected type of the resulting entities. Must be an INamedEntity.
    /// </typeparam>
    /// <param name="name">A name.</param>
    /// <returns>An iterate-only collection of entites.</returns>
    // ----------------------------------------------------------------------------------------------
    public IEnumerable<TEntityType> GetEntities<TEntityType>(string name)
      where TEntityType : class, INamedEntity
    {
      if (_NameTable.ContainsKey(name))
      {
        return from declarationSpaceEntry in _NameTable[name]
               where declarationSpaceEntry.Entity is TEntityType
               select declarationSpaceEntry.Entity as TEntityType;
      }
      return new List<TEntityType>();
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Returns matching entities by entity type, name and number of type parameters.
    /// </summary>
    /// <typeparam name="TEntityType">
    /// The expected type of the resulting entities. Must be an INamedEntity.
    /// </typeparam>
    /// <param name="name">A name.</param>
    /// <param name="typeParameterCount">Number of type parameters.</param>
    /// <returns>An iterate-only collection of entites.</returns>
    // ----------------------------------------------------------------------------------------------
    public IEnumerable<TEntityType> GetEntities<TEntityType>(string name, int typeParameterCount)
      where TEntityType : class, INamedEntity
    {
      if (_NameTable.ContainsKey(name))
      {
        return from declarationSpaceEntry in _NameTable[name]
               where declarationSpaceEntry.TypeParameterCount == typeParameterCount
                     && declarationSpaceEntry.Entity is TEntityType
               select declarationSpaceEntry.Entity as TEntityType;
      }

      return new List<TEntityType>();
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Returns matching entities by entity type and signature.
    /// </summary>
    /// <typeparam name="TEntityType">
    /// The expected type of the resulting entities. Must be an IOverloadableEntity.
    /// </typeparam>
    /// <param name="signature">A signature.</param>
    /// <returns>An iterate-only collection of entites.</returns>
    // ----------------------------------------------------------------------------------------------
    public IEnumerable<TEntityType> GetEntities<TEntityType>(Signature signature)
      where TEntityType : class, IOverloadableEntity
    {
      if (_NameTable.ContainsKey(signature.Name))
      {
        var signatureComparer = new SignatureEqualityComparerForCompleteMatching();

        return from declarationSpaceEntry in _NameTable[signature.Name]
               where signatureComparer.Equals(signature,
                                              new Signature(declarationSpaceEntry.Name,
                                                            declarationSpaceEntry.TypeParameterCount,
                                                            declarationSpaceEntry.Parameters))
                     && declarationSpaceEntry.Entity is TEntityType
               select declarationSpaceEntry.Entity as TEntityType;
      }

      return new List<TEntityType>();
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Returns an entity by entity type and name.
    /// </summary>
    /// <typeparam name="TEntityType">
    /// The expected type of the resulting entity. Must be an INamedEntity.
    /// </typeparam>
    /// <param name="name">A name.</param>
    /// <returns>The found entity, or null if not found.</returns>
    /// <remarks>Throws AmbiguousDeclarationsException if there are multiple matching entities.</remarks>
    // ----------------------------------------------------------------------------------------------
    public TEntityType GetSingleEntity<TEntityType>(string name)
      where TEntityType : class, INamedEntity
    {
      var entityList = GetEntities<TEntityType>(name).ToList();
      
      if (entityList.Count > 1)
      {
        throw new AmbiguousDeclarationsException(entityList.Cast<INamedEntity>());
      }

      if (entityList.Count == 0)
      {
        return null;
      }

      return entityList[0];
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Returns an entity by entity type and name.
    /// </summary>
    /// <typeparam name="TEntityType">
    /// The expected type of the resulting entity. Must be an INamedEntity.
    /// </typeparam>
    /// <param name="name">A name.</param>
    /// <param name="typeParameterCount">Number of type parameters.</param>
    /// <returns>The found entity, or null if not found.</returns>
    /// <remarks>Throws AmbiguousDeclarationsException if there are multiple matching entities.</remarks>
    // ----------------------------------------------------------------------------------------------
    public TEntityType GetSingleEntity<TEntityType>(string name, int typeParameterCount)
      where TEntityType : class, INamedEntity
    {
      var entityList = GetEntities<TEntityType>(name, typeParameterCount).ToList();

      if (entityList.Count > 1)
      {
        throw new AmbiguousDeclarationsException(entityList.Cast<INamedEntity>());
      }

      if (entityList.Count == 0)
      {
        return null;
      }

      return entityList[0];
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Returns an entity by entity type and name.
    /// </summary>
    /// <typeparam name="TEntityType">
    /// The expected type of the resulting entity. Must be an IOverloadableEntity.
    /// </typeparam>
    /// <param name="signature">A signature.</param>
    /// <returns>The found entity, or null if not found.</returns>
    /// <remarks>Throws AmbiguousDeclarationsException if there are multiple matching entities.</remarks>
    // ----------------------------------------------------------------------------------------------
    public TEntityType GetSingleEntity<TEntityType>(Signature signature)
      where TEntityType : class, IOverloadableEntity
    {
      var entityList = GetEntities<TEntityType>(signature).ToList();

      if (entityList.Count > 1)
      {
        throw new AmbiguousDeclarationsException(entityList.Cast<INamedEntity>());
      }

      if (entityList.Count == 0)
      {
        return null;
      }

      return entityList[0];
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets the number of declarations in the declaration space.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public int DeclarationCount
    {
      get
      {
        return _NameTable.Sum(x => x.Value.Count);
      }
    }
  }
}
