using System;
using System.Collections.Generic;
using CSharpTreeBuilder.CSharpSemanticGraph;

namespace CSharpTreeBuilder.CSharpSemanticGraphBuilder
{
  // ================================================================================================
  /// <summary>
  /// This class represents a declaration slot that holds one or more namespace or type entity.
  /// </summary>
  /// <remarks>
  /// Types with the same name but different number of type parameters get registered into the same
  /// declaration slot. Only one entity with a particular number of type parameters can be registered
  /// in the slot, therefore namespaces are always alone in the slot.
  /// </remarks>
  // ================================================================================================
  public sealed class NamespaceOrTypeDeclarationSlot : DeclarationSlot
  {
    /// <summary>A dictionary that holds the registered entities. The key is the number of type parameters.</summary>
    private readonly Dictionary<int, NamespaceOrTypeEntity> _Entities;

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Initializes a new instance of the <see cref="NamespaceOrTypeDeclarationSlot"/> type.
    /// </summary>
    /// <param name="name">The name of the slot.</param>
    // ----------------------------------------------------------------------------------------------
    public NamespaceOrTypeDeclarationSlot(string name)
      : base(name)
    {
      _Entities = new Dictionary<int, NamespaceOrTypeEntity>();
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Registers an entity into this slot.
    /// </summary>
    /// <param name="entity">A namespace or type entity.</param>
    // ----------------------------------------------------------------------------------------------
    public override void Register(INamedEntity entity)
    {
      var namespaceOrTypeEntity = CastToNamespaceOrTypeEntity(entity);

      var typeParameterCount = (namespaceOrTypeEntity is GenericCapableTypeEntity)
                                 ? (namespaceOrTypeEntity as GenericCapableTypeEntity).OwnTypeParameterCount
                                 : 0;

      _Entities.Add(typeParameterCount, namespaceOrTypeEntity);
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
    /// <param name="entity">A namespace or type entity.</param>
    /// <returns>An entity registered in the slot, or null if no matching entity was found.</returns>
    /// <remarks>The criteria for matching is the number of type parameters.</remarks>
    // ----------------------------------------------------------------------------------------------
    public override INamedEntity GetBySimilarEntity(INamedEntity entity)
    {
      var namespaceOrTypeEntity = CastToNamespaceOrTypeEntity(entity);

      var typeParameterCount = (namespaceOrTypeEntity is GenericCapableTypeEntity)
                           ? (namespaceOrTypeEntity as GenericCapableTypeEntity).OwnTypeParameterCount
                           : 0;

      return GetByTypeParameterCount(typeParameterCount);
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets an entity from the slot by name. Assumes zero type parameters.
    /// </summary>
    /// <param name="name">A name.</param>
    /// <returns>An entity with the given name and zero type parameters, or null if not found.</returns>
    // ----------------------------------------------------------------------------------------------
    public override INamedEntity GetByName(string name)
    {
      return GetByTypeParameterCount(0);
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets an entity from the slot that has a certain number of type parameters.
    /// </summary>
    /// <param name="typeParameterCount">The number of type parameters.</param>
    /// <returns>An entity registered in the slot, or null if no matching entity was found.</returns>
    // ----------------------------------------------------------------------------------------------
    public NamespaceOrTypeEntity GetByTypeParameterCount(int typeParameterCount)
    {
      return _Entities.ContainsKey(typeParameterCount) ? _Entities[typeParameterCount] : null;
    }

    #region Private methods

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Casts a named entity to NamespaceOrTypeEntity and throws an exception if could not do it.
    /// </summary>
    /// <param name="entity">A named entity.</param>
    /// <returns>The entity casted to NamespaceOrTypeEntity.</returns>
    // ----------------------------------------------------------------------------------------------
    private static NamespaceOrTypeEntity CastToNamespaceOrTypeEntity(INamedEntity entity)
    {
      var namespaceOrTypeEntity = entity as NamespaceOrTypeEntity;

      if (namespaceOrTypeEntity == null)
      {
        throw new ApplicationException("Entity is null or not an NamespaceOrTypeEntity.");
      }
      return namespaceOrTypeEntity;
    }

    #endregion


  }
}
