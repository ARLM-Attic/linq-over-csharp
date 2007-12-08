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
    /// <param name="parser">Parser instance creating this element.</param>
    // --------------------------------------------------------------------------------
    public ArgumentListOperator(Token token, CSharpSyntaxParser parser) : base(token, parser)
    {
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Creates a new operator using the specified operand.
    /// </summary>
    /// <param name="token">Token providing position information.</param>
    /// <param name="parser">Parser instance creating this element.</param>
    /// <param name="operand">LeftOperand of the operator</param>
    // --------------------------------------------------------------------------------
    public ArgumentListOperator(Token token, CSharpSyntaxParser parser, Expression operand)
      : base(token, parser, operand)
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
    /// <param name="contextType">Type of resolution context.</param>
    /// <param name="declarationScope">Current type declaration context.</param>
    /// <param name="parameterScope">Current type parameter declaration scope.</param>
    // --------------------------------------------------------------------------------
    public override void ResolveTypeReferences(ResolutionContext contextType, 
      ITypeDeclarationScope declarationScope, 
      ITypeParameterScope parameterScope)
    {
      base.ResolveTypeReferences(contextType, declarationScope, parameterScope);
      foreach (Argument arg in _Arguments)
      {
        arg.ResolveTypeReferences(contextType, declarationScope, parameterScope);
      }
    }

    #endregion
  }
}