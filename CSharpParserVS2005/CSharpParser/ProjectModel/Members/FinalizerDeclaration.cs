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
    /// <param name="declaringType">Type declaring this member.</param>
    // --------------------------------------------------------------------------------
    public FinalizerDeclaration(Token token, TypeDeclaration declaringType)
      : base(token, declaringType)
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
      get { return "~" + base.Name; }
    }

    #endregion
  }
}