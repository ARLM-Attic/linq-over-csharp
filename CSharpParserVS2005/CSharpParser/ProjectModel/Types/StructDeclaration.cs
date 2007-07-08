using CSharpParser.ParserFiles;

namespace CSharpParser.ProjectModel
{
  // ==================================================================================
  /// <summary>
  /// This type represents a C# class declaration
  /// </summary>
  // ==================================================================================
  public sealed class StructDeclaration: ClasslikeTypeDeclaration
  {
    #region Private fields

    #endregion

    #region Lifecycle methods

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Creates a new class declaration.
    /// </summary>
    /// <param name="token">Token providing position information.</param>
    /// <param name="parser">Parser instance</param>
    // --------------------------------------------------------------------------------
    public StructDeclaration(Token token, CSharpSyntaxParser parser)
      : base(token, parser)
    {
    }

    #endregion

    #region Public properties

    #endregion
  }
}