using CSharpTreeBuilder.CSharpSemanticGraphBuilder;

namespace CSharpTreeBuilder.CSharpSemanticGraph
{
  // ================================================================================================
  /// <summary>
  /// This class represents a local variable.
  /// </summary>
  // ================================================================================================
  public sealed class LocalVariableEntity : NonFieldVariableEntity
  {
    #region State

    /// <summary>Gets a value indicating whether this is a constant.</summary>
    public bool IsConstant { get; private set; }

    #endregion

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Initializes a new instance of the <see cref="LocalVariableEntity"/> class.
    /// </summary>
    /// <param name="name">The name of the variable.</param>
    /// <param name="type">The type of the variable (a type entity reference).</param>
    /// <param name="isConstant">A value indicating whether this is a constant.</param>
    /// <param name="initializer">The initializer of the variable. Can be null.</param>
    // ----------------------------------------------------------------------------------------------
    public LocalVariableEntity(string name, Resolver<TypeEntity> type, VariableInitializer initializer, bool isConstant)
      : base (name, type, initializer)
    {
      IsConstant = isConstant;
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Initializes a new instance of the <see cref="LocalVariableEntity"/> class 
    /// by constructing it from a template instance.
    /// </summary>
    /// <param name="template">The template for the new instance.</param>
    /// <param name="typeParameterMap">The type parameter map of the new instance.</param>
    // ----------------------------------------------------------------------------------------------
    private LocalVariableEntity(LocalVariableEntity template, TypeParameterMap typeParameterMap)
      : base(template, typeParameterMap)
    {
      IsConstant = template.IsConstant;
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
      return new LocalVariableEntity(this, typeParameterMap);
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
