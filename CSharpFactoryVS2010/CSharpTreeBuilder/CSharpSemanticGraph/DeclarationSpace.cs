using System.Collections.Generic;

namespace CSharpTreeBuilder.CSharpSemanticGraph
{
  // ================================================================================================
  /// <summary>
  /// A declaration space holds information about which name denotes witch entity.
  /// Methods are entered with signature used as name.
  /// </summary>
  // ================================================================================================
  public class DeclarationSpace
  {
    /// <summary>A dictionary of name + NameTableEntry pairs.</summary>
    private Dictionary<string, NameTableEntry> _NameTable;

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Initializes a new instance of the <see cref="DeclarationSpace"/> class.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public DeclarationSpace()
    {
      _NameTable = new Dictionary<string, NameTableEntry>();
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Defines a name+entity pair in the declaration space.
    /// </summary>
    /// <param name="namedEntity">An entity that has a name.</param>
    // ----------------------------------------------------------------------------------------------
    public void Define(INamedEntity namedEntity)
    {
      var name = namedEntity.DistinctiveName;

      if (_NameTable.ContainsKey(name))
      {
        _NameTable[name].AddEntity(namedEntity);
      }
      else
      {
        _NameTable.Add(name, new NameTableEntry(name, namedEntity));
      }
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets a NameTableEntry by name.
    /// </summary>
    /// <param name="name">A name.</param>
    /// <returns>The entry associated with the name, or null if the name is not defined yet.</returns>
    // ----------------------------------------------------------------------------------------------
    public NameTableEntry this[string name]
    {
      get
      {
        if (_NameTable.ContainsKey(name)) return _NameTable[name];
        return null;
      }
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets a value indicating whether the name is already defined.
    /// </summary>
    /// <param name="name">A name.</param>
    /// <returns>True if the name is defined. Otherwise false.</returns>
    // ----------------------------------------------------------------------------------------------
    public bool IsNameDefined(string name)
    {
      return _NameTable.ContainsKey(name);
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets the number of names defined.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public int NameCount
    {
      get { return _NameTable.Keys.Count; }
    }
  }
}
