﻿using System.Collections.Generic;
using System.Linq;

namespace CSharpTreeBuilder.CSharpSemanticGraph
{
  // ================================================================================================
  /// <summary>
  /// This abstract class represents a type that can declare child types.
  /// </summary>
  // ================================================================================================
  public abstract class ChildTypeCapableTypeEntity : GenericCapableTypeEntity, IHasChildTypes
  {
    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Initializes a new instance of the <see cref="ChildTypeCapableTypeEntity"/> class.
    /// </summary>
    /// <param name="name">The name of the entity.</param>
    // ----------------------------------------------------------------------------------------------
    protected ChildTypeCapableTypeEntity(string name)
      : base(name)
    {
      ChildTypes = new List<TypeEntity>();
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets the list of child types.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public List<TypeEntity> ChildTypes { get; private set;}
 
    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Adds a child type. 
    /// Also sets the child's parent property, and defines child's name in the declaration space.
    /// </summary>
    /// <param name="typeEntity">The child type entity.</param>
    // ----------------------------------------------------------------------------------------------
    public void AddChildType(TypeEntity typeEntity)
    {
      ChildTypes.Add(typeEntity);
      typeEntity.Parent = this;
      _DeclarationSpace.Register(typeEntity);
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Removes a child type. 
    /// </summary>
    /// <param name="typeEntity">The type entity.</param>
    // ----------------------------------------------------------------------------------------------
    public void RemoveChildType(TypeEntity typeEntity)
    {
      ChildTypes.Remove(typeEntity);
      typeEntity.Parent = null;
      _DeclarationSpace.Unregister(typeEntity);
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets a collection of child type entities by type and name.
    /// </summary>
    /// <param name="name">The name of the type.</param>
    /// <returns>A collection of child type entities, possibly empty.</returns>
    // ----------------------------------------------------------------------------------------------
    public IEnumerable<TEntityType> GetChildTypes<TEntityType>(string name)
      where TEntityType : TypeEntity
    {
      return GetChildTypes<TEntityType>(name, 0);
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
    /// Gets a child type entity by type and name.
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

    #region Visitor methods

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Accepts a visitor object, according to the Visitor pattern.
    /// </summary>
    /// <param name="visitor">A visitor object</param>
    // ----------------------------------------------------------------------------------------------
    public override void AcceptVisitor(SemanticGraphVisitor visitor)
    {
      base.AcceptVisitor(visitor);

      VisitMutableCollection(ChildTypes, visitor);
    }

    #endregion
  }
}
