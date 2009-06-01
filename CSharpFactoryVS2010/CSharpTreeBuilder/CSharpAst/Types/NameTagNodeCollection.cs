// ================================================================================================
// NameTagNodeCollection.cs
//
// Created: 2009.05.11, by Istvan Novak (DeepDiver)
// ================================================================================================
using System.Text;
using CSharpTreeBuilder.CSharpAstBuilder;

namespace CSharpTreeBuilder.Ast
{
  // ================================================================================================
  /// <summary>
  /// This class represents a collection of name tags.
  /// </summary>
  /// <remarks>
  /// 	<para>This class is used to keep namespace names.</para>
  /// 	<para>Syntax:</para>
  /// 	<blockquote style="MARGIN-RIGHT: 0px" dir="ltr">
  /// 		<para>{ <em>NameTagNode</em> }</para>
  /// 	</blockquote>
  /// 	<para>Representation:</para>
  /// 	<blockquote style="MARGIN-RIGHT: 0px" dir="ltr">
  /// 		<para><em>NameTagNode</em>: Collection item</para>
  /// 	</blockquote>
  /// </remarks>
  // ================================================================================================
  public sealed class NameTagNodeCollection : 
    SyntaxNodeCollection<NameTagNode, NamespaceDeclarationNode>
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
        var first = true;
        foreach (NameTagNode tag in this)
        {
          if (!first) sb.Append(".");
          sb.Append(tag.Identifier);
          first = false;
        }
        return sb.ToString();
      }
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Adds a new NameTagNode with the specified identifier token.
    /// </summary>
    /// <param name="identifier">The identifier token.</param>
    /// <returns>The newly created NameTagNode instance.</returns>
    // ----------------------------------------------------------------------------------------------
    public void Add(Token identifier)
    {
      Add(new NameTagNode(identifier));
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Adds a new NameTagNode with the specified identifier token.
    /// </summary>
    /// <param name="separator">The separator token.</param>
    /// <param name="identifier">The identifier token.</param>
    /// <returns>The newly created NameTagNode instance.</returns>
    // ----------------------------------------------------------------------------------------------
    public void Add(Token separator, Token identifier)
    {
      Add(separator, new NameTagNode(identifier));
    }
  }
}