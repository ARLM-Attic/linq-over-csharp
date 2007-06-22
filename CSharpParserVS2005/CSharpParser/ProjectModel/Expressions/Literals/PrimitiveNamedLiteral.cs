using CSharpParser.ParserFiles;

namespace CSharpParser.ProjectModel
{
  // ==================================================================================
  /// <summary>
  /// This abstract type represents a primitive method literal.
  /// </summary>
  // ==================================================================================
  public sealed class PrimitiveNamedLiteral : Literal
  {
    #region Private fields

    private TypeReference _Type;
    private TypeReferenceCollection _TypeArguments = new TypeReferenceCollection();

    #endregion

    #region Lifecycle methods

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Creates a new primitive method literal.
    /// </summary>
    /// <param name="token">Token providing position information.</param>
    // --------------------------------------------------------------------------------
    public PrimitiveNamedLiteral(Token token)
      : base(token)
    {
    }

    #endregion

    #region Public properties

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets or sets the type of the primitive method.
    /// </summary>
    // --------------------------------------------------------------------------------
    public TypeReference Type
    {
      get { return _Type; }
      set { _Type = value; }
    }

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