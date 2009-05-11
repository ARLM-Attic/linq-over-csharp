// ================================================================================================
// TypeArgumentListNode.cs
//
// Created: 2009.03.14, by Istvan Novak (DeepDiver)
// ================================================================================================
using CSharpFactory.Collections;
using CSharpFactory.ParserFiles;

namespace CSharpFactory.Syntax
{
  // ================================================================================================
  /// <summary>
  /// This node describes the argument list of a type.
  /// </summary>
  /// <remarks>
  /// Opening and closing angle brackets are represented by the start and terminating tokens, 
  /// respectively.
  /// </remarks>
  // ================================================================================================
  public class TypeArgumentListNode : SyntaxNode
  {
    private ImmutableCollection<TypeArgumentTag> _Args = 
      new ImmutableCollection<TypeArgumentTag>();

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Initializes a new instance of the <see cref="TypeArgumentListNode"/> class.
    /// </summary>
    /// <param name="start">Token providing information about the element.</param>
    // ----------------------------------------------------------------------------------------------
    public TypeArgumentListNode(Token start)
      : base(start)
    {
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets or sets the count.
    /// </summary>
    /// <value>The count.</value>
    // ----------------------------------------------------------------------------------------------
    public int Count { get; private set; }

    internal void AddArgument(TypeArgumentTag tag)
    {
      
    }

    internal void AddArgumentContinuation(Token separator, TypeArgumentTag tag)
    {

    }
  }
}