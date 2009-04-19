// ================================================================================================
// PostIncrementNode.cs
//
// Created: 2009.04.19, by Istvan Novak (DeepDiver)
// ================================================================================================
using CSharpFactory.ParserFiles;

namespace CSharpFactory.Syntax
{
  // ================================================================================================
  /// <summary>
  /// This class represents a postincrement (++) operator.
  /// </summary>
  // ================================================================================================
  public sealed class PostIncrementNode : UnaryOperatorNode
  {
    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Initializes a new instance of the <see cref="PostIncrementNode"/> class.
    /// </summary>
    /// <param name="start">Token providing information about the element.</param>
    // ----------------------------------------------------------------------------------------------
    public PostIncrementNode(Token start)
      : base(start)
    {
    }
  }
}