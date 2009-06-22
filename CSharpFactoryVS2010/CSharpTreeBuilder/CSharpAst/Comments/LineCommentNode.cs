// ================================================================================================
// LineCommentNode.cs
//
// Created: 2009.06.18, by Istvan Novak (DeepDiver)
// ================================================================================================
using CSharpTreeBuilder.CSharpAstBuilder;

namespace CSharpTreeBuilder.Ast
{
  // ================================================================================================
  /// <summary>
  /// This class represents a line comment.
  /// </summary>
  // ================================================================================================
  public class LineCommentNode : SingleCommentNodeBase
  {
    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Initializes a new instance of the <see cref="LineCommentNode"/> class.
    /// </summary>
    /// <param name="commentToken">Token providing information about the element.</param>
    // ----------------------------------------------------------------------------------------------
    public LineCommentNode(Token commentToken)
      : base(commentToken)
    {
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets the the distinguishing third character telling if the comment is an XML comment.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    protected override char XmlCommentChar
    {
      get { return '/'; }
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets the comment terminator string.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    protected override string CommentTerminator
    {
      get { return"\r"; }
    }
  }
}