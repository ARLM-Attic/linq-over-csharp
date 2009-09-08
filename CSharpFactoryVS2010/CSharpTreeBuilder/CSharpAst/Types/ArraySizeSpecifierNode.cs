using CSharpTreeBuilder.CSharpAstBuilder;
using CSharpTreeBuilder.Collections;

namespace CSharpTreeBuilder.Ast
{
  // ================================================================================================
  /// <summary>
  /// This class represents an array specifier node: [ expression , ... ]
  /// </summary>
  // ================================================================================================
  public class ArraySizeSpecifierNode : RankSpecifierNode
  {
    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Initializes a new instance of the <see cref="ArraySizeSpecifierNode"/> class.
    /// </summary>
    /// <param name="start">Token providing information about the element.</param>
    // ----------------------------------------------------------------------------------------------
    public ArraySizeSpecifierNode(Token start)
      : base(start)
    {
      Expressions = new ExpressionNodeCollection();
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Initializes a new instance of the <see cref="ArraySizeSpecifierNode"/> class.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public ArraySizeSpecifierNode()
      : this(null)
    {
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets a collection of expressions that specify the array sizes.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public ExpressionNodeCollection Expressions { get; private set; }

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

      foreach (var expression in Expressions)
      {
        expression.AcceptVisitor(visitor);
      }
    }

    #endregion
   }
}