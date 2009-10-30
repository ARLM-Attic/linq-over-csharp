using System;

namespace CSharpTreeBuilder.CSharpSemanticGraph
{
  // ================================================================================================
  /// <summary>
  /// This abstract class represents a member of a type that is not a nested type (field, method, etc.)
  /// </summary>
  // ================================================================================================
  public abstract class NonTypeMemberEntity : SemanticEntity, IMemberEntity
  {
    #region State

    /// <summary>
    /// Backing field for IsStatic property.
    /// </summary>
    /// <remarks>
    /// Not using auto-implemented property to avoid "virtual member call in constructor" warnings
    /// in descendant classes that want to initiliaze this property in the constructor.
    /// </remarks>
    protected bool _IsStatic;
    

    /// <summary>Gets or sets the name of the member.</summary>
    public string Name { get; private set; }

    /// <summary>
    /// Gets or sets a value indicating whether the member is explicitly declared in source code.
    /// </summary>
    /// <remarks>
    /// False for members created by the parser (eg. a backing field for an auto-implemented property)
    /// and for members created from reflected metadata.
    /// </remarks>
    public bool IsDeclaredInSource { get; private set; }

    /// <summary>Gets or sets the declared accessibility of the entity.</summary>
    public AccessibilityKind? DeclaredAccessibility { get; set; }

    /// <summary>Gets or sets a value indicating whether this member intentionally hides an inherited member.</summary>
    public bool IsNew { get; set; }

    #endregion

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Initializes a new instance of the <see cref="NonTypeMemberEntity"/> class.
    /// </summary>
    /// <param name="isDeclaredInSource">True if the member is explicitly declared in source code, false otherwise.</param>
    /// <param name="accessibility">The declared accessibility of the member. Can be null.</param>
    /// <param name="name">The name of the member.</param>
    // ----------------------------------------------------------------------------------------------
    protected NonTypeMemberEntity(bool isDeclaredInSource, AccessibilityKind? accessibility, string name)
    {
      IsDeclaredInSource = isDeclaredInSource;
      DeclaredAccessibility = accessibility;
      Name = name;
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Initializes a new instance of the <see cref="NonTypeMemberEntity"/> class 
    /// by constructing it from a template instance.
    /// </summary>
    /// <param name="template">The template for the new instance.</param>
    /// <param name="typeParameterMap">The type parameter map of the new instance.</param>
    // ----------------------------------------------------------------------------------------------
    protected NonTypeMemberEntity(NonTypeMemberEntity template, TypeParameterMap typeParameterMap)
      : base(template, typeParameterMap)
    {
      _IsStatic = template._IsStatic;

      Name = template.Name;
      IsDeclaredInSource = template.IsDeclaredInSource;
      DeclaredAccessibility = template.DeclaredAccessibility;
      IsNew = template.IsNew;
    }
    
    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets the fully qualified name of the member.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public string FullyQualifiedName
    {
      get
      {
        return Parent is TypeEntity
          ? string.Format("{0}.{1}", (Parent as TypeEntity).FullyQualifiedName, Name)
          : Name;
      }
    }
    
    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets the effective accessibility of the entity.
    /// </summary>
    /// <remarks>
    /// If there's no declared accessibility then returns the default accessibility.
    /// If the default cannot be determined (eg. no parent entity) then returns null.
    /// </remarks>
    // ----------------------------------------------------------------------------------------------
    public AccessibilityKind? EffectiveAccessibility
    {
      get
      {
        if (DeclaredAccessibility != null)
        {
          return DeclaredAccessibility.Value;
        }

        // If no declared accessibility then the default has to be returned,
        // which is based on the type of the containing type.
        if (Parent is ClassEntity || Parent is StructEntity)
        {
          return AccessibilityKind.Private;
        }

        if (Parent is InterfaceEntity || Parent is EnumEntity)
        {
          return AccessibilityKind.Public;
        }

        // If the default accessibility cannot be determined then return null.
        return null;
      }
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets a value indicating whether this entity is accessible by another entity.
    /// </summary>
    /// <param name="accessingEntity">The accessing entity.</param>
    /// <returns>True if the accessing entity can access this entity, false otherwise.</returns>
    // ----------------------------------------------------------------------------------------------
    public bool IsAccessibleBy(SemanticEntity accessingEntity)
    {
      // First check the accessibility of the parent type.
      var parentType = Parent as TypeEntity;

      if (parentType == null || !parentType.IsAccessibleBy(accessingEntity))
      {
        return false;
      }

      // Then check the accessibility of this entity.
      switch (EffectiveAccessibility)
      {
        case AccessibilityKind.Public:
          return true;

        case AccessibilityKind.Assembly:
          return accessingEntity.Program == this.Program;

        case AccessibilityKind.FamilyOrAssembly:
          return accessingEntity.Program == this.Program || parentType.ContainsInFamily(accessingEntity);

        case AccessibilityKind.Family:
          return parentType.ContainsInFamily(accessingEntity);

        case AccessibilityKind.Private:
          return parentType.IsParentOf(accessingEntity);

        default:
          throw new ApplicationException("Effective accessibility is undefined.");
      }
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets or sets a value indicating whether this member belongs to the class itself
    /// (as opposed to an instance object).
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public virtual bool IsStatic
    {
      get { return _IsStatic; }
      set { _IsStatic = value; }
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets a value indicating whether this member is invocable.
    /// </summary>
    /// <remarks>A member is invocable if it's a method or event, 
    /// or if it is a constant, field or property of a delegate type.</remarks>
    // ----------------------------------------------------------------------------------------------
    public virtual bool IsInvocable 
    {
      get { return false; } 
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets the string representation of the object.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public override string ToString()
    {
      var result = (this is IOverloadableEntity) ? (this as IOverloadableEntity).Signature.ToString() : Name;

      return Parent == null ? result : Parent.ToString() + "_" + result;
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
