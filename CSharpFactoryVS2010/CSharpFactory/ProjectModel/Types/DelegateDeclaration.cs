using CSharpFactory.ParserFiles;
using CSharpFactory.Semantics;

namespace CSharpFactory.ProjectModel
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
    /// Creates a new delegate declaration.
    /// </summary>
    /// <param name="token">Token providing position information.</param>
    /// <param name="parser">Parser instance</param>
    /// <param name="declaringType">
    /// Type that declares this type. Null, if this type has no declaring type.
    /// </param>
    // --------------------------------------------------------------------------------
    public DelegateDeclaration(Token token, CSharpSyntaxParser parser, 
      TypeDeclaration declaringType)
      : base(token, parser, declaringType)
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
    public override FormalParameterCollection FormalParameters
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
      return new DelegateDeclaration(Token, Parser, DeclaringType);
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

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Checks if unallowed "volatile" and "override" modifiers are used on this 
    /// type declaration.
    /// </summary>
    // --------------------------------------------------------------------------------
    protected override void CheckVolatileAndOverride()
    {
      // --- "volatile" is not allowed on types
      if ((_DeclaredModifier & Modifier.@volatile) != 0) 
        Parser.Error0106(Token, "volatile");

      // --- "override" is not allowed on types
      if ((_DeclaredModifier & Modifier.@volatile) != 0) 
        Parser.Error0106(Token, "override");
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Checks if type declaration matches with the declaration rules.
    /// </summary>
    // --------------------------------------------------------------------------------
    public override void CheckTypeDeclaration()
    {
      base.CheckTypeDeclaration();
      CheckUnallowedNonClassModifiers();
    }

    #endregion
  }
}
