using CSharpParser.ParserFiles;

namespace CSharpParser.ProjectModel
{
  // ==================================================================================
  /// <summary>
  /// This type represents a "foreach" statement.
  /// </summary>
  // ==================================================================================
  public sealed class ForEachStatement : BlockStatement
  {
    #region Private fields
    #endregion

    #region Lifecycle methods

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Creates a new "foreach" statement declaration.
    /// </summary>
    /// <param name="token">Token providing position information.</param>
    // --------------------------------------------------------------------------------
    public ForEachStatement(Token token)
      : base(token)
    {
    }

    #endregion

    #region Public properties
    #endregion
  }
}