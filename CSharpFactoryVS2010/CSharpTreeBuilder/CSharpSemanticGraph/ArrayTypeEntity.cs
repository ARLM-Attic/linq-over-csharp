namespace CSharpTreeBuilder.CSharpSemanticGraph
{
  // ================================================================================================
  /// <summary>
  /// This class represents an array type entity in the semantic graph.
  /// </summary>
  // ================================================================================================
  public sealed class ArrayTypeEntity: TypeEmbeddingTypeEntity
  {
    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Initializes a new instance of the <see cref="ArrayTypeEntity"/> class.
    /// </summary>
    /// <param name="elementType">The element type.</param>
    /// <param name="rank">The rank of the array.</param>
    // ----------------------------------------------------------------------------------------------
    public ArrayTypeEntity(TypeEntity elementType, int rank)
      : base(elementType)
    {
      Rank = rank;
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets the rank of the array (the number of array dimensions).
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public int Rank { get; private set; }
  }
}