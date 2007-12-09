using CSharpParser.ParserFiles;

namespace CSharpParser.ProjectModel
{
  // ==================================================================================
  /// <summary>
  /// This type represents a simple initializer.
  /// </summary>
  // ==================================================================================
  public class MemberDeclarator : ExpressionInitializer
  {
    #region Private field

    private readonly TypeReference _PredefinedType;
    private readonly bool _IsMemberAccess;

    #endregion

    #region Lifecycle methods

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Creates a new member declarator instance.
    /// </summary>
    /// <param name="token">Token providing position information.</param>
    /// <param name="parser">Parser used by this language element.</param>
    /// <param name="identifier">Identifier belonging to the member</param>
    // --------------------------------------------------------------------------------
    public MemberDeclarator(Token token, CSharpSyntaxParser parser, string identifier) : 
      base(token, parser, null)
    {
      Name = identifier;
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Creates a new member declarator instance.
    /// </summary>
    /// <param name="token">Token providing position information.</param>
    /// <param name="parser">Parser used by this language element.</param>
    /// <param name="expr">Expression element creating this element</param>
    /// <param name="identifier">Identifier belonging to the member</param>
    // --------------------------------------------------------------------------------
    public MemberDeclarator(Token token, CSharpSyntaxParser parser, Expression expr, 
      string identifier) :
      base(token, parser, expr)
    {
      Name = identifier;
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Creates a new member declarator instance.
    /// </summary>
    /// <param name="token">Token providing position information.</param>
    /// <param name="parser">Parser used by this language element.</param>
    /// <param name="expr">Expression element creating this element</param>
    /// <param name="identifier">Identifier belonging to the member</param>
    /// <param name="isMemberAccess">Sets if this is a member access or not.</param>
    // --------------------------------------------------------------------------------
    public MemberDeclarator(Token token, CSharpSyntaxParser parser, Expression expr,
      string identifier, bool isMemberAccess) :
      base(token, parser, expr)
    {
      Name = identifier;
      _IsMemberAccess = isMemberAccess;
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Creates a new member declarator instance.
    /// </summary>
    /// <param name="token">Token providing position information.</param>
    /// <param name="parser">Parser used by this language element.</param>
    /// <param name="typeRef">Built-in type reference</param>
    /// <param name="identifier">Identifier belonging to the member</param>
    // --------------------------------------------------------------------------------
    public MemberDeclarator(Token token, CSharpSyntaxParser parser, TypeReference typeRef,
      string identifier) :
      base(token, parser, null)
    {
      Name = identifier;
      _PredefinedType = typeRef;
    }

    #endregion

    #region Public properties

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets the predefined type belongig to this declaration.
    /// </summary>
    // --------------------------------------------------------------------------------
    public TypeReference PredefinedType
    {
      get { return _PredefinedType; }
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets the flag indicating if this declaration is a member access or not.
    /// </summary>
    // --------------------------------------------------------------------------------
    public bool IsMemberAccess
    {
      get { return _IsMemberAccess; }
    }

    #endregion

  }
}