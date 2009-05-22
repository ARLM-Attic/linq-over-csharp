// ================================================================================================
// ArrayItemInitializerNode.cs
//
// Created: 2009.05.11, by Istvan Novak (DeepDiver)
// ================================================================================================
using CSharpTreeBuilder.CSharpAstBuilder;

namespace CSharpTreeBuilder.Ast
{
  // ================================================================================================
  /// <summary>
  /// This class represents an item of an array initializer.
  /// </summary>
  // ================================================================================================
  public class ArrayItemInitializerNode : SyntaxNode
  {
    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Initializes a new instance of the <see cref="ArrayItemInitializerNode"/> class.
    /// </summary>
    /// <param name="initializer">The initializer.</param>
    // ----------------------------------------------------------------------------------------------
    public ArrayItemInitializerNode(VariableInitializerNode initializer) :
      base(initializer.StartToken)
    {
      Initializer = initializer;
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets the initializer of this item.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public VariableInitializerNode Initializer { get; private set; }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets or sets the optional separator following the initializer.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public Token Separator { get; internal set; }
  }
}