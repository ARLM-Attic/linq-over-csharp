using System;
using CSharpFactory.ParserFiles;
using CSharpFactory.Semantics;
using CSharpSyntaxParser=CSharpFactory.ParserFiles.CSharpSyntaxParser;

namespace CSharpFactory.ProjectModel
{
  // ==================================================================================
  /// <summary>
  /// This abstract class represents a unary operator
  /// </summary>
  // ==================================================================================
  public abstract class BinaryOperator : OperatorExpression
  {
    #region Private fields

    private Expression _LeftOperand;
    private Expression _RightOperand;

    #endregion

    #region Lifecycle methods

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Creates a new unary operator.
    /// </summary>
    /// <param name="token">Token providing position information.</param>
    /// <param name="parser">Parser instance creating this element.</param>
    // --------------------------------------------------------------------------------
    protected BinaryOperator(ParserFiles.Token token, CSharpSyntaxParser parser) : base(token, parser)
    {
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Creates a new operator using the specified left operand.
    /// </summary>
    /// <param name="token">Token providing position information.</param>
    /// <param name="leftOperand">Left operand of the operator</param>
    // --------------------------------------------------------------------------------
    public BinaryOperator(ParserFiles.Token token, Expression leftOperand)
      : base(token, leftOperand.Parser)
    {
      if (leftOperand == null) throw new ArgumentNullException("leftOperand");
      _LeftOperand = leftOperand;
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Creates a new operator using the specified left and right operand.
    /// </summary>
    /// <param name="token">Token providing position information.</param>
    /// <param name="leftOperand">Left operand of the operator</param>
    /// <param name="rightOperand">Right operand of the operator</param>
    // --------------------------------------------------------------------------------
    protected BinaryOperator(Token token, Expression leftOperand, Expression rightOperand)
      : base(token, leftOperand.Parser)
    {
      if (leftOperand == null) throw new ArgumentNullException("leftOperand");
      _LeftOperand = leftOperand;
      if (rightOperand == null) throw new ArgumentNullException("rightOperand");
      _RightOperand = rightOperand;
    }

    #endregion

    #region Public properties

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets or sets the left operand of the unary operator.
    /// </summary>
    // --------------------------------------------------------------------------------
    public Expression LeftOperand
    {
      get { return _LeftOperand; }
      set { _LeftOperand = value; }
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets or sets the right operand of the unary operator.
    /// </summary>
    // --------------------------------------------------------------------------------
    public Expression RightOperand
    {
      get { return _RightOperand; }
      set { _RightOperand = value; }
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets the leftmost expression that has a null left operand including this 
    /// operator.
    /// </summary>
    // --------------------------------------------------------------------------------
    public BinaryOperator LeftMostNonNull
    {
      get
      {
        BinaryOperator current = this;
        do
        {
          if (current._LeftOperand == null) return current;
          BinaryOperator next = current._LeftOperand as BinaryOperator;
          if (next == null) return current;
          current = next;
        } while (true);
      }
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
      if (_LeftOperand != null)
      {
        _LeftOperand.ResolveTypeReferences(contextType, declarationScope, parameterScope);
      }
      if (_RightOperand != null)
      {
        _RightOperand.ResolveTypeReferences(contextType, declarationScope, parameterScope);
      }
    }

    #endregion
  }
}