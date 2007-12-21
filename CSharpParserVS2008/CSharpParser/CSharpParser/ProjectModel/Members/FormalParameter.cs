using CSharpParser.Collections;
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
    /// <summary>Input parameter</summary>
    In,
    /// <summary>Output parameter</summary>
    Out,
    /// <summary>Parameter passed by reference</summary>
    Ref,
    /// <summary>Extension method parameter</summary>
    This
  }

  // ==================================================================================
  /// <summary>
  /// This type represents a formal parameter declaration.
  /// </summary>
  // ==================================================================================
  public class FormalParameter : AttributedElement, IVariableInfo
  {
    #region Private fields

    private FormalParameterKind _Kind;
    private bool _HasParams;
    private TypeReference _Type;
    private VariableCategory _Category;
    private bool _IsInitiallyAssigned;

    #endregion

    #region Lifecycle methods

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Creates a new formal parameter declaration.
    /// </summary>
    /// <param name="token">Token providing position information.</param>
    /// <param name="parser">Parser used by this language element.</param>
    // --------------------------------------------------------------------------------
    public FormalParameter(Token token, CSharpSyntaxParser parser)
      : base(token, parser)
    {
      Kind = FormalParameterKind.In;
    }

    #endregion

    #region Public properties

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets or sets the kind (in, out, ref) of the parameter.
    /// </summary>
    /// <remarks>
    /// The kind of formal parameter determines the type of variable.
    /// </remarks>
    // --------------------------------------------------------------------------------
    public FormalParameterKind Kind
    {
      get { return _Kind; }
      set
      {
        _Kind = value;
        _Category = VariableCategory.ValueParameter;
        if (_Kind == FormalParameterKind.Out) _Category = VariableCategory.OutputParameter;
        else if (_Kind == FormalParameterKind.Ref) _Category = VariableCategory.ReferenceParameter;
        _IsInitiallyAssigned = _Category != VariableCategory.OutputParameter;
      }
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

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Defines the category of the variable.
    /// </summary>
    // --------------------------------------------------------------------------------
    public VariableCategory Category
    {
      get { return _Category; }
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Signs if the variable is initially assigned or not.
    /// </summary>
    // --------------------------------------------------------------------------------
    public bool IsInitiallyAssigned
    {
      get { return _IsInitiallyAssigned; }
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Stores the declaration point (token) of the variable.
    /// </summary>
    // --------------------------------------------------------------------------------
    public int DeclarationPosition
    {
      get { return Token.pos; }
    }

    #endregion

    #region Type resolution

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Resolves all unresolved type references.
    /// </summary>
    /// <param name="contextType">Type of resolution context.</param>
    /// <param name="declarationScope">Current type declaration context.</param>
    /// <param name="parameterScope">Current type parameter declaration scope.</param>
    // --------------------------------------------------------------------------------
    public override void ResolveTypeReferences(ResolutionContext contextType, 
      ITypeDeclarationScope declarationScope, 
      ITypeParameterScope parameterScope)
    {
      base.ResolveTypeReferences(contextType, declarationScope, parameterScope);

      if (_Type != null)
      {
        _Type.ResolveTypeReferences(contextType, declarationScope, parameterScope);
      }
    }

    #endregion
  }

  // ==================================================================================
  /// <summary>
  /// This type represents a collection formal parameter declarations.
  /// </summary>
  // ==================================================================================
  public sealed class FormalParameterCollection : RestrictedCollection<FormalParameter>
  {
  }
}
