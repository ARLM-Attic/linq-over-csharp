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
    /// by constructing it from a template instance.
    /// </summary>
    /// <param name="template">The template for the new instance.</param>
    /// <param name="typeParameterMap">The type parameter map of the new instance.</param>
    // ----------------------------------------------------------------------------------------------
    private StructEntity(StructEntity template, TypeParameterMap typeParameterMap)
      : base(template, typeParameterMap)
    {
      IsPartial = template.IsPartial;
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
      return new StructEntity(this, typeParameterMap);
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Runs before adding a member.
    /// </summary>
    /// <param name="memberEntity">The member entity.</param>
    // ----------------------------------------------------------------------------------------------
    protected override void BeforeAddMember(IMemberEntity memberEntity)
    {
      // Member name and type name cannot be the same.
      if (memberEntity.Name == Name)
      {
        throw new TypeNameMemberNameConflictException(Name);
      }
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
        return IsGenericClone && DirectGenericTemplate == SemanticGraph.NullableGenericTypeDefinition; 
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
      visitor.Visit(this);
      base.AcceptVisitor(visitor);
    }

    #endregion
  }
}