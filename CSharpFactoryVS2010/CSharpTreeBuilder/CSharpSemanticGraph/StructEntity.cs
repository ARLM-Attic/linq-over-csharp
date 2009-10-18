using System.Linq;
using CSharpTreeBuilder.CSharpSemanticGraphBuilder;

namespace CSharpTreeBuilder.CSharpSemanticGraph
{
  // ================================================================================================
  /// <summary>
  /// This class represents a struct entity in the semantic graph.
  /// </summary>
  // ================================================================================================
  public sealed class StructEntity : ChildTypeCapableTypeEntity, ICanBePartial
  {
    #region State

    /// <summary>Gets a value indicating whether this entity is declared as partial. </summary>
    public bool IsPartial { get; private set; }

    #endregion

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Initializes a new instance of the <see cref="StructEntity"/> class.
    /// </summary>
    /// <param name="accessibility">The declared accessibility of the member. Can be null.</param>
    /// <param name="name">The name of the entity.</param>
    /// <param name="isPartial">A value indicating whether this type was declared as partial.</param>
    // ----------------------------------------------------------------------------------------------
    public StructEntity(AccessibilityKind? accessibility, string name, bool isPartial)
      : base(accessibility, name)
    {
      IsPartial = isPartial;
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Initializes a new instance of the <see cref="StructEntity"/> class with no partial.
    /// </summary>
    /// <param name="accessibility">The declared accessibility of the member. Can be null.</param>
    /// <param name="name">The name of the entity.</param>
    // ----------------------------------------------------------------------------------------------
    public StructEntity(AccessibilityKind? accessibility, string name)
      : this(accessibility, name, false)
    {
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Initializes a new instance of the <see cref="StructEntity"/> class 
    /// by deep copying from another instance.
    /// </summary>
    /// <param name="source">The object whose state will be copied to the new object.</param>
    // ----------------------------------------------------------------------------------------------
    public StructEntity(StructEntity source)
      : base(source)
    {
      IsPartial = source.IsPartial;
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Creates a deep copy of the semantic subtree starting at this entity.
    /// </summary>
    /// <returns>The deep clone of this entity and its semantic subtree.</returns>
    // ----------------------------------------------------------------------------------------------
    public override object Clone()
    {
      return new StructEntity(this);
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Adds a member. 
    /// Also sets the member's parent property, and defines member's name in the declaration space.
    /// </summary>
    /// <param name="memberEntity">The member entity.</param>
    // ----------------------------------------------------------------------------------------------
    public override void AddMember(IMemberEntity memberEntity)
    {
      // Member name and type name cannot be the same.
      if (memberEntity.Name == Name)
      {
        throw new TypeNameMemberNameConflictException(Name);
      }

      base.AddMember(memberEntity);
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
    /// Gets a value indicating whether this is a nullable type.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public override bool IsNullableType
    {
      get 
      { 
        return IsConstructed && TemplateEntity == SemanticGraph.NullableGenericTypeDefinition; 
      }
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets the underlying type of a nullable type.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public override TypeEntity UnderlyingOfNullableType
    {
      get 
      { 
        return IsNullableType 
          ? TypeParameterMap.TypeArguments.FirstOrDefault()
          : null; 
      }
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