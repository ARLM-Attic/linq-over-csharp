// ================================================================================================
// TypeOrNamespaceContinuationNode.cs
//
// Created: 2009.05.11, by Istvan Novak (DeepDiver)
// ================================================================================================
using CSharpTreeBuilder.CSharpAstBuilder;

namespace CSharpFactory.Syntax
{
  // ================================================================================================
  /// <summary>
  /// This class represents the continuation of a TypeOrNamespaceNode.
  /// </summary>
  /// <remarks>
  /// Syntax
  ///   TypeOrNamespaceContinuationNode:
  ///     "," TypeOrNamespaceNode
  /// </remarks>
  // ================================================================================================
  public sealed class TypeOrNamespaceContinuationNode : TypeOrNamespaceNode
  {
    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Initializes a new instance of the <see cref="TypeOrNamespaceContinuationNode"/> class.
    /// </summary>
    /// <param name="separator">The separator.</param>
    /// <param name="node">The node.</param>
    // ----------------------------------------------------------------------------------------------
    public TypeOrNamespaceContinuationNode(Token separator, TypeOrNamespaceNode node) : 
      base(separator)
    {
      SeparatorToken = separator;
      QualifierToken = node.QualifierToken;
      QualifierSeparatorToken = node.QualifierSeparatorToken;
      TypeTags = node.TypeTags;
      Terminate(node.TerminatingToken);
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets or sets the separator token.
    /// </summary>
    /// <value>The separator token.</value>
    // ----------------------------------------------------------------------------------------------
    public Token SeparatorToken { get; private set; }
  }
}