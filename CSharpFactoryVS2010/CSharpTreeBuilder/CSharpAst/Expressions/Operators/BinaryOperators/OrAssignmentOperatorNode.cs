// ================================================================================================
// OrAssignmentOperatorNode.cs
//
// Created: 2009.05.06, by Istvan Novak (DeepDiver)
// ================================================================================================
using CSharpTreeBuilder.CSharpAstBuilder;

namespace CSharpTreeBuilder.Ast
{
  // ================================================================================================
  /// <summary>
  /// This class defines an or assignment ("|=") operator node.
  /// </summary>
  // ================================================================================================
  public class OrAssignmentOperatorNode : AssignmentOperatorNodeBase
  {
    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Initializes a new instance of the <see cref="OrAssignmentOperatorNode"/> class.
    /// </summary>
    /// <param name="start">Token providing information about the element.</param>
    // ----------------------------------------------------------------------------------------------
    public OrAssignmentOperatorNode(Token start)
      : base(start)
    {
    }
  }
}