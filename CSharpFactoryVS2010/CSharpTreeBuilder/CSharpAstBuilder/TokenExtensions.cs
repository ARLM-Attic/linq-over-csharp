// ================================================================================================
// TokenExtensions.cs
//
// Created: 2009.06.10, by Istvan Novak (DeepDiver)
// ================================================================================================
namespace CSharpTreeBuilder.CSharpAstBuilder
{
  // ================================================================================================
  /// <summary>
  /// This class contains extension methods for the token class.
  /// </summary>
  // ================================================================================================
  public static class TokenExtensions
  {
    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Determines whether the specified token is comma.
    /// </summary>
    /// <param name="token">The token.</param>
    /// <returns>
    /// 	<c>true</c> if the specified token is comma; otherwise, <c>false</c>.
    /// </returns>
    // ----------------------------------------------------------------------------------------------
    public static bool IsComma(this Token token)
    {
      return token != null && token.Value == ",";
    }
  }
}