// ================================================================================================
// TypeOperatorNode.cs
//
// Created: 2009.05.01, by Istvan Novak (DeepDiver)
// ================================================================================================
using CSharpFactory.ParserFiles;

namespace CSharpFactory.Syntax
{
  // ================================================================================================
  /// <summary>
  /// This class represents an operator holding a type definition.
  /// </summary>
  // ================================================================================================
  public sealed class TypeOperatorNode : LiteralNode
  {
    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Initializes a new instance of the <see cref="LiteralNode"/> class.
    /// </summary>
    /// <param name="typeName">Type name.</param>
    // ----------------------------------------------------------------------------------------------
    public TypeOperatorNode(TypeOrNamespaceNode typeName) : base(typeName.StartToken)
    {
      TypeName = typeName;
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets the name of the type.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public TypeOrNamespaceNode TypeName { get; private set; }
  }
}