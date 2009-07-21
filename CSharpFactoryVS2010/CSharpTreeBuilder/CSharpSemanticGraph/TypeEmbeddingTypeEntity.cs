namespace CSharpTreeBuilder.CSharpSemanticGraph
{
  // ================================================================================================
  /// <summary>
  /// This abstract class represents a type entity that embed (build upon) another type: 
  /// an array, a pointer or a nullable type. (Generics are not in this group.)
  /// </summary>
  // ================================================================================================
  public abstract class TypeEmbeddingTypeEntity : TypeEntity
  {
    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Initializes a new instance of the <see cref="TypeEmbeddingTypeEntity"/> class.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    protected TypeEmbeddingTypeEntity(TypeEntity embeddedType)
    {
      EmbeddedType = embeddedType;
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets the embedded type that this type build upon.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public TypeEntity EmbeddedType { get; private set; }
  }
}
