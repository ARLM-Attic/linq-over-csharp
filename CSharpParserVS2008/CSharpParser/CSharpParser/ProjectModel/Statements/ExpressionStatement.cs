using CSharpParser.ParserFiles;
using CSharpParser.Semantics;

namespace CSharpParser.ProjectModel
{
  // ==================================================================================
  /// <summary>
  /// This type represents an expression statement (contains only an expression).
  /// </summary>
  // ==================================================================================
  public sealed class ExpressionStatement : Statement
  {
    #region Private fields

    private Expression _Expression;

    #endregion

    #region Lifecycle methods

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Creates a new empty statement declaration.
    /// </summary>
    /// <param name="token">Token providing position information.</param>
    /// <param name="parser">Parser instance creating this element.</param>
    /// <param name="parentBlock">Block owning this statement.</param>
    // --------------------------------------------------------------------------------
    public ExpressionStatement(Token token, CSharpSyntaxParser parser, IBlockOwner parentBlock)
      : base(token, parser, parentBlock)
    {
    }

    #endregion

    #region Public properties

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets or sets the expression of the statement.
    /// </summary>
    // --------------------------------------------------------------------------------
    public Expression Expression
    {
      get { return _Expression; }
      set { _Expression = value; }
    }

    #endregion

    #region Type Resolution

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
      if (_Expression != null)
      {
        _Expression.ResolveTypeReferences(contextType, declarationScope, parameterScope);
      }
    }

    #endregion

  }
}