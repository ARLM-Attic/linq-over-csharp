// ================================================================================================
// PragmaPragmaNode.cs
//
// Created: 2009.06.17, by Istvan Novak (DeepDiver)
// ================================================================================================
using CSharpTreeBuilder.CSharpAstBuilder;

namespace CSharpTreeBuilder.Ast
{
  // ================================================================================================
  /// <summary>
  /// This class represents the "#pragma" pragma node.
  /// </summary>
  // ================================================================================================
  public class PragmaPragmaNode : PragmaNode
  {
    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Initializes a new instance of the <see cref="PragmaPragmaNode"/> class.
    /// </summary>
    /// <param name="start">Token providing information about the element.</param>
    // ----------------------------------------------------------------------------------------------
    public PragmaPragmaNode(Token start)
      : base(start)
    {
    }
  }
}