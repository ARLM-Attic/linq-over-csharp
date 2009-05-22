// ================================================================================================
// AttributeContinuationNode.cs
//
// Created: 2009.05.11, by Istvan Novak (DeepDiver)
// ================================================================================================
using CSharpTreeBuilder.CSharpAstBuilder;

namespace CSharpTreeBuilder.Ast
{
  // ================================================================================================
  /// <summary>
  /// This class represents an attribute continuation node.
  /// </summary>
  /// <remarks>
  /// Syntax:
  ///   AttributeContinuationNode:
  ///     "," AttributeNode
  /// </remarks>
  // ================================================================================================
  public sealed class AttributeContinuationNode : AttributeNode, IContinuationTag
  {
    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Initializes a new instance of the <see cref="AttributeContinuationNode"/> class.
    /// </summary>
    /// <param name="separator">The separator token.</param>
    /// <param name="baseNode">The base node to take the attribute parameters from.</param>
    // ----------------------------------------------------------------------------------------------
    public AttributeContinuationNode(Token separator, AttributeNode baseNode)
      : base(separator)
    {
      SeparatorToken = separator;
      TypeName = baseNode.TypeName;
      Arguments = baseNode.Arguments;
    }

    // ----------------------------------------------------------------------------------------------

    #region IContinuationTag Members

    /// <summary>
    /// Gets the separator token.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public Token SeparatorToken { get; private set; }

    #endregion
  }
}