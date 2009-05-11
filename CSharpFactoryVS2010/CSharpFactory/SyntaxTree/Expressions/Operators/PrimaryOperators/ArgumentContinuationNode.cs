// ================================================================================================
// ArgumentContinuationNode.cs
//
// Created: 2009.05.11, by Istvan Novak (DeepDiver)
// ================================================================================================
using CSharpFactory.ParserFiles;

namespace CSharpFactory.Syntax
{
  // ================================================================================================
  /// <summary>
  /// This class represents an argument continuation in the actual parameter list.
  /// </summary>
  // ================================================================================================
  public sealed class ArgumentContinuationNode : ArgumentNode, IContinuationTag
  {
    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Initializes a new instance of the <see cref="ArgumentContinuationNode"/> class.
    /// </summary>
    /// <param name="start">The start token.</param>
    // ----------------------------------------------------------------------------------------------
    public ArgumentContinuationNode(Token start)
      : base(start)
    {
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets or sets the separator token.
    /// </summary>
    /// <value>The separator token.</value>
    // ----------------------------------------------------------------------------------------------
    public Token SeparatorToken { get; internal set; }
  }
}