﻿using System.Linq;
using System.Collections.Generic;

namespace CSharpTreeBuilder.CSharpSemanticGraph
{
  // ================================================================================================
  /// <summary>
  /// This abstract class represents a type that can declare child types.
  /// </summary>
  // ================================================================================================
  public abstract class ChildTypeCapableTypeEntity : GenericCapableTypeEntity, IHasChildTypes
  {
    // Child types are stored as members, so no special state here.

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Initializes a new instance of the <see cref="ChildTypeCapableTypeEntity"/> class.
    /// </summary>
    /// <param name="accessibility">The declared accessibility of the member. Can be null.</param>
    /// <param name="name">The name of the entity.</param>
    // ----------------------------------------------------------------------------------------------
    protected ChildTypeCapableTypeEntity(AccessibilityKind? accessibility, string name)
      : base(accessibility, name)
    {
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Initializes a new instance of the <see cref="ChildTypeCapableTypeEntity"/> class 
    /// by constructing it from a template instance.
    /// </summary>
    /// <param name="template">The template for the new instance.</param>
    /// <param name="typeParameterMap">The type parameter map of the new instance.</param>
    // ----------------------------------------------------------------------------------------------
    protected ChildTypeCapableTypeEntity(ChildTypeCapableTypeEntity template, TypeParameterMap typeParameterMap)
      : base(template, typeParameterMap)
    {
      // Child types are stored and cloned as members, so no special cloneing needed here.
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Adds a child entity.
    /// </summary>
    /// <param name="entity">A child entity.</param>
    // ----------------------------------------------------------------------------------------------
    public override void AddChild(ISemanticEntity entity)
    {
      if (entity is TypeEntity && !(entity is TypeParameterEntity))
      {
        AddChildType(entity as TypeEntity);
      }
      else
      {
        base.AddChild(entity);
      }
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets the list of child types.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public IEnumerable<TypeEntity> ChildTypes
    {
      get 
      {
        return from member in OwnMembers 
               where member is TypeEntity 
               select member as TypeEntity;
      }
    }
 
    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Adds a child type. 
    /// Also sets the child's parent property, and defines child's name in the declaration space.
    /// </summary>
    /// <param name="typeEntity">The child type entity.</param>
    // ----------------------------------------------------------------------------------------------
    public void AddChildType(TypeEntity typeEntity)
    {
      AddMember(typeEntity);
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Removes a child type. 
    /// </summary>
    /// <param name="typeEntity">The type entity.</param>
    // ----------------------------------------------------------------------------------------------
    public void RemoveChildType(TypeEntity typeEntity)
    {
      RemoveMember(typeEntity);
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets a collection of child type entities by type, name and number of type parameters.
    /// </summary>
    /// <param name="name">The name of the type.</param>
    /// <param name="typeParameterCount">The number of type parameters.</param>
    /// <returns>A collection of child type entities, possibly empty.</returns>
    // ----------------------------------------------------------------------------------------------
    public IEnumerable<TEntityType> GetChildTypes<TEntityType>(string name, int typeParameterCount)
      where TEntityType : TypeEntity
    {
      return _DeclarationSpace.GetEntities<TEntityType>(name, typeParameterCount);
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets a child type entity by type and name, with zero type arguments.
    /// </summary>
    /// <param name="name">The name of the type.</param>
    /// <returns>The found type, or null if not found.</returns>
    /// <remarks>Throws AmbiguousDeclarationsException if more than one type was found.</remarks>
    // ----------------------------------------------------------------------------------------------
    public TEntityType GetSingleChildType<TEntityType>(string name)
      where TEntityType : TypeEntity
    {
      return GetSingleChildType<TEntityType>(name, 0);
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets a child type entity by type, name and number of type parameters.
    /// </summary>
    /// <param name="name">The name of the type.</param>
    /// <param name="typeParameterCount">The number of type parameters.</param>
    /// <returns>The found type, or null if not found.</returns>
    /// <remarks>Throws AmbiguousDeclarationsException if more than one type was found.</remarks>
    // ----------------------------------------------------------------------------------------------
    public TEntityType GetSingleChildType<TEntityType>(string name, int typeParameterCount)
      where TEntityType : TypeEntity
    {
      return _DeclarationSpace.GetSingleEntity<TEntityType>(name, typeParameterCount);
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets an accessible child type entity by type, name and number of type parameters.
    /// </summary>
    /// <typeparam name="TEntityType">The type of the entity to be found.</typeparam>
    /// <param name="name">The name of the type.</param>
    /// <param name="typeParameterCount">The number of type parameters.</param>
    /// <param name="accessingEntity">An entity for accessibility checking.</param>
    /// <returns>The found type, or null if not found.</returns>
    /// <remarks>Throws AmbiguousDeclarationsException if more than one type was found.</remarks>
    // ----------------------------------------------------------------------------------------------
    public TEntityType GetAccessibleSingleChildType<TEntityType>(string name, int typeParameterCount, ISemanticEntity accessingEntity)
      where TEntityType : TypeEntity
    {
      var typeEntity = GetSingleChildType<TEntityType>(name, typeParameterCount);

      return typeEntity != null && typeEntity.IsAccessibleBy(accessingEntity) ? typeEntity : null;
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

      // Child types are stored and visited as members, so no special visiting is needed here.
    }

    #endregion
  }
}
