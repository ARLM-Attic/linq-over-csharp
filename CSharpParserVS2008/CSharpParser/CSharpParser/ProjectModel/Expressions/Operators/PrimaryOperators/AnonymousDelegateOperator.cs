using System.Collections.Generic;
using CSharpParser.ParserFiles;

namespace CSharpParser.ProjectModel
{
  // ==================================================================================
  /// <summary>
  /// This abstract class represents an anonymous delegate operator expression.
  /// </summary>
  // ==================================================================================
  public sealed class AnonymousDelegateOperator : AnonymousFunction
  {
    #region Lifecycle methods

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Creates a new operator expression.
    /// </summary>
    /// <param name="token">Token providing position information.</param>
    /// <param name="parser">Parser instance creating this element.</param>
    // --------------------------------------------------------------------------------
    public AnonymousDelegateOperator(Token token, CSharpSyntaxParser parser)
      : base(token, parser)
    {
    }

    #endregion
  }
}