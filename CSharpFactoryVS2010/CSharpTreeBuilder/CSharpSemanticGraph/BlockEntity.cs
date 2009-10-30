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
    /// by constructing it from a template instance.
    /// </summary>
    /// <param name="template">The template for the new instance.</param>
    /// <param name="typeParameterMap">The type parameter map of the new instance.</param>
    // ----------------------------------------------------------------------------------------------
    private BlockEntity(BlockEntity template, TypeParameterMap typeParameterMap)
      : base(template, typeParameterMap)
    {
      _Statements.AddRange(template._Statements.Select(x => x.GetConstructedEntity(typeParameterMap)).Cast<StatementEntity>());
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Creates a new constructed entity.
    /// </summary>
    /// <param name="typeParameterMap">A collection of type parameters and associated type arguments.</param>
    /// <returns>
    /// A new semantic entity constructed from this entity using the specified type parameter map.
    /// </returns>
    // ----------------------------------------------------------------------------------------------
    protected override SemanticEntity ConstructNew(TypeParameterMap typeParameterMap)
    {
      return new BlockEntity(this, typeParameterMap);
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

    #region Visitor methods

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Accepts a visitor object, according to the Visitor pattern.
    /// </summary>
    /// <param name="visitor">A visitor object</param>
    // ----------------------------------------------------------------------------------------------
    public override void AcceptVisitor(SemanticGraphVisitor visitor)
    {
      visitor.Visit(this);
      base.AcceptVisitor(visitor);
    }

    #endregion
  }
}
