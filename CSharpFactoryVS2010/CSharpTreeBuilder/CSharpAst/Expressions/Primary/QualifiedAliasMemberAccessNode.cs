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
    /// <param name="simpleNameNode">The node representing the member name.</param>
    // ----------------------------------------------------------------------------------------------
    public QualifiedAliasMemberAccessNode(Token start, QualifiedAliasMemberNode qualifiedAliasMember, SimpleNameNode simpleNameNode)
      : base(start)
    {
      if (qualifiedAliasMember == null)
      {
        throw new ArgumentNullException("qualifiedAliasMember");
      }
      if (simpleNameNode == null)
      {
        throw new ArgumentNullException("simpleNameNode");
      }
      
      QualifiedAliasMember = qualifiedAliasMember;
      MemberName = simpleNameNode;
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets the qualified alias member node.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public QualifiedAliasMemberNode QualifiedAliasMember { get; private set; }

  }
}
