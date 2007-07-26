using CSharpParser.ParserFiles;

namespace CSharpParser.ProjectModel
{
  // ==================================================================================
  /// <summary>
  /// This type represents a destructor declaration.
  /// </summary>
  // ==================================================================================
  public sealed class FinalizerDeclaration : MethodDeclaration
  {
    #region Lifecycle methods

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Creates a new destructor declaration.
    /// </summary>
    /// <param name="token">Token providing position information.</param>
    // --------------------------------------------------------------------------------
    public FinalizerDeclaration(Token token)
      : base(token)
    {
    }

    #endregion

    #region Public properties

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets the name of the finalizer.
    /// </summary>
    // --------------------------------------------------------------------------------
    public override string Name
    {
      get
      {
        return "~" + base.Name;
      }
    }

    #endregion
  }
}