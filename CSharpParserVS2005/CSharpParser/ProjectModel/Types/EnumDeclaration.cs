using System.Collections.Generic;
using CSharpParser.ParserFiles;
using CSharpParser.Semantics;

namespace CSharpParser.ProjectModel
{
  // ==================================================================================
  /// <summary>
  /// This type represents an enumeration declaration.
  /// </summary>
  // ==================================================================================
  public sealed class EnumDeclaration : TypeDeclaration
  {
    #region Private fields

    private readonly List<EnumValueDeclaration> _Values = new List<EnumValueDeclaration>();

    #endregion

    #region Lifecycle methods

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Creates a new enumeration declaration.
    /// </summary>
    /// <param name="token">Token providing position information.</param>
    /// <param name="parser">Parser used.</param>
    // --------------------------------------------------------------------------------
    public EnumDeclaration(Token token, CSharpSyntaxParser parser)
      : base(token, parser)
    {
    }

    #endregion

    #region Public properties

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets the name of the base type of this enumeration.
    /// </summary>
    // --------------------------------------------------------------------------------
    public string BaseTypeName
    {
      get
      {
        return HasBaseType
                 ? BaseTypes[0].Name
                 : string.Empty;
      }
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets the values of this enumeration.
    /// </summary>
    // --------------------------------------------------------------------------------
    public List<EnumValueDeclaration> Values
    {
      get { return _Values; }
    }

    #endregion

    #region Type resolution

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Resolves all unresolved type references in this namespace fragment.
    /// </summary>
    /// <param name="contextType">Type of context where the resolution occurs.</param>
    /// <param name="contextInstance">Instance of the context.</param>
    // --------------------------------------------------------------------------------
    public override void ResolveTypeReferences(ResolutionContext contextType, IUsesResolutionContext contextInstance)
    {
      base.ResolveTypeReferences(contextType, contextInstance);
      foreach (EnumValueDeclaration enumVal in _Values)
      {
        enumVal.ResolveTypeReferences(contextType, contextInstance);
      }
    }

    #endregion
  }

  // ==================================================================================
  /// <summary>
  /// This type represents an enumeration value declaration.
  /// </summary>
  // ==================================================================================
  public sealed class EnumValueDeclaration : AttributedElement
  {
    #region Private fields

    private Expression _ValueExpression;

    #endregion

    #region Lifecycle methods

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Creates a new enumeration declaration.
    /// </summary>
    /// <param name="token">Token providing position information.</param>
    /// <param name="parser">Parser instance creating this element.</param>
    // --------------------------------------------------------------------------------
    public EnumValueDeclaration(Token token, CSharpSyntaxParser parser) : base(token, parser)
    {
    }

    #endregion

    #region Public properties

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets or sets the expression of the enumeration value.
    /// </summary>
    // --------------------------------------------------------------------------------
    public Expression ValueExpression
    {
      get { return _ValueExpression; }
      set { _ValueExpression = value; }
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets the flag indicating if the enumeration value has an expression or not.
    /// </summary>
    // --------------------------------------------------------------------------------
    public bool HasValueExpression
    {
      get { return _ValueExpression != null; }
    }

    #endregion

    #region IUsesResolutionContext implementation

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Resolves all unresolved type references in this type reference
    /// </summary>
    /// <param name="contextType">Type of context where the resolution occurs.</param>
    /// <param name="contextInstance">Instance of the context.</param>
    // --------------------------------------------------------------------------------
    public override void ResolveTypeReferences(ResolutionContext contextType,
      IUsesResolutionContext contextInstance)
    {
      base.ResolveTypeReferences(contextType, contextInstance);
      if (_ValueExpression != null)
      {
        _ValueExpression.ResolveTypeReferences(contextType, contextInstance);
      }
    }

    #endregion
  }
}
