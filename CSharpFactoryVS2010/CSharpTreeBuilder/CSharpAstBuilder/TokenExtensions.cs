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
    public static bool IsComma(this Token token)
    {
      return token != null && token.Value == ",";
    }
  }
}