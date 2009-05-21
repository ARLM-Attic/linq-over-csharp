// ================================================================================================
// EnumValueContinuationNode.cs
//
// Created: 2009.05.15, by Istvan Novak (DeepDiver)
// ================================================================================================
using CSharpTreeBuilder.CSharpAstBuilder;

namespace CSharpFactory.Syntax
{
  // ================================================================================================
  /// <summary>
  /// This class represents a continuation of an enumeration value.
  /// </summary>
  // ================================================================================================
  public class EnumValueContinuationNode: EnumValueNode, IContinuationTag
  {
    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Initializes a new instance of the <see cref="EnumValueContinuationNode"/> class.
    /// </summary>
    /// <param name="separator">The separator token.</param>
    /// <param name="node">The enumeration value node.</param>
    // ----------------------------------------------------------------------------------------------
    public EnumValueContinuationNode(Token separator, EnumValueNode node)
      : base(separator)
    {
      SeparatorToken = separator;
      IdentifierToken = node.IdentifierToken;
      EqualToken = node.EqualToken;
      Expression = node.Expression;
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets the token separating the continuation tag from the preceding tag.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public Token SeparatorToken { get; internal set; }
  }
}