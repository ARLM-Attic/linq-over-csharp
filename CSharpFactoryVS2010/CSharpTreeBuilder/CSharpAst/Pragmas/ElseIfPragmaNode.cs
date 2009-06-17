// ================================================================================================
// ElseIfPragmaNode.cs
//
// Created: 2009.06.16, by Istvan Novak (DeepDiver)
// ================================================================================================
using CSharpTreeBuilder.CSharpAstBuilder;

namespace CSharpTreeBuilder.Ast
{
  // ================================================================================================
  /// <summary>
  /// This class represents an "#elif" pragma node.
  /// </summary>
  // ================================================================================================
  public class ElseIfPragmaNode : ConditionalPragmaNode
  {
    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Initializes a new instance of the <see cref="ElseIfPragmaNode"/> class.
    /// </summary>
    /// <param name="start">Token providing information about the element.</param>
    // ----------------------------------------------------------------------------------------------
    public ElseIfPragmaNode(Token start)
      : base(start)
    {
    }
  }
}