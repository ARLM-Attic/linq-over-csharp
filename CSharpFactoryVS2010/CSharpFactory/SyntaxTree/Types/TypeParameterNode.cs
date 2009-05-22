// ================================================================================================
// TypeParameters.cs
//
// Created: 2009.04.07, by Istvan Novak (DeepDiver)
// ================================================================================================
using CSharpFactory.ParserFiles;

namespace CSharpTreeBuilder.Ast
{
  // ================================================================================================
  /// <summary>
  /// This node represents a type parameter with its attributes.
  /// </summary>
  // ================================================================================================
  public class TypeParameterNode : NameTagNode, IAttributedDeclaration
  {
    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Initializes a new instance of the <see cref="TypeParameterNode"/> class.
    /// </summary>
    /// <param name="start">Token providing information about the element.</param>
    /// <param name="attrNodes">Attributes of the type parameter.</param>
    // ----------------------------------------------------------------------------------------------
    public TypeParameterNode(Token start, AttributeDecorationNodeCollection attrNodes)
      : base(start)
    {
      AttributeDecorations = attrNodes;
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets the attribute decorations belonging to this declaration.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public AttributeDecorationNodeCollection AttributeDecorations { get; internal set; }
  }
}