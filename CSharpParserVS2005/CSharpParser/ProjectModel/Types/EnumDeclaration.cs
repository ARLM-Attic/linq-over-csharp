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
    /// Creates a new enum declaration.
    /// </summary>
    /// <param name="token">Token providing position information.</param>
    /// <param name="parser">Parser instance</param>
    /// <param name="declaringType">
    /// Type that declares this type. Null, if this type has no declaring type.
    /// </param>
    // --------------------------------------------------------------------------------
    public EnumDeclaration(Token token, CSharpSyntaxParser parser, 
      TypeDeclaration declaringType)
      : base(token, parser, declaringType)
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
                 ? BaseTypeReference.Name
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
    /// <param name="contextType">Type of resolution context.</param>
    /// <param name="declarationScope">Current type declaration context.</param>
    /// <param name="parameterScope">Current type parameter declaration scope.</param>
    // --------------------------------------------------------------------------------
    public override void ResolveTypeReferences(ResolutionContext contextType, 
      ITypeDeclarationScope declarationScope, 
      ITypeParameterScope parameterScope)
    {
      base.ResolveTypeReferences(contextType, declarationScope, this);
      foreach (EnumValueDeclaration enumVal in _Values)
      {
        enumVal.ResolveTypeReferences(contextType, declarationScope, this);
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
      return new EnumDeclaration(Token, Parser, DeclaringType);
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
      EnumDeclaration clone = base.CloneToPart() as EnumDeclaration;
      foreach (EnumValueDeclaration value in _Values) clone._Values.Add(value);
      return clone;
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
    /// <param name="contextType">Type of resolution context.</param>
    /// <param name="declarationScope">Current type declaration context.</param>
    /// <param name="parameterScope">Current type parameter declaration scope.</param>
    // --------------------------------------------------------------------------------
    public override void ResolveTypeReferences(ResolutionContext contextType, 
      ITypeDeclarationScope declarationScope, 
      ITypeParameterScope parameterScope)
    {
      base.ResolveTypeReferences(contextType, declarationScope, parameterScope);
      if (_ValueExpression != null)
      {
        _ValueExpression.ResolveTypeReferences(contextType, declarationScope, parameterScope);
      }
    }

    #endregion
  }
}
