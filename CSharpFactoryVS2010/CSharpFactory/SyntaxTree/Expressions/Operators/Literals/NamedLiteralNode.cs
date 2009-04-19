// ================================================================================================
// NamedLiteralNode.cs
//
// Created: 2009.04.16, by Istvan Novak (DeepDiver)
// ================================================================================================
using System;
using CSharpFactory.ParserFiles;

namespace CSharpFactory.Syntax
{
  // ================================================================================================
  /// <summary>
  /// This class represents the common root class on named literals.
  /// </summary>
  // ================================================================================================
  public class NamedLiteralNode : LiteralNode, IIdentifierSupport, ITypeArguments
  {
    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Initializes a new instance of the <see cref="NamedLiteralNode"/> class.
    /// </summary>
    /// <param name="start">Token providing information about the element.</param>
    // ----------------------------------------------------------------------------------------------
    public NamedLiteralNode(Token start)
      : base(start)
    {
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets or sets the qualifier token.
    /// </summary>
    /// <value>The qualifier token.</value>
    // ----------------------------------------------------------------------------------------------
    public Token QualifierToken { get; internal set; }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets a value indicating whether this instance has qualifier.
    /// </summary>
    /// <value>
    /// 	<c>true</c> if this instance has qualifier; otherwise, <c>false</c>.
    /// </value>
    // ----------------------------------------------------------------------------------------------
    public bool HasQualifier { get { return QualifierToken != null; } }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets the qualifier.
    /// </summary>
    /// <value>The qualifier.</value>
    // ----------------------------------------------------------------------------------------------
    public string Qualifier
    {
      get { return QualifierToken == null ? string.Empty : QualifierToken.val; }
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets or sets the separator token.
    /// </summary>
    /// <value>The separator token.</value>
    // ----------------------------------------------------------------------------------------------
    public Token QualifierSeparatorToken { get; internal set; }

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
    /// Gets the separator token.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public Token SeparatorToken { get; internal set; }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets the node providing type arguments.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public TypeArgumentListNode Arguments { get; internal set; }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets a value indicating whether this instance has type arguments.
    /// </summary>
    /// <value>
    /// 	<c>true</c> if this instance has type arguments; otherwise, <c>false</c>.
    /// </value>
    // ----------------------------------------------------------------------------------------------
    public bool HasTypeArguments
    {
      get { return Arguments != null; }
    }
  }
}