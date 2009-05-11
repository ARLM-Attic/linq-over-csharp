// ================================================================================================
// TypeParameterConstraintTagContinuationNode.cs
//
// Created: 2009.05.11, by Istvan Novak (DeepDiver)
// ================================================================================================
using CSharpFactory.ParserFiles;

namespace CSharpFactory.Syntax
{
  // ================================================================================================
  /// <summary>
  /// Defines a continuation for a parameter constraint tag node.
  /// </summary>
  /// <remarks>
  /// Syntax:
  ///   TypeParameterConstraintTagContinuationNode:
  ///     "," TypeParameterConstraintTagNode
  /// </remarks>
  // ================================================================================================
  public sealed class TypeParameterConstraintTagContinuationNode : TypeParameterConstraintTagNode,
                                                                   IContinuationTag
  {
    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Initializes a new instance of the <see cref="TypeParameterConstraintTagContinuationNode"/> class.
    /// </summary>
    /// <param name="separator">The separator token.</param>
    /// <param name="tag">The tag to clone properties from.</param>
    // ----------------------------------------------------------------------------------------------
    public TypeParameterConstraintTagContinuationNode(Token separator, TypeParameterConstraintTagNode tag)
      : base(separator)
    {
      SeparatorToken = separator;
      ConstraintToken = tag.ConstraintToken;
      OpenParenthesis = tag.OpenParenthesis;
      CloseParenthesis = tag.CloseParenthesis;
      TypeName = tag.TypeName;
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets the separator token.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public Token SeparatorToken { get; private set; }   
  }
}