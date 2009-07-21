namespace CSharpTreeBuilder.CSharpSemanticGraph
{
  // ================================================================================================
  /// <summary>
  /// This class represents a pointer type entity in the semantic graph.
  /// </summary>
  // ================================================================================================
  public sealed class PointerTypeEntity : TypeEmbeddingTypeEntity
  {
    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Initializes a new instance of the <see cref="PointerTypeEntity"/> class.
    /// </summary>
    /// <param name="embeddedType">The referent type.</param>
    // ----------------------------------------------------------------------------------------------
    public PointerTypeEntity(TypeEntity embeddedType)
      : base(embeddedType)
    {
    }
  }
}