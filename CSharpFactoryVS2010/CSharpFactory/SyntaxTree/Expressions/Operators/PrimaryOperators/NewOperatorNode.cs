// ================================================================================================
// NewOperatorNode.cs
//
// Created: 2009.05.13, by Istvan Novak (DeepDiver)
// ================================================================================================
using CSharpFactory.ParserFiles;

namespace CSharpTreeBuilder.Ast
{
  // ================================================================================================
  /// <summary>
  /// This abstract class is intended to be the base class of all kinds of a "new" operator.
  /// </summary>
  // ================================================================================================
  public abstract class NewOperatorNode : PrimaryOperatorNode
  {
    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Initializes a new instance of the <see cref="NewOperatorNode"/> class.
    /// </summary>
    /// <param name="start">Token providing information about the element.</param>
    // ----------------------------------------------------------------------------------------------
    protected NewOperatorNode(Token start)
      : base(start)
    {
    }
  }
}