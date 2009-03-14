// ================================================================================================
// UsingNode.cs
//
// Created: 2009.03.13, by Istvan Novak (DeepDiver)
// ================================================================================================
using CSharpFactory.ParserFiles;

namespace CSharpFactory.Syntax
{
  // ================================================================================================
  /// <summary>
  /// This class represents a using clause.
  /// </summary>
  /// <remarks>
  /// Syntax:
  ///   UsingNode:
  ///     "using" TypeOrNamespaceNode
  /// </remarks>
  // ================================================================================================
  public class UsingNode : SyntaxNode
  {
    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Initializes a new instance of the <see cref="UsingNode"/> class.
    /// </summary>
    /// <param name="start">Token providing information about the element.</param>
    /// <param name="namespaceNode">The namespace node.</param>
    // ----------------------------------------------------------------------------------------------
    public UsingNode(Token start, TypeOrNamespaceNode namespaceNode)
      : base(start)
    {
      TypeName = namespaceNode;
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets the namespace belonging to this using directive.
    /// </summary>
    /// <value>The namespace.</value>
    // ----------------------------------------------------------------------------------------------
    public TypeOrNamespaceNode TypeName { get; private set; }
  }

  // ================================================================================================
  /// <summary>
  /// This class represents a using clause with an alias.
  /// </summary>
  /// <remarks>
  /// Syntax:
  ///   UsingNode:
  ///     "using" alias "=" TypeOrNamespaceNode
  /// </remarks>
  // ================================================================================================
  public sealed class UsingWithAliasNode : UsingNode
  {
    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Initializes a new instance of the <see cref="UsingWithAliasNode"/> class.
    /// </summary>
    /// <param name="start">The start token.</param>
    /// <param name="alias">The alias token.</param>
    /// <param name="equalToken">The equal token.</param>
    /// <param name="typeName">Name of the type.</param>
    // ----------------------------------------------------------------------------------------------
    public UsingWithAliasNode(Token start, Token alias, Token equalToken, 
      TypeOrNamespaceNode typeName)
      : base(start, typeName)
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
    public string Alias { get { return AliasToken.val; } }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets or sets the equal token.
    /// </summary>
    /// <value>The equal token.</value>
    // ----------------------------------------------------------------------------------------------
    public Token EqualToken { get; private set; }
  }
}