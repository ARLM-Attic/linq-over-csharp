// ================================================================================================
// AttributeArgumentContinuationNode.cs
//
// Created: 2009.05.11, by Istvan Novak (DeepDiver)
// ================================================================================================
using CSharpTreeBuilder.CSharpAstBuilder;

namespace CSharpFactory.Syntax
{
  // ================================================================================================
  /// <summary>
  /// This class represents the continuation of an attribute argument.
  /// </summary>
  /// <remarks>
  /// Syntax:
  ///   AttributeArgumentContinuationNode:
  ///     "," AttributeArgumentNode
  /// </remarks>
  // ================================================================================================
  public sealed class AttributeArgumentContinuationNode : AttributeArgumentNode, IContinuationTag
  {
    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Initializes a new instance of the <see cref="AttributeArgumentContinuationNode"/> class.
    /// </summary>
    /// <param name="separator">The separator token.</param>
    /// <param name="identifier">The identifier token.</param>
    /// <param name="equal">The equal token.</param>
    /// <param name="expression">The expression node.</param>
    // ----------------------------------------------------------------------------------------------
    public AttributeArgumentContinuationNode(Token separator, Token identifier, Token equal, 
                                             ExpressionNode expression) : base(identifier, equal, expression)
    {
      StartToken = separator;
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets the separator token.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public Token SeparatorToken { get; internal set; }
  }
}