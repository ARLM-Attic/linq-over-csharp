// ================================================================================================
// TypeOperatorNode.cs
//
// Created: 2009.05.01, by Istvan Novak (DeepDiver)
// ================================================================================================
namespace CSharpTreeBuilder.Ast
{
  // ================================================================================================
  /// <summary>
  /// This class represents an operator holding a type definition.
  /// </summary>
  // ================================================================================================
  public sealed class TypeOperatorNode : LiteralNode
  {
    // --- Backing fields
    private TypeOrNamespaceNode _TypeName;

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
    public TypeOrNamespaceNode TypeName
    {
      get { return _TypeName; }
      private set
      {
        _TypeName = value;
        if (_TypeName != null) _TypeName.ParentNode = this;
      }
    }
  }
}