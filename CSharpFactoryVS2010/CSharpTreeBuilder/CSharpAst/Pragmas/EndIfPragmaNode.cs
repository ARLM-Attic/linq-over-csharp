// ================================================================================================
// EndIfPragmaNode.cs
//
// Created: 2009.06.17, by Istvan Novak (DeepDiver)
// ================================================================================================
using CSharpTreeBuilder.CSharpAstBuilder;

namespace CSharpTreeBuilder.Ast
{
  // ================================================================================================
  /// <summary>
  /// This class represents the "#endif" pragma node.
  /// </summary>
  // ================================================================================================
  public class EndIfPragmaNode : PragmaNode
  {
    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Initializes a new instance of the <see cref="EndIfPragmaNode"/> class.
    /// </summary>
    /// <param name="start">Token providing information about the element.</param>
    // ----------------------------------------------------------------------------------------------
    public EndIfPragmaNode(Token start)
      : base(start)
    {
    }
  }
}