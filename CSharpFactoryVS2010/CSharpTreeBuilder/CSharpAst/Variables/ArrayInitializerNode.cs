// ================================================================================================
// ArrayInitializerNode.cs
//
// Created: 2009.05.11, by Istvan Novak (DeepDiver)
// ================================================================================================
using CSharpTreeBuilder.CSharpAstBuilder;

namespace CSharpFactory.Syntax
{
  // ================================================================================================
  /// <summary>
  /// This class represents an array initializer.
  /// </summary>
  // ================================================================================================
  public class ArrayInitializerNode : VariableInitializerNode
  {
    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Initializes a new instance of the <see cref="ArrayInitializerNode"/> class.
    /// </summary>
    /// <param name="start">Token providing information about the element.</param>
    // ----------------------------------------------------------------------------------------------
    public ArrayInitializerNode(Token start)
      : base(start)
    {
      Items = new ArrayItemInitializerNodeCollection();
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets the items of this initializer.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public ArrayItemInitializerNodeCollection Items { get; private set; }
  }
}