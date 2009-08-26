using System;
using System.Collections.Generic;

namespace CSharpTreeBuilder.CSharpSemanticGraph
{
  // ================================================================================================
  /// <summary>
  /// This class associates a name with one or more named semantic entities.
  /// </summary>
  // ================================================================================================
  public sealed class NameTableEntry
  {
    /// <summary>The list of named entities associated with the name.</summary>
    private readonly List<INamedEntity> _Entities;

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Initializes a new instance of the <see cref="NameTableEntry"/> class.
    /// </summary>
    /// <param name="name">The name of the entry.</param>
    /// <param name="entity">The entity associated with the name.</param>
    // ----------------------------------------------------------------------------------------------
    public NameTableEntry(string name, INamedEntity entity)
    {
      Name = name;
      _Entities = new List<INamedEntity>() { entity };
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets the name of the entry.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public string Name { get; private set; }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets the entity associated with the name. If it is not definite then throws an error.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public INamedEntity Entity
    {
      get
      {
        if (_Entities.Count != 1)
        {
          throw new ApplicationException(String.Format("The name {0} is not definite.", Name));
        }
        return _Entities[0];
      }
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets an iterate-only collection of the entities associated woth the name.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public IEnumerable<INamedEntity> Entities
    {
      get
      {
        return _Entities;
      }
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Adds another entity to the name. The name becomes ambigous.
    /// </summary>
    /// <param name="entity">The entity to be associated with the name.</param>
    // ----------------------------------------------------------------------------------------------
    public void AddEntity(INamedEntity entity)
    {
      _Entities.Add(entity);
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets the state of the entry.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public NameTableEntryState State
    {
      get
      {
        if (_Entities.Count == 0) return NameTableEntryState.Undefined;
        if (_Entities.Count == 1) return NameTableEntryState.Definite;
        return NameTableEntryState.Ambigous;
      }
    }
  }
}
