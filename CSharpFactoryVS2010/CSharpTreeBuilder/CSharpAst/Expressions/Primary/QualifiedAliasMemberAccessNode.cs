using System;
using CSharpTreeBuilder.CSharpAstBuilder;

namespace CSharpTreeBuilder.Ast
{
  // ================================================================================================
  /// <summary>
  /// This type represents the member access on a qualified alias member.
  /// </summary>
  // ================================================================================================
  public sealed class QualifiedAliasMemberAccessNode : MemberAccessNode
  {
    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Initializes a new instance of the <see cref="QualifiedAliasMemberAccessNode"/> class.
    /// </summary>
    /// <param name="start">Token providing information about the element.</param>
    /// <param name="qualifiedAliasMember">The node representing a qualified alias member.</param>
    // ----------------------------------------------------------------------------------------------
    public QualifiedAliasMemberAccessNode(Token start, QualifiedAliasMemberNode qualifiedAliasMember)
      : base(start)
    {
      if (start == null)
      {
        throw new ArgumentNullException("start");
      }
      if (qualifiedAliasMember == null)
      {
        throw new ArgumentNullException("qualifiedAliasMember");
      }
      
      QualifiedAliasMember = qualifiedAliasMember;
      QualifiedAliasMember.ParentNode = this;

      IdentifierToken = start;
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets the qualified alias member node.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public QualifiedAliasMemberNode QualifiedAliasMember { get; private set; }

    #region Visitor methods

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Accepts a visitor object, according to the Visitor pattern.
    /// </summary>
    /// <param name="visitor">A visitor object</param>
    // ----------------------------------------------------------------------------------------------
    public override void AcceptVisitor(ISyntaxNodeVisitor visitor)
    {
      if (!visitor.Visit(this)) { return; }

      if (QualifiedAliasMember != null)
      {
        QualifiedAliasMember.AcceptVisitor(visitor);
      }

      base.AcceptVisitor(visitor);
    }

    #endregion
  }
}
