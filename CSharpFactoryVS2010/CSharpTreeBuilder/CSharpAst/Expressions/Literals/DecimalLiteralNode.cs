// ================================================================================================
// DecimalLiteralNode.cs
//
// Created: 2009.04.15, by Istvan Novak (DeepDiver)
// ================================================================================================
using CSharpTreeBuilder.CSharpAstBuilder;

namespace CSharpTreeBuilder.Ast
{
  // ================================================================================================
  /// <summary>
  /// This class defines a System.Decimal literal.
  /// </summary>
  // ================================================================================================
  public class DecimalLiteralNode : RealLiteralNode
  {
    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Initializes a new instance of the <see cref="DecimalLiteralNode"/> class.
    /// </summary>
    /// <param name="start">The start token.</param>
    /// <param name="value">The value of the constant.</param>
    // ----------------------------------------------------------------------------------------------
    public DecimalLiteralNode(Token start, decimal value)
      : base(start)
    {
      Value = value;
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets the value of the constant.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public decimal Value { get; internal set; }

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