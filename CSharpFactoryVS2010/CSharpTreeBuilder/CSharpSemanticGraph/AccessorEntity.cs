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
    #region State

    /// <summary>Backing field for Body property.</summary>
    private BlockStatementEntity _Body;

    /// <summary>Gets or sets the accessor kind.</summary>
    public AccessorKind AccessorKind { get; private set; }

    /// <summary>Gets or sets the declared accessibility of the entity.</summary>
    public AccessibilityKind? DeclaredAccessibility { get; set; }

    #endregion

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Initializes a new instance of the <see cref="AccessorEntity"/> class.
    /// </summary>
    /// <param name="accessorKind">The kind of the accessor.</param>
    /// <param name="accessibility">The declared accessibility of the member. Can be null.</param>
    /// <param name="isAbstract">True if the accessor has no body, false otherwise.</param>
    // ----------------------------------------------------------------------------------------------
    public AccessorEntity(AccessorKind accessorKind, AccessibilityKind? accessibility, bool isAbstract)
    {
      AccessorKind = accessorKind;
      DeclaredAccessibility = accessibility;

      if (!isAbstract)
      {
        Body = new BlockStatementEntity();
      }
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Initializes a new instance of the <see cref="AccessorEntity"/> class 
    /// by constructing it from a template instance.
    /// </summary>
    /// <param name="template">The template for the new instance.</param>
    /// <param name="typeParameterMap">The type parameter map of the new instance.</param>
    // ----------------------------------------------------------------------------------------------
    private AccessorEntity(AccessorEntity template, TypeParameterMap typeParameterMap)
      : base(template, typeParameterMap)
    {
      AccessorKind = template.AccessorKind;
      DeclaredAccessibility = template.DeclaredAccessibility;

      if (template.Body != null)
      {
        Body = (BlockStatementEntity)template.Body.GetGenericClone(typeParameterMap);
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
      return new AccessorEntity(this, typeParameterMap);
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets or sets the body of the accessor.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public BlockStatementEntity Body
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
    public bool IsAccessibleBy(ISemanticEntity accessingEntity)
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
          return parentType.IsParentOf(accessingEntity);

        default:
          throw new ApplicationException("Effective accessibility is undefined.");
      }
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets the name of the entity.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public string Name 
    {
      get
      {
        return Parent is INamedEntity
                 ? GetAccessorKindName(AccessorKind) + "_" + (Parent as INamedEntity).Name
                 : null;
      }
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
        return Parent != null && Parent.Parent is TypeEntity
          ? string.Format("{0}.{1}", (Parent.Parent as TypeEntity).FullyQualifiedName, Name)
          : Name;
      }
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets the name of an accessor kind 
    /// </summary>
    /// <param name="accessorKind">An accessor kind.</param>
    /// <returns>The name of the accessor kind.</returns>
    // ----------------------------------------------------------------------------------------------
    private static string GetAccessorKindName(AccessorKind accessorKind)
    {
      string name = null;

      switch(accessorKind)
      {
        case AccessorKind.Get:
          name = "get";
          break;
        case AccessorKind.Set:
          name = "set";
          break;
        case AccessorKind.Add:
          name = "add";
          break;
        case AccessorKind.Remove:
          name = "remove";
          break;
      }

      return name;
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

      if (Body != null)
      {
        Body.AcceptVisitor(visitor);
      }
    }

    #endregion
  }
}
