// ================================================================================================
// DefinePragmaNode.cs
//
// Created: 2009.06.16, by Istvan Novak (DeepDiver)
// ================================================================================================
using CSharpTreeBuilder.CSharpAstBuilder;

namespace CSharpTreeBuilder.Ast
{
  // ================================================================================================
  /// <summary>
  /// This class represents a "#define" pragma node.
  /// </summary>
  // ================================================================================================
  public class DefinePragmaNode : PragmaNode
  {
    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Initializes a new instance of the <see cref="DefinePragmaNode"/> class.
    /// </summary>
    /// <param name="start">Token providing information about the element.</param>
    // ----------------------------------------------------------------------------------------------
    public DefinePragmaNode(Token start)
      : base(start)
    {
    }
  }
}