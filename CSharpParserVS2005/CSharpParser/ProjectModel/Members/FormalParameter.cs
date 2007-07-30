using CSharpParser.ParserFiles;
using CSharpParser.Semantics;

namespace CSharpParser.ProjectModel
{
  // ==================================================================================
  /// <summary>
  /// This enumeration defines the types of formal parameters.
  /// </summary>
  // ==================================================================================
  public enum FormalParameterKind
  {
    In,
    Out,
    Ref
  }

  // ==================================================================================
  /// <summary>
  /// This type represents a formal parameter declaration.
  /// </summary>
  // ==================================================================================
  public class FormalParameter : AttributedElement
  {
    #region Private fields

    private FormalParameterKind _Kind;
    private bool _HasParams;
    private TypeReference _Type;

    #endregion

    #region Lifecycle methods

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Creates a new formal parameter declaration.
    /// </summary>
    /// <param name="token">Token providing position information.</param>
    // --------------------------------------------------------------------------------
    public FormalParameter(Token token)
      : base(token)
    {
      _Kind = FormalParameterKind.In;
    }

    #endregion

    #region Public methods

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets or sets the kind (in, out, ref) of the parameter.
    /// </summary>
    // --------------------------------------------------------------------------------
    public FormalParameterKind Kind
    {
      get { return _Kind; }
      set { _Kind = value; }
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets the flag indicating if the specified formal parameter has a "params"
    /// modifier or not.
    /// </summary>
    // --------------------------------------------------------------------------------
    public bool HasParams
    {
      get { return _HasParams; }
      set { _HasParams = value; }
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets or sets the type of the parameter.
    /// </summary>
    // --------------------------------------------------------------------------------
    public TypeReference Type
    {
      get { return _Type; }
      set { _Type = value; }
    }

    #endregion

    #region Type resolution

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Resolves all unresolved type references.
    /// </summary>
    /// <param name="contextType">Type of context where the resolution occurs.</param>
    /// <param name="contextInstance">Instance of the context.</param>
    // --------------------------------------------------------------------------------
    public override void ResolveTypeReferences(ResolutionContext contextType,
      IResolutionRequired contextInstance)
    {
      base.ResolveTypeReferences(contextType, contextInstance);

      if (_Type != null)
      {
        _Type.ResolveTypeReferences(contextType, contextInstance);
      }
    }

    #endregion
  }
}
