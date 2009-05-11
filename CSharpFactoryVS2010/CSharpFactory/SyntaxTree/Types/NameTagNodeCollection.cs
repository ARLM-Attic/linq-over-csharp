// ================================================================================================
// NameTagNodeCollection.cs
//
// Created: 2009.05.11, by Istvan Novak (DeepDiver)
// ================================================================================================
using System.Text;
using CSharpFactory.Collections;
using CSharpFactory.ParserFiles;

namespace CSharpFactory.Syntax
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