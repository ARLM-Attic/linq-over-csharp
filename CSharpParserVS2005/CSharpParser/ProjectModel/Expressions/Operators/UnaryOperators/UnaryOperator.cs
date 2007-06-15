using CSharpParser.ParserFiles;

namespace CSharpParser.ProjectModel
{
  // ==================================================================================
  /// <summary>
  /// This abstract class represents a unary operator
  /// </summary>
  // ==================================================================================
  public abstract class UnaryOperator : OperatorExpression
  {
    #region Private fields

    private Expression _Operand;

    #endregion

    #region Lifecycle methods

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Creates a new unary operator.
    /// </summary>
    /// <param name="token">Token providing position information.</param>
    // --------------------------------------------------------------------------------
    protected UnaryOperator(Token token)
      : base(token)
    {
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Creates a new operator using the specified operand.
    /// </summary>
    /// <param name="token">Token providing position information.</param>
    /// <param name="operand">LeftOperand of the operator</param>
    // --------------------------------------------------------------------------------
    public UnaryOperator(Token token, Expression operand)
      : base(token)
    {
      Operand = operand;
    }

    #endregion

    #region Public properties

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets or sets the operand of the unary operator.
    /// </summary>
    // --------------------------------------------------------------------------------
    public Expression Operand
    {
      get { return _Operand; }
      set { _Operand = value; }
    }

    #endregion
  }
}
