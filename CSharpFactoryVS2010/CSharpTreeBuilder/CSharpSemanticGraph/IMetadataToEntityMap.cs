namespace CSharpTreeBuilder.CSharpSemanticGraph
{
  // ================================================================================================
  /// <summary>
  /// This interface is implemented by those classes that provide a metadataObject-to-entity map.
  /// </summary>
  // ================================================================================================
  public interface IMetadataToEntityMap
  {
    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Adds a metadata+entity pair to the mapping cache.
    /// </summary>
    /// <param name="metadata">A metadata object.</param>
    /// <param name="semanticEntity">A semantic entity.</param>
    // ----------------------------------------------------------------------------------------------
    void AddMapping(System.Reflection.MemberInfo metadata, SemanticEntity semanticEntity);

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Indexer operator that returns the semantic entity that is mapped to the supplied metadata object.
    /// </summary>
    /// <param name="metadata">A metadata object.</param>
    /// <returns>The semantic entity mapped to the metadata object. Null if not found.</returns>
    // ----------------------------------------------------------------------------------------------
    SemanticEntity this[System.Reflection.MemberInfo metadata] { get; }
  }
}
