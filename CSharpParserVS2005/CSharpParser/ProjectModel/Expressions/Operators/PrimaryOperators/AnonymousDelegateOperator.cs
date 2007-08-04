using System.Collections.Generic;
using CSharpParser.ParserFiles;
using CSharpParser.Semantics;

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

    private readonly FormalParameterCollection _FormalParameters = new FormalParameterCollection();
    private readonly StatementCollection _Statements;
    private readonly VariableCollection _Variables = new VariableCollection();
    private readonly List<IBlockOwner> _ChildBlocks = new List<IBlockOwner>();

    #endregion

    #region Lifecycle methods

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Creates a new operator expression.
    /// </summary>
    /// <param name="token">Token providing position information.</param>
    /// <param name="parser">Parser instance creating this element.</param>
    // --------------------------------------------------------------------------------
    public AnonymousDelegateOperator(Token token, CSharpSyntaxParser parser)
      : base(token, parser)
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
    public IBlockOwner Owner
    {
      get { return this; }
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// A method declaration never has a parent block, so this property return null.
    /// </summary>
    // --------------------------------------------------------------------------------
    public IBlockOwner ParentBlock
    {
      get { return null; }
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
    /// Adds a new variable the block.
    /// </summary>
    /// <param name="localVariable">Variable to add to the block.</param>
    // --------------------------------------------------------------------------------
    public void Add(LocalVariable localVariable)
    {
      BlockStatement.AddVariableToBlock(this, localVariable);
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
      base.ResolveTypeReferences(contextType, contextInstance);
      Statement.ResolveTypeReferences(this, contextType, contextInstance);
    }

    #endregion
  }
}