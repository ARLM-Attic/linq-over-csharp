// ================================================================================================
// ThisNode.cs
//
// Created: 2009.04.15, by Istvan Novak (DeepDiver)
// ================================================================================================
using CSharpTreeBuilder.CSharpAstBuilder;

namespace CSharpTreeBuilder.Ast
{
  // ================================================================================================
  /// <summary>
  /// This class defines a "this" access expression.
  /// </summary>
  // ================================================================================================
  public sealed class ThisAccessNode : PrimaryOperatorNode
  {
    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Initializes a new instance of the <see cref="ThisAccessNode"/> class.
    /// </summary>
    /// <param name="start">Token providing information about the element.</param>
    // ----------------------------------------------------------------------------------------------
    public ThisAccessNode(Token start)
      : base(start)
    {
    }
  }
}