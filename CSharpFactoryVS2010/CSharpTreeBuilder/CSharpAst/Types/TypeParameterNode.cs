// ================================================================================================
// TypeParameters.cs
//
// Created: 2009.04.07, by Istvan Novak (DeepDiver)
// ================================================================================================
using CSharpTreeBuilder.CSharpAstBuilder;

namespace CSharpTreeBuilder.Ast
{
  // ================================================================================================
  /// <summary>
  /// This node represents a type parameter with its attributes.
  /// </summary>
  // ================================================================================================
  public class TypeParameterNode : NameTagNode, IAttributedDeclaration
  {
    // --- Backing fields
    private AttributeDecorationNodeCollection _AttributeDecorations;

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Initializes a new instance of the <see cref="TypeParameterNode"/> class.
    /// </summary>
    /// <param name="identifier">Token providing information about the element.</param>
    /// <param name="attrNodes">Attributes of the type parameter.</param>
    // ----------------------------------------------------------------------------------------------
    public TypeParameterNode(Token identifier, AttributeDecorationNodeCollection attrNodes)
      : base(identifier)
    {
      AttributeDecorations = attrNodes;
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Initializes a new instance of the <see cref="TypeParameterNode"/> class.
    /// </summary>
    /// <param name="separator">The separator token.</param>
    /// <param name="identifier">Token providing information about the element.</param>
    /// <param name="attrNodes">Attributes of the type parameter.</param>
    // ----------------------------------------------------------------------------------------------
    public TypeParameterNode(Token separator, Token identifier, AttributeDecorationNodeCollection attrNodes)
      : this(identifier, attrNodes)
    {
      SeparatorToken = separator;
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets the attribute decorations belonging to this declaration.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public AttributeDecorationNodeCollection AttributeDecorations
    {
      get { return _AttributeDecorations; }
      internal set
      {
        _AttributeDecorations = value;
        _AttributeDecorations.AssignToParent(this);
      }
    }

    #region Visitor methods

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Accepts a visitor object, according to the Visitor pattern.
    /// </summary>
    /// <param name="visitor">A visitor object</param>
    // ----------------------------------------------------------------------------------------------
    public override void AcceptVisitor(ISyntaxNodeVisitor visitor)
    {
      visitor.Visit(this);

      foreach (var attributeDecoration in AttributeDecorations)
      {
        attributeDecoration.AcceptVisitor(visitor);
      }
    }

    #endregion
  }
}