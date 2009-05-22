// ================================================================================================
// VariableInitializerNode.cs
//
// Created: 2009.05.06, by Istvan Novak (DeepDiver)
// ================================================================================================
using CSharpFactory.ParserFiles;

namespace CSharpTreeBuilder.Ast
{
  // ================================================================================================
  /// <summary>
  /// This abstract class is intended to be the root of all variable initializers.
  /// </summary>
  /// <remarks>
  /// The start token of the initializer is the "=" token.
  /// </remarks>
  // ================================================================================================
  public abstract class VariableInitializerNode : SyntaxNode
  {
    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Initializes a new instance of the <see cref="VariableInitializerNode"/> class.
    /// </summary>
    /// <param name="start">Token providing information about the element.</param>
    // ----------------------------------------------------------------------------------------------
    protected VariableInitializerNode(Token start)
      : base(start)
    {
    }
  }
}