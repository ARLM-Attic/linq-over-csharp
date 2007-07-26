using System.Collections.Generic;
using CSharpParser.ParserFiles;

namespace CSharpParser.ProjectModel
{
  // ==================================================================================
  /// <summary>
  /// This abstract class represents an anonymous delegate operator expression.
  /// </summary>
  // ==================================================================================
  public sealed class AnonymousDelegateOperator : PrimaryOperator, IBlockOwner
  {
    #region Private fields

    private FormalParameterCollection _FormalParameters = new FormalParameterCollection();
    private StatementCollection _Statements;

    #endregion

    #region Lifecycle methods

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Creates a new operator expression.
    /// </summary>
    /// <param name="token">Token providing position information.</param>
    // --------------------------------------------------------------------------------
    public AnonymousDelegateOperator(Token token)
      : base(token)
    {
      Name = string.Empty;
      _Statements = new StatementCollection(null);
    }

    #endregion

    #region Public properties

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets the list of formal parameters belonging to the method.
    /// </summary>
    // --------------------------------------------------------------------------------
    public FormalParameterCollection FormalParameters
    {
      get { return _FormalParameters; }
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets the list of statements in the method body.
    /// </summary>
    // --------------------------------------------------------------------------------
    public StatementCollection Statements
    {
      get { return _Statements; }
    }

    #endregion

    #region IBlockOwner Members

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets the element owning the block;
    /// </summary>
    // --------------------------------------------------------------------------------
    public LanguageElement Owner
    {
      get { return this; }
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Adds a new statement to the method body.
    /// </summary>
    /// <param name="statement">Statement to add.</param>
    // --------------------------------------------------------------------------------
    public void Add(Statement statement)
    {
      Statements.Add(statement);
    }

    #endregion
  }
}