using CSharpFactory.ParserFiles;
using CSharpFactory.Semantics;

namespace CSharpFactory.ProjectModel
{
  // ==================================================================================
  /// <summary>
  /// This type represents a catch clause in a try..catch..finally statement.
  /// </summary>
  // ==================================================================================
  public sealed class CatchClause : BlockStatement
  {
    #region Private fields

    private TypeReference _ExceptionType;
    private LocalVariable _Variable;

    #endregion

    #region Lifecycle methods

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Creates a new "fixed" statement declaration.
    /// </summary>
    /// <param name="token">Token providing position information.</param>
    /// <param name="parser">Parser instance creating this element.</param>
    /// <param name="parent">Parent block of the statement.</param>
    // --------------------------------------------------------------------------------
    public CatchClause(Token token, CSharpSyntaxParser parser, IBlockOwner parent)
      : base(token, parser, parent)
    {
    }

    #endregion

    #region Public properties

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets or sets the type of exception in this clause.
    /// </summary>
    // --------------------------------------------------------------------------------
    public TypeReference ExceptionType
    {
      get { return _ExceptionType; }
      set { _ExceptionType = value; }
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets or sets the flag indicating if this clause has an exception specified.
    /// </summary>
    // --------------------------------------------------------------------------------
    public bool HasExceptionType
    {
      get { return _ExceptionType != null; }
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets the flag indicating if this clause has an exception instance variable 
    /// specified.
    /// </summary>
    // --------------------------------------------------------------------------------
    public bool HasVariable
    {
      get { return _Variable != null; }
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets the local variable defined by this catch clause.
    /// </summary>
    // --------------------------------------------------------------------------------
    public LocalVariable Variable
    {
      get { return _Variable; }
    }

    #endregion

    #region Public methods

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Creates an instance variable for this catch block.
    /// </summary>
    /// <param name="type">Exception type</param>
    /// <param name="name">Variable name</param>
    // --------------------------------------------------------------------------------
    public void CreateInstanceVariable(TypeReference type, string name)
    {
      _Variable = new LocalVariable(Token, Parser, this);
      _Variable.ResultingType = type;
      _Variable.Name = name;
      Add(_Variable);
    }

    #endregion

    #region Type resolution

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
      if (_ExceptionType != null)
      {
        _ExceptionType.ResolveTypeReferences(contextType, declarationScope, parameterScope);
      }
    }

    #endregion
  }
}