// ================================================================================================
// CTypeMemberAccessOperatorNode.cs
//
// Created: 2009.05.11, by Istvan Novak (DeepDiver)
// ================================================================================================
using CSharpTreeBuilder.CSharpAstBuilder;

namespace CSharpTreeBuilder.Ast
{
  // ================================================================================================
  /// <summary>
  /// This type represents a C-type member access operator ("->").
  /// </summary>
  // ================================================================================================
  public class CTypeMemberAccessOperatorNode : MemberAccessOperatorNodeBase
  {
    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Initializes a new instance of the <see cref="CTypeMemberAccessOperatorNode"/> class.
    /// </summary>
    /// <param name="start">Token providing information about the element.</param>
    // ----------------------------------------------------------------------------------------------
    public CTypeMemberAccessOperatorNode(Token start)
      : base(start)
    {
    }
  }
}