// ================================================================================================
// FormalParameterNode.cs
//
// Created: 2009.05.12, by Istvan Novak (DeepDiver)
// ================================================================================================
using CSharpTreeBuilder.CSharpAstBuilder;

namespace CSharpTreeBuilder.Ast
{
  // ================================================================================================
  /// <summary>
  /// This class declares a formal parameter node on a formal parameter list.
  /// </summary>
  // ================================================================================================
  public class FormalParameterNode : AttributedDeclarationNode, IIdentifierSupport
  {
    // --- Backing fields
    private TypeNode _Type;

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Initializes a new instance of the <see cref="FormalParameterNode"/> class.
    /// </summary>
    /// <param name="start">Token providing information about the element.</param>
    // ----------------------------------------------------------------------------------------------
    public FormalParameterNode(Token start)
      : base(start)
    {
      Modifier = FormalParameterModifier.In;
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets or sets the formal parameter modifier.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public FormalParameterModifier Modifier { get; internal set; }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets or sets the name of the type.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public TypeNode Type
    {
      get { return _Type; }
      internal set
      {
        _Type = value;
        if (_Type != null) _Type.ParentNode = this;
      }
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets the identifier token.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public Token IdentifierToken { get; internal set; }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets the alias identifier.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public string Identifier
    {
      get { return IdentifierToken == null ? string.Empty : IdentifierToken.Value; }
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets a value indicating whether this instance has identifier.
    /// </summary>
    /// <value>
    /// 	<c>true</c> if this instance has identifier; otherwise, <c>false</c>.
    /// </value>
    // ----------------------------------------------------------------------------------------------
    public bool HasIdentifier
    {
      get { return IdentifierToken != null; }
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

      if (AttributeDecorations != null)
      {
        foreach (var attributeDecoration in AttributeDecorations)
        {
          attributeDecoration.AcceptVisitor(visitor);
        }
      }

      if (Type != null)
      {
        Type.AcceptVisitor(visitor);
      }
    }

    #endregion
  }
}