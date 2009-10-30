using System.Collections.Generic;

namespace CSharpTreeBuilder.CSharpSemanticGraph
{
  // ================================================================================================
  /// <summary>
  /// This class represents a local variable (or constant) declaration statement entity.
  /// </summary>
  // ================================================================================================
  public sealed class LocalVariableDeclarationStatementEntity : StatementEntity 
  {
    /// <summary>Backing field for Variables property.</summary>
    private readonly List<LocalVariableEntity> _Variables;

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Initializes a new instance of the <see cref="LocalVariableDeclarationStatementEntity"/> class.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public LocalVariableDeclarationStatementEntity()
    {
      _Variables = new List<LocalVariableEntity>();
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets an iterate-only collection of the variables declared in this statement.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public IEnumerable<LocalVariableEntity> Variables
    {
      get
      {
        return _Variables;
      }
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Adds a variable to the declaration.
    /// </summary>
    /// <param name="variableEntity">A local variable entity.</param>
    // ----------------------------------------------------------------------------------------------
    public void AddVariable(LocalVariableEntity variableEntity)
    {
      _Variables.Add(variableEntity);
      variableEntity.Parent = this;
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
