using CSharpTreeBuilder.CSharpSemanticGraphBuilder;

namespace CSharpTreeBuilder.CSharpSemanticGraph
{
  // ================================================================================================
  /// <summary>
  /// This class represents an enum member (a named constant).
  /// </summary>
  // ================================================================================================
  public sealed class EnumMemberEntity : FieldEntity
  {
    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Initializes a new instance of the <see cref="EnumMemberEntity"/> class.
    /// </summary>
    /// <param name="name">The name of the member.</param>
    /// <param name="type">The type of the field (a type entity reference).</param>
    /// <param name="hasInitializer">Indicates whether this enum member has an initializer.</param>
    // ----------------------------------------------------------------------------------------------
    public EnumMemberEntity(string name, SemanticEntityReference<TypeEntity> type, bool hasInitializer)
      : base(name, true, type, true, hasInitializer ? new ScalarInitializerEntity() : null)
    {
    }
  }
}
