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

    private TypeArgumentCollection _TypeArguments = new TypeArgumentCollection();
    
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
    public TypeArgumentCollection TypeArguments
    {
      get { return _TypeArguments; }
    }
    
#endregion
  }
}