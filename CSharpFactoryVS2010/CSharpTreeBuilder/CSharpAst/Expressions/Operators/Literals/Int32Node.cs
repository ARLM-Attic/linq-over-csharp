// ================================================================================================
// Int32Node.cs
//
// Created: 2009.04.15, by Istvan Novak (DeepDiver)
// ================================================================================================
using CSharpTreeBuilder.CSharpAstBuilder;

namespace CSharpTreeBuilder.Ast
{
  // ================================================================================================
  /// <summary>
  /// This abstract class defines a System.Int32 constant.
  /// </summary>
  // ================================================================================================
  public class Int32Node : IntegerConstantNode
  {
    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Initializes a new instance of the <see cref="Int32Node"/> class.
    /// </summary>
    /// <param name="start">The start token.</param>
    /// <param name="value">The value of the constant.</param>
    // ----------------------------------------------------------------------------------------------
    public Int32Node(Token start, int value)
      : base(start)
    {
      Value = value;
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets the value of the constant.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public int Value { get; internal set; }
  }
}