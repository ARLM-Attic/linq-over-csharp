﻿using CSharpTreeBuilder.CSharpSemanticGraphBuilder;

namespace CSharpTreeBuilder.CSharpSemanticGraph
{
  // ================================================================================================
  /// <summary>
  /// This class represents a class entity in the semantic graph.
  /// </summary>
  // ================================================================================================
  public sealed class ClassEntity : ChildTypeCapableTypeEntity, ICanBePartial
  {
    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Initializes a new instance of the <see cref="ClassEntity"/> class.
    /// </summary>
    /// <param name="accessibility">The declared accessibility of the member. Can be null.</param>
    /// <param name="name">The name of the entity.</param>
    /// <param name="isPartial">A value indicating whether this type was declared as partial.</param>
    // ----------------------------------------------------------------------------------------------
    public ClassEntity(AccessibilityKind? accessibility, string name, bool isPartial)
      : base(accessibility, name)
    {
      IsPartial = isPartial;
      IsAbstract = false;
      IsSealed = false;
      IsStatic = false;
    }
    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Initializes a new instance of the <see cref="ClassEntity"/> class with no partial.
    /// </summary>
    /// <param name="accessibility">The declared accessibility of the member. Can be null.</param>
    /// <param name="name">The name of the entity.</param>
    // ----------------------------------------------------------------------------------------------
    public ClassEntity(AccessibilityKind? accessibility, string name)
      : this(accessibility, name, false)
    {
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets a value indicating whether this entity is declared as partial. 
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public bool IsPartial { get; private set; }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets a value indicating whether this class is static (contains only static members).
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public bool IsStatic { get; set; }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets a value indicating whether this class is abstract (cannot be instantiated directly).
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public bool IsAbstract { get; set; }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets a value indicating whether this class is sealed (no derivation allowed from the class).
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public bool IsSealed { get; set; }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Adds a member. 
    /// Also sets the member's parent property, and defines member's name in the declaration space.
    /// </summary>
    /// <param name="memberEntity">The member entity.</param>
    // ----------------------------------------------------------------------------------------------
    public override void AddMember(IMemberEntity memberEntity)
    {
      // Member name and type name cannot be the same.
      if ( memberEntity.Name == Name)
      {
        throw new TypeNameMemberNameConflictException(Name);
      }

      base.AddMember(memberEntity);
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets a value indicating whether this type is a reference type.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public override bool IsReferenceType
    {
      get { return true; }
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets a value indicating whether this type is a class type.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public override bool IsClassType
    {
      get { return true; }
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
      if (!visitor.Visit(this)) { return; }

      base.AcceptVisitor(visitor);
    }

    #endregion
  }
}