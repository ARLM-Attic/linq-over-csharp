using System.Collections.Generic;
using CSharpParser.ParserFiles;

namespace CSharpParser.ProjectModel
{
  // ==================================================================================
  /// <summary>
  /// This class represents a base indexer operator expression.
  /// </summary>
  // ==================================================================================
  public sealed class BaseIndexerOperator : PrimaryOperator
  {
    #region Private fields

    private List<Expression> _Indexes = new List<Expression>();

    #endregion

    #region Lifecycle methods

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Creates a new operator expression.
    /// </summary>
    /// <param name="token">Token providing position information.</param>
    // --------------------------------------------------------------------------------
    public BaseIndexerOperator(Token token)
      : base(token)
    {
    }

    #endregion

    #region Public properties

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets the container of indexes.
    /// </summary>
    // --------------------------------------------------------------------------------
    public List<Expression> Indexes
    {
      get { return _Indexes; }
    }

    #endregion
  }
}