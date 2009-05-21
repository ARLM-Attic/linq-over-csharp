// ================================================================================================
// StackAllocInitializerNode.cs
//
// Created: 2009.05.06, by Istvan Novak (DeepDiver)
// ================================================================================================
using CSharpTreeBuilder.CSharpAstBuilder;

namespace CSharpFactory.Syntax
{
  // ================================================================================================
  /// <summary>
  /// This class represents a "stackalloc" initializer.
  /// </summary>
  // ================================================================================================
  public sealed class StackAllocInitializerNode : VariableInitializerNode
  {
    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Initializes a new instance of the <see cref="StackAllocInitializerNode"/> class.
    /// </summary>
    /// <param name="start">Token providing information about the element.</param>
    // ----------------------------------------------------------------------------------------------
    public StackAllocInitializerNode(Token start)
      : base(start)
    {
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets or sets the "stackalloc" token.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public Token StackAllocToken { get; internal set; }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets or sets the open square token.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public Token OpenSquareToken { get; internal set; }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets or sets the name of the type.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public TypeOrNamespaceNode TypeName { get; internal set; }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets or sets the close square token.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public Token CloseSquareToken { get; internal set; }
  }
}