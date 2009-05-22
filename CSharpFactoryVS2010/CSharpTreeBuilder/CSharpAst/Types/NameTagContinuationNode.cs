// ================================================================================================
// NameTagContinuationNode.cs
//
// Created: 2009.05.11, by Istvan Novak (DeepDiver)
// ================================================================================================
using System;
using CSharpTreeBuilder.CSharpAstBuilder;

namespace CSharpTreeBuilder.Ast
{
  // ================================================================================================
  /// <summary>
  /// This class represents a name tag as a continuation of a name tag list.
  /// </summary>
  /// <remarks>
  /// Syntax:
  ///   NameTagContinuationNode:
  ///     "." NameTagNode
  /// </remarks>
  // ================================================================================================
  public class NameTagContinuationNode : NameTagNode, IContinuationTag
  {
    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Initializes a new instance of the <see cref="TypeTagContinuationNode"/> class.
    /// </summary>
    /// <param name="start">Token providing information about the element.</param>
    /// <param name="identifier">The identifier.</param>
    // ----------------------------------------------------------------------------------------------
    public NameTagContinuationNode(Token start, Token identifier)
      : base(start)
    {
      if (identifier == null) throw new ArgumentNullException("identifier");
      SeparatorToken = start;
      IdentifierToken = identifier;
    }

    // ----------------------------------------------------------------------------------------------

    #region IContinuationTag Members

    /// <summary>
    /// Gets or sets the separator token.
    /// </summary>
    /// <value>The separator token.</value>
    // ----------------------------------------------------------------------------------------------
    public Token SeparatorToken { get; private set; }

    #endregion

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets the output segment representing this syntax node.
    /// </summary>
    /// <returns>
    /// The OutputSegment instance describing this syntax node, or null; if the node has no output.
    /// </returns>
    // ----------------------------------------------------------------------------------------------
    public override OutputSegment GetOutputSegment()
    {
      return new OutputSegment(
        SeparatorToken,
        IdentifierToken
        );
    }
  }
}