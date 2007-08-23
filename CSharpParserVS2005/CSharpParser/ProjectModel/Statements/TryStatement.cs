using System.Collections.Generic;
using CSharpParser.ParserFiles;
using CSharpParser.Semantics;

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
    private readonly List<CatchClause> _CatchClauses = new List<CatchClause>();

    #endregion

    #region Lifecycle methods

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Creates a new "try...catch...finally" statement declaration.
    /// </summary>
    /// <param name="token">Token providing position information.</param>
    /// <param name="parser">Parser instance creating this element.</param>
    /// <param name="parentBlock">Block owning this statement.</param>
    // --------------------------------------------------------------------------------
    public TryStatement(Token token, CSharpSyntaxParser parser, IBlockOwner parentBlock)
      : base(token, parser, parentBlock)
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
      _TryBlock = new BlockStatement(t, Parser, ParentBlock);
      _TryBlock.SetParent(this);
      ParentBlock.ChildBlocks.Add(_TryBlock);
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Creates an empty finally block.
    /// </summary>
    /// <param name="t">Token of the block.</param>
    // --------------------------------------------------------------------------------
    public void CreateFinallyBlock(Token t)
    {
      _FinallyBlock = new BlockStatement(t, Parser, ParentBlock);
      _FinallyBlock.SetParent(this);
      ParentBlock.ChildBlocks.Add(_FinallyBlock);
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
      CatchClause result = new CatchClause(t, Parser, ParentBlock);  
      result.SetParent(this);
      _CatchClauses.Add(result);
      ParentBlock.ChildBlocks.Add(result);
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

    #region Type resolution

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Resolves all unresolved type references.
    /// </summary>
    /// <param name="contextType">Type of context where the resolution occurs.</param>
    /// <param name="contextInstance">Instance of the context.</param>
    // --------------------------------------------------------------------------------
    public override void ResolveTypeReferences(ResolutionContext contextType,
      IUsesResolutionContext contextInstance)
    {
      base.ResolveTypeReferences(contextType, contextInstance);
      if (_TryBlock != null)
      {
        _TryBlock.ResolveTypeReferences(contextType, contextInstance);
      }
      if (_FinallyBlock != null)
      {
        _FinallyBlock.ResolveTypeReferences(contextType, contextInstance);
      }
      foreach (CatchClause clause in _CatchClauses)
      {
        clause.ResolveTypeReferences(contextType, contextInstance);
      }
    }

    #endregion
  }
}