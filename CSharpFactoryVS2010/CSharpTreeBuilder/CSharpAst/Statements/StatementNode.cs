// ================================================================================================
// StatementNode.cs
//
// Created: 2009.05.06, by Istvan Novak (DeepDiver)
// ================================================================================================
using CSharpTreeBuilder.CSharpAstBuilder;

namespace CSharpTreeBuilder.Ast
{
  // ================================================================================================
  /// <summary>
  /// This abstract node is intended to be the root of all C# language statements.
  /// </summary>
  // ================================================================================================
  public abstract class StatementNode : SyntaxNode<ISyntaxNode>
  {
    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Initializes a new instance of the <see cref="StatementNode"/> class.
    /// </summary>
    /// <param name="start">Token providing information about the element.</param>
    // ----------------------------------------------------------------------------------------------
    protected StatementNode(Token start)
      : base(start)
    {
      Labels = new LabelNodeCollection { ParentNode = this };
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets or sets the labels belonging to this statement.
    /// </summary>
    /// <value>The labels.</value>
    // ----------------------------------------------------------------------------------------------
    public LabelNodeCollection Labels { get; private set; }

    #region Visitor methods

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Accepts a visitor object, according to the Visitor pattern.
    /// </summary>
    /// <param name="visitor">A visitor object</param>
    // ----------------------------------------------------------------------------------------------
    public override void AcceptVisitor(ISyntaxNodeVisitor visitor)
    {
      foreach (var label in Labels)
      {
        label.AcceptVisitor(visitor);
      }
    }

    #endregion
  }
}