// ================================================================================================
// NewOperatorWithImplicitArrayNode.cs
//
// Created: 2009.05.13, by Istvan Novak (DeepDiver)
// ================================================================================================
using CSharpTreeBuilder.CSharpAstBuilder;

namespace CSharpTreeBuilder.Ast
{
  // ================================================================================================
  /// <summary>
  /// This class represents a "new" operator with implicit array declaration.
  /// </summary>
  // ================================================================================================
  public sealed class NewOperatorWithImplicitArrayNode : NewOperatorWithArrayNodeBase
  {
    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Initializes a new instance of the <see cref="NewOperatorWithImplicitArrayNode"/> class.
    /// </summary>
    /// <param name="start">Token providing information about the element.</param>
    // ----------------------------------------------------------------------------------------------
    public NewOperatorWithImplicitArrayNode(Token start)
      : base(start)
    {
    }
  }
}