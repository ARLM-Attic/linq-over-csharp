// ================================================================================================
// ClassDeclarationNode.cs
//
// Created: 2009.04.07, by Istvan Novak (DeepDiver)
// ================================================================================================
using CSharpTreeBuilder.CSharpAstBuilder;

namespace CSharpTreeBuilder.Ast
{
  // ================================================================================================
  /// <summary>
  /// Represents a class declaration.
  /// </summary>
  // ================================================================================================
  public class ClassDeclarationNode : TypeWithMembersDeclarationNode
  {
    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Initializes a new instance of the <see cref="ClassDeclarationNode"/> class.
    /// </summary>
    /// <param name="start">The start token of the declaration.</param>
    /// <param name="name">The name of the delcaration.</param>
    // ----------------------------------------------------------------------------------------------
    public ClassDeclarationNode(Token start, Token name)
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
      visitor.Visit(this);

      foreach (var decoration in AttributeDecorations)
      {
        decoration.AcceptVisitor(visitor);
      }

      foreach (var typeParameter in TypeParameters)
      {
        typeParameter.AcceptVisitor(visitor);
      }

      foreach (var type in BaseTypes)
      {
        type.AcceptVisitor(visitor);
      }

      foreach (var constraint in TypeParameterConstraints)
      {
        constraint.AcceptVisitor(visitor);
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