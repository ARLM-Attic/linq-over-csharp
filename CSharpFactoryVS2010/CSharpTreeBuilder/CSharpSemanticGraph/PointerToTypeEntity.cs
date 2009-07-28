using System.Text;

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

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets the distinctive name of the entity, which is unique for all entities in a declaration space.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public override string DistinctiveName
    {
      get
      {
        return EmbeddedType.DistinctiveName + "*";
      }
    }
  }
}