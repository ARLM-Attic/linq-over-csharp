// ================================================================================================
// MultiplyOperatorNode.cs
//
// Created: 2009.04.27, by Istvan Novak (DeepDiver)
// ================================================================================================
using CSharpFactory.ParserFiles;

namespace CSharpFactory.Syntax
{
  // ================================================================================================
  /// <summary>
  /// This class defines a multiplication ("*") operator node.
  /// </summary>
  // ================================================================================================
  public class MultiplyOperatorNode : BinaryOperatorNode
  {
    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Initializes a new instance of the <see cref="MultiplyOperatorNode"/> class.
    /// </summary>
    /// <param name="start">Token providing information about the element.</param>
    // ----------------------------------------------------------------------------------------------
    public MultiplyOperatorNode(Token start)
      : base(start)
    {
    }
  }
}