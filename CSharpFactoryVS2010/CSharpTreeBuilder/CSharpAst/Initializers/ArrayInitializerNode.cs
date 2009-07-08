// ================================================================================================
// ArrayInitializerNode.cs
//
// Created: 2009.05.11, by Istvan Novak (DeepDiver)
// ================================================================================================
using CSharpTreeBuilder.CSharpAstBuilder;

namespace CSharpTreeBuilder.Ast
{
  // ================================================================================================
  /// <summary>
  /// This class represents an array initializer.
  /// </summary>
  // ================================================================================================
  public sealed class ArrayInitializerNode : VariableInitializerNode
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
      VariableInitializers = new VariableInitializerNodeCollection();
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets the items of this initializer.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public VariableInitializerNodeCollection VariableInitializers { get; private set; }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets or sets the orphan comma token.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public Token OrphanComma { get; internal set; }
  }
}