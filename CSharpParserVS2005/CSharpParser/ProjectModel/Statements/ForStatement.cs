using System.Collections.Generic;
using CSharpParser.ParserFiles;
using CSharpParser.Semantics;

namespace CSharpParser.ProjectModel
{
  // ==================================================================================
  /// <summary>
  /// This type represents a "for" statement.
  /// </summary>
  // ==================================================================================
  public sealed class ForStatement : BlockStatement
  {
    #region Private fields

    private BlockStatement _InitializerBlock;
    private Expression _Condition;
    private BlockStatement _IteratorBlock;

    #endregion

    #region Lifecycle methods

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Creates a new "for" statement declaration.
    /// </summary>
    /// <param name="token">Token providing position information.</param>
    // --------------------------------------------------------------------------------
    public ForStatement(Token token)
      : base(token)
    {
    }

    #endregion

    #region Public properties

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gest or sets the initializer block of the for statements.
    /// </summary>
    // --------------------------------------------------------------------------------
    public BlockStatement InitializerBlock
    {
      get { return _InitializerBlock; }
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gest or sets the condition expression of the for statements.
    /// </summary>
    // --------------------------------------------------------------------------------
    public Expression Condition
    {
      get { return _Condition; }
      set { _Condition = value; }
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gest or sets the iterator block of the for statements.
    /// </summary>
    // --------------------------------------------------------------------------------
    public BlockStatement IteratorBlock
    {
      get { return _IteratorBlock; }
    }

    #endregion

    #region Public methods

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Creates an empty initializer block.
    /// </summary>
    /// <param name="t">Token of the block.</param>
    // --------------------------------------------------------------------------------
    public void CreateInitializerBlock(Token t)
    {
      _InitializerBlock = new BlockStatement(t);  
      _InitializerBlock.SetParent(this);
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Creates an empty iterator block.
    /// </summary>
    /// <param name="t">Token of the block.</param>
    // --------------------------------------------------------------------------------
    public void CreateIteratorBlock(Token t)
    {
      _IteratorBlock = new BlockStatement(t);
      _IteratorBlock.SetParent(this);
    }

    #endregion

    #region Iterator methods

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Iterator enumerating this and all nested statements belonging to this one.
    /// </summary>
    /// <value>Returns recursively all nested statements.</value>
    /// <remarks>
    /// First initialization, then block statements and last the iterator statements
    /// are returned.
    /// </remarks>
    // --------------------------------------------------------------------------------
    public override IEnumerable<Statement> AllStatements
    {
      get
      {
        if (_InitializerBlock != null)
        {
          foreach (Statement stmt in _InitializerBlock.Statements)
          {
            yield return stmt;
          }
        }
        foreach (Statement stmt in NestedStatements)
        {
          yield return stmt;
        }
        if (_IteratorBlock != null)
        {
          foreach (Statement stmt in _IteratorBlock.Statements)
          {
            yield return stmt;
          }
        }
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
      if (_InitializerBlock != null)
      {
        _InitializerBlock.ResolveTypeReferences(contextType, contextInstance);
      }
      if (_Condition != null)
      {
        _Condition.ResolveTypeReferences(contextType, contextInstance);
      }
      if (_IteratorBlock != null)
      {
        _IteratorBlock.ResolveTypeReferences(contextType, contextInstance);
      }
    }

    #endregion
  }
}