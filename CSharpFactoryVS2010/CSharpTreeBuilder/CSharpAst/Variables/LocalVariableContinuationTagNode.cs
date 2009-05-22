// ================================================================================================
// LocalVariableContinuationTagNode.cs
//
// Created: 2009.05.11, by Istvan Novak (DeepDiver)
// ================================================================================================
using CSharpTreeBuilder.CSharpAstBuilder;

namespace CSharpTreeBuilder.Ast
{
  // ================================================================================================
  /// <summary>
  /// This class represents a continuation tag in a local variable declaration.
  /// </summary>
  // ================================================================================================
  public class LocalVariableContinuationTagNode : LocalVariableTagNode, IContinuationTag
  {
    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Initializes a new instance of the <see cref="LocalVariableContinuationTagNode"/> class.
    /// </summary>
    /// <param name="separator">The separator token.</param>
    /// <param name="node">Continuation tag node.</param>
    // ----------------------------------------------------------------------------------------------
    public LocalVariableContinuationTagNode(Token separator, LocalVariableTagNode node)
      : base(separator)
    {
      IdentifierToken = node.IdentifierToken;
      Initializer = node.Initializer;
      SeparatorToken = separator;
    }

    // ----------------------------------------------------------------------------------------------

    #region IContinuationTag Members

    /// <summary>
    /// Gets the token separating the continuation tag from the preceding tag.
    /// </summary>
    /// <value></value>
    // ----------------------------------------------------------------------------------------------
    public Token SeparatorToken { get; internal set; }

    #endregion
  }
}