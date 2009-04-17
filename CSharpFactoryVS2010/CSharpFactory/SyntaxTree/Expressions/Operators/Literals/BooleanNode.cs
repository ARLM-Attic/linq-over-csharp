// ================================================================================================
// BooleanNode.cs
//
// Created: 2009.04.15, by Istvan Novak (DeepDiver)
// ================================================================================================
using CSharpFactory.ParserFiles;

namespace CSharpFactory.Syntax
{
  // ================================================================================================
  /// <summary>
  /// This class defines a boolean literal node.
  /// </summary>
  // ================================================================================================
  public abstract class BooleanNode : LiteralNode
  {
    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Initializes a new instance of the <see cref="BooleanNode"/> class.
    /// </summary>
    /// <param name="start">Token providing information about the element.</param>
    // ----------------------------------------------------------------------------------------------
    protected BooleanNode(Token start)
      : base(start)
    {
    }
  }

  // ================================================================================================
  /// <summary>
  /// This class defines a "true" boolean literal node.
  /// </summary>
  // ================================================================================================
  public sealed class TrueNode : BooleanNode
  {
    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Initializes a new instance of the <see cref="BooleanNode"/> class.
    /// </summary>
    /// <param name="start">Token providing information about the element.</param>
    // ----------------------------------------------------------------------------------------------
    public TrueNode(Token start)
      : base(start)
    {
    }
  }

  // ================================================================================================
  /// <summary>
  /// This class defines a "false" boolean literal node.
  /// </summary>
  // ================================================================================================
  public sealed class FalseNode : BooleanNode
  {
    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Initializes a new instance of the <see cref="BooleanNode"/> class.
    /// </summary>
    /// <param name="start">Token providing information about the element.</param>
    // ----------------------------------------------------------------------------------------------
    public FalseNode(Token start)
      : base(start)
    {
    }
  }
}