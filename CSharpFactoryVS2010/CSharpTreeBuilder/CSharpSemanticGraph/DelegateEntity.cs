using CSharpTreeBuilder.CSharpSemanticGraphBuilder;

namespace CSharpTreeBuilder.CSharpSemanticGraph
{
  // ================================================================================================
  /// <summary>
  /// This class represents a delegate entity in the semantic graph.
  /// </summary>
  // ================================================================================================
  public sealed class DelegateEntity : GenericCapableTypeEntity
  {
    #region State

    /// <summary>Gets or sets the reference to the return type.</summary>
    public Resolver<TypeEntity> ReturnTypeReference { get; set; }

    #endregion
   
    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Initializes a new instance of the <see cref="DelegateEntity"/> class.
    /// </summary>
    /// <param name="accessibility">The declared accessibility of the member. Can be null.</param>
    /// <param name="name">The name of the entity.</param>
    // ----------------------------------------------------------------------------------------------
    public DelegateEntity(AccessibilityKind? accessibility, string name)
      : base(accessibility, name)
    {
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Initializes a new instance of the <see cref="DelegateEntity"/> class 
    /// by constructing it from a template instance.
    /// </summary>
    /// <param name="template">The template for the new instance.</param>
    /// <param name="typeParameterMap">The type parameter map of the new instance.</param>
    // ----------------------------------------------------------------------------------------------
    private DelegateEntity(DelegateEntity template, TypeParameterMap typeParameterMap)
      : base(template, typeParameterMap)
    {
      if (template.ReturnTypeReference != null)
      {
        ReturnTypeReference = (Resolver<TypeEntity>)template.ReturnTypeReference.GetGenericClone(typeParameterMap);
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
      return new DelegateEntity(this, typeParameterMap);
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets or sets the return type.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public TypeEntity ReturnType 
    {
      get
      {
        return ReturnTypeReference != null ? ReturnTypeReference.Target : null;
      }
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets a value indicating whether this type is a reference type.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public override bool IsReferenceType
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