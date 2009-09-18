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
    /// <param name="isExplicitlyDefined">True, if the member is explicitly defined, false otherwise.</param>
    /// <param name="accessibility">The declared accessibility of the member. Can be null.</param>
    /// <param name="name">The name of the member.</param>
    // ----------------------------------------------------------------------------------------------
    protected FunctionMemberEntity(bool isExplicitlyDefined, AccessibilityKind? accessibility, string name)
      : base(isExplicitlyDefined, accessibility, name)
    {
      _DeclarationSpace = new LocalVariableDeclarationSpace();
    }
  }
}
