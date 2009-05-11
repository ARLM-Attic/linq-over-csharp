// ================================================================================================
// MethodInvocationOperatorNode.cs
//
// Created: 2009.05.11, by Istvan Novak (DeepDiver)
// ================================================================================================
using CSharpFactory.ParserFiles;

namespace CSharpFactory.Syntax
{
  // ================================================================================================
  /// <summary>
  /// This class represents a method invocation operator node.
  /// </summary>
  // ================================================================================================
  public sealed class MethodInvocationOperatorNode : InvocationOperatorNode
  {
    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Initializes a new instance of the <see cref="MethodInvocationOperatorNode"/> class.
    /// </summary>
    /// <param name="start">Token providing information about the element.</param>
    // ----------------------------------------------------------------------------------------------
    public MethodInvocationOperatorNode(Token start)
      : base(start)
    {
    }
  }
}