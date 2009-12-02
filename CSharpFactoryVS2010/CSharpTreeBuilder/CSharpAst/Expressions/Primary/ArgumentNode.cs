// ================================================================================================
// ArgumentNode.cs
//
// Created: 2009.04.26, by Istvan Novak (DeepDiver)
// ================================================================================================
using CSharpTreeBuilder.CSharpAstBuilder;

namespace CSharpTreeBuilder.Ast
{
  // ================================================================================================
  /// <summary>
  /// This class represents an argument in the actual parameter list.
  /// </summary>
  // ================================================================================================
  public class ArgumentNode : SyntaxNode<ISyntaxNode>
  {
    /// <summary>Backing field for Expression property.</summary>
    private ExpressionNode _Expression;

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Initializes a new instance of the <see cref="ArgumentNode"/> class.
    /// </summary>
    /// <param name="start">The start token.</param>
    // ----------------------------------------------------------------------------------------------
    public ArgumentNode(Token start)
      : base(start)
    {
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets the kind of parameter (in, out, ref)
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public Token KindToken { get; internal set; }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets a value indicating whether this instance is "in" kind.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public bool IsIn
    {
      get { return KindToken == null; }
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets a value indicating whether this instance is "out" kind.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public bool IsOut
    {
      get { return KindToken != null && KindToken.Value == "out"; }
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets a value indicating whether this instance is "ref" kind.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public bool IsRef
    {
      get { return KindToken != null && KindToken.Value == "ref"; }
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets the expression.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public ExpressionNode Expression
    {
      get
      {
        return _Expression;
      }
      internal set
      {
        _Expression = value;
        if (_Expression != null)
        {
          _Expression.ParentNode = this;
        }
      }
    }

    #region Visitor methods

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Accepts a visitor object, according to the Visitor pattern.
    /// </summary>
    /// <param name="visitor">A visitor object</param>
    // ----------------------------------------------------------------------------------------------
    public override void AcceptVisitor(ISyntaxNodeVisitor visitor)
    {
      if (!visitor.Visit(this))
      {
        return;
      }

      if (Expression != null)
      {
        Expression.AcceptVisitor(visitor);
      }
    }

    #endregion
  }
}