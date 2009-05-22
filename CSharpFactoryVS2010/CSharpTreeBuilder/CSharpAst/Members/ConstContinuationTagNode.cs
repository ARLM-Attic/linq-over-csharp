// ================================================================================================
// ConstContinuationTagNode.cs
//
// Created: 2009.05.11, by Istvan Novak (DeepDiver)
// ================================================================================================
using CSharpTreeBuilder.CSharpAstBuilder;

namespace CSharpTreeBuilder.Ast
{
  // ================================================================================================
  /// <summary>
  /// This class represents a const member continuation tag.
  /// </summary>
  // ================================================================================================
  public sealed class ConstContinuationTagNode : ConstTagNode, IContinuationTag
  {
    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Initializes a new instance of the <see cref="ConstContinuationTagNode"/> class.
    /// </summary>
    /// <param name="separator">The separator token.</param>
    /// <param name="tag">The tag to obtain proerties form.</param>
    // ----------------------------------------------------------------------------------------------
    public ConstContinuationTagNode(Token separator, ConstTagNode tag)
      : base(separator)
    {
      SeparatorToken = separator;
      IdentifierToken = tag.IdentifierToken;
      EqualToken = tag.EqualToken;
      Expression = tag.Expression;
      Terminate(tag.TerminatingToken);
    }

    // ----------------------------------------------------------------------------------------------

    #region IContinuationTag Members

    /// <summary>
    /// Gets the token separating the continuation tag from the preceding tag.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public Token SeparatorToken { get; internal set; }

    #endregion
  }
}