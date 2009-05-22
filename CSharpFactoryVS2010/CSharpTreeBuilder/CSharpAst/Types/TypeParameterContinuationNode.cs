// ================================================================================================
// TypeParameterContinuationNode.cs
//
// Created: 2009.05.11, by Istvan Novak (DeepDiver)
// ================================================================================================
using CSharpTreeBuilder.CSharpAstBuilder;

namespace CSharpTreeBuilder.Ast
{
  // ================================================================================================
  /// <summary>
  /// This type represents a type parameter continuation node.
  /// </summary>
  // ================================================================================================
  public class TypeParameterContinuationNode : TypeParameterNode, IContinuationTag
  {
    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Initializes a new instance of the <see cref="TypeParameterContinuationNode"/> class.
    /// </summary>
    /// <param name="start">Token providing information about the element.</param>
    /// <param name="identifier">The identifier.</param>
    /// <param name="attrNodes">Attributes of the type parameter.</param>
    // ----------------------------------------------------------------------------------------------
    public TypeParameterContinuationNode(Token start, Token identifier,
                                         AttributeDecorationNodeCollection attrNodes)
      : base(start, attrNodes)
    {
      SeparatorToken = start;
      IdentifierToken = identifier;
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