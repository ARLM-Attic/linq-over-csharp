using System.Xml;

namespace CSharpFactory.ProjectModel
{
  // ==================================================================================
  /// <summary>
  /// This class represents XML documentation tag having a "name" attribute (like
  /// param, typeparam, paramref, etc.) 
  /// </summary>
  // ==================================================================================
  public sealed class ParamTag : NamedDocumentationTag
  {
    // --------------------------------------------------------------------------------
    /// <summary>
    /// Creates a new instance of "param" tag from the specified XML node.
    /// </summary>
    /// <param name="node">XML node representing the documentation tag.</param>
    // --------------------------------------------------------------------------------
    public ParamTag(XmlNode node)
      : base(node)
    {
    }
  }
}