// ================================================================================================
// ElsePragmaNode.cs
//
// Created: 2009.06.16, by Istvan Novak (DeepDiver)
// ================================================================================================
using CSharpTreeBuilder.CSharpAstBuilder;

namespace CSharpTreeBuilder.Ast
{
  // ================================================================================================
  /// <summary>
  /// This class represents an "#else" pragma node.
  /// </summary>
  // ================================================================================================
  public class ElsePragmaNode : ConditionalPragmaNode
  {
    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Initializes a new instance of the <see cref="ElsePragmaNode"/> class.
    /// </summary>
    /// <param name="start">Token providing information about the element.</param>
    // ----------------------------------------------------------------------------------------------
    public ElsePragmaNode(Token start)
      : base(start)
    {
    }
  }
}