// ================================================================================================
// NamespaceDeclarationNode.cs
//
// Created: 2009.03.26, by Istvan Novak (DeepDiver)
// ================================================================================================
using CSharpFactory.Collections;
using CSharpFactory.ParserFiles;

namespace CSharpFactory.Syntax
{
  // ================================================================================================
  /// <summary>
  /// This node represents a namespace declaration.
  /// </summary>
  /// <remarks>
  /// Syntax:
  ///   NamespaceDeclarationNode:
  ///     "namespace" ident { "." ident } "{" 
  ///       { ExternalAliasNode } { UsingNode } { NamespaceDeclaration } "}" [ ";" ]
  /// </remarks>
  // ================================================================================================
  public sealed class NamespaceDeclarationNode : NamespaceScopeNode
  {
    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Initializes a new instance of the <see cref="NamespaceDeclarationNode"/> class.
    /// </summary>
    /// <param name="start">Token providing information about the element.</param>
    // ----------------------------------------------------------------------------------------------
    public NamespaceDeclarationNode(Token start): base(start)
    {
      NameTags = new NameTagCollection();
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets the nametags decribing the full name.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public NameTagCollection NameTags { get; private set; }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets the token representing the opening bracket.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public Token OpenBracket { get; internal set; }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets the token representing the closing bracket.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public Token CloseBracket { get; internal set; }

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
      var result = new OutputSegment(
        StartToken,
        NameTags,
        OpenBracket,
        base.GetOutputSegment(),
        CloseBracket
        );
      if (TerminatingToken != CloseBracket)
        result.Append(TerminatingToken);
      return result;
    }
  }

  // ================================================================================================
  /// <summary>
  /// This type represents a collection of namespace declaration nodes.
  /// </summary>
  // ================================================================================================
  public sealed class NamespaceDeclarationNodeCollection : 
    ImmutableCollection<NamespaceDeclarationNode>
  {
  }
}