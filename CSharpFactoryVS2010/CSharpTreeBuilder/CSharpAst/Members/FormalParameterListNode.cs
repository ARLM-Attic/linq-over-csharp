// ================================================================================================
// FormalParameterListNode.cs
//
// Created: 2009.05.13, by Istvan Novak (DeepDiver)
// ================================================================================================
using CSharpTreeBuilder.CSharpAstBuilder;

namespace CSharpTreeBuilder.Ast
{
  // ================================================================================================
  /// <summary>
  /// This class represents a formal parameter list.
  /// </summary>
  /// <remarks>
  /// The opening bracket is represented by the starting token, the closing bracket by the 
  /// terminating token.
  /// </remarks>
  // ================================================================================================
  public class FormalParameterListNode : SyntaxNode<TypeOrMemberDeclarationNode>
  {
    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Initializes a new instance of the <see cref="FormalParameterListNode"/> class.
    /// </summary>
    /// <param name="start">Token providing information about the element.</param>
    // ----------------------------------------------------------------------------------------------
    public FormalParameterListNode(Token start)
      : base(start)
    {
      Items = new FormalParameterNodeCollection {ParentNode = this};
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets collection of formal parameter items.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public FormalParameterNodeCollection Items { get; private set; }

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

      foreach (var item in Items)
      {
        item.AcceptVisitor(visitor);
      }
    }

    #endregion

  }
}