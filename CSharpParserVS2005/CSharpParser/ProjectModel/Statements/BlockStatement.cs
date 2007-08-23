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
    private readonly VariableCollection _Variables = new VariableCollection();
    private readonly List<IBlockOwner> _ChildBlocks = new List<IBlockOwner>();

    #endregion

    #region Lifecyle methods

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Creates a new block statement declaration.
    /// </summary>
    /// <param name="token">Token providing position information.</param>
    /// <param name="parser">Parser instance creating this element.</param>
    /// <param name="parentBlock">Parent block of this block.</param>
    // --------------------------------------------------------------------------------
    public BlockStatement(Token token, CSharpSyntaxParser parser, IBlockOwner parentBlock)
      : base(token, parser, parentBlock)
    {
      _Statements = new StatementCollection(this);
    }

    #endregion

    #region Public properties

    // --------------------------------------------------------------------------------
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
    public IBlockOwner Owner
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
      IBlockOwner blockStatement = statement as IBlockOwner;
      if (blockStatement != null)
      {
        _ChildBlocks.Add(blockStatement);
      }
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets the list ob child block in this one.
    /// </summary>
    // --------------------------------------------------------------------------------
    public List<IBlockOwner> ChildBlocks
    {
      get { return _ChildBlocks; }
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets the collection of variables belonging to this block.
    /// </summary>
    // --------------------------------------------------------------------------------
    public VariableCollection Variables
    {
      get { return _Variables; }
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Adds a new localVariable the block.
    /// </summary>
    /// <param name="localVariable">Variable to add to the block.</param>
    // --------------------------------------------------------------------------------
    public virtual void Add(LocalVariable localVariable)
    {
      AddVariableToBlock(this, localVariable);
    }

    #endregion

    #region Static helper methods

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Adds a new local variable to the specified block.
    /// </summary>
    /// <param name="block">Block that owns the local variable.</param>
    /// <param name="localVariable">Variable to add to the block.</param>
    // --------------------------------------------------------------------------------
    public static void AddVariableToBlock(IBlockOwner block, LocalVariable localVariable)
    {
      // --- Check for duplication in the same scope
      if (block.Variables.Contains(localVariable))
      {
        localVariable.Parser.Error0128(localVariable);
        return;
      }

      // --- Cycle from this block to its parents
      IBlockOwner currentBlock = block;
      do
      {
        if (currentBlock.ParentBlock == null)
        {
          // --- This block does not have a parent, maybe its parent statement has...
          Statement stmt = currentBlock.Owner as Statement;
          currentBlock = stmt == null ? null : stmt.Parent as IBlockOwner;
        }
        else currentBlock = currentBlock.ParentBlock;

        // --- Leave the block, if there is no parent
        if (currentBlock == null) break;

        // --- Check the parent block
        if (currentBlock.Variables.Contains(localVariable))
        {
          localVariable.Parser.Error0136(localVariable, "parent");
          return;
        }
      } while (true);

      // --- Now check embedded blocks recursively
      if (IsVariableInChildBlock(block, localVariable)) return;

      // --- Ok, this is a new variable.
      block.Variables.Add(localVariable);
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Checks if the specified local variable is defined in any of the childs of the
    /// given block
    /// </summary>
    /// <param name="block">Block to check</param>
    /// <param name="localVariable">Local variable to check</param>
    /// <returns>
    /// True, if any of the child blocks defines the variable.; otherwise, false.
    /// </returns>
    // --------------------------------------------------------------------------------
    private static bool IsVariableInChildBlock(IBlockOwner block, LocalVariable localVariable)
    {
      foreach (IBlockOwner childBlock in block.ChildBlocks)
      {
        if (childBlock.Variables.Contains(localVariable))
        {
          localVariable.Parser.Error0136(localVariable, "child");
          return true;
        }
        if (IsVariableInChildBlock(childBlock, localVariable)) return true; 
      }
      return false;      
    }

    #endregion

    #region IUsesResolutionContext implementation

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
