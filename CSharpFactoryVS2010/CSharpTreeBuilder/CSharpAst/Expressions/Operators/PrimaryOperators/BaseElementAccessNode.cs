using CSharpTreeBuilder.CSharpAstBuilder;

namespace CSharpTreeBuilder.Ast
{
  // ================================================================================================
  /// <summary>
  /// This class represents a base[args] type expression
  /// </summary>
  // ================================================================================================
  public class BaseElementAccessNode : BaseAccessNode
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
  }
}