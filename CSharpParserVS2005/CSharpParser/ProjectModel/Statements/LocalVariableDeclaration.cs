using CSharpParser.ParserFiles;
using CSharpParser.Semantics;

namespace CSharpParser.ProjectModel
{
  // ==================================================================================
  /// <summary>
  /// This type represents a local variable declaration statement.
  /// </summary>
  // ==================================================================================
  public sealed class LocalVariableDeclaration : Statement
  {
    #region Private fields

    private TypeReference _ResultingType;
    private Initializer _Initializer;

    #endregion

    #region Lifecycle methods

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Creates a new local variable declaration statement.
    /// </summary>
    /// <param name="token">Token providing position information.</param>
    /// <param name="parentBlock">Block owning this statement.</param>
    // --------------------------------------------------------------------------------
    public LocalVariableDeclaration(Token token, IBlockOwner parentBlock)
      : base(token, parentBlock)
    {
    }

    #endregion

    #region Public properties

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets or sets the type of the constant.
    /// </summary>
    // --------------------------------------------------------------------------------
    public TypeReference ResultingType
    {
      get { return _ResultingType; }
      set { _ResultingType = value; }
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets or sets the initializer of this declaration.
    /// </summary>
    // --------------------------------------------------------------------------------
    public Initializer Initializer
    {
      get { return _Initializer; }
      set { _Initializer = value; }
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
      if (_ResultingType != null)
      {
        _ResultingType.ResolveTypeReferences(contextType, contextInstance);
      }
      if (_Initializer != null)
      {
        _Initializer.ResolveTypeReferences(contextType, contextInstance);
      }
    }

    #endregion
  }
}