// ================================================================================================
// AndAssignmentOperatorNode.cs
//
// Created: 2009.05.06, by Istvan Novak (DeepDiver)
// ================================================================================================
using CSharpFactory.ParserFiles;

namespace CSharpTreeBuilder.Ast
{
  // ================================================================================================
  /// <summary>
  /// This class defines an and assignment ("&=") operator node.
  /// </summary>
  // ================================================================================================
  public class AndAssignmentOperatorNode : AssignmentOperatorNodeBase
  {
    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Initializes a new instance of the <see cref="AndAssignmentOperatorNode"/> class.
    /// </summary>
    /// <param name="start">Token providing information about the element.</param>
    // ----------------------------------------------------------------------------------------------
    public AndAssignmentOperatorNode(Token start)
      : base(start)
    {
    }
  }
}