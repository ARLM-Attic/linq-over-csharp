// ================================================================================================
// NamespaceDeclarationNode.cs
//
// Created: 2009.03.26, by Istvan Novak (DeepDiver)
// ================================================================================================
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
    /// <param name="parent">The parent node.</param>
    /// <param name="start">Token providing information about the element.</param>
    // ----------------------------------------------------------------------------------------------
    public NamespaceDeclarationNode(NamespaceScopeNode parent, Token start): base(start)
    {
      Parent = parent;
      NameTags = new NameTagNodeCollection();
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets the parent of this node.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public NamespaceScopeNode Parent { get; private set; }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets the nametags decribing the full name.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public NameTagNodeCollection NameTags { get; private set; }

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
        IndentationSegment.Apply,
        StartToken,
        MandatoryWhiteSpaceSegment.Default,
        NameTags,
        BraceSegment.OpenType(OpenBracket),
        base.GetOutputSegment(),
        BraceSegment.CloseType(CloseBracket)
        );
      if (TerminatingToken != CloseBracket)
        result.Append(TerminatingToken);
      return result;
    }
  }
}