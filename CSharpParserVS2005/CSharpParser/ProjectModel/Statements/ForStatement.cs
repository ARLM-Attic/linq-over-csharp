using CSharpParser.ParserFiles;

namespace CSharpParser.ProjectModel
{
  // ==================================================================================
  /// <summary>
  /// This type represents a "for" statement.
  /// </summary>
  // ==================================================================================
  public sealed class ForStatement : BlockStatement
  {
    #region Private fields
    #endregion

    #region Lifecycle methods

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Creates a new "for" statement declaration.
    /// </summary>
    /// <param name="token">Token providing position information.</param>
    // --------------------------------------------------------------------------------
    public ForStatement(Token token)
      : base(token)
    {
    }

    #endregion

    #region Public properties
    #endregion
  }
}