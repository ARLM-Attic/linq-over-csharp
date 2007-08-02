using CSharpParser.ParserFiles;
using CSharpParser.Semantics;

namespace CSharpParser.ProjectModel
{
  // ==================================================================================
  /// <summary>
  /// This type represents an accessor method declaration.
  /// </summary>
  // ==================================================================================
  public sealed class AccessorDeclaration : MemberDeclaration, IBlockOwner
  {
    #region Private fields

    private bool _HasBody;
    private readonly StatementCollection _Statements = new StatementCollection(null);

    #endregion

    #region Lifecyle methods

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Creates a new accessor member declaration.
    /// </summary>
    /// <param name="token">Token providing position information.</param>
    // --------------------------------------------------------------------------------
    public AccessorDeclaration(Token token)
      : base(token)
    {
    }

    #endregion

    #region Public methods

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets or sets the flag indicating if this accessor has a body or not.
    /// </summary>
    // --------------------------------------------------------------------------------
    public bool HasBody
    {
      get { return _HasBody; }
      set { _HasBody = value; }
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets the list of statements in the accessor body.
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
    /// An accessor declaration never has a parent block, so this property return null.
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
      Statement.ResolveTypeReferences(this, ResolutionContext.AccessorDeclaration, this);
    }

    #endregion
  }
}
