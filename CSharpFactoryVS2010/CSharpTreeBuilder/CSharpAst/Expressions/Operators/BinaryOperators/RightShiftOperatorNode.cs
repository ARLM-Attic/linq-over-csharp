// ================================================================================================
// RightShiftOperatorNode.cs
//
// Created: 2009.04.30, by Istvan Novak (DeepDiver)
// ================================================================================================
using CSharpTreeBuilder.CSharpAstBuilder;

namespace CSharpTreeBuilder.Ast
{
  // ================================================================================================
  /// <summary>
  /// This class defines a left shift operator node.
  /// </summary>
  // ================================================================================================
  public class RightShiftOperatorNode : BinaryOperatorNode
  {
    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Initializes a new instance of the <see cref="RightShiftOperatorNode"/> class.
    /// </summary>
    /// <param name="start">Token providing information about the element.</param>
    // ----------------------------------------------------------------------------------------------
    public RightShiftOperatorNode(Token start, Token secondAngle)
      : base(start)
    {
      SecondAngleToken = secondAngle;
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets the second angle bracket token of this operator.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public Token SecondAngleToken { get; private set; }
  }
}