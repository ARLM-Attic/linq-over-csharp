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
    #region State

    /// <summary>Gets the reference to the type of the member.</summary>
    public Resolver<TypeEntity> TypeReference { get; private set; }

    /// <summary>
    /// Gets the reference to the interface entity whose member is explicitly implemented.
    /// Null if this member is not an explicitly implemented interface member.
    /// </summary>
    public Resolver<TypeEntity> InterfaceReference { get; private set; }

    #endregion

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
      Resolver<TypeEntity> typeReference,
      Resolver<TypeEntity> interfaceReference,
      string name)
      :
      base(isDeclaredInSource, accessibility, name)
    {
      TypeReference = typeReference;
      InterfaceReference = interfaceReference;
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Initializes a new instance of the <see cref="FunctionMemberWithAccessorsEntity"/> class 
    /// by constructing it from a template instance.
    /// </summary>
    /// <param name="template">The template for the new instance.</param>
    /// <param name="typeParameterMap">The type parameter map of the new instance.</param>
    // ----------------------------------------------------------------------------------------------
    protected FunctionMemberWithAccessorsEntity(FunctionMemberWithAccessorsEntity template, TypeParameterMap typeParameterMap)
      : base(template, typeParameterMap)
    {
      TypeReference = template.TypeReference;
      InterfaceReference = template.InterfaceReference;
    }
    
    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets the type of the member.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public TypeEntity Type
    {
      get
      {
        return TypeReference != null && TypeReference.Target != null
          ? TypeReference.Target.GetMappedType(TypeParameterMap)
          : null;
      }
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets the accessors of this function member.
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
    /// Gets the interface entity whose member is explicitly implemented.
    /// Null if this member is not an explicitly implemented interface member 
    /// or if the reference to the interface entity is not yet resolved.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public InterfaceEntity Interface
    {
      get 
      {
        return InterfaceReference != null && InterfaceReference.Target != null
          ? InterfaceReference.Target.GetMappedType(TypeParameterMap) as InterfaceEntity
          : null;
      }
    }

    #region Visitor methods

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Accepts a visitor object, according to the Visitor pattern.
    /// </summary>
    /// <param name="visitor">A visitor object</param>
    // ----------------------------------------------------------------------------------------------
    public override void AcceptVisitor(SemanticGraphVisitor visitor)
    {
      visitor.Visit(this);
      base.AcceptVisitor(visitor);
    }

    #endregion
  }
}
