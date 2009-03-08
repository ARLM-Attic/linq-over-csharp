using CSharpFactory.ParserFiles;

namespace CSharpFactory.ProjectModel
{
  // ==================================================================================
  /// <summary>
  /// This abstract class represents a reference operator expression.
  /// </summary>
  // ==================================================================================
  public sealed class ReferenceOperator : UnaryOperator
  {
    // --------------------------------------------------------------------------------
    /// <summary>
    /// Creates a new operator expression.
    /// </summary>
    /// <param name="token">Token providing position information.</param>
    /// <param name="parser">Parser instance creating this element.</param>
    // --------------------------------------------------------------------------------
    public ReferenceOperator(Token token, CSharpSyntaxParser parser) : base(token, parser)
    {
    }

  }
}