// ================================================================================================
// FieldContinuationTagNode.cs
//
// Created: 2009.05.16, by Istvan Novak (DeepDiver)
// ================================================================================================
using CSharpFactory.ParserFiles;

namespace CSharpTreeBuilder.Ast
{
  // ================================================================================================
  /// <summary>
  /// This class represents a continuation of a field member tag.
  /// </summary>
  // ================================================================================================
  public class FieldContinuationTagNode: FieldTagNode, IContinuationTag
  {
    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Initializes a new instance of the <see cref="FieldContinuationTagNode"/> class.
    /// </summary>
    /// <param name="separatorToken">The separator token.</param>
    /// <param name="node">The node.</param>
    // ----------------------------------------------------------------------------------------------
    public FieldContinuationTagNode(Token separatorToken, FieldTagNode node) : 
      base(node.StartToken)
    {
      IdentifierToken = node.IdentifierToken;
      EqualToken = node.EqualToken;
      Initializer = node.Initializer;
      SeparatorToken = separatorToken;
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets or sets the separator token.
    /// </summary>
    /// <value>The separator token.</value>
    // ----------------------------------------------------------------------------------------------
    public Token SeparatorToken { get; private set; }
    
  }
}