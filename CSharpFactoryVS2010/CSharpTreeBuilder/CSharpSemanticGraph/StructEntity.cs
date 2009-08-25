namespace CSharpTreeBuilder.CSharpSemanticGraph
{
  // ================================================================================================
  /// <summary>
  /// This class represents a struct entity in the semantic graph.
  /// </summary>
  // ================================================================================================
  public sealed class StructEntity : ChildTypeCapableTypeEntity
  {
    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Initializes a new instance of the <see cref="StructEntity"/> class.
    /// </summary>
    /// <param name="name">The name of the entity.</param>
    /// <param name="isPartial">A value indicating whether this type was declared as partial.</param>
    // ----------------------------------------------------------------------------------------------
    public StructEntity(string name, bool isPartial)
      : base(name)
    {
      _IsPartial = isPartial;
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Initializes a new instance of the <see cref="StructEntity"/> class with no partial.
    /// </summary>
    /// <param name="name">The name of the entity.</param>
    // ----------------------------------------------------------------------------------------------
    public StructEntity(string name)
      : this(name, false)
    {
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets a value indicating whether this type is a value type.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public override bool IsValueType
    {
      get { return true; }
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets a value indicating whether this type is a struct type.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public override bool IsStructType
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
      visitor.Visit(this);

      base.AcceptVisitor(visitor);
    }

    #endregion
  }
}