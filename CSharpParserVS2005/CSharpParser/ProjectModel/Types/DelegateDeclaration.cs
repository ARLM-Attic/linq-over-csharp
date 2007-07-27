using CSharpParser.ParserFiles;
using CSharpParser.Semantics;

namespace CSharpParser.ProjectModel
{
  // ==================================================================================
  /// <summary>
  /// This type represents a delegate type declaration.
  /// </summary>
  // ==================================================================================
  public sealed class DelegateDeclaration : TypeDeclaration
  {
    #region Private fields

    private TypeReference _ReturnType;
    private readonly FormalParameterCollection _FormalParameters = new FormalParameterCollection();

    #endregion

    #region Lifecycle methods

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Creates a new delegate type declaration.
    /// </summary>
    /// <param name="token">Token providing position information.</param>
    /// <param name="parser">Parser instance</param>
    // --------------------------------------------------------------------------------
    public DelegateDeclaration(Token token, CSharpSyntaxParser parser)
      : base(token, parser)
    {
    }

    #endregion

    #region Public properties

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets or sets the type returned by this delegate.
    /// </summary>
    // --------------------------------------------------------------------------------
    public TypeReference ReturnType
    {
      get { return _ReturnType; }
      set { _ReturnType = value; }
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets the formal parameters of this delegate.
    /// </summary>
    // --------------------------------------------------------------------------------
    public FormalParameterCollection FormalParameters
    {
      get { return _FormalParameters; }
    }

    #endregion

    #region Type Resolution

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Resolves all unresolved type references in this namespace fragment.
    /// </summary>
    /// <param name="contextType">Type of context where the resolution occurs.</param>
    /// <param name="contextInstance">Instance of the context.</param>
    // --------------------------------------------------------------------------------
    public override void ResolveTypeReferences(ResolutionContext contextType, IResolutionRequired contextInstance)
    {
      base.ResolveTypeReferences(contextType, contextInstance);
      if (_ReturnType != null)
      {
        _ReturnType.ResolveTypeReferences(contextType, contextInstance);
      }
      foreach (FormalParameter param in _FormalParameters)
      {
        param.ResolveTypeReferences(contextType, contextInstance);
      }
    }

    #endregion
  }
}
