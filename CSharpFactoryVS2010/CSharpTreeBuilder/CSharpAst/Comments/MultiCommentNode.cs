// ================================================================================================
// MultiCommentNode.cs
//
// Created: 2009.06.18, by Istvan Novak (DeepDiver)
// ================================================================================================
using System.Text;
using CSharpTreeBuilder.CSharpAstBuilder;

namespace CSharpTreeBuilder.Ast
{
  // ================================================================================================
  /// <summary>
  /// This class represents a block of adjacent comments decorating an entity.
  /// </summary>
  // ================================================================================================
  public class MultiCommentNode : 
    SyntaxNodeCollection<ICommentNode, ISyntaxNode>, ICommentNode
  {
    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets a value indicating whether this instance has documentation.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public bool HasDocumentation
    {
      get
      {
        foreach (var comment in this)
          if (comment.HasDocumentation) return true;
        return false;
      }
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets the text represented by the comment.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public string Text
    {
      get
      {
        var sb = new StringBuilder(1024);
        foreach (var comment in this)
        {
          if (sb.Length > 0) sb.Append("/r");
          sb.Append(comment.Text);
        }
        return sb.ToString();
      }
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets the syntax node that owns the comment.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public ISyntaxNode OwnerNode { get; set; }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets the token that owns the comment.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public Token OwnerToken { get; set; }
  }
}