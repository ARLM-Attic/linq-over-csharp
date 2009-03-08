using System.Collections.Generic;
using CSharpFactory.ParserFiles;
using CSharpFactory.Semantics;

namespace CSharpFactory.ProjectModel
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
    private readonly MemberDeclaration _DeclaringMember;
    private readonly StatementCollection _Statements = new StatementCollection(null);
    private readonly VariableCollection _Variables = new VariableCollection();
    private readonly List<IBlockOwner> _ChildBlocks = new List<IBlockOwner>();

    #endregion

    #region Lifecyle methods

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Creates a new accessor member declaration.
    /// </summary>
    /// <param name="token">Token providing position information.</param>
    /// <param name="declaringType">Type declaring this member.</param>
    /// <param name="declaringMember">Member declaring this accessor</param>
    // --------------------------------------------------------------------------------
    public AccessorDeclaration(Token token, TypeDeclaration declaringType, 
      MemberDeclaration declaringMember)
      : base(token, declaringType)
    {
      _DeclaringMember = declaringMember;
    }

    #endregion

    #region Public properties and methods

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

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets the member declaring this accessor.
    /// </summary>
    // --------------------------------------------------------------------------------
    public MemberDeclaration DeclaringMember
    {
      get { return _DeclaringMember; }
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets the qualified name of this member.
    /// </summary>
    // --------------------------------------------------------------------------------
    public override string QualifiedName
    {
      get { return DeclaringMember.QualifiedName + "." + Name; }
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
    public void Add(LocalVariable localVariable)
    {
      BlockStatement.AddVariableToBlock(this, localVariable);
    }

    #endregion

    #region IUsesResolutionContext implementation

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Resolves all unresolved type references.
    /// </summary>
    /// <param name="contextType">Type of resolution context.</param>
    /// <param name="declarationScope">Current type declaration context.</param>
    /// <param name="parameterScope">Current type parameter declaration scope.</param>
    // --------------------------------------------------------------------------------
    public override void ResolveTypeReferences(ResolutionContext contextType, 
      ITypeDeclarationScope declarationScope, 
      ITypeParameterScope parameterScope)
    {
      base.ResolveTypeReferences(contextType, declarationScope, parameterScope);
      Statement.ResolveTypeReferences(this, ResolutionContext.AccessorDeclaration, 
        declarationScope, parameterScope);
    }

    #endregion

    #region Semantic checks

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Checks the semantics for the specified field declaration.
    /// </summary>
    // --------------------------------------------------------------------------------
    public override void CheckSemantics()
    {
      NewNotAllowed();
      StaticNotAllowed();
      ReadOnlyNotAllowed();
      VolatileNotAllowed();
      VirtualNotAllowed();
      SealedNotAllowed();
      OverrideNotAllowed();
      AbstractNotAllowed();
      ExternNotAllowed();

      // --- Accessibility modifiers cannot be used
      if (DeclaringType.IsInterface && !HasDefaultVisibility)
      {
        Parser.Error0275(Token, QualifiedName);
      }
    }

    #endregion
  }
}
