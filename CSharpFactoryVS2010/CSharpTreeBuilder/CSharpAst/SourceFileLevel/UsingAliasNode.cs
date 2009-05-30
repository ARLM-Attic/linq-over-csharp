// ================================================================================================
// UsingAliasNode.cs
//
// Created: 2009.05.11, by Istvan Novak (DeepDiver)
// ================================================================================================
using CSharpTreeBuilder.CSharpAstBuilder;

namespace CSharpTreeBuilder.Ast
{
  // ================================================================================================
  /// <summary>
  /// This class represents a using clause with an alias.
  /// </summary>
  /// <remarks>
  /// Syntax:
  ///   UsingNamespaceNode:
  ///     "using" alias "=" TypeOrNamespaceNode ";"
  /// </remarks>
  // ================================================================================================
  public sealed class UsingAliasNode : UsingNamespaceNode
  {
    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Initializes a new instance of the <see cref="UsingAliasNode"/> class.
    /// </summary>
    /// <param name="parent">The parent node.</param>
    /// <param name="start">The start token.</param>
    /// <param name="alias">The alias token.</param>
    /// <param name="equalToken">The equal token.</param>
    /// <param name="typeName">Name of the type.</param>
    /// <param name="terminating">The terminating token.</param>
    // ----------------------------------------------------------------------------------------------
    public UsingAliasNode(NamespaceScopeNode parent, Token start, Token alias, Token equalToken,
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
    public string Alias
    {
      get { return AliasToken.Value; }
    }

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