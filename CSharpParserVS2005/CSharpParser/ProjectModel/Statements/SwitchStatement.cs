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
  }
}