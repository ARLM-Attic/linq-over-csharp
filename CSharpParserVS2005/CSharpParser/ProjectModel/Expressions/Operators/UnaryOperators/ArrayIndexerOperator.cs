using System.Collections.Generic;
using CSharpParser.ParserFiles;

namespace CSharpParser.ProjectModel
{
  // ==================================================================================
  /// <summary>
  /// This abstract class represents an array indexer operator expression.
  /// </summary>
  // ==================================================================================
  public sealed class ArrayIndexerOperator : UnaryOperator
  {
    #region Private fields

    private List<Expression> _Indexers = new List<Expression>();

    #endregion

    #region Lifecycle methods

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Creates a new operator expression.
    /// </summary>
    /// <param name="token">Token providing position information.</param>
    // --------------------------------------------------------------------------------
    public ArrayIndexerOperator(Token token)
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
    public ArrayIndexerOperator(Token token, Expression operand)
      : base(token, operand)
    {
    }

    #endregion

    #region Public properties

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets the list of indexers.
    /// </summary>
    // --------------------------------------------------------------------------------
    public List<Expression> Indexers
    {
      get { return _Indexers; }
    }

    #endregion
  }
}