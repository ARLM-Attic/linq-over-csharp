using System.Collections.Generic;
using CSharpTreeBuilder.CSharpSemanticGraphBuilder;

namespace CSharpTreeBuilder.CSharpSemanticGraph
{
  // ================================================================================================
  /// <summary>
  /// This class represents a function member that has accessors (as opposed to having a body).
  /// </summary>
  // ================================================================================================
  public abstract class FunctionMemberWithAccessorsEntity : FunctionMemberEntity
  {
    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Initializes a new instance of the <see cref="FunctionMemberWithAccessorsEntity"/> class.
    /// </summary>
    /// <param name="name">The name of the member.</param>
    /// <param name="isExplicitlyDefined">True, if the member is explicitly defined, false otherwise.</param>
    /// <param name="typeReference">A reference to the type of the member.</param>
    // ----------------------------------------------------------------------------------------------
    protected FunctionMemberWithAccessorsEntity(
      string name, bool isExplicitlyDefined, SemanticEntityReference<TypeEntity> typeReference)
      : base(name, isExplicitlyDefined)
    {
      TypeReference = typeReference;
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets the reference to the type of the member.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public SemanticEntityReference<TypeEntity> TypeReference { get; private set; }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets the type of the member.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public TypeEntity Type
    {
      get { return TypeReference == null ? null : TypeReference.TargetEntity; }
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets the body of the function member.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public abstract IEnumerable<AccessorEntity> Accessors { get; }
  }
}
