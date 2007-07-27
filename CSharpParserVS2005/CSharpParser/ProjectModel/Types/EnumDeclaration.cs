using System.Collections.Generic;
using CSharpParser.ParserFiles;

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
    // --------------------------------------------------------------------------------
    public EnumValueDeclaration(Token token)
      : base(token)
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
  }
}
