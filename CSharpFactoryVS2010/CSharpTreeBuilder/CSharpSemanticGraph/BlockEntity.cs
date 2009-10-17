using System.Linq;
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
    private readonly List<StatementEntity> _Statements = new List<StatementEntity>();

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Initializes a new instance of the <see cref="BlockEntity"/> class.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public BlockEntity()
    {
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Initializes a new instance of the <see cref="BlockEntity"/> class 
    /// by deep copying from another instance.
    /// </summary>
    /// <param name="source">The object whose state will be copied to the new object.</param>
    // ----------------------------------------------------------------------------------------------
    public BlockEntity(BlockEntity source)
      : base(source)
    {
      _Statements.AddRange(source._Statements.Select(x => x.Clone()).Cast<StatementEntity>());
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Creates a deep copy of the semantic subtree starting at this entity.
    /// </summary>
    /// <returns>The deep clone of this entity and its semantic subtree.</returns>
    // ----------------------------------------------------------------------------------------------
    public override object Clone()
    {
      return new BlockEntity(this);
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
