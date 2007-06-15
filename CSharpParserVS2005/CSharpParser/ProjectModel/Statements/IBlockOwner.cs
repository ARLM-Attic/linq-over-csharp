using System.Collections.Generic;

namespace CSharpParser.ProjectModel
{
  // ==================================================================================
  /// <summary>
  /// This interface marks all the language elements that can have statement blocks
  /// within themselves.
  /// </summary>
  // ==================================================================================
  public interface IBlockOwner
  {
    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets the statement block belonging to the language element.
    /// </summary>
    // --------------------------------------------------------------------------------
    List<Statement> Statements { get; }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Adds a new statement to the statement block.
    /// </summary>
    /// <param name="statement">Statement to add.</param>
    // --------------------------------------------------------------------------------
    void Add(Statement statement);
  }
}
