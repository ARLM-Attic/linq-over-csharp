using System;
using System.Collections.Generic;
using System.Text;

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
  }
}
