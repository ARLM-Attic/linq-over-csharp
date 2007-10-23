using CSharpParser.ParserFiles;

namespace CSharpParser.ProjectModel
{
  // ==================================================================================
  /// <summary>
  /// This class defines a language element that simply wraps a token.
  /// </summary>
  /// <remarks>
  /// This kind of language element is used to tie comments to unspecified
  /// language elements.
  /// </remarks>
  // ==================================================================================
  public sealed class TokenElement : LanguageElement
  {
    // --------------------------------------------------------------------------------
    /// <summary>
    /// Creates a new instance of a token element.
    /// </summary>
    /// <param name="token">Token this element is based on.</param>
    /// <param name="parser">Parser instance.</param>
    // --------------------------------------------------------------------------------
    public TokenElement(Token token, CSharpSyntaxParser parser)
      : base(token, parser)
    {
    }
  }
}
