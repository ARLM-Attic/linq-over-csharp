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
    /// <param name="isDeclaredInSource">True if the member is explicitly declared in source code, false otherwise.</param>
    /// <param name="name">The name of the member.</param>
    /// <param name="type">The type of the field (a type entity reference).</param>
    // ----------------------------------------------------------------------------------------------
    public EnumMemberEntity(bool isDeclaredInSource, string name, SemanticEntityReference<TypeEntity> type)
      : base(isDeclaredInSource, null, type, name)
    {
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets a value indicating whether this member is invocable.
    /// </summary>
    /// <remarks>A member is invocable if it's a method or event, 
    /// or if it is a constant, field or property of a delegate type.</remarks>
    // ----------------------------------------------------------------------------------------------
    public override bool IsInvocable
    {
      get { return false; }
    }
  }
}
