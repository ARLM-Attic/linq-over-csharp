// ================================================================================================
// EnumValueNode.cs
//
// Created: 2009.05.15, by Istvan Novak (DeepDiver)
// ================================================================================================
using CSharpTreeBuilder.CSharpAstBuilder;

namespace CSharpTreeBuilder.Ast
{
  // ================================================================================================
  /// <summary>
  /// This class represents a member of an enumeration type.
  /// </summary>
  // ================================================================================================
  public class EnumValueNode : AttributedDeclarationNode, IIdentifierSupport
  {
    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Initializes a new instance of the <see cref="EnumValueNode"/> class.
    /// </summary>
    /// <param name="identifier">Token providing information about the element.</param>
    // ----------------------------------------------------------------------------------------------
    public EnumValueNode(Token identifier)
      : base(identifier)
    {
      IdentifierToken = identifier;
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets or sets the equal token.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public Token EqualToken { get; internal set; }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets or sets the optional expression of the enumeration value.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public ExpressionNode Expression { get; internal set; }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets or sets the identifier token.
    /// </summary>
    /// <value>The identifier token.</value>
    // ----------------------------------------------------------------------------------------------
    public Token IdentifierToken { get; internal set; }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets the identifier name.
    /// </summary>
    /// <value>The identifier name.</value>
    // ----------------------------------------------------------------------------------------------
    public string Identifier
    {
      get { return IdentifierToken.Value; }
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

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets the output segment representing this syntax node.
    /// </summary>
    /// <returns>
    /// The OutputSegment instance describing this syntax node, or null; if the node has no output.
    /// </returns>
    // ----------------------------------------------------------------------------------------------
    public override OutputSegment GetOutputSegment()
    {
      return new OutputSegment(
        base.GetOutputSegment(),
        SeparatorToken,
        ForceNewLineSegment.Default,
        IdentifierToken,
        EqualToken,
        Expression
        );
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

#warning Visiting of Expression field is missing.
    }

    #endregion
  }
}