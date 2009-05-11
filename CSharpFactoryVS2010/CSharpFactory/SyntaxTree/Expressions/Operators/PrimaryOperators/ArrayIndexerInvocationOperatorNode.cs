// ================================================================================================
// ArrayIndexerInvocationOperatorNode.cs
//
// Created: 2009.05.11, by Istvan Novak (DeepDiver)
// ================================================================================================
using CSharpFactory.ParserFiles;

namespace CSharpFactory.Syntax
{
  // ================================================================================================
  /// <summary>
  /// This class represents an array indexer invocation operator node.
  /// </summary>
  // ================================================================================================
  public sealed class ArrayIndexerInvocationOperatorNode : InvocationOperatorNode
  {
    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Initializes a new instance of the <see cref="ArrayIndexerInvocationOperatorNode"/> class.
    /// </summary>
    /// <param name="start">Token providing information about the element.</param>
    // ----------------------------------------------------------------------------------------------
    public ArrayIndexerInvocationOperatorNode(Token start)
      : base(start)
    {
    }
  }
}