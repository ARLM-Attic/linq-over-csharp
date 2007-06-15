using CSharpParser.ParserFiles;

namespace CSharpParser.ProjectModel
{
  // ==================================================================================
  /// <summary>
  /// This type represents a destructor declaration.
  /// </summary>
  // ==================================================================================
  public sealed class DestructorDeclaration : MethodDeclaration
  {
    #region Private fields
    #endregion

    #region Lifecycle methods

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Creates a new destructor declaration.
    /// </summary>
    /// <param name="token">Token providing position information.</param>
    // --------------------------------------------------------------------------------
    public DestructorDeclaration(Token token)
      : base(token)
    {
    }

    #endregion

    #region Public properties
    #endregion
  }
}