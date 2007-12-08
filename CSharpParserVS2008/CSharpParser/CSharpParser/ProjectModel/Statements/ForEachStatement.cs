using CSharpParser.ParserFiles;
using CSharpParser.Semantics;

namespace CSharpParser.ProjectModel
{
  // ==================================================================================
  /// <summary>
  /// This type represents a "foreach" statement.
  /// </summary>
  // ==================================================================================
  public sealed class ForEachStatement : BlockStatement
  {
    #region Private fields

    private Expression _Expression;
    private readonly LocalVariable _Variable;

    #endregion

    #region Lifecycle methods

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Creates a new "foreach" statement declaration.
    /// </summary>
    /// <param name="token">Token providing position information.</param>
    /// <param name="parser">Parser instance creating this element.</param>
    /// <param name="parentBlock">Block owning this statement.</param>
    // --------------------------------------------------------------------------------
    public ForEachStatement(Token token, CSharpSyntaxParser parser, IBlockOwner parentBlock)
      : base(token, parser, parentBlock)
    {
      _Variable = new LocalVariable(token, parser, parentBlock);
      _Variable.IsInitiallyAssigned = true;
    }

    #endregion

    #region Public properties

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets the foreach variable
    /// </summary>
    // --------------------------------------------------------------------------------
    public LocalVariable Variable
    {
      get { return _Variable; }
    } 
    
    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets or sets the container expression of foreach statement.
    /// </summary>
    // --------------------------------------------------------------------------------
    public Expression Expression
    {
      get { return _Expression; }
      set { _Expression = value; }
    }

    #endregion

    #region Public methods


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
      if (_Variable.ResultingType != null)
      {
        _Variable.ResultingType.
          ResolveTypeReferences(contextType, declarationScope, parameterScope);
      }
      if (_Expression != null)
      {
        _Expression.ResolveTypeReferences(contextType, declarationScope, parameterScope);
      }
    }

    #endregion
  }
}