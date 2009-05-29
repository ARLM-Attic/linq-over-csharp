// ================================================================================================
// TypeTagNode.cs
//
// Created: 2009.05.11, by Istvan Novak (DeepDiver)
// ================================================================================================
using CSharpTreeBuilder.CSharpAstBuilder;

namespace CSharpTreeBuilder.Ast
{
  // ================================================================================================
  /// <summary>
  /// This class represents a type tag.
  /// </summary>
  /// <remarks>
  /// Syntax:
  ///   TypeTagNode:
  ///     identifier [TypeArgumentListNode]
  /// </remarks>
  // ================================================================================================
  public class TypeTagNode : SyntaxNode, IIdentifierSupport, ITypeArguments
  {
    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Initializes a new instance of the <see cref="TypeTagNode"/> class.
    /// </summary>
    /// <param name="identifier">Identifier token.</param>
    /// <param name="argumentListNode">The argument list node.</param>
    // ----------------------------------------------------------------------------------------------
    public TypeTagNode(Token identifier, TypeArgumentListNode argumentListNode)
      : base(identifier)
    {
      IdentifierToken = identifier;
      Arguments = argumentListNode;
      Terminate(argumentListNode == null ? IdentifierToken : argumentListNode.TerminatingToken);
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Initializes a new instance of the <see cref="TypeTagNode"/> class.
    /// </summary>
    /// <param name="separator">The separator token.</param>
    /// <param name="identifier">Identifier token.</param>
    /// <param name="argumentListNode">The argument list node.</param>
    // ----------------------------------------------------------------------------------------------
    public TypeTagNode(Token separator, Token identifier, TypeArgumentListNode argumentListNode)
      : this(identifier, argumentListNode)
    {
      SeparatorToken = separator;
    }

    #region IIdentifierSupport Members

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets or sets the identifier token.
    /// </summary>
    /// <value>The identifier token.</value>
    // ----------------------------------------------------------------------------------------------
    public Token IdentifierToken { get; protected set; }

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

    #endregion

    #region ITypeArguments Members

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets or sets the node providing type arguments.
    /// </summary>
    /// <value>The arguments.</value>
    // ----------------------------------------------------------------------------------------------
    public TypeArgumentListNode Arguments { get; private set; }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets a value indicating whether this instance has type arguments.
    /// </summary>
    /// <value>
    /// 	<c>true</c> if this instance has type arguments; otherwise, <c>false</c>.
    /// </value>
    // ----------------------------------------------------------------------------------------------
    public bool HasTypeArguments
    {
      get { return Arguments != null && Arguments.Count > 0; }
    }

    #endregion

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
        SeparatorToken,
        IdentifierToken,
        Arguments
        );
    }
  }
}