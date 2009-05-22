// ================================================================================================
// AttributedDeclarationNode.cs
//
// Created: 2009.04.03, by Istvan Novak (DeepDiver)
// ================================================================================================
using CSharpFactory.ParserFiles;

namespace CSharpTreeBuilder.Ast
{
  // ================================================================================================
  /// <summary>
  /// This class is intended to be a common base class of declarations holding attributes
  /// </summary>
  // ================================================================================================
  public abstract class AttributedDeclarationNode : SyntaxNode, IAttributedDeclaration
  {
    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Initializes a new instance of the <see cref="AttributedDeclarationNode"/> class.
    /// </summary>
    /// <param name="start">Token providing information about the element.</param>
    // ----------------------------------------------------------------------------------------------
    protected AttributedDeclarationNode(Token start)
      : base(start)
    {
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets the attribute decorations belonging to this declaration.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public AttributeDecorationNodeCollection AttributeDecorations { get; internal set; }
  }
}