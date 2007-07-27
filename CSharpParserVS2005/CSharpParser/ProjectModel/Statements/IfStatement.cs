using System.Collections.Generic;
using CSharpParser.ParserFiles;
using CSharpParser.Semantics;

namespace CSharpParser.ProjectModel
{
  // ==================================================================================
  /// <summary>
  /// This type represents an "if" statement.
  /// </summary>
  // ==================================================================================
  public sealed class IfStatement : Statement
  {
    #region Private fields

    private BlockStatement _ThenStatements;
    private BlockStatement _ElseStatements;
    private Expression _Condition;

    #endregion

    #region Lifecycle methods

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Creates a new "if" statement declaration.
    /// </summary>
    /// <param name="token">Token providing position information.</param>
    // --------------------------------------------------------------------------------
    public IfStatement(Token token)
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
    public BlockStatement ThenStatements
    {
      get { return _ThenStatements; }
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets or sets the statements in the else branch.
    /// </summary>
    // --------------------------------------------------------------------------------
    public BlockStatement ElseStatements
    {
      get { return _ElseStatements; }
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets the flag indicating if "else" branch is presented or not.
    /// </summary>
    // --------------------------------------------------------------------------------
    public bool HasElse
    {
      get { return _ElseStatements != null; }
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets or sets the condition of the statement.
    /// </summary>
    // --------------------------------------------------------------------------------
    public Expression Condition
    {
      get { return _Condition; }
      set { _Condition = value; }
    }

    #endregion

    #region Public methods

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Creates an empty then block.
    /// </summary>
    /// <param name="t">Token of the block.</param>
    // --------------------------------------------------------------------------------
    public void CreateThenBlock(Token t)
    {
      _ThenStatements = new BlockStatement(t);
      _ThenStatements.SetParent(this);
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Creates an empty else block.
    /// </summary>
    /// <param name="t">Token of the block.</param>
    // --------------------------------------------------------------------------------
    public void CreateElseBlock(Token t)
    {
      _ElseStatements = new BlockStatement(t);
      _ElseStatements.SetParent(this);
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
        if (_ThenStatements != null)
        {
          foreach (Statement stmt in _ThenStatements.Statements)
          {
            yield return stmt;
          }
        }
        if (HasElse)
        {
          foreach (Statement stmt in _ElseStatements.Statements)
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
        yield return _ThenStatements;
        yield return _ElseStatements;
      }
    }

    #endregion

    #region Type resolution

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Resolves all unresolved type references.
    /// </summary>
    /// <param name="contextType">Type of context where the resolution occurs.</param>
    /// <param name="contextInstance">Instance of the context.</param>
    // --------------------------------------------------------------------------------
    public override void ResolveTypeReferences(ResolutionContext contextType,
      IResolutionRequired contextInstance)
    {
      base.ResolveTypeReferences(contextType, contextInstance);
      if (_ThenStatements != null)
      {
        _ThenStatements.ResolveTypeReferences(contextType, contextInstance);
      }
      if (_ElseStatements != null)
      {
        _ElseStatements.ResolveTypeReferences(contextType, contextInstance);
      }
    }

    #endregion
  }
}