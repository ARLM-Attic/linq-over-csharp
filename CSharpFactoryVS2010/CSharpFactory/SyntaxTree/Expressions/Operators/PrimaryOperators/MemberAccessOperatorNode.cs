// ================================================================================================
// MemberAccessOperatorNode.cs
//
// Created: 2009.05.11, by Istvan Novak (DeepDiver)
// ================================================================================================
using CSharpFactory.ParserFiles;

namespace CSharpTreeBuilder.Ast
{
  // ================================================================================================
  /// <summary>
  /// This type represents a member access operator (".").
  /// </summary>
  // ================================================================================================
  public class MemberAccessOperatorNode : MemberAccessOperatorNodeBase
  {
    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Initializes a new instance of the <see cref="MemberAccessOperatorNode"/> class.
    /// </summary>
    /// <param name="start">Token providing information about the element.</param>
    // ----------------------------------------------------------------------------------------------
    public MemberAccessOperatorNode(Token start)
      : base(start)
    {
    }
  }
}