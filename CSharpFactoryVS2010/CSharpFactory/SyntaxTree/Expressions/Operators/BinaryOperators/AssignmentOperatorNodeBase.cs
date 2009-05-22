// ================================================================================================
// AssignmentOperatorNodeBase.cs
//
// Created: 2009.05.06, by Istvan Novak (DeepDiver)
// ================================================================================================
using CSharpFactory.ParserFiles;

namespace CSharpTreeBuilder.Ast
{
  // ================================================================================================
  /// <summary>
  /// This class is intended to be the base class of all assignment operator nodes.
  /// </summary>
  // ================================================================================================
  public abstract class AssignmentOperatorNodeBase : BinaryOperatorNode
  {
    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Initializes a new instance of the <see cref="AssignmentOperatorNodeBase"/> class.
    /// </summary>
    /// <param name="start">Token providing information about the element.</param>
    // ----------------------------------------------------------------------------------------------
    protected AssignmentOperatorNodeBase(Token start)
      : base(start)
    {
    }
  }
}