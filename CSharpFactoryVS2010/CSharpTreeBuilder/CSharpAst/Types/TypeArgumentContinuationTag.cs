// ================================================================================================
// TypeArgumentContinuationTag.cs
//
// Created: 2009.05.11, by Istvan Novak (DeepDiver)
// ================================================================================================
using CSharpTreeBuilder.CSharpAstBuilder;

namespace CSharpTreeBuilder.Ast
{
  // ================================================================================================
  /// <summary>
  /// Describes a type argument continuation.
  /// </summary>
  // ================================================================================================
  public sealed class TypeArgumentContinuationTag : TypeArgumentTag, IContinuationTag
  {
    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Initializes a new instance of the <see cref="TypeArgumentContinuationTag"/> class.
    /// </summary>
    /// <param name="start">Token providing information about the element.</param>
    // ----------------------------------------------------------------------------------------------
    public TypeArgumentContinuationTag(Token start)
      : base(start)
    {
    }

    // ----------------------------------------------------------------------------------------------

    #region IContinuationTag Members

    /// <summary>
    /// Gets the token separating the continuation tag from the preceding tag.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public Token SeparatorToken { get; internal set; }

    #endregion
  }
}