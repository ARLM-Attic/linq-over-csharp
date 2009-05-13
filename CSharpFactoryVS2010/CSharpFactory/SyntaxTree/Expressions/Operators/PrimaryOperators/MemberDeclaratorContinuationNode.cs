// ================================================================================================
// MemberDeclaratorContinuationNode.cs
//
// Created: 2009.05.13, by Istvan Novak (DeepDiver)
// ================================================================================================
using CSharpFactory.ParserFiles;

namespace CSharpFactory.Syntax
{
  // ================================================================================================
  /// <summary>
  /// This class represents a continuation of a member declarator.
  /// </summary>
  // ================================================================================================
  public class MemberDeclaratorContinuationNode : MemberDeclaratorNode, IContinuationTag
  {
    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Initializes a new instance of the <see cref="MemberDeclaratorContinuationNode"/> class.
    /// </summary>
    /// <param name="separator">Separator token.</param>
    /// <param name="node">Node representing the continuation.</param>
    // ----------------------------------------------------------------------------------------------
    public MemberDeclaratorContinuationNode(Token separator, MemberDeclaratorNode node)
      : base(separator)
    {
      Kind = node.Kind;
      IdentifierToken = node.IdentifierToken;
      EqualToken = node.EqualToken;
      Expression = node.Expression;
      DotSeparator = node.DotSeparator;
      TypeName = node.TypeName;
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets the separator token.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public Token SeparatorToken { get; private set; }  
  }
}