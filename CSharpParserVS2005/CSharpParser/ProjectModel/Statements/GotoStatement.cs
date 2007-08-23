using CSharpParser.ParserFiles;
using CSharpParser.Semantics;

namespace CSharpParser.ProjectModel
{
  // ==================================================================================
  /// <summary>
  /// This type represents a goto statement.
  /// </summary>
  // ==================================================================================
  public sealed class GotoStatement : Statement
  {
    #region Private fields

    private Expression _LabelExpression;

    #endregion

    #region Lifecycle methods

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Creates a new goto statement declaration.
    /// </summary>
    /// <param name="token">Token providing position information.</param>
    /// <param name="parser">Parser instance creating this element.</param>
    /// <param name="parentBlock">Block owning this statement.</param>
    // --------------------------------------------------------------------------------
    public GotoStatement(Token token, CSharpSyntaxParser parser, IBlockOwner parentBlock)
      : base(token, parser, parentBlock)
    {
    }

    #endregion 

    #region Public properties

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets or sets the label expression belonging to the goto statement.
    /// </summary>
    // --------------------------------------------------------------------------------
    public Expression LabelExpression
    {
      get { return _LabelExpression; }
      set { _LabelExpression = value; }
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets the flag indicating if this is a simple label goto.
    /// </summary>
    // --------------------------------------------------------------------------------
    public bool IsSimpleLabel
    {
      get { return _LabelExpression == null && Name != "default";  }
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets the flag indicating if this is "goto default".
    /// </summary>
    // --------------------------------------------------------------------------------
    public bool IsDefaultLabel
    {
      get { return _LabelExpression == null && Name == "default"; }
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets the flag indicating if this is a "goto case xxx".
    /// </summary>
    // --------------------------------------------------------------------------------
    public bool IsCaseLabel
    {
      get { return _LabelExpression != null; }
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
      IUsesResolutionContext contextInstance)
    {
      base.ResolveTypeReferences(contextType, contextInstance);
      if (_LabelExpression != null)
      {
        _LabelExpression.ResolveTypeReferences(contextType, contextInstance);
      }
    }

    #endregion
  }
}