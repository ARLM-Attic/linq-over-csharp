using CSharpParser.ParserFiles;
using CSharpParser.Semantics;

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

    #region TypeResolution

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Resolves all unresolved type references.
    /// </summary>
    /// <param name="contextType">Type of context where the resolution occurs.</param>
    /// <param name="contextInstance">Instance of the context.</param>
    // --------------------------------------------------------------------------------
    public override void ResolveTypeReferences(ResolutionContext contextType, IResolutionRequired contextInstance)
    {
      base.ResolveTypeReferences(contextType, contextInstance);
      if (_Operand == null)
      {
        _Operand.ResolveTypeReferences(contextType, contextInstance);
      }
    }

    #endregion
  }
}
