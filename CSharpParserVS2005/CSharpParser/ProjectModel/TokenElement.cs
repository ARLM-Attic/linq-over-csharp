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
    public TokenElement(Token token, CSharpSyntaxParser parser) : base(token, parser)
    {
    }
  }
}
