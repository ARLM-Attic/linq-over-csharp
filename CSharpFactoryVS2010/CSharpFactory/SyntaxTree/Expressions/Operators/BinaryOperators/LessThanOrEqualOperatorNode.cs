// ================================================================================================
// LessThanOrEqualOperatorNode.cs
//
// Created: 2009.04.30, by Istvan Novak (DeepDiver)
// ================================================================================================
using CSharpFactory.ParserFiles;

namespace CSharpFactory.Syntax
{
  // ================================================================================================
  /// <summary>
  /// This class defines a less than or equal operator node.
  /// </summary>
  // ================================================================================================
  public class LessThanOrEqualOperatorNode : BinaryOperatorNode
  {
    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Initializes a new instance of the <see cref="LessThanOrEqualOperatorNode"/> class.
    /// </summary>
    /// <param name="start">Token providing information about the element.</param>
    // ----------------------------------------------------------------------------------------------
    public LessThanOrEqualOperatorNode(Token start)
      : base(start)
    {
    }
  }
}