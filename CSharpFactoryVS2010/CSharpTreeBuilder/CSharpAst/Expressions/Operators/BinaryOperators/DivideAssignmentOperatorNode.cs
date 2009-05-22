// ================================================================================================
// DivideAssignmentOperatorNode.cs
//
// Created: 2009.05.06, by Istvan Novak (DeepDiver)
// ================================================================================================
using CSharpTreeBuilder.CSharpAstBuilder;

namespace CSharpTreeBuilder.Ast
{
  // ================================================================================================
  /// <summary>
  /// This class defines a divide assignment ("/=") operator node.
  /// </summary>
  // ================================================================================================
  public class DivideAssignmentOperatorNode : AssignmentOperatorNodeBase
  {
    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Initializes a new instance of the <see cref="DivideAssignmentOperatorNode"/> class.
    /// </summary>
    /// <param name="start">Token providing information about the element.</param>
    // ----------------------------------------------------------------------------------------------
    public DivideAssignmentOperatorNode(Token start)
      : base(start)
    {
    }
  }
}