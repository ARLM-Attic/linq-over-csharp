// ================================================================================================
// ConstMemberDeclarationNode.cs
//
// Created: 2009.04.14, by Istvan Novak (DeepDiver)
// ================================================================================================
using CSharpFactory.Collections;
using CSharpFactory.ParserFiles;

namespace CSharpFactory.Syntax
{
  // ================================================================================================
  /// <summary>
  /// This class represents a const member declaration node.
  /// </summary>
  // ================================================================================================
  public class ConstMemberDeclarationNode : MemberDeclarationNode
  {
    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Initializes a new instance of the <see cref="ConstMemberDeclarationNode"/> class.
    /// </summary>
    /// <param name="start">Token providing information about the element.</param>
    // ----------------------------------------------------------------------------------------------
    public ConstMemberDeclarationNode(Token start)
      : base(start)
    {
      ConstTags = new ConstMemberTagNodeCollection();
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets the collection const tags.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public ConstMemberTagNodeCollection ConstTags { get; private set; }
  }

  // ================================================================================================
  /// <summary>
  /// This class represents a const member tag.
  /// </summary>
  // ================================================================================================
  public class ConstMemberTagNode : SyntaxNode, IIdentifierSupport
  {
    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Initializes a new instance of the <see cref="ConstMemberTagNode"/> class.
    /// </summary>
    /// <param name="start">Token providing information about the element.</param>
    // ----------------------------------------------------------------------------------------------
    public ConstMemberTagNode(Token start)
      : base(start)
    {
      IdentifierToken = start;
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets or sets the identifier token.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public Token IdentifierToken { get; internal set; }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets the identifier name.
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
    /// Gets or sets the equal token.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public Token EqualToken { get; internal set; }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets or sets the expression belonging to this const member.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public ExpressionNode Expression { get; set; }
  }

  // ================================================================================================
  /// <summary>
  /// This class represents a const member continuation tag.
  /// </summary>
  // ================================================================================================
  public sealed class ConstMemberContinuationTagNode : ConstMemberTagNode, IContinuationTag
  {
    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Initializes a new instance of the <see cref="ConstMemberContinuationTagNode"/> class.
    /// </summary>
    /// <param name="separator">The separator token.</param>
    /// <param name="tag">The tag to obtain proerties form.</param>
    // ----------------------------------------------------------------------------------------------
    public ConstMemberContinuationTagNode(Token separator, ConstMemberTagNode tag)
      : base(separator)
    {
      SeparatorToken = separator;
      IdentifierToken = tag.IdentifierToken;
      EqualToken = tag.EqualToken;
      Expression = tag.Expression;
      Terminate(tag.TerminatingToken);
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets the token separating the continuation tag from the preceding tag.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public Token SeparatorToken { get; internal set; }
  }

  // ================================================================================================
  /// <summary>
  /// This class declares a collection of const member tags.
  /// </summary>
  // ================================================================================================
  public sealed class ConstMemberTagNodeCollection : ImmutableCollection<ConstMemberTagNode>
  {
  }
}