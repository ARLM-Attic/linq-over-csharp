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
  }
}