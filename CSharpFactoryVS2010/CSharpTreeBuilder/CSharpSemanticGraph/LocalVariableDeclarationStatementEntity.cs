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
    #region State

    /// <summary>Backing field for Variables property.</summary>
    private readonly List<LocalVariableEntity> _Variables = new List<LocalVariableEntity>();

    #endregion

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Initializes a new instance of the <see cref="LocalVariableDeclarationStatementEntity"/> class.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public LocalVariableDeclarationStatementEntity()
    {
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Initializes a new instance of the <see cref="LocalVariableDeclarationStatementEntity"/> class 
    /// by constructing it from a template instance.
    /// </summary>
    /// <param name="template">The template for the new instance.</param>
    /// <param name="typeParameterMap">The type parameter map of the new instance.</param>
    // ----------------------------------------------------------------------------------------------
    private LocalVariableDeclarationStatementEntity(LocalVariableDeclarationStatementEntity template, TypeParameterMap typeParameterMap)
      : base(template, typeParameterMap)
    {
      foreach(var variable in template.Variables)
      {
        _Variables.Add((LocalVariableEntity) variable.GetGenericClone(typeParameterMap));
      }
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
      return new LocalVariableDeclarationStatementEntity(this, typeParameterMap);
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

      foreach (var variable in Variables)
      {
        variable.AcceptVisitor(visitor);
      }
    }

    #endregion
  }
}
