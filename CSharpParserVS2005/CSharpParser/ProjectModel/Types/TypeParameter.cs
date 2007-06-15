using CSharpParser.ParserFiles;

namespace CSharpParser.ProjectModel
{
  // ==================================================================================
  /// <summary>
  /// This class represents a type parameter used in generic constructions.
  /// </summary>
  // ==================================================================================
  public sealed class TypeParameter : AttributedElement
  {
    // --------------------------------------------------------------------------------
    /// <summary>
    /// Creates a type parameter according to the info provided by the
    /// specified token.
    /// </summary>
    /// <param name="token">Token providing information about the element.</param>
    // --------------------------------------------------------------------------------
    public TypeParameter(Token token)
      : base(token)
    {
    }
  }
}
