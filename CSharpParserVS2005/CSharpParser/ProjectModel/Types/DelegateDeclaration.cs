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
    /// <param name="contextType">Type of resolution context.</param>
    /// <param name="declarationScope">Current type declaration context.</param>
    /// <param name="parameterScope">Current type parameter declaration scope.</param>
    // --------------------------------------------------------------------------------
    public override void ResolveTypeReferences(ResolutionContext contextType, 
      ITypeDeclarationScope declarationScope, 
      ITypeParameterScope parameterScope)
    {
      base.ResolveTypeReferences(contextType, declarationScope, this);
      if (_ReturnType != null)
      {
        _ReturnType.ResolveTypeReferences(contextType, declarationScope, this);
      }
      foreach (FormalParameter param in _FormalParameters)
      {
        param.ResolveTypeReferences(contextType, declarationScope, this);
      }
    }

    #endregion

    #region Overridden methods

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Creates a new instance with the type of this declaration.
    /// </summary>
    // --------------------------------------------------------------------------------
    protected override TypeDeclaration CreateNewPart()
    {
      return new DelegateDeclaration(Token, Parser);
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Clones this type declaration into a new instance.
    /// </summary>
    /// <returns>
    /// The new cloned instance.
    /// </returns>
    // --------------------------------------------------------------------------------
    public override TypeDeclaration CloneToPart()
    {
      DelegateDeclaration clone = base.CloneToPart() as DelegateDeclaration;
      clone._ReturnType = _ReturnType;
      foreach (FormalParameter param in _FormalParameters) 
        clone._FormalParameters.Add(param);
      return clone;
    }

    #endregion
  }
}
