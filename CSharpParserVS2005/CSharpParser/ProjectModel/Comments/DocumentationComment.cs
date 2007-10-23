using System;
using System.Text;
using System.Xml;

namespace CSharpParser.ProjectModel
{
  // ==================================================================================
  /// <summary>
  /// This class represents XML documentation comments extracted from standard comments.
  /// </summary>
  // ==================================================================================
  public sealed class DocumentationComment
  {
    #region Private fields

    private readonly CommentInfo _OriginalComment;
    private readonly string _Text;
    private readonly XmlDocument _XmlDocument;
    private readonly bool _IsWellFormedXml;

    #endregion

    #region Lifecycle methods

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Creates an empty documentation comment.
    /// </summary>
    // --------------------------------------------------------------------------------
    public DocumentationComment()
    {
      _OriginalComment = null;
      _Text = String.Empty;
      _XmlDocument = new XmlDocument();
      _IsWellFormedXml = false;
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Creates a new instance of DocumentationComment that extracts information from
    /// the specified comment.
    /// </summary>
    /// <param name="origComment">Comment to extract the documentation from.</param>
    // --------------------------------------------------------------------------------
    public DocumentationComment(CommentInfo origComment)
    {
      _OriginalComment = origComment;
      _Text = ExtractDocumentation(origComment);
      _XmlDocument = new XmlDocument();
      try
      {
        _XmlDocument.LoadXml("<document>" + _Text + "</document>");
        _IsWellFormedXml = true;
      }
      catch (XmlException)
      {
        _IsWellFormedXml = false;
      }
    }

    #endregion

    #region Public properties

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets the original comment the documentation is extracted from.
    /// </summary>
    // --------------------------------------------------------------------------------
    public CommentInfo OriginalComment
    {
      get { return _OriginalComment; }
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets the XML documentation in text form.
    /// </summary>
    // --------------------------------------------------------------------------------
    public string Text
    {
      get { return _Text; }
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets the XmlDocument instance representing this document.
    /// </summary>
    // --------------------------------------------------------------------------------
    public XmlDocument XmlDocument
    {
      get { return _XmlDocument; }
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets the flag indicating if the document is a well formed XML.
    /// </summary>
    // --------------------------------------------------------------------------------
    public bool IsWellFormedXml
    {
      get { return _IsWellFormedXml; }
    }

    #endregion

    #region Private methods
    
    // --------------------------------------------------------------------------------
    /// <summary>
    /// Extracts the documentation from the specified comment.
    /// </summary>
    /// <param name="comment">Comment to extract the documentation from.</param>
    /// <returns>
    /// String representing the documentation.
    /// </returns>
    // --------------------------------------------------------------------------------
    private string ExtractDocumentation(CommentInfo comment)
    {
      // --- Block comments are not parsed forXML documentation comments.
      if (comment is BlockComment) return String.Empty;

      StringBuilder commentText = new StringBuilder(1000);
      
      // --- Single line XML comment?
      XmlCommentLine xmlLine = comment as XmlCommentLine;
      if (xmlLine != null)
      {
        AddXmlCommentLine(commentText, xmlLine);
        return commentText.ToString();
      }

      // --- Single XML block comment?
      XmlBlockComment xmlBlock = comment as XmlBlockComment;
      if (xmlBlock != null)
      {
        AddXmlBlock(commentText, xmlBlock);
        return commentText.ToString();
      }

      // --- Multiple comment block?
      MultiCommentBlock multiComment = comment as MultiCommentBlock;
      if (multiComment == null) return String.Empty;

      // --- Add elements of the multiple comment block to the XML doxcumentation
      foreach (CommentInfo entry in multiComment.Comments)
      {
        XmlCommentLine xmlLineEntry = entry as XmlCommentLine;
        if (xmlLineEntry != null)
        {
          AddXmlCommentLine(commentText, xmlLineEntry);
          continue;
        }
        XmlBlockComment xmlBlockEntry = entry as XmlBlockComment;
        if (xmlBlockEntry != null) AddXmlBlock(commentText, xmlBlockEntry);
      }
      return commentText.ToString();
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Adds the documentation from a comment block to the specified string builder.
    /// </summary>
    /// <param name="builder">String builder to use.</param>
    /// <param name="block">Comment block holding the documentation.</param>
    // --------------------------------------------------------------------------------
    private void AddXmlBlock(StringBuilder builder, CommentInfo block)
    {
      string[] lines = block.Text.Split('\r');
      foreach (string line in lines)
      {
        string reducedLine = line.TrimStart();
        if (reducedLine.StartsWith("*")) reducedLine = reducedLine.Substring(1);
        builder.Append(reducedLine);
        builder.Append("\r\n");
      }
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Adds the documentation from a comment line to the specified string builder.
    /// </summary>
    /// <param name="builder">String builder to use.</param>
    /// <param name="line">Comment line holding the documentation.</param>
    // --------------------------------------------------------------------------------
    private void AddXmlCommentLine(StringBuilder builder, CommentInfo line)
    {
      builder.Append(line.Text.TrimStart());
      builder.Append("\r\n");
    }

    #endregion
  }
}
