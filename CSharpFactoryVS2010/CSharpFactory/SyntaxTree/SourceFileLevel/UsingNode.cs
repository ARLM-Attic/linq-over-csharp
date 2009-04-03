// ================================================================================================
// UsingNode.cs
//
// Created: 2009.03.13, by Istvan Novak (DeepDiver)
// ================================================================================================
using CSharpFactory.ParserFiles;

namespace CSharpFactory.Syntax
{
  // ================================================================================================
  /// <summary>
  /// This class represents a using clause.
  /// </summary>
  /// <remarks>
  /// Syntax:
  ///   UsingNode:
  ///     "using" TypeOrNamespaceNode ";"
  /// </remarks>
  // ================================================================================================
  public class UsingNode : SyntaxNode
  {
    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Initializes a new instance of the <see cref="UsingNode"/> class.
    /// </summary>
    /// <param name="parent">The parent node.</param>
    /// <param name="start">Token providing information about the element.</param>
    /// <param name="namespaceNode">The namespace node.</param>
    /// <param name="terminating">The terminating token.</param>
    // ----------------------------------------------------------------------------------------------
    public UsingNode(NamespaceScopeNode parent, Token start, TypeOrNamespaceNode namespaceNode, Token terminating)
      : base(start)
    {
      Parent = parent;
      TypeName = namespaceNode;
      Terminate(terminating);
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets the parent of this node.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public NamespaceScopeNode Parent { get; private set; }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets the namespace belonging to this using directive.
    /// </summary>
    /// <value>The namespace.</value>
    // ----------------------------------------------------------------------------------------------
    public TypeOrNamespaceNode TypeName { get; private set; }

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
        IndentationSegment.Apply,
        StartToken,
        MandatoryWhiteSpaceSegment.Default,
        TypeName,
        TerminatingToken,
        ForceNewLineSegment.Default
        );
    }
  }

  // ================================================================================================
  /// <summary>
  /// This class represents a using clause with an alias.
  /// </summary>
  /// <remarks>
  /// Syntax:
  ///   UsingNode:
  ///     "using" alias "=" TypeOrNamespaceNode ";"
  /// </remarks>
  // ================================================================================================
  public sealed class UsingWithAliasNode : UsingNode
  {
    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Initializes a new instance of the <see cref="UsingWithAliasNode"/> class.
    /// </summary>
    /// <param name="parent">The parent node.</param>
    /// <param name="start">The start token.</param>
    /// <param name="alias">The alias token.</param>
    /// <param name="equalToken">The equal token.</param>
    /// <param name="typeName">Name of the type.</param>
    /// <param name="terminating">The terminating token.</param>
    // ----------------------------------------------------------------------------------------------
    public UsingWithAliasNode(NamespaceScopeNode parent, Token start, Token alias, Token equalToken, 
      TypeOrNamespaceNode typeName, Token terminating)
      : base(parent, start, typeName, terminating)
    {
      AliasToken = alias;
      EqualToken = equalToken;
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets or sets the alias token.
    /// </summary>
    /// <value>The alias token.</value>
    // ----------------------------------------------------------------------------------------------
    public Token AliasToken { get; private set; }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets the alias.
    /// </summary>
    /// <value>The alias.</value>
    // ----------------------------------------------------------------------------------------------
    public string Alias { get { return AliasToken.val; } }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets or sets the equal token.
    /// </summary>
    /// <value>The equal token.</value>
    // ----------------------------------------------------------------------------------------------
    public Token EqualToken { get; private set; }

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
        IndentationSegment.Apply,
        StartToken,
        MandatoryWhiteSpaceSegment.Default,
        AliasToken,
        SpaceAroundSegment.AssignmentOp(EqualToken),
        TypeName,
        TerminatingToken,
        ForceNewLineSegment.Default
        );
    }
  }
}