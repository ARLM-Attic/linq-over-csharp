// ================================================================================================
// RightShiftAssignmentOperatorNode.cs
//
// Created: 2009.05.06, by Istvan Novak (DeepDiver)
// ================================================================================================
using CSharpFactory.ParserFiles;

namespace CSharpFactory.Syntax
{
  // ================================================================================================
  /// <summary>
  /// This class defines a left shift assignment operator node.
  /// </summary>
  // ================================================================================================
  public class RightShiftAssignmentOperatorNode : AssignmentOperatorNodeBase
  {
    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Initializes a new instance of the <see cref="RightShiftAssignmentOperatorNode"/> class.
    /// </summary>
    /// <param name="start">Token providing information about the element.</param>
    /// <param name="secondAngle">The second angle bracket.</param>
    // ----------------------------------------------------------------------------------------------
    public RightShiftAssignmentOperatorNode(Token start, Token secondAngle)
      : base(start)
    {
      SecondAngleToken = secondAngle;
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets the second angle bracket token of this operator.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public Token SecondAngleToken { get; private set; }
  }
}