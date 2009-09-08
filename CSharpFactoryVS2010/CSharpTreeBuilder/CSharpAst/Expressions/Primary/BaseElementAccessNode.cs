using CSharpTreeBuilder.CSharpAstBuilder;

namespace CSharpTreeBuilder.Ast
{
  // ================================================================================================
  /// <summary>
  /// This class represents the base[args] expression
  /// </summary>
  // ================================================================================================
  public sealed class BaseElementAccessNode : BaseAccessNode
  {
    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Initializes a new instance of the <see cref="BaseElementAccessNode"/> class.
    /// </summary>
    /// <param name="start">Token providing information about the element.</param>
    // ----------------------------------------------------------------------------------------------
    public BaseElementAccessNode(Token start)
      : base(start)
    {
      Expressions = new ExpressionNodeCollection { ParentNode = this };
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets or sets the expression collection.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public ExpressionNodeCollection Expressions { get; internal set; }

    #region Visitor methods

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Accepts a visitor object, according to the Visitor pattern.
    /// </summary>
    /// <param name="visitor">A visitor object</param>
    // ----------------------------------------------------------------------------------------------
    public override void AcceptVisitor(ISyntaxNodeVisitor visitor)
    {
      if (!visitor.Visit(this)) { return; }

      foreach (var expression in Expressions)
      {
        expression.AcceptVisitor(visitor);
      }
    }

    #endregion
  }
}