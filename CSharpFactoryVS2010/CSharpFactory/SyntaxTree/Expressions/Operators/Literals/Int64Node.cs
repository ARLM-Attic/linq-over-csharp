// ================================================================================================
// Int64Node.cs
//
// Created: 2009.04.15, by Istvan Novak (DeepDiver)
// ================================================================================================
using CSharpFactory.ParserFiles;

namespace CSharpTreeBuilder.Ast
{
  // ================================================================================================
  /// <summary>
  /// This abstract class defines a System.Int64 constant.
  /// </summary>
  // ================================================================================================
  public class Int64Node : IntegerConstantNode
  {
    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Initializes a new instance of the <see cref="Int64Node"/> class.
    /// </summary>
    /// <param name="start">The start token.</param>
    /// <param name="value">The value of the constant.</param>
    // ----------------------------------------------------------------------------------------------
    public Int64Node(Token start, long value)
      : base(start)
    {
      Value = value;
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets the value of the constant.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public long Value { get; internal set; }   
  }
}