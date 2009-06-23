// ================================================================================================
// SwitchSectionNode.cs
//
// Created: 2009.06.06, by Istvan Novak (DeepDiver)
// ================================================================================================
using CSharpTreeBuilder.CSharpAstBuilder;

namespace CSharpTreeBuilder.Ast
{
  // ================================================================================================
  /// <summary>
  /// 
  /// </summary>
  // ================================================================================================
  public class SwitchSectionNode : SyntaxNode<SwitchStatementNode>
  {
    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Initializes a new instance of the <see cref="SwitchSectionNode"/> class.
    /// </summary>
    /// <param name="start">Token providing information about the element.</param>
    // ----------------------------------------------------------------------------------------------
    public SwitchSectionNode(Token start)
      : base(start)
    {
      Labels = new SwitchLabelNodeCollection {ParentNode = this};
      Statements = new SwitchSectionStatementCollection {ParentNode = this};
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets or sets the labels of thi switch section.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public SwitchLabelNodeCollection Labels { get; private set; }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets or sets the statements belonging to this switch section.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public SwitchSectionStatementCollection Statements { get; private set; }

    #region Visitor methods

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Accepts a visitor object, according to the Visitor pattern.
    /// </summary>
    /// <param name="visitor">A visitor object</param>
    // ----------------------------------------------------------------------------------------------
    public override void AcceptVisitor(ISyntaxNodeVisitor visitor)
    {
      visitor.Visit(this);

      foreach (var label in Labels)
      {
        label.AcceptVisitor(visitor);
      }

      foreach (var statement in Statements)
      {
        statement.AcceptVisitor(visitor);
      }
    }

    #endregion
  }
}