using System.Xml;

namespace CSharpFactory.ProjectModel
{
  // ==================================================================================
  /// <summary>
  /// This class represents XML documentation tag having a "name" attribute (like
  /// param, typeparam, paramref, etc.) 
  /// </summary>
  // ==================================================================================
  public abstract class NamedDocumentationTag: DocumentationTag
  {
    private readonly string _Name;

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Creates a new instance of named documentation tag from the specified XML node.
    /// </summary>
    /// <param name="node">XML node representing the documentation tag.</param>
    // --------------------------------------------------------------------------------
    protected NamedDocumentationTag(XmlNode node)
      : base(node)
    {
      if (node == null) _Name = string.Empty;
      else
      {
        XmlAttribute nameAttr = node.Attributes["name"];
        if (nameAttr != null) _Name = nameAttr.Value;
      }
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets the value of the "name" attribute.
    /// </summary>
    // --------------------------------------------------------------------------------
    public string Name
    {
      get { return _Name; }
    }
  }
}