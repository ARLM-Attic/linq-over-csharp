namespace CSharpTreeBuilder.CSharpSemanticGraph
{
  // ================================================================================================
  /// <summary>
  /// This class represents a pointer to a type.
  /// </summary>
  // ================================================================================================
  public sealed class PointerToTypeEntity : ConstructedTypeEntity, IPointerType
  {
    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Initializes a new instance of the <see cref="PointerToTypeEntity"/> class.
    /// </summary>
    /// <param name="embeddedType">The referent type.</param>
    // ----------------------------------------------------------------------------------------------
    public PointerToTypeEntity(TypeEntity embeddedType)
      : base(embeddedType)
    {
    }
  }
}