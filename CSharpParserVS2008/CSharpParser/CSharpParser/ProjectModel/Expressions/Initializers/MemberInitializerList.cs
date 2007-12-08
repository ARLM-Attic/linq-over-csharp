using CSharpParser.ParserFiles;

namespace CSharpParser.ProjectModel
{
  // ==================================================================================
  /// <summary>
  /// This type represents a member initializer list.
  /// </summary>
  // ==================================================================================
  public sealed class MemberInitializerList : ListInitializer
  {
    // --------------------------------------------------------------------------------
    /// <summary>
    /// Creates a new array initializer instance.
    /// </summary>
    /// <param name="token">Token providing position information.</param>
    /// <param name="parser">Parser used by this language element.</param>
    // --------------------------------------------------------------------------------
    public MemberInitializerList(Token token, CSharpSyntaxParser parser)
      : base(token, parser)
    {
    }
  }
}