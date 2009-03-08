using System;
using System.Xml;

namespace CSharpFactory.ProjectModel
{
  // ==================================================================================
  /// <summary>
  /// This class represents XML documentation tag (like summary, remarks, etc.
  /// </summary>
  // ==================================================================================
  public class DocumentationTag
  {
    private readonly string _Text;
    private readonly XmlNode _Node;
    private readonly string _XmlText;

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Creates a new instance of documentation tag from the specified XML node.
    /// </summary>
    /// <param name="node">XML node representing the documentation tag.</param>
    // --------------------------------------------------------------------------------
    public DocumentationTag(XmlNode node)
    {
      if (node == null)
      {
        _Text = String.Empty;
        _Node = null;
        _XmlText = String.Empty;
      }
      else
      {
        _Node = node;
        _Text = node.InnerText;
        _XmlText = node.InnerXml;
      }
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets the text form belonging to the documentation tag.
    /// </summary>
    // --------------------------------------------------------------------------------
    public string Text
    {
      get { return _Text; }
    }


    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets the XML node representing this documentation tag.
    /// </summary>
    // --------------------------------------------------------------------------------
    public XmlNode Node
    {
      get { return _Node; }
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets the XML text form belonging to the documentation tag.
    /// </summary>
    // --------------------------------------------------------------------------------
    public string XmlText
    {
      get { return _XmlText; }
    }
  }
}
