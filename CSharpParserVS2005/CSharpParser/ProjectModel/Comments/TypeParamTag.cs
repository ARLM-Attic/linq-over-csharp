using System.Xml;

namespace CSharpParser.ProjectModel
{
  // ==================================================================================
  /// <summary>
  /// This class represents XML documentation tag having a "name" attribute (like
  /// param, typeparam, paramref, etc.) 
  /// </summary>
  // ==================================================================================
  public sealed class TypeParamTag : NamedDocumentationTag
  {
    // --------------------------------------------------------------------------------
    /// <summary>
    /// Creates a new instance of "typeparam" tag from the specified XML node.
    /// </summary>
    /// <param name="node">XML node representing the documentation tag.</param>
    // --------------------------------------------------------------------------------
    public TypeParamTag(XmlNode node)
      : base(node)
    {
    }
  }
}