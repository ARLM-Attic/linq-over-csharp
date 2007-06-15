using CSharpParser.ParserFiles;

namespace CSharpParser.ProjectModel
{
  // ==================================================================================
  /// <summary>
  /// This abstract class represents an argument list operator expression.
  /// </summary>
  // ==================================================================================
  public sealed class ArgumentListOperator : UnaryOperator
  {
    #region Private fields

    private ArgumentList _Arguments = new ArgumentList();

    #endregion

    #region Lifecycle methods

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Creates a new operator expression.
    /// </summary>
    /// <param name="token">Token providing position information.</param>
    // --------------------------------------------------------------------------------
    public ArgumentListOperator(Token token)
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
    public ArgumentListOperator(Token token, Expression operand)
      : base(token, operand)
    {
    }

    #endregion

    #region Public properties

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets the list of arguments.
    /// </summary>
    // --------------------------------------------------------------------------------
    public ArgumentList Arguments
    {
      get { return _Arguments; }
    }

    #endregion
  }
}