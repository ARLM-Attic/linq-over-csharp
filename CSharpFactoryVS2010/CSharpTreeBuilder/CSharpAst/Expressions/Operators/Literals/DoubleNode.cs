// ================================================================================================
// DoubleNode.cs
//
// Created: 2009.04.15, by Istvan Novak (DeepDiver)
// ================================================================================================
using CSharpTreeBuilder.CSharpAstBuilder;

namespace CSharpTreeBuilder.Ast
{
  // ================================================================================================
  /// <summary>
  /// This abstract class defines a System.Double constant.
  /// </summary>
  // ================================================================================================
  public class DoubleNode : RealConstantNode
  {
    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Initializes a new instance of the <see cref="DoubleNode"/> class.
    /// </summary>
    /// <param name="start">The start token.</param>
    /// <param name="value">The value of the constant.</param>
    // ----------------------------------------------------------------------------------------------
    public DoubleNode(Token start, double value)
      : base(start)
    {
      Value = value;
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets the value of the constant.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public double Value { get; internal set; }
  }
}