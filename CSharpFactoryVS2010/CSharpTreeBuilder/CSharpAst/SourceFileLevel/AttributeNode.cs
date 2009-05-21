// ================================================================================================
// AttributeNode.cs
//
// Created: 2009.03.29, by Istvan Novak (DeepDiver)
// ================================================================================================
using CSharpTreeBuilder.Collections;
using CSharpTreeBuilder.CSharpAstBuilder;

namespace CSharpFactory.Syntax
{
  // ================================================================================================
  /// <summary>
  /// This node represents an attribute with its arguments.
  /// </summary>
  /// <remarks>
  /// Syntax:
  ///   AttributeNode: 
  ///     TypeOrNamespaceNode 
  ///       [ "(" [ AttributeArgumentNode ] { AttributeArgumentContinuationNode }  ")" ]
  /// </remarks>
  // ================================================================================================
  public class AttributeNode : SyntaxNode , IParentheses
  {
    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Initializes a new instance of the <see cref="AttributeNode"/> class.
    /// </summary>
    /// <param name="start">Token providing information about the element.</param>
    // ----------------------------------------------------------------------------------------------
    public AttributeNode(Token start)
      : base(start)
    {
      Arguments = new ImmutableCollection<AttributeArgumentNode>();
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets the full name of the attribute.
    /// </summary>
    /// <value>The namespace.</value>
    // ----------------------------------------------------------------------------------------------
    public TypeOrNamespaceNode TypeName { get; internal set; }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets the opening parenthesis token.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public Token OpenParenthesis { get; internal set; }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets the closing parenthesis token.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public Token CloseParenthesis { get { return TerminatingToken; } }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets a value indicating whether this attribute defines arguments.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public bool DefinesArguments
    {
      get { return OpenParenthesis != null; }
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets the arguments belonging to this attribute.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public ImmutableCollection<AttributeArgumentNode> Arguments { get; protected set; }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets a value indicating whether this instance has arguments.
    /// </summary>
    /// <value>
    /// 	<c>true</c> if this instance has arguments; otherwise, <c>false</c>.
    /// </value>
    // ----------------------------------------------------------------------------------------------
    public bool HasArguments
    {
      get { return DefinesArguments && Arguments != null && Arguments.Count > 0; }
    }
  }
}