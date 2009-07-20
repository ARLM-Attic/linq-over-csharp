namespace CSharpTreeBuilder.CSharpSemanticGraph
{
  // ================================================================================================
  /// <summary>
  /// Enumerates the possible states of nametable entries.
  /// </summary>
  // ================================================================================================
  public enum NameTableEntryState
  {
    /// <summary>The name is not associated with any entity.</summary>
    Undefined,

    /// <summary>The name definitely identifies one and only one entity.</summary>
    Definite,

    /// <summary>The name is ambigous, because more than one entity is associated with it.</summary>
    Ambigous
  }
}
