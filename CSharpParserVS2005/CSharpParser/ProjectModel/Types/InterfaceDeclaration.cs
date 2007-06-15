using CSharpParser.ParserFiles;

namespace CSharpParser.ProjectModel
{
  // ==================================================================================
  /// <summary>
  /// This type represents a C# interface declaration
  /// </summary>
  // ==================================================================================
  public sealed class InterfaceDeclaration: ClasslikeTypeDeclaration
  {
    #region Private fields

    #endregion

    #region Lifecycle methods

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Creates a new class declaration.
    /// </summary>
    /// <param name="token">Token providing position information.</param>
    // --------------------------------------------------------------------------------
    public InterfaceDeclaration(Token token)
      : base(token)
    {
    }

    #endregion

    #region Public properties

    #endregion
  }
}