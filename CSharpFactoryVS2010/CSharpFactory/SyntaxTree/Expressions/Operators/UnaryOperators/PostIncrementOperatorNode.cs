// ================================================================================================
// PostIncrementOperatorNode.cs
//
// Created: 2009.04.19, by Istvan Novak (DeepDiver)
// ================================================================================================
using CSharpFactory.ParserFiles;

namespace CSharpTreeBuilder.Ast
{
  // ================================================================================================
  /// <summary>
  /// This class represents a postincrement (++) operator.
  /// </summary>
  // ================================================================================================
  public sealed class PostIncrementOperatorNode : UnaryOperatorNode
  {
    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Initializes a new instance of the <see cref="PostIncrementOperatorNode"/> class.
    /// </summary>
    /// <param name="start">Token providing information about the element.</param>
    // ----------------------------------------------------------------------------------------------
    public PostIncrementOperatorNode(Token start)
      : base(start)
    {
    }
  }
}