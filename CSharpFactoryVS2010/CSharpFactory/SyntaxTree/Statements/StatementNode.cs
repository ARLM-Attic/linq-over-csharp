// ================================================================================================
// StatementNode.cs
//
// Created: 2009.05.06, by Istvan Novak (DeepDiver)
// ================================================================================================
using CSharpFactory.Collections;
using CSharpFactory.ParserFiles;

namespace CSharpFactory.Syntax
{
  // ================================================================================================
  /// <summary>
  /// This abstract node is intended to be the root of all C# language statements.
  /// </summary>
  // ================================================================================================
  public abstract class StatementNode : SyntaxNode
  {
    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Initializes a new instance of the <see cref="StatementNode"/> class.
    /// </summary>
    /// <param name="start">Token providing information about the element.</param>
    // ----------------------------------------------------------------------------------------------
    protected StatementNode(Token start)
      : base(start)
    {
      Labels = new LabelNodeCollection();
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets or sets the labels belonging to this statement.
    /// </summary>
    /// <value>The labels.</value>
    // ----------------------------------------------------------------------------------------------
    public LabelNodeCollection Labels { get; private set; }
  }

  // ================================================================================================
  /// <summary>
  /// This node describes a label before a statement.
  /// </summary>
  // ================================================================================================
  public sealed class LabelNode : SyntaxNode, IIdentifierSupport
  {
    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Initializes a new instance of the <see cref="LabelNode"/> class.
    /// </summary>
    /// <param name="identifier">Label identifier token.</param>
    /// <param name="colon">The colon token of the label.</param>
    // ----------------------------------------------------------------------------------------------
    public LabelNode(Token identifier, Token colon)
      : base(identifier)
    {
      IdentifierToken = identifier;
      ColonToken = colon;
    }

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
    /// Gets or sets the colon token.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public Token ColonToken { get; private set; }
  }

  // ================================================================================================
  /// <summary>
  /// This class represents a collection of label nodes.
  /// </summary>
  // ================================================================================================
  public sealed class LabelNodeCollection : ImmutableCollection<LabelNode>
  {
    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Adds the label to the first position of the collection..
    /// </summary>
    /// <param name="labelNode">The label node to add.</param>
    // ----------------------------------------------------------------------------------------------
    public void AddLabel(LabelNode labelNode)
    {
      Insert(0, labelNode);
    }
  }

  // ================================================================================================
  /// <summary>
  /// This class represents a collection of statements.
  /// </summary>
  // ================================================================================================
  public sealed class StatementNodeCollection : ImmutableCollection<StatementNode>
  {
  }
}