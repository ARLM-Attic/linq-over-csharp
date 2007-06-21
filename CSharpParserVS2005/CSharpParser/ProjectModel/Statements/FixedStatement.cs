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

    #region Iterator methods

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Iterator enumerating this and all nested statements belonging to this one.
    /// </summary>
    /// <value>Returns recursively all nested statements.</value>
    /// <remarks>
    /// First the variable declaration statements are returned, then the nested 
    /// statements in the order of their declaration.
    /// </remarks>
    // --------------------------------------------------------------------------------
    public override IEnumerable<Statement> AllStatements
    {
      get
      {
        foreach (ValueAssignmentStatement stmt in _Assignments)
        {
          yield return stmt;
        }
        foreach (Statement stmt in NestedStatements)
        {
          yield return stmt;
        }
      }
    }

    #endregion

  }
}