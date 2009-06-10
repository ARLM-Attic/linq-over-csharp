// ================================================================================================
// TypeTagContinuationNode.cs
//
// Created: 2009.05.11, by Istvan Novak (DeepDiver)
// ================================================================================================
using System;
using CSharpTreeBuilder.CSharpAstBuilder;

namespace CSharpTreeBuilder.Ast
{
  // ================================================================================================
  /// <summary>
  /// This class represents a type tag as a continuation ofa type tag list.
  /// </summary>
  /// <remarks>
  /// Syntax:
  ///   TypeTagContinuationNode:
  ///     "." TypeTagNode
  /// </remarks>
  // ================================================================================================
  public class TypeTagContinuationNode : TypeTagNode, IContinuationTag
  {
    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Initializes a new instance of the <see cref="TypeTagContinuationNode"/> class.
    /// </summary>
    /// <param name="start">Token providing information about the element.</param>
    /// <param name="identifier">The identifier.</param>
    /// <param name="argumentListNode">The argument list node.</param>
    // ----------------------------------------------------------------------------------------------
    public TypeTagContinuationNode(Token start, Token identifier,
                                   TypeArgumentListNode argumentListNode)
      : base(start, argumentListNode)
    {
      if (identifier == null) throw new ArgumentNullException("identifier");
      SeparatorToken = start;
      IdentifierToken = identifier;
      Terminate(argumentListNode == null ? IdentifierToken : argumentListNode.TerminatingToken);
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
        IdentifierToken,
        Arguments
        );
    }
  }
}