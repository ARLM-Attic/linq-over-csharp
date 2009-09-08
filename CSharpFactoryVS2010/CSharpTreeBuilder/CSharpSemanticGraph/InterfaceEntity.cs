namespace CSharpTreeBuilder.CSharpSemanticGraph
{
  // ================================================================================================
  /// <summary>
  /// This class represents an interface entity in the semantic graph.
  /// </summary>
  // ================================================================================================
  public sealed class InterfaceEntity : GenericCapableTypeEntity, ICanBePartial
  {
    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Initializes a new instance of the <see cref="InterfaceEntity"/> class.
    /// </summary>
    /// <param name="name">The name of the entity.</param>
    /// <param name="isPartial">A value indicating whether this type was declared as partial.</param>
    // ----------------------------------------------------------------------------------------------
    public InterfaceEntity(string name, bool isPartial)
      : base(name)
    {
      IsPartial = isPartial;
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Initializes a new instance of the <see cref="InterfaceEntity"/> class with no partial.
    /// </summary>
    /// <param name="name">The name of the entity.</param>
    // ----------------------------------------------------------------------------------------------
    public InterfaceEntity(string name)
      : this(name, false)
    {
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets a value indicating whether this entity is declared as partial. 
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public bool IsPartial { get; private set; }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets a value indicating whether this type is a reference type.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public override bool IsReferenceType
    {
      get { return true; }
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets a value indicating whether this type is an interface type.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public override bool IsInterfaceType
    {
      get { return true; }
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
      if (!visitor.Visit(this)) { return; }

      base.AcceptVisitor(visitor);
    }

    #endregion
  }
}