using CSharpFactory.ParserFiles;
using CSharpFactory.Semantics;

namespace CSharpFactory.ProjectModel
{
  // ==================================================================================
  /// <summary>
  /// This abstract type represents an expression.
  /// </summary>
  // ==================================================================================
  public abstract class Expression : LanguageElement, IUsesResolutionContext
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
    /// <param name="parser">Parser instance creating this element.</param>
    // --------------------------------------------------------------------------------
    protected Expression(Token token, CSharpSyntaxParser parser)
      : base(token, parser)
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

    #region IUsesResolutionContext implementation

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Resolves all unresolved type references.
    /// </summary>
    /// <param name="contextType">Type of resolution context.</param>
    /// <param name="declarationScope">Current type declaration context.</param>
    /// <param name="parameterScope">Current type parameter declaration scope.</param>
    // --------------------------------------------------------------------------------
    public virtual void ResolveTypeReferences(ResolutionContext contextType, 
      ITypeDeclarationScope declarationScope, 
      ITypeParameterScope parameterScope)
    {
    }

    #endregion
  }
}
