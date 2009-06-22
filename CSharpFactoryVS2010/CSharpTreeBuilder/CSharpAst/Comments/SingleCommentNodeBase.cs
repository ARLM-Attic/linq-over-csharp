// ================================================================================================
// SingleCommentNodeBase.cs
//
// Created: 2009.06.17, by Istvan Novak (DeepDiver)
// ================================================================================================
using CSharpTreeBuilder.CSharpAstBuilder;

namespace CSharpTreeBuilder.Ast
{
  // ================================================================================================
  /// <summary>
  /// This abstract class is intended to be the base class for all comment nodes.
  /// </summary>
  // ================================================================================================
  public abstract class SingleCommentNodeBase : SyntaxNode<ISyntaxNode>, ICommentNode
  {
    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Initializes a new instance of the <see cref="SingleCommentNodeBase"/> class.
    /// </summary>
    /// <param name="commentToken">Token providing information about the element.</param>
    // ----------------------------------------------------------------------------------------------
    protected SingleCommentNodeBase(Token commentToken)
      : base(commentToken)
    {
      // ReSharper disable DoNotCallOverridableMethodsInConstructor
      CommentBodyStart = 2;
      var commentText = commentToken.Value;
      if (commentText.Length > 2)
      {
        // --- Check for XML commment
        if (commentText[2] == XmlCommentChar)
        {
          HasDocumentation = true;
          CommentBodyStart = 3;
        }
      }
      // --- Set the end of the comment body
      if (commentText.EndsWith(CommentTerminator))
      {
        CommentBodyEnd = commentText.Length - 1 - CommentTerminator.Length;
      }
      // ReSharper restore DoNotCallOverridableMethodsInConstructor
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets or sets the index position where comment body starts.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    protected int CommentBodyStart { get; set; }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets or sets the index position where comment body ends.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    protected int CommentBodyEnd { get; set; }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets a value indicating whether this instance has documentation.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public bool HasDocumentation { get; private set; }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets the text represented by the comment.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public string Text
    {
      get
      {
        return CommentBodyEnd < CommentBodyStart
                 ? string.Empty
                 : StartToken.Value.Substring(CommentBodyStart, CommentBodyEnd - CommentBodyStart + 1);
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

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets the the distinguishing third character telling if the comment is an XML comment.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    protected abstract char XmlCommentChar { get; }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets the comment terminator string.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    protected abstract string CommentTerminator { get; }
  }
}