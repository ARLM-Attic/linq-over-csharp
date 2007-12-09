using CSharpParser.ParserFiles;

namespace CSharpParser.ProjectModel
{
  // ==================================================================================
  /// <summary>
  /// This type represents an member declarator list initializer.
  /// </summary>
  // ==================================================================================
  public sealed class MemberDeclaratorList : ListInitializer
  {
    // --------------------------------------------------------------------------------
    /// <summary>
    /// Creates a new member declarator list instance.
    /// </summary>
    /// <param name="token">Token providing position information.</param>
    /// <param name="parser">Parser used by this language element.</param>
    // --------------------------------------------------------------------------------
    public MemberDeclaratorList(Token token, CSharpSyntaxParser parser)
      : base(token, parser)
    {
    }
  }
}