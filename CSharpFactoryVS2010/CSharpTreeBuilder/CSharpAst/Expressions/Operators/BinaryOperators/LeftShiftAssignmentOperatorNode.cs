// ================================================================================================
// LeftShiftAssignmentOperatorNode.cs
//
// Created: 2009.05.06, by Istvan Novak (DeepDiver)
// ================================================================================================
using CSharpTreeBuilder.CSharpAstBuilder;

namespace CSharpTreeBuilder.Ast
{
  // ================================================================================================
  /// <summary>
  /// This class defines a left shift assignment operator node.
  /// </summary>
  // ================================================================================================
  public class LeftShiftAssignmentOperatorNode : AssignmentOperatorNodeBase
  {
    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Initializes a new instance of the <see cref="LeftShiftAssignmentOperatorNode"/> class.
    /// </summary>
    /// <param name="start">Token providing information about the element.</param>
    // ----------------------------------------------------------------------------------------------
    public LeftShiftAssignmentOperatorNode(Token start)
      : base(start)
    {
    }
  }
}