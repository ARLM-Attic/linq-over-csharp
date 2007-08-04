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

    private readonly LocalVariable _Variable;

    #endregion

    #region Lifecycle methods

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Creates a new local variable declaration statement.
    /// </summary>
    /// <param name="token">Token providing position information.</param>
    /// <param name="parser">Parser instance creating this element.</param>
    /// <param name="parentBlock">Block owning this statement.</param>
    // --------------------------------------------------------------------------------
    public LocalVariableDeclaration(Token token, CSharpSyntaxParser parser, IBlockOwner parentBlock)
      : base(token, parser, parentBlock)
    {
      _Variable = new LocalVariable(token, parser, parentBlock);
    }

    #endregion

    #region Public properties

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets the variable information for this declaration.
    /// </summary>
    // --------------------------------------------------------------------------------
    public LocalVariable Variable
    {
      get { return _Variable; }
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
      if (_Variable.ResultingType != null)
      {
        _Variable.ResultingType.ResolveTypeReferences(contextType, contextInstance);
      }
      if (_Variable.Initializer != null)
      {
        _Variable.Initializer.ResolveTypeReferences(contextType, contextInstance);
      }
    }

    #endregion
  }
}