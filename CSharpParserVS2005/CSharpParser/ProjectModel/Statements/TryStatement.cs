using System.Collections.Generic;
using CSharpParser.ParserFiles;

namespace CSharpParser.ProjectModel
{
  // ==================================================================================
  /// <summary>
  /// This type represents a "try...catch...finally" statement.
  /// </summary>
  // ==================================================================================
  public sealed class TryStatement : Statement
  {
    #region Private fields

    private BlockStatement _TryBlock;
    private BlockStatement _FinallyBlock;
    private List<CatchClause> _CatchClauses = new List<CatchClause>();

    #endregion

    #region Lifecycle methods

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Creates a new "try...catch...finally" statement declaration.
    /// </summary>
    /// <param name="token">Token providing position information.</param>
    // --------------------------------------------------------------------------------
    public TryStatement(Token token)
      : base(token)
    {
    }

    #endregion

    #region Public properties

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets or sets the statements in the then branch.
    /// </summary>
    // --------------------------------------------------------------------------------
    public BlockStatement TryBlock
    {
      get { return _TryBlock; }
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets or sets the statements in the else branch.
    /// </summary>
    // --------------------------------------------------------------------------------
    public BlockStatement FinallyBlock
    {
      get { return _FinallyBlock; }
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets the flag indicating if "else" branch is presented or not.
    /// </summary>
    // --------------------------------------------------------------------------------
    public bool HasFinally
    {
      get { return _FinallyBlock != null; }
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets or sets the list of catch clauses belonging to this statement.
    /// </summary>
    // --------------------------------------------------------------------------------
    public List<CatchClause> CatchClauses
    {
      get { return _CatchClauses; }
    }

    #endregion

    #region Public methods

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Creates an empty try block.
    /// </summary>
    /// <param name="t">Token of the block.</param>
    // --------------------------------------------------------------------------------
    public void CreateTryBlock(Token t)
    {
      _TryBlock = new BlockStatement(t);
      _TryBlock.SetParent(this);
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Creates an empty finally block.
    /// </summary>
    /// <param name="t">Token of the block.</param>
    // --------------------------------------------------------------------------------
    public void CreateFinallyBlock(Token t)
    {
      _FinallyBlock = new BlockStatement(t);
      _FinallyBlock.SetParent(this);
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Creates a new empty catch clause.
    /// </summary>
    /// <param name="t">Token of the block.</param>
    /// <returns>The newly created catch clause.</returns>
    // --------------------------------------------------------------------------------
    public CatchClause CreateCatchClause(Token t)
    {
      CatchClause result = new CatchClause(t);  
      result.SetParent(this);
      _CatchClauses.Add(result);
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
        if (_TryBlock != null)
        {
          foreach (Statement stmt in _TryBlock.NestedStatements)
          {
            yield return stmt;
          }
        }
        foreach (CatchClause cc in _CatchClauses)
        {
          yield return cc;
        }
        if (_FinallyBlock != null)
        {
          foreach (Statement stmt in _FinallyBlock.NestedStatements)
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
        if (_TryBlock != null) yield return _TryBlock;
        foreach (CatchClause cc in _CatchClauses)
        {
          yield return cc;
        }
        if (_FinallyBlock != null) yield return _FinallyBlock;
      }
    }

    #endregion

  }
}