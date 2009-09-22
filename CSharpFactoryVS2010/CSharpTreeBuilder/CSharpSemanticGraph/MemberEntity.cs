using System;

namespace CSharpTreeBuilder.CSharpSemanticGraph
{
  // ================================================================================================
  /// <summary>
  /// This abstract class represents a member of a type.
  /// </summary>
  // ================================================================================================
  public abstract class MemberEntity : SemanticEntity, INamedEntity, IHasAccessibility
  {
    /// <summary>
    /// Backing field for IsStatic property.
    /// </summary>
    /// <remarks>
    /// Not using auto-implemented property to avoid "virtual member call in constructor" warnings
    /// in descendant classes that want to initiliaze this property in the constructor.
    /// </remarks>
    protected bool _IsStatic;

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Initializes a new instance of the <see cref="MemberEntity"/> class.
    /// </summary>
    /// <param name="isExplicitlyDefined">True, if the member is explicitly defined, false otherwise.</param>
    /// <param name="accessibility">The declared accessibility of the member. Can be null.</param>
    /// <param name="name">The name of the member.</param>
    // ----------------------------------------------------------------------------------------------
    protected MemberEntity(bool isExplicitlyDefined, AccessibilityKind? accessibility, string name)
    {
      IsExplicitlyDefined = isExplicitlyDefined;
      DeclaredAccessibility = accessibility;
      Name = name;
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets or sets the name of the member.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public string Name { get; private set; }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets or sets a value indicating whether the member is explicitly defined, or created by the parser.
    /// Eg. value types have a default constructor which is implicitly declared.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public bool IsExplicitlyDefined { get; private set; }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets or sets the declared accessibility of the entity.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public AccessibilityKind? DeclaredAccessibility { get; set; }

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
        if (Parent != null && Parent is TypeEntity)
        {
          var parentType = Parent as TypeEntity;

          if (parentType.IsClassType || parentType.IsStructType)
          {
            return AccessibilityKind.Private;
          }

          if (parentType.IsInterfaceType || parentType.IsEnumType)
          {
            return AccessibilityKind.Public;
          }
        }

        // If the default accessibility cannot be determined then return null.
        return null;
      }
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets a value indicating whether this entity is accessible by another entity.
    /// </summary>
    /// <param name="entity">The accessing entity.</param>
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
          return parentType.Contains(accessingEntity);

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
    /// Gets or sets a value indicating whether this member can be overridden.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public virtual bool IsVirtual { get; set; }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets or sets a value indicating whether this member is on override of an inherited member.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public virtual bool IsOverride { get; set; }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets or sets a value indicating whether this member intentionally hides an inherited member.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public virtual bool IsNew { get; set; }

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
  }
}
