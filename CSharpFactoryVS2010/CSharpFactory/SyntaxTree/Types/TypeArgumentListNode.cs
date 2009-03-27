// ================================================================================================
// TypeArgumentListNode.cs
//
// Created: 2009.03.14, by Istvan Novak (DeepDiver)
// ================================================================================================
using CSharpFactory.ParserFiles;

namespace CSharpFactory.Syntax
{
  // ================================================================================================
  /// <summary>
  /// This node describes the argument list of a type.
  /// </summary>
  // ================================================================================================
  public class TypeArgumentListNode : SyntaxNode
  {
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
  }

  // ================================================================================================
  /// <summary>
  /// Describes a type argument
  /// </summary>
  // ================================================================================================
  public class TypeArgument : SyntaxNode
  {
    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Initializes a new instance of the <see cref="TypeArgument"/> class.
    /// </summary>
    /// <param name="start">Token providing information about the element.</param>
    // ----------------------------------------------------------------------------------------------
    public TypeArgument(Token start)
      : base(start)
    {
    }
  }
}