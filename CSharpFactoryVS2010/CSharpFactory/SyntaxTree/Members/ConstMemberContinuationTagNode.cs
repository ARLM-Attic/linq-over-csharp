// ================================================================================================
// ConstMemberContinuationTagNode.cs
//
// Created: 2009.05.11, by Istvan Novak (DeepDiver)
// ================================================================================================
using CSharpFactory.ParserFiles;

namespace CSharpFactory.Syntax
{
  // ================================================================================================
  /// <summary>
  /// This class represents a const member continuation tag.
  /// </summary>
  // ================================================================================================
  public sealed class ConstMemberContinuationTagNode : ConstMemberTagNode, IContinuationTag
  {
    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Initializes a new instance of the <see cref="ConstMemberContinuationTagNode"/> class.
    /// </summary>
    /// <param name="separator">The separator token.</param>
    /// <param name="tag">The tag to obtain proerties form.</param>
    // ----------------------------------------------------------------------------------------------
    public ConstMemberContinuationTagNode(Token separator, ConstMemberTagNode tag)
      : base(separator)
    {
      SeparatorToken = separator;
      IdentifierToken = tag.IdentifierToken;
      EqualToken = tag.EqualToken;
      Expression = tag.Expression;
      Terminate(tag.TerminatingToken);
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets the token separating the continuation tag from the preceding tag.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public Token SeparatorToken { get; internal set; }
  }
}