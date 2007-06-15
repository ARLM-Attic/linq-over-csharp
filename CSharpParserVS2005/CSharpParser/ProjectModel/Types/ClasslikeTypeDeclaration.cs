using CSharpParser.ParserFiles;

namespace CSharpParser.ProjectModel
{
  // ==================================================================================
  /// <summary>
  /// This abstract type represents a class like type declaration that can be used 
  /// as "partial" definition (class, struct or interface).
  /// </summary>
  // ==================================================================================
  public abstract class ClasslikeTypeDeclaration : TypeDeclaration
  {
    private bool _IsPartial;

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Creates a new classlike type declaration.
    /// </summary>
    /// <param name="token">Token providing position information.</param>
    // --------------------------------------------------------------------------------
    protected ClasslikeTypeDeclaration(Token token)
      : base(token)
    {
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets or sets the flag indicating if this type declaration is partial or not.
    /// </summary>
    // --------------------------------------------------------------------------------
    public bool IsPartial
    {
      get { return _IsPartial; }
      set { _IsPartial = value; }
    }
  }
}
