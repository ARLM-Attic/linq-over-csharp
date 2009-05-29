// ================================================================================================
// AttributeDecorationNode.cs
//
// Created: 2009.05.11, by Istvan Novak (DeepDiver)
// ================================================================================================
using CSharpTreeBuilder.Collections;
using CSharpTreeBuilder.CSharpAstBuilder;

namespace CSharpTreeBuilder.Ast
{
  // ================================================================================================
  /// <summary>
  /// This node represents a global or local argument decoration.
  /// </summary>
  /// <remarks>
  /// Syntax:
  ///   AttributeDecorationNode:
  ///     "[" [ identifier ":" ] AttributeNode { AttributeContinuationNode } [ "," ] "]"
  /// 
  /// The openining bracket is represented by StartToken, the closing bracket with
  /// TerminatingToken.
  /// </remarks>
  // ================================================================================================
  public sealed class AttributeDecorationNode : SyntaxNode
  {
    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Initializes a new instance of the <see cref="AttributeDecorationNode"/> class.
    /// </summary>
    /// <param name="start">Token providing information about the element.</param>
    // ----------------------------------------------------------------------------------------------
    public AttributeDecorationNode(Token start)
      : base(start)
    {
      Attributes = new ImmutableCollection<AttributeNode>();
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets or sets the target token.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public Token TargetToken { get; internal set; }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets the target information.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public string Target
    {
      get { return TargetToken == null ? string.Empty : TargetToken.Value; }
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets a value indicating whether this instance has explicit target.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public bool HasExplicitTarget { get { return TargetToken != null; } }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets or sets the attributes.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public ImmutableCollection<AttributeNode> Attributes { get; internal set; }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets or sets the optional closing separator.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public Token ClosingSeparator { get; internal set; }
  }
}