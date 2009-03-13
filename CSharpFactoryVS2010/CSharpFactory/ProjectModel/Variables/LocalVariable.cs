using CSharpFactory.Collections;
using CSharpFactory.ParserFiles;

namespace CSharpFactory.ProjectModel
{
  // ==================================================================================
  /// <summary>
  /// This type represents a variable defined as according to S12.1.7
  /// </summary>
  // ==================================================================================
  public sealed class LocalVariable : LanguageElement, IVariableInfo
  {
    #region Private fields

    private readonly IBlockOwner _ParentBlock;
    private TypeReference _ResultingType;
    private Initializer _Initializer;
    private bool _IsInitiallyAssigned;
    private bool _IsImplicit;

    #endregion

    #region Lifecycle methods

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Creates a new variable belonging to the specified parent block.
    /// </summary>
    /// <param name="token">Token providing position information.</param>
    /// <param name="parser">Parser instance creating this element.</param>
    /// <param name="parentBlock">Block owning this statement.</param>
    // --------------------------------------------------------------------------------
    public LocalVariable(Token token, CSharpSyntaxParser parser, IBlockOwner parentBlock)
      : base(token, parser)
    {
      _ParentBlock = parentBlock;
      _IsInitiallyAssigned = false;
    }

    #endregion

    #region Public methods

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets or sets the type of the variable.
    /// </summary>
    // --------------------------------------------------------------------------------
    public TypeReference ResultingType
    {
      get { return _ResultingType; }
      set { _ResultingType = value; }
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets or sets the initializer of this variable.
    /// </summary>
    // --------------------------------------------------------------------------------
    public Initializer Initializer
    {
      get { return _Initializer; }
      set
      {
        _Initializer = value;
        _IsInitiallyAssigned = value != null;
      }
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets the block owning the statement
    /// </summary>
    // --------------------------------------------------------------------------------
    public IBlockOwner ParentBlock
    {
      get { return _ParentBlock; }
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Defines the category of the variable.
    /// </summary>
    // --------------------------------------------------------------------------------
    public VariableCategory Category
    {
      get { return VariableCategory.Local; }
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Signs if the variable is initially assigned or not.
    /// </summary>
    // --------------------------------------------------------------------------------
    public bool IsInitiallyAssigned
    {
      get { return _IsInitiallyAssigned; }
      set { _IsInitiallyAssigned = value; }
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets or sets the flag indicating if the variable has been declared with the
    /// "var" keyword.
    /// </summary>
    // --------------------------------------------------------------------------------
    public bool IsImplicit
    {
      get { return _IsImplicit; }
      set { _IsImplicit = value; }
    } 
    
    // --------------------------------------------------------------------------------
    /// <summary>
    /// Stores the declaration point (token) of the variable.
    /// </summary>
    // --------------------------------------------------------------------------------
    public int DeclarationPosition
    {
      get { return Token.pos; }
    }

    #endregion
  }

  // ==================================================================================
  /// <summary>
  /// This type defines a collection of variables that can be indexed by the
  /// signature of the method.
  /// </summary>
  // ==================================================================================
  public class VariableCollection : ImmutableIndexedCollection<LocalVariable>
  {
    // --------------------------------------------------------------------------------
    /// <summary>
    /// Defines the key to be used by the indexing.
    /// </summary>
    /// <param name="item">MethodDeclaration item.</param>
    /// <returns>
    /// Signature of the member declaration.
    /// </returns>
    // --------------------------------------------------------------------------------
    protected override string GetKeyOfItem(LocalVariable item)
    {
      return item.Name;
    }
  }
}
