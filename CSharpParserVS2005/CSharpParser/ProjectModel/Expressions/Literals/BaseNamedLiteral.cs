using CSharpParser.ParserFiles;

namespace CSharpParser.ProjectModel
{
  // ==================================================================================
  /// <summary>
  /// This abstract type represents a "base" literal.
  /// </summary>
  // ==================================================================================
  public sealed class BaseNamedLiteral : BaseLiteral
  {
    #region Private fields

    private TypeReferenceCollection _TypeArguments = new TypeReferenceCollection();
    
    #endregion

    #region Lifecycle methods

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Creates a new "base" literal.
    /// </summary>
    /// <param name="token">Token providing position information.</param>
    // --------------------------------------------------------------------------------
    public BaseNamedLiteral(Token token)
      : base(token)
    {
    }

    #endregion

    #region Public properties

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets or sets the type arguments of the primitive method.
    /// </summary>
    // --------------------------------------------------------------------------------
    public TypeReferenceCollection TypeArguments
    {
      get { return _TypeArguments; }
    }
    
#endregion
  }
}