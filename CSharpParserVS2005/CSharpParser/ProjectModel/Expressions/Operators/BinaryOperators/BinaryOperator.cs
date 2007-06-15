using CSharpParser.ParserFiles;

namespace CSharpParser.ProjectModel
{
  // ==================================================================================
  /// <summary>
  /// This abstract class represents a unary operator
  /// </summary>
  // ==================================================================================
  public abstract class BinaryOperator : OperatorExpression
  {
    #region Private fields

    private Expression _LeftOperand;
    private Expression _RightOperand;

    #endregion

    #region Lifecycle methods

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Creates a new unary operator.
    /// </summary>
    /// <param name="token">Token providing position information.</param>
    // --------------------------------------------------------------------------------
    protected BinaryOperator(Token token)
      : base(token)
    {
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Creates a new operator using the specified left operand.
    /// </summary>
    /// <param name="token">Token providing position information.</param>
    /// <param name="leftOperand">Left operand of the operator</param>
    // --------------------------------------------------------------------------------
    public BinaryOperator(Token token, Expression leftOperand)
      : base(token)
    {
      _LeftOperand = leftOperand;
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Creates a new operator using the specified left and right operand.
    /// </summary>
    /// <param name="token">Token providing position information.</param>
    /// <param name="leftOperand">Left operand of the operator</param>
    /// <param name="rightOperand">Right operand of the operator</param>
    // --------------------------------------------------------------------------------
    protected BinaryOperator(Token token, Expression leftOperand, Expression rightOperand)
      : base(token)
    {
      _LeftOperand = leftOperand;
      _RightOperand = rightOperand;
    }

    #endregion

    #region Public properties

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets or sets the left operand of the unary operator.
    /// </summary>
    // --------------------------------------------------------------------------------
    public Expression LeftOperand
    {
      get { return _LeftOperand; }
      set { _LeftOperand = value; }
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets or sets the right operand of the unary operator.
    /// </summary>
    // --------------------------------------------------------------------------------
    public Expression RightOperand
    {
      get { return _RightOperand; }
      set { _RightOperand = value; }
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets the leftmost expression that has a null left operand including this 
    /// operator.
    /// </summary>
    // --------------------------------------------------------------------------------
    public BinaryOperator LeftMostNonNull
    {
      get
      {
        BinaryOperator current = this;
        do
        {
          if (current._LeftOperand == null) return current;
          BinaryOperator next = current._LeftOperand as BinaryOperator;
          if (next == null) return current;
          current = next;
        } while (true);
      }
    }

    #endregion
  }
}