using CSharpParser.ParserFiles;
using CSharpParser.Semantics;

namespace CSharpParser.ProjectModel
{
  // ==================================================================================
  /// <summary>
  /// This abstract type represents an expression.
  /// </summary>
  // ==================================================================================
  public abstract class Expression : LanguageElement, IResolutionRequired
  {
    #region Privat fields

    private bool _BracketsUsed;

    #endregion

    #region Lifecycle methods

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Creates a new expression.
    /// </summary>
    /// <param name="token">Token providing position information.</param>
    // --------------------------------------------------------------------------------
    protected Expression(Token token)
      : base(token)
    {
    }

    #endregion

    #region Public properties

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets or sets the flag indicating that expression was defined between brackets.
    /// </summary>
    // --------------------------------------------------------------------------------
    public bool BracketsUsed
    {
      get { return _BracketsUsed; }
      set { _BracketsUsed = value; }
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets the leftmost part of this expression.
    /// </summary>
    // --------------------------------------------------------------------------------
    public Expression LeftmostExpression
    {
      get
      {
        UnaryOperator unOp = this as UnaryOperator;
        if (unOp != null) return unOp.Operand.LeftmostExpression;
        else
        {
          BinaryOperator binOp = this as BinaryOperator;
          if (binOp != null) return binOp.LeftOperand.LeftmostExpression;
        }
        return this;
      }
    }

    #endregion

    #region IResolutionRequired implementation

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Resolves all unresolved type references.
    /// </summary>
    /// <param name="contextType">Type of context where the resolution occurs.</param>
    /// <param name="contextInstance">Instance of the context.</param>
    // --------------------------------------------------------------------------------
    public virtual void ResolveTypeReferences(ResolutionContext contextType,
      IResolutionRequired contextInstance)
    {
    }

    #endregion
  }
}
