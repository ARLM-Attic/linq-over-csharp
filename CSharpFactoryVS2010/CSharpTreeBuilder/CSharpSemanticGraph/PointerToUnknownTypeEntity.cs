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
  public sealed class PointerToUnknownTypeEntity : TypeEntity, IPointerType
  {
  }
}
