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
    // --------------------------------------------------------------------------------
    public ExpressionStatement(Token token)
      : base(token)
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
    /// <param name="contextType">Type of context where the resolution occurs.</param>
    /// <param name="contextInstance">Instance of the context.</param>
    // --------------------------------------------------------------------------------
    public override void ResolveTypeReferences(ResolutionContext contextType,
      IResolutionRequired contextInstance)
    {
      base.ResolveTypeReferences(contextType, contextInstance);
      if (_Expression != null)
      {
        _Expression.ResolveTypeReferences(contextType, contextInstance);
      }
    }

    #endregion

  }
}