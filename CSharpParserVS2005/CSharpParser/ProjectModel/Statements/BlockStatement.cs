using System.Collections.Generic;
using CSharpParser.ParserFiles;
using CSharpParser.Semantics;

namespace CSharpParser.ProjectModel
{
  // ==================================================================================
  /// <summary>
  /// This type represents a block of statements.
  /// </summary>
  // ==================================================================================
  public class BlockStatement : Statement, IBlockOwner
  {
    #region Private fields

    private readonly StatementCollection _Statements;

    #endregion

    #region Lifecyle methods

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Creates a new block statement declaration.
    /// </summary>
    /// <param name="token">Token providing position information.</param>
    // --------------------------------------------------------------------------------
    public BlockStatement(Token token)
      : base(token)
    {
      _Statements = new StatementCollection(this);
    }

    #endregion

    #region Public properties

    /// <summary>
    /// Gets the list of statements in this block.
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
      statement.SetParent(this);
    }

    #endregion

    #region IResolutionRequired implementation

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
      ResolveTypeReferences(this, contextType, contextInstance);
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
        foreach (Statement stmt in _Statements)
        {
          BlockStatement block = stmt as BlockStatement;
          if (block == null)
          {
            yield return stmt;
          }
          else
          {
            foreach (Statement nested in block.NestedStatements)
            yield return nested;
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
        foreach (Statement stmt in _Statements)
        {
          yield return stmt;
        }
      }
    }

    #endregion
  }
}
