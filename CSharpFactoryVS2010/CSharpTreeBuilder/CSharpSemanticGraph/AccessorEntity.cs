using System;

namespace CSharpTreeBuilder.CSharpSemanticGraph
{
  // ================================================================================================
  /// <summary>
  /// This class represents an accessor. (eg. get accessor of a property)
  /// </summary>
  // ================================================================================================
  public sealed class AccessorEntity : SemanticEntity, IHasBody, IHasAccessibility
  {
    /// <summary>
    /// Backing field for Body property.
    /// </summary>
    private BlockEntity _Body;

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Initializes a new instance of the <see cref="AccessorEntity"/> class.
    /// </summary>
    /// <param name="accessibility">The declared accessibility of the member. Can be null.</param>
    /// <param name="isAbstract">True if the accessor has no body, false otherwise.</param>
    // ----------------------------------------------------------------------------------------------
    public AccessorEntity(AccessibilityKind? accessibility, bool isAbstract)
    {
      DeclaredAccessibility = accessibility;

      if (!isAbstract)
      {
        Body = new BlockEntity();
      }
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets or sets the body of the accessor.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public BlockEntity Body
    {
      get
      {
        return _Body;
      }
      
      set
      {
        _Body = value;
        if (_Body != null)
        {
          _Body.Parent = this;
        }
      }
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets a value indicating whether this accessor is abstract (ie. no implementation).
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public bool IsAbstract
    {
      get { return Body == null; }
    }

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

        // If no declared accessibility then the default is the parent's effective accessibility.
        var parentMember = Parent as FunctionMemberWithAccessorsEntity;
        return parentMember == null ? null : parentMember.EffectiveAccessibility;
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
      var parentMember = Parent as FunctionMemberWithAccessorsEntity;
      
      if (parentMember == null)
      {
        return false;
      }

      var parentType = parentMember.Parent as TypeEntity;

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

  }
}
