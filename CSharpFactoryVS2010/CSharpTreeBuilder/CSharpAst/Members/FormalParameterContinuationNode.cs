// ================================================================================================
// FormalParameterContinuationNode.cs
//
// Created: 2009.05.13, by Istvan Novak (DeepDiver)
// ================================================================================================
using CSharpTreeBuilder.CSharpAstBuilder;

namespace CSharpTreeBuilder.Ast
{
  // ================================================================================================
  /// <summary>
  /// This class represents a continuation of a FormalParameterNode.
  /// </summary>
  // ================================================================================================
  public class FormalParameterContinuationNode : FormalParameterNode, IContinuationTag
  {
    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Initializes a new instance of the <see cref="FormalParameterContinuationNode"/> class.
    /// </summary>
    /// <param name="separator">The separator token.</param>
    /// <param name="parNode">The parameter node.</param>
    // ----------------------------------------------------------------------------------------------
    public FormalParameterContinuationNode(Token separator, FormalParameterNode parNode) :
      base(separator)
    {
      IdentifierToken = parNode.IdentifierToken;
      Modifier = parNode.Modifier;
      TypeName = parNode.TypeName;
      SeparatorToken = separator;
    }

    // ----------------------------------------------------------------------------------------------

    #region IContinuationTag Members

    /// <summary>
    /// Gets the token separating the continuation tag from the preceding tag.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public Token SeparatorToken { get; private set; }

    #endregion
  }
}