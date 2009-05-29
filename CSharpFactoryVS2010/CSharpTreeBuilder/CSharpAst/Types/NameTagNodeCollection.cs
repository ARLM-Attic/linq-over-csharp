// ================================================================================================
// NameTagNodeCollection.cs
//
// Created: 2009.05.11, by Istvan Novak (DeepDiver)
// ================================================================================================
using System.Text;
using CSharpTreeBuilder.Collections;
using CSharpTreeBuilder.CSharpAstBuilder;

namespace CSharpTreeBuilder.Ast
{
  // ================================================================================================
  /// <summary>
  /// This class represents a collection of name tags.
  /// </summary>
  // ================================================================================================
  public sealed class NameTagNodeCollection : ImmutableCollection<NameTagNode>
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
    public NameTagNode Add(Token identifier)
    {
      var result = new NameTagNode(identifier);
      Add(result);
      return result;
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Adds a new NameTagNode with the specified identifier token.
    /// </summary>
    /// <param name="separator">The separator token.</param>
    /// <param name="identifier">The identifier token.</param>
    /// <returns>The newly created NameTagNode instance.</returns>
    // ----------------------------------------------------------------------------------------------
    public NameTagNode Add(Token separator, Token identifier)
    {
      var result = new NameTagNode(separator, identifier);
      Add(result);
      return result;
    }
  }
}