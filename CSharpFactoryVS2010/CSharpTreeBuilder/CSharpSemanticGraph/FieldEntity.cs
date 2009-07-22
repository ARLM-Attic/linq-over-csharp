namespace CSharpTreeBuilder.CSharpSemanticGraph
{
  // ================================================================================================
  /// <summary>
  /// This class represents a field of a type.
  /// </summary>
  // ================================================================================================
  public class FieldEntity : MemberEntity, IVariableEntity
  {
    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Initializes a new instance of the <see cref="FieldEntity"/> class.
    /// </summary>
    /// <param name="name">The name of the member.</param>
    /// <param name="isExplicitlyDefined">True, if the member is explicitly defined, false otherwise.</param>
    /// <param name="type">The type of the field (a type entity reference).</param>
    /// <param name="isStatic">True, if the field is static, false otherwise.</param>
    // ----------------------------------------------------------------------------------------------
    public FieldEntity(string name, bool isExplicitlyDefined, TypeEntityReference type, bool isStatic)
      : base(name, isExplicitlyDefined)
    {
      Type = type;
      IsStatic = isStatic;
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets the type of the field.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public TypeEntityReference Type { get; private set; }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets a value indicating whether the field is static.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public bool IsStatic { get; private set; }

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
    }

    #endregion
  }
}
