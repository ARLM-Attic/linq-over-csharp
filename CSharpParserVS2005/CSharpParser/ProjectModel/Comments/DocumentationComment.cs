using System;
using System.Collections.Generic;
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
    private readonly string _BadlyFormedReason;
    private bool _TagsExtracted;
    private DocumentationTag _Summary;
    private DocumentationTag _Remarks;
    private DocumentationTag _Returns;
    private DocumentationTag _Value;
    private readonly List<ParamTag> _Parameters = new List<ParamTag>();
    private readonly List<TypeParamTag> _TypeParameters = new List<TypeParamTag>();

    #endregion

    #region Static fields

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Reresents an empty documentation comment.
    /// </summary>
    // --------------------------------------------------------------------------------
    public static DocumentationComment EmptyComment = new DocumentationComment();

    #endregion

    #region Lifecycle methods

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Creates an empty documentation comment.
    /// </summary>
    // --------------------------------------------------------------------------------
    private DocumentationComment()
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
      catch (XmlException ex)
      {
        _BadlyFormedReason = ex.Message;
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
    /// Gets the flag indicating if the documentation comment is empty.
    /// </summary>
    // --------------------------------------------------------------------------------
    public bool IsEmpty
    {
      get { return _OriginalComment == null; }
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

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets the reason why the XML text is badly formatted.
    /// </summary>
    // --------------------------------------------------------------------------------
    public string BadlyFormedReason
    {
      get { return _BadlyFormedReason; }
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets the "summary" part of the documentation.
    /// </summary>
    // --------------------------------------------------------------------------------
    public DocumentationTag Summary
    {
      get
      {
        ExtractTags();
        return _Summary;
      }
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets the "remarks" part of the documentation.
    /// </summary>
    // --------------------------------------------------------------------------------
    public DocumentationTag Remarks
    {
      get
      {
        ExtractTags();
        return _Remarks;
      }
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets the "returns" part of the documentation.
    /// </summary>
    // --------------------------------------------------------------------------------
    public DocumentationTag Returns
    {
      get
      {
        ExtractTags();
        return _Returns;
      }
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets the "value" part of the documentation.
    /// </summary>
    // --------------------------------------------------------------------------------
    public DocumentationTag Value
    {
      get
      {
        ExtractTags();
        return _Value;
      }
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets the list of parameter documentation.
    /// </summary>
    // --------------------------------------------------------------------------------
    public List<ParamTag> Parameters
    {
      get
      {
        ExtractTags();
        return _Parameters;
      }
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets the list of type parameter documentation.
    /// </summary>
    // --------------------------------------------------------------------------------
    public List<TypeParamTag> TypeParameters
    {
      get
      {
        ExtractTags();
        return _TypeParameters;
      }
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

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Extracts tags from the XML documentation.
    /// </summary>
    // --------------------------------------------------------------------------------
    private void ExtractTags()
    {
      if (_TagsExtracted) return;
      
      // --- Init tags
      _Summary = new DocumentationTag(null);
      _Remarks = new DocumentationTag(null);
      _Returns = new DocumentationTag(null);
      _Value = new DocumentationTag(null);
      if (!IsWellFormedXml) return;

      // --- Extract information from tags
      foreach (XmlNode node in _XmlDocument.DocumentElement.ChildNodes)
      {
        if (node.LocalName == "summary") _Summary = new DocumentationTag(node);
        else if (node.LocalName == "remarks") _Remarks = new DocumentationTag(node);
        else if (node.LocalName == "returns") _Returns = new DocumentationTag(node);
        else if (node.LocalName == "value") _Value = new DocumentationTag(node);
        else if (node.LocalName == "param") _Parameters.Add(new ParamTag(node));
        else if (node.LocalName == "typeparam") _TypeParameters.Add(new TypeParamTag(node));

        // TODO: Extract other tags
      }
      _TagsExtracted = true;
    }

    #endregion
  }
}
