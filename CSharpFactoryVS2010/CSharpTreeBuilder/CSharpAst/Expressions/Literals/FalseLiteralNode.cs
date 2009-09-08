// ================================================================================================
// FalseLiteralNode.cs
//
// Created: 2009.05.11, by Istvan Novak (DeepDiver)
// ================================================================================================
using CSharpTreeBuilder.CSharpAstBuilder;

namespace CSharpTreeBuilder.Ast
{
  // ================================================================================================
  /// <summary>
  /// This class defines a "false" boolean literal node.
  /// </summary>
  // ================================================================================================
  public sealed class FalseLiteralNode : BooleanLiteralNode
  {
    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Initializes a new instance of the <see cref="BooleanLiteralNode"/> class.
    /// </summary>
    /// <param name="start">Token providing information about the element.</param>
    // ----------------------------------------------------------------------------------------------
    public FalseLiteralNode(Token start)
      : base(start)
    {
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets the value of the literal.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public override bool Value { get { return false; } }

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
    }

    #endregion
  }
}