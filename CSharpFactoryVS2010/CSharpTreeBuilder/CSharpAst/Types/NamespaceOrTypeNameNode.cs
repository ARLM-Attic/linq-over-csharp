﻿using System.Text;
using CSharpTreeBuilder.CSharpAstBuilder;

namespace CSharpTreeBuilder.Ast
{
  // ================================================================================================
  /// <summary>
  /// This class represents a namespace-or-type-name AST node.
  /// </summary>
  // ================================================================================================
  public sealed class NamespaceOrTypeNameNode : SyntaxNode<ISyntaxNode>
  {
    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Initializes a new instance of the <see cref="NamespaceOrTypeNameNode"/> class.
    /// </summary>
    /// <param name="identifier">Token providing information about the element.</param>
    // ----------------------------------------------------------------------------------------------
    public NamespaceOrTypeNameNode(Token identifier)
      : base(identifier)
    {
      TypeTags = new TypeTagNodeCollection {ParentNode = this};
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets or sets the type tags.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public TypeTagNodeCollection TypeTags { get; private set; }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets a value indicating whether this is an empty type name. (used in unbound generic types)
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public bool IsEmpty
    {
      get { return TypeTags == null || TypeTags.Count == 0; }
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets or sets the qualifier token.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public Token QualifierToken { get; internal set; }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets a value indicating whether this instance has qualifier.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public bool HasQualifier
    {
      get { return QualifierToken != null; }
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets the qualifier.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public string Qualifier
    {
      get { return QualifierToken == null ? null : QualifierToken.Value; }
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets or sets the qualifier separator ("::") token.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public Token QualifierSeparatorToken { get; internal set; }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Create a namespace-or-type-name node from the specified token used for a simple type.
    /// </summary>
    /// <param name="t">The token for the simple type.</param>
    /// <returns>A new NamespaceOrTypeNameNode</returns>
    // ----------------------------------------------------------------------------------------------
    public static NamespaceOrTypeNameNode CreateSimpleName(Token t)
    {
      var result = new NamespaceOrTypeNameNode(t);
      result.TypeTags.Add(new TypeTagNode(t, null));
      result.Terminate(t);
      return result;
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets the string representation of this language element.
    /// </summary>
    /// <returns>Full name of the language element.</returns>
    // ----------------------------------------------------------------------------------------------
    public override string ToString()
    {
      var result = new StringBuilder();

      if (Qualifier != null)
      {
        result.Append(Qualifier);
        result.Append("::");
      }

      result.Append(TypeTags.ToString());

      return result.Length == 0 ? GetType().ToString() : result.ToString();
    }

    #region Visitor methods

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Accepts a visitor object, according to the Visitor pattern.
    /// </summary>
    /// <param name="visitor">A visitor object</param>
    // ----------------------------------------------------------------------------------------------
    public override void AcceptVisitor(ISyntaxNodeVisitor visitor)
    {
      visitor.Visit(this);

      foreach (var tag in TypeTags)
      {
        tag.AcceptVisitor(visitor);
      }
    }

    #endregion
  }
}