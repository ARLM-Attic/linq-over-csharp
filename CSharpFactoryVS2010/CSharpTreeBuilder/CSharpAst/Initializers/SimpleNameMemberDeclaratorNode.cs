using System;

namespace CSharpTreeBuilder.Ast
{
  // ================================================================================================
  /// <summary>
  /// This class represents a simple-name member declarator node.
  /// </summary>
  // ================================================================================================
  public class SimpleNameMemberDeclaratorNode : MemberDeclaratorNode
  {
    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Initializes a new instance of the <see cref="SimpleNameMemberDeclaratorNode"/> class.
    /// </summary>
    /// <param name="simpleNameNode">A simple name node.</param>
    // ----------------------------------------------------------------------------------------------
    public SimpleNameMemberDeclaratorNode(SimpleNameNode simpleNameNode)
      : base(simpleNameNode.StartToken)
    {
      if (simpleNameNode == null)
      {
        throw new ArgumentNullException("simpleNameNode");
      }

      SimpleName = simpleNameNode;
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets or sets the SimpleName expression.
    /// </summary>
    /// <value>The simple name expression.</value>
    // ----------------------------------------------------------------------------------------------
    public SimpleNameNode SimpleName { get; private set; }

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

      if (SimpleName != null)
      {
        SimpleName.AcceptVisitor(visitor);
      }
    }

    #endregion
  }
}