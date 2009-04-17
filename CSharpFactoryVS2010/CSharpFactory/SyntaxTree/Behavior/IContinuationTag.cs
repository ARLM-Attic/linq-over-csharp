// ================================================================================================
// IContinuationTag.cs
//
// Created: 2009.04.14, by Istvan Novak (DeepDiver)
// ================================================================================================
using CSharpFactory.ParserFiles;

namespace CSharpFactory.Syntax
{
  // ================================================================================================
  /// <summary>
  /// This interface defines the behavior of a continuation tag.
  /// </summary>
  // ================================================================================================
  public interface IContinuationTag
  {
    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets the token separating the continuation tag from the preceding tag.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    Token SeparatorToken { get; }
  }
}