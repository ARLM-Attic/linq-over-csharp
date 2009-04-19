// ================================================================================================
// PostDecrementNode.cs
//
// Created: 2009.04.19, by Istvan Novak (DeepDiver)
// ================================================================================================
using CSharpFactory.ParserFiles;

namespace CSharpFactory.Syntax
{
  // ================================================================================================
  /// <summary>
  /// This class represents a postdecrement (--) operator.
  /// </summary>
  // ================================================================================================
  public sealed class PostDecrementNode : UnaryOperatorNode
  {
    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Initializes a new instance of the <see cref="PostDecrementNode"/> class.
    /// </summary>
    /// <param name="start">Token providing information about the element.</param>
    // ----------------------------------------------------------------------------------------------
    public PostDecrementNode(Token start)
      : base(start)
    {
    }
  }
}