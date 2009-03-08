using CSharpFactory.ParserFiles;

namespace CSharpFactory.ProjectModel
{
  // ==================================================================================
  /// <summary>
  /// This type represents an expression list initializer.
  /// </summary>
  // ==================================================================================
  public sealed class ExpressionListInitializer : ListInitializer
  {
    // --------------------------------------------------------------------------------
    /// <summary>
    /// Creates a new array initializer instance.
    /// </summary>
    /// <param name="token">Token providing position information.</param>
    /// <param name="parser">Parser used by this language element.</param>
    // --------------------------------------------------------------------------------
    public ExpressionListInitializer(Token token, CSharpSyntaxParser parser)
      : base(token, parser)
    {
    }
  }
}