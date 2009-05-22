// ================================================================================================
// PreIncrementOperatorNode.cs
//
// Created: 2009.04.15, by Istvan Novak (DeepDiver)
// ================================================================================================
using CSharpFactory.ParserFiles;

namespace CSharpTreeBuilder.Ast
{
  // ================================================================================================
  /// <summary>
  /// This class represents a preincrement (++) operator.
  /// </summary>
  // ================================================================================================
  public sealed class PreIncrementOperatorNode : UnaryOperatorNode
  {
    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Initializes a new instance of the <see cref="PreIncrementOperatorNode"/> class.
    /// </summary>
    /// <param name="start">Token providing information about the element.</param>
    // ----------------------------------------------------------------------------------------------
    public PreIncrementOperatorNode(Token start)
      : base(start)
    {
    }
  }
}