using System.Collections.Generic;
using CSharpParser.ParserFiles;

namespace CSharpParser.ProjectModel
{
  // ==================================================================================
  /// <summary>
  /// This type represents a "switch" statement.
  /// </summary>
  // ==================================================================================
  public class SwitchStatement : Statement
  {
    #region Private fields

    private Expression _Expression;
    private List<SwitchSection> _Sections = new List<SwitchSection>();

    #endregion

    #region Lifecycle methods

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Creates a new "return" statement declaration.
    /// </summary>
    /// <param name="token">Token providing position information.</param>
    // --------------------------------------------------------------------------------
    public SwitchStatement(Token token)
      : base(token)
    {
    }

    #endregion

    #region Public properties

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets or sets the expression belonging to this switch statement.
    /// </summary>
    // --------------------------------------------------------------------------------
    public Expression Expression
    {
      get { return _Expression; }
      set { _Expression = value; }
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets the sections belonging to this switch statement.
    /// </summary>
    // --------------------------------------------------------------------------------
    public List<SwitchSection> Sections
    {
      get { return _Sections; }
    }

    #endregion

    #region Public methods

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Creates a new empty switch section.
    /// </summary>
    /// <param name="t">Token of the block.</param>
    /// <returns>The newly created switch section.</returns>
    // --------------------------------------------------------------------------------
    public SwitchSection CreateSwitchSection(Token t)
    {
      SwitchSection result = new SwitchSection(t);
      result.SetParent(this);
      _Sections.Add(result);
      return result;
    }

    #endregion

    #region Iterator methods

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Iterator enumerating all nested statements belonging to this one.
    /// </summary>
    /// <value>Returns recursively all nested statements.</value>
    /// <remarks>Statements are returned in the order of their declaration.</remarks>
    // --------------------------------------------------------------------------------
    public override IEnumerable<Statement> NestedStatements
    {
      get
      {
        foreach (SwitchSection section in _Sections)
        {
          foreach (Statement stmt in section.NestedStatements)
          {
            yield return stmt;
          }
        }
      }
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Iterator enumerating all direclty nested statements belonging to this one.
    /// </summary>
    /// <value>Returns only the directly nested statements, does not do recursion.</value>
    /// <remarks>Statements are returned in the order of their declaration.</remarks>
    // --------------------------------------------------------------------------------
    public override IEnumerable<Statement> DirectNestedStatements
    {
      get
      {
        foreach (SwitchSection section in _Sections)
        {
          yield return section;
        }
      }
    }

    #endregion

  }
}