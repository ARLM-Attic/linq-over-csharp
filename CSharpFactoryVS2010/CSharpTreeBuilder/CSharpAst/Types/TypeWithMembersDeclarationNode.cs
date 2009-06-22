// ================================================================================================
// TypeWithMembersDeclarationNode.cs
//
// Created: 2009.04.07, by Istvan Novak (DeepDiver)
// ================================================================================================
using CSharpTreeBuilder.CSharpAstBuilder;

namespace CSharpTreeBuilder.Ast
{
  // ================================================================================================
  /// <summary>
  /// This class represents a common base class for type declarations having members.
  /// </summary>
  // ================================================================================================
  public abstract class TypeWithMembersDeclarationNode : TypeWithBodyDeclarationNode
  {
    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Initializes a new instance of the <see cref="TypeWithMembersDeclarationNode"/> class.
    /// </summary>
    /// <param name="start">The start token of the declaration.</param>
    /// <param name="name">The name of the declaration.</param>
    // ----------------------------------------------------------------------------------------------
    protected TypeWithMembersDeclarationNode(Token start, Token name)
      : base(start, name)
    {
    }

    #region Visitor methods

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Accepts a visitor object, according to the Visitor pattern.
    /// </summary>
    /// <param name="visitor">A visitor object</param>
    // ----------------------------------------------------------------------------------------------
    public override void AcceptVisitor(ISyntaxNodeVisitor visitor)
    {
      foreach (var decoration in AttributeDecorations)
      {
        decoration.AcceptVisitor(visitor);
      }

      foreach (var typeParameter in TypeParameters)
      {
        typeParameter.AcceptVisitor(visitor);
      }

      foreach (var baseType in BaseTypes)
      {
        baseType.AcceptVisitor(visitor);
      }

      foreach (var typeParameterConstraint in TypeParameterConstraints)
      {
        typeParameterConstraint.AcceptVisitor(visitor);
      }

      foreach (var memberDeclaration in MemberDeclarations)
      {
        memberDeclaration.AcceptVisitor(visitor);
      }

      foreach (var nestedType in NestedTypes)
      {
        nestedType.AcceptVisitor(visitor);
      }
    }

    #endregion
  }
}