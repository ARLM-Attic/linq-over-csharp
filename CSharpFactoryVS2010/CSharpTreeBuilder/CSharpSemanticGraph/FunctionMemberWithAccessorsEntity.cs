using System.Collections.Generic;
using CSharpTreeBuilder.CSharpSemanticGraphBuilder;

namespace CSharpTreeBuilder.CSharpSemanticGraph
{
  // ================================================================================================
  /// <summary>
  /// This class represents a function member that has accessors (as opposed to having a body).
  /// </summary>
  // ================================================================================================
  public abstract class FunctionMemberWithAccessorsEntity : FunctionMemberEntity, ICanBeExplicitlyImplementedMember
  {
    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Initializes a new instance of the <see cref="FunctionMemberWithAccessorsEntity"/> class.
    /// </summary>
    /// <param name="isDeclaredInSource">True if the member is explicitly declared in source code, false otherwise.</param>
    /// <param name="accessibility">The declared accessibility of the member. Can be null.</param>
    /// <param name="typeReference">A reference to the type of the member.</param>
    /// <param name="interfaceReference">
    /// A reference to in interface, if the member is explicitly implemented interface member.
    /// Null otherwise.
    /// </param>
    /// <param name="name">The name of the member.</param>
    // ----------------------------------------------------------------------------------------------
    protected FunctionMemberWithAccessorsEntity(
      bool isDeclaredInSource,
      AccessibilityKind? accessibility,
      SemanticEntityReference<TypeEntity> typeReference,
      SemanticEntityReference<TypeEntity> interfaceReference,
      string name)
      :
      base(isDeclaredInSource, accessibility, name)
    {
      TypeReference = typeReference;
      InterfaceReference = interfaceReference;
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

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets a value indicating whether this member is an explicitly implemented interface member.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public bool IsExplicitlyImplemented
    {
      get
      {
        return InterfaceReference != null;
      }
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets the reference to the interface entity whose member is explicitly implemented.
    /// Null if this member is not an explicitly implemented interface member.
    /// </summary>
    /// <remarks>
    /// The reference points to a TypeEntity rather then an InterfaceEntity, 
    /// because it can be a ConstructedGenericType as well (if the interface is a generic).
    /// </remarks>
    // ----------------------------------------------------------------------------------------------
    public SemanticEntityReference<TypeEntity> InterfaceReference { get; private set; }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets the interface entity whose member is explicitly implemented.
    /// Null if this member is not an explicitly implemented interface member 
    /// or if the reference to the interface entity is not yet resolved.
    /// </summary>
    /// <remarks>
    /// The type is a TypeEntity rather then an InterfaceEntity, 
    /// because it can be a ConstructedGenericType as well (if the interface is a generic).
    /// </remarks>
    // ----------------------------------------------------------------------------------------------
    public TypeEntity Interface
    {
      get
      {
        return InterfaceReference != null && InterfaceReference.ResolutionState == ResolutionState.Resolved
                 ? InterfaceReference.TargetEntity
                 : null;
      }
    }
  }
}
