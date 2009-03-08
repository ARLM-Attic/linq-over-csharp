using CSharpFactory.ParserFiles;

namespace CSharpFactory.ProjectModel
{
  // ==================================================================================
  /// <summary>
  /// This type represents a collection initializer.
  /// </summary>
  // ==================================================================================
  public sealed class CollectionInitializer : ListInitializer
  {
    // --------------------------------------------------------------------------------
    /// <summary>
    /// Creates a new array initializer instance.
    /// </summary>
    /// <param name="token">Token providing position information.</param>
    /// <param name="parser">Parser used by this language element.</param>
    // --------------------------------------------------------------------------------
    public CollectionInitializer(Token token, CSharpSyntaxParser parser)
      : base(token, parser)
    {
    }
  }
}