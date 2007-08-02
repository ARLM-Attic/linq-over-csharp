using CSharpParser.ParserFiles;
using CSharpParser.Semantics;

namespace CSharpParser.ProjectModel
{
  // ==================================================================================
  /// <summary>
  /// This type represents a "do...while" statement.
  /// </summary>
  // ==================================================================================
  public sealed class DoWhileStatement : BlockStatement
  {
    #region Private fields

    private Expression _Condition;
    
    #endregion

    #region Lifecycle methods

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Creates a new "do...while" statement declaration.
    /// </summary>
    /// <param name="token">Token providing position information.</param>
    /// <param name="parent">Parent block of the statement.</param>
    // --------------------------------------------------------------------------------
    public DoWhileStatement(Token token, IBlockOwner parent)
      : base(token, parent)
    {
    }

    #endregion

    #region Public properties

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets or sets the condition of the statement.
    /// </summary>
    // --------------------------------------------------------------------------------
    public Expression Condition
    {
      get { return _Condition; }
      set { _Condition = value; }
    }

    #endregion

    #region Type Resolution

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
      if (_Condition != null)
      {
        _Condition.ResolveTypeReferences(contextType, contextInstance);
      }
    }

    #endregion
  }
}