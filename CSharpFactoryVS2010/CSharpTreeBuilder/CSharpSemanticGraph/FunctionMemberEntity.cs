﻿using CSharpTreeBuilder.CSharpSemanticGraphBuilder;

namespace CSharpTreeBuilder.CSharpSemanticGraph
{
  // ================================================================================================
  /// <summary>
  /// This class represents a function members, that is a member that contains executable code.
  /// These are: methods, constructors, properties, indexers, events, operators, and destructors.
  /// </summary>
  // ================================================================================================
  public abstract class FunctionMemberEntity : MemberEntity
  {
    /// <summary>Backing field for DeclarationSpace property.</summary>
    protected DeclarationSpace _DeclarationSpace;

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Initializes a new instance of the <see cref="FunctionMemberEntity"/> class.
    /// </summary>
    /// <param name="isDeclaredInSource">True if the member is explicitly declared in source code, false otherwise.</param>
    /// <param name="accessibility">The declared accessibility of the member. Can be null.</param>
    /// <param name="name">The name of the member.</param>
    // ----------------------------------------------------------------------------------------------
    protected FunctionMemberEntity(bool isDeclaredInSource, AccessibilityKind? accessibility, string name)
      : base(isDeclaredInSource, accessibility, name)
    {
      _DeclarationSpace = new LocalVariableDeclarationSpace();
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets or sets a value indicating whether this member can be overridden.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public virtual bool IsVirtual { get; set; }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets or sets a value indicating whether this member is on override of an inherited member.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public virtual bool IsOverride { get; set; }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets or sets a value indicating whether this member is sealed, meaning that it cannot be overridden.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public virtual bool IsSealed { get; set; }

  }
}
