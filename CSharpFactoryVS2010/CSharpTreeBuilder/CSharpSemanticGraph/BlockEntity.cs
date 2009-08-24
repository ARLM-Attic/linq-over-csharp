using System.Collections.Generic;

namespace CSharpTreeBuilder.CSharpSemanticGraph
{
  // ================================================================================================
  /// <summary>
  /// This class represents a block (a collection of statements) in the semantic graph.
  /// </summary>
  // ================================================================================================
  public sealed class BlockEntity : StatementEntity 
  {
    /// <summary>Backing field for Statements property.</summary>
    private List<StatementEntity> _Statements;

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Initializes a new instance of the <see cref="BlockEntity"/> class.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public BlockEntity()
    {
      _Statements = new List<StatementEntity>();
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets an iterate-only collection of the statements in the block.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public IEnumerable<StatementEntity> Statements
    {
      get
      {
        return _Statements;
      }
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Adds a statement to the block.
    /// </summary>
    /// <param name="statementEntity">A statement.</param>
    // ----------------------------------------------------------------------------------------------
    public void AddStatement(StatementEntity statementEntity)
    {
      _Statements.Add(statementEntity);
      statementEntity.Parent = this;
    }
  }
}
