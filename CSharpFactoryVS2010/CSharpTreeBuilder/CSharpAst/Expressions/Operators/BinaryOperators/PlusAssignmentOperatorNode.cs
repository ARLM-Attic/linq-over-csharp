// ================================================================================================
// PlusAssignmentOperatorNode.cs
//
// Created: 2009.05.06, by Istvan Novak (DeepDiver)
// ================================================================================================
using CSharpTreeBuilder.CSharpAstBuilder;

namespace CSharpTreeBuilder.Ast
{
  // ================================================================================================
  /// <summary>
  /// This class defines a plus assignment ("+=") operator node.
  /// </summary>
  // ================================================================================================
  public class PlusAssignmentOperatorNode : AssignmentOperatorNodeBase
  {
    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Initializes a new instance of the <see cref="PlusAssignmentOperatorNode"/> class.
    /// </summary>
    /// <param name="start">Token providing information about the element.</param>
    // ----------------------------------------------------------------------------------------------
    public PlusAssignmentOperatorNode(Token start)
      : base(start)
    {
    }
  }
}