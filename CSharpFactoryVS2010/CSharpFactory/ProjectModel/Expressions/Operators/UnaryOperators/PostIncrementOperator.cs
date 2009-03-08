using CSharpFactory.ParserFiles;
using CSharpSyntaxParser=CSharpFactory.ParserFiles.CSharpSyntaxParser;

namespace CSharpFactory.ProjectModel
{
  // ==================================================================================
  /// <summary>
  /// This abstract class represents a postincrement operator expression.
  /// </summary>
  // ==================================================================================
  public sealed class PostIncrementOperator : UnaryOperator
  {
    // --------------------------------------------------------------------------------
    /// <summary>
    /// Creates a new operator expression.
    /// </summary>
    /// <param name="token">Token providing position information.</param>
    /// <param name="parser">Parser instance creating this element.</param>
    // --------------------------------------------------------------------------------
    public PostIncrementOperator(ParserFiles.Token token, CSharpSyntaxParser parser) : base(token, parser)
    {
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Creates a new operator using the specified operand.
    /// </summary>
    /// <param name="token">Token providing position information.</param>
    /// <param name="parser">Parser instance creating this element.</param>
    /// <param name="operand">LeftOperand of the operator</param>
    // --------------------------------------------------------------------------------
    public PostIncrementOperator(Token token, CSharpSyntaxParser parser, Expression operand)
      : base(token, parser, operand)
    {
    } 
  }
}