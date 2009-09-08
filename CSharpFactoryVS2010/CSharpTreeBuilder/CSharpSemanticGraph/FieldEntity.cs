using CSharpTreeBuilder.CSharpSemanticGraphBuilder;

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
    /// <param name="initializer">The initializer of the field.</param>
    // ----------------------------------------------------------------------------------------------
    public FieldEntity(
      string name, 
      bool isExplicitlyDefined, 
      SemanticEntityReference<TypeEntity> type,
      bool isStatic,
      IVariableInitializer initializer)
      : 
      base(name, isExplicitlyDefined)
    {
      TypeReference = type;
      IsStatic = isStatic;
      Initializer = initializer;
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets the type of the field.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public SemanticEntityReference<TypeEntity> TypeReference { get; private set; }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets the type of the field.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public TypeEntity Type
    {
      get { return TypeReference == null ? null : TypeReference.TargetEntity; }
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets a value indicating whether this variable is an array. 
    /// Null if the type of the variable is not yet resolved.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public bool? IsArray
    {
      get
      {
        return Type == null ? null : Type.IsArrayType as bool?;
      }
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets the initializer of the variable.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public IVariableInitializer Initializer { get; private set; }

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
      if (!visitor.Visit(this)) { return; }

      if (Initializer!=null)
      {
        Initializer.AcceptVisitor(visitor);
      }
    }

    #endregion
  }
}
