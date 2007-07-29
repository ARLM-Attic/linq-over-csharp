using CSharpParser.ParserFiles;
using CSharpParser.Semantics;

namespace CSharpParser.ProjectModel
{
  // ==================================================================================
  /// <summary>
  /// This abstract type represents a value assignemnt statement, e.g. a "const".
  /// </summary>
  // ==================================================================================
  public class ValueAssignmentStatement : Statement
  {
    #region Private fields

    private TypeReference _ResultingType;
    private Expression _Expression;

    #endregion

    #region Lifecycle methods

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Creates a new "const" statement declaration.
    /// </summary>
    /// <param name="token">Token providing position information.</param>
    // --------------------------------------------------------------------------------
    public ValueAssignmentStatement(Token token)
      : base(token)
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
    /// Gets or sets the expression defining the assignment.
    /// </summary>
    // --------------------------------------------------------------------------------
    public Expression Expression
    {
      get { return _Expression; }
      set { _Expression = value; }
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
      if (_Expression != null)
      {
        _Expression.ResolveTypeReferences(contextType, contextInstance);
      }
    }

    #endregion
  }
}