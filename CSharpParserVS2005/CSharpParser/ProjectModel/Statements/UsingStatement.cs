using System.Collections.Generic;
using CSharpParser.ParserFiles;

namespace CSharpParser.ProjectModel
{
  // ==================================================================================
  /// <summary>
  /// This type represents a "using" statement.
  /// </summary>
  // ==================================================================================
  public sealed class UsingStatement : BlockStatement
  {
    #region Private fields

    private Expression _ResourceExpression;
    private BlockStatement _ResourceDeclarations;

    #endregion

    #region Lifecycle methods

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Creates a new "using" statement declaration.
    /// </summary>
    /// <param name="token">Token providing position information.</param>
    // --------------------------------------------------------------------------------
    public UsingStatement(Token token)
      : base(token)
    {
    }

    #endregion

    #region Public properties

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets or sets the resource acquisition expression belonging to the "using".
    /// </summary>
    // --------------------------------------------------------------------------------
    public Expression ResourceExpression
    {
      get { return _ResourceExpression; }
      set { _ResourceExpression = value; }
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets or sets the resource declaration block belonging to the "using".
    /// </summary>
    // --------------------------------------------------------------------------------
    public BlockStatement ResourceDeclarations
    {
      get { return _ResourceDeclarations; }
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets the flag indicating if the "using" has a resource declaration block.
    /// </summary>
    // --------------------------------------------------------------------------------
    public bool HasResourceDeclaration
    {
      get { return _ResourceDeclarations != null; }
    }

    #endregion

    #region Public methods

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Creates a new resource declaration block.
    /// </summary>
    /// <param name="t">Token of the block.</param>
    // --------------------------------------------------------------------------------
    public void CreateResourceDeclarations(Token t)
    {
      _ResourceDeclarations = new BlockStatement(t);
      _ResourceDeclarations.SetParent(this);
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
        if (_ResourceDeclarations != null)
        {
          foreach (Statement stmt in _ResourceDeclarations.NestedStatements)
          {
            yield return stmt;
          }
        }
        foreach (Statement stmt in base.NestedStatements)
        {
          yield return stmt;
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
        if (_ResourceDeclarations != null) yield return _ResourceDeclarations;
        foreach (Statement stmt in base.DirectNestedStatements)
        {
          yield return stmt;
        }
      }
    }

    #endregion
  }
}