using System.Collections.Generic;
using CSharpParser.ParserFiles;

namespace CSharpParser.ProjectModel
{
  // ==================================================================================
  /// <summary>
  /// This type represents a "fixed" statement.
  /// </summary>
  // ==================================================================================
  public sealed class FixedStatement : BlockStatement
  {
    #region Private fields

    private List<ValueAssignmentStatement> _Assignments = new List<ValueAssignmentStatement>();

    #endregion

    #region Lifecycle methods

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Creates a new "fixed" statement declaration.
    /// </summary>
    /// <param name="token">Token providing position information.</param>
    // --------------------------------------------------------------------------------
    public FixedStatement(Token token)
      : base(token)
    {
    }

    #endregion

    #region Public properties

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets the list of assignment statements belonging to this "fixed" statement.
    /// </summary>
    // --------------------------------------------------------------------------------
    public List<ValueAssignmentStatement> Assignments
    {
      get { return _Assignments; }
    }

    #endregion
  }
}