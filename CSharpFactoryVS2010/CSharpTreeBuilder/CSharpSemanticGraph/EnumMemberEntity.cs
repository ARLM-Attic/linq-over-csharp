using CSharpTreeBuilder.CSharpSemanticGraphBuilder;
using System;

namespace CSharpTreeBuilder.CSharpSemanticGraph
{
  // ================================================================================================
  /// <summary>
  /// This class represents an enum member (a named constant).
  /// </summary>
  // ================================================================================================
  public sealed class EnumMemberEntity : ConstantMemberEntity
  {
    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Initializes a new instance of the <see cref="EnumMemberEntity"/> class.
    /// </summary>
    /// <param name="name">The name of the member.</param>
    /// <param name="type">The type of the field (a type entity reference).</param>
    // ----------------------------------------------------------------------------------------------
    public EnumMemberEntity(string name, SemanticEntityReference<TypeEntity> type)
      : base(true, null, type, name)
    {
    }
  }
}
