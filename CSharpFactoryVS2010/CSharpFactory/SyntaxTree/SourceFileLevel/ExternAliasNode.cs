// ================================================================================================
// ExternAliasNode.cs
//
// Created: 2009.03.26, by Istvan Novak (DeepDiver)
// ================================================================================================
using CSharpFactory.ParserFiles;

namespace CSharpFactory.Syntax
{
  // ================================================================================================
  /// <summary>
  /// This class represnts an "extern alias" node.
  /// </summary>
  /// <remarks>
  /// Syntax:
  ///   ExternAliasNode:
  ///     "extern" "alias" identifier ";"
  /// </remarks>
  // ================================================================================================
  public sealed class ExternAliasNode: SyntaxNode
  {
    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Initializes a new instance of the <see cref="ExternAliasNode"/> class.
    /// </summary>
    /// <param name="parent">The parent namespace scope node.</param>
    /// <param name="start">The start token.</param>
    /// <param name="alias">The alias token.</param>
    /// <param name="identifier">The identifier token.</param>
    /// <param name="terminating">The terminating token.</param>
    // ----------------------------------------------------------------------------------------------
    public ExternAliasNode(NamespaceScopeNode parent, Token start, Token alias, Token identifier, 
      Token terminating) : base(start)
    {
      Parent = parent;
      AliasToken = alias;
      IdentifierToken = identifier;
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
    /// Gets the alias token.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public Token AliasToken { get; private set; }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets the identifier token.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public Token IdentifierToken { get; private set; }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets the alias identifier.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public string Identifier 
    { 
      get { return IdentifierToken == null ? string.Empty : IdentifierToken.val; }
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
        IndentationSegment.Apply,
        StartToken,
        MandatoryWhiteSpaceSegment.Default,
        AliasToken,
        MandatoryWhiteSpaceSegment.Default,
        IdentifierToken,
        TerminatingToken,
        ForceNewLineSegment.Default
        );
    }
  }
}