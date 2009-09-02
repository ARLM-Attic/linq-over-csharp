using System.Collections.Generic;
using CSharpTreeBuilder.Common;
using CSharpTreeBuilder.CSharpSemanticGraph;

namespace CSharpTreeBuilder.CSharpSemanticGraphBuilder
{
  // ================================================================================================
  /// <summary>
  /// This exception is raised when multiple matching declarations were found 
  /// for a search in a declaration space.
  /// </summary>
  // ================================================================================================
  public sealed class AmbiguousDeclarationsException : CSharpParserException
  {
    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Initializes a new instance of the <see cref="AmbiguousDeclarationsException"/> class.
    /// </summary>
    /// <param name="entities">The list of entities with ambiguous declarations.</param>
    // ----------------------------------------------------------------------------------------------
    public AmbiguousDeclarationsException(IEnumerable<INamedEntity> entities)
    {
      Entities = entities;
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets the iterate-only list of ambigous afully qualified name of the first entity found.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public IEnumerable<INamedEntity> Entities { get; private set; }
  }
}
