using CSharpParser.ParserFiles;
using CSharpParser.Semantics;

namespace CSharpParser.ProjectModel
{
  // ==================================================================================
  /// <summary>
  /// This abstract class represents a type expression.
  /// </summary>
  // ==================================================================================
  public sealed class TypeOperator : PrimaryOperator
  {
    #region Private fields

    private readonly TypeReference _Type;

    #endregion

    #region Lifecycle methods

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Creates a new operator expression.
    /// </summary>
    /// <param name="token">Token providing position information.</param>
    /// <param name="parser">Parser instance creating this element.</param>
    // --------------------------------------------------------------------------------
    public TypeOperator(Token token, CSharpSyntaxParser parser) : base(token, parser)
    {
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Creates a new operator expression.
    /// </summary>
    /// <param name="token">Token providing position information.</param>
    /// <param name="type">Type used by the operator</param>
    // --------------------------------------------------------------------------------
    public TypeOperator(Token token, TypeReference type)
      : base(token, type.Parser)
    {
      _Type = type;
    }

    #endregion

    #region Public properties

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets or sets the type of this cast operator.
    /// </summary>
    // --------------------------------------------------------------------------------
    public TypeReference Type
    {
      get { return _Type; }
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
      if (_Type != null)
      {
        _Type.ResolveTypeReferences(contextType, declarationScope, parameterScope);
      }
    }

    #endregion
  }
}