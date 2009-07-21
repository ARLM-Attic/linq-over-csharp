namespace CSharpTreeBuilder.CSharpSemanticGraph
{
  // ================================================================================================
  /// <summary>
  /// This class represents a nullable type entity in the semantic graph.
  /// </summary>
  // ================================================================================================
  public sealed class NullableTypeEntity : TypeEmbeddingTypeEntity
  {
    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Initializes a new instance of the <see cref="NullableTypeEntity"/> class.
    /// </summary>
    /// <param name="embeddedType">The underlying type.</param>
    // ----------------------------------------------------------------------------------------------
    public NullableTypeEntity(TypeEntity embeddedType)
      : base(embeddedType)
    {
    }
  }
}