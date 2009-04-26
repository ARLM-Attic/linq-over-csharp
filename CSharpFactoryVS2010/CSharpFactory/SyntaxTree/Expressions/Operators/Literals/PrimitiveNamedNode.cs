// ================================================================================================
// PrimitiveNamedNode.cs
//
// Created: 2009.04.16, by Istvan Novak (DeepDiver)
// ================================================================================================
using CSharpFactory.ParserFiles;

namespace CSharpFactory.Syntax
{
  // ================================================================================================
  /// <summary>
  /// This class defines a primitive type node.
  /// </summary>
  // ================================================================================================
  public class PrimitiveNamedNode : SimpleNameNode
  {
    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Initializes a new instance of the <see cref="PrimitiveNamedNode"/> class.
    /// </summary>
    /// <param name="start">Token providing information about the element.</param>
    // ----------------------------------------------------------------------------------------------
    public PrimitiveNamedNode(Token start)
      : base(start)
    {
    }
  }
}