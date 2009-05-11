// ================================================================================================
// TypeParameterConstraintTagNode.cs
//
// Created: 2009.05.11, by Istvan Novak (DeepDiver)
// ================================================================================================
using CSharpFactory.ParserFiles;

namespace CSharpFactory.Syntax
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
  public class TypeParameterConstraintTagNode : SyntaxNode, IParentheses
  {
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
    public TypeParameterConstraintTagNode(TypeOrNamespaceNode typeNode) : 
      base(typeNode.StartToken)
    {
      ConstraintToken = StartToken;
      TypeName = typeNode;
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
    public bool IsClass { get { return ConstraintToken.val == "class"; } }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets a value indicating whether this is a "struct" constraint tag.
    /// </summary>
    /// <value><c>true</c> if this instance is "struct" constraint tag; otherwise, <c>false</c>.</value>
    // ----------------------------------------------------------------------------------------------
    public bool IsStruct { get { return ConstraintToken.val == "struct"; } }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets a value indicating whether this is a "new" constraint tag.
    /// </summary>
    /// <value><c>true</c> if this instance is "new" constraint tag; otherwise, <c>false</c>.</value>
    // ----------------------------------------------------------------------------------------------
    public bool IsNew { get { return ConstraintToken.val == "new"; } }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets a value indicating whether this is a type name constraint tag.
    /// </summary>
    /// <value><c>true</c> if this instance is type name constraint tag; otherwise, <c>false</c>.</value>
    // ----------------------------------------------------------------------------------------------
    public bool IsTypeName { get { return TypeName != null; } }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets the constarint type name.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public TypeOrNamespaceNode TypeName { get; protected set; }

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
  }
}