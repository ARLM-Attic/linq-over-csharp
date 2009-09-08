using System;

namespace CSharpTreeBuilder.Ast
{
  // ================================================================================================
  /// <summary>
  /// This class represents a base-member-access member declarator node.
  /// </summary>
  // ================================================================================================
  public class BaseMemberAccessMemberDeclaratorNode : MemberDeclaratorNode
  {
    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Initializes a new instance of the <see cref="BaseMemberAccessMemberDeclaratorNode"/> class.
    /// </summary>
    /// <param name="baseMemberAccessNode">A base member access node.</param>
    // ----------------------------------------------------------------------------------------------
    public BaseMemberAccessMemberDeclaratorNode(BaseMemberAccessNode baseMemberAccessNode)
      : base(baseMemberAccessNode.StartToken)
    {
      if (baseMemberAccessNode == null)
      {
        throw new ArgumentNullException("baseMemberAccessNode");
      }

      BaseMemberAccess = baseMemberAccessNode;
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets or sets the BaseMemberAccess expression.
    /// </summary>
    /// <value>The base member access expression.</value>
    // ----------------------------------------------------------------------------------------------
    public BaseMemberAccessNode BaseMemberAccess { get; private set; }

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

      if (BaseMemberAccess!=null)
      {
        BaseMemberAccess.AcceptVisitor(visitor);
      }
    }

    #endregion
  }
}