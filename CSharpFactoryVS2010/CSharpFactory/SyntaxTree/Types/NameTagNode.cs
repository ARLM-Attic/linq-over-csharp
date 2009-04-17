// ================================================================================================
// NameTagNode.cs
//
// Created: 2009.03.26, by Istvan Novak (DeepDiver)
// ================================================================================================
using System;
using System.Text;
using CSharpFactory.Collections;
using CSharpFactory.ParserFiles;

namespace CSharpFactory.Syntax
{
  // ================================================================================================
  /// <summary>
  /// This class represents a name tag.
  /// </summary>
  /// <remarks>
  /// Syntax:
  ///   NameTagNode:
  ///     identifier
  /// </remarks>
  // ================================================================================================
  public class NameTagNode : SyntaxNode, IIdentifierSupport
  {
    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Initializes a new instance of the <see cref="NameTagNode"/> class.
    /// </summary>
    /// <param name="start">Token providing information about the element.</param>
    // ----------------------------------------------------------------------------------------------
    public NameTagNode(Token start)
      : base(start)
    {
      IdentifierToken = start;
    }

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
    public string Identifier { get { return IdentifierToken.val; } }

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
      return new OutputSegment(IdentifierToken);
    }
  }

  // ================================================================================================
  /// <summary>
  /// This class represents a name tag as a continuation of a name tag list.
  /// </summary>
  /// <remarks>
  /// Syntax:
  ///   NameTagContinuationNode:
  ///     "." NameTagNode
  /// </remarks>
  // ================================================================================================
  public class NameTagContinuationNode : NameTagNode, IContinuationTag
  {
    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Initializes a new instance of the <see cref="TypeTagContinuationNode"/> class.
    /// </summary>
    /// <param name="start">Token providing information about the element.</param>
    /// <param name="identifier">The identifier.</param>
    // ----------------------------------------------------------------------------------------------
    public NameTagContinuationNode(Token start, Token identifier)
      : base(start)
    {
      if (identifier == null) throw new ArgumentNullException("identifier");
      SeparatorToken = start;
      IdentifierToken = identifier;
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets or sets the separator token.
    /// </summary>
    /// <value>The separator token.</value>
    // ----------------------------------------------------------------------------------------------
    public Token SeparatorToken { get; private set; }

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
        IdentifierToken
        );
    }
  }

  // ================================================================================================
  /// <summary>
  /// This class represents a collection of name tags.
  /// </summary>
  // ================================================================================================
  public sealed class NameTagCollection : ImmutableCollection<NameTagNode>
  {
    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets the full name representedby the tags of this collection.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public string FullName
    {
      get
      {
        var sb = new StringBuilder();
        foreach (var tag in this)
        {
          if (tag is NameTagContinuationNode) sb.Append(".");
          sb.Append(tag.Identifier);
        }
        return sb.ToString();
      }
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Adds a new NameTagNode with the specified start token.
    /// </summary>
    /// <param name="start">The start token.</param>
    /// <returns>The newly created NameTagNode instance.</returns>
    // ----------------------------------------------------------------------------------------------
    public NameTagNode Add(Token start)
    {
      var result = new NameTagNode(start);
      Add(result);
      return result;
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Adds a new NameTagContinuationNode with the specified start and identifier tokens.
    /// </summary>
    /// <param name="start">The start token.</param>
    /// <param name="identifier">The identifier token.</param>
    /// <returns>
    /// The newly created NameTagContinuationNode instance.
    /// </returns>
    // ----------------------------------------------------------------------------------------------
    public NameTagContinuationNode Add(Token start, Token identifier )
    {
      var result = new NameTagContinuationNode(start, identifier);
      Add(result);
      return result;
    }
  }
}