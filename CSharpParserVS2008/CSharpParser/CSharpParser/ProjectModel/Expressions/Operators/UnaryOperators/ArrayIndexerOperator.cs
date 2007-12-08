using System.Collections.Generic;
using CSharpParser.ParserFiles;
using CSharpParser.Semantics;

namespace CSharpParser.ProjectModel
{
  // ==================================================================================
  /// <summary>
  /// This abstract class represents an array indexer operator expression.
  /// </summary>
  // ==================================================================================
  public sealed class ArrayIndexerOperator : UnaryOperator
  {
    #region Private fields

    private List<Expression> _Indexers = new List<Expression>();

    #endregion

    #region Lifecycle methods

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Creates a new operator expression.
    /// </summary>
    /// <param name="token">Token providing position information.</param>
    /// <param name="parser">Parser instance creating this element.</param>
    // --------------------------------------------------------------------------------
    public ArrayIndexerOperator(Token token, CSharpSyntaxParser parser) : base(token, parser)
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
    public ArrayIndexerOperator(Token token, CSharpSyntaxParser parser, Expression operand)
      : base(token, parser, operand)
    {
    }

    #endregion

    #region Public properties

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets the list of indexers.
    /// </summary>
    // --------------------------------------------------------------------------------
    public List<Expression> Indexers
    {
      get { return _Indexers; }
    }

    #endregion
  
    #region TypeResolution

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
      foreach (Expression expr in _Indexers)
      {
        expr.ResolveTypeReferences(contextType, declarationScope, parameterScope);
      }
    }

    #endregion
  }
}