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
    public SemanticEntityReference<TypeEntity> ReturnTypeReference { get; set; }

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
    /// <param name="resolveTypeParameters">True to resolve type parameters immediately, false to defer it.</param>
    // ----------------------------------------------------------------------------------------------
    private DelegateEntity(DelegateEntity template, TypeParameterMap typeParameterMap, bool resolveTypeParameters)
      : base(template, typeParameterMap, resolveTypeParameters)
    {
      ReturnTypeReference = template.ReturnTypeReference;
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Creates a new constructed entity.
    /// </summary>
    /// <param name="typeParameterMap">A collection of type parameters and associated type arguments.</param>
    /// <param name="resolveTypeParameters">True to resolve type parameters during construction, 
    /// false to defer it to a later phase.</param>
    /// <returns>
    /// A new semantic entity constructed from this entity using the specified type parameter map.
    /// </returns>
    // ----------------------------------------------------------------------------------------------
    protected override SemanticEntity ConstructNew(TypeParameterMap typeParameterMap, bool resolveTypeParameters)
    {
      return new DelegateEntity(this, typeParameterMap, resolveTypeParameters);
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
        return ReturnTypeReference == null ? null : ReturnTypeReference.TargetEntity;
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
      if (!visitor.Visit(this)) { return; }

      base.AcceptVisitor(visitor);
    }

    #endregion
  }
}