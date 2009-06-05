// ================================================================================================
// DelegateDeclarationNode.cs
//
// Created: 2009.04.07, by Istvan Novak (DeepDiver)
// ================================================================================================
using CSharpTreeBuilder.CSharpAstBuilder;

namespace CSharpTreeBuilder.Ast
{
  // ================================================================================================
  /// <summary>
  /// Represents a delegate declaration.
  /// </summary>
  // ================================================================================================
  public class DelegateDeclarationNode : TypeDeclarationNode
  {
    // --- Backing fields
    private TypeOrNamespaceNode _TypeName;

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Initializes a new instance of the <see cref="DelegateDeclarationNode"/> class.
    /// </summary>
    /// <param name="start">The start token of the declaration.</param>
    /// <param name="name">The name of the delcaration.</param>
    // ----------------------------------------------------------------------------------------------
    public DelegateDeclarationNode(Token start, Token name)
      : base(start, name)
    {
      IdentifierToken = name;
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets or sets the name of the type.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public TypeOrNamespaceNode TypeName
    {
      get { return _TypeName; }
      internal set
      {
        _TypeName = value;
        if (_TypeName != null) _TypeName.ParentNode = this;
      }
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets or sets the formal parameters.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public FormalParameterListNode FormalParameters { get; internal set; }
  }
}