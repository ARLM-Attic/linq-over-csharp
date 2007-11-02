using System.Collections.Generic;

namespace CSharpParser.ProjectModel
{
  // ==================================================================================
  /// <summary>
  /// This interface defines the behaviour of language elements that can have XML
  /// documentation comments.
  /// </summary>
  // ==================================================================================
  public interface ISupportsDocumentationComment
  {
    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets the documentation comment belonging to the language element.
    /// </summary>
    // --------------------------------------------------------------------------------
    DocumentationComment DocumentationComment { get; }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets the members of of this language element that are also can be documented.
    /// </summary>
    // --------------------------------------------------------------------------------
    IEnumerable<ISupportsDocumentationComment> DocumentableMembers { get; }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Processes and check the documentation comment belonging to the language 
    /// element.
    /// </summary>
    // --------------------------------------------------------------------------------
    void ProcessDocumentationComment();

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets the collection of formal parameters.
    /// </summary>
    // --------------------------------------------------------------------------------
    FormalParameterCollection FormalParameters { get; }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets the collection of type parameters.
    /// </summary>
    // --------------------------------------------------------------------------------
    TypeParameterCollection TypeParameters { get; }
  }
}
