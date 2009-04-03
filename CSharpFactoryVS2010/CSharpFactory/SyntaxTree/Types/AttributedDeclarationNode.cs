// ================================================================================================
// AttributedDeclarationNode.cs
//
// Created: 2009.04.03, by Istvan Novak (DeepDiver)
// ================================================================================================
using CSharpFactory.ParserFiles;

namespace CSharpFactory.Syntax
{
  // ================================================================================================
  /// <summary>
  /// This class is intended to be a common base class of declarations holding attributes
  /// </summary>
  // ================================================================================================
  public abstract class AttributedDeclarationNode : SyntaxNode
  {
    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Initializes a new instance of the <see cref="AttributedDeclarationNode"/> class.
    /// </summary>
    /// <param name="parent">The parent node.</param>
    /// <param name="start">Token providing information about the element.</param>
    // ----------------------------------------------------------------------------------------------
    protected AttributedDeclarationNode(AttributedDeclarationNode parent, Token start)
      : base(start)
    {
      Parent = parent;
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets the parent of this node.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public AttributedDeclarationNode Parent { get; private set; }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets the attribute decorations belonging to this declaration.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public AttributeDecorationNodeCollection AttributeDecorations { get; internal set; }
  }
}