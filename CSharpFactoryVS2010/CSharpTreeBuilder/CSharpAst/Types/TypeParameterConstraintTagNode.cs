// ================================================================================================
// TypeParameterConstraintTagNode.cs
//
// Created: 2009.05.11, by Istvan Novak (DeepDiver)
// ================================================================================================
using System;
using CSharpTreeBuilder.CSharpAstBuilder;

namespace CSharpTreeBuilder.Ast
{
  // ================================================================================================
  /// <summary>
  /// Defines a node for a parameter constraint
  /// </summary>
  /// <remarks>
  /// Syntax:
  ///   TypeParameterConstraintTagNode:
  ///     "class" | "struct" | "new" "(" ")" | TypeOrNamespaceNode
  /// </remarks>
  // ================================================================================================
  public class TypeParameterConstraintTagNode : SyntaxNode<TypeOrMemberDeclarationNode>, IParentheses
  {
    // --- Backing fields
    private TypeNode _Type;

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Initializes a new instance of the <see cref="TypeParameterConstraintTagNode"/> class.
    /// </summary>
    /// <param name="start">Token providing information about the element.</param>
    // ----------------------------------------------------------------------------------------------
    public TypeParameterConstraintTagNode(Token start)
      : base(start)
    {
      ConstraintToken = start;
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Initializes a new instance of the <see cref="TypeParameterConstraintTagNode"/> class.
    /// </summary>
    /// <param name="start">The start token.</param>
    /// <param name="openPar">The opening parenthesis.</param>
    /// <param name="closePar">The closing parenthesis.</param>
    // ----------------------------------------------------------------------------------------------
    public TypeParameterConstraintTagNode(Token start, Token openPar, Token closePar)
      : base(start)
    {
      ConstraintToken = start;
      OpenParenthesis = openPar;
      CloseParenthesis = closePar;
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Initializes a new instance of the <see cref="TypeParameterConstraintTagNode"/> class.
    /// </summary>
    /// <param name="typeNode">The type node.</param>
    // ----------------------------------------------------------------------------------------------
    public TypeParameterConstraintTagNode(TypeNode typeNode) :
      base(typeNode.StartToken)
    {
      ConstraintToken = StartToken;
      Type = typeNode;
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets the constraint token.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public Token ConstraintToken { get; protected set; }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets a value indicating whether this is a "class" constraint tag.
    /// </summary>
    /// <value><c>true</c> if this instance is "class" constraint tag; otherwise, <c>false</c>.</value>
    // ----------------------------------------------------------------------------------------------
    public bool IsClass
    {
      get { return ConstraintToken.Value == "class"; }
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets a value indicating whether this is a "struct" constraint tag.
    /// </summary>
    /// <value><c>true</c> if this instance is "struct" constraint tag; otherwise, <c>false</c>.</value>
    // ----------------------------------------------------------------------------------------------
    public bool IsStruct
    {
      get { return ConstraintToken.Value == "struct"; }
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets a value indicating whether this is a "new" constraint tag.
    /// </summary>
    /// <value><c>true</c> if this instance is "new" constraint tag; otherwise, <c>false</c>.</value>
    // ----------------------------------------------------------------------------------------------
    public bool IsNew
    {
      get { return ConstraintToken.Value == "new"; }
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets a value indicating whether this is a type name constraint tag.
    /// </summary>
    /// <value><c>true</c> if this instance is type name constraint tag; otherwise, <c>false</c>.</value>
    // ----------------------------------------------------------------------------------------------
    public bool IsTypeName
    {
      get { return Type != null; }
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets the constarint type name.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public TypeNode Type
    {
      get { return _Type; }
      protected set
      {
        _Type = value;
        if (_Type != null) _Type.ParentNode = this;
      }
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets the opening parenthesis token.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public Token OpenParenthesis { get; protected set; }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets the closing parenthesis token.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public Token CloseParenthesis { get; protected set; }

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
      var separatorSegment = new OutputSegment(
        SeparatorToken,
        SeparatorToken.IsComma() ? SpaceAfterSegment.AfterComma() : null);

      if (IsClass || IsStruct) return new OutputSegment(separatorSegment, StartToken);
      return IsNew
        ? new OutputSegment(separatorSegment, StartToken, OpenParenthesis, CloseParenthesis)
        : new OutputSegment(separatorSegment, Type);
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

      if (Type!=null)
      {
        Type.AcceptVisitor(visitor);
      }
    }

    #endregion
  }
}