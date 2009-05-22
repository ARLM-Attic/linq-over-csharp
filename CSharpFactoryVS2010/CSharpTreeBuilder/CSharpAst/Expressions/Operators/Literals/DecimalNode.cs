// ================================================================================================
// DecimalNode.cs
//
// Created: 2009.04.15, by Istvan Novak (DeepDiver)
// ================================================================================================
using CSharpTreeBuilder.CSharpAstBuilder;

namespace CSharpTreeBuilder.Ast
{
  // ================================================================================================
  /// <summary>
  /// This abstract class defines a System.Decimal constant.
  /// </summary>
  // ================================================================================================
  public class DecimalNode : RealConstantNode
  {
    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Initializes a new instance of the <see cref="DecimalNode"/> class.
    /// </summary>
    /// <param name="start">The start token.</param>
    /// <param name="value">The value of the constant.</param>
    // ----------------------------------------------------------------------------------------------
    public DecimalNode(Token start, decimal value)
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
  }
}