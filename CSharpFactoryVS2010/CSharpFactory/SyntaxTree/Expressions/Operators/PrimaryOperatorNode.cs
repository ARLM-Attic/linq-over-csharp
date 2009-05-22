// ================================================================================================
// PrimaryOperatorNode.cs
//
// Created: 2009.05.11, by Istvan Novak (DeepDiver)
// ================================================================================================
using CSharpFactory.ParserFiles;

namespace CSharpTreeBuilder.Ast
{
  // ================================================================================================
  /// <summary>
  /// Common root class of all primary operator nodes.
  /// </summary>
  // ================================================================================================
  public abstract class PrimaryOperatorNode : OperatorNode
  {
    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Initializes a new instance of the <see cref="PrimaryOperatorNode"/> class.
    /// </summary>
    /// <param name="start">Token providing information about the element.</param>
    // ----------------------------------------------------------------------------------------------
    protected PrimaryOperatorNode(Token start)
      : base(start)
    {
    }
  }
}