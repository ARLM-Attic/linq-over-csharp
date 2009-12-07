using System.Collections.Generic;

namespace CSharpTreeBuilder.CSharpSemanticGraph
{
  // ================================================================================================
  /// <summary>
  /// This interface is implemented by semantic entities that can have statements as child entities.
  /// </summary>
  // ================================================================================================
  public interface IHasStatements : ISemanticEntity
  {
    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Adds a child statement. 
    /// </summary>
    /// <param name="statementEntity">A statement entity.</param>
    // ----------------------------------------------------------------------------------------------
    void AddStatement(StatementEntity statementEntity);

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets an iterate-only collection of child statements.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    IEnumerable<StatementEntity> Statements { get; }
  }
}
