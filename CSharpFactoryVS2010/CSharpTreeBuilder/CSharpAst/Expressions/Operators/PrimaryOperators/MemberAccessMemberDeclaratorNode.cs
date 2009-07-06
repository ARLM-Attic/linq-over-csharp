using System;

namespace CSharpTreeBuilder.Ast
{
  // ================================================================================================
  /// <summary>
  /// This class represents a member-access member declarator node.
  /// </summary>
  // ================================================================================================
  public class MemberAccessMemberDeclaratorNode : MemberDeclaratorNode
  {
    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Initializes a new instance of the <see cref="MemberAccessMemberDeclaratorNode"/> class.
    /// </summary>
    /// <param name="memberAccessNode">A member access node.</param>
    // ----------------------------------------------------------------------------------------------
    public MemberAccessMemberDeclaratorNode(MemberAccessOperatorNodeBase memberAccessNode)
      : base(memberAccessNode.StartToken)
    {
      if (memberAccessNode == null)
      {
        throw new ArgumentNullException("memberAccessNode");
      }

      MemberAccess = memberAccessNode;
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets or sets the MemberAccess expression.
    /// </summary>
    /// <value>The member access expression.</value>
    // ----------------------------------------------------------------------------------------------
    public MemberAccessOperatorNodeBase MemberAccess { get; private set; }
  }
}