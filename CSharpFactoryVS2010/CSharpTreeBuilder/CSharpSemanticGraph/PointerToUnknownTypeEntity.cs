using CSharpTreeBuilder.CSharpSemanticGraphBuilder;

namespace CSharpTreeBuilder.CSharpSemanticGraph
{
  // ================================================================================================
  /// <summary>
  /// This class represents a pointer to an unknown type (void*).
  /// </summary>
  /// <remarks>
  /// This type cannot be represented as a PointerType, because PointerType needs an underlying type,
  /// but 'void' is not a type. (Though 'void*' is a type).
  /// </remarks>
  // ================================================================================================
  public sealed class PointerToUnknownTypeEntity : TypeEntity
  {
    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Initializes a new instance of the <see cref="PointerToUnknownTypeEntity"/> class.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public PointerToUnknownTypeEntity()
      : base("void*")
    {
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets a value indicating whether this type is a pointer type.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public override bool IsPointerType
    {
      get { return true; }
    }
  }
}
