using CSharpParser.ParserFiles;
using CSharpParser.Semantics;

namespace CSharpParser.ProjectModel
{
  // ==================================================================================
  /// <summary>
  /// This abstract class represents an argument list operator expression.
  /// </summary>
  // ==================================================================================
  public sealed class ArgumentListOperator : UnaryOperator
  {
    #region Private fields

    private readonly ArgumentList _Arguments = new ArgumentList();

    #endregion

    #region Lifecycle methods

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Creates a new operator expression.
    /// </summary>
    /// <param name="token">Token providing position information.</param>
    // --------------------------------------------------------------------------------
    public ArgumentListOperator(Token token)
      : base(token)
    {
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Creates a new operator using the specified operand.
    /// </summary>
    /// <param name="token">Token providing position information.</param>
    /// <param name="operand">LeftOperand of the operator</param>
    // --------------------------------------------------------------------------------
    public ArgumentListOperator(Token token, Expression operand)
      : base(token, operand)
    {
    }

    #endregion

    #region Public properties

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets the list of arguments.
    /// </summary>
    // --------------------------------------------------------------------------------
    public ArgumentList Arguments
    {
      get { return _Arguments; }
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
      foreach (Argument arg in _Arguments)
      {
        arg.ResolveTypeReferences(contextType, contextInstance);
      }
    }

    #endregion
  }
}